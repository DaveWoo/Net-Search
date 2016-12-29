using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Net.Utils.Common;

namespace Net.Utils
{
	/// <summary>
	/// HttpHelper class
	/// </summary>
	public class HttpHelper
	{
		// 无 referrer 的 POST  // by hook.hu@gmail.com
		public string Post(string url, string content)
		{
			string data = Post(url, content, _lastUrl);
			_lastUrl = url;
			return data;
		}

		/// <summary>
		/// Send post data to server
		/// </summary>
		/// <param name="url"></param>
		/// <param name="content"></param>
		/// <returns>Return content</returns>
		public string Post(string url, string content, string referer)
		{
			int failedTimes = _tryTimes;
			while (failedTimes-- > 0)
			{
				try
				{
					if (_delayTime > 0)
					{
						Thread.Sleep(_delayTime * 1000);
					}
					HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(url));
					req.UserAgent = reqUserAgent;
					req.CookieContainer = _cc;
					req.Referer = referer;
					byte[] buff = Encoding.GetEncoding("GB2312").GetBytes(content);
					req.Method = "POST";
					req.Timeout = _timeout;
					req.ContentType = "application/x-www-form-urlencoded";
					req.ContentLength = buff.Length;
					if (null != _proxy && null != _proxy.Credentials)
					{
						req.UseDefaultCredentials = true;
					}
					req.Proxy = _proxy;
					//req.Connection = "Keep-Alive";
					Stream reqStream = req.GetRequestStream();
					reqStream.Write(buff, 0, buff.Length);
					reqStream.Close();

					//接收返回字串
					HttpWebResponse res = (HttpWebResponse)req.GetResponse();
					StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
					return sr.ReadToEnd();
				}
				catch (Exception e)
				{
					TraceLog.Error("HTTP POST Error: " + e.Message);
					TraceLog.Error("Url: " + url);
					TraceLog.Error("Data: " + content);
				}
			}

			return string.Empty;
		}

		// 无 referer 的 POST  // by hook.hu@gmail.com
		public string Get(string url)
		{
			string data = Get(url, _lastUrl);
			_lastUrl = url;
			return data;
		}

		/// <summary>
		/// Get data from server
		/// </summary>
		/// <param name="url"></param>
		/// <returns>Return content</returns>
		public string Get(string url, string referer)
		{
			int failedTimes = _tryTimes;
			while (failedTimes-- > 0)
			{
				try
				{
					if (_delayTime > 0)
					{
						Thread.Sleep(_delayTime * 1000);
					}
					if (url.IndexOf("http") == -1)
					{
						url = "http://" + url;
					}
					HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(url));
					req.UserAgent = reqUserAgent;
					req.CookieContainer = _cc;
					req.Referer = referer;

					req.Method = "GET";
					req.Timeout = _timeout;
					if (null != _proxy && null != _proxy.Credentials)
					{
						req.UseDefaultCredentials = true;
					}
					req.Proxy = _proxy;
					//req.Connection = "Keep-Alive";

					//接收返回字串
					HttpWebResponse res = (HttpWebResponse)req.GetResponse();
					StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
					return sr.ReadToEnd();
				}
				catch (Exception e)
				{
					TraceLog.Error("HTTP GET Error: " + e.Message);
					TraceLog.Error("Url: " + url);
				}
			}

			return string.Empty;
		}

		/// <summary>
		/// Set Proxy
		/// </summary>
		/// <param name="server"></param>
		/// <param name="port"></param>
		public void SetProxy(string server, int port, string username, string password)
		{
			if (null != server && port > 0)
			{
				_proxy = new WebProxy(server, port);
				if (null != username && null != password)
				{
					_proxy.Credentials = new NetworkCredential(username, password);
					_proxy.BypassProxyOnLocal = true;
				}
			}
		}

		/// <summary>
		/// Set delay connect time
		/// </summary>
		/// <param name="delayTime"></param>
		public void SetDelayConnect(int delayTime)
		{
			_delayTime = delayTime;
		}

		/// <summary>
		/// Set the timeout for each http request
		/// </summary>
		/// <param name="timeout"></param>
		public void SetTimeOut(int timeout)
		{
			if (timeout > 0)
			{
				_timeout = timeout;
			}
		}

		/// <summary>
		/// Set the try times for each http request
		/// </summary>
		/// <param name="timeout"></param>
		public void SetTryTimes(int times)
		{
			if (times > 0)
			{
				_tryTimes = times;
			}
		}

		/// <summary>
		/// Encode post data
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public static string EncodePostData(string data)
		{
			return HttpUtility.UrlEncode(data);
		}

		public void Download(string url, string localfile)
		{
			WebClient client = new WebClient();
			client.DownloadFile(url, localfile);
		}

		public static string GetIp()
		{
			try
			{
				if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
					return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
				else
					return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			}
			catch (Exception ex)
			{
				Log.Error("GetIp", ex);
			}
			return "-1";
		}

		#region Member Fields

		private CookieContainer _cc = new CookieContainer();
		private WebProxy _proxy;

		private int _delayTime;
		private int _timeout = 120000; // The default is 120000 milliseconds (120 seconds).
		private int _tryTimes = 3; // retry 3 times
		private string _lastUrl = string.Empty;
		private string reqUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.0.04506; InfoPath.2)";

		#endregion Member Fields
	}
}