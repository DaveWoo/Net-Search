using System;
using System.Linq;
using Net.Models;
using Net.Server.ServiceReference;

namespace Net.Server
{
	public partial class Dashboard : System.Web.UI.Page
	{
		protected DateTime begin;
		protected string processLinksCount;
		protected string siteInfoCount;
		protected string sitePageCount;
		protected string wordsCount;
		protected string linkedCount;
		ManagerClient client;

		protected override void OnLoad(EventArgs e)
		{
			begin = DateTime.Now;
			client = new ManagerClient();
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
			var processLinks = client.SelectSiteLinkByDefault();
			processLinksCount = processLinks.Count().ToString();
			var siteInfo = client.SelectSiteInfoByDefault();
			siteInfoCount = siteInfo.Count().ToString();
			var sitePage = client.SelectSitePageByDefault();
			sitePageCount = sitePage.Count().ToString();

			var words = client.SelectWordsByDefault();
			wordsCount = words.Count().ToString();

			var linked = client.SelectLinkedByDefault();
			linkedCount = linked.Count().ToString();
		}
	}
}