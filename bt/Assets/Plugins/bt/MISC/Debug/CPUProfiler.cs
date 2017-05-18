using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BTMISC
{
	public enum TCpuProfilerIndexType
	{
		enName = 1,
		enCallTime = 2,
		enTimeMax = 3,
		enTimeMin = 4,
		enTimeTotal = 5,
	}

	/// <summary>
	///  CPU时间统计分析
	/// </summary>
	public class CCpuProfiler
	{
		private string m_strName;
		private Int32 m_nCallTime;
		private TimeSpan m_timeMax;
		private TimeSpan m_timeMin;
		private TimeSpan m_timeTotal;
		private Stopwatch m_stopwatch;

		public string StrName { get { return m_strName; } }
		public Int32 NCallTime { get { return m_nCallTime; } }
		public TimeSpan TTimeMax { get { return m_timeMax; } }
		public TimeSpan TTimeMin { get { return m_timeMin; } }
		public TimeSpan TTimeTotal { get { return m_timeTotal; } }

		/// <summary>
		/// Constructor
		/// </summary>
		public CCpuProfiler()
		{
			m_nCallTime = 0;
			m_strName = string.Empty;
			m_timeMax = TimeSpan.Zero;
			m_timeMin = TimeSpan.MaxValue;
			m_stopwatch = new Stopwatch();
		}

		public void Clear()
		{
			m_nCallTime = 0;
			m_strName = string.Empty;
			m_timeMax = TimeSpan.Zero;
			m_timeMin = TimeSpan.MaxValue;
			m_stopwatch.Reset();
		}

		/// <summary>
		/// 设置名称
		/// </summary>
		/// <param name="strName"></param>
		public void SetName(string strName)
		{
			m_strName = strName;
		}

		/// <summary>
		/// 开始计时
		/// </summary>
		public void StartNewCall()
		{
			if (m_stopwatch.IsRunning == true)
			{
				BTDebug.Warning("Start A CPU Profiler While It is Running, Result will not be correct", "PROFILER");
			}
			m_stopwatch.Start();
			m_nCallTime += 1;
		}

		/// <summary>
		/// 结束计时
		/// </summary>
		public void EndCall()
		{
			m_stopwatch.Stop();
			TimeSpan timeSpan = m_stopwatch.Elapsed;
			if (timeSpan.CompareTo(m_timeMax) > 0)
			{
				m_timeMax = timeSpan;
			}
			if (timeSpan.CompareTo(m_timeMin) < 0)
			{
				m_timeMin = timeSpan;
			}
			m_timeTotal.Add(timeSpan);
			m_stopwatch.Reset();
		}

		/// <summary>
		/// 格式化输出
		/// </summary>
		/// <returns></returns>
		public string Format()
		{
			string strFormat = string.Empty;
			strFormat = string.Format("{0}\t{1}\t{2}\t{3}\t{5}", m_strName, m_nCallTime, m_timeMin.Milliseconds, m_timeMax.Milliseconds, m_timeTotal.Milliseconds);
			return strFormat;
		}

		/// <summary>
		/// 格式化输出一份分析表
		/// </summary>
		/// <param name="profilerList"></param>
		/// <param name="tSortType"></param>
		/// <returns></returns>
		public static string FormtTable(List<CCpuProfiler> profilerList, TCpuProfilerIndexType tSortType = TCpuProfilerIndexType.enTimeMax)
		{
			StringBuilder strBuilder = new StringBuilder();
			strBuilder.Append("Name\tCallTime\tMinTime\tMaxTime\tTotalTime\t\n");
			if (profilerList == null)
			{
				return strBuilder.ToString();
			}

			switch (tSortType)
			{
				case TCpuProfilerIndexType.enName:
					profilerList.Sort(CmpName);
					break;
				case TCpuProfilerIndexType.enTimeMax:
					profilerList.Sort(CmpMaxTime);
					break;
				case TCpuProfilerIndexType.enTimeMin:
					profilerList.Sort(CmpMinTime);
					break;
				case TCpuProfilerIndexType.enCallTime:
					profilerList.Sort(CmpCallTime);
					break;
				case TCpuProfilerIndexType.enTimeTotal:
					profilerList.Sort(CmpTotalTime);
					break;
				default:
					break;
			}
			Int32 nSize = profilerList.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				CCpuProfiler profiler = profilerList[i];
				if (profiler == null)
				{
					continue;
				}
				if (profiler.NCallTime <= 0)
				{
					continue;
				}
				strBuilder.AppendLine(profiler.Format());
			}
			return strBuilder.ToString();
		}

		private static Int32 CmpTotalTime(CCpuProfiler a, CCpuProfiler b)
		{
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			return a.TTimeTotal.CompareTo(b.TTimeTotal);
		}

		private static Int32 CmpMinTime(CCpuProfiler a, CCpuProfiler b)
		{
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			return a.TTimeMin.CompareTo(b.TTimeMin);
		}

		private static Int32 CmpMaxTime(CCpuProfiler a, CCpuProfiler b)
		{
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			return a.TTimeMax.CompareTo(b.TTimeMax);
		}

		private static Int32 CmpCallTime(CCpuProfiler a, CCpuProfiler b)
		{
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			return a.NCallTime.CompareTo(b.NCallTime);
		}

		private static Int32 CmpName(CCpuProfiler a, CCpuProfiler b)
		{
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			return a.StrName.CompareTo(b.StrName);
		}
	}
}
