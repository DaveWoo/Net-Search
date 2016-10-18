using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Net.Models;
using Net.Utils;
using Net.Utils.Common;

namespace Net.Api
{
	public partial class Manager : IManager
	{
		static Manager()
		{
			Log.Loginfo = log4net.LogManager.GetLogger(Assembly.GetAssembly(typeof(Manager)), "Net.Api");
		}

		#region Fields

		private static HttpHelper httpHelper = new HttpHelper();
		private static int currentGrabedSiteCount = 0;
		private static object objectLock = new object();
		private static int currentGrabedBaiscLinksCount = 0;
		private static object objectLockBasicLink = new object();
		private static object objectLockGrabContents = new object();

		#endregion Fields

		#region Create

		public bool CreateSitePage(SitePage value)
		{
			return Insert<SitePage>(value);
		}

		public bool CreateSiteInfo(SiteInfo value)
		{
			return Insert<SiteInfo>(value);
		}

		public bool CreateSiteServerConfig(NetServerConfig value)
		{
			return Insert<NetServerConfig>(value);
		}

		public bool CreateSiteLink(Link value)
		{
			return Insert<Link>(value);
		}

		public bool CreateSiteAD(SiteAD value)
		{
			return Insert<SiteAD>(value);
		}

		public bool AddSiteAD(string url, string title, string content, string company, string tag)
		{
			if (string.IsNullOrWhiteSpace(tag))
				throw new ArgumentNullException("tag");
			SiteAD adSitePage = new SiteAD();
			adSitePage.Id = SDB.ADBox.NewId();
			adSitePage.Title = title;
			adSitePage.Content = content;
			adSitePage.Url = url;
			adSitePage.VerifiedSiteName = company;
			adSitePage.CreatedTimeStamp = System.DateTime.Now;
			adSitePage.ModifiedTimeStamp = System.DateTime.Now;
			adSitePage.Tag = tag;

			return Insert<SiteAD>(adSitePage);
		}

		public bool CreateSiteSearchWords(Words value)
		{
			return Insert<Words>(value);
		}

		public bool CreateSiteClickedLink(Linked value)
		{
			return Insert<Linked>(value);
		}

		#endregion Create

		#region Update

		public bool UpdateSitePage(SitePage value)
		{
			return Update<SitePage>(value);
		}

		public bool UpdateSiteInfo(SiteInfo value)
		{
			return Update<SiteInfo>(value);
		}

		public bool UpdateSiteSeverConfig(NetServerConfig value)
		{
			return Update<NetServerConfig>(value);
		}

		public bool UpdateSiteLink(Link value)
		{
			return Update<Link>(value);
		}

		public bool UpdateSiteAD(SiteAD value)
		{
			return Update<SiteAD>(value);
		}

		public bool UpdateSiteAD(string url, string title, string content, string company, string tag)
		{
			if (string.IsNullOrWhiteSpace(tag))
				throw new ArgumentNullException("tag");
			SiteAD adSitePage = new SiteAD();
			adSitePage.Id = SDB.ADBox.NewId();
			adSitePage.Title = title;
			adSitePage.Content = content;
			adSitePage.Url = url;
			adSitePage.VerifiedSiteName = company;
			adSitePage.ModifiedTimeStamp = System.DateTime.Now;
			adSitePage.Tag = tag;
			return Update<SiteAD>(adSitePage);
		}

		public bool UpdateSiteWords(Words value)
		{
			return Update<Words>(value);
		}

		public bool UpdateSiteLinked(Linked value)
		{
			return Update<Linked>(value);
		}

		#endregion Update

		#region Select

		#region Select All

		public IEnumerable<SitePage> SelectAllSitePage()
		{
			var result = Select<SitePage>();
			return result;
		}

		public IEnumerable<SiteInfo> SelectAllSiteInfo()
		{
			return Select<SiteInfo>();
		}

		public IEnumerable<NetServerConfig> SelectAllSiteServerConfig()
		{
			return Select<NetServerConfig>();
		}

		public IEnumerable<Link> SelectAllLink()
		{
			return Select<Link>();
		}

		public IEnumerable<SiteAD> SelectAllSiteAD()
		{
			return Select<SiteAD>();
		}

		public IEnumerable<Words> SelectAllWords()
		{
			return Select<Words>();
		}

		public IEnumerable<Linked> SelectAllLinked()
		{
			return Select<Linked>();
		}

		#endregion Select All

		#region Select sql like

		public IEnumerable<SitePage> SelectSitePage(string sqlLike, object args)
		{
			return Select<SitePage>(sqlLike, args);
		}

		public IEnumerable<SiteInfo> SelectSiteInfo(string sqlLike, object args)
		{
			return Select<SiteInfo>(sqlLike, args);
		}

