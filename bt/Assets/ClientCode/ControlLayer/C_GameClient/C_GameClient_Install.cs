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
		public void StartInstallGame()
		{
#if UNITY_EDITOR
			if (RunningType == TSysRunningType.enEditor)
			{
				OnInstallComplete();
				return;
			}
#endif
			// todo Install Game

		}

		private void OnInstallComplete()
		{
			m_GameFSM.ChangeToState((UInt32)TGameStateType.enUpdate, null);
		}
	}
}
