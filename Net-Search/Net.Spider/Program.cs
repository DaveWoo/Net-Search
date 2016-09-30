using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Net.Api;
using Net.Models;
using Net.Utils;

namespace Spider
{
	internal class Program
	{
		private static HttpHelper _hh1 = new HttpHelper();

		private static void Main(string[] args)
		{
			//iBoxDB.LocalServer.DB.Root("/tmp/");
			//var text = iBoxDB.NDB.RunALL();

			// Step 1:
			//GetAllSiteInfoFromChinaZ();
			//var sds = SDB.SearchDB.Select<SiteInfo>(string.Format(Constants.LIKESQL, Constants.TABLE_SITEINFO));

			// Step 2:
			//SearchBy360();
			//GetAlllinksFromSite();

			// Step 3: 1
			// GrabLinksContent();

			// Step 3 : 2
			//GrabLinks();

			// Step 4 : Add ad
			AddAd("abc.com",
				"这是您的第一份免费广告",
				"这是您的第一份免费广告,我们将竭诚为您服务",
				"广告",
				"新闻；news");
		}

		private static void AddAd(string url, string title, string description, string company, string tag)
		{
			SitePage sitePage = new SitePage();
			sitePage.Title = title;
			sitePage.Description = description;

			sitePage.Url = url;

			sitePage.VerifiedCompmany = company;
			sitePage.CreatedTimeStamp = System.DateTime.Now;
			sitePage.ModifiedTimeStamp = System.DateTime.Now;
			sitePage.Tag = tag;
			SDB.AssistBox.Insert(Constants.TABLE_AD, sitePage);
			TraceLog.PrintLn("Add ad: " + sitePage.Title);
		}

		private static void GrabLinks()
		{
			var configList = SDB.AssistBox.Select<NetServerConfig>(string.Format(Constants.LIKESQL, Constants.TABLE_NETSERVERCONFIG));
			List<string> sitePageList = new List<string>();

			NetServerConfig processLinkConfig = GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABLINKS);

			try
			{
				string likeSqlProcessLink = string.Format("from {0} Id >? order by Id", Constants.TABLE_LINK);
				var processLinks = SDB.AssistBox.Select<Link>(likeSqlProcessLink, processLinkConfig.ProcessedLinkAnchorId);
				for (int i = 0; i < processLinks.Count(); i = i + Constants.takeCount)
				{
					var prepareProcessLinks = processLinks.Where(p => p.Id > processLinkConfig.ProcessedLinkAnchorId).Take(1);

					if (prepareProcessLinks.LastOrDefault().Url == "http://baby.sina.com.cn")
					{
					}

					if (prepareProcessLinks != null && prepareProcessLinks.LastOrDefault() != null)
					{
						Parallel.ForEach<Link>(prepareProcessLinks,
						(e) =>
						{
							TraceLog.PrintLn(processLinkConfig.ProcessedLinkAnchorId + " link processing: " + e.Url);
							GrabLinksToDB(sitePageList, e.Url);
							TraceLog.PrintLn(processLinkConfig.ProcessedLinkAnchorId + " link processed: " + e.Url);
						});
					}

					processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Id;
					processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
					SDB.AssistBox.Update(Constants.TABLE_NETSERVERCONFIG, processLinkConfig);
				}
			}
			catch (Exception ex)
			{
				processLinkConfig.ProcessedLinkAnchorId += 1;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				SDB.AssistBox.Update(Constants.TABLE_NETSERVERCONFIG, processLinkConfig);
				TraceLog.Error("Error: " + ex.Message);
			}
		}

