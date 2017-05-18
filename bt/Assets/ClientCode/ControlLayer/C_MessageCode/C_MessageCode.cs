using System;
using TMessageCode = System.Int32;

namespace NS_GAMESHARE
{
	public class CMessageCode
	{
#region Message Define
		/*
		 * 消息码分段规则
		 * 小于0 平台消息码
		 * 1-10000 系统消息码
		 * [xyzzz] x: 模块ID， y:模块子功能ID，zzz 消息码
		 */
		public const TMessageCode cnMSG_SUCCESS = 0;
		public const TMessageCode cnMSG_SYS_Unknown = 100; // 未知错误
		public const TMessageCode cnMSG_SYS_NULLError = 1000; // NULL错误
		public const TMessageCode cnMSG_SYS_EMPTYError = 1001; // 空错误
		
		/* 更新模块
		 */
		public const TMessageCode cnMSG_Update_NoUpdateConfig = 10001; // 未获取更新列表

		/* 登录模块
		 */
		public const TMessageCode cnMSG_Login_NoServer = 20001; // 未获取服务器列表

		/* 资源模块
		 */
		public const TMessageCode cnMSG_RES_ResListErr = 30001; // 资源列表错误

		/* 实体模块
		 */
		//  实体逻辑
		public const TMessageCode cnMSG_ENTITY_SheetEmpty = 40001; // 配置为空
		
		// 实体Instance
		public const TMessageCode cnMSG_ENTITY_ResourceEmpty = 41001; // 资源为空


#endregion

	}
}
