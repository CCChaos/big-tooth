using System;
using System.Threading;
using System.Collections.Generic;

namespace BTMISC
{
	/// <summary>
	/// 线程安全队列
	/// </summary>
	public class SafeQueue
	{
		// 数据存储
		private System.Object[] m_ObjArray;
		private Int32 m_nHead;
		private Int32 m_nTail;
		private Int32 m_nSize;	      // m_nSize = 1 << x;
		private Int32 m_nSizeMask;// m_nSizeMask = m_nSize - 1;
		private System.Object m_ObjLock;

		/// <summary>
		/// Construct
		/// </summary>
		/// <param name="nBuffLen"></param>
		public SafeQueue(Int32 nBuffLen)
		{
			m_ObjLock = new System.Object();
			if (nBuffLen < 0)
			{
				nBuffLen = 0;
			}
			if (GetCapacity(nBuffLen, ref m_nSize, ref m_nSizeMask) == false)
			{
				m_nSize = 4;
				m_nSizeMask = 4 - 1;
			}
			m_ObjArray = new System.Object[m_nSize];
			m_nHead = 0;
			m_nTail = 0;
		}

		/// <summary>
		/// 加入队尾
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public bool Push(System.Object obj)
		{
			lock (m_ObjLock)
			{
				//if (m_nHead == ((m_nTail + 1) % m_nSize)) // full
				if (m_nHead == ((m_nTail + 1) & m_nSizeMask) &&
					Extend() == false) // full
				{
					return false;
				}
				m_ObjArray[m_nTail] = obj;

				if (m_nTail + 1 == m_nSize)
				{
					m_nTail = 0;
				}
				else
				{
					m_nTail += 1;
				}
				return true;
			}
		}

		/// <summary>
		/// 弹出队头
		/// </summary>
		/// <param name="rOutObj"></param>
		/// <returns></returns>
		public bool Pop(ref System.Object rOutObj)
		{
			lock (m_ObjLock)
			{
				if (m_nHead == m_nTail) // empty
				{
					return false;
				}
				rOutObj = m_ObjArray[m_nHead];
				if (m_nHead + 1 == m_nSize)
				{
					m_nHead = 0;
				}
				else
				{
					m_nHead += 1;
				}
				return true;
			}
		}

		/// <summary>
		/// 队头对象，不弹出
		/// </summary>
		/// <param name="rOutObj"></param>
		/// <returns></returns>
		public bool Front(ref System.Object rOutObj)
		{
			lock (m_ObjLock)
			{
				if (m_nHead == m_nTail) // Empty
				{
					return false;
				}
				rOutObj = m_ObjArray[m_nHead];
				return true;
			}
		}

		/// <summary>
		/// 弹出队列所有对象
		/// </summary>
		/// <param name="rOutArray"></param>
		/// <returns></returns>
		public bool PopAll(ref List<System.Object> rOutList)
		{
			lock (m_ObjLock)
			{
				if (rOutList == null)
				{
					rOutList = new List<System.Object>();
				}
				if (m_nHead == m_nTail)
				{
					return true;
				}
				//for (Int32 i = m_nHead; i != m_nTail; i = ((i + 1) % m_nSize))
				for (Int32 i = m_nHead; i != m_nTail; i = ((i + 1) & m_nSizeMask))
				{
					rOutList.Add(m_ObjArray[i]);
				}
				m_nHead = 0;
				m_nTail = 0;
				return true;
			}	
		}

		/// <summary>
		/// 是否为空
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			lock (m_ObjLock)
			{
				return (m_nHead == m_nTail);
			}
		}

		private bool Extend()
		{
			Int32 nNewSize = 0;
			Int32 nNewMask = 0;
			if (GetCapacity(m_nSize + 1, ref nNewSize, ref nNewMask) == false)
			{
				return false;
			}
			System.Object[] newArray = new System.Object[nNewSize];
			Int32 nNewArrayIndex = 0;
			for (Int32 i = m_nHead; i != m_nTail; i = (i + 1) & m_nSizeMask, ++nNewArrayIndex )
			{
				newArray[nNewArrayIndex] = m_ObjArray[m_nHead];
			}

			m_nSize = nNewSize;
			m_nSizeMask = nNewMask;
			m_nHead = 0;
			m_nTail = nNewArrayIndex;
			m_ObjArray = newArray;
			return true;
		}

		private bool GetCapacity(Int32 nSize, ref Int32 rOutNCap, ref Int32 rOutNMask)
		{
			rOutNCap = 4;
			rOutNMask = 4 - 1;
			if (nSize <= 0)
			{
				return true;
			}
			while (rOutNCap < nSize)
			{
				if (rOutNCap > 0x1FFFF)
				{
					return false;
				}
				rOutNCap <<= 1;
				rOutNMask -= 1;
			}
			return true;
		}
	}
}