		private static void GrabLinksContent()
		{
			NetServerConfig processLinkConfig = GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABCONTENTS);
			try
			{
				string likeSqlProcessLink = string.Format("from {0} Id >? order by Id", Constants.TABLE_LINK);
				var processLinks = SDB.AssistBox.Select<Link>(likeSqlProcessLink, processLinkConfig.ProcessedLinkAnchorId);
				for (int i = 0; i < processLinks.Count(); i = i + Constants.takeCount)
				{
					var prepareProcessLinks = processLinks.Where(p => p.Id > processLinkConfig.ProcessedLinkAnchorId).Take(Constants.takeCount);

					if (prepareProcessLinks != null && prepareProcessLinks.LastOrDefault() != null)
					{
						Parallel.ForEach<Link>(prepareProcessLinks,
						(e) =>
						{
							TraceLog.PrintLn(e.Id + " processing: " + e.Url);
							SearchResource.IndexText(e.Url, false);
							TraceLog.PrintLn(e.Id + " processed: " + e.Url);
						});
						processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Id;
						processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
						SDB.AssistBox.Update(Constants.TABLE_NETSERVERCONFIG, processLinkConfig);
					}
				}
			}
			catch (Exception ex)
			{
				processLinkConfig.ProcessedLinkAnchorId += 1;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				SDB.AssistBox.Update(Constants.TABLE_NETSERVERCONFIG, processLinkConfig);
				TraceLog.Error("Error: " + ex.Message);
			}
		}

		private static void Test1()
		{
			string pattern = "<a href=hit.php+(.*)+</a>";
			string temp = "http://www.cwrank.com/main/rank.php?geo=all&page=";
			Dictionary<string, string> siteDic = new Dictionary<string, string>();
			List<string> items = new List<string>();
			for (int i = 10; i < 62; i++)
			{
				var page = temp + i;
				var contents = _hh1.Get(page);
				var body = ContentHelper.GetMidString(contents, "<tr bgcolor=ffffff height=15>", "</table></td></tr>");
				var bodyItemList = body.Split('\n');

				foreach (var item in bodyItemList)
				{
					var siteItem = ContentHelper.GetMidString(item, "<a href=hit.php", "/a>");
					if (siteItem != null)
					{
						var site = ContentHelper.GetMidString(siteItem, "title=", "target");
						if (site != null)
						{
							var siteName = ContentHelper.GetMidString(siteItem, "_blank>", "<");
							siteDic.Add(site, siteName);
						}
					}
				}
			}

			foreach (var item in siteDic)
			{
				var name = item.Key;

				if (name.Length > 500)
				{
					name = "";
					return;
				}
				name = name.Replace("<", " ").Replace(">", " ")
					.Replace("\"", " ").Replace(",", " ")
						.Replace("\\$", " ").Trim();

				bool? isdelete = null;

				if (name.StartsWith("http://") || name.StartsWith("https://"))
				{
					isdelete = false;
				}
				else if (name.StartsWith("delete")
				  && (name.Contains("http://") || name.Contains("https://")))
				{
					isdelete = true;
				}
				if (!isdelete.HasValue)
				{
					SearchResource.searchList.Enqueue(name);
					while (SearchResource.searchList.Count > 15)
					{
						String t;
						SearchResource.searchList.TryDequeue(out t);
					}
				}
				else
				{
					name = SearchResource.IndexText(name, isdelete.Value);
					System.GC.Collect();
				}
			}
		}

		private static void GetAllSiteInfoFromChinaZ()
		{
			string pattern = "<a href=hit.php+(.*)+</a>";
			string temp = "http://top.chinaz.com/all/index_";

			SiteInfo info = null;
			Dictionary<string, SiteInfo> siteDic = new Dictionary<string, SiteInfo>();

			List<string> allPages = new List<string>();
			allPages.Add("http://top.chinaz.com/all/index.html");
			for (int i = 1; i <= 1727; i++)
			//for (int i = 2; i <= 10; i++)
			{
				allPages.Add(temp + i + ".html");
			}
			string alexa = "http://alexa.chinaz.com/?domain=";
			string linkedR = "http://outlink.chinaz.com/?h=";
			foreach (var page in allPages)
			{
				var contents = _hh1.Get(page);
				var body = ContentHelper.GetMidString(contents, "<ul class=\"listCentent\">", "</ul>");
				var bodyItemList = body.Split(new string[] { "</li>" }, StringSplitOptions.None);

				foreach (var item in bodyItemList)
				{
					if (!string.IsNullOrWhiteSpace(item))
					{
						info = new SiteInfo();
						//site name
						info.Name = ContentHelper.GetMidString(item, "class=\"pr10 fz14\">", "</a>");
						info.Url = ContentHelper.GetMidString(item, "class=\"col-gray\">", "</span>");
						info.Alexa = ContentHelper.GetMidInteger(item, alexa + info.Url + "\">", "</a>");
						info.LinkedReversed = ContentHelper.GetMidInteger(item, linkedR + info.Url + "\">", "</a>");
						info.Description = ContentHelper.GetMidString(item, "<p class=\"RtCInfo\">", "</p>");
						info.Score = ContentHelper.GetMidInteger(item, "<span>得分:", "</span>");
						info.CreatedTimeStamp = System.DateTime.Now;
						info.UpdatedTimeStamp = System.DateTime.Now;
						siteDic.Add(info.Url, info);
						SDB.AssistBox.Insert(Constants.TABLE_SITEINFO, info);
						Console.WriteLine("Processing... " + info.Name);
					}
				}
			}
		}

		private static void SearchBy360()
		{
			string pattern = "<a href=hit.php+(.*)+</a>";
			//string temp = "https://www.baidu.com/s?wd=site:{0}&pn={1}";
			string temp = "https://www.so.com/s?q=site:{0}&pn={1}";

			Engine engine = new Engine();
			int pangeCounts = 1; //Max 200
			List<string> toProcessList = new List<string>();
			SitePage sitePage = null;
			List<SitePage> sitePageList = new List<SitePage>();
			foreach (SiteInfo p in SDB.AssistBox.Select<SiteInfo>("from SiteInfo"))
			{
				if (p.Score != 4999)
				{
					continue;
				}
				for (int i = 1; i <= pangeCounts; i = i + 1)
				{
					string searchUrl = string.Format(temp, p.Url, i);

					var contents = _hh1.Get(searchUrl);

					var body = ContentHelper.GetMidString(contents, "<ul id=\"m-result\" class=\"result\">", "<div id=\"side\">");
					if (!string.IsNullOrWhiteSpace(body))
					{
						var bodyItemList = body.Split(new string[] { "</li>" }, StringSplitOptions.None);

						foreach (var item in bodyItemList)
						{
							if (!string.IsNullOrWhiteSpace(item))
							{
								sitePage = new SitePage();
								sitePage.Title = ContentHelper.GetMidString(item, "target=\"_blank\">", "</a></h3>");
								sitePage.Description = ContentHelper.GetMidString(item, "data-sabv=\"1\"> ", "<div");
								if (string.IsNullOrWhiteSpace(sitePage.Description))
								{
									sitePage.Description = ContentHelper.GetMidString(item, "<p class=\"res-desc\">", "</p");
								}
								sitePage.Url = ContentHelper.GetMidString(item, "<cite>", "</cite>");

								if (string.IsNullOrWhiteSpace(sitePage.Url))
								{
									sitePage.Url = ContentHelper.GetMidString(item, "<a href=\"", "\" rel=");
								}
								sitePage.Content = sitePage.Description;
								sitePage.VerifiedCompmany = p.Name;
								sitePage.CreatedTimeStamp = System.DateTime.Now;
								sitePage.ModifiedTimeStamp = System.DateTime.Now;

								if (!string.IsNullOrWhiteSpace(sitePage.Title)
									&& !string.IsNullOrWhiteSpace(sitePage.Description)
									&& !string.IsNullOrWhiteSpace(sitePage.Url))
									sitePageList.Add(sitePage);
							}
						}
					}
				}
			}

			try
			{
				System.IO.Directory.CreateDirectory(Constants.SERVERDATA_PATH);
			}
			catch (UnauthorizedAccessException ex)
			{
				System.IO.Directory.CreateDirectory(Constants.SERVERDATA_PATH);
			}

			foreach (var item in sitePageList)
			{
				item.Id = SDB.SearchBox.NewId();

				SearchResource.InsertSitePage(item, true);
				Console.WriteLine("Processing... " + item.VerifiedCompmany);
			}
		}

		private static void GetAlllinksFromSite()
		{
			List<string> sitePageList = new List<string>();
			foreach (SiteInfo p in SDB.AssistBox.Select<SiteInfo>(string.Format(Constants.LIKESQL, Constants.TABLE_SITEINFO)))
			{
				try
				{
					if (p.Score != 4932) //qq
					{
						continue;
					}

					GrabLinksToDB(sitePageList, p.Url);
				}
				catch (Exception ex)
				{
					TraceLog.Error("Error: " + ex.Message);
				}
			}
		}

		private static void GrabLinksToDB(List<string> sitePageList, string url)
		{
			try
			{
				var contents = _hh1.Get(url);
				Link link = null;

				Regex reg = new Regex(Constants.LINKPATTERN, RegexOptions.IgnoreCase);
				MatchCollection matchList = reg.Matches(contents);
				if (matchList != null)
				{
					foreach (Capture item in matchList)
					{
						var insideUrl = item.Value.Replace('\'', '"');
						var pos = insideUrl.IndexOf("\"");
						link = new Link();
						link.Url = item.Value.Substring(pos + 1, item.Length - pos - 2);
						link.CreatedTimeStamp = System.DateTime.Now;
						link.ModifiedTimeStamp = System.DateTime.Now;
						if (!sitePageList.Contains(link.Url))
						{
							sitePageList.Add(link.Url);
							link.Id = SDB.AssistBox.NewId();

							SDB.AssistBox.Insert(Constants.TABLE_LINK, link);
							Console.WriteLine(string.Format("Host:{0}	Url:{1}", link.Host, link.Url));
						}
					}
				}
			}
			catch (Exception ex)
			{
				TraceLog.Error("Error: " + ex.Message);
			}
		}

		#region public

		private static void ResetProcessLinkConfig()
		{
			// Reset ProcessLinkConfig
			ResetProcessLink(Constants.PROCESSLINKCONFIG_NAME_GRABLINKS);
			ResetProcessLink(Constants.PROCESSLINKCONFIG_NAME_GRABCONTENTS);
		}

		/// <summary>
		/// PROCESSLINKCONFIG_NAME_GRABCONTENTS = 'GrabLinks' or PROCESSLINKCONFIG_NAME_GRABLINKS = 'GrabContents'
		/// </summary>
		/// <param name="name">PROCESSLINKCONFIG_NAME_GRABCONTENTS = 'GrabLinks'
		/// or PROCESSLINKCONFIG_NAME_GRABLINKS = 'GrabContents'
		/// </param>
		private static void ResetProcessLink(string name)
		{
			var configList = SDB.AssistBox.Select<NetServerConfig>(string.Format(Constants.LIKESQL, Constants.TABLE_NETSERVERCONFIG));

			NetServerConfig processLinkConfig = null;
			if (configList != null)
			{
				processLinkConfig = configList.Where(p => p.Name == name).FirstOrDefault();
				processLinkConfig.ProcessedLinkAnchorId = 0;
				SDB.AssistBox.Update(Constants.TABLE_NETSERVERCONFIG, processLinkConfig);
			}
			if (processLinkConfig == null)
			{
				processLinkConfig = new NetServerConfig();
				processLinkConfig.ProcessedLinkAnchorId = 0;
				processLinkConfig.Name = Constants.PROCESSLINKCONFIG_NAME_GRABCONTENTS;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				SDB.AssistBox.Insert(Constants.TABLE_NETSERVERCONFIG, processLinkConfig);
			}
		}

		public static NetServerConfig GetCurrentProcessLinkAnchorID(string name)
		{
			var configList = SDB.AssistBox.Select<NetServerConfig>(string.Format(Constants.LIKESQL, Constants.TABLE_NETSERVERCONFIG));

			NetServerConfig processLinkConfig = null;
			if (configList != null)
			{
				processLinkConfig = configList.Where(p => p.Name == name).FirstOrDefault();
			}
			if (processLinkConfig == null)
			{
				processLinkConfig = new NetServerConfig();
				processLinkConfig.ProcessedLinkAnchorId = 0;
				processLinkConfig.Name = Constants.PROCESSLINKCONFIG_NAME_GRABCONTENTS;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				SDB.AssistBox.Insert(Constants.TABLE_NETSERVERCONFIG, processLinkConfig);
			}
			return processLinkConfig;
		}

		#endregion public
	}
}