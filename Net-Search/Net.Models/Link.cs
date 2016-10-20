using System;
using System.Runtime.Serialization;
using Net.Utils.Common;

namespace Net.Models
{
	[DataContract]
	public class Link : IID, IUrl
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
						Log.Error(ex.Message, ex);
						return null;
					}
				}
			}
		}
	}
}