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
		public void StartGame()
		{
			return;
		}

		private void OnGameComplete()
		{
			BTDebug.Error("UnExcepted GameComplete Found", "CLIENT");
			return;
		}
	}
}
