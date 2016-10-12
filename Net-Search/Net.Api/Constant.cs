using System;

namespace Net.Api
{
	public class Constants
	{
		public const string TABLE_SITEPAGE = "SitePage";
		public const string TABLE_SITEINFO = "SiteInfo";
		public const string TABLE_LINK = "Link";
		public const string TABLE_AD = "AD";
		public const string TABLE_WORDS = "Words";
		public const string TABLE_LINKED = "Linked";

		public const string TABLE_FIELD_ID = "Id";
		public const string TABLE_FIELD_URL = "Url";

		public const string TABLE_NETSERVERCONFIG = "NetServerConfig";

		public const string PROCESSLINKCONFIG_NAME_GRABCONTENTS = "GrabContents";
		public const string PROCESSLINKCONFIG_NAME_GRABLINKS = "GrabLinks";
		public const string PROCESSLINKCONFIG_NAME_CHINAZINDEX = "ChinaZIndex";
		public const string PROCESSLINKCONFIG_NAME_GRABBASICLINKS = "GrabBasicLinks";

		public const string SERVERDATA_NAME = "/NetServerData/";

		public const string SQLLIKE = "from {0}";
		public const string LINKPATTERN = @"(href)[ ]*=[ ]*[""']*(http)?[^""'#>]+[""']*";

		public const int PAGECOUNT = 10;
		public const int PAGECOUNTLIMIT = 100;
		public const int PAGEINDEXS = 10;
		public const int TAKECOUNT = 10;
		public const int TAKECOUNTCHINAZ = 20;
		public const int TAKECOUNTBASICLINKS = 5;
		public const int URLLENGTH = 200;


		public static string SERVERDATA_PATH = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Constants.SERVERDATA_NAME;
	}
}