using System;
using System.Collections.Generic;
using BTMISC;


namespace NS_GAME.DATA
{
	public class Data_ConfigTree : GameDataSingle<PBData_ResourceConfigTree, Data_ConfigTree>
	{
		public readonly static string DataFileKey = PackageSetting.sConfigTreeKey;

		/// <summary>
		/// 更新配置信息
		/// </summary>
		/// <param name="newInfo"></param>
		/// <returns></returns>
		public static bool UpdateResourceConfigTree(PBData_ResourceConfigInfo newInfo)
		{
			PBData_ResourceConfigTree pbTree = GetData();
			if (pbTree == null)
			{
				return false;
			}
			if (newInfo == null)
			{
				return false;
			}

			if (pbTree.config_list == null)
			{
				pbTree.config_list = new List<PBData_ResourceConfigInfo>();
			}
			bool bUpdate = false;
			Int32 nSize = pbTree.config_list.Count;
			for (Int32 i = 0; i < nSize; ++i)
			{
				PBData_ResourceConfigInfo tmpInfo = pbTree.config_list[i];
				if (tmpInfo == null)
				{
					continue;
				}
				if (tmpInfo.key == newInfo.key)
				{
					pbTree.config_list[i] = newInfo;
					bUpdate = true;
				}
			}
			if (bUpdate == false)
			{
				pbTree.config_list.Add(newInfo);
			}
			return true;
		}

		/// <summary>
		/// 获取配置的资源配置路径
		/// </summary>
		/// <param name="strKey"></param>
		/// <param name="rOutPath"></param>
		/// <returns></returns>
		public static bool TryGetConfigInfo(string strKey, out PBData_ResourceConfigInfo rOutInfo)
		{
			rOutInfo = null;
			PBData_ResourceConfigTree pbData = GetData();
			if (pbData == null || pbData.config_list == null)
			{
				return false;
			}

			Int32 nSize = pbData.config_list.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				PBData_ResourceConfigInfo info = pbData.config_list[i];
				if (info == null)
				{
					continue;
				}
				if (info.key == strKey)
				{
					rOutInfo = info;
					return true;
				}
			}
			return false;
		}
	}

}

