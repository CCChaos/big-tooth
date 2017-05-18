using System;


namespace NS_GAME.KIT
{
	/// <summary>
	/// 运行时设置
	/// </summary>
	public class RunningTimeSettings
	{
		// 系统运行类型
		private static TSysRunningType m_SysRunningType = TSysRunningType.enUnDefined;


		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		public static TSysRunningType SystemRunningType 
		{
			get { return m_SysRunningType; }
			set { m_SysRunningType = value; }
		}
	}

}
