using iBoxDB.LocalServer;
using Net.Models;
using Net.Utils.Common;
using System;

namespace Net.Api
{
    public class SDB
    {
        private static object lockObject = new object();

        static SDB()
        {
            lock (lockObject)
            {
                InitSitePageBox();
                InitSiteInfoBox();
                InitLinkBox();
                InitNetServerConfigBox();
                InitADBox();
                InitWordsBox();
            }
        }

        public static DB.AutoBox SitePageBox { get; internal set; }
        public static DB.AutoBox SiteInfoBox { get; internal set; }
        public static DB.AutoBox LinkBox { get; internal set; }
        public static DB.AutoBox ADBox { get; internal set; }
        public static DB.AutoBox NetServerConfigBox { get; internal set; }
        public static DB.AutoBox WordsBox { get; internal set; }

        public bool IsInit { get; set; }

        //SitePage
        private static void InitSitePageBox()
        {
            try
            {
                DB server = InitServer(1);
                server.GetConfig().EnsureTable<SitePage>(Constants.TABLE_SITEPAGE, Constants.TABLE_FIELD_ID);
                server.GetConfig().EnsureIndex<SitePage>(Constants.TABLE_SITEPAGE, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");

                if (SitePageBox == null)
                    SitePageBox = server.Open();
            }
            catch (Exception ex)
            {
                Log.Error("Error: InitSitePageBox", ex);
            }
        }
        //siteInfoBox
        private static void InitSiteInfoBox()
        {
            try
            {
                DB server = InitServer(2);

                server.GetConfig().EnsureTable<SiteInfo>(Constants.TABLE_SITEINFO, Constants.TABLE_FIELD_ID);
                //server.GetConfig().EnsureUpdateIncrementIndex<SiteInfo>(Constants.TABLE_SITEINFO, Constants.TABLE_FIELD_ID);
                server.GetConfig().EnsureIndex<SiteInfo>(Constants.TABLE_SITEINFO, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");
                if (SiteInfoBox == null)
                    SiteInfoBox = server.Open();
            }
            catch (Exception ex)
            {
                Log.Error("Error: InitSiteInfoBox", ex);
            }
            finally
            {
            }
        }

        //link
        private static void InitLinkBox()
        {
            try
            {
                DB server = InitServer(3);
                server.GetConfig().EnsureTable<Link>(Constants.TABLE_LINK, Constants.TABLE_FIELD_ID);
                server.GetConfig().EnsureIndex<Link>(Constants.TABLE_LINK, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");
                if (LinkBox == null)
                    LinkBox = server.Open();
            }
            catch (Exception ex)
            {
                Log.Error("Error: InitLinkBox", ex);
            }
        }

        //ad
        private static void InitADBox()
        {
            try
            {
                DB server = InitServer(4);

                server.GetConfig().EnsureTable<SitePage>(Constants.TABLE_AD, Constants.TABLE_FIELD_ID);
                server.GetConfig().EnsureIndex<SitePage>(Constants.TABLE_AD, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");
                if (ADBox == null)
                    ADBox = server.Open();
            }
            catch (Exception ex)
            {
                Log.Error("Error: InitADBox", ex);
            }
        }

        //NetServerConfigBox
        private static void InitNetServerConfigBox()
        {
            try
            {
                DB server = InitServer(5);
                server.GetConfig().EnsureTable<NetServerConfig>(Constants.TABLE_NETSERVERCONFIG, "Name");
                if (NetServerConfigBox == null)
                    NetServerConfigBox = server.Open();
            }
            catch (Exception ex)
            {
                Log.Error("Error: InitNetServerConfigBox", ex);
            }
        }

        //NetServerConfigBox
        private static void InitWordsBox()
        {
            try
            {
                DB server = InitServer(6);
                server.GetConfig().EnsureTable<Words>(Constants.TABLE_WORDS, "Name");
                if (WordsBox == null)
                    WordsBox = server.Open();
            }
            catch (Exception ex)
            {
                Log.Error("Error: InitWordsBox", ex);
            }
        }

        private static void CreateServerPath(String path)
        {
            try
            {
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                    Log.Info("DBPath=" + path);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error("", ex);
            }
        }

        private static DB InitServer(long dbID)
        {
            return InitServer(dbID, Constants.SERVERDATA_PATH, false);
        }
        private static DB InitServer(long dbID, string path, bool isVM)
        {
            CreateServerPath(path);
            DB.Root(path);

            DB server = new DB(dbID);
            if (isVM)
            {
                server.GetConfig().DBConfig.CacheLength = server.GetConfig().DBConfig.MB(16);
            }
            server.GetConfig().DBConfig.SwapFileBuffer = (int)server.GetConfig().DBConfig.MB(4);
            server.GetConfig().DBConfig.FileIncSize = (int)server.GetConfig().DBConfig.MB(16);
            new Engine().Config(server.GetConfig().DBConfig);
            return server;
        }

        public static void Close()
        {
            if (SitePageBox != null)
            {
                SitePageBox.GetDatabase().Close();
            }
            SitePageBox = null;

            if (SiteInfoBox != null)
            {
                SiteInfoBox.GetDatabase().Close();
            }
            SiteInfoBox = null;

            if (LinkBox != null)
            {
                LinkBox.GetDatabase().Close();
            }
            LinkBox = null;

            if (NetServerConfigBox != null)
            {
                NetServerConfigBox.GetDatabase().Close();
            }
            NetServerConfigBox = null;

            if (ADBox != null)
            {
                ADBox.GetDatabase().Close();
            }
            ADBox = null;

            if (WordsBox != null)
            {
                WordsBox.GetDatabase().Close();
            }
            WordsBox = null;

            Log.Info("DBClosed");
        }
    }
}