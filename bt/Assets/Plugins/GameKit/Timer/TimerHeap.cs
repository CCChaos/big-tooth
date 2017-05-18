using System;
using System.Collections.Generic;
using BTMISC;

namespace NS_GAME.KIT
{
	/// <summary>
	/// 计时器管理器
	/// </summary>
	public class CTimerHeap : CBTSingleton<CTimerHeap>, IGameSingleton
	{
		/// <summary>
		/// 计时器
		/// </summary>
		private class CTimer : IRecycleable
		{
			protected UInt32 m_uTimerId;
			private float m_fStartTime;
			private float m_fLifeTimeRemain;
			private float m_fLastUpdateTime;

			private float m_fKeepDecimal;
			private float m_fIntervalTime;
			
			private TUnityTimeType m_TimeType;

			private HandleObjectAction m_handleTriggerAction;
			private HandleObjectAction m_handleIntervalAction;
			private System.Object m_objTriggerParam;
			private System.Object m_objIntervalParam;

			/// <summary>
			/// Constructor
			/// </summary>
			public CTimer()
			{
				Init();
			}

			/// <summary>
			/// 初始化
			/// </summary>
			public void Init()
			{
				m_uTimerId = 0;
				m_fStartTime = 0;
				m_fLifeTimeRemain = 0;
				m_fLastUpdateTime = 0;

				m_fKeepDecimal = 0;
				m_fIntervalTime = 0;
				
				m_handleTriggerAction = null;
				m_handleIntervalAction = null;
				m_objTriggerParam = null;
				m_objIntervalParam = null;
				
				m_TimeType = TUnityTimeType.enGameTime;
			}

			/// <summary>
			/// 清理
			/// </summary>
			public void Clear()
			{
				m_uTimerId = 0;
				m_fStartTime = 0;
				m_fLifeTimeRemain = 0;
				m_fKeepDecimal = 0;
				m_fIntervalTime = 0;
				m_handleTriggerAction = null;
				m_handleIntervalAction = null;
				m_objTriggerParam = null;
				m_objIntervalParam = null;
				m_fLastUpdateTime = 0;
				m_TimeType = TUnityTimeType.enGameTime;
			}

			/// <summary>
			/// 获取ID 
			/// </summary>
			/// <returns></returns>
			public UInt32 GetID()
			{
				return m_uTimerId;
			}

			/// <summary>
			/// 注册
			/// </summary>
			/// <param name="fTrigger"></param>
			/// <param name="handleAction"></param>
			/// <param name="objTrigger"></param>
			public void Register(UInt32 uId, float fCountDownTime, HandleObjectAction handleOnTimeUp, System.Object objTimeUpParam, TUnityTimeType tTimeType, float fStartDelay)
			{
				float fTimeNow = CTimeAlgorithm.GetGameTimeNow(tTimeType);
				m_uTimerId = uId;
				m_fLifeTimeRemain = fCountDownTime;
				m_handleTriggerAction = handleOnTimeUp;
				m_objTriggerParam = objTimeUpParam;
				m_fStartTime = fTimeNow + fStartDelay;
			}

			/// <summary>
			/// 注册
			/// </summary>
			/// <param name="fTrigger">触发事件 秒</param>
			/// <param name="handleAction"></param>
			/// <param name="objTriggerParam"></param>
			/// <param name="fInverval"></param>
			/// <param name="handleInterval"></param>
			/// <param name="objInvervalParam"></param>
			public void Register(UInt32 uId, float fCountDownTime, HandleObjectAction handleOnTimeUp, float fInterval, HandleObjectAction handleOnIntervalTrigger, System.Object objTimeUpParam, System.Object objInvtervalTriggerParam, TUnityTimeType tTimeType, float fStartDelay)
			{
				float fTimeNow = CTimeAlgorithm.GetGameTimeNow(tTimeType);
				m_uTimerId = uId;
				m_fLifeTimeRemain = fCountDownTime;
				m_handleTriggerAction = handleOnTimeUp;
				m_objTriggerParam = objTimeUpParam;
				m_fStartTime = fTimeNow + fStartDelay;
				m_fIntervalTime = fInterval;
				m_handleIntervalAction = handleOnIntervalTrigger;
				m_objIntervalParam = objInvtervalTriggerParam;
			}

			/// <summary>
			/// 更新
			/// </summary>
			public void TickUpdate()
			{
				float fTimeNow = CTimeAlgorithm.GetGameTimeNow(m_TimeType);
				if (fTimeNow < m_fStartTime || IsTimeUp())
				{
					return;
				}
				float fDeltaTime = fTimeNow - m_fLastUpdateTime;
				m_fLastUpdateTime = fTimeNow;
				m_fLifeTimeRemain -= fDeltaTime;

				if (IsTimeUp() == true)
				{
					OnTimeUp();
					return;
				}

				UpdateInterval(fDeltaTime);
			}

			/// <summary>
			/// 已经完成
			/// </summary>
			/// <returns></returns>
			public bool IsTimeUp()
			{
				return m_fLifeTimeRemain <= 0;
			}

