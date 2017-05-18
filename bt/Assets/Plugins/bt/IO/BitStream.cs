using System;
using System.Collections.Generic;
using System.Text;
using BTIO;

namespace BTIO
{
	/// <summary>
	/// Bit流
	/// 支持Big-Endian以及Little-Endian
	/// </summary>
	public class CBitStream : CBTStream
	{
		#region Members
		// 当前位上空闲比特位数
		private Int32 m_nFreeBits;
		#endregion
		#region Methods
		/// <summary>
		/// Construct
		/// </summary>
		public CBitStream(bool bIsLittelEndian)
			: base(bIsLittelEndian)
		{
			m_nFreeBits = 8;
		}
		/// <summary>
		/// Construct
		/// </summary>
		/// <param name="bIsRead"></param>
		public CBitStream(Byte[] dataArray, bool bIsLitteEndian)
			: base(dataArray, bIsLitteEndian)
		{
			m_nFreeBits = 8;
		}
		/// <summary>
		/// Construct
		/// </summary>
		/// <param name="byteArrayData"></param>
		public CBitStream(Byte[] dataArray, Int32 nOffset, Int32 nLength, bool bIsLitteEndian)
			: base(dataArray, nOffset, nLength, bIsLitteEndian)
		{
			m_nFreeBits = 8;
		}

		/// <summary>
		/// 序列化最长64位长度数据
		/// </summary>
		/// <param name="rObjValue"></param>
		/// <param name="nBitSize"></param>
		/// <returns></returns>
		public bool SerialUInt64InBit(ref UInt64 rObjValue, Int32 nBitSize)
		{
			if (nBitSize <= 0 || nBitSize > 64)
			{
				return false;
			}
			UInt32 valueHigh = (UInt32)(rObjValue >> 32);
			UInt32 valueLow = (UInt32)(rObjValue & 0xFFFFFFFF);
			if (nBitSize > 32)
			{
				if (m_bIsLittleEndian == true)
				{
					SerialUInt32InBit(ref valueHigh, nBitSize - 32);
					SerialUInt32InBit(ref valueLow, 32);
				}
				else
				{
					SerialUInt32InBit(ref valueLow, 32);
					SerialUInt32InBit(ref valueHigh, nBitSize - 32);
				}
			}
			else
			{
				SerialUInt32InBit(ref valueLow, 32);
			}

			if (IsReading())
			{
				rObjValue = ((UInt64)valueHigh << 32) | valueLow;
			}

			return true;
		}

		/// <summary>
		/// 序列化最长32位长度数据
		/// </summary>
		/// <param name="rObjValue"></param>
		/// <param name="nBitSize"></param>
		/// <returns></returns>
		public bool SerialUInt32InBit(ref UInt32 rObjValue, Int32 nBitSize)
		{
			if (nBitSize <= 0 || nBitSize > 32)
			{
				return false;
			}
			UInt16 valueHigh = (UInt16)(rObjValue >> 16);
			UInt16 valueLow = (UInt16)(rObjValue & 0xFFFF);
			if (nBitSize > 16)
			{
				if (m_bIsLittleEndian == true)
				{
					SerialUInt16InBit(ref valueHigh, nBitSize - 16);
					SerialUInt16InBit(ref valueLow, 16);
				}
				else
				{
					SerialUInt16InBit(ref valueLow, 16);
					SerialUInt16InBit(ref valueHigh, nBitSize - 16);
				}
			}
			else
			{
				SerialUInt16InBit(ref valueLow, 16);
			}

			if (IsReading())
			{
				rObjValue = ((UInt32)valueHigh << 16) | valueLow;
			}

			return true;
		}

		/// <summary>
		/// 序列化最长16位长度数据
		/// </summary>
		/// <param name="rObjValue"></param>
		/// <param name="nBitSize"></param>
		/// <returns></returns>
		public bool SerialUInt16InBit(ref UInt16 rObjValue, Int32 nBitSize)
		{
			if (nBitSize <= 0 || nBitSize > 16)
			{
				return false;
			}
			Byte byteHigh = (Byte)(rObjValue >> 8);
			Byte byteLow = (Byte)(rObjValue & 0xFF);
			if (nBitSize > 8)
			{
				if (m_bIsLittleEndian == true)
				{
					SerialByteInBit(ref byteHigh, nBitSize - 8);
					SerialByteInBit(ref byteLow, 8);
				}
				else
				{
					SerialByteInBit(ref byteLow, 8);
					SerialByteInBit(ref byteHigh, nBitSize - 8);
				}
			}
			else
			{
				SerialByteInBit(ref byteLow, 8);
			}

			if (IsReading())
			{
				rObjValue = (UInt16)(((UInt16)byteHigh << 8) | byteLow);
			}

			return true;
		}

