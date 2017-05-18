using System;
using System.Net;
using BTMISC;
using System.IO;
using System.Text;

namespace BTNET
{
	/// <summary>
	/// Http Get 封装
	/// </summary>
	public class CBTHttpGetRequest : CBTSingleton<CBTHttpGetRequest>
	{
		private StringBuilder m_strBuilder;
		private const Int32 c_nBuffLength = 0x100; // 每次读取buff长度

		/// <summary>
		/// Constructor
		/// </summary>
		public CBTHttpGetRequest()
		{
			m_strBuilder = new StringBuilder();
		}

		/// <summary>
		/// 开始一个Get请求
		/// </summary>
		/// <param name="strUrl"></param>
		/// <param name="handleOnDone"></param>
		/// <param name="handleOnFailed"></param>
		public void StartRequest(string strUrl, HandleStringAction handleOnDone, HandleTAction<HttpStatusCode> handleOnFailed)
		{
			HttpWebRequest httpWebRequest = null;
			HttpWebResponse httpWebResponse = null;
			try
			{
				httpWebRequest = (HttpWebRequest)WebRequest.Create(strUrl);
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			}
			catch (Exception)
			{
				if (handleOnFailed != null)
				{
					handleOnFailed(HttpStatusCode.NotAcceptable);
				}
				return;
			}


			if (httpWebResponse.StatusCode != HttpStatusCode.OK)
			{
				if (handleOnFailed != null)
				{
					handleOnFailed(httpWebResponse.StatusCode);
				}
				return;
			}

			m_strBuilder.Length = 0;
			Stream streamResponse = httpWebResponse.GetResponseStream();
			Encoding encoding = Encoding.UTF8;
			StreamReader streamReader = new StreamReader(streamResponse, encoding);
			char[] charBuffArray = new char[c_nBuffLength];
			int nReadLength = streamReader.Read(charBuffArray, 0, c_nBuffLength);
			while (nReadLength > 0)
			{
				string str = new string(charBuffArray, 0, nReadLength);
				m_strBuilder.Append(str);
				nReadLength = streamReader.Read(charBuffArray, 0, c_nBuffLength);
			}

			if (handleOnDone != null)
			{
				handleOnDone(m_strBuilder.ToString());
			}
		}
	}
}
