using System;
using System.Collections.Generic;
using System.Net;

namespace BTNET
{
	public enum TNetState
	{
		NS_Null = 0,								// NULL
		State_Initialized = 1,					// 初始化
		State_Connected = 2,				// 连接
		State_ConnectFailed = 4,			// 连接失败
		State_TryConnecting = 8,			// 正在连接
		State_DisconRecvErr1 = 16,		// 由于系统错误导致的断开
		State_DisconRecvErr2 = 32,		// 由于Socket错误导致的断开
		State_DisconSendErr1 = 64,		// 由于系统错误导致的断开
		State_DisconSendErr2 = 128,		// 由于Socket错误导致的断开
		State_ClientClose = 256,			// 客户端关闭连接
		State_Disconnected = 1024,		// 断开
		State_ServerClose = 2048,			// 服务器断开
		State_ConLoseEfficacy = 4096
	}

	public enum TNetAction
	{
		Action_NULL = 0,
		Action_Connect = 1,					// 连接
		Action_DisConnect = 2,				// 断开
		Action_Receive = 3,					// 接收消息
		Action_Send = 4,						// 发送消息
	}

	public interface INetConnect
	{
		bool ConnectHost(IPAddress connectPoint, Int32 nPort);
		Int32 Disconnect(TNetState state);
		Int32 Send(Byte[] byteArray, Int32 nPos, Int32 nLength);
		void OnUpdateConnect();
	}

	public class CNetStaticConfig
	{
		// Socket连接接收队列最大数量
		public const Int32 cnNetReceiveQueueSize = 128;
		// Socket消息Byte长度
		public const Int32 cnNetReceiveMsgByteSize = 65536;
	}
}
