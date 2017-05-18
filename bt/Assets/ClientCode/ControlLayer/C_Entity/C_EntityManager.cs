using System;
using UnityEngine;
using System.Collections;
using BTMISC;
using TMessageCode = System.Int32;
using NS_GAMESHARE;

namespace NS_GAME
{
	/// <summary>
	/// 实体类管理器
	/// </summary>
	public class EntityManager : CBTGameSingleton<EntityManager>
	{
		private QuickList<UInt32, EntityBase> m_EntityList;

		public TMessageCode AddEntity()
		{
			return CMessageCode.cnMSG_SUCCESS;
		}

		public TMessageCode RemoveEntity(UInt32 uEntityId)
		{
			return CMessageCode.cnMSG_SUCCESS;
		}

		public override void OnUpdate()
		{
			if (m_EntityList == null)
			{
				return;
			}
			Int32 nSize = m_EntityList.Size();
			for (Int32 i = 0; i < nSize; ++i )
			{
				EntityBase entity = m_EntityList.ValueIndexOf(i);
				if (entity == null)
				{
					continue;
				}
				entity.UpdateEntity();
			}
		}
		
		public override void Init()
		{
			m_EntityList = new QuickList<UInt32, EntityBase>();
		}

		public override void Release()
		{

		}
	}
}