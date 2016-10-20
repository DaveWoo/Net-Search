using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Api;
using System.IO;
using System.Reflection;
using Net.Utils.Common;
using Net.Utils;

namespace Net.Api.UnitTest
{
	[TestClass]
	public class ManagerTest
	{
		Manager manager;

		[TestInitialize]
		public void Init()
		{
			manager = new Manager();
			Log.Loginfo = log4net.LogManager.GetLogger(Assembly.GetAssembly(typeof(ManagerTest)), "ManagerTest");
		}

		[TestMethod]
		public void TestAnalyzeContentsLink()
		{
			try
			{
				Log.Info("Begin running TestAnalyzeContentsLink...");
				var url = "http://news.qq.com/";
				var contents = File.ReadAllText("TestPageContent.txt");
				manager.AnalyzeContentsLink(url, contents);
				Log.Info("End running TestAnalyzeContentsLink...");
			}
			catch (Exception)
			{

				throw;
			}
		}

		[TestMethod]
		public void TestGrabLinksToDB()
		{
			try
			{
				Log.Info("Begin running TestGrabLinksToDB...");
				var url = "http://news.163.com/";
				manager.GrabLinksToDB(url);
				Log.Info("End running TestGrabLinksToDB...");
			}
			catch (Exception)
			{

				throw;
			}
		}

		[TestMethod]
		public void TestAddPageByUrl()
		{
			try
			{
				Log.Info("Begin running TestGrabLinksToDB...");
				var url = "http://sports.163.com/nba/";
				string contents = manager.AddPageByUrl(url, false);
				var all = manager.SelectAllSitePage();
				var single = manager.SelectSitePage(string.Format(Constants.SQLLIKEURL, Constants.TABLE_SITEPAGE), url);
				Log.Info("End running TestGrabLinksToDB...");
			}
			catch (Exception)
			{

				throw;
			}


		}
	}
}
