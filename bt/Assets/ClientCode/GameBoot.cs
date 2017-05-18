using UnityEngine;
using System.Collections;
using BTMISC;
using NS_GAME.DATA;
using NS_GAME.KIT;

public class GameBoot : MonoBehaviour 
{
#if UNITY_EDITOR
	public TSysRunningType RunningType = TSysRunningType.enEditor;
#else
	public TSysRunningType RunningType = TSysRunningType.enRelease;
#endif

	// 挂载游戏主逻辑驱动的GameObject
	private GameObject goDriver = null;

	void Awake()
	{
		goDriver = new GameObject("Driver");
		GameObject.DontDestroyOnLoad(goDriver);
	}

	void Start ()
	{
		// 初始化程序核心模块
		StartKernel();
		// 启动游戏驱动
		StartDrive();
		// 删除引导
		GameObject.Destroy(gameObject);
	}

#region 程序初始化
	void StartKernel()
	{
		InitGameKit();
		InitGameDebug();
		InitApplicationSetting();
		InitGameAction();
		InitResourceLoader();

		Init3rdSDK();
	}
	void StartDrive()
	{
		GameDrive gameDrive = goDriver.AddComponent<GameDrive>();
		if (gameDrive == null)
		{
			return;
		}
		if (gameDrive.GameStart(RunningType) == false)
		{
			Debug.LogError("Game Start Failed");
			return;
		}
	}

	// 初始化游戏工具
	void InitGameKit()
	{
		RunningTimeSettings.SystemRunningType = RunningType;
		CGameKitSingletonCollection.InitGameKit();
	}
	// 初始化Debug
	void InitGameDebug()
	{
		BTDebug.RegisterHandle(UnityEngine.Debug.Log, BTDebug.TDebugLevel.enLog);
		BTDebug.RegisterHandle(UnityEngine.Debug.LogWarning, BTDebug.TDebugLevel.enWarning);
		BTDebug.RegisterHandle(UnityEngine.Debug.LogError, BTDebug.TDebugLevel.enError);
		BTDebug.RegisterHandle(UnityEngine.Debug.LogError, BTDebug.TDebugLevel.enException);
	}
	// 初始化游戏设置
	void InitApplicationSetting()
	{
		Application.targetFrameRate = 30;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	// 初始化事件
	void InitGameAction()
	{
		NS_GAMESHARE.CGameAction.InitGameAction();
	}
	// 资源加载器
	void InitResourceLoader()
	{
#if UNITY_EDITOR
		// Editor 加载器
		goDriver.AddComponent<LoaderEditorLoad>();
#endif
		// WWW 加载器
		goDriver.AddComponent<LoaderWWWLoad>();
		// Resource.Load 加载器
		// todo
	}
	// 初始化SDK
	void Init3rdSDK()
	{

	}
#endregion
}