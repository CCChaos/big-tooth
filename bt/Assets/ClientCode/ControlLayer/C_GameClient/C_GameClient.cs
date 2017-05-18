using System;
using NS_GAMESHARE;
using BTMISC;
using BTFSM;
using NS_GAME.KIT;

namespace NS_GAME
{
	public partial class CGameClient :
		CBTGameSingleton<CGameClient>, IFSMEntity
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public CGameClient()
		{

		}

		// 系统运行类型
		public TSysRunningType RunningType
		{
			get
			{
				return RunningTimeSettings.SystemRunningType;
			}
		}

		// 初始化注册事件
		private void InitAction()
		{
			CActionDispatcher.Instance.AddHandle(CGameAction.cuAction_SYS_ConnectEnd, OnClientConnected);
			CActionDispatcher.Instance.AddHandle(CGameAction.cuAction_SYS_Disconnect, OnClientDisconnect);
		}

		// 释放注册事件
		private void ReleaseAction()
		{
			CActionDispatcher.Instance.RemoveHandle(CGameAction.cuAction_SYS_ConnectEnd, OnClientConnected);
			CActionDispatcher.Instance.RemoveHandle(CGameAction.cuAction_SYS_Disconnect, OnClientDisconnect);
		}

		#region Client Action
		private void OnClientDisconnect(System.Object objParam)
		{
			if (m_GameFSM == null)
			{
				return;
			}
			m_GameFSM.ChangeToState((UInt32)TGameStateType.enDisconnect, null);
		}

		private void OnClientConnected(System.Object objParam)
		{
			if (m_GameFSM == null)
			{
				return;
			}
			m_GameFSM.OnFSMEvent(CGameAction.cuAction_SYS_ConnectEnd);
		}

		#endregion
		#region Singleton Override
		public override void Init()
		{
			InitFSM();
			InitAction();
		}

		public override void OnUpdate()
		{
			UpdateFSM();
		}

		public override void Release()
		{
			ReleaseFSM();
			ReleaseAction();
		}
		#endregion
	}
}