using System;
using System.Runtime.Serialization;

namespace Net.Models
{
	[DataContract]
	public class Words : IID
	{
		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public DateTime CreatedTimeStamp { get; set; }

		[DataMember]
		public string IP { get; set; }
	}
}