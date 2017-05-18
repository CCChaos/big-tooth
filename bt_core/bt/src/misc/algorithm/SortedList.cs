using System;
using System.Collections.Generic;

namespace BTMISC
{
	/// <summary>
	/// 有序列表 将Value排序
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class SortedQueue<TKey, TValue> 
		where TKey : IComparable<TKey>
		where TValue : IComparable<TValue>
	{

		private List<CKeyValueCompable<TKey, TValue>> m_Array;

		/// <summary>
		/// Constructor
		/// </summary>
		public SortedQueue()
		{
			m_Array = new List<CKeyValueCompable<TKey, TValue>>();
		}

		/// <summary>
		/// 添加一个元素
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Push(TKey key, TValue value)
		{
			if (m_Array == null)
			{
				m_Array = new List<CKeyValueCompable<TKey, TValue>>();
			}

			TValue existValue;
			if (Find(key, out existValue) == true)
			{
				return false;
			}

			bool bInserted = false;
			Int32 nSize = m_Array.Count;
			CKeyValueCompable<TKey, TValue> newKeyValuePair = new CKeyValueCompable<TKey, TValue>(key, value);
			for (Int32 i = 0; i < nSize; ++i )
			{
				CKeyValueCompable<TKey, TValue> keyValuePair = m_Array[i];
				if (keyValuePair == null)
				{
					continue;
				}
				if (keyValuePair.GetValue().CompareTo(value) <= 0)
				{
					m_Array.Insert(i, newKeyValuePair);
					bInserted = true;
				}
			}

			if (bInserted == false)
			{
				m_Array.Add(newKeyValuePair);
			}

			return true;
		}

		/// <summary>
		/// 弹出第一个元素
		/// </summary>
		/// <param name="rOutKey"></param>
		/// <param name="rOutValue"></param>
		/// <returns></returns>
		public bool Pop(out TKey rOutKey, out TValue rOutValue)
		{
			rOutKey = default(TKey);
			rOutValue = default(TValue);
			if (m_Array == null || m_Array.Count < 1)
			{
				return false;
			}

			CKeyValueCompable<TKey, TValue> keyValuePair = m_Array[0];
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
		/// 第一个元素
		/// </summary>
		/// <param name="rOutKey"></param>
		/// <param name="rOutValue"></param>
		/// <returns></returns>
		public bool Front(out TKey rOutKey, out TValue rOutValue)
		{
			rOutKey = default(TKey);
			rOutValue = default(TValue);
			if (m_Array == null || m_Array.Count < 1)
			{
				return false;
			}

			CKeyValueCompable<TKey, TValue> keyValuePair = m_Array[0];
			if (keyValuePair == null)
			{
				return false;
			}
			rOutKey = keyValuePair.GetKey();
			rOutValue = keyValuePair.GetValue();
			return true;
		}

		/// <summary>
		/// 查找一个元素
		/// </summary>
		/// <param name="key"></param>
		/// <param name="rOutValue"></param>
		/// <returns></returns>
		public bool Find(TKey key, out TValue rOutValue)
		{
			rOutValue = default(TValue);
			if (m_Array == null)
			{
				return false;
			}
			Int32 nSize = 0;
			for (Int32 i = 0; i < nSize; ++i )
			{
				CKeyValueCompable<TKey, TValue> keyValuePair = m_Array[i];
				if (keyValuePair == null)
				{
					continue;
				}
				if (keyValuePair.GetKey().CompareTo(key) == 0)
				{
					rOutValue = keyValuePair.GetValue();
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 元素数量
		/// </summary>
		/// <returns></returns>
		public Int32 GetCount()
		{
			if (m_Array == null)
			{
				return 0;
			}
			return m_Array.Count;
		}

	}
}
