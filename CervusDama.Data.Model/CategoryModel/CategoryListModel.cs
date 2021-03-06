using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.CategoryModel
{
	public class CategoryListModel
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public string Icon { get; set; }

		public string Color { get; set; }

		public string Description { get; set; }

		public string Slug { get; set; }

		public byte Status { get; set; }

		public int ParentID { get; set; }

		public int Hit { get; set; }

	}
}
