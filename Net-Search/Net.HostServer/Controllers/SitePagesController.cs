using System;
using System.Linq;
using System.Web.Mvc;
using Net.HostServer.ServiceReference;
using Net.Models;

namespace Net.HostServer.Controllers
{
	public class SitePagesController : Controller
	{
		private ManagerClient client = new ManagerClient();

		// GET: SitePages
		public ActionResult Index(string searchString)
		{
			var pages = client.SelectSitePageByDefault();
			if (!String.IsNullOrWhiteSpace(searchString))
			{
				return View(pages.Where(p => p.Title.IndexOf(searchString) > -1));
			}
			return View(pages);
		}

		// GET: SitePages/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: SitePages/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: SitePages/Create
		[HttpPost]
		public ActionResult Create(SitePage page)
		{
			try
			{
				// TODO: Add insert logic here
				client.CreateSitePageAsync(page, false);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: SitePages/Edit/5
		public ActionResult Edit(int id)
		{
			var pages = client.SelectSitePageByDefault();
			SitePage page = null;
			if (pages != null)
			{
				page = pages.Where(p => p.Id == id).FirstOrDefault();
			}

			return View(page);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, SitePage ad)
		{
			try
			{
				// TODO: Add update logic here
				bool isSucceed = client.UpdateSitePage(ad);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: SitePages/Delete/5
		public ActionResult Delete(int id)
		{
			var pages = client.SelectSiteADByDefault();
			SiteAD ad = null;
			if (pages != null)
			{
				ad = pages.Where(p => p.Id == id).FirstOrDefault();
			}

			return View(ad);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, SitePage ad)
		{
			try
			{
				// TODO: Add delete logic here
				var isSucceed = client.DeleteSiteAD((Int64)id);

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}