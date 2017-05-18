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
		public void StartLogin()
		{

		}

		private void OnLoginComplete()
		{
			m_GameFSM.ChangeToState((UInt32)TGameStateType.enLogin, null);
		}
	}
}
