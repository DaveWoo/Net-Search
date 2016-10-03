using System;
using iBoxDB.LocalServer;
using Net.Models;

namespace Net.Api
{
	public class SDB
	{
		private static DB.AutoBox searchDB = null;
		private static object lockObject = new object();

		public static DB.AutoBox SearchBox
		{
			get
			{
				if (searchDB == null)
				{
					lock (lockObject)
					{
						InitSearchBox(Constants.SERVERDATA_PATH, false);
					}
				}
				return searchDB;
			}
		}

		private static DB.AutoBox searchLinkDB = null;

		public static DB.AutoBox AssistBox
		{
			get
			{
				if (searchLinkDB == null)
				{
					InitAssistBox(Constants.SERVERDATA_PATH, false);
				}
				return searchLinkDB;
			}
		}

		public bool IsInit { get; set; }

		private static void InitSearchBox(String path, bool isVM)
		{
			try
			{
				Console.WriteLine("DBPath=" + path);

				DB.Root(path);

				DB server = new DB(1);
				if (isVM)
				{
					server.GetConfig().DBConfig.CacheLength = server.GetConfig().DBConfig.MB(16);
				}
				server.GetConfig().DBConfig.SwapFileBuffer = (int)server.GetConfig().DBConfig.MB(4);
				server.GetConfig().DBConfig.FileIncSize = (int)server.GetConfig().DBConfig.MB(16);
				new Engine().Config(server.GetConfig().DBConfig);
				server.GetConfig().EnsureTable<SitePage>(Constants.TABLE_SITEPAGE, "Id");
				server.GetConfig().EnsureIndex<SitePage>(Constants.TABLE_SITEPAGE, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");
				//server.GetConfig().EnsureIndex<SitePage>(Constants.TABLE_SITEPAGE, true, "Url");

				if (searchDB == null)
					searchDB = server.Open();
			}
			catch (Exception ex)
			{
			}
		}

		private static void InitAssistBox(String path, bool isVM)
		{
			try
			{
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                catch (UnauthorizedAccessException ex)
                {
                }
				Console.WriteLine("DBPath=" + path);

				DB.Root(path);

				DB server = new DB(2);
				if (isVM)
				{
					server.GetConfig().DBConfig.CacheLength = server.GetConfig().DBConfig.MB(16);
				}
				server.GetConfig().DBConfig.SwapFileBuffer = (int)server.GetConfig().DBConfig.MB(4);
				server.GetConfig().DBConfig.FileIncSize = (int)server.GetConfig().DBConfig.MB(16);
				new Engine().Config(server.GetConfig().DBConfig);
				server.GetConfig().EnsureTable<Link>(Constants.TABLE_LINK, "Id");
				server.GetConfig().EnsureIndex<Link>(Constants.TABLE_LINK, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");
				server.GetConfig().EnsureTable<NetServerConfig>(Constants.TABLE_NETSERVERCONFIG, "Name");
				server.GetConfig().EnsureTable<SiteInfo>(Constants.TABLE_SITEINFO, "Url");
				//server.GetConfig().EnsureIndex<SiteInfo>("siteInfo", true, "url(" + SitePage.MAX_URL_LENGTH + ")");
				server.GetConfig().EnsureTable<SitePage>(Constants.TABLE_AD, "Id");
				server.GetConfig().EnsureIndex<SitePage>(Constants.TABLE_AD, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");

				searchLinkDB = server.Open();
			}
			catch (Exception ex)
			{
			}
		}

		public static void Close()
		{
			if (searchDB != null)
			{
				searchDB.GetDatabase().Close();
			}
			searchDB = null;

			if (searchLinkDB != null)
			{
				searchLinkDB.GetDatabase().Close();
			}
			searchLinkDB = null;
			Console.WriteLine("DBClosed");
		}
	}
}