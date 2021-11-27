using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.CategoryModel
{
	public class CategoryAllListModel
	{
		public string Icon { get; set; }

		public string Color { get; set; }

		public string Title { get; set; }

		public string Slug { get; set; }

		public List<CategoryListModel> SubCategories { get; set; }
	}
}
