using UnityEngine;
using System;
using Object = UnityEngine.Object;
using System.Xml;

/// <summary>
/// 资源加载器的接口类
/// 管理AssetsBundle包资源以及内存的资源
/// </summary>
public interface IAssetsLoader
{
	void LoadAsset(string strName, Action<Object> handleOnLoaded, Action<float> fProgress);

	//void SecondLoadAsset(string strName, Action<Object> handleOnLoaded, Action<float> fProgress);
	//void SecondLoadUIAsset(string strName, Action<Object> handleOnLoaded, Action<float> fProgress);
	//Object SynLoadAsset(string strName);
	//void LoadSceneAsset(string strName, Action<Object> handleOnLoaded);
	//Object LoadLocalAsset(string strName);

	//void ReleaseLocalAsset(string strName);
	void ReleaseAsset(string strName);
	//void Release(string strName, Boolean releaseAsset);
	void UnLoadUnusedAssets();

	//void SetPathMap();
	//void ForceClear();
	//void ClearLoadAssetTasks();
	//void UnloadAsset(string strName);
}
