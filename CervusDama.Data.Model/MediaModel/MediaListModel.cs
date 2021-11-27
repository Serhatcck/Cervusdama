using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.MediaModel
{
	public class MediaListModel
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public string MediaType { get; set; }

		public string Description { get; set; }

		public DateTime CreateAt { get; set; }
	}
}
