using System;
using System.Collections.Generic;

namespace BTMISC
{
	/// <summary>
	/// Map，用于小数量KeyValue对象存储与操作
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class QuickList<TKey, TValue> where TKey : IComparable<TKey>
	{
		// 数据列表
		private List<CKeyValue<TKey, TValue>> m_Array;

		/// <summary>
		/// Constructor
		/// </summary>
		public QuickList()
		{
			m_Array = new List<CKeyValue<TKey, TValue>>();
		}

		/// <summary>
		/// 是否有键值为key的对象
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainKey(TKey key)
		{
			TValue tmpValue = default(TValue);
			if (QuickFind(key, ref tmpValue) == false)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 查找
		/// </summary>
		/// <param name="key"></param>
		/// <param name="rOutValue"></param>
		/// <returns></returns>
		public bool QuickFind(TKey key, ref TValue rOutValue)
		{
			rOutValue = default(TValue);

			if (m_Array == null || m_Array.Count < 1)
			{
				return false;
			}

			Int32 nFront = 0;
			Int32 nEnd = m_Array.Count - 1;
			Int32 nMid = (nFront + nEnd) / 2;

			while (nFront <= nEnd)
			{
				CKeyValue<TKey, TValue> keyValuePair = m_Array[nMid];
				if (keyValuePair == null)
				{
					return false;
				}

				int compare = keyValuePair.GetKey().CompareTo(key);

				if (compare == 0)
				{
					rOutValue = keyValuePair.GetValue();
					return true;
				}
				if (compare < 0)
				{
					nFront = nMid + 1;
				}
				else
				{
					nEnd = nMid - 1;
				}
				nMid = (nFront + nEnd) / 2;
			}

			return false;
		}

		/// <summary>
		/// 更新值
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool UpdateValue(TKey key, TValue value)
		{
			if (m_Array == null || m_Array.Count < 1)
			{
				return false;
			}

			Int32 nFront = 0;
			Int32 nEnd = m_Array.Count - 1;
			Int32 nMid = (nFront + nEnd) / 2;

			while (nFront <= nEnd)
			{
				CKeyValue<TKey, TValue> keyValuePair = m_Array[nMid];
				if (keyValuePair == null)
				{
					return false;
				}

				int compare = keyValuePair.GetKey().CompareTo(key);

				if (compare == 0)
				{
					keyValuePair.SetValue(value);
					return true;
				}
				if (compare < 0)
				{
					nFront = nMid + 1;
				}
				else
				{
					nEnd = nMid - 1;
				}
				nMid = (nFront + nEnd) / 2;
			}

			return false;
		}
		/// <summary>
		/// 添加
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Add(TKey key, TValue value)
		{
			CKeyValue<TKey, TValue> newKeyValuePair = new CKeyValue<TKey, TValue>(key, value);
			if (m_Array.Count == 0)
			{
				m_Array.Add(newKeyValuePair);
				return true;
			}
			Int32 nSize = m_Array.Count;

			for (Int32 i = 0; i < nSize; ++i)
			{
				CKeyValue<TKey, TValue> keyValuePair = m_Array[i];
				if (keyValuePair == null)
				{
					continue;
				}
				Int32 nCmpRet = keyValuePair.GetKey().CompareTo(key);
				if (nCmpRet == 0)
				{
					return false;
				}
				if (nCmpRet > 0)
				{
					m_Array.Insert(i, newKeyValuePair);
					return true;
				}
			}
			m_Array.Add(newKeyValuePair);
			return true;
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Erase(TKey key)
		{
			if (m_Array == null || m_Array.Count < 1)
			{
				return false;
			}

			Int32 nFront = 0;
			Int32 nEnd = m_Array.Count - 1;
			Int32 nMid = (nFront + nEnd) / 2;

			while (nFront <= nEnd)
			{
				CKeyValue<TKey, TValue> keyValuePair = m_Array[nMid];
				if (keyValuePair == null)
				{
					return false;
				}

				int compare = keyValuePair.GetKey().CompareTo(key);

				if (compare == 0)
				{
					m_Array.RemoveAt(nMid);
					return true;
				}
				if (compare < 0)
				{
					nFront = nMid + 1;
				}
				else
				{
					nEnd = nMid - 1;
				}
				nMid = (nFront + nEnd) / 2;
			}
			return false;
		}
		
		/// <summary>
		/// 键值列表
		/// </summary>
		/// <returns></returns>
		public List<TKey> KeyList()
		{
			List<TKey> keyList = new List<TKey>();
			Int32 nSize = m_Array == null ? 0 : m_Array.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				CKeyValue<TKey, TValue> keyValuePair = m_Array[i];
				if (keyValuePair == null)
				{
					continue;
				}
				keyList.Add(keyValuePair.GetKey());
			}
			return keyList;
		}

		/// <summary>
		/// 值列表
		/// </summary>
		public List<TValue> ValueList()
		{
			List<TValue> valueList = new List<TValue>();
			Int32 nSize = m_Array == null ? 0 : m_Array.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				CKeyValue<TKey, TValue> keyValuePair = m_Array[i];
				if (keyValuePair == null)
				{
					continue;
				}
				valueList.Add(keyValuePair.GetValue());
			}
			return valueList;
		}

		/// <summary>
		/// 弹出队列头元素
		/// </summary>
		/// <param name="rOutKey"></param>
		/// <param name="rOutValue"></param>
		/// <returns></returns>
		public bool PopFront(ref TKey rOutKey, ref TValue rOutValue)
		{
			if (m_Array == null || m_Array.Count <= 0)
			{
				return false;
			}
			CKeyValue<TKey, TValue> keyValuePair = m_Array[0];
			if (keyValuePair == null)
			{
				return false;
			}
			m_Array.RemoveAt(0);
			rOutKey = keyValuePair.GetKey();
			rOutValue = keyValuePair.GetValue();
			return true;
		}

		/// <summary>
		/// 弹出元素
		/// </summary>
		/// <param name="key"></param>
		/// <param name="rOutValue"></param>
		/// <returns></returns>
		public bool Pop(TKey key, ref TValue rOutValue)
		{
			if (m_Array == null || m_Array.Count < 1)
			{
				return false;
			}

			Int32 nFront = 0;
			Int32 nEnd = m_Array.Count - 1;
			Int32 nMid = (nFront + nEnd) / 2;

			while (nFront <= nEnd)
			{
				CKeyValue<TKey, TValue> keyValuePair = m_Array[nMid];
				if (keyValuePair == null)
				{
					return false;
				}

				int compare = keyValuePair.GetKey().CompareTo(key);

				if (compare == 0)
				{
					rOutValue = keyValuePair.GetValue();
					m_Array.RemoveAt(nMid);
					return true;
				}
				if (compare < 0)
				{
					nFront = nMid + 1;
				}
				else
				{
					nEnd = nMid - 1;
				}
				nMid = (nFront + nEnd) / 2;
			}
			return false;
		}

		/// <summary>
		/// 清理
		/// </summary>
		public void Clear()
		{
			if (m_Array == null)
			{
				return;
			}
			m_Array.Clear();
		}

		/// <summary>
		/// 数量
		/// </summary>
		/// <returns></returns>
		public Int32 Size()
		{
			if (m_Array == null)
			{
				return 0;
			}
			return m_Array.Count;
		}

		/// <summary>
		/// 索引
		/// </summary>
		/// <param name="nIndex"></param>
		/// <returns></returns>
		public CKeyValue<TKey, TValue> IndexOf(Int32 nIndex)
		{
			if (nIndex < 0 || nIndex >= Size())
			{
				return null;
			}
			return m_Array[nIndex];
		}
	}
}

