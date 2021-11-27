using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.SeacrhModel
{
	public class DetailSearchModel
	{
		public string SearchQuery { get; set; }

		public bool InTitle { get; set; }

		public bool InContent { get; set; }

		public bool InCategory { get; set; }

		public bool InTicket { get; set; }

		public DateTime? StartAt { get; set; }

		public DateTime? EndAt { get; set; } 
	}
}
