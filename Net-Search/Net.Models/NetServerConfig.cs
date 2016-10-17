using System;
using System.Runtime.Serialization;

namespace Net.Models
{
	[DataContract]
	public class NetServerConfig
	{
		[DataMember]
		public long ProcessedLinkAnchorId { get; set; }

		[DataMember]
		public DateTime ModifiedTimeStamp { get; set; }

		[DataMember]
		public string Name { get; set; }
	}
}