			// 间隔计时更新
			private void UpdateInterval(float fDeltaTime)
			{
				if (m_fIntervalTime <= 0 || m_handleIntervalAction == null)
				{
					return;
				}
				m_fKeepDecimal += fDeltaTime;
				if (m_fKeepDecimal < m_fIntervalTime)
				{
					return;
				}
				m_fKeepDecimal -= m_fIntervalTime;
				OnTimeInterval();
			}

			// 计时结束触发
			private void OnTimeUp()
			{
				if (m_handleTriggerAction == null)
				{
					return;	
				}
				m_handleTriggerAction(m_objTriggerParam);
			}

			// 间隔计时触发
			private void OnTimeInterval()
			{
				if (m_handleIntervalAction == null)
				{
					return;
				}
				m_handleIntervalAction(m_objIntervalParam);
			}
		}

		// TimerID
		private UInt32 m_uNextTimerID;
		private List<CTimer> m_TimerList;
		private CObjectPool m_TimerPool;

		/// <summary>
		/// Constructor
		/// </summary>
		public CTimerHeap()
		{
			m_uNextTimerID = 1;
			m_TimerList = new List<CTimer>();
			m_TimerPool = new CObjectPool();
		}

		/// <summary>
		/// 帧更新
		/// </summary>
		public void Update()
		{
			Int32 nSize = m_TimerList.Count;
			for (Int32 i = nSize -  1; i >= 0; --i )
			{
				CTimer timer = m_TimerList[i];
				if (timer == null)
				{
					m_TimerList.RemoveAt(i);
					continue;
				}
				timer.TickUpdate();
				
				// 如果已经完成，释放
				if (timer.IsTimeUp())
				{
					m_TimerList.RemoveAt(i);
				}
				m_TimerPool.GiveBackObject(timer);
			}
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public void Init()
		{
			m_TimerPool.InitPool(typeof(CTimer), 16, 512, null);
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Release()
		{
			Int32 nSize = m_TimerList.Count;
			for (Int32 i = nSize - 1; i >= 0; --i)
			{
				CTimer timer = m_TimerList[i];
				if (timer == null)
				{
					m_TimerList.RemoveAt(i);
					continue;
				}
				m_TimerList.RemoveAt(i);
				m_TimerPool.GiveBackObject(timer);
			}
		}

		/// <summary>
		/// 注册倒计时
		/// </summary>
		/// <param name="fCountDownTime"></param>
		/// <param name="handleOnTimeUp"></param>
		/// <param name="objTimeUpParam"></param>
		/// <param name="tTimeType"></param>
		/// <param name="fStartDelay"></param>
		/// <returns></returns>
		public UInt32 RegisterCountDownTimer(float fCountDownTime, HandleObjectAction handleOnTimeUp, System.Object objTimeUpParam = null, TUnityTimeType tTimeType = TUnityTimeType.enGameTime, float fStartDelay = 0)
		{
			CTimer timer = m_TimerPool.RentObject() as CTimer;
			if (timer == null)
			{
				return 0;
			}
			UInt32 uId = m_uNextTimerID;
			m_uNextTimerID += 1;
			timer.Register(uId, fCountDownTime, handleOnTimeUp, objTimeUpParam, tTimeType, fStartDelay);
			m_TimerList.Add(timer);
			return uId;
		}

		/// <summary>
		/// 注册倒计时，带间隔时间触发
		/// </summary>
		/// <param name="fCountDownTime"></param>
		/// <param name="handleOnTimeUp"></param>
		/// <param name="fInterval"></param>
		/// <param name="handleOnIntervalTrigger"></param>
		/// <param name="objTimeUpParam"></param>
		/// <param name="objInvtervalTriggerParam"></param>
		/// <param name="tTimeType"></param>
		/// <param name="fStartDelay"></param>
		/// <returns></returns>
		public UInt32 RegisterCountDownTimer(float fCountDownTime, HandleObjectAction handleOnTimeUp, float fInterval, HandleObjectAction handleOnIntervalTrigger, System.Object objTimeUpParam = null, System.Object objInvtervalTriggerParam = null, TUnityTimeType tTimeType = TUnityTimeType.enGameTime, float fStartDelay = 0)
		{
			CTimer timer = m_TimerPool.RentObject() as CTimer;
			if (timer == null)
			{
				return 0;
			}
			UInt32 uId = m_uNextTimerID;
			m_uNextTimerID += 1;
			timer.Register(uId, fCountDownTime, handleOnTimeUp, fInterval,  handleOnIntervalTrigger, objTimeUpParam, objInvtervalTriggerParam, tTimeType, fStartDelay);
			m_TimerList.Add(timer);
			return uId;
		}

		/// <summary>
		/// 取消计时
		/// </summary>
		/// <param name="uTimerId"></param>
		/// <returns></returns>
		public bool CancelTimer(UInt32 uTimerId)
		{
			if (uTimerId == 0)
			{
				return false;
			}
			Int32 nSize = m_TimerList.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				CTimer timer = m_TimerList[i];
				if (timer == null)
				{
					continue;
				}
				if (timer.GetID() == uTimerId)
				{
					m_TimerList.RemoveAt(i);
					m_TimerPool.GiveBackObject(timer);
					return true;
				}
			}
			return false;
		}
	}
}
