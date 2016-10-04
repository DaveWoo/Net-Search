using iBoxDB.LocalServer;
using Net.Models;

namespace Net.Api
{
    public class Manager
    {
        public bool Create<T>(params T[] values) where T : class
        {
            if (typeof(T).Name == Constants.TABLE_SITEPAGE)
            {
                return SDB.SearchBox.Insert<T>(Constants.TABLE_SITEPAGE, (T[])values);
            }
            else if (typeof(T).Name == Constants.TABLE_SITEINFO)
            {
                return SDB.AssistBox.Insert<T>(Constants.TABLE_SITEINFO, (T[])values);
            }
            else if (typeof(T).Name == Constants.TABLE_NETSERVERCONFIG)
            {
                return SDB.AssistBox.Insert<T>(Constants.TABLE_NETSERVERCONFIG, (T[])values);
            }
            else if (typeof(T).Name == Constants.TABLE_LINK)
            {
                return SDB.AssistBox.Insert<T>(Constants.TABLE_LINK, (T[])values);
            }
            return false;
        }

        public IBEnumerable<T> Select<T>() where T : class, new()
        {
            if (typeof(T).Name == Constants.TABLE_SITEPAGE)
            {
                return SDB.SearchBox.Select<T>(string.Format(Constants.LIKESQL, Constants.TABLE_SITEPAGE));
            }
            else if (typeof(T).Name == Constants.TABLE_SITEINFO)
            {
                return SDB.AssistBox.Select<T>(string.Format(Constants.LIKESQL, Constants.TABLE_SITEINFO));
            }
            else if (typeof(T).Name == Constants.TABLE_NETSERVERCONFIG)
            {
                return SDB.AssistBox.Select<T>(string.Format(Constants.LIKESQL, Constants.TABLE_NETSERVERCONFIG));
            }
            else if (typeof(T).Name == Constants.TABLE_LINK)
            {
                return SDB.AssistBox.Select<T>(string.Format(Constants.LIKESQL, Constants.TABLE_LINK));
            }
            return null;
        }

        public IBEnumerable<T> Select<T>(string ql, object args) where T : class, new()
        {
            if (typeof(T).Name == Constants.TABLE_SITEPAGE)
            {
                return SDB.SearchBox.Select<T>(ql, args);
            }
            else if (typeof(T).Name == Constants.TABLE_SITEINFO)
            {
                return SDB.AssistBox.Select<T>(ql, args);
            }
            else if (typeof(T).Name == Constants.TABLE_NETSERVERCONFIG)
            {
                return SDB.AssistBox.Select<T>(ql, args);
            }
            else if (typeof(T).Name == Constants.TABLE_LINK)
            {
                return SDB.AssistBox.Select<T>(ql,args);
            }
            return null;
        }

        public bool Update<T>(params T[] values) where T : class
        {
            if (typeof(T).Name == Constants.TABLE_SITEPAGE)
            {
                return SDB.SearchBox.Update<T>(Constants.TABLE_SITEPAGE, (T[])values);
            }
            else if (typeof(T).Name == Constants.TABLE_SITEINFO)
            {
                return SDB.AssistBox.Update<T>(Constants.TABLE_SITEINFO, (T[])values);
            }
            else if (typeof(T).Name == Constants.TABLE_NETSERVERCONFIG)
            {
                return SDB.AssistBox.Update<T>(Constants.TABLE_NETSERVERCONFIG, (T[])values);
            }
            else if (typeof(T).Name == Constants.TABLE_LINK)
            {
                return SDB.AssistBox.Update<T>(Constants.TABLE_LINK, (T[])values);
            }
            return false;
        }

        public bool Delete<T>(params T[] values) where T : class, new()
        {
            if (typeof(T).Name == Constants.TABLE_SITEPAGE)
            {
                return SDB.AssistBox.Delete(Constants.TABLE_SITEPAGE, (T[])values);
            }
            else if (typeof(T).Name == Constants.TABLE_SITEINFO)
            {
                return SDB.AssistBox.Delete(Constants.TABLE_SITEINFO, (T[])values);
            }
            else if (typeof(T).Name == Constants.TABLE_NETSERVERCONFIG)
            {
                return SDB.AssistBox.Delete(Constants.TABLE_NETSERVERCONFIG, (T[])values);
            }
            else if (typeof(T).Name == Constants.TABLE_LINK)
            {
                return SDB.AssistBox.Delete(Constants.TABLE_LINK, (T[])values);
            }
            return false;
        }
    }
}