using iBoxDB.LocalServer;
using Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Api
{
    public class Manager
    {
        public bool Create<T>(params object[] values) where T : class
        {
            if (values is SitePage[])
            {
                return SDB.SearchBox.Insert<T>(Constants.TABLE_SITEPAGE, (T[])values);
            }
            else if (values is SiteInfo[])
            {
                return SDB.AssistBox.Insert<T>(Constants.TABLE_SITEINFO, (T[])values);
            }
            else if (values is NetServerConfig[])
            {
                return SDB.AssistBox.Insert<T>(Constants.TABLE_NETSERVERCONFIG, (T[])values);
            }
            else if (values is Link[])
            {
                return SDB.AssistBox.Insert<T>(Constants.TABLE_LINK, (T[])values);
            }
            return false;
        }

        public IBEnumerable<T> Select<T>(params object[] values) where T : class, new()
        {
            if (values is SitePage[])
            {
                return SDB.SearchBox.Select<T>(string.Format(Constants.LIKESQL,Constants.TABLE_SITEPAGE));
            }
            else if (values is SiteInfo[])
            {
                return SDB.AssistBox.Select<T>(string.Format(Constants.LIKESQL, Constants.TABLE_SITEINFO));
            }
            else if (values is NetServerConfig[])
            {
                return SDB.AssistBox.Select<T>(string.Format(Constants.LIKESQL, Constants.TABLE_NETSERVERCONFIG));
            }
            else if (values is Link[])
            {
                return SDB.AssistBox.Select<T>(string.Format(Constants.LIKESQL, Constants.TABLE_LINK));
            }
            return null;
        }

        public bool Update<T>(params object[] values) where T : class
        {
            if (values is SitePage[])
            {
                return SDB.SearchBox.Update<T>(Constants.TABLE_SITEPAGE, (T[])values);
            }
            else if (values is SiteInfo[])
            {
                return SDB.AssistBox.Update<T>(Constants.TABLE_SITEINFO, (T[])values);
            }
            else if (values is NetServerConfig[])
            {
                return SDB.AssistBox.Update<T>(Constants.TABLE_NETSERVERCONFIG, (T[])values);
            }
            else if (values is Link[])
            {
                return SDB.AssistBox.Update<T>(Constants.TABLE_LINK, (T[])values);
            }
            return false;
        }

        public bool Delete<T>(params object[] values) where T : class, new()
        {
            if (values is SitePage[])
            {
                return SDB.AssistBox.Delete(Constants.TABLE_SITEPAGE, (T[])values);
            }
            else if (values is SiteInfo[])
            {
                return SDB.AssistBox.Delete(Constants.TABLE_SITEINFO, (T[])values);
            }
            else if (values is NetServerConfig[])
            {
                return SDB.AssistBox.Delete(Constants.TABLE_NETSERVERCONFIG, (T[])values);
            }
            else if (values is Link[])
            {
                return SDB.AssistBox.Delete(Constants.TABLE_LINK, (T[])values);
            }
            return false;
        }
    }

}
