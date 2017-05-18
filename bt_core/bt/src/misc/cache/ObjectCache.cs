using System;
using System.Collections.Generic;

namespace BTMISC
{
	/// <summary>
	/// 资源缓存
	/// </summary>
	public class CObjectCache
	{
		/// <summary>
		/// 缓存仓库
		/// </summary>
		protected class CCacheSet
		{
			// 不可用Slot
			private const Int32 cnInvalidSlot = 0x7FFFFFFF;
			// 缓存数据
			private System.Object[] m_CacheBlock;
			// 下一个空闲Slot
			private Int32 m_nIndexNextFreeSlot;
			// 下一个缓存Slot
			private Int32 m_nIndexNextCacheSlot;
			// 缓存权重
			private Int32 m_nCacheSetWeight;
			// 缓存数量
			private Int32 m_nSize;

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="nCacheSize"></param>
			public CCacheSet(Int32 nCacheSize)
			{
				if (nCacheSize <= 0)
				{
					m_nIndexNextCacheSlot = cnInvalidSlot;
					m_nIndexNextFreeSlot = cnInvalidSlot;
					return;
				}
				m_CacheBlock = new System.Object[nCacheSize];
				m_nIndexNextCacheSlot = cnInvalidSlot;
				m_nIndexNextFreeSlot = 0;
				m_nSize = 0;
			}

			/// <summary>
			/// 添加缓存
			/// </summary>
			/// <param name="objCache"></param>
			/// <returns></returns>
			public bool PushCache(System.Object objCache)
			{
				if (objCache == null)
				{
					return false;
				}
				if (m_CacheBlock == null)
				{
					return false;
				}
				if (m_nIndexNextFreeSlot == cnInvalidSlot)
				{
					return false;
				}

				m_CacheBlock[m_nIndexNextFreeSlot] = objCache;
				m_nIndexNextCacheSlot = m_nIndexNextFreeSlot;
				m_nIndexNextFreeSlot = cnInvalidSlot;
				m_nSize += 1;

				Int32 nSize = m_CacheBlock.Length;
				for (Int32 i = 0; i < nSize; ++i )
				{
					if (m_CacheBlock[i] == null)
					{
						m_nIndexNextFreeSlot = i;
						break;
					}
				}
				return true;
			}

			/// <summary>
			/// 获取缓存
			/// </summary>
			/// <returns></returns>
			public System.Object PopCache()
			{
				if (m_CacheBlock == null)
				{
					return null;
				}
				if (m_nIndexNextCacheSlot == cnInvalidSlot)
				{
					return null;
				}

				System.Object cacheObj = m_CacheBlock[m_nIndexNextCacheSlot];
				m_CacheBlock[m_nIndexNextCacheSlot] = null;
				m_nIndexNextFreeSlot = m_nIndexNextCacheSlot;
				m_nIndexNextCacheSlot = cnInvalidSlot;
				m_nSize -= 1;

				Int32 nSize = m_CacheBlock.Length;
				for (Int32 i = 0; i < nSize; ++i )
				{
					if (m_CacheBlock[i] != null)
					{
						m_nIndexNextCacheSlot = i;
						break;
					}
				}
				return cacheObj;
			}

			/// <summary>
			/// 清空缓存数据
			/// </summary>
			public List<System.Object> PopAllCache()
			{
				List<System.Object> cacheList = new List<System.Object>();
				if (m_CacheBlock == null)
				{
					return cacheList;
				}
				Int32 nSize = m_CacheBlock.Length;
				for (Int32 i = 0; i < nSize; ++i )
				{
					if (m_CacheBlock[i] == null)
					{
						continue;
					}
					cacheList.Add(m_CacheBlock[i]);
					m_CacheBlock[i] = null;
				}
				m_nIndexNextCacheSlot = cnInvalidSlot;
				m_nIndexNextFreeSlot = 0;
				m_nSize = 0;

				return cacheList;
			}

			/// <summary>
			/// 设置缓存权重
			/// </summary>
			/// <param name="nWeight"></param>
			public void SetCacheWeight(Int32 nWeight)
			{
				m_nCacheSetWeight = nWeight;
			}

			/// <summary>
			/// 获取缓存权重
			/// </summary>
			/// <returns></returns>
			public Int32 CacheWeight()
			{
				return m_nCacheSetWeight;
			}

			/// <summary>
			/// 缓存仓库已经存储的缓存个数
			/// </summary>
			/// <returns></returns>
			public Int32 CacheSize()
			{
				return m_nSize;
			}

			/// <summary>
			/// 缓存仓库最大缓存数量
			/// </summary>
			/// <returns></returns>
			public Int32 SlotSize()
			{
				return m_CacheBlock == null ? 0 : m_CacheBlock.Length;
			}
		}

#if BTDEBUG
		public class CCacheHitRatio
		{
			// 命中次数
			public Int32 HitTime { get; set; }
			// 丢失次数
			public Int32 MissTime { get; set; }

			/// <summary>
			/// Constructor
			/// </summary>
			public CCacheHitRatio()
			{
				HitTime = 0;
				MissTime = 0;
			}

			/// <summary>
			/// 命中率
			/// </summary>
			/// <returns></returns>
			public float HitRation()
			{
				Int32 nTotalTime = HitTime + MissTime;
				if (nTotalTime == 0)
				{
					return 0;
				}
				float fRation = (float)HitTime / (float)nTotalTime;
				return fRation;
			}
		}
#endif

