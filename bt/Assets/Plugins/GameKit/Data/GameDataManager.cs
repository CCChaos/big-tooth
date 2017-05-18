using System;
using System.Collections.Generic;
using System.IO;
using BTMISC;
using NS_GAME.KIT;
using NS_GAME.DATA;

/// <summary>
/// 文本数据类资源管理器
/// </summary>
public class GameDataManager : CBTSingleton<GameDataManager>
{
	private const string cStrGameDataLoadMethodName = "Load";
	private const string cStrGameDataClearMethodName = "Clear";

	protected List<Type> mLoadedGameDataList;

	/// <summary>
	/// Constructor
	/// </summary>
	public GameDataManager()
	{
		mLoadedGameDataList = new List<Type>();
	}

	/// <summary>
	/// 加载数据
	/// </summary>
	/// <param name="dataTypeList"></param>
	/// <param name="handleOnLoadComplete"></param>
	/// <param name="handleOnLoadError"></param>
	/// <param name="bClearIfLoaded"></param>
	/// <returns></returns>
	public bool StartLoadData(List<Type> dataTypeList, Action handleOnLoadComplete, Action<string> handleOnLoadError, bool bClearIfLoaded = false)
	{
		if (dataTypeList == null)
		{
			return false;
		}
		Int32 nSize = dataTypeList.Count;
		bool bErrorOccurred = false;
		string strErrorInfo = string.Empty;

		for (Int32 i = 0; i < nSize; ++i )
		{
			Action tmpActionLoadDone = () =>
				{
					if (i != nSize - 1) // 加载完
					{
						return;
					}
					if (bErrorOccurred == false && handleOnLoadComplete != null)
					{
						handleOnLoadComplete();
					}
					if (bErrorOccurred == true && handleOnLoadError != null)
					{
						handleOnLoadError(strErrorInfo);
					}
				};

			Action<string> tmpActionLoadError = (info) =>
				{
					bErrorOccurred = true;
					strErrorInfo += info;
					if (i != nSize - 1) // 加载完
					{
						return;
					}
					if (handleOnLoadError != null)
					{
						handleOnLoadError(strErrorInfo);
					}
				};

			if (StartLoadData(dataTypeList[i], tmpActionLoadDone, tmpActionLoadError, bClearIfLoaded) == false)
			{
				bErrorOccurred = true;
			}
		}
		return !bErrorOccurred;
	}

