using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using CsQuery;
using iBoxDB.LocalServer;
using Net.Models;

namespace Net.Api
{
	public class SearchResource
	{
		public static ConcurrentQueue<String> searchList = new ConcurrentQueue<String>();
		public static ConcurrentQueue<String> urlList = new ConcurrentQueue<String>();
		public readonly static Engine Engine = new Engine();
		private static int commitCount = 100;

		public static String IndexText(String name, bool isDelete)
		{
			try
			{
				String url = getUrl(name);

				foreach (SitePage p in SDB.SearchBox.Select<SitePage>("from SitePage where url==?", url))
				{
					Engine.indexTextNoTran(SDB.SearchBox, commitCount, p.Id, p.Content.ToString(), true);
					Engine.indexTextNoTran(SDB.SearchBox, commitCount, p.RankUpId(), p.RankUpDescription(), true);
					SDB.SearchBox.Delete(Constants.TABLE_SITEPAGE, p.Id);
				}

				if (isDelete)
				{
					return "deleted";
				}
				{
					SitePage p = SitePage.Get(url);
					if (p == null)
					{
						return "temporarily unreachable";
					}
					else
					{
						p.Id = SDB.SearchBox.NewId();
						InsertSitePage(p, isDelete);
						urlList.Enqueue(p.Url);
						while (urlList.Count > 3)
						{
							String t;
							urlList.TryDequeue(out t);
						}
						return p.Url;
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return string.Empty;
		}

		public static void InsertSitePage(SitePage p, bool isDeleteCurrentThenAddNew)
		{
			if (p == null || p.Content == null || string.IsNullOrWhiteSpace(p.Url) || string.IsNullOrWhiteSpace(p.Title))
			{
				return;
			}
			if (isDeleteCurrentThenAddNew)
			{
				foreach (SitePage sitePage in SDB.SearchBox.Select<SitePage>("from SitePage where Url==?", p.Url))
				{
					Engine.indexTextNoTran(SDB.SearchBox, commitCount, sitePage.Id, sitePage.Content.ToString(), true);
					Engine.indexTextNoTran(SDB.SearchBox, commitCount, sitePage.RankUpId(), sitePage.RankUpDescription(), true);
					SDB.SearchBox.Delete(Constants.TABLE_SITEPAGE, sitePage.Id);
				}
			}

			SDB.SearchBox.Insert(Constants.TABLE_SITEPAGE, p);
			Engine.indexTextNoTran(SDB.SearchBox, commitCount, p.Id, p.Content.ToString(), false);
			Engine.indexTextNoTran(SDB.SearchBox, commitCount, p.RankUpId(), p.RankUpDescription(), false);
		}

		private static String getUrl(String name)
		{
			int p = name.IndexOf("http://");
			if (p < 0)
			{
				p = name.IndexOf("https://");
			}
			if (p >= 0)
			{
				name = name.Substring(p).Trim();
				var t = name.IndexOf("#");
				if (t > 0)
				{
					name = name.Substring(0, t);
				}
				return name;
			}
			return "";
		}
	}
}