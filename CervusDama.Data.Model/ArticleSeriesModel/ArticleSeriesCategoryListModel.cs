using System.Collections.Generic;

namespace CervusDama.Data.Model.ArticleSeriesModel
{
	public class ArticleSeriesCategoryListModel
	{
		public string Title { get; set; }

		public string Slug { get; set; }

		public List<ArticleSeriesListModel> Categories { get; set; }
	}
}
