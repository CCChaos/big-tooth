using System;
using NS_GAMESHARE;
using BTMISC;
using BTFSM;

namespace NS_GAME
{
	public class CGameStateGame : CFSMState
	{
		public override bool OnEnter(CFSM fsmEntity, CFSMEvent fsmEvent)
		{
			return true;
		}

		public override bool OnExit(CFSM fsmEntity, CFSMEvent fsmEvent)
		{
			return true;
		}

		public override bool Process(CFSM fsmEntity, CFSMEvent fsmEvent)
		{
			return true;
		}

		public override UInt32 GetStateID()
		{
			return (UInt32)TGameStateType.enGame;
		}

		protected override void InitState()
		{

		}

	}
}