using System;

namespace CervusDama.Data.Model.ArticleSeriesModel
{
	public class ArticleSeriesListModel
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public int CategoryCount { get; set; }

		public int ArticleCount { get; set; }

		public DateTime CreateAt { get; set; }

		public string Slug { get; set; }

		public string CategoryTitle { get; set; }

		public string CategorySlug { get; set; }

		public byte Status { get; set; }
	}
}
