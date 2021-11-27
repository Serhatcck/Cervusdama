namespace CervusDama.Data.Model.ArticleSeriesModel
{
	public class ArticleSeriesModel
	{
		public string Title { get; set; }

		public string Slug { get; set; }

		public ArticleModel.ArticleDetailModel Article { get; set; }
	}
}
