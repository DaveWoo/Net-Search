using System.Collections.Generic;
using System.ServiceModel;
using iBoxDB.LocalServer;
using Net.Models;

namespace Net.Services
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
	}
}