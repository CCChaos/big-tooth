using System;
using NS_GAMESHARE;
using BTMISC;
using BTFSM;
using NS_GAME.KIT;
using NS_GAME.DATA;

namespace NS_GAME
{
	public partial class CGameClient :
		CBTGameSingleton<CGameClient>, IFSMEntity
	{
		/// <summary>
		/// 初始化游戏设置
		/// </summary>
		public void StartInitGame()
		{
#if UNITY_EDITOR
			if (RunningType == TSysRunningType.enEditor)
			{

				OnInitComplete();
				return;
			}
#endif
			// todo Init Game
			
		}

		private void OnInitComplete()
		{
			m_GameFSM.ChangeToState((UInt32)TGameStateType.enLogin, null);
		}
	}
}
