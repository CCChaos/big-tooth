using System;
using System.Collections.Generic;
using System.Text;
using BTMISC;
using UnityEngine;

namespace NS_GAME.KIT
{
	/// <summary>
	/// HTTP请求封装类
	/// </summary>
	public class CHttpRequest : CBTSingleton<CHttpRequest>, IGameSingleton
	{
#region Members
		public delegate void HandleHTTPOnResponse(string strError, WWW wwwLoaderResult, System.Object objSessionRequest);
		private bool m_bIsRequesting;
		private HandleHTTPOnResponse m_HandleOnResponse;
		private WWW m_WWWWorker;
		private System.Object m_ObjSessionRequest;
#endregion

		/// <summary>
		/// 开始请求
		/// </summary>
		/// <param name="strURL"></param>
		/// <param name="handleOnResponse"></param>
		/// <param name="postForm"></param>
		/// <param name="objSessionRequest"></param>
		/// <returns></returns>
		public bool StartGet(string strURL, HandleHTTPOnResponse handleOnResponse, WWWForm postForm = null, System.Object objSessionRequest = null)
		{
			if (m_bIsRequesting == true)
			{
				return false;
			}
			try
			{
				m_WWWWorker = new WWW(strURL);
			}
			catch (System.Exception ex)
			{
				BTDebug.ExceptionEx(ex);
			}
			
			if (m_WWWWorker == null)
			{
				return false;
			}
			m_HandleOnResponse = handleOnResponse;
			m_ObjSessionRequest = objSessionRequest;
			m_bIsRequesting = true;

			return true;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public void Init()
		{
			m_bIsRequesting = false;
		}

		/// <summary>
		/// 更新
		/// </summary>
		public void Update()
		{
			if (m_bIsRequesting == false)
			{
				return;
			}
			UpdateRequest();
		}

		/// <summary>
		/// Release
		/// </summary>
		public void Release()
		{

		}

		// 检测请求状态
		private void UpdateRequest()
		{
			if (m_WWWWorker == null)
			{
				return;
			}

			WWW wwwWorker = m_WWWWorker;
			HandleHTTPOnResponse handleResponse = m_HandleOnResponse;
			System.Object objSessionRequest = m_ObjSessionRequest;
			// 发生错误
			if (string.IsNullOrEmpty(wwwWorker.error) == false)
			{
				m_bIsRequesting = false;
				m_HandleOnResponse = null;
				m_WWWWorker = null;
				m_ObjSessionRequest = null;
				if (handleResponse != null)
				{
					handleResponse(wwwWorker.error, null, objSessionRequest);
				}
				return;
			}

			// 未结束
			if (wwwWorker.isDone == false)
			{
				return;
			}

			// 完成
			m_bIsRequesting = false;
			m_HandleOnResponse = null;
			m_WWWWorker = null;
			m_ObjSessionRequest = null;
			if (handleResponse != null)
			{
				handleResponse(string.Empty, wwwWorker, objSessionRequest);
			}
		}



	}
}
