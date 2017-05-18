using System;
using System.Collections.Generic;
using System.Text;

namespace BTMISC
{
	/// <summary>
	/// KeyValue Pair
	/// </summary>
	/// <typeparam name="TKeyEx"></typeparam>
	/// <typeparam name="TValueEx"></typeparam>
	public class CKeyValue<TKeyEx, TValueEx> where TKeyEx : IComparable<TKeyEx>
	{
		private TKeyEx m_key;
		private TValueEx m_value;

		public CKeyValue(TKeyEx key, TValueEx value)
		{
			m_key = key;
			m_value = value;
		}
		/// <summary>
		/// 获取Key
		/// </summary>
		/// <returns></returns>
		public TKeyEx GetKey()
		{
			return m_key;
		}

		/// <summary>
		/// 获取Value
		/// </summary>
		/// <returns></returns>
		public TValueEx GetValue()
		{
			return m_value;
		}

		/// <summary>
		/// 设置值
		/// </summary>
		public void SetValue(TValueEx value)
		{
			m_value = value;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TKeyEx"></typeparam>
	/// <typeparam name="TValueEx"></typeparam>
	public class CKeyValueCompable<TKeyEx, TValueEx>
		where TKeyEx : IComparable<TKeyEx>
		where TValueEx : IComparable<TValueEx>
	{
		
		private TKeyEx m_key;
		private TValueEx m_value;

		public CKeyValueCompable(TKeyEx key, TValueEx value)
		{
			m_key = key;
			m_value = value;
		}
		/// <summary>
		/// 获取Key
		/// </summary>
		/// <returns></returns>
		public TKeyEx GetKey()
		{
			return m_key;
		}

		/// <summary>
		/// 获取Value
		/// </summary>
		/// <returns></returns>
		public TValueEx GetValue()
		{
			return m_value;
		}
	}
}
