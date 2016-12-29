using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Net.HostServer.ServiceReference;
using Net.Models;
using Net.Utils;
using Net.Utils.Common;

namespace Net.HostServer.Controllers
{
	public class SController : Controller
	{
		protected string relatedSearchCount;
		private List<SitePage> pagesAd = null;
		protected string pageIndexString;
		protected string processLinksCount;
		protected string siteInfoCount;
		protected string sitePageCount;
		private static ManagerClient client = null;

		static SController()
		{
			client = new ManagerClient();
		}

		public ActionResult Index(string q, string pn)
		{
			List<SitePage> pages = GetPages(q, pn);
			ViewBag.Pages = pages;
			ViewBag.PageCount = pages.Count();
			ViewBag.SiteName = Constants.SITE_NAME;
			return View(pages);
		}

		private List<SitePage> GetPages(string name, string pageNumberString)
		{
			List<SitePage> pages = new List<SitePage>();

			Log.Info("Enter search page");

			#region Handle parameter

			Log.Info("Handle parameter start");

			if (string.IsNullOrWhiteSpace(name))
			{
				name = "";
				return pages;
			}
			if (string.IsNullOrWhiteSpace(pageNumberString))
			{
				pageNumberString = "1";
			}
			int pageNumber = 1;
			if (!int.TryParse(pageNumberString, out pageNumber))
			{
				pageNumber = 1;	// Default page number is 1
			}
			if (name.Length > 200)
			{
				name = "";
				return pages;
			}
			name = name.Replace("<", " ").Replace(">", " ")
				.Replace("\"", " ").Replace(",", " ")
					.Replace("\\$", " ").Trim();

			#endregion Handle parameter

			#region Search words

			Log.Info("Search words start");
			ViewBag.SearchName = name;
			Words word = new Words();
			word.IP = HttpHelper.GetIp();
			word.Name = name;
			word.CreatedTimeStamp = System.DateTime.Now;

			client.CreateSiteSearchWordsAsync(word);

			#endregion Search words

			#region Calc

			#region Ad query

			Log.Info("Calc ad query start");
			//todo
			var ads = client.SelectSiteADByDefault();
			if (ads != null)
			{
				pagesAd = ads.Where(p => (!p.Disabled && p.Tag != null) && p.Tag.ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Contains(name)).ToList<SitePage>();
			}

			#endregion Ad query

			#region body query

			Log.Info("Calc body query start");
			var pageList = client.GetPages(name);
			var relatedSearchResultCount = client.GetRelatedResutsCount(name);
			if (relatedSearchResultCount != -1)
			{
				ViewBag.RelatedSearchCount = relatedSearchResultCount.ToString("N0");
			}

			var currentPages = pageList.Skip((pageNumber - 1) * 10).Take(Constants.PAGECOUNT);
			if (currentPages == null || currentPages.Count() == 0)
			{
				SitePage p = new SitePage();
				p.Title = "NotFound";
				p.Description = "";
				p.Content = "Please contact administrator";
				p.Url = "./";
				pageList.ToList<SitePage>().Add(p);
			}

			#endregion body query

			Log.Info("Query factory start");
			if (pagesAd != null && pageNumber == 1)
			{
				pages.AddRange(pagesAd);
			}
			Log.Info("Query factory start pagesAd");
			if (currentPages != null)
			{
				//todo
				foreach (var page in currentPages)
				{
					if (!string.IsNullOrWhiteSpace(page.Host))
					{
						var host = page.Host.Replace("www", string.Empty);
						if (host != null)
						{
							string sqlLikeSiteInfo = string.Format("from {0} Url ==?", Constants.TABLE_SITEINFO);
							//var siteInfo = manager.Select<SiteInfo>().Where(p => p.Url.IndexOf(host) != -1).FirstOrDefault();
							var siteInfo = client.SelectSiteInfo(sqlLikeSiteInfo, host);
							if (siteInfo != null && siteInfo.FirstOrDefault() != null)
							{
								page.Tag = siteInfo.FirstOrDefault();
								page.VerifiedSiteName = siteInfo.FirstOrDefault().Name;
							}
						}
					}
				}
				pages.AddRange(currentPages);
			}

			#endregion Calc

			#region Pane index

			Log.Info("Page index start");

			ViewBag.PageIndexString = GenerateNextPage(name, pageNumber);
			return pages;

			#endregion Pane index
		}

		private string GenerateNextPage(string name, int pageNumber)
		{
			Log.Info("FilePath:" + Request.FilePath);
			string pageIndexString = "<ul class='pagination'>";
			string index = "<li><a href=\"" + Request.FilePath + "?q={0}&pn={1}\">{2}</a></li>";
			string indexCurrent = "<li class='active'><a class='#'>{0}</a></li>";
			string nextIndexStr = "<li><a id=\"snext\" href=\"" + Request.FilePath + "?q={0}&pn={1}\">Next</a></li>";
			string preIndexStr = "<li><a id=\"spre\" href=\"" + Request.FilePath + "?q={0}&pn={1}\">Previous</a></li>";
			bool isAddPreindex = false;
			int pageIndexCount = pageNumber < Constants.PAGEINDEXS ? Constants.PAGEINDEXS : pageNumber;
			int indexStart = pageNumber < Constants.PAGEINDEXS ? 1 : pageNumber - Constants.PAGEINDEXS + 1;
			for (int i = indexStart; i <= pageIndexCount; i++)
			{
				if (pageNumber > 1 && !isAddPreindex)
				{
					int reviousIndex = pageNumber - 1;
					pageIndexString += string.Format(preIndexStr, name, reviousIndex);
					isAddPreindex = true;
				}
				if (i == pageNumber)
				{
					pageIndexString += string.Format(indexCurrent, i);
				}
				else
				{
					pageIndexString += string.Format(index, name, i, i);
				}
			}
			int nextIndex = pageNumber + 1;
			pageIndexString += string.Format(nextIndexStr, name, nextIndex);
			pageIndexString += "</ul>";
			return pageIndexString;
		}
	}
}