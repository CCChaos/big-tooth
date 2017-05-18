using System;
using BTIO;

namespace BTMISC
{
	public delegate void HandleVoidAction();
	public delegate void HandleIntAction(Int32 nActionParam);
	public delegate void HandleStringAction(string strActionParam);
	public delegate void HandleByteArrayAction(Byte[] byteArrayParam);
	public delegate void HandleStreamAction(CByteStream streamActionParam);
	public delegate void HandleObjectAction(System.Object objectParam);
	public delegate void HandleTAction<T>(T tParam);
}
