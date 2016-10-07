using System;
using System.Collections.Generic;
using System.Linq;
using Net.Api;
using Net.Models;

namespace Net.Server
{
	public partial class Dashboard : System.Web.UI.Page
	{
		protected DateTime begin;
		protected string processLinksCount;
		protected string siteInfoCount;
		protected string sitePageCount;
		private static Manager manager = new Manager();

		protected override void OnLoad(EventArgs e)
		{
			begin = DateTime.Now;

			base.OnLoad(e);

			#region Summary

			GenerateSummaryInfo();

			#endregion Summary
		}

		protected void btnProcessLinks_Click(object sender, EventArgs e)
		{

		}

		protected void btnSiteInfo_Click(object sender, EventArgs e)
		{

		}

		protected void btnSitePage_Click(object sender, EventArgs e)
		{

		}

		private void GenerateSummaryInfo()
		{
			var processLinks = manager.Select<Link>();
			processLinksCount = processLinks.Count().ToString();
			var siteInfo = manager.Select<SiteInfo>();
			siteInfoCount = siteInfo.Count().ToString();
			var sitePage = manager.Select<SitePage>();
			sitePageCount = sitePage.Count().ToString();
		}
	}
}