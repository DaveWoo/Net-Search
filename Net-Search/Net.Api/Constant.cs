using System;

namespace Net.Api
{
	public class Constants
	{
		public const string TABLE_SITEPAGE = "SitePage";
		public const string TABLE_SITEINFO = "SiteInfo";
		public const string TABLE_LINK = "Link";
		public const string TABLE_AD = "AD";

		public const string TABLE_NETSERVERCONFIG = "NetServerConfig";

		public const string PROCESSLINKCONFIG_NAME_GRABCONTENTS = "GrabContents";
		public const string PROCESSLINKCONFIG_NAME_GRABLINKS = "GrabLinks";
		public const string SERVERDATA_NAME = "/NetServerData/";

		public const string LIKESQL = "from {0}";
		public const string LINKPATTERN = @"(href)[ ]*=[ ]*[""']*(http)?[^""'#>]+[""']*";

		public const int PAGECOUNT = 10;
		public const int PAGECOUNTLIMIT = 100;
		public const int PAGEINDEXS = 10;
		public const int takeCount = 10;


		public static string SERVERDATA_PATH = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Constants.SERVERDATA_NAME;
	}
}