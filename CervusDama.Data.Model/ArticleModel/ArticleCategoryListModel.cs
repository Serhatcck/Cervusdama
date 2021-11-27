using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.ArticleModel
{
	public class ArticleCategoryListModel
	{
		public List<ArticleBigListModel> Articles { get; set; }

		public CategoryModel.CategoryListModel Category { get; set; } 
	}
}
