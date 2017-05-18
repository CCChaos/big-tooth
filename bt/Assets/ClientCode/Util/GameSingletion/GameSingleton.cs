using System;
using BTMISC;
using NS_GAMESHARE;
using NS_GAME;

/// <summary>
/// 游戏单件
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class CBTGameSingleton<T> : IGameSingleton
	where T : class , IGameSingleton, new()
{
	// 实例
	private static T m_Instance;
	// 生效状态
	protected UInt32 m_uRunningStateSlot = 0xFFFFFFFF;

	// 获取单件实例
	public static T Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new T();
				m_Instance.Init();
				CSingletonRegister.RegisterSingleton(m_Instance);
			}
			return m_Instance;
		}
	}

	public void RemoveRunningState(TGameStateType tGameStateType)
	{
		UInt32 uSlotMask = ~((UInt32)tGameStateType);
		m_uRunningStateSlot &= uSlotMask;
	}

	public void AddRunningState(TGameStateType tGameStateType)
	{
		m_uRunningStateSlot |= (UInt32)tGameStateType;
	}

	public void SetRunningState(TGameStateType tGameStateType)
	{
		m_uRunningStateSlot = (UInt32)tGameStateType;
	}

	public void Update()
	{
		UInt32 uCurGameStateSlot = (UInt32)CGameClient.Instance.GetCurrentState();
		if ((uCurGameStateSlot & m_uRunningStateSlot) == 0)
		{
			return;
		}
		OnUpdate();
	}

	// 接口方法 
	public abstract void OnUpdate();
	public abstract void Init();
	public abstract void Release();

}