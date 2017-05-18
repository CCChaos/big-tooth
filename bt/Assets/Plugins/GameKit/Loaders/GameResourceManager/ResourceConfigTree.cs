using UnityEngine;
using System.Collections;
using NS_GAME.DATA;
using System;
using System.Collections.Generic;

// 配置树
public class ResourceConfigTree
{
	public static bool AddOrUpdateResourceConfigTree(PBData_ResourceConfigInfo info, ref PBData_ResourceConfigTree rConfigTree)
	{
		if (rConfigTree == null || info == null)
		{
			return false;
		}
		if (rConfigTree.config_list == null)
		{
			rConfigTree.config_list = new List<PBData_ResourceConfigInfo>();
		}
		bool bUpdate = false;
		Int32 nSize = rConfigTree.config_list.Count;
		for (Int32 i = 0; i < nSize; ++i )
		{
			PBData_ResourceConfigInfo tmpInfo = rConfigTree.config_list[i];
			if (tmpInfo == null)
			{
				continue;
			}
			if (tmpInfo.key == info.key)
			{
				rConfigTree.config_list[i] = info;
				bUpdate = true;
			}
		}
		if (bUpdate == false)
		{
			rConfigTree.config_list.Add(info);
		}
		return true;
	}

	public static PBData_ResourceConfigInfo GetConfigTreeLeaf(string strKey, PBData_ResourceConfigTree rConfigTree)
	{
		if (rConfigTree == null)
		{
			return null;
		}
		if (rConfigTree.config_list == null)
		{
			return null;
		}
		Int32 nSize = rConfigTree.config_list.Count;
		for (Int32 i = 0; i < nSize; ++i)
		{
			PBData_ResourceConfigInfo tmpInfo = rConfigTree.config_list[i];
			if (tmpInfo == null)
			{
				continue;
			}
			if (tmpInfo.key == strKey)
			{
				return tmpInfo;
			}
		}
		return null;
	}
}