		// 缓存数据
		protected QuickList<string, CCacheSet> m_Cache;
		// 销毁缓存回调
		private HandleObjectAction m_HandleReleaseCache;
		// 每个缓存仓库缓存大小
		private Int32 m_nCacheSetSize;
#if BTDEBUG
		// 缓存击中统计
		protected QuickList<string, CCacheHitRatio> m_CacheHitRatio;
#endif
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="nCacheSetSize"></param>
		/// <param name="handleReleaseCache"></param>
		public CObjectCache(Int32 nCacheSetSize, HandleObjectAction handleReleaseCache = null)
		{
			if (nCacheSetSize <= 0)
			{
				BTDebug.Exception("<BT> Object Cache Exception");
				return;
			}
			m_nCacheSetSize = nCacheSetSize;
			m_Cache = new QuickList<string, CCacheSet>();
			m_HandleReleaseCache = handleReleaseCache;
		}

		/// <summary>
		/// 存储缓存到缓存仓库
		/// </summary>
		/// <param name="strCacheKey"></param>
		/// <param name="cacheObject"></param>
		/// <returns></returns>
		public bool CacheCache(string strCacheKey, System.Object cacheObject)
		{
			if (string.IsNullOrEmpty(strCacheKey) == true ||
				cacheObject == null)
			{
				return false;
			}
			if (m_Cache == null)
			{
				return false;
			}
			CCacheSet cacheSet = null;
			if (m_Cache.QuickFind(strCacheKey, ref cacheSet) == false || cacheSet == null)
			{
				cacheSet = new CCacheSet(m_nCacheSetSize);
				if (m_Cache.Add(strCacheKey, cacheSet) == false)
				{
					return false;
				}
			}

			if (cacheSet.PushCache(cacheObject) == false)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 试图命中缓存
		/// </summary>
		/// <param name="strCacheKey"></param>
		/// <param name="rOutCacheObject"></param>
		/// <returns></returns>
		public bool TryHitCache(string strCacheKey, ref System.Object rOutCacheObject)
		{
			rOutCacheObject = null;
			if (string.IsNullOrEmpty(strCacheKey) == true)
			{
				return false;
			}
#if BTDEBUG
			CCacheHitRatio cacheHit = null;
			if (m_CacheHitRatio.QuickFind(strCacheKey, ref cacheHit) == false || cacheHit == null)
			{
				cacheHit = new CCacheHitRatio();
				m_CacheHitRatio.Add(strCacheKey, cacheHit);
			}
#endif
			if (m_Cache == null)
			{
#if BTDEBUG
				cacheHit.MissTime += 1;
#endif
				return false;
			}
			CCacheSet cacheSet = null;
			if (m_Cache.QuickFind(strCacheKey, ref cacheSet) == false ||
				cacheSet == null)
			{
#if BTDEBUG
				cacheHit.MissTime += 1;
#endif
				return false;
			}

			rOutCacheObject = cacheSet.PopCache();
			if (rOutCacheObject == null)
			{
#if BTDEBUG
				cacheHit.MissTime += 1;
#endif
				return false;
			}
#if BTDEBUG
			cacheHit.HitTime += 1;
#endif
			return true;
		}

		/// <summary>
		/// 收缩
		/// </summary>
		/// <returns></returns>
		public void Shrink(Int32 nMaxRemainCount)
		{
			if (nMaxRemainCount < 0)
			{
				nMaxRemainCount = 0;
			}
			if (m_Cache == null)
			{
				return;
			}
			List<string> keyList = m_Cache.KeyList();
			if (keyList == null)
			{
				return;
			}
			Int32 nKeySize = keyList.Count;
			for (Int32 i = 0; i < nKeySize; ++i )
			{
				string strCacheKey = keyList[i];
				Shrink(strCacheKey, nMaxRemainCount);
			}
		}

		/// <summary>
		/// 收缩
		/// </summary>
		/// <param name="strCacheKey"></param>
		/// <param name="nMaxRemainCount"></param>
		public void Shrink(string strCacheKey, Int32 nMaxRemainCount)
		{
			if (nMaxRemainCount < 0)
			{
				nMaxRemainCount = 0;
			}
			if (m_Cache == null)
			{
				return;
			}
			CCacheSet cacheSet = null;
			if (m_Cache.QuickFind(strCacheKey, ref cacheSet) == false ||
				cacheSet == null)
			{
				return;
			}
			while (cacheSet.CacheSize() > nMaxRemainCount)
			{
				System.Object objCache = cacheSet.PopCache();
				if (objCache != null && m_HandleReleaseCache != null)
				{
					m_HandleReleaseCache(objCache);
				}
			}
		}

#if BTDEBUG
		public string DebugCacheStatus()
		{
			string strStatus = string.Empty;
			List<string> strKeyList = m_CacheHitRatio.KeyList();
			Int32 nSize = strKeyList == null ? 0 : strKeyList.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				string strKey = strKeyList[i];
				CCacheHitRatio cacheHit = null;
				CCacheSet cacheSet = null;
				if (m_CacheHitRatio.QuickFind(strKey, ref cacheHit) == false || cacheHit == null)
				{
					continue;
				}
				m_Cache.QuickFind(strKey, ref cacheSet);

				strStatus += string.Format("Cache:{0}, Try Times:{1}, Hit Times:{2}, Hit Ratio:{3:P2}, Cache Count:{4}, Cache Slot:{5}",
					strKey,
					cacheHit.MissTime + cacheHit.HitTime,
					cacheHit.HitTime,
					cacheHit.HitRation(),
					cacheSet == null ? 0 : cacheSet.CacheSize(),
					cacheSet == null ? 0 : cacheSet.SlotSize());
			}

			return strStatus;
		}
#endif
	}
}
