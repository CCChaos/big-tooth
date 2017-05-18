#if UNITY_EDITOR
#define BTDEBUG
#endif

using System;
using System.Collections.Generic;
using System.Reflection;
using BTMISC;

namespace BTFSM
{
	/// <summary>
	/// FSM实体接口
	/// </summary>
	public interface IFSMEntity
	{
		
	}

	/// <summary>
	/// 状态机
	/// </summary>
	public class CFSM
	{
		#region Members
		// 当前状态
		protected CFSMState m_CurrentState;
		// 公共状态
		protected CFSMState m_AnyState;
		// 进入状态
		protected UInt32 m_uEnteranceStateId;
		// 公共状态ID
		protected UInt32 m_uAnyStateId;
		// 状态列表
		protected List<CFSMState> m_StateList;
		// 注册的实体数据解释器
		protected CMemberReflector m_Reflector;
		// 监听到的事件
		private List<CFSMEvent> m_ListenedEventList;
		// 绑定实体
		private IFSMEntity m_BindEntity;
		// 暂停
		private bool m_bPause;
#if BTDEBUG
		// debug 开关
		private bool m_bOpenDebug = false;
#endif
		#endregion
		#region Methods
		/// <summary>
		/// Constructor
		/// </summary>
		public CFSM()
		{
			m_uEnteranceStateId = CFSMState.cuInvalieStateId;
			m_uAnyStateId = CFSMState.cuInvalieStateId;
			m_Reflector = new CMemberReflector();
			m_StateList = new List<CFSMState>();
			m_ListenedEventList = new List<CFSMEvent>();
			m_bPause = false;
		}

		/// <summary>
		/// 从配置创建状态机
		/// </summary>
		/// <param name="fsmXMLEntity"></param>
		/// <returns></returns>
		public bool CreateFromFSMXML(CFSMXMLEntity fsmXMLEntity)
		{
			if (fsmXMLEntity == null)
			{
				return false;
			}
			m_uEnteranceStateId = fsmXMLEntity.m_uEntranceStateId;
			m_uAnyStateId = fsmXMLEntity.m_uAnyStateId;
			Int32 nStateSize = fsmXMLEntity.m_FSMStateList == null ? 0 : fsmXMLEntity.m_FSMStateList.Count;
			for (Int32 i = 0; i < nStateSize; ++i )
			{
				CFSMXMLState xmlState = fsmXMLEntity.m_FSMStateList[i];
				if (xmlState == null)
				{
					continue;
				}
				CFSMExecutableState fsmState = new CFSMExecutableState();
				if (fsmState.CreateFromXML(xmlState) == false)
				{
					BTDebug.Warning("Create FSM State Failed", "FSM");
					continue;
				}
				AddState(fsmState);
			}
			return true;
		}

		/// <summary>
		/// 绑定实体到状态机
		/// </summary>
		/// <returns></returns>
		public bool BindEntity(IFSMEntity fsmEntity)
		{
			if (fsmEntity == null)
			{
				return false;
			}
			m_BindEntity = fsmEntity;
			bool bRet = true;
			bRet = m_Reflector.Init(m_BindEntity.GetType());
			return bRet;
		}

		/// <summary>
		/// 同实体解绑
		/// </summary>
		/// <returns></returns>
		public bool UnBind()
		{
			m_BindEntity = null;
			m_Reflector.Clear();
			return true;
		}

		/// <summary>
		/// 获取绑定实体
		/// </summary>
		/// <returns></returns>
		public IFSMEntity GetFSMBindEntity()
		{
			return m_BindEntity;
		}

		/// <summary>
		/// 设置FSM进入状态,状态机开始运行
		/// </summary>
		/// <param name="uEntranceStateId"></param>
		public bool Start(UInt32 uEntranceStateId)
		{
			if (m_CurrentState != null)
			{
				return false;
			}
			m_uEnteranceStateId = uEntranceStateId;
			m_AnyState = GetState(m_uAnyStateId);
			bool bRet = ChangeToState(m_uEnteranceStateId, null);
			return bRet;
		}

