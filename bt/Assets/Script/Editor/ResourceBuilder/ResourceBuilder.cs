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

/// <summary>
/// 资源打包器
/// 1. BuildAll.xml  配置各个类型资源打包的场景
/// 2. config_tree.xml 各个类型资源配置的信息
/// </summary>
public partial class ResourceBuilder
{
	#region Members
	#region BuildAll Config (配置打包的场景)
	// 配置路径
	private const string sStrBuildSceneConfig = "Assets/Arts/BuildAll.config";
	private static CBuildAllConfig sBuildAllConfig;

	// 配置结构
	[System.Xml.Serialization.XmlType("ConfigRoot")]
	public class CBuildAllConfig
	{
		[System.Xml.Serialization.XmlElement("Scene")]
		public List<CBuildAllScene> SceneList;
		[System.Xml.Serialization.XmlElement("UI")]
		public List<CBuildAllUI> UIList;
		[System.Xml.Serialization.XmlElement("Object")]
		public List<CBuildAllObject> ObjectList;
		[System.Xml.Serialization.XmlElement("Particle")]
		public List<CBuildAllParticle> ParticleList;
		[System.Xml.Serialization.XmlElement("Audio")]
		public List<CBuildAllAudio> AudioList;
	}

	[System.Xml.Serialization.XmlType("Scene")]
	public class CBuildAllScene
	{
		[System.Xml.Serialization.XmlAttribute("Path")]
		public string Path;
	}

	[System.Xml.Serialization.XmlType("UI")]
	public class CBuildAllUI
	{
		[System.Xml.Serialization.XmlAttribute("Path")]
		public string Path;
	}

	[System.Xml.Serialization.XmlType("Object")]
	public class CBuildAllObject
	{
		[System.Xml.Serialization.XmlAttribute("Path")]
		public string Path;
	}

	[System.Xml.Serialization.XmlType("Particle")]
	public class CBuildAllParticle
	{
		[System.Xml.Serialization.XmlAttribute("Path")]
		public string Path;
	}

	[System.Xml.Serialization.XmlType("Audio")]
	public class CBuildAllAudio
	{
		[System.Xml.Serialization.XmlAttribute("Path")]
		public string Path;
	}


