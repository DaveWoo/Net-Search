using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Utils
{
	public class NetConfigData
	{
		public string ProxyServer; // 代理
		public int ProxyPort; // 代理
		public string ProxyUser; // 代理
		public string ProxyPass; // 代理
		public int HttpTimeout = 120000; // 设置每次链接的超时(秒)，默认是120秒
		public int HttpTryTimes = 3; // 设置每次HTTP请求失败后尝试的次数，默认是3次
		public int ConnectDelayTime; // 链接延时时间
	}

	public class ContentHelper
	{
		public static string GetMidString(string str, string start, string end)
		{
			int pos;
			return GetMidString(str, start, end, out pos);
		}

		public static int GetMidInteger(string str, string start, string end)
		{
			int pos;
			return GetMidInteger(str, start, end, out pos);
		}

		public static int GetMidInteger(string str, string start, string end, out int pos)
		{
			string content = GetMidString(str, start, end, out pos);
			return GetInteger(content);
		}

		public static string GetMidString(string str, string start, string end, out int pos)
		{
			pos = -1;
			if (str == null)
			{
				return null;
			}
			int index = str.IndexOf(start);
			if (-1 == index)
			{
				return null;
			}
			index += start.Length;
			int num2 = str.IndexOf(end, index);
			if (-1 == num2)
			{
				return null;
			}
			pos = num2;
			return str.Substring(index, num2 - index).Trim();
		}

		public static int GetInteger(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return -1;
			}
			int id;
			if (int.TryParse(str, out id))
			{
				return id;
			}
			return -1;
		}

		public static List<int> GetIntegerList(string str, string del)
		{
			List<int> result = new List<int>();
			if (string.IsNullOrEmpty(str))
			{
				return result;
			}
			string[] parts = str.Split(del.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			foreach (string part in parts)
			{
				int id = GetInteger(part);
				if (id != -1)
				{
					result.Add(id);
				}
			}
			return result;
		}

		public static List<int> GetIntegerList(string str)
		{
			return GetIntegerList(str, ",");
		}

		public static string NoHtml(string result)
		{
			if (result == null)
			{
				return string.Empty;
			}
			StringBuilder builder = new StringBuilder();
			bool flag = false;
			foreach (char ch in result)
			{
				switch (ch)
				{
					case '<':
						flag = true;
						break;

					case '>':
						flag = false;
						break;

					default:
						if (!flag)
						{
							if (ch == '\n')
							{
								builder.Append(" ");
							}
							else if (((ch != ' ') && (ch != '\t')) && (ch != '\r'))
							{
								builder.Append(ch);
							}
						}
						break;
				}
			}
			return builder.ToString().Trim().Replace("&yen;", "￥").Replace("&nbsp;", "");
		}

		/// <summary>
		/// Helper class
		/// </summary>
		/// <param name="str"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		/// Added by zhuangjq@gmail.cm 2008-9-27
		public static string getMid(string str, int startPos, string start, string end, out int pos)
		{
			pos = -1;
			if (str == null)
			{
				return null;
			}
			int posStart = str.IndexOf(start, startPos);
			if (-1 == posStart)
			{
				return null;
			}

			posStart += start.Length;
			int posEnd = str.IndexOf(end, posStart);
			if (-1 == posEnd)
			{
				return null;
			}

			pos = posEnd;

			return str.Substring(posStart, posEnd - posStart);
		}

		/// Modify by zhuangjq@gmail.cm 2008-9-27
		public static string getMid(string str, string start, string end, out int pos)
		{
			return getMid(str, 0, start, end, out pos);
		}
	}
}