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
#region Members
		private CFSM m_GameFSM;
#endregion
#region Methods
		public bool Start()
		{
			BTDebug.Log("Game Client Start", "CLIENT");
			if (m_GameFSM == null)
			{
				return false;
			}
#if UNITY_EDITOR
			m_GameFSM.SetOpenFSMDebug(true);
#endif
			UInt32 uStateIDUpdate = (UInt32)TGameStateType.enInstall;
			bool bFSMStartRet = m_GameFSM.Start(uStateIDUpdate);
			return bFSMStartRet;
		}

		public TGameStateType GetCurrentState()
		{
			if (m_GameFSM == null)
			{
				return TGameStateType.enNone;
			}
			UInt32 uCurrentStateId = m_GameFSM.GetCurrentStateID();
			if (uCurrentStateId == CFSMState.cuInvalieStateId)
			{
				return TGameStateType.enNone;
			}
			return (TGameStateType)uCurrentStateId;
		}

		private void InitFSM()
		{
			m_GameFSM = new CFSM();
			m_GameFSM.BindEntity(this);
			m_GameFSM.AddState(new CGameStateInstall());
			m_GameFSM.AddState(new CGameStateUpdate());
			m_GameFSM.AddState(new CGameStateLoading());
			m_GameFSM.AddState(new CGameStateInit());
			m_GameFSM.AddState(new CGameStateLogin());
			m_GameFSM.AddState(new CGameStateGame());
			m_GameFSM.AddState(new CGameStateDisconnect());
		}

		private void UpdateFSM()
		{
			if (m_GameFSM == null)
			{
				return;
			}
			m_GameFSM.UpdateFSM();
		}

		private void ReleaseFSM()
		{
			if (m_GameFSM != null)
			{
				m_GameFSM.UnBind();
			}
		}
#endregion
	}
}

