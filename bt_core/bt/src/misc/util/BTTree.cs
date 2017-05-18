using System;
using System.Collections.Generic;
using System.Text;

namespace BTMISC
{
	// 树节点
	public class CTreeNode
	{
		// 节点ID
		private UInt32 m_uNodeId;
		// 节点数据
		private System.Object m_objData;
		// 父节点
		private CTreeNode m_NodeParent;
		// 子节点
		private QuickList<UInt32, CTreeNode> m_Children;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="objData"></param>
		/// <returns></returns>
		public static CTreeNode NewTreeRootNode(System.Object objData)
		{
			CTreeNode rootNode = new CTreeNode(0, objData);
			return rootNode;
		}

		/// <summary>
		/// 挂接子节点
		/// </summary>
		/// <param name="childNode"></param>
		/// <returns></returns>
		public bool AttachChild(CTreeNode childNode)
		{
			if (childNode == null)
			{
				return false;
			}
			if (childNode.m_NodeParent != null)
			{
				return false;
			}
			UInt32 uChildID = childNode.GetNodeID();
			if (ContainChild(uChildID) == true)
			{
				return false;
			}

			// 挂接到父节点
			childNode.m_NodeParent = this;
			// 添加到子节点
			if (m_Children == null)
			{
				m_Children = new QuickList<UInt32, CTreeNode>();
			}
			if (m_Children.Add(uChildID, childNode) == false)
			{
				childNode.m_NodeParent = null;
				return false;
			}
			return true;
		}

		/// <summary>
		/// 添加子节点
		/// </summary>
		/// <param name="uNodeId"></param>
		/// <param name="sysObj"></param>
		/// <returns></returns>
		public bool AddChild(UInt32 uNodeId, System.Object sysObj)
		{
			if (m_Children.ContainKey(uNodeId) == true)
			{
				return false;
			}
			if (m_Children == null)
			{
				m_Children = new QuickList<UInt32, CTreeNode>();
			}

			CTreeNode newNode = new CTreeNode(uNodeId, sysObj);
			bool bAdd = AttachChild(newNode);
			return bAdd;
		}

		/// <summary>
		/// 移出子节点
		/// </summary>
		/// <param name="uChildId"></param>
		/// <returns></returns>
		public CTreeNode RemoveChild(UInt32 uChildId)
		{
			if (m_Children == null)
			{
				return null;
			}
			CTreeNode childNode = null;
			if (m_Children.Pop(uChildId, ref childNode) == false || childNode == null)
			{
				return null;
			}
			childNode.m_NodeParent = null;

			return childNode;
		}

		/// <summary>
		/// 包含子节点
		/// </summary>
		/// <param name="uNodeId"></param>
		/// <returns></returns>
		public bool ContainChild(UInt32 uNodeId)
		{
			if (m_Children == null)
			{
				return false;
			}
			bool bContain = m_Children.ContainKey(uNodeId);
			return bContain;
		}

		/// <summary>
		/// 自己点个数
		/// </summary>
		/// <returns></returns>
		public Int32 GetChildCount()
		{
			if (m_Children == null)
			{
				return 0;
			}
			Int32 nSize = m_Children.Size();
			return nSize;
		}

		/// <summary>
		/// 子节点ID列表
		/// </summary>
		/// <returns></returns>
		public List<UInt32> GetChildIDList()
		{
			if (m_Children == null)
			{
				return null;
			}
			List<UInt32> uKeyIDList = m_Children.KeyList();
			return uKeyIDList;
		}

		/// <summary>
		/// 获取子节点
		/// </summary>
		/// <param name="uChildId"></param>
		/// <returns></returns>
		public CTreeNode GetChildNode(UInt32 uChildId)
		{
			if (m_Children == null)
			{
				return null;
			}
			CTreeNode child = null;
			if (m_Children.QuickFind(uChildId, ref child) == false)
			{
				return null;
			}
			return child;
		}

		/// <summary>
		/// 节点ID
		/// </summary>
		/// <returns></returns>
		public UInt32 GetNodeID()
		{
			return m_uNodeId;
		}

		/// <summary>
		/// 节点数据
		/// </summary>
		/// <returns></returns>
		public System.Object GetNodeData()
		{
			return m_objData;
		}

		/// <summary>
		/// 设置节点数据
		/// </summary>
		/// <param name="nodeData"></param>
		public void SetNodeData(System.Object nodeData)
		{
			m_objData = nodeData;
		}

		/// <summary>
		/// 是否是叶子节点
		/// </summary>
		/// <returns></returns>
		public bool IsLeaf()
		{
			Int32 nCount = GetChildCount();
			if (nCount == 0)
			{
				return true;
			}
			return false;
		}

		// Constructor
		protected CTreeNode(UInt32 uId, System.Object objData)
		{
			m_uNodeId = uId;
			m_objData = null;
			m_NodeParent = null;
			m_Children = null;
		}
	}


	public class CBTTree
	{
		// 根节点
		private CTreeNode m_NodeRoot;

		public CBTTree(System.Object objRootData)
		{
			m_NodeRoot = CTreeNode.NewTreeRootNode(objRootData);
		}

		public CTreeNode Root()
		{
			return m_NodeRoot;
		}
	}
}
