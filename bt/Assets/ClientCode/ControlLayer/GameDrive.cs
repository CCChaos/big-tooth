using UnityEngine;
using System;
using BTMISC;
using NS_GAME;
using BTFSM;
using NS_GAMESHARE;

/// <summary>
/// 游戏引擎
/// </summary>
public class GameDrive : MonoBehaviour
{
	#region Members
	private static GameDrive s_Instance;
	public static GameDrive Instance { get { return s_Instance; } }
#endregion

	#region Method
	/// <summary>
	/// 启动游戏机
	/// </summary>
	/// <returns></returns>
	public bool GameStart(TSysRunningType tType)
	{
		bool bStartRet = CGameClient.Instance.Start();
		return bStartRet;
	}

	/// <summary>
	/// 游戏驱动更新函数
	/// </summary>
	private void DriveUpdate()
	{
		CSingletonRegister.OnUpdate();
		CGameKitSingletonCollection.UpdateGameKit();
	}

	/// <summary>
	/// 游戏驱动资源释放
	/// </summary>
	private void DriveRelease()
	{
		CSingletonRegister.Release();
		CGameKitSingletonCollection.ReleaseGameKit();
	}
#endregion

	#region Unity Method
	void Awake()
	{
		s_Instance = this;
	}

	void Start()
	{
	}

	void Update()
	{
		DriveUpdate();
	}

	void OnDestroy()
	{
		DriveRelease();
	}

#endregion

#if UNITY_EDITOR
	void OnGUI()
	{
		if (GUI.Button(new Rect(20, 20, 100, 40), "Test"))
		{
			//InstanceCacheManager.Instance.GetInstance("bl_00", (name, obj) =>
			//    {
			//        BTDebug.Warning("Load:" + name + " Type:" + obj.GetType());
			//    });
			string strDump = NS_GAME.DATA.Data_ConfigTree.DumpData();
			BTDebug.Log(strDump);
			strDump = NS_GAME.DATA.Data_EditorTextList.DumpData();
			BTDebug.Log(strDump);
			strDump = NS_GAME.DATA.Data_EditorObjectList.DumpData();
			BTDebug.Log(strDump);

			InstanceCacheManager.Instance.GetInstance("bl_00", (name, instance) =>
			{
				
			});

		}
	}
#endif
	
}
