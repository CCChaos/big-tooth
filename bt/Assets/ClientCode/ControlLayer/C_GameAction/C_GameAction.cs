using System;
using BTMISC;
using NS_GAME.KIT;

namespace NS_GAMESHARE
{
	public class CGameAction
	{
		private CGameAction() { }


#region Action ID
		/* 事件ID分段  
		 * 1-1000					系统事件
		 * 10000-19999			
		*/
		public const UInt32 cuAction_SYS_Error = 0;	// 系统错误
		public const UInt32 cuAction_SYS_Message = 1; // 系统消息

		public const UInt32 cuAction_SYS_ConnectStart = 101;				// 开始连接游戏服务器
		public const UInt32 cuAction_SYS_ConnectEnd = 102;				// 连接游戏服务器返回
		public const UInt32 cuAction_SYS_Disconnect = 103;				// 从服务器断开连接

		/*
		 * 10000-19999			客户端状态事件
		 */
		public const UInt32 cuAction_STATE_InstallStart = 10101;					// 开始安装
		public const UInt32 cuAction_STATE_InstallComplete = 10102;			// 安装完成
		public const UInt32 cuAction_STATE_StartUpdate = 10103;				// 开始更新
		public const UInt32 cuAction_STATE_UpdateComplete = 10104;		// 客户端更新完成
		public const UInt32 cuAction_STATE_StartLoadData = 10105;			// 开始加载初始数据
		public const UInt32 cuAction_STATE_LoadDataComplete = 10106;	// 加载数据完成
		public const UInt32 cuAction_STATE_StartInitGame = 10107;			// 初始化游戏
		public const UInt32 cuAction_STATE_InitGameComplete = 10108;		// 初始化游戏完成
		public const UInt32 cuAction_STATE_StartLogin = 10108;					// 开始登录
		public const UInt32 cuAction_STATE_LoginComplete = 10109;			// 登录完成
		public const UInt32 cuAction_STATE_StartGame = 10110;					// 进入游戏
		public const UInt32 cuAction_STATE_ExitGame = 10111;					// 退出游戏状态
		public const UInt32 cuAction_STATE_Disconnected = 10112;				// 断线
		public const UInt32 cuAction_STATE_ExitDisconnected = 10113;		// 离开断线状态


#endregion

		/// <summary>
		/// 注册事件属性
		/// </summary>
		/// <returns></returns>
		public static bool InitGameAction()
		{
			CActionDispatcher.Instance.RegisterAction(cuAction_STATE_UpdateComplete, null);
			CActionDispatcher.Instance.RegisterAction(cuAction_STATE_LoginComplete, null);
			CActionDispatcher.Instance.RegisterAction(cuAction_SYS_ConnectStart, null);
			CActionDispatcher.Instance.RegisterAction(cuAction_SYS_ConnectEnd, typeof(Int32));
			CActionDispatcher.Instance.RegisterAction(cuAction_SYS_Disconnect, typeof(Int32));

			return true;
		}
	}
}

