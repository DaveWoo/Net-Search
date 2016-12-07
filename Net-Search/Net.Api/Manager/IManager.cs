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
		IEnumerable<SiteInfo> SelectSiteInfoByDefault();

		[OperationContract]
		IEnumerable<SitePage> SelectSitePageByDefault();

		[OperationContract]
		IEnumerable<NetServerConfig> SelectSiteServerConfigByDefault();

		[OperationContract]
		IEnumerable<Link> SelectSiteLinkByDefault();

		[OperationContract]
		IEnumerable<SiteAD> SelectSiteADByDefault();

		[OperationContract]
		IEnumerable<Words> SelectWordsByDefault();

		[OperationContract]
		IEnumerable<Linked> SelectLinkedByDefault();

		#endregion Select all

		#region Select sql like

		[OperationContract]
		IEnumerable<SitePage> SelectSitePage(string sqlLike, object args);

		[OperationContract]
		IEnumerable<SiteInfo> SelectSiteInfo(string sqlLike, object args);

		[OperationContract]
		IEnumerable<NetServerConfig> SelectSiteServerConfig(string sqlLike, object args);

		[OperationContract]
		IEnumerable<Link> SelectSiteLink(string sqlLike, object args);

		[OperationContract]
		IEnumerable<SiteAD> SelectSiteAD(string sqlLike, object args);

		[OperationContract]
		IEnumerable<Words> SelectSiteWords(string sqlLike, object args);

		[OperationContract]
		IEnumerable<Linked> SelectSiteLinked(string sqlLike, object args);

		#endregion

		#region Delete
		[OperationContract]
		bool DeleteSitePage(string url);

		[OperationContract]
		bool DeleteSiteInfo(object id);

		[OperationContract]
		bool DeleteSiteServerConfig(string name);

		[OperationContract]
		bool DeleteLink(string url);

		[OperationContract]
		bool DeleteSiteAD(object id);

		[OperationContract]
		bool DeleteSiteWords(string url);

		[OperationContract]
		bool DeleteSiteLinked(string url);
		#endregion

		#region Site operation

		[OperationContract]
		bool CreateSiteSearchWords(Words value);

		[OperationContract]
		List<SitePage> GetPages(string searchValue);

		[OperationContract]
		void CreateSiteInfoFromUrl(string url);

		[OperationContract]
		int CreateSitePageFromUrl(string url);

		[OperationContract]
		void CreateSiteLinkFromUrl(string url);

		[OperationContract]
		bool CreateSiteInfo(SiteInfo value);

		[OperationContract]
		bool UpdateSiteInfo(SiteInfo value);

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

		[OperationContract]
		bool UpdateSiteSeverConfig(NetServerConfig value);

		#endregion Site operation
	}
}