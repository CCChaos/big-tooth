
#if UNITY_EDITOR
#define BTDEBUG
#endif

using System;
using System.Collections.Generic;
using BTMISC;

namespace NS_GAME.KIT
{
	/// <summary>
	/// 事件系统
	/// </summary>
	public class CActionDispatcher : CBTSingleton<CActionDispatcher>, IGameSingleton
	{
		/// <summary>
		/// 事件回调信息
		/// </summary>
		private class CHandleInfo
		{
			// 回调
			private HandleObjectAction m_Handle;
			// 参数类型
			private Type m_ArgType;
#if BTDEBUG
			// 调用次数
			private CCpuProfiler m_cpuProfiler;
#endif

			/// <summary>
			/// Constructor
			/// </summary>
			public CHandleInfo()
			{
				m_Handle = null;
				m_ArgType = null;
#if BTDEBUG
				m_cpuProfiler = new CCpuProfiler();
#endif
			}

			/// <summary>
			/// 设置回调参数类型
			/// </summary>
			/// <param name="type"></param>
			public void SetArgType(Type type)
			{
				m_ArgType = type;
			}

			/// <summary>
			/// 设置名称
			/// </summary>
			/// <param name="strName"></param>
			public void SetName(string strName)
			{
#if BTDEBUG
				m_cpuProfiler.SetName(strName);
#endif
			}

#if BTDEBUG
			/// <summary>
			/// 获取CPU消耗数据
			/// </summary>
			/// <returns></returns>
			public CCpuProfiler GetCpuProfiler()
			{
				return m_cpuProfiler;
			}
#endif

			/// <summary>
			/// 获取回调参数类型
			/// </summary>
			/// <returns></returns>
			public Type GetArgType()
			{
				return m_ArgType;
			}

			/// <summary>
			/// 触发回调
			/// </summary>
			/// <param name="objParam"></param>
			/// <returns></returns>
			public bool TriggerHandle(System.Object objParam)
			{
				if (CheckEventParamType(objParam) == false)
				{
					BTDebug.Warning("Trigger Event With An Param Not Fit", "ACTION");
					return false;
				}

#if BTDEBUG
				m_cpuProfiler.StartNewCall();
#endif
				if (m_Handle == null)
				{
					return true;
				}
				Delegate[] delArray = m_Handle.GetInvocationList();
				Int32 nSize = delArray == null ? 0 : delArray.Length;
				for (Int32 i = 0; i < nSize; ++i )
				{
					try
					{
						HandleObjectAction handle = (HandleObjectAction)delArray[i];
						if (handle == null)
						{
							continue;
						}
						handle(objParam);
					}
					catch (System.Exception ex)
					{
						BTDebug.ExceptionEx(ex);
					}
				}
#if BTDEBUG
				m_cpuProfiler.EndCall();
#endif
				return true;
			}

			/// <summary>
			/// 移除回调
			/// </summary>
			/// <param name="handle"></param>
			/// <returns></returns>
			public bool RemoveHandle(HandleObjectAction handle)
			{
				if (handle == null)
				{
					return false;
				}
				if (m_Handle == null)
				{
					return false;
				}
				Delegate[] delArray = m_Handle.GetInvocationList();
				if (delArray == null || delArray.Length == 0)
				{
					return false;
				}
				Int32 nSize = delArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					if (delArray[i] == (Delegate)handle)
					{
						m_Handle -= handle;
						return true;
					}
				}
				return false;
			}

			/// <summary>
			/// 添加回调
			/// </summary>
			/// <param name="handle"></param>
			/// <returns></returns>
			public bool AddHandle(HandleObjectAction handle)
			{
				if (handle == null)
				{
					return false;
				}
				if (m_Handle == null)
				{
					m_Handle += handle;
					return true;
				}
				Delegate[] delArray = m_Handle.GetInvocationList();
				if (delArray == null || delArray.Length == 0)
				{
					m_Handle += handle;
					return true;
				}
				Int32 nSize = delArray.Length;
				for (Int32 i = 0; i < nSize; ++i )
				{
					if (delArray[i] == (Delegate)handle)
					{
						return true;
					}
				}
				m_Handle += handle;
				return true;
			}

			// 检查参数是否合适回调
			private bool CheckEventParamType(System.Object objParam)
			{
				if (objParam == null)
				{
					return true;
				}
				if (objParam.GetType().Equals(m_ArgType) == true)
				{
					return true;
				}
				return false;
			}
		}

		/// <summary>
		/// 监听到的事件信息
		/// </summary>
		private class CEventInfo : IRecycleable
		{
			// ID
			private UInt32 m_uEventId;
			// 参数
			private System.Object m_objParam;

			/// <summary>
			/// Constructor
			/// </summary>
			public CEventInfo()
			{

			}

			/// <summary>
			///  初始化
			/// </summary>
			public void Init()
			{
				m_uEventId = 0;
				m_objParam = null;
			}

			/// <summary>
			/// 清理
			/// </summary>
			public void Clear()
			{
				m_uEventId = 0;
				m_objParam = null;
			}

