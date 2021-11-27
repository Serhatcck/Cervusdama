using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.FeedBackModel
{
	public class FeedBackListModel
	{
		public int ID { get; set; }

		public int FeedBackType { get; set; }

		public string FeedBackTypeText { get; set; }

		public string Content { get; set; }

		public DateTime CreateAt { get; set; }

		public byte Status { get; set; }
	}
}
