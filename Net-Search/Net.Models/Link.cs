using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Models
{
	public class Link
	{
		public long Id { get; set; }
		public String Url { get; set; }
		public String Level { get; set; }
		public DateTime CreatedTimeStamp { get; set; }
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
						return null;
					}

				}
			}
		}
	}
}
