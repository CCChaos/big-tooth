using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using TDebugTag = System.String;

namespace BTMISC
{
	/// <summary>
	/// 日志系统
	/// </summary>
	public class BTDebug
	{
		public enum TDebugLevel
		{
			enLog = 1,
			enWarning = 2,
			enError = 4,
			enException = 8,
		}

		private static HandleObjectAction m_handleLog;
		private static HandleObjectAction m_handleWarning;
		private static HandleObjectAction m_handleError;
		private static HandleObjectAction m_handleException;
		private static bool m_bWithStackInfo = false;
		private static Int64 m_nIndex = 1;
		/// <summary>
		/// 注册重定向输出
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="tDebugType"></param>
		/// <returns></returns>
		public static bool RegisterHandle(HandleObjectAction handle, TDebugLevel tDebugType)
		{
			if (handle == null)
			{
				return false;
			}
			switch (tDebugType)
			{
				case TDebugLevel.enLog:
					{
						m_handleLog += handle;
					}
					break;
				case TDebugLevel.enWarning:
					{
						m_handleWarning += handle;
					}
					break;
				case TDebugLevel.enError:
					{
						m_handleError += handle;
					}
					break;
				case TDebugLevel.enException:
					{
						m_handleException += handle;
					}
					break;
				default:
					{
						return false;
					}
			}
			return true;
		}

		/// <summary>
		/// 取消重定向输出
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="tDebugType"></param>
		/// <returns></returns>
		public static bool UnRegisterHandle(HandleObjectAction handle, TDebugLevel tDebugType)
		{
			if (handle == null)
			{
				return false;
			}
			switch (tDebugType)
			{
				case TDebugLevel.enLog:
					{
						m_handleLog -= handle;
					}
					break;
				case TDebugLevel.enWarning:
					{
						m_handleWarning -= handle;
					}
					break;
				case TDebugLevel.enError:
					{
						m_handleError -= handle;
					}
					break;
				case TDebugLevel.enException:
					{
						m_handleException -= handle;
					}
					break;
				default:
					{
						return false;
					}
			}
			return true;
		}

		/// <summary>
		/// 日志
		/// </summary>
		/// <param name="logObject"></param>
		public static void Log(System.Object logObject, TDebugTag tag = "")
		{
			string strLog = FormatMessage(TDebugLevel.enLog, logObject, tag);
			if (m_handleLog == null)
			{
				DefaultLog(strLog);
				return;
			}
			m_handleLog(strLog);
		}

		/// <summary>
		/// 警告
		/// </summary>
		/// <param name="logObject"></param>
		public static void Warning(System.Object logObject, TDebugTag tag = "")
		{
			string strLog = FormatMessage(TDebugLevel.enWarning, logObject, tag);
			if (m_handleWarning == null)
			{
				DefaultLog(strLog);
				return;
			}
			m_handleWarning(strLog);
		}

		/// <summary>
		/// 错误
		/// </summary>
		/// <param name="logObject"></param>
		public static void Error(System.Object logObject, TDebugTag tag = "")
		{
			string strLog = FormatMessage(TDebugLevel.enError, logObject, tag);
			if (m_handleError == null)
			{
				DefaultLog(strLog);
				return;
			}
			m_handleError(strLog);
		}

		/// <summary>
		/// 异常
		/// </summary>
		/// <param name="logObject"></param>
		public static void Exception(System.Object logObject, TDebugTag tag = "")
		{
			string strLog = FormatMessage(TDebugLevel.enException, logObject, tag);
			if (m_handleException == null)
			{
				DefaultLog(strLog);
				return;
			}
			m_handleException(strLog);
		}

		/// <summary>
		/// 异常
		/// </summary>
		/// <param name="exception"></param>
		public static void ExceptionEx(Exception exception, TDebugTag tag = "")
		{
			if (exception == null)
			{
				return;
			}
			string strException = string.Format("<BTEXCEPTION> ExType:{0}, Message:{1}", exception.GetType(), exception.Message);
			Exception(strException, tag);
		}

		private static void DefaultLog(System.Object logObject)
		{
			if (logObject == null)
			{
				return;
			}
			Console.WriteLine(logObject.ToString());
		}

		private static string FormatMessage(TDebugLevel tLevel, System.Object logObject, TDebugTag tag = "")
		{
			string strDebugLevel = "[UNKNOWN]";
			switch (tLevel)
			{
				case TDebugLevel.enLog:
					strDebugLevel = "[I]";
					break;
				case TDebugLevel.enWarning:
					strDebugLevel = "[W]";
					break;
				case TDebugLevel.enError:
					strDebugLevel = "[E]";
					break;
				case TDebugLevel.enException:
					strDebugLevel = "[X]";
					break;
			}
			// Index Time Level Message
			string strFormatedMsg = string.Empty;
			if (string.IsNullOrEmpty(tag) == true)
			{
				strFormatedMsg = string.Format("{0}\t{1}\t{2}\t<NOTAG> {3}",
					m_nIndex++,
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"),
					strDebugLevel,
					logObject);
			}
			else
			{
				strFormatedMsg = string.Format("{0}\t{1}\t{2}\t<{3}> {4}",
					m_nIndex++,
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"),
					strDebugLevel,
					tag,
					logObject);
			}
			if (m_bWithStackInfo == true)
			{
				strFormatedMsg += "\n" + GetStacksInfo();
			}
			return strFormatedMsg;
		}

		private static string GetStacksInfo()
		{
			StringBuilder strBuilder = new StringBuilder(2048);
			strBuilder.Length = 0;
			StackFrame[] stackFrames = new StackTrace().GetFrames();
			Int32 nSize = stackFrames == null ? 0 : stackFrames.Length;
			for (Int32 i = 2; i < nSize; ++i )
			{
				StackFrame frame = stackFrames[i];
				AppendFormatedFrameInfo(frame, ref strBuilder);
			}
			return strBuilder.ToString();
		}


		private static string GetStackInfo()
		{
			StackTrace stackTrace = new StackTrace();
			MethodBase methodBase = stackTrace.GetFrame(2).GetMethod();
			return string.Format("{0}.{1}", methodBase.ReflectedType.Name, methodBase.Name);
		}

		private static bool AppendFormatedFrameInfo(StackFrame stackFrame, ref StringBuilder stringBuilder)
		{
			if (stackFrame == null || stringBuilder == null)
			{
				return false;
			}
			stringBuilder.AppendLine(stackFrame.ToString());
			return true;

		}
	}
}
