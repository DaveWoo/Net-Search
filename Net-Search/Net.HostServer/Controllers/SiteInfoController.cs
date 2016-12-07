using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Net.HostServer.ServiceReference;
using Net.Models;

namespace Net.HostServer.Controllers
{
	public class SiteInfoController : Controller
	{
		ManagerClient client = new ManagerClient();
		// GET: AD
		public ActionResult Index(string searchString)
		{

			var siteInfo = client.SelectSiteInfoByDefault();
			return View(siteInfo);
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
		public ActionResult Create(Net.Models.SiteInfo siteInfo)
		{
			try
			{
				// TODO: Add insert logic here
				var isSucceed = client.CreateSiteInfoAsync(siteInfo);
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
			var infos = client.SelectSiteInfoByDefault();
			SiteInfo info = null;
			if (infos != null)
			{
				info = infos.Where(p => p.Id == id).FirstOrDefault();
			}

			return View(info);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, SiteInfo info)
		{
			try
			{
				// TODO: Add update logic here
				bool isSucceed = client.UpdateSiteInfo(info);
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
			var infos = client.SelectSiteInfoByDefault();
			SiteInfo info = null;
			if (infos != null)
			{
				info = infos.Where(p => p.Id == id).FirstOrDefault();
			}

			return View(info);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, SiteInfo info)
		{
			try
			{
				// TODO: Add delete logic here		
				var isSucceed = client.DeleteSiteInfo((Int64)id);

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
