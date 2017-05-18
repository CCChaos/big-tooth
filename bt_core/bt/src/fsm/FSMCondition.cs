using System;
using System.Collections.Generic;
using BTCOMMON;

namespace BTFSM
{
	/// <summary>
	/// 状态机条件触发器
	/// </summary>
	public class CFSMConditionTrigger
	{
		/// <summary>
		/// 比较条件
		/// </summary>
		protected class CFSMConditionCmp
		{
			private string m_strMemberName; // 属性名称
			private TCompareType m_cmpType; // 比较类型
			//private IComparable m_cmpValue; // 比较值
			private string m_cmpValue; // 比较值

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="strAttrName"></param>
			/// <param name="tCmpType"></param>
			/// <param name="objCmpValue"></param>
			public CFSMConditionCmp(string strMemberName, TCompareType tCmpType, string objCmpValue)
			{
				m_strMemberName = strMemberName;
				m_cmpType = tCmpType;
				m_cmpValue = objCmpValue;
			}

			/// <summary>
			/// 条件是否可用
			/// </summary>
			/// <returns></returns>
			public bool IsValid()
			{
				return true;
			}

			/// <summary>
			/// 条件是否成立
			/// </summary>
			/// <param name="objConditonValue"></param>
			/// <returns></returns>
			public bool IsFit(IComparable objConditonValue)
			{
				if (objConditonValue == null)
				{
					return false;
				}
				System.Object objCmpValue = null;
				if (CCompareAlgorithm.GetValueFromString(m_cmpValue, objConditonValue.GetType(), out objCmpValue) == false)
				{
					return false;
				}
				Int32 nCmpRet = objConditonValue.CompareTo(objCmpValue);
				bool bFit = CCompareAlgorithm.CmpResultMatch(nCmpRet, m_cmpType);
				return bFit;
			}

			/// <summary>
			/// 属性名称
			/// </summary>
			/// <returns></returns>
			public string GetMemberName()
			{
				return m_strMemberName;
			}
		}

		private List<CFSMConditionCmp> m_ConditionCmpList; // 比较条件列表

		/// <summary>
		/// Constructor
		/// </summary>
		public CFSMConditionTrigger()
		{
			m_ConditionCmpList = new List<CFSMConditionCmp>();
		}

		/// <summary>
		/// 是否条件符合
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <returns></returns>
		public bool IsFit(CFSM fsmEntity)
		{
			if (fsmEntity == null)
			{
				return false;
			}
			Int32 nSize = m_ConditionCmpList == null ? 0 : m_ConditionCmpList.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				CFSMConditionCmp conCmp = m_ConditionCmpList[i];
				if (conCmp == null)
				{
					continue;
				}
				string strMemberName = conCmp.GetMemberName();
				System.Object objCondition = null;
				if (fsmEntity.GetFSMCondition(strMemberName, out objCondition) == false || objCondition == null)
				{
					return false;
				}
				IComparable cmpObj = objCondition as IComparable;
				if (cmpObj == null)
				{
					return false;
				}
				bool bFit = conCmp.IsFit((IComparable)objCondition);
				if (bFit == false)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// 增加条件
		/// </summary>
		/// <param name="strMemberName"></param>
		/// <param name="tCmpType"></param>
		/// <param name="cmpValue"></param>
		/// <returns></returns>
		public bool AddConditionCmp(string strMemberName, TCompareType tCmpType, string cmpValue)
		{
			if (string.IsNullOrEmpty(strMemberName) == true)
			{
				return false;
			}
			CFSMConditionCmp conCmp = new CFSMConditionCmp(strMemberName, tCmpType, cmpValue);
			m_ConditionCmpList.Add(conCmp);
			return true;
		}
		
	}
}
