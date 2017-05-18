using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BTMISC;
using BTIO;

namespace BTNET
{
	/// <summary>
	///  封装Socket连接
	///  接收和发送以Byte[] 为处理单位
	/// </summary>
	public class CSocketConnect : INetConnect
	{
#region Members
		//  Socket 实例
		protected Socket m_Socket;
		// 当前连接状态
		protected TNetState m_State;
		// 接收数据
		protected SafeQueue m_QueMsgReceive;
		// 接收数据缓存
		protected Byte[] m_ReceiveByteBuff;
		// 读取数据在缓存上的位置
		protected Int32 m_nReceiveBuffPos;
		// 读取数据的长度
		protected Int32 m_nReceiveBuffReadLength;
		// 接收进程
		private Thread m_ThreadReceive = null;
		// 接收到消息的回调
		private HandleByteArrayAction m_HandleOnReceiveMsg;
		// 连接的回调
		private HandleIntAction m_HandleOnConnected;
		// 断开连接的回调
		private HandleIntAction m_HandleOnDisConnected;
#endregion
		
#region Methods
		/// <summary>
		/// Construct
		/// </summary>
		public CSocketConnect()
		{
			m_Socket = null;
			m_State = TNetState.NS_Null;
			m_QueMsgReceive = new SafeQueue(CNetStaticConfig.cnNetReceiveQueueSize);
			m_ReceiveByteBuff = new Byte[CNetStaticConfig.cnNetReceiveMsgByteSize];
			m_nReceiveBuffPos = 0;
			m_nReceiveBuffReadLength = CNetStaticConfig.cnNetReceiveMsgByteSize;
			InitReceiveThread();
		}

		~CSocketConnect()
		{
			if (m_State == TNetState.State_Connected)
			{
				Disconnect(TNetState.State_ClientClose);
			}
			if (m_ThreadReceive != null)
			{
				m_ThreadReceive.Abort();
			}
			m_ThreadReceive = null;
			m_HandleOnConnected = null;
			m_HandleOnDisConnected = null;
			m_HandleOnReceiveMsg = null;
		}

		/// <summary>
		/// 连接一个远程主机
		/// </summary>
		/// <param name="connectPoint"></param>
		/// <returns></returns>
		public bool ConnectHost(IPAddress connectPoint, Int32 nPort)
		{
			try
			{
				if (m_State == TNetState.State_TryConnecting)
				{
					// connect waiting
					return false;
				}
				if (m_State == TNetState.State_Initialized)
				{
					// initialized
					return false;
				}
				if (m_State == TNetState.State_Connected)
				{
					// connected, do not connect again
					return false;
				}

				m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				if (m_Socket == null)
				{
					m_State = TNetState.NS_Null;
					return false;
				}
				m_State = TNetState.State_Initialized;

				m_Socket.BeginConnect(connectPoint, nPort, new AsyncCallback(ConnectCallback), m_Socket);
				m_State = TNetState.State_TryConnecting;

				return true;
			}
			catch (Exception)
			{
				Disconnect(TNetState.State_ConnectFailed);
				return false;
			}
		}

		/// <summary>
		/// 断开连接
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Int32 Disconnect(TNetState state)
		{
			BTDebug.Log("Disconnect For:" + state.ToString());
			m_State = state;
			if (m_Socket == null)
			{
				return 0;
			}
			try
			{
				if (m_Socket.Connected == true)
				{
					m_Socket.Shutdown(SocketShutdown.Both);
					if (m_HandleOnDisConnected != null)
					{
						m_HandleOnDisConnected(0);
					}
				}
			}
			catch (System.Exception ex)
			{
				BTDebug.Exception("ShutDown Socket Error:" + ex.Message);
				if (m_HandleOnDisConnected != null)
				{
					m_HandleOnDisConnected(-1);
				}
			}
			m_Socket.Close();
			m_Socket = null;
			return 0;
		}

		// 发送数据
		public Int32 Send(Byte[] byteArray, Int32 nPos = 0, Int32 nLength = CBTStream.EOS)
		{
			if (m_State != TNetState.State_Connected || m_Socket == null)
			{
				return -1;
			}

			if (byteArray == null)
			{
				return -1;
			}
			Int32 nArrayLength = byteArray.Length;
			if (nLength == CBTStream.EOS)
			{
				nLength = nArrayLength - nPos;
			}
			if (nPos < 0 || nPos >= nArrayLength || nLength <= 0 || nLength > nArrayLength - nPos)
			{
				return -1;
			}

			Int32 nSendStart = nPos;
			Int32 nSendLength = 0;
			try
			{
				do
				{
					nSendLength = m_Socket.Send(byteArray, nSendStart, nLength, SocketFlags.None);
					nSendStart += nSendLength;
				} while (nSendStart < nPos + nLength);
			}
			catch (System.Exception ex)
			{
				BTDebug.Exception("Scoket Send:" + ex.Message);
				return -1;
			}

			return 0;
		}

		// 更新
		public void OnUpdateConnect()
		{
			while (m_QueMsgReceive.IsEmpty() == false)
			{
				System.Object refObj = null;
				if (m_QueMsgReceive.Pop(ref refObj) == false)
				{
					break;
				}
				if (refObj == null)
				{
					continue;
				}
				Byte[] receByteArray = refObj as Byte[];
				if (receByteArray == null)
				{
					BTDebug.Warning("<BTNET>Receive Data Not Packed as Byte Array");
					continue;
				}
				if (m_HandleOnReceiveMsg != null)
				{
					m_HandleOnReceiveMsg(receByteArray);
				}
			}
		}

