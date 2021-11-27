using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.CommentModel
{
	public class CommentEditModel
	{
		[Required(ErrorMessage = "Makale seçilmeli.")]
		public int ArticleID { get; set; }

		[Required(ErrorMessage = "Düzenlenecek yorum seçilmeli.")]
		public int CommentID { get; set; }

		[Required(ErrorMessage = "Makale bilgisi eksik gönderildi.")]
		public string ArticleSlug { get; set; }

		[Required(ErrorMessage = "Yorum içeriği gereklidir.")]
		[MaxLength(5000, ErrorMessage = "Yorum içeriği en fazla 5000 karakter olabilir.")]
		public string CommentContent { get; set; }
	}
}
