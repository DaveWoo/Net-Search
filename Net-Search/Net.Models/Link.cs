using System;
using System.Runtime.Serialization;

namespace Net.Models
{
	[DataContract]
	public class Link
	{
		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public String Url { get; set; }

		[DataMember]
		public String Level { get; set; }

		[DataMember]
		public DateTime CreatedTimeStamp { get; set; }

		[DataMember]
		public DateTime ModifiedTimeStamp { get; set; }

		[DataMember]
		public string Host
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Url))
					return string.Empty;
				else
				{
					try
					{
						return new Uri(Url).Host;
					}
					catch (Exception ex)
					{
						return null;
					}
				}
			}
		}
	}
}