	#endregion
	#endregion
	#region Builder
	/// <summary>
	/// 设置打包运行时设置
	/// </summary>
	private static void SetBuilderRunningTimeSettings(TSysRunningType tType)
	{
		RunningTimeSettings.SystemRunningType = tType;
		sBuildAllConfig = GetBuildConfigAll();
		switch (tType)
		{
			case TSysRunningType.enEditor:
				{

				}
				break;
			case TSysRunningType.enRelease:
				{

				}
				break;
		}

	}
	#region Editor
	/// <summary>
	/// 创建空的BuildAll文件
	/// </summary>
	static void CreateEmptyEditorConfigTree()
	{
		PBData_ResourceConfigTree pbData = new PBData_ResourceConfigTree();
		pbData.config_list = new global::System.Collections.Generic.List<PBData_ResourceConfigInfo>();
		string strFilePath = PackageSetting.sEditorConfigTreeName;
		if (File.Exists(strFilePath) == true)
		{
			GameEditorUtil.ShowMessageBox(" config_tree.xml文件已经存在，继续生成会覆盖原文件，要继续吗？ ", "警告", 
				() =>
				{
					GameUtil.SaveToXmlFile(pbData, strFilePath);
					GameEditorUtil.ShowMessageBox("生成成功");
				});
			return;
		}
		GameUtil.SaveToXmlFile(pbData, strFilePath);
		GameEditorUtil.ShowMessageBox("生成成功");
	}
	static void CreateEmptyBuildAll()
	{
		CBuildAllConfig config = new CBuildAllConfig();
		config.AudioList = new List<CBuildAllAudio>();
		config.ObjectList = new List<CBuildAllObject>();
		config.ParticleList = new List<CBuildAllParticle>();
		config.SceneList = new List<CBuildAllScene>();
		config.UIList = new List<CBuildAllUI>();
		if (File.Exists(sStrBuildSceneConfig) == true)
		{
			NS_GAMEEDITOR.GameEditorUtil.ShowMessageBox(
				"配置已经存在，重新生成会覆盖原来的配置，继续生成吗？", "警告",
				() =>
				{
					GameUtil.SaveToXmlFile(config, sStrBuildSceneConfig);
					NS_GAMEEDITOR.GameEditorUtil.ShowMessageBox("生成成功");
				},
				null);
			return;
		}
		GameUtil.SaveToXmlFile(config, sStrBuildSceneConfig);
		NS_GAMEEDITOR.GameEditorUtil.ShowMessageBox("生成成功");
	}
	static bool BuildObjectEditor()
	{
		// 获取BuildAll.config
		if (sBuildAllConfig == null)
		{
			UnityEngine.Debug.LogError("Load BuildAll.config Failed");
			return false;
		}

		// 获取配置树, 如果没有则增加
		PBData_ResourceConfigTree configTree = GetBuildConfigTree(TResourceBuildType.enEditorBuild);
		if (configTree == null)
		{
			UnityEngine.Debug.LogError("Get Config Tree Failed");
			return false;
		}
		PBData_ResourceConfigInfo objectConfigInfo = ResourceConfigTree.GetConfigTreeLeaf(PackageSetting.sConfigTreeObjectKey, configTree);
		if (objectConfigInfo == null)
		{
			objectConfigInfo = new PBData_ResourceConfigInfo();
			objectConfigInfo.key = PackageSetting.sConfigTreeObjectKey;
			objectConfigInfo.path = GetConfigTreeLeafPath(TResourceBuildType.enEditorBuild, PackageSetting.sConfigTreeObjectKey);
			ResourceConfigTree.AddOrUpdateResourceConfigTree(objectConfigInfo, ref configTree);
		}

		// 读取Object资源已有配置
		PBData_EditorResourceInfoList resourceInfoList = null;
		string strXML = GameEditorUtil.TextResourceLoadAtPath(objectConfigInfo.path);
		if (string.IsNullOrEmpty(strXML) == false)
		{
			resourceInfoList = GameUtil.ReadFromXmlString<PBData_EditorResourceInfoList>(strXML);
		}
		// 单个打包还是所有打包
		GameObject selectObj = Selection.activeGameObject;
		bool bBuildRet = false;
		if (selectObj == null)
		{
			bBuildRet = ObjectResourceBuilder.BuildEditorObject(sBuildAllConfig.ObjectList, ref resourceInfoList);
		}
		else
		{
			bBuildRet = ObjectResourceBuilder.BuildEditorObject(selectObj, ref resourceInfoList);
		}
		if (bBuildRet == false)
		{
			UnityEngine.Debug.LogError("Build Object Failed");
			return false;
		}
		
		GameUtil.SaveToXmlFile(resourceInfoList, objectConfigInfo.path);
		SaveConfigTree(TResourceBuildType.enEditorBuild, configTree);

		return bBuildRet;
	}
	static bool BuildUIEditor()
	{
		return true;
	}
	static bool BuildAudioEditor()
	{
		return true;
	}
	static bool BuildParticleEditor()
	{
		return true;
	}
	static bool BuildSceneEditor()
	{
		return true;
	}
	static bool BuildTextEditor()
	{
		// 获取BuildAll.config
		if (sBuildAllConfig == null)
		{
			UnityEngine.Debug.LogError("Load BuildAll.config Failed");
			return false;
		}

		// 获取配置树, 如果没有则增加
		PBData_ResourceConfigTree configTree = GetBuildConfigTree(TResourceBuildType.enEditorBuild);
		if (configTree == null)
		{
			UnityEngine.Debug.LogError("Get Config Tree Failed");
			return false;
		}
		PBData_ResourceConfigInfo textConfigInfo = ResourceConfigTree.GetConfigTreeLeaf(PackageSetting.sConfigTreeTextKey, configTree);
		if (textConfigInfo == null)
		{
			textConfigInfo = new PBData_ResourceConfigInfo();
			textConfigInfo.key = PackageSetting.sConfigTreeTextKey;
			textConfigInfo.path = GetConfigTreeLeafPath(TResourceBuildType.enEditorBuild, PackageSetting.sConfigTreeTextKey);
			ResourceConfigTree.AddOrUpdateResourceConfigTree(textConfigInfo, ref configTree);
		}
		// todo build Text
		PBData_EditorResourceInfoList resourceInfoList = new PBData_EditorResourceInfoList();

		GameUtil.SaveToXmlFile(resourceInfoList, textConfigInfo.path);
		SaveConfigTree(TResourceBuildType.enEditorBuild, configTree);
		
		return true;
	}
	#endregion
	#region Asset
	static bool BuildObjectAsset()
	{
		return true;
	}
	static bool BuildUIAsset()
	{
		return true;
	}
	static bool BuildAudioAsset()
	{
		return true;
	}
	static bool BuildParticleAsset()
	{
		return true;
	}
	static bool BuildSceneAsset()
	{
		return true;
	}
	static bool BuildTextAsset()
	{
		return true;
	}
	#endregion
	#endregion
	#region Methods
	/// <summary>
	/// 获取配置树
	/// </summary>
	/// <param name="tType"></param>
	/// <returns></returns>
	static PBData_ResourceConfigTree GetBuildConfigTree(TResourceBuildType tType)
	{
		PBData_ResourceConfigTree configTree = null;
		switch (tType)
		{
			case TResourceBuildType.enAssetBuild:
				break;
			case TResourceBuildType.enEditorBuild:
				{
					string strConfigXML = GameEditorUtil.TextResourceLoadAtPath(PackageSetting.sEditorConfigTreeName);
					if (string.IsNullOrEmpty(strConfigXML) == false)
					{
						configTree = GameUtil.ReadFromXmlString<PBData_ResourceConfigTree>(strConfigXML);
					}
				}
				break;
			case TResourceBuildType.enResourceBuild:
				break;
			default:
				break;
		}
		return configTree;
	}

