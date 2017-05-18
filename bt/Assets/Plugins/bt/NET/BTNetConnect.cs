using System;
using System.Collections.Generic;
using System.Net;
using BTMISC;

namespace BTNET
{
	/// <summary>
	/// BT网络连接
	/// 接收和发送以CBTNetStream为处理单位
	/// </summary>
	public class CBTNetConnect : CSocketConnect
	{
		/// <summary>
		/// 是否还在读取头信息
		/// </summary>
		private bool m_bIsReadingHead;
		/// <summary>
		/// Contruct
		/// </summary>
		public CBTNetConnect()
		{
			m_nReceiveBuffReadLength = CBTByteNetStream.cnBTMsgHeadSize;
			m_bIsReadingHead = true;
		}

		/// <summary>
		/// 发送BT流
		/// </summary>
		/// <param name="sendStream"></param>
		/// <returns></returns>
		public bool SendBTStream(CBTBitNetStream sendStream)
		{
			if (sendStream == null)
			{
				return false;
			}
			sendStream.FixMsgHead();
			Byte[] byteArray = sendStream.GetByteArray();
			Int32 nLength = sendStream.GetLength();
			if (Send(byteArray, 0, nLength) != 0)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 发送BT流
		/// </summary>
		/// <param name="sendStream"></param>
		/// <returns></returns>
		public bool SendBTStream(CBTByteNetStream sendStream)
		{
			if (sendStream == null)
			{
				return false;
			}
			sendStream.FixMsgHead();
			Byte[] byteArray = sendStream.GetByteArray();
			Int32 nLength = sendStream.GetLength();
			if (Send(byteArray, 0, nLength) != 0)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		///  收到消息
		/// </summary>
		/// <param name="nReadByteSize"></param>
		protected override void OnReceiveMessage(int nReadByteSize)
		{
			m_nReceiveBuffReadLength -= nReadByteSize;
			if (m_nReceiveBuffReadLength != 0)
			{
				return;
			}
			OnReadBuffComplete();
		}

		/// <summary>
		/// 在缓冲区接收到一个完整消息
		/// </summary>
		private void OnReadBuffComplete()
		{
			if (m_bIsReadingHead == true)
			{
				Int32 nSize = BitConverter.ToInt32(m_ReceiveByteBuff, 0);
				nSize = IPAddress.NetworkToHostOrder(nSize);

				m_nReceiveBuffReadLength = nSize;
				m_bIsReadingHead = false;
			}
			else
			{
				bool bPackReceivedMsg = false;
				if (m_nReceiveBuffPos > 0)
				{
					Int32 nSize = m_nReceiveBuffPos + 1;
					Byte[] byteDataArray = new Byte[nSize];
					for (Int32 i = 0; i < nSize; ++i)
					{
						byteDataArray[i] = m_ReceiveByteBuff[i];
					}
					bPackReceivedMsg = m_QueMsgReceive.Push(byteDataArray);
				}
				if (bPackReceivedMsg == false)
				{
					BTDebug.Error("Push BTMessage Failed, Message Package Lose", "NET");
				}

				m_nReceiveBuffReadLength = CBTByteNetStream.cnBTMsgHeadSize;
				m_bIsReadingHead = true;
			}
			m_nReceiveBuffPos = 0;
		}


	}
}
