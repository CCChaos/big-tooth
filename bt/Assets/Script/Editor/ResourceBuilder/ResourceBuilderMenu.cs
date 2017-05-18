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

public partial class ResourceBuilder
{
	#region Menu Option( 打包菜单 )
	private const string strMenuEditorBuild = "BT/EditorBuild/";
	private const string strMenuAssetBuild = "BT/AssetBuild/";
	private const string strMenuBuildObject = "BuildObject( 打包模型 角色怪物等 )";
	private const string strMenuBuildAudio = "BuildAudio( 打包音效 )";
	private const string strMenuBuildParticle = "BuildParticle( 打包特效 )";
	private const string strMenuBuildScene = "BuildScene( 打包场景 )";
	private const string strMenuBuildUI = "BuildUI( 打包UI )";
	private const string strMenuBuildText = "BuildConfig（打包配置）";
	#endregion
	#region Menu Entrance
	#region Editor Function
	/// <summary>
	/// 创建空的BuildAll文件
	/// </summary>
	[MenuItem(strMenuEditorBuild + strMenuBuildObject)]
	static void MenuBuildObjectEditor()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enEditor);
		bool bRet = BuildObjectEditor();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	[MenuItem(strMenuEditorBuild + strMenuBuildUI)]
	static void MenuBuildUIEditor()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enEditor);
		bool bRet = BuildUIEditor();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	[MenuItem(strMenuEditorBuild + strMenuBuildAudio)]
	static void MenuBuildAudioEditor()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enEditor);
		bool bRet = BuildAudioEditor();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	[MenuItem(strMenuEditorBuild + strMenuBuildParticle)]
	static void MenuBuildParticleEditor()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enEditor);
		bool bRet = BuildParticleEditor();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	[MenuItem(strMenuEditorBuild + strMenuBuildScene)]
	static void MenuBuildSceneEditor()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enEditor);
		bool bRet = BuildSceneEditor();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	[MenuItem(strMenuEditorBuild + strMenuBuildText)]
	static void MenuBuildTextEditor()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enEditor);
		bool bRet = BuildTextEditor();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	[MenuItem(strMenuEditorBuild + "CreateEmptyEditorConfigTree(生成Editor模式资源的config_tree.xml空文件)")]
	static void MenuCreateEmptyEditorConfigTree()
	{
		CreateEmptyEditorConfigTree();
	}
	[MenuItem("BT/EditorBuild/CreateEmptyBuildAll( 创建一个空的BuildAll.config文件 )")]
	static void MenuCreateEmptyBuildAll()
	{
		CreateEmptyBuildAll();
	}
	
	#endregion
	#region Asset Function
	[MenuItem(strMenuAssetBuild + strMenuBuildObject)]
	static void MenuBuildObjectAsset()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enRelease);
		bool bRet = BuildObjectAsset();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	[MenuItem(strMenuAssetBuild + strMenuBuildUI)]
	static void MenuBuildUIAsset()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enRelease);
		bool bRet = BuildUIAsset();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	[MenuItem(strMenuAssetBuild + strMenuBuildAudio)]
	static void MenuBuildAudioAsset()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enRelease);
		bool bRet = BuildAudioAsset();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	[MenuItem(strMenuAssetBuild + strMenuBuildParticle)]
	static void MenuBuildParticleAsset()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enRelease);
		bool bRet = BuildParticleAsset();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	[MenuItem(strMenuAssetBuild + strMenuBuildScene)]
	static void MenuBuildSceneAsset()
	{
		SetBuilderRunningTimeSettings(TSysRunningType.enRelease);
		bool bRet = BuildSceneAsset();
		GameEditorUtil.ShowMessageBox(bRet ? "成功" : "失败");
	}
	#endregion
	#endregion
}