			/// <summary>
			/// 设置信息
			/// </summary>
			/// <param name="uEventId"></param>
			/// <param name="objParam"></param>
			public void Set(UInt32 uEventId, System.Object objParam)
			{
				m_uEventId = uEventId;
				m_objParam = objParam;
			}

			/// <summary>
			/// 获取事件ID
			/// </summary>
			/// <returns></returns>
			public UInt32 GetEventId()
			{
				return m_uEventId;
			}

			/// <summary>
			/// 获取事件参数
			/// </summary>
			/// <returns></returns>
			public System.Object GetEventParam()
			{
				return m_objParam;
			}
		}
		// 事件以及回调列表
		private QuickList<UInt32, CHandleInfo> m_EventActionList;
		// 事件信息对象池
		private CObjectPool m_EventInfoPool;
		// 监听到的事件列表
		private SafeQueue m_ListenedEvent;

		/// <summary>
		/// Constructor
		/// </summary>
		public CActionDispatcher()
		{
			m_EventActionList = new QuickList<UInt32, CHandleInfo>();
			m_EventInfoPool = new CObjectPool();
			m_ListenedEvent = new SafeQueue(32);
		}

		/// <summary>
		/// 初始化资源
		/// </summary>
		public void Init()
		{
			m_EventInfoPool.InitPool(typeof(CEventInfo), 16, 128, null);
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		public void Release()
		{
			m_EventInfoPool.ClearPool();
			m_EventActionList.Clear();
		}

		/// <summary>
		/// 注册事件回调信息
		/// </summary>
		/// <param name="uEventId"></param>
		/// <param name="tArgsType"></param>
		/// <returns></returns>
		public bool RegisterAction(UInt32 uEventId, Type tArgsType, string strName = "")
		{
			CHandleInfo handInfo = null;
			if (m_EventActionList.QuickFind(uEventId, ref handInfo) == true)
			{
				return false;
			}
			handInfo = new CHandleInfo();
			handInfo.SetArgType(tArgsType);
			if (string.IsNullOrEmpty(strName) == true)
			{
				strName = "Event_" + uEventId.ToString();
			}
			handInfo.SetName(strName);
			if (m_EventActionList.Add(uEventId, handInfo) == false)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 为事件增加一个回调
		/// </summary>
		/// <param name="uEventId"></param>
		/// <param name="handle"></param>
		/// <returns></returns>
		public bool AddHandle(UInt32 uEventId, HandleObjectAction handle)
		{
			CHandleInfo handleInfo = null;
			if (m_EventActionList.QuickFind(uEventId, ref handleInfo) == false || handleInfo == null)
			{
				return false;
			}
			if (handleInfo.AddHandle(handle) == false)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 为事件移除一个回调
		/// </summary>
		/// <param name="uEventId"></param>
		/// <param name="handle"></param>
		/// <returns></returns>
		public bool RemoveHandle(UInt32 uEventId, HandleObjectAction handle)
		{
			CHandleInfo handleInfo = null;
			if (m_EventActionList.QuickFind(uEventId, ref handleInfo) == false || handleInfo == null)
			{
				return false;
			}
			if (handleInfo.RemoveHandle(handle) == false)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 触发事件
		/// </summary>
		/// <param name="uEventId"></param>
		/// <param name="objParam"></param>
		/// <returns></returns>
		public bool TriggerAction(UInt32 uEventId, System.Object objParam)
		{
			CEventInfo eventInfo = m_EventInfoPool.RentObject() as CEventInfo;
			if (eventInfo == null)
			{
				BTDebug.Warning(string.Format("Action {0} Trigger Failed, Event Info NULL", uEventId), "ACTION");
				return false;
			}
			eventInfo.Set(uEventId, objParam);
			if (m_ListenedEvent.Push(eventInfo) == false)
			{
				BTDebug.Warning(string.Format("Action {0} Trigger Failed, Event Info Push Failed", uEventId), "ACTION");
				return false;
			}
			return true;
		}

		/// <summary>
		/// 帧更新
		/// </summary>
		public void Update()
		{
			while (m_ListenedEvent.IsEmpty() == false)
			{
				System.Object objInfo = null;
				if (m_ListenedEvent.Pop(ref objInfo) == false || objInfo == null)
				{
					continue;
				}
				CEventInfo info = objInfo as CEventInfo;
				if (info == null)
				{
					continue;
				}
				UInt32 uEventId = info.GetEventId();
				System.Object objParam = info.GetEventParam();
				
				m_EventInfoPool.GiveBackObject(info);

				CHandleInfo handleInfo = null;
				if (m_EventActionList.QuickFind(uEventId, ref handleInfo) == false || handleInfo == null)
				{
					continue;
				}
				bool bTriggerRet = handleInfo.TriggerHandle(objParam);
				if (bTriggerRet == false)
				{
					BTDebug.Warning(string.Format("Trigger Event:{0} With Param:{2} Failed", uEventId, objParam), "ACTION");
				}
			}

		}

	}
}
