using System;
using System.Collections;
using NS_GAMESHARE;

using TMessageCode = System.Int32;

namespace NS_GAME
{
	/// <summary>
	/// 实体逻辑类基类
	/// </summary>
	public abstract class EntityBase
	{
		private UInt32 m_uEntityId;
		private UInt32 m_uSheetId;

		public TMessageCode InitEntity(UInt32 uEntityId, UInt32 uSheetId)
		{
			return CMessageCode.cnMSG_SUCCESS;
		}

		public virtual void UpdateEntity()
		{

		}
	}
}
