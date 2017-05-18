using System;
using System.Collections.Generic;
using System.Text;

namespace BTFSM
{
	/// <summary>
	/// 状态机动作
	/// </summary>
	public class CFSMAction
	{
		// 动作名称
		private string m_StrActionName;
		// 动作参数
		private System.Object[] m_ParamArray;
		// 动作管道
		protected CFSMPipelineCollection m_PipelineCollection;

		/// <summary>
		/// Constructor
		/// </summary>
		public CFSMAction()
		{
			m_StrActionName = string.Empty;
			m_PipelineCollection = new CFSMPipelineCollection();
		}

		/// <summary>
		/// 设置动作
		/// </summary>
		/// <param name="strName"></param>
		/// <param name="paramArray"></param>
		/// <returns></returns>
		public bool SetAction(string strName, System.Object[] paramArray)
		{
			m_StrActionName = strName;
			m_ParamArray = paramArray;
			return true;
		}

		public bool CreateFromXML(CFSMXMLAction xmlAction)
		{
			if (xmlAction == null)
			{
				return false;
			}
			m_StrActionName = xmlAction.m_strName;
			//m_ParamArray = xmlAction.m_strParamList;
			m_PipelineCollection.AddConfigPipel(xmlAction.m_fsmEventList);
			m_PipelineCollection.AddConfigPipel(xmlAction.m_fsmConditionList);
			return true;
		}

		/// <summary>
		/// 条件符合时执行操作
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <param name="rOutReturnValue"></param>
		/// <returns>是否执行成功，若未执行依然返回true</returns>
		public bool InvokeWhenPipelineOpen(CFSM fsmEntity, out System.Object rOutReturnValue)
		{
			rOutReturnValue = default(System.Object);
			if (CheckAction(fsmEntity) == false)
			{
				return true;
			}
			return ForceInvoke(fsmEntity, out rOutReturnValue);
		}

		/// <summary>
		/// 执行动作
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <param name="rOutReturnValue"></param>
		/// <returns></returns>
		public bool ForceInvoke(CFSM fsmEntity, out System.Object rOutReturnValue)
		{
			rOutReturnValue = default(System.Object);
			if (fsmEntity == null)
			{
				return false;
			}
			if (fsmEntity.InvokeFSMAction(m_StrActionName, m_ParamArray, out rOutReturnValue) == false)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 增加条件
		/// </summary>
		/// <param name="pipeline"></param>
		/// <returns></returns>
		public bool AddPipel(CFSMPipeline pipeline)
		{
			if (m_PipelineCollection == null)
			{
				return false;
			}
			bool bRet = m_PipelineCollection.AddPipel(pipeline);
			return bRet;
		}

		/// <summary>
		/// 检查当前是否有管道畅通可以执行动作
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <returns></returns>
		public bool CheckAction(CFSM fsmEntity)
		{
			if (m_PipelineCollection == null)
			{
				return false;
			}
			bool bCheck = m_PipelineCollection.IsOpenToFSM(fsmEntity);
			return bCheck;
		}
	}
}
