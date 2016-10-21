using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsQuery;
using CsQuery.Web;
using Net.Models;
using Net.Utils;
using Net.Utils.Common;

namespace Net.Api
{
	public partial class Manager : IManager
	{
		public static ConcurrentQueue<string> searchList = new ConcurrentQueue<string>();
		public static ConcurrentQueue<string> urlList = new ConcurrentQueue<string>();
		public readonly static Engine Engine;

		static Manager()
		{
			Engine = new Engine();
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

		public bool CreateSitePage(SitePage value, bool isDeleteCurrentThenAddNew)
		{
			Log.Info("CreateSitePage start");
			var isSucceed = false;
			if (value == null || value.Content == null || string.IsNullOrWhiteSpace(value.Url) || string.IsNullOrWhiteSpace(value.Title))
			{
				throw new ArgumentNullException("value is invalid");
			}

			if (isDeleteCurrentThenAddNew)
			{
				foreach (SitePage sitePage in SDB.SitePageBox.Select<SitePage>(string.Format(Constants.SQLLIKEURL, Constants.TABLE_SITEPAGE), value.Url))
				{
					Engine.indexTextNoTran(SDB.SitePageBox, Constants.COMMITCOUNT, sitePage.Id, sitePage.Content.ToString(), true);
					Engine.indexTextNoTran(SDB.SitePageBox, Constants.COMMITCOUNT, sitePage.RankUpId(), sitePage.RankUpDescription(), true);
					SDB.SitePageBox.Delete(Constants.TABLE_SITEPAGE, sitePage.Id);
				}
			}
			var isExist = Select<SitePage>(string.Format(Constants.SQLLIKEURL, Constants.TABLE_SITEPAGE), value.Url);

			if (isExist == null || isExist.Count() == 0)
			{
				value.Id = SDB.SitePageBox.NewId();
				isSucceed = Insert<SitePage>(value);
				if (isSucceed)
				{
					Log.Info("index start");
					Engine.indexTextNoTran(SDB.SitePageBox, Constants.COMMITCOUNT, value.Id, value.Content.ToString(), false);
					Log.Info("index start 1");
					Engine.indexTextNoTran(SDB.SitePageBox, Constants.COMMITCOUNT, value.RankUpId(), value.RankUpDescription(), false);
					Log.Info("index end");
				}
			}
			return isSucceed;
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
			var isExist = Select<Link>(string.Format(Constants.SQLLIKEURL, Constants.TABLE_LINK), value.Url);

			if (isExist == null || isExist.Count() == 0)
			{
				return Insert<Link>(value);
			}
			return false;
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

		public IEnumerable<SitePage> SelectSitePageByDefault()
		{
			var result = Select<SitePage>();
			return result;
		}

		public IEnumerable<SiteInfo> SelectSiteInfoByDefault()
		{
			return Select<SiteInfo>();
		}

		public IEnumerable<NetServerConfig> SelectSiteServerConfigByDefault()
		{
			return Select<NetServerConfig>();
		}

		public IEnumerable<Link> SelectSiteLinkByDefault()
		{
			return Select<Link>();
		}

		public IEnumerable<SiteAD> SelectSiteADByDefault()
		{
			return Select<SiteAD>();
		}

		public IEnumerable<Words> SelectWordsByDefault()
		{
			return Select<Words>();
		}

		public IEnumerable<Linked> SelectLinkedByDefault()
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
			foreach (SitePage p in SDB.SitePageBox.Select<SitePage>(string.Format(Constants.SQLLIKEURL, Constants.TABLE_SITEPAGE), url))
			{
				Engine.indexTextNoTran(SDB.SitePageBox, Constants.COMMITCOUNT, p.Id, p.Content.ToString(), true);
				Engine.indexTextNoTran(SDB.SitePageBox, Constants.COMMITCOUNT, p.RankUpId(), p.RankUpDescription(), true);
				return Delete<SitePage>(url);
				//SDB.SitePageBox.Delete(Constants.TABLE_SITEPAGE, p.Id);
			}

			return false;
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
				var results = Engine.SearchDistinct(box, searchValue).OrderBy(p => p.Position);
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
							content = Engine.getDesc(c1, p.keyWord, 80);
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
				var results = Engine.SearchDistinct(box, searchValue);
				if (results != null)
				{
					return results.Count();
				}
			}

			return -1;
		}

		#region Site manager

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
				CreateSitePage(item, true);
				Log.Info("Processing... " + item.VerifiedSiteName);
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

