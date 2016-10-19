using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Models
{
	interface IID
	{
		long Id { get; set; }
	}

	interface IUrl
	{
		string Url { get; set; }
	}
}
