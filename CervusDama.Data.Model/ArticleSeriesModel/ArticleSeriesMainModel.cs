using System;

namespace CervusDama.Data.Model.ArticleSeriesModel
{
	public class ArticleSeriesMainModel
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public DateTime CreateAt { get; set; }

		public string Description { get; set; }

		public string MediaName { get; set; }

		public string Slug { get; set; }
	}
}
