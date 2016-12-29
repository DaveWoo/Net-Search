using iBoxDB.LocalServer;
using Net.Models;
using Net.Utils;

namespace Net.Api
{
	public partial class Manager
	{
		public bool Insert<T>(T value) where T : class
		{
			if (typeof(T).Name == Constants.TABLE_SITEPAGE)
			{
				return SDB.SitePageBox.Insert<T>(Constants.TABLE_SITEPAGE, value);
			}
			else if (typeof(T).Name == Constants.TABLE_SITEINFO)
			{
				return SDB.SiteInfoBox.Insert<T>(Constants.TABLE_SITEINFO, value);
			}
			else if (typeof(T).Name == Constants.TABLE_NETSERVERCONFIG)
			{
				return SDB.NetServerConfigBox.Insert<T>(Constants.TABLE_NETSERVERCONFIG, value);
			}
			else if (typeof(T).Name == Constants.TABLE_LINK)
			{
				return SDB.SiteLinkBox.Insert<T>(Constants.TABLE_LINK, value);
			}
			else if (typeof(T).Name == Constants.TABLE_AD)
			{
				return SDB.ADBox.Insert<T>(Constants.TABLE_AD, value);
			}
			else if (typeof(T).Name == Constants.TABLE_WORDS)
			{
				return SDB.SearchedWordsBox.Insert<T>(Constants.TABLE_WORDS, value);
			}
			else if (typeof(T).Name == Constants.TABLE_LINKED)
			{
				return SDB.SiteClickedLinkeBox.Insert<T>(Constants.TABLE_LINKED, value);
			}
			return false;
		}

		public IBEnumerable<T> Select<T>() where T : class, new()
		{
			if (typeof(T).Name == Constants.TABLE_SITEPAGE)
			{
				return SDB.SitePageBox.Select<T>(string.Format(Constants.SQLLIKE, Constants.TABLE_SITEPAGE));
			}
			else if (typeof(T).Name == Constants.TABLE_SITEINFO)
			{
				return SDB.SiteInfoBox.Select<T>(string.Format(Constants.SQLLIKE, Constants.TABLE_SITEINFO));
			}
			else if (typeof(T).Name == Constants.TABLE_NETSERVERCONFIG)
			{
				return SDB.NetServerConfigBox.Select<T>(string.Format(Constants.SQLLIKE, Constants.TABLE_NETSERVERCONFIG));
			}
			else if (typeof(T).Name == Constants.TABLE_LINK)
			{
				return SDB.SiteLinkBox.Select<T>(string.Format(Constants.SQLLIKE, Constants.TABLE_LINK));
			}
			else if (typeof(T).Name == Constants.TABLE_AD)
			{
				return SDB.ADBox.Select<T>(string.Format(Constants.SQLLIKE, Constants.TABLE_AD));
			}
			else if (typeof(T).Name == Constants.TABLE_WORDS)
			{
				return SDB.SearchedWordsBox.Select<T>(string.Format(Constants.SQLLIKE, Constants.TABLE_WORDS));
			}
			else if (typeof(T).Name == Constants.TABLE_LINKED)
			{
				return SDB.SiteClickedLinkeBox.Select<T>(string.Format(Constants.SQLLIKE, Constants.TABLE_LINKED));
			}
			return null;
		}

		public IBEnumerable<T> Select<T>(string sqlLike, object args) where T : class, new()
		{
			if (typeof(T).Name == Constants.TABLE_SITEPAGE)
			{
				return SDB.SitePageBox.Select<T>(sqlLike, args);
			}
			else if (typeof(T).Name == Constants.TABLE_SITEINFO)
			{
				return SDB.SiteInfoBox.Select<T>(sqlLike, args);
			}
			else if (typeof(T).Name == Constants.TABLE_NETSERVERCONFIG)
			{
				return SDB.NetServerConfigBox.Select<T>(sqlLike, args);
			}
			else if (typeof(T).Name == Constants.TABLE_LINK)
			{
				return SDB.SiteLinkBox.Select<T>(sqlLike, args);
			}
			else if (typeof(T).Name == Constants.TABLE_AD)
			{
				return SDB.ADBox.Select<T>(sqlLike, args);
			}
			else if (typeof(T).Name == Constants.TABLE_WORDS)
			{
				return SDB.SearchedWordsBox.Select<T>(sqlLike, args);
			}
			else if (typeof(T).Name == Constants.TABLE_LINKED)
			{
				return SDB.SiteClickedLinkeBox.Select<T>(sqlLike, args);
			}
			return null;
		}

		public bool Update<T>(long key, T value) where T : class
		{
			if (typeof(T).Name == Constants.TABLE_SITEPAGE)
			{
				return SDB.SitePageBox.Update<T>(Constants.TABLE_SITEPAGE, key, value);
			}
			else if (typeof(T).Name == Constants.TABLE_SITEINFO)
			{
				return SDB.SiteInfoBox.Update<T>(Constants.TABLE_SITEINFO, key, value);
			}
			else if (typeof(T).Name == Constants.TABLE_NETSERVERCONFIG)
			{
				return SDB.NetServerConfigBox.Update<T>(Constants.TABLE_NETSERVERCONFIG, key, value);
			}
			else if (typeof(T).Name == Constants.TABLE_LINK)
			{
				return SDB.SiteLinkBox.Update<T>(Constants.TABLE_LINK, key, value);
			}
			else if (typeof(T).Name == Constants.TABLE_AD)
			{
				return SDB.ADBox.Update<T>(Constants.TABLE_AD, key, value);
			}
			else if (typeof(T).Name == Constants.TABLE_WORDS)
			{
				return SDB.SearchedWordsBox.Update<T>(Constants.TABLE_WORDS, key, value);
			}
			else if (typeof(T).Name == Constants.TABLE_LINKED)
			{
				return SDB.SiteClickedLinkeBox.Update<T>(Constants.TABLE_LINKED, key, value);
			}
			return false;
		}

		public bool Delete<T>(object value) where T : class, new()
		{
			if (typeof(T).Name == Constants.TABLE_SITEPAGE)
			{
				return SDB.SitePageBox.Delete(Constants.TABLE_SITEPAGE, value);
			}
			else if (typeof(T).Name == Constants.TABLE_SITEINFO)
			{
				return SDB.SiteInfoBox.Delete(Constants.TABLE_SITEINFO, value);
			}
			else if (typeof(T).Name == Constants.TABLE_NETSERVERCONFIG)
			{
				return SDB.NetServerConfigBox.Delete(Constants.TABLE_NETSERVERCONFIG, value);
			}
			else if (typeof(T).Name == Constants.TABLE_LINK)
			{
				return SDB.SiteLinkBox.Delete(Constants.TABLE_LINK, value);
			}
			else if (typeof(T).Name == Constants.TABLE_AD)
			{
				return SDB.ADBox.Delete(Constants.TABLE_AD, value);
			}
			else if (typeof(T).Name == Constants.TABLE_WORDS)
			{
				return SDB.SearchedWordsBox.Delete(Constants.TABLE_WORDS, value);
			}
			else if (typeof(T).Name == Constants.TABLE_LINKED)
			{
				return SDB.SiteClickedLinkeBox.Delete(Constants.TABLE_LINKED, value);
			}
			return false;
		}
	}
}