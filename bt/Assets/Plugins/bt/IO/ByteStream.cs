using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BTIO
{
	/// <summary>
	/// Byte流
	/// 支持Little-Endian以及Big-Endian
	/// </summary>
	public class CByteStream : CBTStream
	{
#region Serial Methods
		public override bool SerialBuffer(Byte[] bufferArray, Int32 nPos, Int32 nLength)
		{
			Int32 nBufferLength = bufferArray == null ? 0 : bufferArray.Length;
			if (nPos + nLength > nBufferLength)
			{
				return false;
			}

			if (IsReading())
			{
				for (Int32 i = 0; i < nLength; ++i)
				{
					bufferArray[nPos + i] = m_ByteDataArray[m_nPos + i];
				}
			}
			else
			{
				IncreaseLengthIfNessary(nLength);
				for (Int32 i = 0; i < nLength; ++i)
				{
					m_ByteDataArray[m_nPos + i] = bufferArray[nPos + i];
				}
			}
			m_nPos += nLength;
			return true;
		}
#endregion
#region Methods
		/// <summary>
		/// Construct
		/// </summary>
		public CByteStream(bool bIsLittelEndian)
			: base(bIsLittelEndian)
		{
		}
		/// <summary>
		/// Construct
		/// </summary>
		/// <param name="bIsRead"></param>
		public CByteStream(Byte[] dataArray, bool bIsLitteEndian)
			: base(dataArray, bIsLitteEndian)
		{
		}
		/// <summary>
		/// Construct
		/// </summary>
		/// <param name="byteArrayData"></param>
		public CByteStream(Byte[] dataArray, Int32 nOffset, Int32 nLength, bool bIsLitteEndian)
			: base(dataArray, nOffset, nLength, bIsLitteEndian)
		{
		}
		
#endregion
#region Override Methods
		/// <summary>
		/// Get Length
		/// </summary>
		/// <returns></returns>
		public override Int32 GetLength()
		{
			if (IsReading() == true)
			{
				return m_ByteDataArray == null ? 0 : m_ByteDataArray.Length;
			}
			return m_nPos;
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
				for (Int32 i = 0; i < nSize; ++i)
				{
					m_ByteDataArray[i] = 0;
				}
				m_nPos = BeginPos();
				m_bIsRead = false;
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
	}
}
