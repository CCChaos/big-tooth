using System;
using System.Collections.Generic;
using System.Text;

namespace BTMISC
{
	/// <summary>
	/// 循环使用对象接口
	/// </summary>
	public interface IRecycleable
	{
		void Init();
		void Clear();
	}
}
