using System;
using NS_GAMESHARE;
using BTMISC;
using BTFSM;
using NS_GAME.KIT;
using NS_GAME.DATA;
using System.Collections.Generic;

namespace NS_GAME
{
	public partial class CGameClient :
		CBTGameSingleton<CGameClient>, IFSMEntity
	{
		private readonly static List<Type> m_preLoadDataTypeList = new List<Type>{
													   typeof(Data_ConfigTree),
													   typeof(Data_EditorTextList),
													   typeof(Data_EditorObjectList)
												   };

		/// <summary>
		/// 开始加载游戏启动数据
		/// </summary>
		public void StartLoading()
		{
			GameDataManager.Instance.StartLoadData(m_preLoadDataTypeList, OnLoadComplete, (info) => 
			{
				BTDebug.Exception("Load GameData Error");
			});
		}

		private void OnLoadComplete()
		{
			m_GameFSM.ChangeToState((UInt32)TGameStateType.enInitGame, null);
		}
	}
}
