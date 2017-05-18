using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BTIO
{
	/// <summary>
	/// 文件读取器
	/// </summary>
	public class CBTFileReader
	{
		public enum TBTFileStatus
		{
			enClosed = 0,				// 未打开
			enOpenHandle = 1,		// 打开文件句柄
			enCached = 2,				// 已经缓存到内存
		}

		public const Int32 cnMaxFileLength = Int32.MaxValue;

		// 文件流
		private FileStream m_fileStream;
		// 当前文件状态
		private TBTFileStatus m_tFileStatue;
		// 缓存文件数据
		private Byte[] m_CacheByteArray;

		/// <summary>
		/// Constructor
		/// </summary>
		public CBTFileReader()
		{
			m_tFileStatue = TBTFileStatus.enCached;
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~CBTFileReader()
		{
			CloseIfOpened();
		}
		/// <summary>
		/// 打开文件
		/// </summary>
		/// <param name="strFilePath"></param>
		/// <param name="bCacheInMemory"></param>
		/// <returns></returns>
		public bool Open(string strFilePath, bool bCacheInMemory)
		{
			if (string.IsNullOrEmpty(strFilePath) == true)
			{
				return false;
			}
			if (File.Exists(strFilePath) == false)
			{
				return false;
			}

			CloseIfOpened();

			m_fileStream = File.Open(strFilePath, FileMode.Open, FileAccess.Read);
			if (m_fileStream == null || m_fileStream.CanRead == false || m_fileStream.Length > cnMaxFileLength)
			{
				return false;
			}

			Int64 n64FileLength = m_fileStream.Length;
			if (n64FileLength > cnMaxFileLength || n64FileLength < 0)
			{
				return false;
			}

			if (bCacheInMemory == true)
			{
				m_tFileStatue = TBTFileStatus.enCached;
				if (n64FileLength > 0)
				{
					m_CacheByteArray = new Byte[n64FileLength];
					m_fileStream.Read(m_CacheByteArray, 0, (Int32)n64FileLength);
				}
				m_fileStream.Close();
				m_fileStream = null;
			}
			else
			{
				m_tFileStatue = TBTFileStatus.enOpenHandle;
				m_CacheByteArray = null;
			}

			return true;
		}

		/// <summary>
		/// 关闭
		/// </summary>
		/// <returns></returns>
		public bool CloseIfOpened()
		{
			if (m_tFileStatue == TBTFileStatus.enClosed)
			{
				return true;
			}
			if (m_tFileStatue == TBTFileStatus.enOpenHandle && m_fileStream != null)
			{
				m_tFileStatue = TBTFileStatus.enClosed;
				m_fileStream.Close();
				return true;
			}
			if (m_tFileStatue == TBTFileStatus.enCached)
			{
				m_tFileStatue = TBTFileStatus.enClosed;
				m_CacheByteArray = null;
				return true;
			}
			return false;
		}

		/// <summary>
		/// 读取数据
		/// </summary>
		/// <param name="nOffset">文件偏移</param>
		/// <param name="nLength">读取长度</param>
		/// <param name="rOutByteArray"></param>
		/// <returns></returns>
		public bool ReadBytes(Int32 nOffset, Int32 nLength, ref Byte[] rOutByteArray)
		{
			// 参数检查
			if (nOffset < 0 || nLength <= 0)
			{
				return false;
			}

			// 数据检查
			if (CanRead() == false)
			{
				return false;
			}

			// 数据检查
			Int32 nFileLength = Length();
			if (nOffset + nLength > nFileLength)
			{
				return false;
			}

			// 输出内存初始化
			if (rOutByteArray != null)
			{
				if (rOutByteArray.Length < nLength)
				{
					return false;
				}
			}
			else
			{
				rOutByteArray = new Byte[nLength];
			}

			// 读取数据返回
			if (m_tFileStatue == TBTFileStatus.enOpenHandle &&
				m_fileStream != null &&
				m_fileStream.CanRead)
			{
				m_fileStream.Seek(nOffset, SeekOrigin.Begin);
				m_fileStream.Read(rOutByteArray, 0, nLength);
				return true;
			}
			if (m_tFileStatue == TBTFileStatus.enCached)
			{
				for (Int32 i = 0; i < nLength; ++i )
				{
					rOutByteArray[i] = m_CacheByteArray[i + nOffset];
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// 当前是否可读
		/// </summary>
		/// <returns></returns>
		public bool CanRead()
		{
			if (m_tFileStatue == TBTFileStatus.enCached ||
				m_tFileStatue == TBTFileStatus.enOpenHandle)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 文件长度
		/// </summary>
		/// <returns></returns>
		public Int32 Length()
		{
			Int32 nLength = 0;
			if (m_tFileStatue == TBTFileStatus.enCached)
			{
				nLength = m_CacheByteArray == null ? 0 : m_CacheByteArray.Length;
			}
			if (m_tFileStatue == TBTFileStatus.enOpenHandle)
			{
				if (m_fileStream == null || m_fileStream.CanRead == false)
				{
					nLength = 0;
				}
				else
				{
					nLength = (Int32)m_fileStream.Length;
				}
			}
			return nLength;
		}
	}
}