	/// <summary>
	/// 保存配置树
	/// </summary>
	/// <param name="tType"></param>
	/// <param name="pbData"></param>
	/// <returns></returns>
	static bool SaveConfigTree(TResourceBuildType tType, PBData_ResourceConfigTree pbData)
	{
		bool bRet = false;
		switch (tType)
		{
			case TResourceBuildType.enAssetBuild:
				{

				}
				break;
			case TResourceBuildType.enEditorBuild:
				{
					string strXML = GameUtil.WriteToXmlString(pbData);
					byte[] dataArray = PackageSetting.cTextFileEncoding.GetBytes(strXML);
					bRet = GameEditorUtil.TextResourceSaveAtPath(PackageSetting.sEditorConfigTreeName, dataArray);
				}
				break;
			case TResourceBuildType.enResourceBuild:
				{

				}
				break;
		}
		return bRet;
	}

	// 获取各个类型配置的文件位置
	static string GetConfigTreeLeafPath(TResourceBuildType tType, string strConfigTreeLeafKey)
	{
		string strPath = string.Empty;
		switch (tType)
		{
			case TResourceBuildType.enAssetBuild:
				{

				}
				break;
			case TResourceBuildType.enEditorBuild:
				{
					strPath = PackageSetting.sEditorConfigPath + strConfigTreeLeafKey + ".xml";
				}
				break;
			case TResourceBuildType.enResourceBuild:
				{

				}
				break;
		}

		return strPath;
	}

	// 获取打包场景配置
	static CBuildAllConfig GetBuildConfigAll()
	{
		if (File.Exists(sStrBuildSceneConfig) == false)
		{
			return null;
		}
		string strConfig = GameEditorUtil.TextResourceLoadAtPath(sStrBuildSceneConfig);
		if (string.IsNullOrEmpty(strConfig))
		{
			return null;
		}
		CBuildAllConfig buildConfig = GameUtil.ReadFromXmlString<CBuildAllConfig>(strConfig);
		return buildConfig;
	}

	#endregion
}

