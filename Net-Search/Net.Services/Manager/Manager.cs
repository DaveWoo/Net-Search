using System.Collections.Generic;
using iBoxDB.LocalServer;
using Net.Models;
using System.Linq;

namespace Net.Services
{
	public class Manager : IManager
	{
		#region Fields

		private DB.AutoBox sitePageBox = SDB.SitePageBox;
		private DB.AutoBox siteInfoBox = SDB.SiteInfoBox;
		private DB.AutoBox linkBox = SDB.LinkBox;
		private DB.AutoBox adBox = SDB.ADBox;
		private DB.AutoBox netServerConfigBox = SDB.NetServerConfigBox;
		private DB.AutoBox wordsBox = SDB.WordsBox;
		private DB.AutoBox linkedBox = SDB.LinkedBox;

		#endregion Fields

		#region Create

		public bool CreateSitePage(SitePage value)
		{
			return sitePageBox.Insert<SitePage>(Constants.TABLE_SITEPAGE, value);
		}

		public bool CreateSiteInfo(SiteInfo value)
		{
			return siteInfoBox.Insert<SiteInfo>(Constants.TABLE_SITEINFO, value);
		}

		public bool CreateSiteServerConfig(NetServerConfig value)
		{
			return adBox.Insert<NetServerConfig>(Constants.TABLE_NETSERVERCONFIG, value);
		}

		public bool CreateSiteLink(Link value)
		{
			return linkBox.Insert<Link>(Constants.TABLE_LINK, value);
		}

		public bool CreateSiteAD(SiteAD value)
		{
			return adBox.Insert<SiteAD>(Constants.TABLE_AD, value);
		}

		public bool CreateSiteSearchWords(Words value)
		{
			return wordsBox.Insert<Words>(Constants.TABLE_WORDS, value);
		}

		public bool CreateSiteClickedLink(Linked value)
		{
			return linkedBox.Insert<Linked>(Constants.TABLE_LINKED, value);
		}

		#endregion Create

		#region Update

		public bool UpdateSitePage(SitePage value)
		{
			return sitePageBox.Update<SitePage>(Constants.TABLE_SITEPAGE, value);
		}

		public bool UpdateSiteInfo(SiteInfo value)
		{
			return siteInfoBox.Update<SiteInfo>(Constants.TABLE_SITEINFO, value);
		}

		public bool UpdateSiteSeverConfig(NetServerConfig value)
		{
			return adBox.Update<NetServerConfig>(Constants.TABLE_NETSERVERCONFIG, value);
		}

		public bool UpdateSiteLink(Link value)
		{
			return linkBox.Update<Link>(Constants.TABLE_LINK, value);
		}

		public bool UpdateSiteAD(SiteAD value)
		{
			return adBox.Update<SiteAD>(Constants.TABLE_AD, value);
		}

		public bool UpdateSiteWords(Words value)
		{
			return wordsBox.Update<Words>(Constants.TABLE_WORDS, value);
		}

		public bool UpdateSiteLinked(Linked value)
		{
			return linkedBox.Update<Linked>(Constants.TABLE_LINKED, value);
		}

		#endregion Update

		#region Select

		#region Select All

		public IEnumerable<SitePage> SelectAllSitePage()
		{
			var result = sitePageBox.Select<SitePage>(string.Format(Constants.SQLLIKE, Constants.TABLE_SITEPAGE));
			return result;
		}

		public IEnumerable<SiteInfo> SelectAllSiteInfo()
		{
			return siteInfoBox.Select<SiteInfo>(string.Format(Constants.SQLLIKE, Constants.TABLE_SITEINFO));
		}

		public IEnumerable<NetServerConfig> SelectAllSiteServerConfig()
		{
			return adBox.Select<NetServerConfig>(string.Format(Constants.SQLLIKE, Constants.TABLE_NETSERVERCONFIG));
		}

		public IEnumerable<Link> SelectAllLink()
		{
			return linkBox.Select<Link>(string.Format(Constants.SQLLIKE, Constants.TABLE_LINK));
		}

		public IEnumerable<SiteAD> SelectAllSiteAD()
		{
			return adBox.Select<SiteAD>(string.Format(Constants.SQLLIKE, Constants.TABLE_AD));
		}

		public IEnumerable<Words> SelectAllWords()
		{
			return wordsBox.Select<Words>(string.Format(Constants.SQLLIKE, Constants.TABLE_WORDS));
		}

		public IEnumerable<Linked> SelectAllLinked()
		{
			return linkedBox.Select<Linked>(string.Format(Constants.SQLLIKE, Constants.TABLE_LINKED));
		}

		#endregion Select All

		#region Select sql like

		public IEnumerable<SitePage> SelectSitePage(string sqlLike, object args)
		{
			return sitePageBox.Select<SitePage>(sqlLike, args);
		}

		public IEnumerable<SiteInfo> SelectSiteInfo(string sqlLike, object args)
		{
			return siteInfoBox.Select<SiteInfo>(sqlLike, args);
		}

		public IEnumerable<NetServerConfig> SelectSiteServerConfig(string sqlLike, object args)
		{
			return adBox.Select<NetServerConfig>(sqlLike, args);
		}

		public IEnumerable<Link> SelectSiteLink(string sqlLike, object args)
		{
			return linkBox.Select<Link>(sqlLike, args);
		}

		public IEnumerable<SiteAD> SelectSiteAD(string sqlLike, object args)
		{
			return adBox.Select<SiteAD>(sqlLike, args);
		}

		public IEnumerable<Words> SelectSiteWords(string sqlLike, object args)
		{
			return wordsBox.Select<Words>(sqlLike, args);
		}

		public IEnumerable<Linked> SelectSiteLinked(string sqlLike, object args)
		{
			return linkedBox.Select<Linked>(sqlLike, args);
		}

		#endregion Select sql like

		#endregion Select

		#region Delete

		public bool DeleteSitePage(SitePage value)
		{
			return siteInfoBox.Delete(Constants.TABLE_SITEPAGE, value);
		}

		public bool DeleteSiteInfo(SiteInfo value)
		{
			return siteInfoBox.Delete(Constants.TABLE_SITEINFO, value);
		}

		public bool DeleteSiteServerConfig(NetServerConfig value)
		{
			return adBox.Delete(Constants.TABLE_NETSERVERCONFIG, value);
		}

		public bool Delete(Link value)
		{
			return linkBox.Delete(Constants.TABLE_LINK, value);
		}

		public bool DeleteSiteAD(SiteAD value)
		{
			return adBox.Delete(Constants.TABLE_AD, value);
		}

		public bool DeleteSiteWords(Words value)
		{
			return wordsBox.Delete(Constants.TABLE_WORDS, value);
		}

		public bool DeleteSiteLinked(Linked value)
		{
			return linkedBox.Delete(Constants.TABLE_LINKED, value);
		}

		#endregion Delete

		public List<SitePage> GetPages(string name)
		{
			List<SitePage> pageList = new List<SitePage>();
			using (var box = SDB.SitePageBox.Cube())
			{
				var results = SearchResource.Engine.SearchDistinct(box, name).OrderBy(p => p.Position);
				if (results != null)
				{
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

			return pageList;
		}

	}
}