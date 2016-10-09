using iBoxDB.LocalServer;
using Net.Models;
using Net.Utils.Common;
using System;
using System.Threading;

namespace Net.Api
{
    public class SDB
    {
        private static DB.AutoBox sitePageBox = null;
        private static DB.AutoBox siteInfoBox = null;
        private static DB.AutoBox linkBox = null;
        private static DB.AutoBox adBox = null;
        private static DB.AutoBox netServerConfigBox = null;

        private static object lockObject = new object();

        static SDB()
        {
          
        }

        public static DB.AutoBox SitePageBox
        {
            get
            {
                if (sitePageBox == null)
                {
                    lock (lockObject)
                    {
                        InitSitePageBox(Constants.SERVERDATA_PATH, false);
                    }
                }
                return sitePageBox;
            }
        }

        public static DB.AutoBox SiteInfoBox
        {
            get
            {
                if (siteInfoBox == null)
                {
                    InitSiteInfoBox(Constants.SERVERDATA_PATH, false);
                }
                return siteInfoBox;
            }
        }

        public static DB.AutoBox LinkBox
        {
            get
            {
                if (linkBox == null)
                {
                    InitLinkBox(Constants.SERVERDATA_PATH, false);
                }
                return linkBox;
            }
        }

        public static DB.AutoBox ADBox
        {
            get
            {
                if (adBox == null)
                {
                    InitADBox(Constants.SERVERDATA_PATH, false);
                }
                return adBox;
            }
        }

        public static DB.AutoBox NetServerConfigBox
        {
            get
            {
                if (netServerConfigBox == null)
                {
                    InitNetServerConfigBox(Constants.SERVERDATA_PATH, false);
                }
                return netServerConfigBox;
            }
        }

        public bool IsInit { get; set; }

        //SitePage
        private static void InitSitePageBox(String path, bool isVM)
        {
            try
            {
                CreateServerPath(path);
                DB server = InitServer(1, path, isVM);
                server.GetConfig().EnsureTable<SitePage>(Constants.TABLE_SITEPAGE, Constants.TABLE_FIELD_ID);
                server.GetConfig().EnsureIndex<SitePage>(Constants.TABLE_SITEPAGE, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");

                if (sitePageBox == null)
                    sitePageBox = server.Open();
            }
            catch (Exception ex)
            {
                Log.Error("Error: InitSitePageBox", ex);
            }
        }

        //NetServerConfigBox
        private static void InitNetServerConfigBox(String path, bool isVM)
        {
            try
            {
                CreateServerPath(path);
                DB server = InitServer(2, path, isVM);
                server.GetConfig().EnsureTable<NetServerConfig>(Constants.TABLE_NETSERVERCONFIG, "Name");
                if (netServerConfigBox == null)
                    netServerConfigBox = server.Open();
            }
            catch (Exception ex)
            {
                Log.Error("Error: InitNetServerConfigBox", ex);
            }
        }

        //link
        private static void InitLinkBox(String path, bool isVM)
        {
            try
            {
                CreateServerPath(path);

                DB.Root(path);

                CreateServerPath(path);
                DB server = InitServer(3, path, isVM);
                server.GetConfig().EnsureTable<Link>(Constants.TABLE_LINK, "Url");
                server.GetConfig().EnsureIndex<Link>(Constants.TABLE_LINK, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");
                if (linkBox == null)
                    linkBox = server.Open();
            }
            catch (Exception ex)
            {
                Log.Error("Error: InitLinkBox", ex);
            }
        }

        //siteInfoBox
        private static void InitSiteInfoBox(String path, bool isVM)
        {
            try
            {
                Monitor.Enter(lockObject);

                CreateServerPath(path);
                DB server = InitServer(4, path, isVM);

                server.GetConfig().EnsureTable<SiteInfo>(Constants.TABLE_SITEINFO, Constants.TABLE_FIELD_ID);
                //server.GetConfig().EnsureUpdateIncrementIndex<SiteInfo>(Constants.TABLE_SITEINFO, Constants.TABLE_FIELD_ID);
                server.GetConfig().EnsureIndex<SiteInfo>(Constants.TABLE_SITEINFO, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");
                if (siteInfoBox == null)
                    siteInfoBox = server.Open();

            }
            catch (Exception ex)
            {
                Log.Error("Error: InitSiteInfoBox", ex);
            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }

        //ad
        private static void InitADBox(String path, bool isVM)
        {
            try
            {
                CreateServerPath(path);
                DB server = InitServer(5, path, isVM);

                server.GetConfig().EnsureTable<SitePage>(Constants.TABLE_AD, Constants.TABLE_FIELD_ID);
                server.GetConfig().EnsureIndex<SitePage>(Constants.TABLE_AD, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");
                if (sitePageBox == null)
                    adBox = server.Open();
            }
            catch (Exception ex)
            {
                Log.Error("Error: InitADBox", ex);
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

        private static DB InitServer(long dbID, string path, bool isVM)
        {
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
            if (sitePageBox != null)
            {
                sitePageBox.GetDatabase().Close();
            }
            sitePageBox = null;

            if (siteInfoBox != null)
            {
                siteInfoBox.GetDatabase().Close();
            }
            siteInfoBox = null;

            if (linkBox != null)
            {
                linkBox.GetDatabase().Close();
            }
            linkBox = null;

            if (netServerConfigBox != null)
            {
                netServerConfigBox.GetDatabase().Close();
            }
            netServerConfigBox = null;

            if (adBox != null)
            {
                adBox.GetDatabase().Close();
            }
            adBox = null;

            Log.Info("DBClosed");
        }
    }
}