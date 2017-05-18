using System;
using System.Collections.Generic;

namespace BTMISC
{
	/// <summary>
	/// 单件接口
	/// </summary>
	public interface IGameSingleton
	{
		void Update();
		void Init();
		void Release();
	}

	/// <summary>
	/// 单件注册器
	/// </summary>
	public class CSingletonRegister
	{
		// 单件列表
		private static List<IGameSingleton> m_SingletonList;

		/// <summary>
		/// 注册单件
		/// </summary>
		/// <param name="singleton"></param>
		/// <returns></returns>
		public static bool RegisterSingleton(IGameSingleton singleton)
		{
			if (singleton == null)
			{
				return false;
			}
			if (m_SingletonList == null)
			{
				m_SingletonList = new List<IGameSingleton>();
			}
			Int32 nSize = m_SingletonList.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				if (m_SingletonList[i] == singleton)
				{
					return false;
				}
			}
			m_SingletonList.Add(singleton);
			return true;
		}

		/// <summary>
		/// 所有单件的更新
		/// </summary>
		public static void OnUpdate()
		{
			if (m_SingletonList == null)
			{
				return;
			}
			Int32 nSize = m_SingletonList.Count;
			for (Int32 i = 0; i < nSize; ++i)
			{
				IGameSingleton singleton = m_SingletonList[i];
				if (singleton == null)
				{
					continue;
				}
				singleton.Update();
			}
		}

		/// <summary>
		/// 销毁所有注册单件
		/// </summary>
		public static void Release()
		{
			if (m_SingletonList == null)
			{
				return;
			}
			Int32 nSize = m_SingletonList.Count;
			for (Int32 i = 0; i < nSize; ++i)
			{
				IGameSingleton singleton = m_SingletonList[i];
				if (singleton == null)
				{
					continue;
				}
				singleton.Release();
			}
		}
	}
}
