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
		private List<CTreeNode> m_Children;

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
				m_Children = new List<CTreeNode>();
			}
			m_Children.Add(childNode);
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
			if (ContainChild(uNodeId) == true)
			{
				return false;
			}
			if (m_Children == null)
			{
				m_Children = new List<CTreeNode>();
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
			CTreeNode childNode = GetChildNode(uChildId);
			if (childNode == null)
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
			Int32 nSize = m_Children.Count;
			for (Int32 i = 0; i < nSize; ++i )
			{
				CTreeNode node = m_Children[i];
				if (node == null)
				{
					continue;
				}
				if (node.GetNodeID() == uNodeId)
				{
					return true;
				}
			}
			return false;
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
			Int32 nSize = m_Children.Count;
			return nSize;
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
			Int32 nSize = m_Children.Count;
			for (Int32 i = 0; i < nSize; ++i)
			{
				CTreeNode node = m_Children[i];
				if (node == null)
				{
					continue;
				}
				if (node.GetNodeID() == uChildId)
				{
					return node;
				}
			}
			return null;
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

		/// <summary>
		/// 深度
		/// </summary>
		/// <returns></returns>
		public Int32 Depth()
		{
			if (m_Children == null)
			{
				return 1;
			}
			Int32 nChildSize = m_Children.Count;
			if (nChildSize == 0)
			{
				return 1;
			}
			Int32 nMaxDepth = 1;
			for (Int32 i = 0; i < nChildSize; ++i )
			{
				CTreeNode childNode = m_Children[i];
				if (childNode == null)
				{
					continue;
				}
				Int32 nDepth = childNode.Depth();
				if (nDepth >= nMaxDepth)
				{
					nMaxDepth = nDepth + 1;
				}
			}
			return nMaxDepth;
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

		/// <summary>
		/// 根节点
		/// </summary>
		/// <returns></returns>
		public CTreeNode Root()
		{
			return m_NodeRoot;
		}

		/// <summary>
		/// 深度
		/// </summary>
		/// <returns></returns>
		public Int32 Depth()
		{
			if (m_NodeRoot == null)
			{
				return 0;
			}
			return m_NodeRoot.Depth();
		}
	}
}