		/// <summary>
		/// 设置FSM进入状态,状态机开始运行
		/// </summary>
		public virtual bool Start()
		{
			if (m_CurrentState != null)
			{
				return false;
			}
			m_AnyState = GetState(m_uAnyStateId);
			bool bRet = ChangeToState(m_uEnteranceStateId, null);
			return bRet;
		}

		/// <summary>
		/// 暂停
		/// </summary>
		public void Pause()
		{
			m_bPause = true;
		}

		/// <summary>
		/// 继续
		/// </summary>
		public void Run()
		{
			m_bPause = false;
		}

		/// <summary>
		/// 获取状态机数据
		/// </summary>
		/// <param name="strConditionName"></param>
		/// <param name="rOutRet"></param>
		/// <returns></returns>
		public bool GetFSMCondition(string strConditionName, out System.Object rOutRet)
		{
			rOutRet = default(System.Object);
			if (m_Reflector == null)
			{
				return false;
			}
			bool bRet = m_Reflector.GetValue(strConditionName, m_BindEntity, out rOutRet);
			return bRet;
		}

		/// <summary>
		/// 执行FSM动作
		/// </summary>
		/// <param name="strActionName"></param>
		/// <param name="objParamArray"></param>
		/// <param name="rOutResultValue"></param>
		/// <returns></returns>
		public bool InvokeFSMAction(string strActionName, System.Object[] objParamArray, out System.Object rOutResultValue)
		{
			rOutResultValue = default(System.Object);
			if (m_Reflector == null)
			{
				return false;
			}
			bool bRet = m_Reflector.InvokeMethod(strActionName, m_BindEntity, objParamArray, out rOutResultValue);
#if BTDEBUG
			BTDebug.Log(string.Format("FSM Invoke Action:{0} {1}", strActionName, bRet ? "Success" : "Failed", "FSM"));
#endif
			return bRet;
		}

		/// <summary>
		/// 获取当前状态ID
		/// </summary>
		/// <returns></returns>
		public UInt32 GetCurrentStateID()
		{
			if (m_CurrentState == null)
			{
				return CFSMState.cuInvalieStateId;
			}
			UInt32 uID = m_CurrentState.GetStateID();
			return uID;
		}

		/// <summary>
		/// 状态机更新函数
		/// </summary>
		public virtual void UpdateFSM()
		{
			if (m_bPause == true)
			{
				return;
			}
			UpdateCurrentState();
			UpdateAnyState();
			if (m_ListenedEventList != null)
			{
				m_ListenedEventList.Clear();
			}
		}

		/// <summary>
		/// 添加一个状态
		/// </summary>
		/// <param name="newState"></param>
		/// <returns></returns>
		public bool AddState(CFSMState newState)
		{
			if (newState == null)
			{
				return false;
			}
			if (GetState(newState.GetStateID()) != null) // dup
			{
				return false;
			}
			if (m_StateList == null)
			{
				m_StateList = new List<CFSMState>();
			}
			m_StateList.Add(newState);
			return true;
		}

		/// <summary>
		/// 切换到一个状态
		/// </summary>
		/// <param name="uNextState"></param>
		/// <param name="fsmEvent"></param>
		/// <returns></returns>
		public bool ChangeToState(UInt32 uNextState, CFSMEvent fsmEvent)
		{
			CFSMState nextFSMState = GetState(uNextState);
			if (nextFSMState == null)
			{
				BTDebug.Warning(string.Format("Found NO State With ID:{0}, Change State Failed", uNextState), "FSM");
				return false;
			}

			if (m_CurrentState != null)
			{
				if (m_CurrentState.GetStateID() == uNextState)
				{
					return true;
				}
			
				if (m_CurrentState.OnExit(this, fsmEvent) == false)
				{
					BTDebug.Warning(string.Format("State:{0} Exit Failed", m_CurrentState.GetStateName()), "FSM");
				}
			}

#if BTDEBUG
			if (m_bOpenDebug)
			{
				string strLog =string.Format("FSM Trans From:{0} To:{1}",
					m_CurrentState == null ? "NULL" : m_CurrentState.GetStateName(),
					nextFSMState.GetStateName());

				BTDebug.Log(strLog, "BTFSM");
			}
#endif

			m_CurrentState = nextFSMState;

			if (nextFSMState.OnEnter(this, fsmEvent) == false)
			{
				BTDebug.Warning(string.Format("State:{0} Enter Failed", nextFSMState.GetStateName()), "FSM");
			}

			return true;
		}

