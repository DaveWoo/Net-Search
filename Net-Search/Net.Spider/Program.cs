using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using Net.Api;
using Net.Models;
using Net.Utils;
using Net.Utils.Common;
using Spider.ServiceReference;

namespace Spider
{
	internal class Program
	{
		private static HttpHelper httpHelper = new HttpHelper();
		static ManagerClient client = new ManagerClient();

		private static void Main(string[] args)
		{
			try
			{
				//var pageList = client.GetPages("新闻");
				//SitePage sitePage = new SitePage();
				//sitePage.Id = 1234567;
				//sitePage.Title = "Hello";
				//sitePage.Description = "Description";
				//sitePage.Url = "http://www.ac.com";

				//sitePage.Content = sitePage.Description;
				//sitePage.CreatedTimeStamp = System.DateTime.Now;
				//sitePage.ModifiedTimeStamp = System.DateTime.Now;
				//var sds2 = client.CreateSitePage(sitePage);

				//string sqlLikeSiteInfo = string.Format("from {0} Url ==?", Constants.TABLE_SITEINFO);
				//var host = "www.baidu.com";
				//var siteInfo = manager.Select<SiteInfo>(sqlLikeSiteInfo, host);

				Log.Info("Begin running...");
				//Log.Warn("Warn running...");
				//Log.Fatal("Fatal running...");
				//iBoxDB.LocalServer.DB.Root("/tmp/");
				//var text = iBoxDB.NDB.RunALL();

				// Step 1:1 Get all basic url and site name
				//client.GetAllSiteInfoFromChinaZ();
				//var SiteInfoList = client.SelectAllSiteInfo();
				//var processSiteInfoConfig = client.GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_CHINAZINDEX);

				// Step 1:2 Get links from basic url
				//client.GetBasicLinks();
				//var processLinkConfig = client.GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABBASICLINKS);
				//var linkList = client.SelectAllLink();
				// Step 2:
				//SearchBy360();

				// Step 3: 1
				//client.GrabLinks();
				//var linkList = manager.Select<Link>();

				// Step 3 : 2
				//client.GrabLinksContent();
				//var pages = client.SelectAllSitePage();

				// Step 4 : Add ad
				client.AddAd("www.abcdef.com",
					"这是您的第一份免费广告",
					"这是您的第一份免费广告,我们将竭诚为您服务",
					"广告",
					"新闻;news");

				var sdf = client.SelectAllSiteAD();
				SearchWords();

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
			//todo
		}

		private static void CountLinked()
		{
			Linked linked = new Linked();
			linked.IP = "11,1,2,4";
			linked.Url = "http://baidu.com";
			linked.CreatedTimeStamp = System.DateTime.Now;
			//todo
			//manager.Create<Linked>(linked);
			//var sds = manager.Select<Linked>();
		}
	}
}