		/// <summary>
		/// 序列化最长8位长度数据
		/// </summary>
		/// <param name="rObjValue"></param>
		/// <param name="nBitSize"></param>
		/// <returns></returns>
		protected bool SerialByteInBit(ref Byte rObjValue, Int32 nBitSize)
		{
			if (nBitSize > 8 || nBitSize <= 0)
			{
				return false;
			}
			Byte mask = (Byte)((1 << m_nFreeBits) - 1);
			if (m_nFreeBits >= nBitSize) // 当前位数据够
			{
				Int32 nSurplusBit = m_nFreeBits - nBitSize;
				if (IsReading() == true)
				{
					Byte tmpValue = m_ByteDataArray[m_nPos];
					tmpValue &= mask;
					tmpValue >>= nSurplusBit;
					rObjValue |= tmpValue;
				}
				else
				{
					Byte tmpValue = (Byte)rObjValue;
					tmpValue &= mask;
					tmpValue <<= nSurplusBit;
					m_ByteDataArray[m_nPos] |= tmpValue;
				}
				m_nFreeBits = nSurplusBit;
				if (m_nFreeBits == 0)
				{
					m_nFreeBits = 8;
					m_nPos += 1;
				}
			}
			else
			{
				Int32 nRemainBit = nBitSize - m_nFreeBits;
				if (IsReading() == true)
				{
					Byte tmpValue = m_ByteDataArray[m_nPos];
					tmpValue &= mask;
					tmpValue <<= nRemainBit;
					rObjValue |= tmpValue;
				}
				else
				{
					Byte tmpValue = (Byte)(rObjValue >> nRemainBit);
					tmpValue &= mask;
					m_ByteDataArray[m_nPos] |= tmpValue;
				}
				m_nFreeBits = 8;
				m_nPos += 1;
				SerialByteInBit(ref rObjValue, nRemainBit);
			}
			return true;
		}

		/// <summary>
		/// 序列化一个bool值到一个bit位
		/// </summary>
		/// <param name="value"></param>
		public void SerialBoolInOneBit(ref bool value)
		{
			Byte tmpValue = value == true ? (Byte)1 : (Byte)0;
			SerialByteInBit(ref tmpValue, 1);
			if (IsReading())
			{
				value = (tmpValue != 0);
			}
		}
		
		#endregion

#region Override Methods
		public override Int32 GetLength()
		{
			Int32 nSize = 0;
			if (IsReading() == true)
			{
				nSize = m_ByteDataArray == null ? 0 : m_ByteDataArray.Length;
			}
			else
			{
				nSize = m_nPos;
			}

			if (m_nFreeBits < 8)
			{
				nSize += 1;
			}
			return nSize;
		}
		/// <summary>
		/// 起始状态
		/// </summary>
		/// <returns></returns>
		public override bool AtBegin()
		{
			return m_nPos == 0;
		}

		/// <summary>
		/// 其实位置
		/// </summary>
		/// <returns></returns>
		public override Int32 BeginPos()
		{
			return 0;
		}

		/// <summary>
		/// 颠倒读写模式
		/// </summary>
		/// <returns></returns>
		public override bool Reverse()
		{
			if (IsReading() == true)
			{
				Int32 nSize = GetLength();
				for (Int32 i = 0; i < nSize; ++i )
				{
					m_ByteDataArray[i] = 0;
				}
				m_nPos = BeginPos();
				m_bIsRead = false;
				m_nFreeBits = 8;
			}
			else
			{
				Int32 nSize = GetLength();
				if (nSize == 0)
				{
					m_ByteDataArray = null;
				}
				if (nSize > 0 && m_nPos != nSize - 1)
				{
					Byte[] newByte = new Byte[nSize];
					for (Int32 i = 0; i < nSize; ++i)
					{
						newByte[i] = m_ByteDataArray[i];
					}
					m_ByteDataArray = newByte;
				}
				m_nPos = BeginPos();
				m_bIsRead = true;
				m_nFreeBits = 8;
			}
			return true;
		}

		/// <summary>
		/// 移动
		/// </summary>
		/// <param name="nStep"></param>
		/// <returns>return true if Success</returns>
		public override bool Move(Int32 nStep)
		{
			if (m_ByteDataArray == null)
			{
				return false;
			}
			Int32 nNewStep = nStep + m_nPos;
			if (nNewStep < 0)
			{
				return false;
			}
			if (nNewStep >= m_ByteDataArray.Length)
			{
				return false;
			}
			m_nPos = nNewStep;
			return true;
		}
#endregion

#region Override Serial Method
		public override bool SerialBuffer(Byte[] bufferArray, Int32 nPos, Int32 nLength)
		{
			Int32 nBufferLength = bufferArray == null ? 0 : bufferArray.Length;
			if (nPos + nLength > nBufferLength || nLength <= 0)
			{
				return false;
			}
			for (Int32 i = nPos; i < nBufferLength; ++i )
			{
				if (IsReading())
				{
					Byte tmpValue = 0;
					SerialByteInBit(ref tmpValue, 8);
					bufferArray[i] = (Byte)tmpValue;
				}
				else
				{
					Byte tmpValue = bufferArray[i];
					SerialByteInBit(ref tmpValue, 8);
				}
			}
			return true;
		}
#endregion
	}
}
