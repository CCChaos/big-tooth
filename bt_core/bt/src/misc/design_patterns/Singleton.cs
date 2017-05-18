using System;

namespace BTMISC
{
	/// <summary>
	/// 普通单件
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CBTSingleton<T>
		where T : class , new()
	{
		// 实例
		private static T m_Instance;

		// 获取单件实例
		public static T Instance
		{
			get
			{
				if (m_Instance == null)
				{
					m_Instance = new T();
				}
				return m_Instance;
			}
		}
	}

}
