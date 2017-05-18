using System;

namespace BTCOMMON
{
	public enum TCompareType
	{
		enLT = 1,						// <
		enLE = enLT | enEQ,		// <=
		enEQ = 2,						// ==
		enGE = enGT | enEQ,		// >=
		enGT = 4,						// >
	}

	/// <summary>
	///  比较算法
	/// </summary>
	public class CCompareAlgorithm
	{
		/// <summary>
		/// 获取字符对应的比较类型
		/// </summary>
		/// <param name="strCmpType"></param>
		/// <returns></returns>
		public static TCompareType GetCompareType(string strCmpType)
		{
			if (strCmpType == "<" || strCmpType == "LT")
			{
				return TCompareType.enLT;
			}
			if (strCmpType == "<=" || strCmpType == "LE")
			{
				return TCompareType.enLE;
			}
			if (strCmpType == "=" || strCmpType == "EQ")
			{
				return TCompareType.enEQ;
			}
			if (strCmpType == ">=" || strCmpType == "GE")
			{
				return TCompareType.enGE;
			}
			if (strCmpType == ">" || strCmpType == "GT")
			{
				return TCompareType.enGT;
			}
			return TCompareType.enEQ;
		}

		/// <summary>
		/// 比较结果与比较类型是否匹配
		/// </summary>
		/// <param name="nCmpRet"></param>
		/// <param name="tCmpType"></param>
		/// <returns></returns>
		public static bool CmpResultMatch(Int32 nCmpRet, TCompareType tCmpType)
		{
			bool bMatch = false;
			if (nCmpRet < 0)
			{
				bMatch = (tCmpType & TCompareType.enLT) != 0;
			}
			else if (nCmpRet == 0)
			{
				bMatch = (tCmpType & TCompareType.enEQ) != 0;
			}
			else
			{
				bMatch = (tCmpType & TCompareType.enGT) != 0;
			}
			return bMatch;
		}

		/// <summary>
		/// 装换字符串到相应类型数据
		/// </summary>
		/// <param name="strValue"></param>
		/// <param name="tValueType"></param>
		/// <param name="rOutObjValue"></param>
		/// <returns></returns>
		public static bool GetValueFromString(string strValue, Type tValueType, out System.Object rOutObjValue)
		{
			rOutObjValue = default(System.Object);
			if (string.IsNullOrEmpty(strValue) == true)
			{
				return false;
			}
			if (tValueType == null)
			{
				return false;
			}
			bool bRet = false;

			if (tValueType == typeof(string))
			{
				rOutObjValue = strValue;
				bRet = true;
			}
			if (tValueType == typeof(int))
			{
				rOutObjValue = Convert.ToInt32(Convert.ToDouble(strValue));
				bRet = true;
			}
			if (tValueType == typeof(float))
			{
				rOutObjValue = float.Parse(strValue);
				bRet = true;
			}
			if (tValueType == typeof(byte))
			{
				rOutObjValue = Convert.ToByte(Convert.ToDouble(strValue));
				bRet = true;
			}
			if (tValueType == typeof(sbyte))
			{
				rOutObjValue = Convert.ToSByte(Convert.ToDouble(strValue));
				bRet = true;
			}
			if (tValueType == typeof(uint))
			{
				rOutObjValue = Convert.ToUInt32(Convert.ToDouble(strValue));
			}
			if (tValueType == typeof(short))
			{
				rOutObjValue = Convert.ToInt16(Convert.ToDouble(strValue));
				bRet = true;
			}
			if (tValueType == typeof(long))
			{
				rOutObjValue = Convert.ToInt64(Convert.ToDouble(strValue));
				bRet = true;
			}
			if (tValueType == typeof(ushort))
			{
				rOutObjValue = Convert.ToUInt16(Convert.ToDouble(strValue));
				bRet = true;
			}
			if (tValueType == typeof(ulong))
			{
				rOutObjValue = Convert.ToUInt64(Convert.ToDouble(strValue));
				bRet = true;
			}
			if (tValueType == typeof(double))
			{
				rOutObjValue = double.Parse(strValue);
				bRet = true;
			}
			if (tValueType == typeof(bool))
			{
				if (strValue == "0")
				{
					rOutObjValue = false;
				}
				rOutObjValue = ((strValue == "1") || bool.Parse(strValue));
				bRet = true;
			}
			return bRet;
		}
	}

}
