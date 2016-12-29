using System;
using System.Linq;
using System.Web.Mvc;
using Net.HostServer.ServiceReference;

namespace Net.HostServer.Controllers
{
	public class DashboardController : Controller
	{
		protected DateTime Begin;
		private ManagerClient client;

		// GET: Dashboard
		public ActionResult Index()
		{
			ViewBag.Begin = DateTime.Now;
			client = new ManagerClient();
			GenerateSummaryInfo();
			return View();
		}

		private void GenerateSummaryInfo()
		{
			var processLinks = client.SelectSiteLinkByDefault();
			ViewBag.ProcessLinksCount = processLinks.Count().ToString();
			var siteInfo = client.SelectSiteInfoByDefault();
			ViewBag.SiteInfoCount = siteInfo.Count().ToString();
			var sitePage = client.SelectSitePageByDefault();
			ViewBag.SitePageCount = sitePage.Count().ToString();

			var words = client.SelectWordsByDefault();
			ViewBag.WordsCount = words.Count().ToString();

			var linked = client.SelectLinkedByDefault();
			ViewBag.LinkedCount = linked.Count().ToString();
		}
	}
}