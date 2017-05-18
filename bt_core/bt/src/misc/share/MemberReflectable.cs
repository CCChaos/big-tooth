using System;
using System.Collections.Generic;
using System.Reflection;

namespace BTMISC
{
	/// <summary>
	/// 属性反射
	/// </summary>
	public class CMemberReflector
	{
		public QuickList<string, MethodInfo> m_MethodInfoList;
		private QuickList<string, PropertyInfo> m_PropertyInfoList;

		/// <summary>
		/// Constructor
		/// </summary>
		public CMemberReflector()
		{
			m_MethodInfoList = new QuickList<string, MethodInfo>();
			m_PropertyInfoList = new QuickList<string, PropertyInfo>();
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool Init(Type type)
		{
			bool bRet = InitPropertyRelfector(type) &
							   InitMethodRelfector(type);
			return bRet;
		}

		/// <summary>
		/// 清除
		/// </summary>
		/// <returns></returns>
		public bool Clear()
		{
			m_MethodInfoList.Clear();
			m_PropertyInfoList.Clear();
			return true;
		}

		// 初始化方法数据
		private bool InitMethodRelfector(Type type)
		{
			if (type == null || type.IsClass == false)
			{
				return false;
			}
			if (m_MethodInfoList == null)
			{
				return false;
			}
			MethodInfo[] methodInfoArray = type.GetMethods();
			Int32 nPropSize = methodInfoArray == null ? 0 : methodInfoArray.Length;
			for (Int32 i = 0; i < nPropSize; ++i)
			{
				MethodInfo info = methodInfoArray[i];
				if (info == null)
				{
					continue;
				}
				CMethodAttribute[] cusAttrArray = info.GetCustomAttributes(typeof(CMethodAttribute), false) as CMethodAttribute[];
				Int32 nAttrSize = cusAttrArray == null ? 0 : cusAttrArray.Length;
				for (Int32 nAttrIndex = 0; nAttrIndex < nAttrSize; ++nAttrIndex )
				{
					CMethodAttribute attribute = cusAttrArray[nAttrIndex];
					if (attribute == null)
					{
						continue;
					}
					string strName = attribute.GetName();
					if (string.IsNullOrEmpty(strName) == true ||
						m_MethodInfoList.Add(strName, info) == false)
					{
						BTDebug.Warning(string.Format("<BTRELFECT> Type:{0} Add Method Info:{1} Failed", type.Name, strName));
						continue;
					}
				}
			}
			return true;
		}

		// 初始化属性数据
		private bool InitPropertyRelfector(Type type)
		{
			if (type == null || type.IsClass == false)
			{
				return false;
			}
			if (m_PropertyInfoList == null)
			{
				return false;
			}
			PropertyInfo[] propertyInfoArray = type.GetProperties();
			Int32 nPropSize = propertyInfoArray == null ? 0 : propertyInfoArray.Length;
			for (Int32 i = 0; i < nPropSize; ++i)
			{
				PropertyInfo info = propertyInfoArray[i];
				if (info == null)
				{
					continue;
				}
				CPropertyAttribute[] cusAttrArray = info.GetCustomAttributes(typeof(CPropertyAttribute), false) as CPropertyAttribute[];
				Int32 nAttrSize = cusAttrArray == null ? 0 : cusAttrArray.Length;
				for (Int32 nAttrIndex = 0; nAttrIndex < nAttrSize; ++nAttrIndex )
				{
					CPropertyAttribute attribute = cusAttrArray[nAttrIndex];
					string strName = attribute.GetName();
					if (string.IsNullOrEmpty(strName) == true ||
						m_PropertyInfoList.Add(strName, info) == false)
					{
						BTDebug.Warning(string.Format("<BTRELFECT> Type:{0} Add Property Info:{1} Failed", type.Name, strName));
						continue;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// 设置属性值
		/// </summary>
		/// <param name="strName"></param>
		/// <param name="objEntity"></param>
		/// <param name="objValue"></param>
		/// <returns></returns>
		public bool SetValue(string strName, System.Object objEntity, System.Object objValue)
		{
			if (objEntity == null)
			{
				return false;
			}

			PropertyInfo info = null;
			if (m_PropertyInfoList.QuickFind(strName, ref info) == false || info == null)
			{
				return false;
			}
			if (objEntity.GetType() != info.DeclaringType &&
				objEntity.GetType().IsAssignableFrom(info.DeclaringType) == false)
			{
				return false;
			}
			if (info.CanWrite == false)
			{
				return false;
			}
			try
			{
				info.SetValue(objEntity, objValue, null);
			}
			catch (System.Exception ex)
			{
				BTDebug.Exception(ex);
				return false;
			}
			return true;
		}

		/// <summary>
		/// 获取属性值
		/// </summary>
		/// <param name="strName"></param>
		/// <param name="objEntity"></param>
		/// <param name="rOutValue"></param>
		/// <returns></returns>
		public bool GetValue(string strName, System.Object objEntity, out System.Object rOutValue)
		{
			rOutValue = default(System.Object);
			if(objEntity == null)
			{
				return false;
			}

			PropertyInfo info = null;
			if (m_PropertyInfoList.QuickFind(strName, ref info) == false || info == null)
			{
				return false;
			}
			if (objEntity.GetType() != info.DeclaringType &&
				objEntity.GetType().IsAssignableFrom(info.DeclaringType) == false)
			{
				return false;
			}
			if (info.CanRead == false)
			{
				return false;
			}
			try
			{
				rOutValue = info.GetValue(objEntity, null);
			}
			catch (System.Exception ex)
			{
				BTDebug.Exception(ex);
				return false;
			}
			return true; ;
		}

		/// <summary>
		/// 调用方法
		/// </summary>
		/// <param name="strName"></param>
		/// <param name="objEntity"></param>
		/// <param name="objParamArray"></param>
		/// <param name="rOutReturnValue"></param>
		/// <returns></returns>
		public bool InvokeMethod(string strName, System.Object objEntity, System.Object[] objParamArray, out System.Object rOutReturnValue)
		{
			rOutReturnValue = default(System.Object);
			if (objEntity == null)
			{
				return false;
			}

			MethodInfo info = null;
			if (m_MethodInfoList.QuickFind(strName, ref info) == false || info == null)
			{
				return false;
			}
			if (objEntity.GetType() != info.DeclaringType &&
				objEntity.GetType().IsAssignableFrom(info.DeclaringType) == false)
			{
				return false;
			}

			try
			{
				rOutReturnValue = info.Invoke(objEntity, objParamArray);
			}
			catch (System.Exception ex)
			{
				BTDebug.Exception(ex);
				return false;
			}
			return true;
		}
	}

	/// <summary>
	/// 方法标记
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class CMethodAttribute : Attribute
	{
		private string m_strName;
		private string m_strDesc;

		public CMethodAttribute(string strName, string strDesc)
		{
			m_strName = strName;
			m_strDesc = strDesc;
		}

		public CMethodAttribute(string strName)
		{
			m_strName = strName;
			m_strDesc = string.Empty;
		}

		public string GetName()
		{
			return m_strName;
		}

		public string GetDesc()
		{
			return m_strDesc;
		}
	}

	/// <summary>
	/// 属性标记
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class CPropertyAttribute : Attribute
	{
		private string m_strName;
		private string m_strDesc;

		public CPropertyAttribute(string strName, string strDescription)
		{
			m_strName = strName;
			m_strDesc = strDescription;
		}

		public CPropertyAttribute(string strName)
		{
			m_strName = strName;
			m_strDesc = string.Empty;
		}

		public string GetName()
		{
			return m_strName;
		}

		public string GetDesc()
		{
			return m_strDesc;
		}
	}
}
