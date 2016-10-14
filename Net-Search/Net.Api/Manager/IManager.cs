using System.Collections.Generic;
using System.ServiceModel;
using iBoxDB.LocalServer;
using Net.Models;

namespace Net.Api
{
	[ServiceContract]
	internal interface IManager
	{
		[OperationContract]
		bool CreateSitePage(SitePage value);

		[OperationContract]
		IEnumerable<SiteInfo> SelectAllSiteInfo();

		[OperationContract]
		IEnumerable<SitePage> SelectAllSitePage();

		[OperationContract]
		List<SitePage> GetPages(string name);

		[OperationContract]
		IEnumerable<SiteInfo> SelectSiteInfo(string sqlLike, object args);

		[OperationContract]
		bool CreateSiteSearchWords(Words value);
	}
}