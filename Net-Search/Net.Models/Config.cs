using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Models
{
	public class ProcessLinkConfig
	{
		public long ProcessedLinkId { get; set; }
		public DateTime ModifiedTimeStamp { get; set; }
		public string Name { get; set; }
	}
}
