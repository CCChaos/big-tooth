using System;
using System.Collections.Generic;
using System.Collections;

namespace BTMISC
{
	/// <summary>
	/// 对象池
	/// </summary>
	public class CObjectPool
	{
		// 最大长度
		private Int32 m_nCapacity;
		// 默认构造函数参数
		private System.Object[] m_objDefaultCreateParam;
		// 对象类型
		private Type m_ObjectType;
		// 对象支持IRecycleable
		private bool m_bSupportRecycle;

		// 对象缓存
		private Hashtable m_ItemTable;
		// 闲置对象Key
		private ArrayList m_FreeIndexArray;
		// 使用对象Key
		private ArrayList m_UsingIndexArray;

#if BTDEBUG
		// 对象使用频率
		private Hashtable m_ItemUseFrequencyTable;
#endif

		/// <summary>
		/// Consturctor
		/// </summary>
		public CObjectPool()
		{
			m_nCapacity = 0;
			m_bSupportRecycle = false;
		}

		/// <summary>
		/// 清空缓存池数据
		/// 缓存池属性依旧保留
		/// </summary>
		public void ClearPool()
		{
			if (m_ItemTable != null)
			{
				m_ItemTable.Clear();
			}
			if (m_FreeIndexArray != null)
			{
				m_FreeIndexArray.Clear();
			}
			if (m_UsingIndexArray != null)
			{
				m_UsingIndexArray.Clear();
			}
#if BTDEBUG
			if (m_ItemUseFrequencyTable != null)
			{
				m_ItemUseFrequencyTable.Clear();
			}
#endif
		}

		/// <summary>
		///  初始化对象池
		/// </summary>
		/// <param name="type"></param>
		/// <param name="nInitSize"></param>
		/// <param name="nCapacity"></param>
		/// <param name="objDefaultCreateParam"></param>
		/// <returns></returns>
		public bool InitPool(Type type, Int32 nInitSize, Int32 nCapacity, System.Object[] objDefaultCreateParam)
		{
			if (nInitSize < 0 || nCapacity < 1 || nInitSize > nCapacity)
			{
				BTDebug.Exception("<BT> Object Pool Init Exception");
				return false;
			}
			m_nCapacity = nCapacity;
			m_ObjectType = type;
			m_objDefaultCreateParam = objDefaultCreateParam;
			m_ItemTable = new Hashtable(nCapacity);
			m_FreeIndexArray = new ArrayList();
			m_UsingIndexArray = new ArrayList();
			if (type.IsAssignableFrom(typeof(IRecycleable)) == true)
			{
				m_bSupportRecycle = true;
			}
			else
			{
				m_bSupportRecycle = false;
			}

			for (Int32 i = 0; i < nInitSize; ++i )
			{
				ExtendPool();
			}

#if BTDEBUG
			m_ItemUseFrequencyTable = new Hashtable(nCapacity);
#endif
			return true;
		}

		/// <summary>
		/// 空闲对象个数
		/// </summary>
		/// <returns></returns>
		public Int32 FreeCount()
		{
			if (m_FreeIndexArray == null)
			{
				return 0;
			}
			return m_FreeIndexArray.Count;
		}

		/// <summary>
		/// 对象个数
		/// </summary>
		/// <returns></returns>
		public Int32 Count()
		{
			if (m_ItemTable == null)
			{
				return 0;
			}
			return m_ItemTable.Count;
		}

		/// <summary>
		/// 从对象池获取对象
		/// </summary>
		/// <returns></returns>
		public System.Object RentObject()
		{
			lock (this)
			{
				if (FreeCount() <= 0 && ExtendPool() == false)
				{
					return null;
				}
				Int32 nHashCode = (Int32)m_FreeIndexArray[0];
				m_FreeIndexArray.RemoveAt(0);
				System.Object obj = m_ItemTable[nHashCode];
				m_UsingIndexArray.Add(nHashCode);
				
				if (m_bSupportRecycle == true)
				{
					IRecycleable recyObj = (IRecycleable)obj;
					if (recyObj != null)
					{
						recyObj.Init();
					}
				}

#if BTDEBUG
				if (m_ItemUseFrequencyTable.Contains(nHashCode) == false)
				{
					m_ItemUseFrequencyTable.Add(nHashCode, 0);
				}
				Int32 nFrequency = (Int32)m_ItemUseFrequencyTable[nHashCode];
				m_ItemUseFrequencyTable[nHashCode] = nFrequency + 1;
#endif
				return obj;
			}
		}

		/// <summary>
		/// 释放对象
		/// </summary>
		/// <returns></returns>
		public bool GiveBackObject(System.Object obj)
		{
			lock (this)
			{
				if (obj == null)
				{
					return false;
				}
				if (m_UsingIndexArray == null || m_FreeIndexArray == null || m_ItemTable == null)
				{
					return false;
				}
				if (m_bSupportRecycle == true)
				{
					IRecycleable recycleable = (IRecycleable)obj;
					if (recycleable != null)
					{
						recycleable.Clear();
					}
				}
				Int32 nHashCode = obj.GetHashCode();
				if (m_UsingIndexArray.Contains(nHashCode) == false)
				{
					return false;
				}

				m_UsingIndexArray.Remove(nHashCode);
				m_FreeIndexArray.Add(nHashCode);
				return true;
			}
		}

		private bool ExtendPool()
		{
			lock (this)
			{
				if (Count() >= m_nCapacity)
				{
					return false;
				}

				System.Object newObject = null;
				try
				{
					newObject = System.Activator.CreateInstance(m_ObjectType, m_objDefaultCreateParam);
				}
				catch (System.Exception ex)
				{
					BTDebug.Exception(ex);
					return false;
				}

				Int32 nHashCode = newObject.GetHashCode();
				m_ItemTable.Add(nHashCode, newObject);
				m_FreeIndexArray.Add(nHashCode);
				return true;
			}
		}

#if BTDEBUG
		public string DebugPoolStatus()
		{
			Int32 nTotalTouchCount = 0;
			Int32 nTotalItemCount = Count();
			foreach (System.Object obj in m_ItemUseFrequencyTable)
			{
				Int32 nCount = (Int32)obj;
				nTotalTouchCount += nCount;
			}
			float fRecycleRatio = nTotalItemCount == 0 ? 0 : (float)nTotalTouchCount / (float)nTotalItemCount;
			string strPoolStatus = string.Empty;
			strPoolStatus = string.Format("Pool Object:{0} Status: Total Size:{1}, FreeSize:{2}, TotalTouch:{3}, Recycleable:{4:N2}",
				m_ObjectType.Name,
				Count(),
				FreeCount(),
				nTotalTouchCount,
				fRecycleRatio);

			return strPoolStatus;
		}
#endif
	}
}
