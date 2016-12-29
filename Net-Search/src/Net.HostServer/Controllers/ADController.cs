using System;
using System.Linq;
using System.Web.Mvc;
using Net.HostServer.ServiceReference;
using Net.Models;

namespace Net.HostServer.Controllers
{
	public class ADController : Controller
	{
		private ManagerClient client = new ManagerClient();

		// GET: AD
		public ActionResult Index(string searchString)
		{
			var ads = client.SelectSiteADByDefault();
			return View(ads);
		}

		// GET: AD/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: AD/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: AD/Create
		[HttpPost]
		public ActionResult Create([Bind(Include = "Id,Url,Title,Content,Tag")] Net.Models.SiteAD ad)
		{
			try
			{
				// TODO: Add insert logic here
				client.AddSiteAD(ad.Url, ad.Title, ad.Content, "AD", ad.Tag);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: AD/Edit/5
		public ActionResult Edit(int id)
		{
			var ads = client.SelectSiteADByDefault();
			SiteAD ad = null;
			if (ads != null)
			{
				ad = ads.Where(p => p.Id == id).FirstOrDefault();
			}

			return View(ad);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Net.Models.SiteAD ad)
		{
			try
			{
				// TODO: Add update logic here
				bool isSucceed = client.UpdateSiteAD(ad.Url, ad.Title, ad.Content, "AD", ad.Disabled, ad.Tag);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: AD/Delete/5
		public ActionResult Delete(int id)
		{
			var ads = client.SelectSiteADByDefault();
			SiteAD ad = null;
			if (ads != null)
			{
				ad = ads.Where(p => p.Id == id).FirstOrDefault();
			}

			return View(ad);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, [Bind(Include = "Id,Url,Title,Content,Tag")] Net.Models.SiteAD ad)
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