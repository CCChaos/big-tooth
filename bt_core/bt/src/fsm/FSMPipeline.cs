using System;
using System.Collections.Generic;
using System.Text;

namespace BTFSM
{
	/// <summary>
	/// 状态机转换条件
	/// </summary>
	public abstract class CFSMPipeline
	{
		public abstract bool IsPipelineOpen(CFSM fsmEntity);
	}

	/// <summary>
	///  始终通畅的FSM管道
	/// </summary>
	public class CFSMOpenPipeline : CFSMPipeline
	{
		private static CFSMOpenPipeline s_OpenPipeline;

		// 空管道
		public static CFSMOpenPipeline OpenPipeline
		{
			get
			{
				if (s_OpenPipeline == null)
				{
					s_OpenPipeline = new CFSMOpenPipeline();
				}
				return s_OpenPipeline;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		private CFSMOpenPipeline()
		{

		}

		/// <summary>
		/// 是否畅通
		/// 只会返回true
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <returns></returns>
		public override bool IsPipelineOpen(CFSM fsmEntity)
		{
			return true;
		}
	}

	/// <summary>
	/// 事件类型转换条件
	/// 根据事件判断是否转换
	/// 打断状态转换
	/// </summary>
	public class CFSMEventPipelie : CFSMPipeline
	{
		private CFSMEventTrigger m_EventTrigger; // 时间触发器

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="trigger"></param>
		public CFSMEventPipelie(CFSMEventTrigger trigger)
		{
			m_EventTrigger = trigger;
		}

		/// <summary>
		/// 转换通道是否畅通
		/// 重载
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <returns></returns>
		public override bool IsPipelineOpen(CFSM fsmEntity)
		{
			if (m_EventTrigger == null)
			{
				return false;
			}
			List<CFSMEvent> listenedEventList = fsmEntity.GetListenedEvent();
			if (listenedEventList == null)
			{
				return false;
			}
			Int32 nSize = listenedEventList == null ? 0 : listenedEventList.Count;
			for (Int32 i = 0; i < nSize; ++i)
			{
				CFSMEvent listenedEvent = listenedEventList[i];
				bool bTrigger = m_EventTrigger.Trigger(listenedEvent);
				if (bTrigger == true)
				{
					return true;
				}
			}
			return false;
		}
	}

	/// <summary>
	/// 状态类型转换条件
	/// 根据FSM当前状态数据判断是否转换
	/// 非打断状态转换
	/// </summary>
	public class CFSMConditionPipeline : CFSMPipeline
	{
		private CFSMConditionTrigger m_ConditionTrigger; // 条件触发

		/// <summary>
		/// Constructor
		/// </summary>
		public CFSMConditionPipeline()
		{

		}

		/// <summary>
		/// 获取触发条件数据
		/// </summary>
		/// <returns></returns>
		public CFSMConditionTrigger GetTrigger()
		{
			return m_ConditionTrigger;
		}

		/// <summary>
		/// 设置条件触发数据
		/// </summary>
		/// <param name="trigger"></param>
		/// <returns></returns>
		public bool SetTrigger(CFSMConditionTrigger trigger)
		{
			if (trigger == null)
			{
				return false;
			}
			m_ConditionTrigger = trigger;
			return true;
		}

		/// <summary>
		/// 转换通道是否畅通
		/// 重载
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <returns></returns>
		public override bool IsPipelineOpen(CFSM fsmEntity)
		{
			if (m_ConditionTrigger == null)
			{
				return true;
			}
			bool bIsOpen = m_ConditionTrigger.IsFit(fsmEntity);
			return bIsOpen;
		}
	}

	/// <summary>
	/// FSM条件
	/// </summary>
	public class CFSMPipelineCollection
	{
		private List<CFSMPipeline>  m_ConditionPipelineList;

		public CFSMPipelineCollection()
		{
		}

		public bool AddPipel(CFSMPipeline newPipel)
		{
			if (newPipel == null)
			{
				return false;
			}
			if (m_ConditionPipelineList == null)
			{
				m_ConditionPipelineList = new List<CFSMPipeline>();
			}
			m_ConditionPipelineList.Add(newPipel);
			return true;
		}

		public bool IsOpenToFSM(CFSM fsmEntity)
		{
			if (m_ConditionPipelineList == null)
			{
				return true;
			}
			Int32 nSize = m_ConditionPipelineList.Count;
			for (Int32 i = 0; i < nSize; ++i)
			{
				CFSMPipeline pipeline = m_ConditionPipelineList[i];
				if (pipeline == null)
				{
					continue;
				}
				if (pipeline.IsPipelineOpen(fsmEntity) == false)
				{
					return false;
				}
			}
			return true;
		}

		public bool AddConfigPipel(List<CFSMXMLEventPipel> eventXMLList)
		{
			Int32 nEventPipelSize = eventXMLList == null ? 0 : eventXMLList.Count;
			for (Int32 i = 0; i < nEventPipelSize; ++i)
			{
				CFSMXMLEventPipel eventPipel = eventXMLList[i];
				if (eventPipel == null)
				{
					continue;
				}
				CFSMEventTrigger trigger = new CFSMEventTrigger();
				trigger.SetTriggerEvent(eventPipel.m_uEventId);
				CFSMEventPipelie newPipeline = new CFSMEventPipelie(trigger);
				AddPipel(newPipeline);
			}
			return true;
		}

		public bool AddConfigPipel(List<CFSMXMLConditionPipel> conXMLList)
		{
			Int32 nConditionPipelSize = conXMLList == null ? 0 : conXMLList.Count;
			for (Int32 i = 0; i < nConditionPipelSize; ++i)
			{
				CFSMXMLConditionPipel conditionPipelxml = conXMLList[i];
				if (conditionPipelxml == null)
				{
					continue;
				}

				CFSMConditionTrigger trigger = new CFSMConditionTrigger();
				Int32 nSizeConCmp = conditionPipelxml.m_conditionList == null ? 0 : conditionPipelxml.m_conditionList.Count;
				for (Int32 nCmpIndex = 0; nCmpIndex < nSizeConCmp; ++nCmpIndex)
				{
					CFSMXMLCondition conditionxml = conditionPipelxml.m_conditionList[nCmpIndex];
					if (conditionxml == null)
					{
						continue;
					}
					trigger.AddConditionCmp(conditionxml.m_strName, conditionxml.GetCmpType(), conditionxml.m_strValue);
				}
				CFSMConditionPipeline newPipeline = new CFSMConditionPipeline();
				newPipeline.SetTrigger(trigger);
				AddPipel(newPipeline);
			}
			return true;
		}
	}
}
