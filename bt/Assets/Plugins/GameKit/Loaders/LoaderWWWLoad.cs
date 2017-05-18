using System;
using UnityEngine;
using System.Collections;
using BTMISC;
using Object = UnityEngine.Object;

public class LoaderWWWLoad : MonoBehaviour, IAssetsLoader
{
	public void LoadAsset(string strName, Action<Object> handleOnLoaded, Action<float> fProgress)
	{

	}

	public void ReleaseAsset(string strName)
	{

	}

	public void UnLoadUnusedAssets()
	{

	}
}