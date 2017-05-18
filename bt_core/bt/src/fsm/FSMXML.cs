using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using BTCOMMON;

namespace BTFSM
{
	[Serializable]
	[XmlRoot("fsm")]
	public class CFSMXMLEntity
	{
		[XmlAttribute("fsm_name")]
		public string m_strFSMName;

		[XmlElement("state")]
		public List<CFSMXMLState> m_FSMStateList;

		[XmlAttribute("entrance_state")]
		public UInt32 m_uEntranceStateId;

		[XmlAttribute("any_state")]
		public UInt32 m_uAnyStateId;
	}

	[Serializable]
	public class CFSMXMLState
	{
		[XmlAttribute("id")]
		public UInt32 m_uStateId;

		[XmlAttribute("name")]
		public string m_strName;

		[XmlElement("transition")]
		public List<CFSMXMLTranslation> m_transArray;

		[XmlElement("process")]
		public CFSMXMLActionArray m_Process;

		[XmlElement("enter")]
		public CFSMXMLActionArray m_Enter;

		[XmlElement("exit")]
		public CFSMXMLActionArray m_Exit;
	}

	[Serializable]
	public class CFSMXMLActionArray
	{
		[XmlElement("action")]
		public List<CFSMXMLAction> m_ActionList;
	}


	[Serializable]
	public class CFSMXMLTranslation
	{
		[XmlAttribute("target_id")]
		public UInt32 m_uTargetId;

		[XmlElement("condition")]
		public List<CFSMXMLConditionPipel> m_fsmConditionList;

		[XmlElement("event")]
		public List<CFSMXMLEventPipel> m_fsmEventList;
	}

	[Serializable]
	public class CFSMXMLAction
	{
		[XmlAttribute("name")]
		public string m_strName;

		[XmlAttribute("param_list")]
		public string m_strParamList;

		[XmlElement("condition")]
		public List<CFSMXMLConditionPipel> m_fsmConditionList;

		[XmlElement("event")]
		public List<CFSMXMLEventPipel> m_fsmEventList;
	}

	[Serializable]
	public class CFSMXMLEventPipel
	{
		[XmlAttribute("event_id")]
		public UInt32 m_uEventId;
	}

	[Serializable]
	public class CFSMXMLConditionPipel
	{
		[XmlElement("condition_cmp")]
		public List<CFSMXMLCondition> m_conditionList;
	}

	[Serializable]
	public class CFSMXMLCondition
	{
		[XmlAttribute("name")]
		public string m_strName;

		[XmlAttribute("cmp")]
		public string m_strCmp;

		[XmlAttribute("value")]
		public string m_strValue;

		public TCompareType GetCmpType()
		{
			return CCompareAlgorithm.GetCompareType(m_strCmp);
		}
	}


}
