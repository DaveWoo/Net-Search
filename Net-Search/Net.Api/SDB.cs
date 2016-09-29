using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading;
using CsQuery;
using iBoxDB.LocalServer;
using Net.Models;

namespace Net.Api
{
	public class SDB
	{
		private static DB.AutoBox searchDB = null;
		private static object lockObject = new object();

		public static DB.AutoBox SearchDB
		{
			get
			{
				if (searchDB == null)
				{
					lock (lockObject)
					{
						InitSearchDB(Constants.SERVERDATA_PATH, false);
					}
				}
				return searchDB;
			}
		}

		private static DB.AutoBox searchLinkDB = null;

		public static DB.AutoBox SearchLinkDB
		{
			get
			{
				if (searchLinkDB == null)
				{
					InitSearchLinkDB(Constants.SERVERDATA_PATH, false);
				}
				return searchLinkDB;
			}
		}

		public bool IsInit { get; set; }

		private static void InitSearchDB(String path, bool isVM)
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
				server.GetConfig().EnsureTable<SiteInfo>(Constants.TABLE_SITEINFO, "Url");
				//server.GetConfig().EnsureIndex<SiteInfo>("siteInfo", true, "url(" + SitePage.MAX_URL_LENGTH + ")");
				if (searchDB == null)
					searchDB = server.Open();

			}
			catch (Exception ex)
			{

			}

		}

		private static void InitSearchLinkDB(String path, bool isVM)
		{
			try
			{
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
				server.GetConfig().EnsureTable<ProcessLink>(Constants.TABLE_PROCESSLINK, "Id");
				server.GetConfig().EnsureIndex<ProcessLink>(Constants.TABLE_PROCESSLINK, true, "Url(" + SitePage.MAX_URL_LENGTH + ")");
				server.GetConfig().EnsureTable<ProcessLinkConfig>(Constants.TABLE_PROCESSLINKCONFIG, "Name");

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