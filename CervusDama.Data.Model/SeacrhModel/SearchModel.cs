using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.SeacrhModel
{
	public class SearchModel
	{
		public List<ArticleModel.ArticleBigListModel> Articles { get; set; }

		public DetailSearchModel SearchData { get; set; }
	}
}
