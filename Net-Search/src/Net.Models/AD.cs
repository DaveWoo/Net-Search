using CsQuery;
using iBoxDB.LocalServer;
using System;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace Net.Models
{
	[DataContract]
	public class SiteAD : SitePage
	{
		[DataMember]
		public new string Tag { get; set; }
	}
}