		public List<string> GetDiscover()
		{
			List<string> discoveries = new List<string>();
			using (var box = SDB.SitePageBox.Cube())
			{
				foreach (string skw in Engine.discover(box, 'a', 'z', 4,
															   '\u2E80', '\u9fa5', 1))
				{
					discoveries.Add(skw);
				}
			}

			return discoveries;
		}

		public string CreateSitePageByName(string name, bool isDelete)
		{
			try
			{
				Log.Info("AddPageByUrl");
				string url = GetUrl(name);
				SitePage p = SitePage.Get(url);
				Log.Info("generated model SitePage");

				if (p == null)
				{
					return "temporarily unreachable";
				}
				else
				{
					CreateSitePage(p, isDelete);
					Log.Info("stored model to db");
					urlList.Enqueue(p.Url);
					while (urlList.Count > 3)
					{
						string t;
						urlList.TryDequeue(out t);
					}
					return p.Url;
				}
			}
			catch (Exception ex)
			{
				Log.Error("IndexText: " + name, ex);
			}
			return string.Empty;
		}

		public int CreateSitePageFromUrl(string url)
		{
			try
			{
				if (url == null || url.Length > Constants.MAX_URL_LENGTH || url.Length < Constants.Min_URL_LENGTH)
				{
					return -1;
				}

				var result = CQ.CreateFromUrlAsync(url, SuccessResponseSitePageFromUrl, FailResponseSitePageFromUrl);
				return result;
			}
			catch (Exception ex)
			{
				Log.Error("CreateSitePageFromUrl: " + url, ex);
				return -1;
			}
		}

