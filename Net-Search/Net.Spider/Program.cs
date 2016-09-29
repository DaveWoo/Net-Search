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
		private const string likeSql = "from {0}";
		private const string linkPattern = @"(href)[ ]*=[ ]*[""']*(http)?[^""'#>]+[""']*";

		private static void Main(string[] args)
		{
			//iBoxDB.LocalServer.DB.Root("/tmp/");
			//var text = iBoxDB.NDB.RunALL();
			// Step 1:
			//GetAllSiteInfoFromChinaZ();
			// Step 2:
			//SearchBy360();
			//GetAlllinksFromSite();

			// Step 3: 1
			//ProcessLinksToSitePage();

			ProcessLinksToSitePage();

			// Step 3 : 2
			//GrabLinks();
		}

		private static void GrabLinks()
		{
			long count = 1;
			var configList = SDB.SearchLinkDB.Select<ProcessLinkConfig>(string.Format(likeSql, Constants.TABLE_PROCESSLINKCONFIG));
			List<string> sitePageList = new List<string>();

			ProcessLinkConfig processLinkConfig = null;
			if (configList != null)
			{
				processLinkConfig = configList.Where(p => p.Name == Constants.PROCESSLINKCONFIG_NAME_GRABLINKS).FirstOrDefault();
			}
			if (processLinkConfig == null)
			{
				processLinkConfig = new ProcessLinkConfig();
				processLinkConfig.ProcessedLinkId = 0;
				processLinkConfig.Name = Constants.PROCESSLINKCONFIG_NAME_GRABLINKS;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				SDB.SearchLinkDB.Insert(Constants.TABLE_PROCESSLINKCONFIG, processLinkConfig);
			}
			try
			{
				string likeSqlProcessLink = string.Format("from {0} Id >? order by Id", Constants.TABLE_PROCESSLINK);
				foreach (ProcessLink link in SDB.SearchLinkDB.Select<ProcessLink>(likeSqlProcessLink, processLinkConfig.ProcessedLinkId))
				{
					var sds = SDB.SearchLinkDB.Select<ProcessLink>(likeSqlProcessLink, (long)0);

					GrabLinksToDB(sitePageList, link.Url);
					var sds2 = SDB.SearchLinkDB.Select<ProcessLink>(likeSqlProcessLink, (long)0);

					TraceLog.PrintLn(count++ + " processed: " + link.Url);

					processLinkConfig.ProcessedLinkId = link.Id;
					processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
					SDB.SearchLinkDB.Update(Constants.TABLE_PROCESSLINKCONFIG, processLinkConfig);
				}
			}
			catch (Exception ex)
			{
				TraceLog.Error("Error: " + ex.Message);
			}
			finally
			{
			}
		}

		private static void ProcessLinksToSitePage()
		{
			long count = 1;
			int takeCount = 10;
			ProcessLinkConfig processLinkConfig = GetCurrentProcessLinkID();
			try
			{
				string likeSqlProcessLink = string.Format("from {0} Id >? order by Id", Constants.TABLE_PROCESSLINK);
				var processLinks = SDB.SearchLinkDB.Select<ProcessLink>(likeSqlProcessLink, processLinkConfig.ProcessedLinkId);
				for (int i = 0; i < processLinks.Count(); i = i + takeCount)
				{
					var prepareProcessLinks = processLinks.Where(p => p.Id > processLinkConfig.ProcessedLinkId).Take(takeCount);

					if (prepareProcessLinks != null && prepareProcessLinks.LastOrDefault() != null)
					{
						Parallel.ForEach<ProcessLink>(prepareProcessLinks,
						(e) =>
						{
							TraceLog.PrintLn(count++ + " processing: " + e.Url);
							SearchResource.IndexText(e.Url, false);
							TraceLog.PrintLn(count++ + " processed: " + e.Url);
						});
						processLinkConfig.ProcessedLinkId = prepareProcessLinks.LastOrDefault().Id;
						processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
						SDB.SearchLinkDB.Update(Constants.TABLE_PROCESSLINKCONFIG, processLinkConfig);
					}
				}
			}
			catch (Exception ex)
			{
				TraceLog.Error("Error: " + ex.Message);
			}
		}

		private static ProcessLinkConfig GetCurrentProcessLinkID()
		{
			var configList = SDB.SearchLinkDB.Select<ProcessLinkConfig>(string.Format(likeSql, Constants.TABLE_PROCESSLINKCONFIG));

			ProcessLinkConfig processLinkConfig = null;
			if (configList != null)
			{
				processLinkConfig = configList.Where(p => p.Name == Constants.PROCESSLINKCONFIG_NAME_GRABCONTENTS).FirstOrDefault();
			}
			if (processLinkConfig == null)
			{
				processLinkConfig = new ProcessLinkConfig();
				processLinkConfig.ProcessedLinkId = 0;
				processLinkConfig.Name = Constants.PROCESSLINKCONFIG_NAME_GRABCONTENTS;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				SDB.SearchLinkDB.Insert(Constants.TABLE_PROCESSLINKCONFIG, processLinkConfig);
			}
			return processLinkConfig;
		}

		private static void Test1()
		{
			HttpHelper _hh1 = new HttpHelper();
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
			HttpHelper _hh1 = new HttpHelper();
			string pattern = "<a href=hit.php+(.*)+</a>";
			string temp = "http://top.chinaz.com/all/index_";

			SiteInfo info = null;
			Dictionary<string, SiteInfo> siteDic = new Dictionary<string, SiteInfo>();

			List<string> allPages = new List<string>();
			allPages.Add("http://top.chinaz.com/all/index.html");
			//for (int i = 1; i <= 1727; i++)
			for (int i = 2; i <= 1; i++)
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
					}
				}
			}

			foreach (var item in siteDic)
			{
				//var sds = SDB.SearchDB.Select<SiteInfo>(string.Format("from {0}", Constants.TABLE_SITEINFO));
				//SDB.SearchDB.Insert(Constants.TABLE_SITEINFO, item.Value);
				Console.WriteLine("Processing... " + item.Value.Name);
			}
		}

		private static void SearchBy360()
		{
			HttpHelper _hh1 = new HttpHelper();
			string pattern = "<a href=hit.php+(.*)+</a>";
			//string temp = "https://www.baidu.com/s?wd=site:{0}&pn={1}";
			string temp = "https://www.so.com/s?q=site:{0}&pn={1}";

			Engine engine = new Engine();
			int pangeCounts = 1; //Max 200
			List<string> toProcessList = new List<string>();
			SitePage sitePage = null;
			List<SitePage> sitePageList = new List<SitePage>();
			foreach (SiteInfo p in SDB.SearchDB.Select<SiteInfo>("from SiteInfo"))
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
				item.Id = SDB.SearchDB.NewId();

				SearchResource.InsertSitePage(item, true);
				Console.WriteLine("Processing... " + item.VerifiedCompmany);
			}
		}

		private static void GetAlllinksFromSite()
		{
			List<string> sitePageList = new List<string>();
			foreach (SiteInfo p in SDB.SearchDB.Select<SiteInfo>(string.Format("from {0}", Constants.TABLE_SITEINFO)))
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
				ProcessLink link = null;

				Regex reg = new Regex(linkPattern, RegexOptions.IgnoreCase);
				MatchCollection matchList = reg.Matches(contents);
				if (matchList != null)
				{
					foreach (Capture item in matchList)
					{
						var insideUrl = item.Value.Replace('\'', '"');
						var pos = insideUrl.IndexOf("\"");
						link = new ProcessLink();
						link.Url = item.Value.Substring(pos + 1, item.Length - pos - 2);
						link.CreatedTimeStamp = System.DateTime.Now;
						link.ModifiedTimeStamp = System.DateTime.Now;
						if (!sitePageList.Contains(link.Url))
						{
							sitePageList.Add(link.Url);
							link.Id = SDB.SearchLinkDB.NewId();

							SDB.SearchLinkDB.Insert(Constants.TABLE_PROCESSLINK, link);
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
			var configList = SDB.SearchLinkDB.Select<ProcessLinkConfig>(string.Format(likeSql, Constants.TABLE_PROCESSLINKCONFIG));

			ProcessLinkConfig processLinkConfig = null;
			if (configList != null)
			{
				processLinkConfig = configList.Where(p => p.Name == name).FirstOrDefault();
				processLinkConfig.ProcessedLinkId = 0;
				SDB.SearchLinkDB.Update(Constants.TABLE_PROCESSLINKCONFIG, processLinkConfig);
			}
			if (processLinkConfig == null)
			{
				processLinkConfig = new ProcessLinkConfig();
				processLinkConfig.ProcessedLinkId = 0;
				processLinkConfig.Name = Constants.PROCESSLINKCONFIG_NAME_GRABCONTENTS;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				SDB.SearchLinkDB.Insert(Constants.TABLE_PROCESSLINKCONFIG, processLinkConfig);
			}
		}
	}
}