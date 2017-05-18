#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace NS_GAME.DATA
{
	/// <summary>
	/// Editor类型资源配置基类
	/// </summary>
	public class Data_EditorResourceConfigList<U> : GameDataSingle<PBData_EditorResourceInfoList, U>
		where U : GameDataSingle<PBData_EditorResourceInfoList, U>
	{
		/// <summary>
		/// 查找资源路径
		/// </summary>
		/// <param name="strKey"></param>
		/// <param name="rOutPath"></param>
		/// <returns></returns>
		public static bool TryGetFilePath(string strKey, out string rOutPath)
		{
			rOutPath = string.Empty;
			PBData_EditorResourceInfoList pbData = GetData();
			if (pbData == null || pbData.resouce_list == null)
			{
				return false;
			}
			Int32 nSize = pbData.resouce_list.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				PBData_EditorResourceInfo info = pbData.resouce_list[i];
				if (info == null)
				{
					continue;
				}
				if (info.key == strKey)
				{
					rOutPath = info.path;
					return true;
				}
			}
			return false;
		}
	}

	/// <summary>
	/// 数据文件资源配置
	/// </summary>
	public class Data_EditorTextList : Data_EditorResourceConfigList<Data_EditorTextList>
	{
		public readonly static string DataFileKey = PackageSetting.sConfigTreeTextKey;
	}

	/// <summary>
	/// 模型资源配置
	/// </summary>
	public class Data_EditorObjectList : Data_EditorResourceConfigList<Data_EditorObjectList>
	{
		public readonly static string DataFileKey = PackageSetting.sConfigTreeObjectKey;
	}
}

#endif