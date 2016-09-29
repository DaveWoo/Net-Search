using System;

namespace Net.Api
{
	public class Constants
	{
		public const string TABLE_SITEPAGE = "SitePage";
		public const string TABLE_SITEINFO = "SiteInfo";
		public const string TABLE_PROCESSLINK = "ProcessLink";
		public const string TABLE_PROCESSLINKCONFIG = "ProcessLinkConfig";

		public const string PROCESSLINKCONFIG_NAME_GRABCONTENTS = "GrabContents";
		public const string PROCESSLINKCONFIG_NAME_GRABLINKS = "GrabLinks";
		public const string SERVERDATA_NAME = "/ftsdata7/";
		public const int PAGECOUNT = 9;
		public const int PAGECOUNTLIMIT = 100;
		public const int PAGEINDEXS = 10;


		public static string SERVERDATA_PATH = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Constants.SERVERDATA_NAME;
	}
}