		public void CreateSiteInfoFromUrl(string url)
		{
			try
			{
				string alexa = "http://alexa.chinaz.com/?domain=";
				string linkedR = "http://outlink.chinaz.com/?h=";

				var contents = httpHelper.Get(url);
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
			catch (Exception ex)
			{
				Log.Error("CreateSitePageFromUrl: " + url, ex);
			}
		}

		public void CreateSiteLinkFromUrl(string url)
		{
			try
			{
				var contents = httpHelper.Get(url);
				AnalyzeContentsLink(url, contents);
			}
			catch (Exception ex)
			{
				Log.Error("CreateSiteInfoFromUrl: " + url, ex);
			}
		}

		public void AnalyzeContentsLink(string url, string contents)
		{
			Link link = null;
			List<Link> linkList = new List<Link>();
			Regex reg = new Regex(Constants.LINKPATTERN, RegexOptions.IgnoreCase);
			MatchCollection matchList = reg.Matches(contents);
			var value = string.Empty;

			if (matchList != null)
			{
				foreach (Capture item in matchList)
				{
					try
					{
						value = item.Value;
						var insideUrl = item.Value.Replace('\'', '"');
						var pos = insideUrl.IndexOf("\"");
						var contenetUrl = item.Value.Substring(pos + 1, item.Length - pos - 2);

						#region Handle non-valid url

						if (contenetUrl.EndsWith(".css")
							|| contenetUrl.EndsWith(".js")
							|| contenetUrl.EndsWith(".jpg")
							|| contenetUrl.EndsWith(".png")
							|| contenetUrl.EndsWith(".jpeg")
							|| contenetUrl.EndsWith(".gif")
							|| contenetUrl.EndsWith(".mp3")
							|| contenetUrl.EndsWith(".mp4")
							|| contenetUrl.EndsWith(".avi")
							|| contenetUrl.EndsWith(".rm")
							|| contenetUrl.EndsWith(".rmvb")
							|| contenetUrl.EndsWith(".mpg")
							|| contenetUrl.EndsWith(".mpeg")
							|| contenetUrl.EndsWith(".exe")
							|| contenetUrl.EndsWith(".cmd")
							|| contenetUrl.EndsWith(".rbs")
							|| contenetUrl.EndsWith(".bat")
							|| contenetUrl.EndsWith(".doc")
							|| contenetUrl.EndsWith(".tmp")
							|| contenetUrl.EndsWith(".xls")
							|| contenetUrl.EndsWith(".mdf")
							|| contenetUrl.EndsWith(".mid")
							|| contenetUrl.EndsWith(".zip")
							|| contenetUrl.EndsWith(".rar")
							|| contenetUrl.EndsWith(".zp")
								|| contenetUrl.EndsWith(".mid")
							|| contenetUrl.EndsWith(".zip")
							|| contenetUrl.EndsWith(".rar")
							|| contenetUrl.EndsWith(".zp")
							|| contenetUrl.EndsWith(".ps1"))
						{
							continue;
						}

						if (contenetUrl.StartsWith("javascript"))
						{
							continue;
						}
						else if (contenetUrl.StartsWith("/"))
						{
							Uri uri = new Uri(url);
							contenetUrl = uri.Host + contenetUrl;
						}

						#endregion Handle non-valid url

						link = new Link();
						link.Id = SDB.LinkBox.NewId();
						link.Url = contenetUrl;
						link.CreatedTimeStamp = System.DateTime.Now;
						link.ModifiedTimeStamp = System.DateTime.Now;
						if (link.Url.Length < Constants.URLLENGTH && link.Url.Length > 10)
						{
							var isSuccessed = CreateSiteLink(link);
							lock (objectLockBasicLink)
							{
								if (isSuccessed)
									Log.Info(string.Format("Index:{2}   Host:{0}	Url:{1}", link.Host, link.Url, currentGrabedBaiscLinksCount++));
							}
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Format("GrabLinksToDB: url: {0}		value: {1}", url, value), ex);
					}
				}
			}
		}

		#region Private

		private string GetUrl(string name)
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

		private void SuccessResponseSitePageFromUrl(ICsqWebResponse response)
		{
			try
			{
				var sitePage = GenerateSitePageModel(response);

				if (sitePage != null)
				{
					CreateSitePage(sitePage, false);
					Log.Info("stored model to db");
					urlList.Enqueue(sitePage.Url);
					while (urlList.Count > 3)
					{
						string t;
						urlList.TryDequeue(out t);
					}
				}
				return;
			}
			catch (Exception ex)
			{
				Log.Error("SuccessResponseSitePageFromUrl: ", ex);
			}
		}

		private SitePage GenerateSitePageModel(ICsqWebResponse response)
		{
			SitePage page = null;
			try
			{
				page = new SitePage();
				page.Url = response.Url;
				CQ doc = response.Dom;

				//Console.WriteLine(doc.Html());
				doc["script"].Remove();
				doc["Script"].Remove();

				doc["style"].Remove();
				doc["Style"].Remove();

				doc["textarea"].Remove();
				doc["Textarea"].Remove();

				doc["noscript"].Remove();
				doc["Noscript"].Remove();
				Log.Info("Remove");

				page.Title = doc["title"].Text();
				if (page.Title == null)
				{
					page.Title = doc["Title"].Text();
				}
				if (page.Title == null)
				{
					page.Title = response.Url;
				}
				page.Title = page.Title.Trim();
				if (page.Title.Length < 2)
				{
					page.Title = response.Url;
				}
				if (page.Title.Length > Constants.TITLE_LENGTH)
				{
					page.Title = page.Title.Substring(0, Constants.TITLE_LENGTH);
				}
				page.Title = page.Title.Replace("<", " ")
					.Replace(">", " ").Replace("$", " ");
				doc["title"].Remove();
				doc["Title"].Remove();

				page.Description = doc["meta[name='description']"].Attr("content");
				if (page.Description == null)
				{
					page.Description = doc["meta[name='Description']"].Attr("content");
				}
				if (page.Description == null)
				{
					page.Description = "";
				}
				if (page.Description.Length > Constants.DESCRIPTION_LENGTH)
				{
					page.Description = page.Description.Substring(0, Constants.DESCRIPTION_LENGTH);
				}
				page.Description = page.Description.Replace("<", " ")
					.Replace(">", " ").Replace("$", " ");

				string content = doc.Text().Replace("　", " ");
				content = Regex.Replace(content, "\t|\r|\n|�|<|>", " ");
				content = Regex.Replace(content, "\\$", " ");
				content = Regex.Replace(content, "\\s+", " ");
				content = content.Trim();

				if (content.Length < 50)
				{
					return null;
				}
				if (content.Length > 5000)
				{
					content = content.Substring(0, 5000);
				}

				page.Content = content + " " + page.Url;
				page.CreatedTimeStamp = System.DateTime.Now;
				page.ModifiedTimeStamp = System.DateTime.Now;
			}
			catch (Exception ex)
			{
				Log.Error("GenerateSitePageModel: ", ex);
				page = null;
			}
			return page;
		}

		private void FailResponseSitePageFromUrl(ICsqWebResponse response)
		{
			Log.Error(string.Format("url:{0},	status:{1} ", response.Url, response.HttpStatus));
		}
		#endregion

		#endregion Site manager
	}
}