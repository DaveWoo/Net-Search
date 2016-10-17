using System;
using System.Collections.Generic;
using System.ServiceModel;
using Net.Models;

namespace Net.Api
{
	[ServiceContract]
	internal interface IManager
	{
		#region Select all

		[OperationContract]
		IEnumerable<SiteInfo> SelectAllSiteInfo();

		[OperationContract]
		IEnumerable<SitePage> SelectAllSitePage();

		[OperationContract]
		IEnumerable<NetServerConfig> SelectAllSiteServerConfig();

		[OperationContract]
		IEnumerable<Link> SelectAllLink();

		[OperationContract]
		IEnumerable<SiteAD> SelectAllSiteAD();

		[OperationContract]
		IEnumerable<Words> SelectAllWords();

		[OperationContract]
		IEnumerable<Linked> SelectAllLinked();

		#endregion Select all

		[OperationContract]
		IEnumerable<SiteInfo> SelectSiteInfo(string sqlLike, object args);

		[OperationContract]
		IEnumerable<SiteAD> SelectSiteAD(string sqlLike, object args);

		#region Site operation

		[OperationContract]
		bool CreateSiteSearchWords(Words value);

		[OperationContract]
		List<SitePage> GetPages(string name);

		[OperationContract]
		void GetAllSiteInfoFromChinaZ();

		[OperationContract]
		void GetBasicLinks();

		[OperationContract]
		void GrabLinks();

		[OperationContract]
		void GrabLinksContent();

		[OperationContract]
		void AddAd(string url, string title, string description, string company, string tag);

		[OperationContract]
		bool CreateSitePage(SitePage value);

		[OperationContract]
		NetServerConfig GetCurrentProcessLinkAnchorID(string name);

		[OperationContract]
		List<String> GetDiscover();

		#endregion Site operation
	}
}