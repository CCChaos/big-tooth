using System;
using System.Collections.Generic;
using System.Text;
using BTMISC;

namespace BTFSM
{
	/// <summary>
	/// 可执行脚本的状态机状态
	/// </summary>
	public class CFSMExecutableState : CFSMState
	{
		// ID
		private UInt32 m_uStateID;
		// 名称
		private string m_strStateName;
		// 进入动作
		private List<CFSMAction> m_EnterActionList;
		// 更新动作
		private List<CFSMAction> m_ProcessActionList;
		// 退出动作
		private List<CFSMAction> m_ExitActionList;


		/// <summary>
		/// 获取状态名称
		/// </summary>
		/// <returns></returns>
		public override string GetStateName()
		{
			return m_strStateName;
		}

		/// <summary>
		/// 从xml脚本数据创建
		/// </summary>
		/// <param name="xmlState"></param>
		/// <returns></returns>
		public bool CreateFromXML(CFSMXMLState xmlState)
		{
			if (xmlState == null)
			{
				return false;
			}
			m_strStateName = xmlState.m_strName;
			m_uStateID = xmlState.m_uStateId;
			List<CFSMTransition> transList = CreateTransitionListFromXML(xmlState.m_transArray);
			if (transList != null && m_TransitionList != null)
			{
				m_TransitionList.AddRange(transList);
			}
			m_EnterActionList = CreateActionListFromXML(xmlState.m_Enter);
			m_ExitActionList = CreateActionListFromXML(xmlState.m_Exit);
			m_ProcessActionList = CreateActionListFromXML(xmlState.m_Process);


			return true;
		}

		// 创建转换列表
		private List<CFSMTransition> CreateTransitionListFromXML(List<CFSMXMLTranslation> transitionXMLList)
		{
			if (transitionXMLList == null)
			{
				return null;
			}
			List<CFSMTransition> tranList = new List<CFSMTransition>();
			Int32 nTransSize = transitionXMLList == null ? 0 : transitionXMLList.Count;
			for (Int32 i = 0; i < nTransSize; ++i)
			{
				CFSMXMLTranslation xmlTrans = transitionXMLList[i];
				if (xmlTrans == null)
				{
					continue;
				}
				CFSMTransition newTrans = new CFSMTransition(xmlTrans.m_uTargetId);
				if (newTrans.CreateFromXML(xmlTrans) == false)
				{
					BTMISC.BTDebug.Warning("<BTFSM> Create FSM Transition Failed");
					continue;
				}
				tranList.Add(newTrans);
			}
			return tranList;
		}

		// 从xml数据创建动作列表
		private List<CFSMAction> CreateActionListFromXML(CFSMXMLActionArray actionxmlList)
		{
			if (actionxmlList == null)
			{
				return null;
			}
			List<CFSMAction> actionList = new List<CFSMAction>();
			Int32 nSize = actionxmlList.m_ActionList == null ? 0 : actionxmlList.m_ActionList.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				CFSMXMLAction actionxml = actionxmlList.m_ActionList[i];
				if (actionxml == null)
				{
					continue;
				}
				CFSMAction newAction = new CFSMAction();
				if (newAction.CreateFromXML(actionxml) == false)
				{
					BTDebug.Warning("<BTFSM> Create FSM Action Failed");
					continue;
				}
				actionList.Add(newAction);
			}
			return actionList;
		}

		/// <summary>
		/// 初始化状态机状态
		/// </summary>
		protected override void InitState()
		{

		}
		/// <summary>
		/// 状态ID
		/// </summary>
		/// <returns></returns>
		public override UInt32 GetStateID()
		{
			return m_uStateID;
		}
		/// <summary>
		/// 进入状态
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <param name="fsmEvent"></param>
		public override bool OnEnter(CFSM fsmEntity, CFSMEvent fsmEvent)
		{
			if (m_EnterActionList == null)
			{
				return true;
			}
			if (fsmEntity == null)
			{
				return false;
			}
			Int32 nActionSize = m_EnterActionList.Count;
			bool bInvokeRet = true;
			for (Int32 i = 0; i < nActionSize; ++i )
			{
				CFSMAction action = m_EnterActionList[i];
				if (action == null)
				{
					continue;
				}
				System.Object objReturnValue = null;
				bInvokeRet &= action.InvokeWhenPipelineOpen(fsmEntity, out objReturnValue);
			}
			return bInvokeRet;
		}
		/// <summary>
		/// 转出状态
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <param name="fsmEvent"></param>
		public override bool OnExit(CFSM fsmEntity, CFSMEvent fsmEvent)
		{
			if (m_ExitActionList == null)
			{
				return true;
			}
			if (fsmEntity == null)
			{
				return false;
			}
			Int32 nActionSize = m_ExitActionList.Count;
			bool bInvokeRet = true;
			for (Int32 i = 0; i < nActionSize; ++i)
			{
				CFSMAction action = m_ExitActionList[i];
				if (action == null)
				{
					continue;
				}
				System.Object objReturnValue = null;
				bInvokeRet &= action.InvokeWhenPipelineOpen(fsmEntity, out objReturnValue);
			}
			return bInvokeRet;
		}
		/// <summary>
		/// 状态更新
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <param name="fsmEvent"></param>
		public override bool Process(CFSM fsmEntity, CFSMEvent fsmEvent)
		{
			if (m_ProcessActionList == null)
			{
				return true;
			}
			if (fsmEntity == null)
			{
				return false;
			}
			Int32 nActionSize = m_ProcessActionList.Count;
			bool bInvokeRet = true;
			for (Int32 i = 0; i < nActionSize; ++i)
			{
				CFSMAction action = m_ProcessActionList[i];
				if (action == null)
				{
					continue;
				}
				System.Object objReturnValue = null;
				bInvokeRet &= action.InvokeWhenPipelineOpen(fsmEntity, out objReturnValue);
			}
			return bInvokeRet;
		}

	}
}