		/// <summary>
		/// CallBack After Socket Connectd
		/// </summary>
		/// <param name="ar"></param>
		private void ConnectCallback(IAsyncResult ar)
		{
			try
			{
				ar.AsyncWaitHandle.Close();
				if (m_Socket != null)
				{
					m_Socket.EndConnect(ar);
					m_Socket.Blocking = false; // 非阻断
					m_Socket.ReceiveTimeout = 3000;
					m_Socket.SendTimeout = 3000;
				}
				m_State = TNetState.State_Connected;
				if (m_HandleOnConnected != null)
				{
					m_HandleOnConnected(0);
				}
			}
			catch (Exception ex)
			{
				if (m_HandleOnConnected != null)
				{
					m_HandleOnConnected(-1);
				}
				BTDebug.Exception("Socket Connect CallBack Exception:" + ex.Message);
				Disconnect(TNetState.State_ConnectFailed);
			}
		}

		/// <summary>
		/// Init And Start Receive Thread
		/// </summary>
		/// <returns></returns>
		private bool InitReceiveThread()
		{
			if (m_ThreadReceive != null && m_ThreadReceive.IsAlive)
			{
				m_ThreadReceive.Abort();
			}

			m_ThreadReceive = new Thread(new ThreadStart(ThreadReceiveMessage));
			if (m_ThreadReceive == null)
			{
				return false;
			}
			m_ThreadReceive.Name = "BTSocketReceive";
			m_ThreadReceive.Start();
			return true;
		}

		public TNetState GetState()
		{
			return m_State;
		}
#region Receive Thread
		// 接收消息线程入口
		private void ThreadReceiveMessage()
		{
			while (true)
			{
				ReveiceMessage();
				Thread.Sleep(33);
			}
		}

		// 接收消息工作函数
		private void ReveiceMessage()
		{
			if (m_State != TNetState.State_Connected || m_Socket == null)
			{
				return;
			}
			// 读取
			Int32 nReadByteSize = 0;
			try
			{
				if (m_Socket.Poll(0, SelectMode.SelectRead) == false)
				{
					return;
				}
				nReadByteSize = m_Socket.Receive(
					m_ReceiveByteBuff,					// 缓存
					m_nReceiveBuffPos,					// 开始放置数据的缓存位置
					m_nReceiveBuffReadLength,		// 读取数据的长度
					SocketFlags.None);					// SocketFlag
			}
			catch (SocketException )
			{
				Disconnect(TNetState.State_DisconRecvErr2);
			}
			catch (System.Exception )
			{
				Disconnect(TNetState.State_DisconRecvErr1);
			}

			if (nReadByteSize == 0) // 服务器关闭连接
			{
				Disconnect(TNetState.State_ServerClose);
				return;
			}

			if (nReadByteSize < 0)
			{
				Disconnect(TNetState.State_DisconRecvErr1);
				return;
			}
			
			m_nReceiveBuffPos += nReadByteSize; 
			OnReceiveMessage(nReadByteSize);
		}

		protected virtual void OnReceiveMessage(Int32 nReadByteSize)
		{
			Byte[] msgByteArray = new Byte[nReadByteSize];
			for (Int32 i = 0; i < nReadByteSize; ++i)
			{
				msgByteArray[i] = m_ReceiveByteBuff[i];
			}

			if (m_QueMsgReceive.Push(msgByteArray) == false)
			{
				BTDebug.Error("Receive Msg Push Queue Error, Message Lose!");
			}

			m_nReceiveBuffPos = 0;
		}
#endregion
#region 回调注册
		/// <summary>
		/// Register Receive Message Handle
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public bool RegisterOnReceiveMsgHandle(HandleByteArrayAction handle)
		{
			if (handle == null)
			{
				return false;
			}
			m_HandleOnReceiveMsg += handle;
			return true;
		}

		/// <summary>
		/// UnRegister Receive Message Handle
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public bool UnRegisterOnReceiveMsgHandle(HandleByteArrayAction handle)
		{
			if (handle == null)
			{
				return false;
			}
			m_HandleOnReceiveMsg -= handle;
			return true;
		}

		/// <summary>
		/// Register Receive Message Handle
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public bool RegisterOnConnectHandle(HandleIntAction handle)
		{
			if (handle == null)
			{
				return false;
			}
			m_HandleOnConnected += handle;
			return true;
		}

		/// <summary>
		/// UnRegister Receive Message Handle
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public bool UnRegisterOnConnectHandle(HandleIntAction handle)
		{
			if (handle == null)
			{
				return false;
			}
			m_HandleOnConnected -= handle;
			return true;
		}

		/// <summary>
		/// Register Receive Message Handle
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public bool RegisterOnDisConnectHandle(HandleIntAction handle)
		{
			if (handle == null)
			{
				return false;
			}
			m_HandleOnDisConnected += handle;
			return true;
		}

		/// <summary>
		/// UnRegister Receive Message Handle
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public bool UnRegisterOnDisConnectHandle(HandleIntAction handle)
		{
			if (handle == null)
			{
				return false;
			}
			m_HandleOnDisConnected -= handle;
			return true;
		}
#endregion

#endregion
		

	}
}