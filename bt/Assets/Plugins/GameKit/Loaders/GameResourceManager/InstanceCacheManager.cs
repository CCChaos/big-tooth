using System;
using UnityEngine;
using System.Collections.Generic;
using BTMISC;
using Object = UnityEngine.Object;
using NS_GAME.KIT;

/// <summary>
/// 资源实体管理器
/// 管理GameObject实体
/// </summary>
public class InstanceCacheManager : CBTSingleton<InstanceCacheManager>, IGameSingleton
{
	// 加载器
	private IAssetsLoader m_AssetsLoader;
	// 实体Map
	private Dictionary<Int32, string> m_InstanceMap;

	// 实体缓存池
	//private CObjectPool m_InstancePool;
	//// 缓存池资源挂载点
	//private GameObject m_objPoolRoot;

	/// <summary>
	/// Constructor
	/// </summary>
	public InstanceCacheManager()
	{
		m_InstanceMap = new Dictionary<Int32, string>();
		//m_objPoolRoot = new GameObject("AssetsPool");
		//GameObject.DontDestroyOnLoad(m_objPoolRoot);
	}

	/// <summary>
	/// 获取GameObject资源实体
	/// </summary>
	/// <param name="strName"></param>
	/// <param name="handleOnComplete"></param>
	/// <param name="handleOnProgress"></param>
	/// <returns></returns>
	public bool GetInstance(string strName, Action<string, Object> handleOnComplete, Action<float> handleOnProgress)
	{
		if (string.IsNullOrEmpty(strName) == true)
		{
			return false;
		}
		// 适配加载器
		m_AssetsLoader = AdapterLoader(strName);
		if (m_AssetsLoader == null)
		{
			return false;
		}
		// 加载
		m_AssetsLoader.LoadAsset(strName, (objectGo) =>
		{
			if (objectGo == null && handleOnComplete != null)
			{
				handleOnComplete(strName, null);
				return;
			}

			GameObject go = GameObject.Instantiate(objectGo) as GameObject;
			Int32 guid = -1;
			if (go != null)
			{
				guid = go.GetInstanceID();
			}
			m_InstanceMap.Add(guid, strName);
			if (handleOnComplete != null)
			{
				handleOnComplete(strName, go);
			}
		}, handleOnProgress);

		return true;
	}

	/// <summary>
	/// 获取GameObject实体资源
	/// </summary>
	/// <param name="strName">资源名称</param>
	/// <param name="handleOnComplete">回调（资源名称，ID，资源）</param>
	/// <returns></returns>
	public bool GetInstance(string strName, Action<string, Object> handleOnComplete)
	{
		bool bGetRet = GetInstance(strName, handleOnComplete, null);
		return bGetRet;
	}

	/// <summary>
	/// 获取GameObject实体，实体在一定时间之后自动销毁
	/// </summary>
	/// <param name="strName"></param>
	/// <param name="handleOnComplete"></param>
	/// <param name="fDuration"></param>
	/// <returns></returns>
	public bool GetInstanceAutoRelease(string strName, Action<string, Object> handleOnComplete, float fDuration)
	{
		if (fDuration <= 0)
		{
			return false;
		}

		bool bRet = GetInstance(strName, (name, objectGo) =>
			{
				CTimerHeap.Instance.RegisterCountDownTimer(fDuration, (releaseGo) =>
					{
						ReleaseInstance(releaseGo as GameObject);
					}, objectGo);
			});
		return bRet;
	}
	/// <summary>
	/// 加载资源
	/// </summary>
	/// <param name="strName">资源名称</param>
	/// <param name="handleOnComplete">加载完成回调</param>
	/// <returns></returns>
	public bool GetAsset(string strName, Action<string, Int32, Object> handleOnComplete)
	{
		return false;
	}

	/// <summary>
	/// 释放GameObject实体
	/// </summary>
	/// <param name="go"></param>
	/// <returns></returns>
	public bool ReleaseInstance(Object go)
	{
		if (go == null)
		{
			return false;
		}
		Int32 guid = go.GetInstanceID();
		string strName = string.Empty;
		if (m_InstanceMap.TryGetValue(guid, out strName) == false)
		{
			BTDebug.Warning(string.Format("Release Game Object:{0} No Record in Map", go.name), "RESOURCE");
			return false;
		}

		m_InstanceMap.Remove(guid);
		m_AssetsLoader.ReleaseAsset(strName);
		GameObject.Destroy(go);
		return true;
	}

	/// <summary>
	/// 释放资源
	/// </summary>
	/// <param name="go"></param>
	/// <returns></returns>
	public bool ReleaseAsset(Object go)
	{
		return false;
	}

	public void Update()
	{

	}

	public void Init()
	{

	}

	public void Release()
	{

	}

	// 根据资源选择加载器
	private IAssetsLoader AdapterLoader(string strResName)
	{
		IAssetsLoader loader = null;
		// todo 根据资源打包的配置确定资源被打包的方式，选择不同的加载器
		loader = LoaderEditorLoad.Instance;
		return loader;
	}
}