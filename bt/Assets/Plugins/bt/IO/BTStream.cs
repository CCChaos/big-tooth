using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace BTIO
{
	/// <summary>
	/// 流对象接口
	/// </summary>
	public abstract class CBTStream
	{
#region Members
		// End of Stream
		public const Int32 EOS = -1;
		// Init Buffer length 
		public const Int32 InitBufferLength = 128;
		
		// Is Read Mode
		protected bool m_bIsRead;
		// Stream Data
		protected Byte[] m_ByteDataArray;
		// Current Position
		protected Int32 m_nPos;
		// LittleEndian or BigEndian
		protected bool m_bIsLittleEndian;
		// 临时交换区
		private static Byte[] m_TmpByteArray2 = new Byte[2];
		private static Byte[] m_TmpByteArray4 = new Byte[4];
		private static Byte[] m_TmpByteArray8 = new Byte[8];
#endregion
#region abstract Serial interface
		public abstract bool SerialBuffer(Byte[] bufferArray, Int32 nPos, Int32 nLength);
		public virtual void Serial(ref bool rObj)
		{
			if (IsReading())
			{
				rObj = (0 == m_ByteDataArray[m_nPos]) ? true : false;
			}
			else
			{
				IncreaseLengthIfNessary(1);
				m_ByteDataArray[m_nPos] = rObj ? (Byte)(1) : (Byte)(0);
			}
			m_nPos += 1;
		}
		public virtual void Serial(ref Byte rObj)
		{
			if (IsReading())
			{
				rObj = m_ByteDataArray[m_nPos];
			}
			else
			{
				IncreaseLengthIfNessary(1);
				m_ByteDataArray[m_nPos] = rObj;
			}
			m_nPos += 1;
		}
		public virtual void Serial(ref Int16 rObj)
		{
			if (IsReading())
			{
				lock (this)
				{
					SerialBuffer(m_TmpByteArray2, 0, 2);
					if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
					{
						Array.Reverse(m_TmpByteArray2);
					}
					rObj = BitConverter.ToInt16(m_TmpByteArray2, 0);
				}
			}
			else
			{
				IncreaseLengthIfNessary(2);
				Byte[] byteArray = BitConverter.GetBytes(rObj);
				if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
				{
					Array.Reverse(byteArray);
				}
				SerialBuffer(byteArray, 0, 2);
				//byteArray.CopyTo(m_ByteDataArray, m_nPos);
				//m_nPos += 4;
			}
		}
		public virtual void Serial(ref UInt16 rObj)
		{
			if (IsReading())
			{
				lock (this)
				{
					SerialBuffer(m_TmpByteArray2, 0, 2);
					if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
					{
						Array.Reverse(m_TmpByteArray2);
					}
					rObj = BitConverter.ToUInt16(m_TmpByteArray2, 0);
				}
			}
			else
			{
				IncreaseLengthIfNessary(2);
				Byte[] byteArray = BitConverter.GetBytes(rObj);
				if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
				{
					Array.Reverse(byteArray);
				}
				SerialBuffer(byteArray, 0, 2);
				//byteArray.CopyTo(m_ByteDataArray, m_nPos);
				//m_nPos += 4;
			}

		}
		public virtual void Serial(ref Int32 rObj)
		{
			if (IsReading())
			{
				lock (this)
				{
					SerialBuffer(m_TmpByteArray4, 0, 4);
					if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
					{
						Array.Reverse(m_TmpByteArray4);
					}
					rObj = BitConverter.ToInt32(m_TmpByteArray4, 0);
				}
			}
			else
			{
				IncreaseLengthIfNessary(4);
				Byte[] byteArray = BitConverter.GetBytes(rObj);
				if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
				{
					Array.Reverse(byteArray);
				}
				SerialBuffer(byteArray, 0, 4);
				//byteArray.CopyTo(m_ByteDataArray, m_nPos);
				//m_nPos += 4;
			}

		}
		public virtual void Serial(ref UInt32 rObj)
		{
			if (IsReading())
			{
				lock (this)
				{
					SerialBuffer(m_TmpByteArray4, 0, 4);
					if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
					{
						Array.Reverse(m_TmpByteArray4);
					}
					rObj = BitConverter.ToUInt32(m_TmpByteArray4, 0);
				}
			}
			else
			{
				IncreaseLengthIfNessary(4);
				Byte[] byteArray = BitConverter.GetBytes(rObj);
				if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
				{
					Array.Reverse(byteArray);
				}
				SerialBuffer(byteArray, 0, 4);
				//byteArray.CopyTo(m_ByteDataArray, m_nPos);
				//m_nPos += 4;
			}

		}
		public virtual void Serial(ref Single rObj)
		{
			if (IsReading())
			{
				lock (this)
				{
					SerialBuffer(m_TmpByteArray4, 0, 4);
					if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
					{
						Array.Reverse(m_TmpByteArray4);
					}
					rObj = BitConverter.ToSingle(m_TmpByteArray4, 0);
				}
			}
			else
			{
				IncreaseLengthIfNessary(4);
				Byte[] byteArray = BitConverter.GetBytes(rObj);
				if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
				{
					Array.Reverse(byteArray);
				}
				SerialBuffer(byteArray, 0, 4);
				//byteArray.CopyTo(m_ByteDataArray, m_nPos);
				//m_nPos += 4;
			}

		}
		public virtual void Serial(ref Double rObj)
		{
			if (IsReading())
			{
				lock (this)
				{
					SerialBuffer(m_TmpByteArray8, 0, 8);
					if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
					{
						Array.Reverse(m_TmpByteArray8);
					}
					rObj = BitConverter.ToDouble(m_TmpByteArray8, 0);
				}
			}
			else
			{
				IncreaseLengthIfNessary(8);
				Byte[] byteArray = BitConverter.GetBytes(rObj);
				if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
				{
					Array.Reverse(byteArray);
				}
				SerialBuffer(byteArray, 0, 8);
				//byteArray.CopyTo(m_ByteDataArray, m_nPos);
				//m_nPos += 8;
			}

		}
		public virtual void Serial(ref Int64 rObj)
		{
			if (IsReading())
			{
				lock (this)
				{
					SerialBuffer(m_TmpByteArray8, 0, 8);
					if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
					{
						Array.Reverse(m_TmpByteArray8);
					}
					rObj = BitConverter.ToInt64(m_TmpByteArray8, 0);
				}
			}
			else
			{
				IncreaseLengthIfNessary(8);
				Byte[] byteArray = BitConverter.GetBytes(rObj);
				if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
				{
					Array.Reverse(byteArray);
				}
				SerialBuffer(byteArray, 0, 8);
				//byteArray.CopyTo(m_ByteDataArray, m_nPos);
				//m_nPos += 8;
			}

		}
		public virtual void Serial(ref UInt64 rObj)
		{
			if (IsReading())
			{
				lock (this)
				{
					SerialBuffer(m_TmpByteArray8, 0, 8);
					if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
					{
						Array.Reverse(m_TmpByteArray8);
					}
					rObj = BitConverter.ToUInt64(m_TmpByteArray8, 0);
				}
			}
			else
			{
				IncreaseLengthIfNessary(8);
				Byte[] byteArray = BitConverter.GetBytes(rObj);
				if (BitConverter.IsLittleEndian != m_bIsLittleEndian)
				{
					Array.Reverse(byteArray);
				}
				SerialBuffer(byteArray, 0, 8);
				//byteArray.CopyTo(m_ByteDataArray, m_nPos);
				//m_nPos += 8;
			}
		}
		public virtual void SerialUTF8String(ref string rObj)
		{
			Int32 strLen = 0;
			if (IsReading())
			{
				Serial(ref strLen);
				if (strLen <= 0)
				{
					rObj = string.Empty;
					return;
				}
				rObj = System.Text.Encoding.UTF8.GetString(m_ByteDataArray, m_nPos, strLen);
			}
			else
			{
				if (string.IsNullOrEmpty(rObj) == true)
				{
					strLen = 0;
					Serial(ref strLen);
					return;
				}
				Byte[] by = System.Text.Encoding.UTF8.GetBytes(rObj);
				strLen = by.Length;
				IncreaseLengthIfNessary(strLen + 4);
				Serial(ref strLen);
				by.CopyTo(m_ByteDataArray, m_nPos);
			}
			m_nPos += strLen;
		}
		public virtual void Serial<T>(ref T rObj) where T : IStreamable, new()
		{
			CBTStream stream = this;
			if (rObj == null)
			{
				if (IsReading() == true)
				{
					rObj = new T();
				}
				else
				{
					throw new NullReferenceException("Can Not Serial A Null Object");
				}
			}
			rObj.Serial(ref stream);
		}
		public virtual void SerialCont(ref bool[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new bool[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref Byte[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new Byte[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref Int16[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new Int16[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref UInt16[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new UInt16[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref Int32[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new Int32[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref UInt32[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new UInt32[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref Single[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new Single[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref Double[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new Double[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref Int64[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new Int64[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref UInt64[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new UInt64[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref string[] rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new string[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					SerialUTF8String(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					SerialUTF8String(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont<T>(ref T[] rObjArray) where T : IStreamable, new()
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new T[nSize];
				for (Int32 i = 0; i < nSize; ++i)
				{
					rObjArray[i] = new T();
					Serial(ref rObjArray[i]);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Length;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Serial(ref rObjArray[i]);
				}
			}
		}
		public virtual void SerialCont(ref List<bool> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<bool>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					bool tmpValue = false;
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					bool tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont(ref List<Byte> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<Byte>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					Byte tmpValue = 0;
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Byte tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont(ref List<Int16> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<Int16>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					Int16 tmpValue = 0;
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Int16 tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont(ref List<UInt16> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<UInt16>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					UInt16 tmpValue = 0;
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					UInt16 tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont(ref List<Int32> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<Int32>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					Int32 tmpValue = 0;
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Int32 tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont(ref List<UInt32> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<UInt32>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					UInt32 tmpValue = 0;
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					UInt32 tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont(ref List<Single> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<Single>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					Single tmpValue = 0;
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Single tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont(ref List<Double> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<Double>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					Double tmpValue = 0;
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Double tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont(ref List<Int64> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<Int64>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					Int64 tmpValue = 0;
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					Int64 tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont(ref List<UInt64> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<UInt64>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					UInt64 tmpValue = 0;
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					UInt64 tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont(ref List<string> rObjArray)
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<string>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					string tmpValue = "";
					SerialUTF8String(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					string tmpValue = rObjArray[i];
					SerialUTF8String(ref tmpValue);
				}
			}
		}
		public virtual void SerialCont<T>(ref List<T> rObjArray) where T : IStreamable, new()
		{
			Int32 nSize = 0;
			if (IsReading())
			{
				Serial(ref nSize);
				if (nSize <= 0)
				{
					return;
				}
				rObjArray = new List<T>();
				for (Int32 i = 0; i < nSize; ++i)
				{
					T tmpValue = new T();
					Serial(ref tmpValue);
					rObjArray.Add(tmpValue);
				}
			}
			else
			{
				if (rObjArray == null)
				{
					nSize = 0;
					Serial(ref nSize);
					return;
				}
				nSize = rObjArray.Count;
				for (Int32 i = 0; i < nSize; ++i)
				{
					T tmpValue = rObjArray[i];
					Serial(ref tmpValue);
				}
			}
		}
#endregion
#region abstract interface
		// 获取数据长度
		public abstract Int32 GetLength();
		// 是否在初始状态
		public abstract bool AtBegin();
		// 初始位置
		public abstract Int32 BeginPos();
		// 颠倒读写状态
		public abstract bool Reverse();
		// 移动
		public abstract bool Move(Int32 nStep);
#endregion
#region Methods
		/// <summary>
		/// Constructor
		/// 初始化流，没有数据，可以写入
		/// </summary>
		/// <param name="bIsLittelEndian"></param>
		public CBTStream(bool bIsLittelEndian)
		{
			m_bIsRead = false;
			m_bIsLittleEndian = bIsLittelEndian;
			m_ByteDataArray = new Byte[InitBufferLength];
			m_nPos = BeginPos();
		}

		/// <summary>
		/// Constructor
		/// 初始化一个流，有数据，可以读取
		/// </summary>
		/// <param name="dataArray"></param>
		public CBTStream(Byte[] dataArray, bool bIsLitteEndian)
		{
			m_bIsRead = true;
			m_bIsLittleEndian = bIsLitteEndian;
			m_ByteDataArray = dataArray;
			m_nPos = BeginPos();
		}

		/// <summary>
		/// Constructor
		/// 初始化流，有数据，可以读取
		/// </summary>
		/// <param name="dataArray"></param>
		/// <param name="nOffset"></param>
		/// <param name="nLength"></param>
		/// <param name="bIsLitteEndian"></param>
		public CBTStream(Byte[] dataArray, Int32 nOffset, Int32 nLength, bool bIsLitteEndian)
		{
			m_bIsRead = true;
			m_bIsLittleEndian = bIsLitteEndian;
			if (nOffset < 0 || nLength <= 0 || dataArray == null || nOffset + nLength > dataArray.Length)
			{
				m_ByteDataArray = null;
				m_nPos = 0;
				BTMISC.BTDebug.Exception("Stream Init Error", "IO");
				return;
			}
			m_ByteDataArray = new Byte[nLength];
			for (Int32 i = 0; i < nLength; ++i )
			{
				m_ByteDataArray[i] = dataArray[i + nOffset];
			}
			m_nPos = BeginPos();
		}

		/// <summary>
		/// Is Read Stream
		/// </summary>
		/// <returns></returns>
		public bool IsReading()
		{
			return m_bIsRead;
		}

		/// <summary>
		/// 设置字节序
		/// 如果已经存在处理的数据则不能设置
		/// </summary>
		/// <param name="bIsLittleEndian"></param>
		/// <returns></returns>
		public virtual bool SetIsLittleEndian(bool bIsLittleEndian)
		{
			if (m_bIsLittleEndian == bIsLittleEndian)
			{
				return true;
			}
			if (AtBegin() == false)
			{
				return false;
			}
			m_bIsLittleEndian = bIsLittleEndian;
			return true;
		}

		/// <summary>
		/// 获取数据数组
		/// </summary>
		/// <returns></returns>
		public virtual Byte[] GetByteArray()
		{
			Int32 nSize = GetLength();
			if (nSize <= 0)
			{
				return null;
			}
			Byte[] array = new Byte[nSize];
			for (Int32 i = 0; i < nSize; ++i)
			{
				array[i] = m_ByteDataArray[i];
			}
			return array;
		}

		/// <summary>
		/// Increase Byte Array If Necessary
		/// </summary>
		/// <param name="nLength"></param>
		/// <returns></returns>
		protected Int32 IncreaseLengthIfNessary(Int32 nIncLength)
		{
			if (m_ByteDataArray == null)
			{
				m_ByteDataArray = new Byte[1024];
			}

			Int32 nLength = m_ByteDataArray.Length;
			if (nLength >= m_nPos + nIncLength)
			{
				return nLength;
			}
			Int32 nNewLength = (nLength > nIncLength) ? (nLength << 1/*nLength + nLength*/) : (nLength + nIncLength);
			Byte[] newByteArray = new Byte[nNewLength];

			for (Int32 i = 0; i < nLength; ++i)
			{
				newByteArray[i] = m_ByteDataArray[i];
			}

			return nNewLength;
		}
#endregion
#region Debug
		/// <summary>
		/// 获取数据显示
		/// </summary>
		/// <returns></returns>
		public string GetDataString()
		{
			string strData = string.Empty;
			Int32 nSize = GetLength();
			for (Int32 i = 0; i < nSize; ++i)
			{
				strData += string.Format("{0:X2}", m_ByteDataArray[i]);
				if ((i + 1) % 8 == 0)
				{
					strData += "\n";
				}
				else if ((i + 1) % 4 == 0)
				{
					strData += "\t";
				}
				else if ((i + 1) % 2 == 0)
				{
					strData += "  ";
				}
				else
				{
					strData += " ";
				}
			}
			strData = strData.ToUpper();
			strData += string.Format("\nTotal Length:{0}", nSize);
			return strData;
		}
#endregion
	}


	/// <summary>
	/// 可序列化对象接口
	/// </summary>
	public interface IStreamable
	{
		void Serial(ref CBTStream rStream);
	}
}
