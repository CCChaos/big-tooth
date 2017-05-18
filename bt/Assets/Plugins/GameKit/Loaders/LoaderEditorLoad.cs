#if UNITY_EDITOR
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BTMISC;
using NS_GAME.DATA;
using NS_GAME.KIT;
using Object = UnityEngine.Object;


/// <summary>
/// 加载器
/// 通过Resources.Load加载
/// </summary>
public class LoaderEditorLoad : MonoBehaviour, IAssetsLoader
{
	/// <summary>
	/// 资源信息
	/// </summary>
	private class AssetInfo4EditorLoad
	{
		// 加载的资源
		private GameObject m_GameObject;
		// 名称 
		private string m_strName;
		// 路径
		private string m_strPath;
		// 资源文件名称
		private string m_strResourceName;
		// 引用次数
		private Int32 m_nRefCount;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="strName"></param>
		/// <param name="strPath"></param>
		public AssetInfo4EditorLoad(string strName, string strPath)
		{
			m_strName = strName;
			m_strPath = strPath;
			m_strResourceName = Util.GetFileNameWithoutExtention(Util.NormalizeFilePath(m_strPath));
			m_nRefCount = 0;
			m_GameObject = null;
		}

		public string GetName() { return m_strName; }
		public string GetPath() { return m_strPath; }

		public GameObject GetLoadedGameObject() { return m_GameObject; }
		public bool IsLoaded() { return m_GameObject != null; }
		public Int32 GetRefCount() { return m_nRefCount; }
		public Int32 AddRefCount() { m_nRefCount += 1; return m_nRefCount; }
		public Int32 DecRefCount() { m_nRefCount -= 1; return m_nRefCount; }
		public void UpdatePath(string strNewPath)
		{
			m_strPath = strNewPath;
			m_strResourceName = Util.GetFileNameWithoutExtention(Util.NormalizeFilePath(m_strPath));
		}
		/// <summary>
		/// 加载
		/// </summary>
		/// <param name="handleOnLoadComplete"></param>
		/// <returns></returns>
		public IEnumerator AsynLoad(Action<Object> handleOnLoadComplete)
		{
			while (true)
			{
				if (m_GameObject != null)
				{
					if (handleOnLoadComplete != null)
					{
						handleOnLoadComplete(m_GameObject);
					}
					yield break;
				}

				try
				{
					m_GameObject = Resources.LoadAssetAtPath(m_strPath, typeof(GameObject)) as GameObject;
					if (m_GameObject == null)
					{
						BTDebug.Warning(string.Format("Editor Load {0} Failed, Record Path:{1}", m_strName, m_strPath), "RESOURCE");
					}
				}
				catch (System.Exception ex)
				{
					BTDebug.ExceptionEx(ex);
				}
				yield return null;

				if (handleOnLoadComplete != null)
				{
					handleOnLoadComplete(m_GameObject);
				}

				yield break;
			}
		}
	}

	// 资源信息Map
	private Dictionary<string, AssetInfo4EditorLoad> m_LoadedResourceInfoMap;
	// 对象
	private static LoaderEditorLoad m_Instance;
	public static LoaderEditorLoad Instance
	{
		get
		{
			return m_Instance;
		}
	}
	/// <summary>
	/// Constructor
	/// </summary>
	void Awake()
	{
		m_Instance = this;
		m_LoadedResourceInfoMap = new Dictionary<string, AssetInfo4EditorLoad>();
	}

	/// <summary>
	/// Destructor
	/// </summary>
	void OnDestroy()
	{
		m_Instance = null;
		m_LoadedResourceInfoMap.Clear();
	}

	/// <summary>
	/// 加载资源
	/// </summary>
	/// <param name="strName"></param>
	/// <param name="handleOnLoaded"></param>
	/// <param name="fProgress"></param>
	public void LoadAsset(string strName, Action<Object> handleOnLoaded, Action<float> fProgress)
	{
		AssetInfo4EditorLoad info = null;
		if (m_LoadedResourceInfoMap.TryGetValue(strName, out info) == false ||
			info == null)
		{
			string strPath = string.Empty;
			if (Data_EditorObjectList.TryGetFilePath(strName, out strPath) == false)
			{
				BTDebug.Warning(string.Format("Resource:{0} Not Found In Path Info Map", strName), "RESOURCE");
				if (handleOnLoaded != null)
				{
					handleOnLoaded(null);
					return;
				}
			}
			info = new AssetInfo4EditorLoad(strName, strPath);
			m_LoadedResourceInfoMap.Add(strName, info);
		}
		info.AddRefCount();
		StartCoroutine(info.AsynLoad(handleOnLoaded));
	}

	/// <summary>
	/// 释放资源
	/// </summary>
	/// <param name="strName"></param>
	public void ReleaseAsset(string strName)
	{
		AssetInfo4EditorLoad info = null;
		if (m_LoadedResourceInfoMap.TryGetValue(strName, out info) == false ||
			info == null)
		{
			return;
		}
		info.DecRefCount();
	}

	/// <summary>
	/// 从内存卸载资源
	/// </summary>
	public void UnLoadUnusedAssets()
	{
		BTDebug.Exception("TODO UnLoad");
	}
}
#endif // UNITY_EDITOR