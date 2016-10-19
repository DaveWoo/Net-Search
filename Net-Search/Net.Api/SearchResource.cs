using System;
using System.Collections.Concurrent;
using Net.Models;
using Net.Utils;
using Net.Utils.Common;

namespace Net.Api
{
	public class SearchResource
	{
		public static ConcurrentQueue<String> searchList = new ConcurrentQueue<String>();
		public static ConcurrentQueue<String> urlList = new ConcurrentQueue<String>();
		private static int commitCount = 100;
		public readonly static Engine Engine = new Engine();

		public static String IndexText(String name, bool isDelete)
		{
			try
			{
				String url = getUrl(name);

				SitePage p = SitePage.Get(url);
				if (p == null)
				{
					return "temporarily unreachable";
				}
				else
				{
					p.Id = SDB.SitePageBox.NewId();
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
			catch (Exception ex)
			{
				Log.Error("IndexText", ex);
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
				foreach (SitePage sitePage in SDB.SitePageBox.Select<SitePage>(string.Format(Constants.SQLLIKEURL, Constants.TABLE_SITEPAGE), p.Url))
				{
					Engine.indexTextNoTran(SDB.SitePageBox, commitCount, sitePage.Id, sitePage.Content.ToString(), true);
					Engine.indexTextNoTran(SDB.SitePageBox, commitCount, sitePage.RankUpId(), sitePage.RankUpDescription(), true);
					SDB.SitePageBox.Delete(Constants.TABLE_SITEPAGE, sitePage.Id);
				}
			}

			SDB.SitePageBox.Insert(Constants.TABLE_SITEPAGE, p);
			Engine.indexTextNoTran(SDB.SitePageBox, commitCount, p.Id, p.Content.ToString(), false);
			Engine.indexTextNoTran(SDB.SitePageBox, commitCount, p.RankUpId(), p.RankUpDescription(), false);
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