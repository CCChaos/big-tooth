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
		/// 尝试重连
		/// </summary>
		public void TryReconnect()
		{

		}
	}
}
