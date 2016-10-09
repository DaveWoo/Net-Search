using Net.Models;
using Net.Utils;
using Net.Utils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Net.Api
{
    public class Site
    {
        private static Manager manager = new Manager();
        private static HttpHelper httpHelper = new HttpHelper();
        private static int currentGrabedSiteCount = 0;
        private static object objectLock = new object();
        private static int currentGrabedBaiscLinksCount = 0;
        private static object objectLockBasicLink = new object();
        private static object objectLockGrabContents = new object();

        public static void GetBasicLinks()
        {
            NetServerConfig processLinkConfig = GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABBASICLINKS);
            var SiteInfoList = manager.Select<SiteInfo>();
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
                            Log.Info(currentGrabedSiteCount++ + "Grabed basic link name: " + e.Name);
                        }
                    });
                    processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Id;
                    processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
                    manager.Update<NetServerConfig>(processLinkConfig);
                    Log.Info("Grab basic link end: " + i);
                }
            }
        }

        public static void AddAd(string url, string title, string description, string company, string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentNullException("tag");
            AD adSitePage = new AD();
            adSitePage.Id = SDB.ADBox.NewId();
            adSitePage.Title = title;
            adSitePage.Description = description;

            adSitePage.Url = url;

            adSitePage.VerifiedSiteName = company;
            adSitePage.CreatedTimeStamp = System.DateTime.Now;
            adSitePage.ModifiedTimeStamp = System.DateTime.Now;
            adSitePage.Tag = tag;
            manager.Create<AD>(adSitePage);
            Log.Info("Add ad: " + adSitePage.Title);
        }

        public static void GrabLinks()
        {
            var configList = manager.Select<NetServerConfig>();
            List<string> sitePageList = new List<string>();

            NetServerConfig processLinkConfig = GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABLINKS);

            try
            {
                string likeSqlProcessLink = string.Format("from {0} Id >? order by Id", Constants.TABLE_LINK);
                var processLinks = manager.Select<Link>(likeSqlProcessLink, processLinkConfig.ProcessedLinkAnchorId);
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
                    manager.Update<NetServerConfig>(processLinkConfig);
                }
            }
            catch (Exception ex)
            {
                processLinkConfig.ProcessedLinkAnchorId += 1;
                processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
                manager.Update<NetServerConfig>(processLinkConfig);
                Log.Error("Error: " + ex.Message);
            }
        }

        public static void GrabLinksContent()
        {
            NetServerConfig processLinkConfig = GetCurrentProcessLinkAnchorID(Constants.PROCESSLINKCONFIG_NAME_GRABCONTENTS);
            try
            {
                string likeSqlProcessLink = string.Format("from {0} Id >? order by Id", Constants.TABLE_LINK);
                var processLinks = manager.Select<Link>(likeSqlProcessLink, processLinkConfig.ProcessedLinkAnchorId);
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
                                Log.Info(e.Id + " grabed contents: " + e.Url);
                            }
                        });
                        processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Id;
                        processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
                        manager.Update<NetServerConfig>(processLinkConfig);
                    }
                }
            }
            catch (Exception ex)
            {
                processLinkConfig.ProcessedLinkAnchorId += 1;
                processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
                manager.Update<NetServerConfig>(processLinkConfig);
                Log.Error("Error: " + ex.Message);
            }
        }

        public static void GetAllSiteInfoFromChinaZ()
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
            if (manager.Select<SiteInfo>() != null)
            {
                currentGrabedSiteCount = manager.Select<SiteInfo>().Count();
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
                        Log.Info("Grabed link name: " + e.Value);
                    });
                    processLinkConfig.ProcessedLinkAnchorId = prepareProcessLinks.LastOrDefault().Key;
                    processLinkConfig.ModifiedTimeStamp = System.DateTime.Now;
                    manager.Update<NetServerConfig>(processLinkConfig);
                    Log.Info("Grab chinaz link end: " + i);
                }
            }
        }

        private static void GrabChinaZ(KeyValuePair<long, string> page)
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
                        bool isSucceed = manager.Create<SiteInfo>(info);
                        lock (objectLock)
                        {
                            Log.Info("Grabing site Index: " + currentGrabedSiteCount++ + "Name: " + info.Name);
                        }
                    }
                }
            }
        }

        public static void SearchBy360()
        {
            string pattern = "<a href=hit.php+(.*)+</a>";
            //string temp = "https://www.baidu.com/s?wd=site:{0}&pn={1}";
            string temp = "https://www.so.com/s?q=site:{0}&pn={1}";

            Engine engine = new Engine();
            int pangeCounts = 1; //Max 200
            List<string> toProcessList = new List<string>();
            SitePage sitePage = null;
            List<SitePage> sitePageList = new List<SitePage>();
            // foreach (SiteInfo p in manager.Select<SiteInfo>("from SiteInfo"))
            foreach (SiteInfo p in manager.Select<SiteInfo>())
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
                System.IO.Directory.CreateDirectory(Constants.SERVERDATA_PATH);
            }
            catch (UnauthorizedAccessException ex)
            {
                System.IO.Directory.CreateDirectory(Constants.SERVERDATA_PATH);
            }

            foreach (var item in sitePageList)
            {
                item.Id = SDB.SitePageBox.NewId();

                SearchResource.InsertSitePage(item, true);
                Log.Info("Processing... " + item.VerifiedSiteName);
            }
        }

        private static void GrabLinksToDB(List<string> sitePageList, string url)
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
                                var isExist = manager.Select<Link>().Where(p => p.Url == link.Url);

                                if (isExist == null || isExist.Count() == 0)
                                {
                                    var isSuccessed = manager.Create<Link>(link);
                                    var Links = manager.Select<Link>();
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
                //if (linkList.Count > 0)
                //{
                //    var isSuccessed = manager.Create<Link>(linkList.ToArray());
                //    Log.Info(string.Format("Grabed Link count: {0} url: {0}", currentGrabedBaiscLinksCount++, url));
                //    var Links = manager.Select<Link>();
                //}
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
        public static NetServerConfig GetCurrentProcessLinkAnchorID(string name)
        {
            // var configList = manager.Select<NetServerConfig>(string.Format(Constants.LIKESQL, Constants.TABLE_NETSERVERCONFIG));
            var configList = manager.Select<NetServerConfig>();

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
                manager.Create<NetServerConfig>(processLinkConfig);
            }
            return processLinkConfig;
        }
    }
}