using System;
using System.Collections.Generic;
using System.ServiceModel;
using Net.Models;

namespace Net.Api
{
	[ServiceContract]
	public interface IManager
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
		List<SitePage> GetPages(string searchValue);

		[OperationContract]
		void GetAllSiteInfoFromChinaZ();

		[OperationContract]
		void GetBasicLinks();

		[OperationContract]
		void GrabLinks();

		[OperationContract]
		void GrabLinksContent();

		[OperationContract]
		bool AddSiteAD(string url, string title, string content, string company, string tag);

		[OperationContract]
		bool UpdateSiteAD(string url, string title, string content, string company, string tag);

		[OperationContract]
		bool CreateSitePage(SitePage value, bool isDeleteCurrentThenAddNew);

		[OperationContract]
		bool CreateSiteLink(Link value);

		[OperationContract]
		bool CreateSiteClickedLink(Linked value);

		[OperationContract]
		NetServerConfig GetCurrentProcessLinkAnchorID(string name);

		[OperationContract]
		List<String> GetDiscover();

		[OperationContract]
		int GetRelatedResutsCount(string searchValue);

		#endregion Site operation
	}
}