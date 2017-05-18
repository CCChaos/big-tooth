using System;
using System.Collections.Generic;

namespace BTFSM
{
	/// <summary>
	/// 状态机状态
	/// </summary>
	public abstract class CFSMState
	{
		// 空状态ID
		public const UInt32 cuInvalieStateId = 0xFFFFFFFF;

		// 状态内转换关系数据
		protected List<CFSMTransition> m_TransitionList;

		/// <summary>
		/// Constructor
		/// </summary>
		public CFSMState()
		{
			m_TransitionList = new List<CFSMTransition>();
			InitState();
		}

		/// <summary>
		/// 添加一条转换
		/// </summary>
		/// <param name="fsmTrans"></param>
		/// <returns></returns>
		public bool AddTransition(CFSMTransition fsmTrans)
		{
			if (fsmTrans == null || m_TransitionList == null)
			{
				return false;
			}
			m_TransitionList.Add(fsmTrans);
			return true;
		}

		/// <summary>
		/// 获取状态名称
		/// </summary>
		/// <returns></returns>
		public virtual string GetStateName()
		{
			string strName = string.Format("Stete_{0}", GetStateID());
			return strName;
		}

		/// <summary>
		/// 获取状态机该状态下的下一个转换状态
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <returns></returns>
		public virtual UInt32 GetFSMTransState(CFSM fsmEntity)
		{
			UInt32 uNextState = cuInvalieStateId;
			if (m_TransitionList == null)
			{
				return uNextState;
			}
			Int32 nSize = m_TransitionList.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				CFSMTransition fsmTrans = m_TransitionList[i];
				if (fsmTrans == null)
				{
					continue; 
				}
				if (fsmTrans.CheckTransition(fsmEntity) == true)
				{
					uNextState = fsmTrans.GetTargetStateId();
					break;
				}
			}
			return uNextState;
		}

		/// <summary>
		/// 初始化状态机状态
		/// </summary>
		protected abstract void InitState();
		/// <summary>
		/// 状态ID
		/// </summary>
		/// <returns></returns>
		public abstract UInt32 GetStateID();
		/// <summary>
		/// 进入状态
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <param name="fsmEvent"></param>
		public abstract bool OnEnter(CFSM fsmEntity, CFSMEvent fsmEvent);
		/// <summary>
		/// 转出状态
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <param name="fsmEvent"></param>
		public abstract bool OnExit(CFSM fsmEntity, CFSMEvent fsmEvent);
		/// <summary>
		/// 状态更新
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <param name="fsmEvent"></param>
		public abstract bool Process(CFSM fsmEntity, CFSMEvent fsmEvent);
		
	}
}