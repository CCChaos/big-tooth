using System;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace BTMISC
{
	public class Util
	{
#region 文件路径处理
		public static string GetFileNameWithoutExtention(string fileName, char separator = '/')
		{
			string name = GetFileName(fileName, separator);
			return GetFilePathWithoutExtention(name);
		}

		public static string GetFilePathWithoutExtention(string fileName)
		{
			return fileName.Substring(0, fileName.LastIndexOf('.'));
		}

		public static string GetDirectoryName(string fileName)
		{
			return fileName.Substring(0, fileName.LastIndexOf('/'));
		}

		public static string GetFileName(string path, char separator = '/')
		{
			return path.Substring(path.LastIndexOf(separator) + 1);
		}

		public static string NormalizeFilePath(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			return str.Replace("\\", "/");
		}
#endregion
#region MD5

		public static Byte[] CreateMD5(Byte[] data)
		{
			using (var md5 =MD5.Create())
			{
				return md5.ComputeHash(data);
			}
		}

		public static string FormatMD5(Byte[] data)
		{
			return System.BitConverter.ToString(data).Replace("-", "").ToLower();
		}

		#endregion
#region 反射
		public static List<string> DumpInstance(Object obj, string strName)
		{
			if (obj == null)
			{
				return null;
			}
			List<string> strRet = new List<string>();
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
			Type typeObj = obj.GetType();
			if (typeObj.IsValueType == true || typeObj == typeof(string))
			{
				strBuilder.Length = 0;
				strBuilder.AppendFormat("<Name>:{0}\t<Value>:{1}\t<Type>:{2}\n",strName, obj, typeObj);
				strRet.Add(strBuilder.ToString());
				return strRet;
			}

			if (typeObj.GetInterface("IEnumerable") != null)
			//if (typeObj.IsAssignableFrom(typeof(System.Collections.IEnumerable)))
			{
				MethodInfo getIEnumMethod = typeObj.GetMethod("GetEnumerator");
				if (getIEnumMethod == null)
				{
					return strRet;
				}
				System.Collections.IEnumerator enumerator = getIEnumMethod.Invoke(obj, null) as System.Collections.IEnumerator;
				if (enumerator == null)
				{
					return strRet;
				}
				System.Object arrayObj = null;
				Int32 nIndex = 0;
				strBuilder.Length = 0;
				strBuilder.AppendFormat("<Collections>{0}:\n", strName);
				while (enumerator.MoveNext())
				{
					arrayObj = enumerator.Current;
					if (arrayObj == null)
					{
						break;
					}
					List<string> arrayChild = DumpInstance(arrayObj, "\tElement_" + nIndex);
					foreach (string child in arrayChild)
					{
						strBuilder.AppendFormat("\t{0}\n", child);
					}
					nIndex += 1;
				}
				strRet.Add(strBuilder.ToString());
				return strRet;
			}

			PropertyInfo[] propertyInfos = typeObj.GetProperties();
			strBuilder.Length = 0;
			strBuilder.AppendFormat("<Name>:{0}, <Type>:{1}\n", strName, typeObj.Name);
			strRet.Add(strBuilder.ToString());
			foreach (PropertyInfo info in propertyInfos)
			{
				strBuilder.Length = 0;
				Object objValue = info.GetValue(obj, null);
				if (info.PropertyType.IsValueType == true || info.PropertyType == typeof(string))
				{
					strBuilder.AppendFormat("\t<Name>:{0}\t<Value>:{1}\t<Type>:{2}\n", info.Name, objValue, info.PropertyType.Name);
				}
				else
				{
					List<string> dumpList = DumpInstance(objValue, "\t" + info.Name);
					foreach (string strChildDump in dumpList)
					{
						strBuilder.AppendFormat("\t{0}", strChildDump);
					}
				}
				strRet.Add(strBuilder.ToString());
			}
			return strRet;
		}
#endregion
	}

}
