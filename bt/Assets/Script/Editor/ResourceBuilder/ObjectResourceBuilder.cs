using System.Collections.Generic;
using System.IO;
using NS_GAME.DATA;
using BTIO;
using UnityEditor;
using UnityEngine;
using System;
using BTMISC;
using NS_GAME.KIT;
using NS_GAMEEDITOR;
using Object = UnityEngine.Object;

public class ObjectResourceBuilder
{
	// 打包一个场景中的模型资源
	private static bool _BuildEditorObject(GameObject objInScene, ref PBData_EditorResourceInfoList resourceList)
	{
		if (objInScene == null)
		{
			return false;
		}
		Object objPrefab = PrefabUtility.GetPrefabParent(objInScene);
		if (objPrefab == null)
		{
			return false;
		}
		string strKey = objPrefab.name;
		string strPath = AssetDatabase.GetAssetPath(objPrefab);
		if (resourceList == null)
		{
			resourceList = new PBData_EditorResourceInfoList();
		}
		if (resourceList.resouce_list == null)
		{
			resourceList.resouce_list = new List<PBData_EditorResourceInfo>();
		}

		bool bUpdate = false;
		foreach (PBData_EditorResourceInfo info in resourceList.resouce_list)
		{
			if (info == null)
			{
				continue;
			}
			if (info.key == strKey)
			{
				info.path = strPath;
				bUpdate = true;
			}
		}
		if (bUpdate == false)
		{
			PBData_EditorResourceInfo newInfo = new PBData_EditorResourceInfo();
			newInfo.key = strKey;
			newInfo.path = strPath;
			resourceList.resouce_list.Add(newInfo);
		}
		return true;
	}

	/// <summary>
	/// 打包选中模型或者选中节点下的所有模型
	/// </summary>
	/// <param name="objSelected"></param>
	/// <param name="resourceList"></param>
	/// <returns></returns>
	public static bool BuildEditorObject(GameObject objSelected, ref PBData_EditorResourceInfoList resourceList)
	{
		if (objSelected == null)
		{
			return false;
		}
		List<GameObject> objList = new List<GameObject>();
		GetBuildObject(ref objList, objSelected);
		if (objList == null)
		{
			return true;
		}
		bool bNoError = true;
		foreach (GameObject obj in objList)
		{
			if (_BuildEditorObject(obj, ref resourceList) == false)
			{
				bNoError = false;
				UnityEngine.Debug.LogError(string.Format("Build Object:{0} Failed", obj == null ? "NULL" : obj.name));
			}
		}
		return bNoError;
	}

	/// <summary>
	/// 打包输入场景中的所有模型
	/// </summary>
	/// <param name="objectList"></param>
	/// <param name="resourceList"></param>
	/// <returns></returns>
	public static bool BuildEditorObject(List<ResourceBuilder.CBuildAllObject> objectList, ref PBData_EditorResourceInfoList resourceList)
	{
		if (objectList == null)
		{
			return false;
		}
		if (resourceList == null)
		{
			resourceList = new PBData_EditorResourceInfoList();
		}
		Int32 nSize = objectList == null ? 0 : objectList.Count;

		bool bNoError = true;
		for (Int32 i = 0; i < nSize; ++i )
		{
			string strUnityScenePath = objectList[i].Path;
			if (EditorApplication.OpenScene(strUnityScenePath) == false)
			{
				UnityEngine.Debug.LogError(string.Format("Open Scene:<{0}> Failed", strUnityScenePath));
				continue;
			}
			EditorApplication.SaveScene(strUnityScenePath);
			List<GameObject> allObjectToPack = new List<GameObject>();
			GetBuildObject(ref allObjectToPack, null);
			foreach (GameObject objToPack in allObjectToPack)
			{
				if (_BuildEditorObject(objToPack, ref resourceList) == false)
				{
					bNoError = false;
					UnityEngine.Debug.LogError(string.Format("Build Object:{0} Failed", objToPack == null ? "NULL" : objToPack.name));
				}
			}
		}
		return bNoError;
	}

	public static bool BuildAssetObject(Object objSelected, ref PBData_AssetResourceInfoList resourceList)
	{
		return false;
	}

	public static bool BuildAssetObject(List<ResourceBuilder.CBuildAllObject> objectList, ref PBData_AssetResourceInfoList resourceList)
	{
		return false;
	}

	// 获取所有objParent下（包含）需要打包的资源节点
	private static void GetBuildObject(ref List<GameObject> rOutGoList, GameObject objParent = null)
	{
		if (rOutGoList == null)
		{
			rOutGoList = new List<GameObject>();
		}
		if (objParent == null)
		{
			List<GameObject> objTop = GameEditorUtil.GetALLTopGameObjectInScene();
			foreach (GameObject go in objTop)
			{
				GetBuildObject(ref rOutGoList, go);
			}
			return;
		}

		if (PrefabUtility.GetPrefabParent(objParent) == null)
		{
			Int32 nChildCount = objParent.transform.childCount;
			for (Int32 i = 0; i < nChildCount; ++i)
			{
				Transform transChild = objParent.transform.GetChild(i);
				if (transChild == null || transChild.gameObject == null)
				{
					continue;
				}
				GetBuildObject(ref rOutGoList, transChild.gameObject);
			}
		}
		else
		{
			rOutGoList.Add(objParent);
		}
	}
}