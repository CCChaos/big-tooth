using UnityEngine;
using System.Collections;
using BTMISC;
using System.Xml.Serialization;
using System.IO;
using System;

public enum TFilePathType
{
	enStreamingAssets = 1,		// 安装包内
	enLocalWriteable = 2,			// 本地可写
	enWWW = 3,						// 网络资源
}

namespace NS_GAME.KIT
{
	/// <summary>
	/// 以Unity为依赖的杂项集合
	/// </summary>
	public partial class GameUtil
	{
		/// <summary>
		/// 获取资源上层路径
		/// </summary>
		/// <param name="bStreamingAssets">在StreamingAsset中</param>
		/// <returns></returns>
		public static string GetAssetsBasePath(TFilePathType tType)
		{
			string strPath = string.Empty;
			switch (tType)
			{
				case TFilePathType.enStreamingAssets:
					{
#if UNITY_EDITOR
						strPath = string.Format("file://{0}/../", Application.dataPath);
#elif UNITY_ANDROID
						strPath = string.Format("jar:file://{0}!/assets/",Application.dataPath);
#elif UNITY_IPHONE
						strPath = string.Format("file://{0}/Raw/", Application.dataPath);
#elif UNITY_STANDALONE_WIN
						strPath = string.Format("file://{0}/../", Application.dataPath);
#else
						strPath = "";
#endif
					}
					break;
				default:
					{

					}
					break;
			}

			return strPath;
		}
	}
}