	/// <summary>
	/// 加载数据
	/// </summary>
	/// <param name="dataType"></param>
	/// <param name="handleOnLoadComplete"></param>
	/// <param name="handleOnLoadError"></param>
	/// <param name="bClearIfLoaded"></param>
	/// <returns></returns>
	public bool StartLoadData(Type dataType, Action handleOnLoadComplete, Action<string> handleOnLoadError, bool bClearIfLoaded = false)
	{
		if (dataType == null)
		{
			return false;
		}
		bool bRet = false;
		Type gameDataType = GetGameDataCollectionType(dataType.BaseType);
		if (gameDataType == null)
		{
			BTDebug.Warning(string.Format("Load DataType:{0} Not AssignableFrom GameData", dataType.Name), "DATA");
			return false;
		}

		if (mLoadedGameDataList.Contains(dataType))
		{
			if (bClearIfLoaded == true)
			{
				ClearData(dataType);
			}
			else
			{
				BTDebug.Warning(string.Format("Dup Load DataType:{0} ", dataType.Name), "DATA");
				return false;
			}
		}

		System.Reflection.MethodInfo methodInfo = gameDataType.GetMethod(
			cStrGameDataLoadMethodName,
			System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

		if (methodInfo == null)
		{
			BTDebug.Warning(string.Format("DataType:{0} No {1} Method", dataType.Name, cStrGameDataLoadMethodName), "DATA");
			return false;
		}
		try
		{
			Object[] paramArray = { handleOnLoadComplete, handleOnLoadError };
			bRet = (bool)methodInfo.Invoke(null, paramArray);
		}
		catch (System.Exception ex)
		{
			BTDebug.ExceptionEx(ex);
			bRet = false;
		}
		if (bRet == true)
		{
			mLoadedGameDataList.Add(dataType);
		}
		return bRet;
	}

	/// <summary>
	/// 清除数据
	/// </summary>
	/// <param name="dataTypeList"></param>
	/// <returns></returns>
	public bool ClearData(List<Type> dataTypeList)
	{
		if (dataTypeList == null)
		{
			return false;
		}
		Int32 nSize = dataTypeList.Count;
		bool bErrorOccurred = false;
		for (Int32 i = 0; i < nSize; ++i)
		{
			if (ClearData(dataTypeList[i]) == false)
			{
				bErrorOccurred = true;
			}
		}
		return !bErrorOccurred;
	}

	/// <summary>
	/// 清除数据
	/// </summary>
	/// <param name="dataType"></param>
	/// <returns></returns>
	public bool ClearData(Type dataType)
	{
		if (mLoadedGameDataList == null)
		{
			return false;
		}
		if (mLoadedGameDataList.Contains(dataType) == false)
		{
			return false;
		}
		Type gameDataType = GetGameDataCollectionType(dataType.BaseType);
		if (gameDataType == null)
		{
			return false;
		}
		System.Reflection.MethodInfo methodInfo = gameDataType.GetMethod(
			cStrGameDataClearMethodName,
			System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

		if (methodInfo == null)
		{
			BTDebug.Warning(string.Format("DataType:{0} No {1} Method", dataType.Name, cStrGameDataClearMethodName), "DATA");
			return false;
		}

		bool bRet = false;
		try
		{
			bRet = (bool)methodInfo.Invoke(null, null);
		}
		catch (System.Exception ex)
		{
			BTDebug.ExceptionEx(ex);
			bRet = false;
		}
		if (bRet == true)
		{
			mLoadedGameDataList.Remove(dataType);
		}
		return bRet;
	}

	/// <summary>
	/// 获取数据层GameDataSingle或者GameDataMap基础类类定义
	/// </summary>
	/// <param name="typeGameData"></param>
	/// <returns></returns>
	protected Type GetGameDataCollectionType(Type typeGameData)
	{
		if (typeGameData == null)
		{
			return null;
		}
		if (typeGameData.IsGenericType == false)
		{
			return null;
		}
		Type generacTypeDefine = typeGameData.GetGenericTypeDefinition();
		if (generacTypeDefine == null)
		{
			return null;
		}
		bool bAssignFromSingle = generacTypeDefine == (typeof(GameDataSingle<,>));
		bool bAssignFromMap = generacTypeDefine == (typeof(GameDataMap<>));
		if (bAssignFromMap == false && bAssignFromSingle == false)
		{
			return GetGameDataCollectionType(typeGameData.BaseType);
		}
		return typeGameData;
	}

	////////////////////////加载与路径相关///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// 加载数据文件资源
	/// </summary>
	/// <param name="strKey"></param>
	/// <param name="handOnLoadComplete"></param>
	/// <returns></returns>
	public bool AsynLoadGameDataFile(string strKey, System.Action<string, byte[]> handOnLoadComplete)
	{
		if (string.IsNullOrEmpty(strKey) == true)
		{
			return false;
		}
		string strFilePath = GetDataFilePath(strKey);
		if (string.IsNullOrEmpty(strFilePath) == true)
		{
			return false;
		}

		// todo 支持多类型加载器
		if (File.Exists(strFilePath) == false)
		{
			return false;
		}
		byte[] dataArray = File.ReadAllBytes(strFilePath);
		if (handOnLoadComplete != null)
		{
			handOnLoadComplete(string.Empty, dataArray);
		}
		return true;
	}

	// 获取数据文件资源路径
	protected string GetDataFilePath(string strKey)
	{
		string strPath = string.Empty;
#if UNITY_EDITOR
		switch (RunningTimeSettings.SystemRunningType)
		{
			case TSysRunningType.enEditor:
				strPath = GetFilePathEditorMode(strKey);
				break;
			case TSysRunningType.enRelease:
				strPath = GetFilePathReleaseMode(strKey);
				break;
			default:
				break;
		}
#else
		strPath = GetFilePathReleaseMode(strKey);
#endif
		return strPath;
	}

	protected string GetFilePathReleaseMode(string strKey)
	{
		string strPath = string.Empty;
		return strPath;
	}
#if UNITY_EDITOR
	protected string GetFilePathEditorMode(string strKey)
	{
		string strPath = string.Empty;
		if (strKey == PackageSetting.sConfigTreeKey)
		{
			strPath = GetConfigTreePath();
			return strPath;
		}

		PBData_ResourceConfigInfo pbInfo = null;

		if (strKey == PackageSetting.sConfigTreeObjectKey ||
			strKey == PackageSetting.sConfigTreeUIKey||
			strKey == PackageSetting.sConfigTreeParticleKey ||
			strKey == PackageSetting.sConfigTreeAudioKey ||
			strKey == PackageSetting.sConfigTreeTextKey )
		{
			if (Data_ConfigTree.TryGetConfigInfo(strKey, out pbInfo) == true && pbInfo != null)
			{
				strPath = pbInfo.path;
			}
			return strPath;
		}

		if (Data_EditorTextList.TryGetFilePath(strKey, out strPath) == false)
		{
			return string.Empty;
		}
		return strPath;
	}
#endif

	// 获取config_tree路径
	protected string GetConfigTreePath()
	{
		string strPath = string.Empty;
#if UNITY_EDITOR
		switch (RunningTimeSettings.SystemRunningType)
		{
			case TSysRunningType.enEditor:
				{
					strPath = PackageSetting.sEditorConfigTreeName;
				}
				break;
			case TSysRunningType.enRelease:
				{
					string strBasePath = GameUtil.GetAssetsBasePath(TFilePathType.enStreamingAssets);
					strPath = string.Format("{0}{1}", strBasePath, PackageSetting.sReleaseConfigTreeName);
				}
				break;
			default:
				break;
		}
#else
		string strBasePath = GameUtil.GetAssetsBasePath(TFilePathType.enStreamingAssets);
		strPath = string.Format("{0}{1}", strBasePath, PackageSetting.sReleaseConfigTreeName);
#endif
		return strPath;
	}
}
