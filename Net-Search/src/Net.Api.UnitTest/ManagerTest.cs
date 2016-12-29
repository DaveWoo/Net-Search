using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.Api;
using System.IO;
using System.Reflection;
using Net.Utils.Common;
using Net.Utils;
using System.Linq;
using Net.Models;

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
				manager.CreateSiteLinkFromUrl(url);
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
				var url = "http://tech.163.com/";
				var contents = manager.CreateSitePageFromUrl(url);
				var all = manager.SelectSitePageByDefault();
				var single = manager.SelectSitePage(string.Format(Constants.SQLLIKEURL, Constants.TABLE_SITEPAGE), url);

				Assert.AreEqual(single.FirstOrDefault().Url, url);
				Log.Info("End running TestGrabLinksToDB...");
			}
			catch (Exception)
			{

				throw;
			}


		}

		[TestMethod]
		public void CreateSiteInfoFromUrl()
		{
			try
			{
				Log.Info("Begin running CreateSiteInfoFromUrl...");
				var url = "http://tech.163.com/";
				manager.CreateSiteInfoFromUrl(url);
				var all = manager.SelectSiteInfoByDefault();
				System.Threading.Thread.Sleep(15000);
				var single = manager.SelectSitePage(string.Format(Constants.SQLLIKEURL, Constants.TABLE_SITEPAGE), url);

				Log.Info("End running CreateSiteInfoFromUrl...");
			}
			catch (Exception)
			{

				throw;
			}


		}

		[TestMethod]
		public void EditSiteAD()
		{
			try
			{
				var isSucceed = manager.UpdateSiteAD("www.soso.com",
						"这是您的第一份免费广告5",
						"这是您的第一份免费广告,我们将竭诚为您服务5",
						"广告",
						false,
						"新闻;news");

				var dsd = manager.SelectSiteADByDefault();

				Log.Info("End running CreateSiteInfoFromUrl...");
			}
			catch (Exception)
			{

				throw;
			}
		}

		[TestMethod]
		public void DeleteSiteAD()
		{
			try
			{
				var dsd = manager.SelectSiteADByDefault();
				var dsd3 = manager.DeleteSiteAD(dsd.FirstOrDefault().Id);

				Log.Info("End running CreateSiteInfoFromUrl...");
			}
			catch (Exception)
			{

				throw;
			}
		}


		[TestMethod]
		public void CreateLinked()
		{
			try
			{
				Log.Info("Begin running CreateLinked...");

				Linked linked = new Linked();
				linked.IP = HttpHelper.GetIp();
				linked.Url = "http://news.cctv.com/special/jujiao/2016/529/index.shtml";
				linked.CreatedTimeStamp = System.DateTime.Now;
				var single = manager.CreateSiteClickedLink(linked);
				var sds = manager.SelectLinkedByDefault();
				Log.Info("End running CreateLinked...");
			}
			catch (Exception)
			{

				throw;
			}


		}

		[TestMethod]
		public void GetPages()
		{
			try
			{
				Log.Info("Begin running GetPages...");

				var single = manager.GetPages("新闻");

				Log.Info("End running GetPages...");
			}
			catch (Exception)
			{

				throw;
			}
		}

		[TestMethod]
		public void SelectWordsByDefault()
		{
			try
			{
				Log.Info("Begin running SelectSiteADByDefault...");

				var single = manager.SelectWordsByDefault();

				Log.Info("End running SelectWordsByDefault...");
			}
			catch (Exception)
			{
				throw;
			}
		}

	}
}
