/********************************************************************	
*	file name:			GameData
*	file auth:			T3-CC.Chaos
*	
*	purpose:      		数据逻辑封装类
 *							管理逻辑数据并做获取接口
 *							可以对原始数据进行加工以达到逻辑层更加方便读取的目的
*********************************************************************/

using System;
using System.Collections.Generic;
using NS_GAME.KIT;
using System.IO;
using TGameDataID = System.UInt32;
using BTMISC;
using System.Reflection;

namespace NS_GAME.DATA
{
	// 数据封装基础类
	public class GameData<T>
		where T : class, new()
	{
		private T m_Data;

		public T data
		{
			get
			{
				return m_Data;
			}

			protected set
			{
				m_Data = value;
			}
		}

		public bool ReadFromXML(string strXMLString)
		{
			if (string.IsNullOrEmpty(strXMLString) == true)
			{
				return false;
			}
			m_Data = GameUtil.ReadFromXmlString<T>(strXMLString);
			return true;
		}

		public bool SerialFromByteData(TGameDataID uID, byte[] byteData)
		{
			m_Data = GameUtil.ReadPBFromData<T>(byteData);
			return false;
		}

		public void Clear()
		{
			m_Data = null;
		}
	}

	// 单配置封装基础类
	// T: PB数据类
	// U: 自定义数据操作类
	public class GameDataSingle<T, U>
		where T : class , new()
		where U : GameDataSingle<T, U>
	{
		// 数据
		private static GameData<T> m_DataSingleton;
		// 子类中数据文件Key的类定义成员名称（子类需要一个public只读成员）
		protected readonly static string strFieldNameFileKey = "DataFileKey";
		protected readonly static string strMethodNameOnLoadSuccess = "LoadDependences";
		/// <summary>
		/// Constructor
		/// </summary>
		protected GameDataSingle()
		{
		}

		protected static string GetDataFileKey()
		{
			var fileInfo = typeof(U).GetField(strFieldNameFileKey);
			if (fileInfo == null)
			{
				return string.Empty;
			}
			string strFileKye = string.Empty;
			try
			{
				strFileKye = fileInfo.GetValue(null) as string;
			}
			catch (System.Exception ex)
			{
				BTDebug.ExceptionEx(ex);
				strFileKye = string.Empty;
			}
			return strFileKye;
		}

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns></returns>
		public static T GetData()
		{
			if (m_DataSingleton == null)
			{
				return null;
			}
			T data = m_DataSingleton.data;
			return data;
		}

		// 加载 由GameDataManager管理
		private static bool Load(Action handleOnLoadComplete, Action<string> handleOnLoadError)
		{
			Action<string> tmpActionOnError = (strInfo) =>
			{
				if (handleOnLoadError != null)
				{
					handleOnLoadError(strInfo);
				}
			};

			if (m_DataSingleton != null) // 重复加载
			{
				string strErrorInfo = string.Format("Duplicate Load:{0}", typeof(U));
				BTDebug.Warning(strErrorInfo, "DATA");
				tmpActionOnError(strErrorInfo);
				return false;
			}
			m_DataSingleton = new GameData<T>();
			string strDataFileKey = GetDataFileKey();
			if (string.IsNullOrEmpty(strDataFileKey) == true)
			{
				string strErrorInfo = string.Format("GameData:{0} Has No DataFileKey", typeof(U).ToString());
				BTDebug.Exception(strErrorInfo, "DATA");
				tmpActionOnError(strErrorInfo);
				return false;
			}

			if (GameDataManager.Instance.AsynLoadGameDataFile(strDataFileKey, (errorInfo, dataArray) =>
				{
					if (string.IsNullOrEmpty(errorInfo) == false)
					{
						BTDebug.Exception(string.Format("GameData:{0} Load File Error:{1}", typeof(U), errorInfo), "DATA");
						tmpActionOnError(errorInfo);
						return;
					}
					if (m_DataSingleton.ReadFromXML(PackageSetting.cTextFileEncoding.GetString(dataArray)) == false)
					{
						string info = string.Format("GameData:{0} Parse Failed", typeof(U).ToString());
						BTDebug.Exception(info, "DATA");
						tmpActionOnError(errorInfo);
						return;
					}
					if (handleOnLoadComplete != null)
					{
						handleOnLoadComplete();
					}
				}
				) == false)
			{
				string strInfo = string.Format("GameData:{0} Load File Failed", typeof(U).ToString());
				BTDebug.Exception(strInfo, "DATA");
				tmpActionOnError(strInfo);
				return false;
			}

			return true;
		}

		/// <summary>
		/// 加载完成
		/// </summary>
		//protected static void OnLoadSuccess()
		//{
		//    Type typeData = typeof(U);
		//    if (typeData == null)
		//    {
		//        return;
		//    }
		//    MethodInfo methodLoadSuccess = typeData.GetMethod(strMethodNameOnLoadSuccess, BindingFlags.Static | BindingFlags.NonPublic);
		//    if (methodLoadSuccess == null)
		//    {
		//        BTDebug.Log(string.Format("{0} Has No Dependence", typeof(U).Name), "DATA");
		//        return;
		//    }
		//    try
		//    {
		//        methodLoadSuccess.Invoke(null, null);
		//    }
		//    catch (System.Exception ex)
		//    {
		//        BTDebug.Error(string.Format("{0} Load Dependence Error, See Next Log For Details", typeof(U).Name), "DATA");
		//        BTDebug.ExceptionEx(ex);
		//    }
		//}

		/// <summary>
		/// 清空
		/// </summary>
		public void Clear()
		{
			if (m_DataSingleton != null)
			{
				m_DataSingleton.Clear();
			}
			m_DataSingleton = null;
		}

#if UNITY_EDITOR
		public static string DumpData()
		{
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
			strBuilder.AppendFormat("======>Dump Data:{0}<======\n", typeof(T));
			if (m_DataSingleton == null || m_DataSingleton.data == null)
			{
				strBuilder.Append("<No Data>");
				return strBuilder.ToString();
			}
			List<string> dumpList = Util.DumpInstance(m_DataSingleton.data, typeof(U).Name);
			foreach (string dumpInfo in dumpList)
			{
				strBuilder.Append(dumpInfo);
			}
			return strBuilder.ToString();
		}
#endif
	}

	// 数据集封装基础类
	public abstract class GameDataMap<T>
		where T : class, new()
	{
		private Dictionary<TGameDataID, GameData<T>> m_DataMap;

		public T GetData(TGameDataID id)
		{
			if (m_DataMap == null)
			{
				return null;
			}
			GameData<T> gameData = null;
			if (m_DataMap.TryGetValue(id, out gameData) == false)
			{
				return null;
			}
			T data = gameData == null ? null : gameData.data;
			return data;
		}

		public bool AddData(TGameDataID id, GameData<T> data)
		{
			if (m_DataMap == null)
			{
				m_DataMap = new Dictionary<TGameDataID, GameData<T>>();
			}
			if (m_DataMap.ContainsKey(id) == true)
			{
				return false;
			}
			m_DataMap.Add(id, data);
			return true;
		}

		// 索引文件Key
		public abstract string GetIndexFileKey();
		// 数据文件Key
		public abstract string GetDataFileKey();
	}
}