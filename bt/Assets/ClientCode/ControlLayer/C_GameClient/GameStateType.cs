using UnityEngine;
using System.Collections;

namespace NS_GAMESHARE
{
	// 游戏状态
	public enum TGameStateType
	{
		enInstall = 1,				// 游戏安装
		enUpdate = 2,				// 更新资源
		enLoading = 4,				// 加载数据
		enInitGame = 8,			// 初始化
		enLogin = 16,				// 登录
		enGame = 32,				// 游戏
		enDisconnect = 64,		// 断线

		enNone = 0,
	}
}