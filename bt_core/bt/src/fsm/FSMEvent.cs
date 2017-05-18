using System;
using System.Collections;

namespace BTFSM
{
	/// <summary>
	/// 状态机事件
	/// </summary>
	public class CFSMEvent
	{
		private UInt32 m_uEventId;

		/// <summary>
		/// 设置事件ID
		/// </summary>
		/// <param name="uEventId"></param>
		public void SetEventId(UInt32 uEventId)
		{
			m_uEventId = uEventId;
		}

		/// <summary>
		/// 获取事件ID
		/// </summary>
		/// <returns></returns>
		public UInt32 GetEventId()
		{
			return m_uEventId;
		}
	}

	/// <summary>
	/// 状态机事件触发器
	/// </summary>
	public class CFSMEventTrigger
	{
		// 触发的事件ID
		private UInt32 m_uTriggerEventId;

		/// <summary>
		/// 设置触发的事件ID
		/// </summary>
		/// <param name="uEventId"></param>
		public void SetTriggerEvent(UInt32 uEventId)
		{
			m_uTriggerEventId = uEventId;
		}
		/// <summary>
		/// 触发状态机事件
		/// </summary>
		/// <param name="fsmEvent"></param>
		/// <returns></returns>
		public bool Trigger(CFSMEvent fsmEvent)
		{
			if (fsmEvent == null)
			{
				return false;
			}
			if (fsmEvent.GetEventId() != m_uTriggerEventId)
			{
				return false;
			}
			return true;
		}

	}
}