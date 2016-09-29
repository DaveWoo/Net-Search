using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Models
{
	public class SiteInfo
	{
		public String Url { get; set; }
		public String Name { get; set; }
		public int Alexa { get; set; }
		public int RankBaidu { get; set; }
		public int PR { get; set; }
		public int Linked { get; set; }
		public int LinkedReversed { get; set; }
		public String Description { get; set; }
		public int Score { get; set; }
		public DateTime CreatedTimeStamp { get; set; }
		public DateTime UpdatedTimeStamp { get; set; }
	}
}
