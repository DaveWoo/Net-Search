using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Net.Api;
using Net.Models;
using Net.Utils;
using Net.Utils.Common;

namespace Spider
{
	internal class Program
	{
		private static HttpHelper httpHelper = new HttpHelper();
		private static Manager manager = new Manager();

		private static void Main(string[] args)
		{
			try
			{
				//string sqlLikeSiteInfo = string.Format("from {0} Url ==?", Constants.TABLE_SITEINFO);
				//var host = "www.baidu.com";
				//var siteInfo = manager.Select<SiteInfo>(sqlLikeSiteInfo, host);

				Log.Info("Begin running...");
				//Log.Warn("Warn running...");
				//Log.Fatal("Fatal running...");
				//iBoxDB.LocalServer.DB.Root("/tmp/");
				//var text = iBoxDB.NDB.RunALL();

				// Step 1:1 Get all basic url and site name
				//Site.GetAllSiteInfoFromChinaZ();
				var SiteInfoList = manager.Select<SiteInfo>();
				var processSiteInfoConfig = Site.GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_CHINAZINDEX);

				// Step 1:2 Get links from basic url
				//Site.GetBasicLinks();
				var processLinkConfig = Site.GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABBASICLINKS);
				var linkList = manager.Select<Link>();
				// Step 2:
				//SearchBy360();

				// Step 3: 1
				//Site.GrabLinks();
				//var linkList = manager.Select<Link>();

				// Step 3 : 2
				//Site.GrabLinksContent();
				var pages = manager.Select<SitePage>();

				// Step 4 : Add ad
				//Site.AddAd("www.abcdef.com",
				//	"这是您的第一份免费广告",
				//	"这是您的第一份免费广告,我们将竭诚为您服务",
				//	"广告",
				//	"新闻;news");

				//SearchWords();

				CountLinked();

			}
			catch (Exception ex)
			{
				Log.Error("Program", ex);
			}
		}

		private static void SearchWords()
		{
			Words word = new Words();
			word.IP = "11,1,2,4";
			word.Name = "name";
			word.CreatedTimeStamp = System.DateTime.Now;
			manager.Create<Words>(word);
			var sds = manager.Select<Words>();
		}

		private static void CountLinked()
		{
			Linked linked = new Linked();
			linked.IP = "11,1,2,4";
			linked.Url = "http://baidu.com";
			linked.CreatedTimeStamp = System.DateTime.Now;
			manager.Create<Linked>(linked);
			var sds = manager.Select<Linked>();
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
				var contents = httpHelper.Get(page);
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
	}
}