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
		public string Tag { get; set; }
	}
}