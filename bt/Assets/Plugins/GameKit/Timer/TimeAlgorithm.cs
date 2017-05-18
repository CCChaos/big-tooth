using System;
using System.Collections.Generic;
using System.Text;

namespace NS_GAME.KIT
{
	/// <summary>
	/// Unity 时间类型
	/// </summary>
	public enum TUnityTimeType
	{
		enGameTime = 1,		 // 游戏时间，受Unity的TimeScale影响
		enPhysicalTime = 2,  // 物理时间，不受Unity的TimeScale影响
	}

	/// <summary>
	/// 游戏时间常用算法
	/// 基于Unity.Time
	/// </summary>
	public class CTimeAlgorithm
	{
		// 1970年1月1日
		private static DateTime sDateTime19700101 = new DateTime(1970, 1, 1);


		/// <summary>
		/// 获取时间
		/// </summary>
		/// <param name="nTimeSeconds"></param>
		/// <param name="rOutDays"></param>
		/// <param name="rOutHours"></param>
		/// <param name="rOutMinutes"></param>
		/// <param name="rOutSeconds"></param>
		public static void GetTime(Int32 nTimeSeconds, out Int32 rOutDays, out Int32 rOutHours, out Int32 rOutMinutes, out Int32 rOutSeconds)
		{
			rOutDays = 0;
			rOutHours = 0;
			rOutMinutes = 0;
			rOutSeconds = 0;
			if (nTimeSeconds <= 0)
			{
				return;
			}
			rOutDays = nTimeSeconds / (24 * 60 * 60);
			rOutHours = (nTimeSeconds % (24 * 60 * 60)) / (60 * 60);
			rOutMinutes = (nTimeSeconds % 60 * 60) / 60;
			rOutSeconds = nTimeSeconds % 60;
		}

		/// <summary>
		/// 获取日期
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string GetFormatDateString(DateTime dateTime)
		{
			string strDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
			return strDate;
		}

		/// <summary>
		/// 获取时间
		/// </summary>
		/// <param name="tTimeSeconds"></param>
		/// <returns></returns>
		public static string GetFormatTimeString(Int32 nTimeSeconds, string strLangStringDay = "day")
		{
			Int32 nDays = 0;
			Int32 nHours = 0;
			Int32 nMinutes = 0;
			Int32 nSeconds = 0;
			string strDate = string.Empty;
			GetTime(nTimeSeconds, out nDays, out nHours, out nMinutes, out nSeconds);
			if (nDays > 0)
			{
				strDate = string.Format("{0}{1}", nDays, strLangStringDay);
			}
			else
			{
				strDate = string.Format("{0:D2}:{1:D2}:{2:D2}", nHours, nMinutes, nSeconds);
			}
			return strDate;
		}

		/// <summary>
		/// 获取本地时间
		/// </summary>
		/// <param name="uTimeStamp"></param>
		/// <returns></returns>
		public static DateTime GetDataTimeFromTimeStamp(UInt32 uTimeStamp)
		{
			DateTime dataTime = sDateTime19700101.AddSeconds((double)uTimeStamp);
			dataTime = TimeZone.CurrentTimeZone.ToLocalTime(dataTime);
			return dataTime;
		}

		/// <summary>
		/// 获取时间戳
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static UInt32 GetTimeStamp(DateTime dateTime)
		{
			UInt32 timeNow = 0;
			timeNow = (UInt32)((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
			return timeNow;
		}

		/// <summary>
		/// 获取本地时间时间戳
		/// </summary>
		/// <returns></returns>
		public static UInt32 GetTimeNowStamp()
		{
			UInt32 timeNow = 0;
			timeNow = (UInt32)((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
			return timeNow;
		}

		/// <summary>
		/// 获取游戏时间
		/// </summary>
		/// <param name="tTimeType"></param>
		/// <returns></returns>
		public static float GetGameTimeNow(TUnityTimeType tTimeType)
		{
			float fTimeNow = 0;
			if (tTimeType == TUnityTimeType.enGameTime)
			{
				fTimeNow = UnityEngine.Time.time;
			}
			if (tTimeType == TUnityTimeType.enPhysicalTime)
			{
				fTimeNow = UnityEngine.Time.realtimeSinceStartup;
			}
			return fTimeNow;
		}

	}
}