		public IEnumerable<NetServerConfig> SelectSiteServerConfig(string sqlLike, object args)
		{
			return Select<NetServerConfig>(sqlLike, args);
		}

		public IEnumerable<Link> SelectSiteLink(string sqlLike, object args)
		{
			return Select<Link>(sqlLike, args);
		}

		public IEnumerable<SiteAD> SelectSiteAD(string sqlLike, object args)
		{
			return Select<SiteAD>(sqlLike, args);
		}

		public IEnumerable<Words> SelectSiteWords(string sqlLike, object args)
		{
			return Select<Words>(sqlLike, args);
		}

		public IEnumerable<Linked> SelectSiteLinked(string sqlLike, object args)
		{
			return Select<Linked>(sqlLike, args);
		}

		#endregion Select sql like

		#endregion Select

		#region Delete

		public bool DeleteSitePage(string url)
		{
			return Delete<SitePage>(url);
		}

		public bool DeleteSiteInfo(string url)
		{
			return Delete<SiteInfo>(url);
		}

		public bool DeleteSiteServerConfig(string name)
		{
			return Delete<NetServerConfig>(name);
		}

		public bool DeleteLink(string url)
		{
			return Delete<Link>(url);
		}

		public bool DeleteSiteAD(string url)
		{
			return Delete<SiteAD>(url);
		}

		public bool DeleteSiteWords(string url)
		{
			return Delete<Words>(url);
		}

		public bool DeleteSiteLinked(string url)
		{
			return Delete<Linked>(url);
		}

		#endregion Delete

		public List<SitePage> GetPages(string searchValue)
		{
			List<SitePage> pageList = new List<SitePage>();
			using (var box = SDB.SitePageBox.Cube())
			{
				var results = SearchResource.Engine.SearchDistinct(box, searchValue).OrderBy(p => p.Position);
				if (results != null)
				{
					foreach (KeyWord kw in results)
					{
						long id = kw.ID;
						id = SitePage.RankDownId(id);
						SitePage p = box[Constants.TABLE_SITEPAGE, id].Select<SitePage>();
						//todo
						if (p == null)
							continue;
						p.keyWord = kw;

						#region Clac page body contents

						string content = string.Empty;
						if (p.keyWord == null)
						{
							content = p.Description + "...";
							if (p.Content != null)
							{
								content += p.Content.ToString();
							}
						}
						else if (p.Id != p.keyWord.ID)
						{
							content = p.Description;
							if (content.Length < 20)
							{
								content += p.GetRandomContent();
							}
						}
						else
						{
							var c1 = p.Content != null ? p.Content.ToString() : p.Description;
							content = SearchResource.Engine.getDesc(c1, p.keyWord, 80);
							if (content.Length < 100)
							{
								content += p.GetRandomContent();
							}
							if (content.Length < 100)
							{
								content += p.Description;
							}
							if (content.Length > 200)
							{
								content = content.Substring(0, 200) + "..";
							}
						}
						p.Content = content;

						#endregion Clac page body contents

						pageList.Add(p);
						if (pageList.Count >= Constants.PAGECOUNTLIMIT)
						{
							break;
						}
					}
				}
			}

			return pageList;
		}

		public int GetRelatedResutsCount(string searchValue)
		{
			using (var box = SDB.SitePageBox.Cube())
			{
				var results = SearchResource.Engine.SearchDistinct(box, searchValue);
				if (results != null)
				{
					return results.Count();
				}
			}

			return -1;
		}

		#region Site manager

