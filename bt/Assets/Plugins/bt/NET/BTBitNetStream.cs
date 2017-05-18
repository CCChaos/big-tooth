using System;
using System.Collections.Generic;
using System.Text;
using BTIO;
using System.Net;

namespace BTMISC
{
	/// <summary>
	/// BT网络包
	///  支持 BigEndian 以及 LittleEndian
	/// 消息头: 4字节长度 消息头不受大小端影响，消息头数据按照网络字节序传送
	/// </summary>
	public class CBTBitNetStream : CBitStream
	{
		/// <summary>
		/// BT消息协议头长度
		/// </summary>
		public const Int32 cnBTMsgHeadSize = 4;
#region Methods
		/// <summary>
		/// Construct
		/// </summary>
		public CBTBitNetStream(bool bIsLittelEndian)
			: base(bIsLittelEndian)
		{
			m_nPos = cnBTMsgHeadSize;
		}
		/// <summary>
		/// Construct
		/// </summary>
		/// <param name="bIsRead"></param>
		public CBTBitNetStream(Byte[] dataArray, bool bIsLitteEndian)
			: base(dataArray, bIsLitteEndian)
		{
			m_nPos = cnBTMsgHeadSize;
		}
		/// <summary>
		/// Construct
		/// </summary>
		/// <param name="byteArrayData"></param>
		public CBTBitNetStream(Byte[] dataArray, Int32 nOffset, Int32 nLength, bool bIsLitteEndian)
			: base(dataArray, nOffset, nLength, bIsLitteEndian)
		{
			m_nPos = cnBTMsgHeadSize;
		}

		/// <summary>
		/// 更新消息头
		/// </summary>
		/// <returns></returns>
		public bool FixMsgHead()
		{
			if (m_ByteDataArray == null)
			{
				return false;
			}

			Int32 nLength = GetLength();
			if (nLength < cnBTMsgHeadSize)
			{
				return false;
			}
			nLength = nLength - cnBTMsgHeadSize;
			nLength = IPAddress.HostToNetworkOrder(nLength);
			Byte[] headByteArray = BitConverter.GetBytes(nLength);
			headByteArray.CopyTo(m_ByteDataArray, 0);
			return true;
		}

		/// <summary>
		/// 起始状态
		/// </summary>
		/// <returns></returns>
		public override bool AtBegin()
		{
			return m_nPos == cnBTMsgHeadSize;
		}

		/// <summary>
		/// 其实位置
		/// </summary>
		/// <returns></returns>
		public override int BeginPos()
		{
			return cnBTMsgHeadSize;
		}
#endregion
	}
}