		/// <summary>
		/// FSM事件
		/// </summary>
		/// <param name="uEventID"></param>
		public void OnFSMEvent(UInt32 uEventID)
		{
			CFSMEvent newFSMEvent = new CFSMEvent();
			newFSMEvent.SetEventId(uEventID);
			OnFSMEvent(newFSMEvent);
		}

		/// <summary>
		/// FSM事件
		/// </summary>
		/// <param name="fsmEvent"></param>
		public void OnFSMEvent(CFSMEvent fsmEvent)
		{
#if BTDEBUG
			if ( m_bOpenDebug == true && fsmEvent != null)
			{
				BTDebug.Log(string.Format("FSM Receive Event:{0}", fsmEvent.GetEventId()), "BTFSM");
			}
#endif
			if (m_ListenedEventList == null)
			{
				return;
			}
			m_ListenedEventList.Add(fsmEvent);
		}

		/// <summary>
		/// 获取监听到的事件
		/// </summary>
		/// <returns></returns>
		public List<CFSMEvent> GetListenedEvent()
		{
			return m_ListenedEventList;
		}

		/// <summary>
		/// 设置FSM调试开关
		/// 仅对BTDEBUG下的BTCore有效
		/// </summary>
		/// <param name="bOpen"></param>
		public void SetOpenFSMDebug(bool bOpen)
		{
#if BTDEBUG
			m_bOpenDebug = bOpen;
#else
			BTDebug.Warning("BTCore On Release Mode Not Support FSM Debug", "FSM");
#endif
		}

		/// <summary>
		/// 获取一个状态
		/// </summary>
		/// <param name="uStateId"></param>
		/// <returns></returns>
		protected CFSMState GetState(UInt32 uStateId)
		{
			Int32 nSize = m_StateList == null ? 0 : m_StateList.Count;
			for (Int32 i = 0; i < nSize; ++i)
			{
				CFSMState state = m_StateList[i];
				if (state != null && state.GetStateID() == uStateId)
				{
					return state;
				}
			}
			return null;
		}


		// 更新当前状态
		protected virtual void UpdateCurrentState()
		{
			if (m_CurrentState == null)
			{
				return;
			}
			// 获取下一个状态, 如果没有则循环执行当前状态
			UInt32 uNextStateId = m_CurrentState.GetFSMTransState(this);
			if (uNextStateId == CFSMState.cuInvalieStateId)
			{
				if (m_CurrentState.Process(this, null) == false)
				{
					BTDebug.Warning(string.Format("State:{0} Process Failed", m_CurrentState.GetStateName()), "FSM");
				}
				return;
			}
			// 切换到下一状态
			bool bChangeRet = ChangeToState(uNextStateId, null);
			if (bChangeRet == false)
			{
				BTDebug.Warning(string.Format("Trans From:{0} To:{1} Failed", m_CurrentState.GetStateID(), uNextStateId), "FSM");
			}
		}

		// 更新公共状态
		protected virtual void UpdateAnyState()
		{
			if (m_AnyState == null)
			{
				return;
			}
			// 获取下一个状态, 如果没有则循环执行当前状态
			UInt32 uNextStateId = m_AnyState.GetFSMTransState(this);
			if (uNextStateId != CFSMState.cuInvalieStateId)
			{
				ChangeToState(uNextStateId, null);
				return;
			}
			if (m_AnyState.Process(this, null) == false)
			{
				BTDebug.Warning(string.Format("State:{0} Process Failed", m_CurrentState.GetStateName()), "FSM");
			}
			return;
		}
		#endregion
	}
}