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
		/// 游戏更新
		/// </summary>
		public void StartUpdate()
		{
#if UNITY_EDITOR
			if (RunningType == TSysRunningType.enEditor)
			{
				OnUpdateComplete();
				return;
			}
#endif
			// todo check
		}

		private void OnUpdateComplete()
		{
			m_GameFSM.ChangeToState((UInt32)TGameStateType.enLoading, null);
		}
	}
}
