using UnityEngine;
using System.Collections;
using BTMISC;
using System.Xml.Serialization;
using System.IO;
using System;

namespace NS_GAME.KIT
{
	/// <summary>
	/// 以Unity为依赖的杂项集合
	/// </summary>
	public partial class GameUtil
	{
		// 全局变量
		private static MemoryStream sDataBox = new MemoryStream();

#region MessageBox

#endregion
#region 文件
#if UNITY_EDITOR
		/// <summary>
		/// 从xml文件中读取类的属性值
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static T LoadFromXmlFile<T>(string fileName) where T : class
		{
			// Read the xml to a string from file
			XmlSerializer x = new XmlSerializer(typeof(T));
			FileStream f = new FileStream(fileName, FileMode.Open);
			return x.Deserialize(f) as T;
		}

		/// <summary>
		/// 将类的属性值按xml格式化输出到文件
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="xmlString"></param>
		public static void SaveToXmlFile(System.Object obj, string fileName)
		{
			XmlSerializer x = new XmlSerializer(obj.GetType());
			FileStream f = new FileStream(fileName, FileMode.Create);
			StreamWriter sw = new StreamWriter(f, PackageSetting.cTextFileEncoding);
			x.Serialize(sw, obj);
			f.Close();
		}
#endif
#endregion
#region 序列化
		/// <summary>
		/// 从流中读取数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="memStream"></param>
		/// <param name="rOutT"></param>
		/// <returns></returns>
		public static bool ReadPBFromStream<T>(MemoryStream memStream, ref T rOutT) where T : class
		{
			if (memStream == null)
			{
				return false;
			}
			try
			{
				rOutT = ProtoBuf.Serializer.Deserialize<T>(memStream);
			}
			catch (System.Exception ex)
			{
				BTDebug.ExceptionEx(ex);
				rOutT = null;
			}
			return true;
		}

		/// <summary>
		/// 将PB写入流
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="memStream"></param>
		/// <param name="objT"></param>
		/// <returns></returns>
		public static bool WritePBToStream<T>(ref MemoryStream memStream, T objT) where T : class
		{
			if (objT == null)
			{
				return false;
			}
			if (memStream == null)
			{
				memStream = new MemoryStream();
			}
			try
			{
				ProtoBuf.Serializer.Serialize<T>(memStream, objT);
			}
			catch (System.Exception ex)
			{
				BTDebug.ExceptionEx(ex);
				return false;
			}
			return true;
		}

		/// <summary>
		/// 从数据序列化PB
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dataArray"></param>
		/// <returns></returns>
		public static T ReadPBFromData<T>(byte[] dataArray) where T : class
		{
			sDataBox.SetLength(0);
			T retObj = null;
			sDataBox.Write(dataArray, 0, dataArray.Length);
			if (ReadPBFromStream(sDataBox, ref retObj) == false)
			{
				retObj = null;
			}
			sDataBox.SetLength(0);
			return retObj;
		}

		/// <summary>
		/// 序列化PB数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="pbObject"></param>
		/// <returns></returns>
		public static byte[] WritePBToData<T>(T pbObject) where T : class
		{
			sDataBox.SetLength(0);
			if (WritePBToStream(ref sDataBox, pbObject) == false)
			{
				return null;
			}

			if (sDataBox.Length <= 0 || sDataBox.Length > Int32.MaxValue)
			{
				return null;
			}
			Int32 nSize = (Int32)sDataBox.Length;
			byte[] data = new byte[nSize];
			sDataBox.Read(data, 0, nSize);
			sDataBox.SetLength(0);
			return data;
		}

		public static string WriteToXmlString(System.Object obj)
		{
			XmlSerializer x = new XmlSerializer(obj.GetType());
			MemoryStream ms = new MemoryStream();
			StreamWriter sw = new StreamWriter(ms, PackageSetting.cTextFileEncoding);
			XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
			xsn.Add(string.Empty, string.Empty);
			x.Serialize(sw, obj, xsn);
			return PackageSetting.cTextFileEncoding.GetString(ms.GetBuffer());
		}

		public static T ReadFromXmlString<T>(string xmlString) where T : class
		{
			XmlSerializer x = new XmlSerializer(typeof(T));
			T ret = null;
			using (MemoryStream ms = new MemoryStream(PackageSetting.cTextFileEncoding.GetBytes(xmlString)))
			{
				ret = x.Deserialize(ms) as T;
			}
			return ret;
		}
#endregion

	}
}

