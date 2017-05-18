using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using BTMISC;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace NS_GAMEEDITOR
{
	public class GameEditorUtil
	{
		public static void ShowMessageBox(string strMessage, string strTitle = "", System.Action handleOnOk = null, System.Action handleOnCancel = null, string strBtnOkText = @"确定", string strBtnCancelText=@"取消")
		{
			bool bRet = EditorUtility.DisplayDialog(strTitle, strMessage, strBtnOkText, strBtnCancelText);
			if (handleOnOk != null && bRet)
			{
				handleOnOk();
			}
			if (handleOnCancel != null && !bRet)
			{
				handleOnCancel();
			}
		}

		/// <summary>
		/// 获取当前场景所有最顶层节点
		/// </summary>
		/// <returns></returns>
		public static List<GameObject> GetALLTopGameObjectInScene()
		{
			List<GameObject> objTopList = new List<GameObject>();
			Object[] objArray = Object.FindObjectsOfType(typeof(GameObject));
			Int32 nSize = objArray == null ? 0 : objArray.Length;
			for (Int32 i = 0; i < nSize; ++i )
			{
				GameObject go = objArray[i] as GameObject;
				if (go != null && go.transform.parent == null)
				{
					objTopList.Add(go);
				}
			}
			return objTopList;
		}

		/// <summary>
		/// 保存TextAsset资源
		/// Path以Assets开始
		/// 如果未发现资源则创建
		/// 如果已有资源则覆盖
		/// </summary>
		/// <param name="strFilePath"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static bool TextResourceSaveAtPath(string strFilePath, byte[] strSaveDataArray)
		{
			if (strSaveDataArray == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(strFilePath))
			{
				return false;
			}

			strFilePath = Util.NormalizeFilePath(strFilePath);
			FileStream fileStream = File.Open(strFilePath, FileMode.Create);
			if (fileStream == null)
			{
				return false;
			}
			fileStream.Write(strSaveDataArray, 0, strSaveDataArray.Length);
			fileStream.Close();
			return true;
		}

		/// <summary>
		/// 加载Assets下的TextAsset资源
		/// 未找到资源则返回null
		/// 只能查找Assets下的资源
		/// </summary>
		/// <param name="strFilePath"></param>
		/// <returns></returns>
		public static string TextResourceLoadAtPath(string strFilePath)
		{
			strFilePath = Util.NormalizeFilePath(strFilePath);
			if (File.Exists(strFilePath) == false)
			{
				return null;
			}
			FileStream fileStream = File.Open(strFilePath, FileMode.Open);
			if (fileStream == null)
			{
				return null;
			}
			if (fileStream.Length >= Int32.MaxValue)
			{
				UnityEngine.Debug.LogError(string.Format("File:{0} Too Large", strFilePath));
				fileStream.Close();
				return null;
			}
			if (fileStream.Length <= 0)
			{
				return string.Empty;
			}

			Int32 nLength = (Int32)fileStream.Length;
			byte[] data = new byte[nLength];
			fileStream.Read(data, 0, nLength);
			fileStream.Close();
			string strFileData = PackageSetting.cTextFileEncoding.GetString(data);
			return strFileData;
		}
	}
}

