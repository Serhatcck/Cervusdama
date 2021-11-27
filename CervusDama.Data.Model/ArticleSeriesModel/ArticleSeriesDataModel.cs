using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.ArticleSeriesModel
{
	public class ArticleSeriesDataModel
	{
		[Required(ErrorMessage = "kategori için bir isim giriniz.")]
		public string CategoryTitle { get; set; }

		[Required(ErrorMessage = "Kategori için makale seçmelisiniz.")]
		public int[] Articles { get; set; }
	}
}
