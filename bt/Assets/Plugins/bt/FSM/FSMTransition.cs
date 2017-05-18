using System;
using System.Collections.Generic;

namespace BTFSM
{
	/// <summary>
	/// 状态机状态转换
	/// </summary>
	public class CFSMTransition
	{
		// 目标状态ID
		protected UInt32 m_uTargetStateId;
		// 转换管道
		protected CFSMPipelineCollection m_PipelineCollection;

		public CFSMTransition()
		{
			m_uTargetStateId = CFSMState.cuInvalieStateId;
			m_PipelineCollection = new CFSMPipelineCollection();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="uTargetStateId"></param>
		public CFSMTransition(UInt32 uTargetStateId)
		{
			m_uTargetStateId = uTargetStateId;
			m_PipelineCollection = new CFSMPipelineCollection();
		}

		/// <summary>
		/// 从XML配置创建
		/// </summary>
		/// <param name="xmlTrans"></param>
		/// <returns></returns>
		public bool CreateFromXML(CFSMXMLTranslation xmlTrans)
		{
			if (xmlTrans == null)
			{
				return false;
			}
			m_uTargetStateId = xmlTrans.m_uTargetId;
			m_PipelineCollection.AddConfigPipel(xmlTrans.m_fsmEventList);
			m_PipelineCollection.AddConfigPipel(xmlTrans.m_fsmConditionList);
			return true;
		}

		/// <summary>
		/// 获取目标状态ID
		/// </summary>
		/// <returns></returns>
		public UInt32 GetTargetStateId()
		{
			return m_uTargetStateId;
		}
		
		/// <summary>
		/// 增加一条转换管道
		/// </summary>
		/// <param name="condition"></param>
		/// <returns></returns>
		public bool AddPipel(CFSMPipeline pipeline)
		{
			if (pipeline == null)
			{
				return false;
			}
			if (m_PipelineCollection == null)
			{
				return false;
			}
			bool bRet = m_PipelineCollection.AddPipel(pipeline);
			return bRet;
		}

		/// <summary>
		/// 检查当前是否有管道畅通可以进行状态转换
		/// </summary>
		/// <param name="fsmEntity"></param>
		/// <returns></returns>
		public bool CheckTransition(CFSM fsmEntity)
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
