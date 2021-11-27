using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.ArticleSeriesModel
{
	public class ArticleSeriesInsertModel
	{
		[Required(ErrorMessage = "Makale serisi için bir isim girmelisiniz.")]
		[MaxLength(100, ErrorMessage = "Makale serisi ismi en fazla 100 karakter olabilir.")]
		public string SeriesName { get; set; }

		[Required(ErrorMessage = "Makale serisi için kısa açıklama girmelisiniz.")]
		[MaxLength(1000, ErrorMessage = "Makale serisi açıklaması en fazla 1000 karakter olabilir.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Makale serisi için öne çıkmış görsel seçmelisiniz")]
		public int ImageID { get; set; }

		[Required(ErrorMessage = "Makale serisi için kategorileri tanımlamalı ve makaleleri seçmelisiniz.")]
		public List<ArticleSeriesDataModel> ArticleSeriesData { get; set; }
	}
}
