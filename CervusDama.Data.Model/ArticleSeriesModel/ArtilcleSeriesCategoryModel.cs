using System.Collections.Generic;

namespace CervusDama.Data.Model.ArticleSeriesModel
{
	public class ArtilcleSeriesCategoryModel
	{
		public string Title { get; set; }

		public int Order { get; set; }

		public List<int> SelectedArticles { get; set; }
	}
}
