using System;
using System.Collections;
using UnityEngine;

// 系统运行方式
public enum TSysRunningType
{
	enUnDefined = 0,		// 未指定
#if UNITY_EDITOR
	enEditor = 1,				// Debug 模式
#endif
	enRelease = 2,			// Release 模式
}

// 资源打包方式
public enum TResourceBuildType
{
	enAssetBuild = 1,  // assetbundle包打包
	enResourceBuild = 2, // Resource模式打包
#if UNITY_EDITOR
	enEditorBuild = 4, // Editor模式打包
#endif
}

// 游戏系统配置
public class PackageSetting
{
	// TextAsset资源 编码
	public readonly static System.Text.Encoding cTextFileEncoding = System.Text.Encoding.UTF8;
	
	// Asset资源包位置
	public readonly static string sAssetRootPath = "StreamingAssets";
#if UNITY_EDITOR
	public readonly static string sBuildAssetRootPath = "../";  // 打包的资源包位置
#endif

#if UNITY_ANDROID 
	public readonly static string sAssetBundlePath = "DataAndroid/";
#elif UNITY_IPHONE
	public readonly static string sAssetBundlePath = "DataiOS/";
#elif UNITY_STANDALONE_WIN
	public readonly static string sAssetBundlePath = "DataPC/";
#elif UNITY_WEBPLAYER
	public readonly static string sAssetBundlePath = "DataWEB/";
#elif UNITY_WP8
	public readonly static string PkgDataFolder = "";
#else
	public readonly static string sAssetBundlePath = "DataOther/";
#endif
#if UNITY_EDITOR
	public readonly static string sEditorConfigPath = "Assets/EditorResourceConfig/";
#endif

	// 配置树
	public readonly static string sConfigTreeKey = "config_tree";

#if UNITY_EDITOR
	public readonly static string sEditorConfigTreeName = sEditorConfigPath + sConfigTreeKey + ".xml"; // Editor模式config_tree位置 "Assets/EditorResourceConfig/config_tree.xml"
#endif
	public readonly static string sReleaseConfigTreeName = sAssetBundlePath + sConfigTreeKey + ".xml";  // Release模式config_tree位置 "[AssetBundlePath]/config_tree.xml";

	// AssetBundle包文件后缀
	public readonly static string sAssetBundleSuffix = ".ab";
	public readonly static string sConfigTreeUIKey = "gui";  // UI 资源
	public readonly static string sConfigTreeParticleKey = "fx"; // 特效资源
	public readonly static string sConfigTreeAudioKey = "audio"; // 音效资源
	public readonly static string sConfigTreeObjectKey = "object"; // 模型资源
	public readonly static string sConfigTreeTextKey = "data"; // 数据资源


	// 客户端配置
	public readonly static string sClientConfigKey = "client_config";

	// 用户自定义设置
	public readonly static string sCustomConfigPath = Application.persistentDataPath + "custom_config.xml";

}