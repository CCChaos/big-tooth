using System;
using System.Collections.Generic;
using BTMISC;
using NS_GAME.KIT;

/// <summary>
/// 游戏组件集合
/// </summary>
public class CGameKitSingletonCollection
{
	// 所有注册的单件游戏组件
	private static List<IGameSingleton> ms_SingletonKitList;

	/// <summary>
	/// 初始化
	/// </summary>
	public static void InitGameKit()
	{
		RegisterGameKit();
		Int32 nSize = ms_SingletonKitList == null ? 0 : ms_SingletonKitList.Count;
		for (Int32 i = 0; i < nSize; ++i )
		{
			IGameSingleton gameKit = ms_SingletonKitList[i];
			if (gameKit == null)
			{
				continue;
			}
			gameKit.Init();
		}
	}

	/// <summary>
	/// 更新
	/// </summary>
	public static void UpdateGameKit()
	{
		Int32 nSize = ms_SingletonKitList == null ? 0 : ms_SingletonKitList.Count;
		for (Int32 i = 0; i < nSize; ++i)
		{
			IGameSingleton gameKit = ms_SingletonKitList[i];
			if (gameKit == null)
			{
				continue;
			}
			gameKit.Update();
		}
	}

	/// <summary>
	/// 释放
	/// </summary>
	public static void ReleaseGameKit()
	{
		Int32 nSize = ms_SingletonKitList == null ? 0 : ms_SingletonKitList.Count;
		for (Int32 i = 0; i < nSize; ++i)
		{
			IGameSingleton gameKit = ms_SingletonKitList[i];
			if (gameKit == null)
			{
				continue;
			}
			gameKit.Release();
		}
	}

	private static void RegisterGameKit()
	{
		if (ms_SingletonKitList == null)
		{
			ms_SingletonKitList = new List<IGameSingleton>();
		}
		ms_SingletonKitList.Add(CActionDispatcher.Instance);
		ms_SingletonKitList.Add(CTimerHeap.Instance);
		ms_SingletonKitList.Add(CHttpRequest.Instance);
	}
}