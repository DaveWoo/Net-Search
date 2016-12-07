using System;
using System.Runtime.Serialization;

namespace Net.Models
{
	[DataContract]
	public class SiteInfo : IID, IUrl, IEnalbe
	{
		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public String Url { get; set; }

		[DataMember]
		public String Name { get; set; }

		[DataMember]
		public int Alexa { get; set; }

		[DataMember]
		public int RankBaidu { get; set; }

		[DataMember]
		public int PR { get; set; }

		[DataMember]
		public int Linked { get; set; }

		[DataMember]
		public bool IsGuard { get; set; }

		[DataMember]
		public int LinkedReversed { get; set; }

		[DataMember]
		public String Description { get; set; }

		[DataMember]
		public int Score { get; set; }

		[DataMember]
		public bool Enabled { get; set; }

		[DataMember]
		public DateTime CreatedTimeStamp { get; set; }

		[DataMember]
		public DateTime UpdatedTimeStamp { get; set; }
	}
}