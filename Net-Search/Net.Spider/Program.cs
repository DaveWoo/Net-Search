using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
		private static ManagerClient client = new ManagerClient();
		private static int currentGrabedSiteCount = 0;
		private static object objectLock = new object();
		private static object objectLockBasicLink = new object();
		private static object objectLockGrabContents = new object();

		private static void Main(string[] args)
		{
			try
			{
				Log.Loginfo = log4net.LogManager.GetLogger(Assembly.GetAssembly(typeof(Program)), "Spider.Program");
				var SelectSiteInfoByDefault = client.SelectSiteInfoByDefault();
				var SelectLinkByDefault = client.SelectLinkByDefault();
				var SelectSitePageByDefault = client.SelectSitePageByDefault();

				Run();
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
				//client.AddSiteAD("www.abcdef.com",
				//	"这是您的第一份免费广告",
				//	"这是您的第一份免费广告,我们将竭诚为您服务",
				//	"广告",
				//	"新闻;news");

				//var sdf = client.SelectAllSiteAD();
				//SearchWords();

				//CountLinked();
			}
			catch (Exception ex)
			{
				Log.Error("Program", ex);
			}
		}

		private static void Run()
		{
			Task taskGetBasicSiteInfo = Task.Run(() =>
			{
				Log.Info("GetAllSiteInfoFromChinaZ start");
				GetAllSiteInfoFromChinaZ();
				Log.Info("GetAllSiteInfoFromChinaZ end");
			});

			Task taskGetBasicLinks = Task.Run(() =>
			{
				Log.Info("GetBasicLinks start");
				GetBasicLinks();
				Log.Info("GetBasicLinks end");
			});

			Task taskGrabLinks = Task.Run(() =>
			{
				Log.Info("GrabLinks start");
				GrabLinks();
				Log.Info("GrabLinks end");
			});

			Task taskGrabLinksContent = Task.Run(() =>
			{
				Log.Info("GrabLinksContent start");
				GrabLinksContent();
				Log.Info("GrabLinksContent end");
			});
			taskGetBasicSiteInfo.Wait();
			taskGetBasicLinks.Wait();
			taskGrabLinks.Wait();
			taskGrabLinksContent.Wait();
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

		public static void GetBasicLinks()
		{
			NetServerConfig processLinkConfig = client.GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABBASICLINKS);
			try
			{
				var SiteInfoList = client.SelectSiteInfoByDefault();
				var toBeProcessedLinks = SiteInfoList.OrderBy(p => p.Id).Where(p => p.Id > processLinkConfig.ProcessedLinkAnchorId);

				for (int i = 0; i < toBeProcessedLinks.Count(); i = i + Constants.TAKECOUNTBASICLINKS)
				{
					var prepareProcessLinks = toBeProcessedLinks.Where(p => p.Id > processLinkConfig.ProcessedLinkAnchorId).Take(Constants.TAKECOUNTBASICLINKS);

					if (prepareProcessLinks != null && prepareProcessLinks.Count() > 0)
					{
						Log.Info("Grab basic link start: " + i);
						Parallel.ForEach(prepareProcessLinks,
						(e) =>
						{
							client.CreateSiteLinkFromUrlAsync(e.Url);
							lock (objectLockGrabContents)
							{
								Log.Info(currentGrabedSiteCount++ + "Grabbed basic link name: " + e.Name);
							}
						});
						processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Id;
						processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
						client.UpdateSiteSeverConfig(processLinkConfig);

						Log.Info("Grab basic link end: " + i);
					}
				}
			}
			catch (Exception ex)
			{
				processLinkConfig.ProcessedLinkAnchorId += 1;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				client.UpdateSiteSeverConfig(processLinkConfig);
				Log.Error(ex.Message, ex);
			}
		}

		public static void GrabLinks()
		{
			NetServerConfig processLinkConfig = client.GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABLINKS);

			try
			{
				string likeSqlProcessLink = string.Format("from {0} Id >? order by Id", Constants.TABLE_LINK);
				var toBeProcessedLinks = client.SelectSiteLink(likeSqlProcessLink, processLinkConfig.ProcessedLinkAnchorId);
				GrabLinksAsync(processLinkConfig, toBeProcessedLinks);
			}
			catch (Exception ex)
			{
				processLinkConfig.ProcessedLinkAnchorId += 1;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				client.UpdateSiteSeverConfig(processLinkConfig);
				Log.Error(ex.Message, ex);
			}
		}

		private static void GrabLinksAsync(NetServerConfig processLinkConfig, Link[] toBeProcessedLinks)
		{
			for (int i = 0; i < toBeProcessedLinks.Count(); i = i + Constants.TAKECOUNT)
			{
				var prepareProcessLinks = toBeProcessedLinks.Where(p => p.Id > processLinkConfig.ProcessedLinkAnchorId).Take(Constants.TAKECOUNT);

				if (prepareProcessLinks != null && prepareProcessLinks.LastOrDefault() != null)
				{
					Parallel.ForEach<Link>(prepareProcessLinks,
					(e) =>
					{
						Log.Info(processLinkConfig.ProcessedLinkAnchorId + " link processing: " + e.Url);
						client.CreateSiteLinkFromUrlAsync(e.Url);
						Log.Info(processLinkConfig.ProcessedLinkAnchorId + " link processed: " + e.Url);
					});
				}

				processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Id;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				client.UpdateSiteSeverConfig(processLinkConfig);
			}
		}

		public static void GrabLinksContent()
		{
			NetServerConfig processLinkConfig = client.GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABCONTENTS);
			try
			{
				string likeSqlProcessLink = string.Format("from {0} Id >? order by Id", Constants.TABLE_LINK);
				var processLinks = client.SelectSiteLink(likeSqlProcessLink, processLinkConfig.ProcessedLinkAnchorId);
				for (int i = 0; i < processLinks.Count(); i = i + Constants.TAKECOUNT)
				{
					var prepareProcessLinks = processLinks.Where(p => p.Id > processLinkConfig.ProcessedLinkAnchorId).Take(Constants.TAKECOUNT);

					if (prepareProcessLinks != null && prepareProcessLinks.LastOrDefault() != null)
					{
						Parallel.ForEach<Link>(prepareProcessLinks,
						(e) =>
						{
							var contents = client.CreateSitePageFromUrlAsync(e.Url);
							Log.Info(e.Id + " grabbing contents: " + e.Url);
						});
						processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Id;
						processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
						client.UpdateSiteSeverConfig(processLinkConfig);
					}
				}
			}
			catch (Exception ex)
			{
				processLinkConfig.ProcessedLinkAnchorId += 1;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				client.UpdateSiteSeverConfig(processLinkConfig);
				Log.Error(ex.Message, ex);
			}
		}

		public static void GetAllSiteInfoFromChinaZ()
		{
			NetServerConfig processLinkConfig = client.GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_CHINAZINDEX);

			string temp = "http://top.chinaz.com/all/index_";
			Dictionary<long, string> allPages = new Dictionary<long, string>();
			allPages.Add(1, "http://top.chinaz.com/all/index.html");
			// for (long i = processLinkConfig.ProcessedLinkAnchorId + 2; i <= 1727; i++)
			for (long i = 2; i <= 1727; i++)
			{
				allPages.Add(i, temp + i + ".html");
			}

			var toBeProcessedLinks = allPages.Where(page => page.Key > processLinkConfig.ProcessedLinkAnchorId);

			for (int i = 0; i < toBeProcessedLinks.Count(); i = i + Constants.TAKECOUNTCHINAZ)
			{
				var prepareProcessLinks = toBeProcessedLinks.Where(p => p.Key > processLinkConfig.ProcessedLinkAnchorId).Take(Constants.TAKECOUNTCHINAZ);

				if (prepareProcessLinks != null && prepareProcessLinks.Count() > 0)
				{
					Log.Info("Grab chinaz link start: " + i);
					Parallel.ForEach<KeyValuePair<long, string>>(prepareProcessLinks,
					(e) =>
					{
						client.CreateSiteInfoFromUrlAsync(e.Value);
						Log.Info("Grabbed link name: " + e.Value);
					});
					processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Key;
					processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
					client.UpdateSiteSeverConfig(processLinkConfig);
					Log.Info("Grab chinaz link end: " + i);
				}
			}
		}
	}
}