		public void GetBasicLinks()
		{
			NetServerConfig processLinkConfig = GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABBASICLINKS);
			var SiteInfoList = Select<SiteInfo>();
			List<string> sitePageList = new List<string>();
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
						GrabLinksToDB(sitePageList, e.Url);
						lock (objectLockGrabContents)
						{
							Log.Info(currentGrabedSiteCount++ + "Grabbed basic link name: " + e.Name);
						}
					});
					processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Id;
					processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
					Update<NetServerConfig>(processLinkConfig);
					Log.Info("Grab basic link end: " + i);
				}
			}
		}

		public void GrabLinks()
		{
			var configList = Select<NetServerConfig>();
			List<string> sitePageList = new List<string>();

			NetServerConfig processLinkConfig = GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABLINKS);

			try
			{
				string likeSqlProcessLink = string.Format("from {0} Id >? order by Id", Constants.TABLE_LINK);
				var processLinks = Select<Link>(likeSqlProcessLink, processLinkConfig.ProcessedLinkAnchorId);
				for (int i = 0; i < processLinks.Count(); i = i + Constants.TAKECOUNT)
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
							Log.Info(processLinkConfig.ProcessedLinkAnchorId + " link processing: " + e.Url);
							GrabLinksToDB(sitePageList, e.Url);
							Log.Info(processLinkConfig.ProcessedLinkAnchorId + " link processed: " + e.Url);
						});
					}

					processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Id;
					processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
					Update<NetServerConfig>(processLinkConfig);
				}
			}
			catch (Exception ex)
			{
				processLinkConfig.ProcessedLinkAnchorId += 1;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				Update<NetServerConfig>(processLinkConfig);
				Log.Error("Error: " + ex.Message);
			}
		}

		public void GrabLinksContent()
		{
			NetServerConfig processLinkConfig = GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABCONTENTS);
			try
			{
				string likeSqlProcessLink = string.Format("from {0} Id >? order by Id", Constants.TABLE_LINK);
				var processLinks = Select<Link>(likeSqlProcessLink, processLinkConfig.ProcessedLinkAnchorId);
				for (int i = 0; i < processLinks.Count(); i = i + Constants.TAKECOUNT)
				{
					var prepareProcessLinks = processLinks.Where(p => p.Id > processLinkConfig.ProcessedLinkAnchorId).Take(Constants.TAKECOUNT);

					if (prepareProcessLinks != null && prepareProcessLinks.LastOrDefault() != null)
					{
						Parallel.ForEach<Link>(prepareProcessLinks,
						(e) =>
						{
							string contents = SearchResource.IndexText(e.Url, false);
							if (contents == "temporarily unreachable")
							{
								Log.Warn(contents + "Url: " + e.Url);
							}
							else
							{
								Log.Info(e.Id + " grabbed contents: " + e.Url);
							}
						});
						processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Id;
						processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
						Update<NetServerConfig>(processLinkConfig);
					}
				}
			}
			catch (Exception ex)
			{
				processLinkConfig.ProcessedLinkAnchorId += 1;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				Update<NetServerConfig>(processLinkConfig);
				Log.Error(ex.Message, ex);
			}
		}

		public void GetAllSiteInfoFromChinaZ()
		{
			NetServerConfig processLinkConfig = GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_CHINAZINDEX);

			string temp = "http://top.chinaz.com/all/index_";
			Dictionary<long, string> allPages = new Dictionary<long, string>();
			allPages.Add(1, "http://top.chinaz.com/all/index.html");
			// for (long i = processLinkConfig.ProcessedLinkAnchorId + 2; i <= 1727; i++)
			for (long i = 2; i <= 1727; i++)
			{
				allPages.Add(i, temp + i + ".html");
			}

			var toBeProcessedLinks = allPages.Where(page => page.Key > processLinkConfig.ProcessedLinkAnchorId);
			if (Select<SiteInfo>() != null)
			{
				currentGrabedSiteCount = Select<SiteInfo>().Count();
			}
			for (int i = 0; i < toBeProcessedLinks.Count(); i = i + Constants.TAKECOUNTCHINAZ)
			{
				var prepareProcessLinks = toBeProcessedLinks.Where(p => p.Key > processLinkConfig.ProcessedLinkAnchorId).Take(Constants.TAKECOUNTCHINAZ);

				if (prepareProcessLinks != null && prepareProcessLinks.Count() > 0)
				{
					Log.Info("Grab chinaz link start: " + i);
					Parallel.ForEach<KeyValuePair<long, string>>(prepareProcessLinks,
					(e) =>
					{
						GrabChinaZ(e);
						Log.Info("Grabbed link name: " + e.Value);
					});
					processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Key;
					processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
					Update<NetServerConfig>(processLinkConfig);
					Log.Info("Grab chinaz link end: " + i);
				}
			}
		}

		private void GrabChinaZ(KeyValuePair<long, string> page)
		{
			string alexa = "http://alexa.chinaz.com/?domain=";
			string linkedR = "http://outlink.chinaz.com/?h=";

			var contents = httpHelper.Get(page.Value);
			var body = ContentHelper.GetMidString(contents, "<ul class=\"listCentent\">", "</ul>");
			if (body != null)
			{
				var bodyItemList = body.Split(new string[] { "</li>" }, StringSplitOptions.None);

				foreach (var item in bodyItemList)
				{
					if (!string.IsNullOrWhiteSpace(item))
					{
						var info = new SiteInfo();
						//site name
						info.Id = SDB.SiteInfoBox.NewId();
						info.Name = ContentHelper.GetMidString(item, "class=\"pr10 fz14\">", "</a>");
						info.Url = ContentHelper.GetMidString(item, "class=\"col-gray\">", "</span>");
						info.Alexa = ContentHelper.GetMidInteger(item, alexa + info.Url + "\">", "</a>");
						info.LinkedReversed = ContentHelper.GetMidInteger(item, linkedR + info.Url + "\">", "</a>");
						info.Description = ContentHelper.GetMidString(item, "<p class=\"RtCInfo\">", "</p>");
						info.Score = ContentHelper.GetMidInteger(item, "<span>得分:", "</span>");
						info.CreatedTimeStamp = System.DateTime.Now;
						info.UpdatedTimeStamp = System.DateTime.Now;
						bool isSucceed = Insert<SiteInfo>(info);
						lock (objectLock)
						{
							Log.Info("Grabbing site Index: " + currentGrabedSiteCount++ + "Name: " + info.Name);
						}
					}
				}
			}
		}

		public void SearchBy360()
		{
			string temp = "https://www.so.com/s?q=site:{0}&pn={1}";

			Engine engine = new Engine();
			int pangeCounts = 1; //Max 200
			List<string> toProcessList = new List<string>();
			SitePage sitePage = null;
			List<SitePage> sitePageList = new List<SitePage>();
			foreach (SiteInfo p in Select<SiteInfo>())
			{
				if (p.Score != 4999)
				{
					continue;
				}
				for (int i = 1; i <= pangeCounts; i = i + 1)
				{
					string searchUrl = string.Format(temp, p.Url, i);

					var contents = httpHelper.Get(searchUrl);

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
								sitePage.VerifiedSiteName = p.Name;
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
				System.IO.Directory.CreateDirectory(Constants.SERVERDATA_FULLPATH);
			}
			catch (UnauthorizedAccessException ex)
			{
				System.IO.Directory.CreateDirectory(Constants.SERVERDATA_FULLPATH);
			}

			foreach (var item in sitePageList)
			{
				item.Id = SDB.SitePageBox.NewId();

				SearchResource.InsertSitePage(item, true);
				Log.Info("Processing... " + item.VerifiedSiteName);
			}
		}

		private void GrabLinksToDB(List<string> sitePageList, string url)
		{
			try
			{
				var contents = httpHelper.Get(url);
				Link link = null;
				List<Link> linkList = new List<Link>();
				Regex reg = new Regex(Constants.LINKPATTERN, RegexOptions.IgnoreCase);
				MatchCollection matchList = reg.Matches(contents);
				if (matchList != null)
				{
					foreach (Capture item in matchList)
					{
						try
						{
							var insideUrl = item.Value.Replace('\'', '"');
							var pos = insideUrl.IndexOf("\"");
							link = new Link();
							link.Id = SDB.LinkBox.NewId();
							link.Url = item.Value.Substring(pos + 1, item.Length - pos - 2);
							link.CreatedTimeStamp = System.DateTime.Now;
							link.ModifiedTimeStamp = System.DateTime.Now;
							if (link.Url.Length < Constants.URLLENGTH)
							{
								var isExist = Select<Link>().Where(p => p.Url == link.Url);

								if (isExist == null || isExist.Count() == 0)
								{
									var isSuccessed = Insert<Link>(link);
									var Links = Select<Link>();
									lock (objectLockBasicLink)
									{
										Log.Info(string.Format("Index:{2}   Host:{0}	Url:{1}", link.Host, link.Url, currentGrabedBaiscLinksCount++));
									}
								}
							}
						}
						catch (Exception ex)
						{
							Log.Error("GrabLinksToDB: " + url, ex);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error: " + ex.Message, ex);
			}
		}

		/// <summary>
		/// PROCESSLINKCONFIG_NAME_GRABCONTENTS = 'GrabLinks' or PROCESSLINKCONFIG_NAME_GRABLINKS = 'GrabContents'
		/// </summary>
		/// <param name="name">PROCESSLINKCONFIG_NAME_GRABCONTENTS = 'GrabLinks'
		/// or PROCESSLINKCONFIG_NAME_GRABLINKS = 'GrabContents'
		/// </param>
		public NetServerConfig GetCurrentProcessLinkAnchorID(string name)
		{
			var configList = Select<NetServerConfig>();

			NetServerConfig processLinkConfig = null;
			if (configList != null)
			{
				processLinkConfig = configList.Where(p => p.Name == name).FirstOrDefault();
			}
			if (processLinkConfig == null)
			{
				processLinkConfig = new NetServerConfig();
				processLinkConfig.ProcessedLinkAnchorId = 0;
				processLinkConfig.Name = name;
				processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
				//manager.Insert(Constants.TABLE_NETSERVERCONFIG, processLinkConfig);
				Insert<NetServerConfig>(processLinkConfig);
			}
			return processLinkConfig;
		}

		public List<String> GetDiscover()
		{
			List<String> discoveries = new List<String>();
			using (var box = SDB.SitePageBox.Cube())
			{
				foreach (String skw in SearchResource.Engine.discover(box, 'a', 'z', 4,
															   '\u2E80', '\u9fa5', 1))
				{
					discoveries.Add(skw);
				}
			}

			return discoveries;
		}

		#endregion Site manager
	}
}