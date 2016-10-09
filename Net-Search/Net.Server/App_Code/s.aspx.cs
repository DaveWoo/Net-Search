using System;
using System.Collections.Generic;
using System.Linq;
using Net.Api;
using Net.Models;

namespace Net.Server
{
    public partial class Default : System.Web.UI.Page
    {
        protected string name;
        protected string searchResult;
        protected List<SitePage> pagesAll = new List<SitePage>();
        List<SitePage> pagesAd = null;
        protected string pageIndexString;
        protected DateTime begin;
        protected string processLinksCount;
        protected string siteInfoCount;
        protected string sitePageCount;
        private static Manager manager = new Manager();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            #region Handle parameter

            name = Request["q"];
            var pageNumberString = Request["pn"];
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "";
                return;
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
                return;
            }
            name = name.Replace("<", " ").Replace(">", " ")
                .Replace("\"", " ").Replace(",", " ")
                    .Replace("\\$", " ").Trim();

            #endregion Handle parameter

            #region Calc
            List<SitePage> pageList = new List<SitePage>();

            #region Ad query

            var ads = SDB.ADBox.Select<AD>(string.Format(Constants.LIKESQL, Constants.TABLE_AD));
            if (ads != null)
            {
                pagesAd = ads.Where(p => (p.Tag!=null)&&p.Tag.ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Contains(name)).ToList<SitePage>();

            }
            #endregion

            begin = DateTime.Now;

            #region body query

            using (var box = SDB.SitePageBox.Cube())
            {
                var results = SearchResource.Engine.SearchDistinct(box, name).OrderBy(p => p.Position);
                if (results != null)
                {

                    searchResult = results.Count().ToString();
                    //searchResult.TakeWhile()
                    foreach (KeyWord kw in results)
                    {
                        long id = kw.ID;
                        id = SitePage.RankDownId(id);
                        SitePage page = box[Constants.TABLE_SITEPAGE, id].Select<SitePage>();
                        //todo
                        if (page == null)
                            continue;
                        page.keyWord = kw;
                        pageList.Add(page);
                        if (pageList.Count >= Constants.PAGECOUNTLIMIT)
                        {
                            break;
                        }
                    }
                }
            }

            var pages = pageList.Skip((pageNumber - 1) * 10).Take(Constants.PAGECOUNT);
            if (pages == null || pages.Count() == 0)
            {
                SitePage p = new SitePage();
                p.Title = "NotFound";
                p.Description = "";
                p.Content = "Please contact administrator";
                p.Url = "./";
                pageList.Add(p);
            }
            #endregion

            if (pagesAd != null)
            {
                pagesAll.AddRange(pagesAd);

            }
            if (pages != null)
            {
                foreach (var page in pages)
                {
                    if (!string.IsNullOrWhiteSpace(page.Host))
                    {
                        var host = page.Host.Replace("www", string.Empty);
                        if (host != null)
                        {
                            var siteInfo = manager.Select<SiteInfo>().Where(p => p.Url.IndexOf(host) != -1).FirstOrDefault();
                            if (siteInfo != null)
                            {
                                page.Tag = siteInfo;
                                page.VerifiedSiteName = siteInfo.Name;
                            }
                        }
                    }
                }
                pagesAll.AddRange(pages);
            }
            #endregion

            #region Pange index
            GenerateNextPage(pageNumber);
            #endregion
        }

        private void GenerateNextPage(int pageNumber)
        {
            string index = "<a href=\"\\s.aspx?q={0}&pn={1}\">{2}</a>";
            string indexCurrent = "<strong>{0}</strong>";
            string nextIndexStr = "<a id=\"snext\" href=\"\\s.aspx?q={0}&pn={1}\">Next</a>";
            string preIndexStr = "<a id=\"spre\" href=\"\\s.aspx?q={0}&pn={1}\">Previous</a>";
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
        }


    }
}