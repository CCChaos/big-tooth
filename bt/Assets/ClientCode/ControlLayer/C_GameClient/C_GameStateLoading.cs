using System;
using NS_GAMESHARE;
using BTMISC;
using BTFSM;
namespace NS_GAME
{
	public class CGameStateLoading : CFSMState
	{
		public override bool OnEnter(CFSM fsmEntity, CFSMEvent fsmEvent)
		{
			if (fsmEntity == null)
			{
				return false;
			}
			CGameClient gameClient = fsmEntity.GetFSMBindEntity() as CGameClient;
			if (gameClient == null)
			{
				return false;
			}
			gameClient.StartLoading();
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
			return (UInt32)TGameStateType.enLoading;
		}

		public override string GetStateName()
		{
			return string.Format("State_{0}", (TGameStateType)GetStateID());
		}

		protected override void InitState()
		{

		}
	}
}