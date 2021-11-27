using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.CommentModel
{
	public class CommentInsertModel
	{
		[Required(ErrorMessage = "Yorum eklenecek makale seçilmeli.")]
		public int ArticleID { get; set; }

		[Required(ErrorMessage = "Boş yorum eklenemez.")]
		[MaxLength(5000, ErrorMessage = "Yorumunuz en fazla 5000 karakter olabilir.")]
		public string Content { get; set; }

		[Required(ErrorMessage = "Yorum eklenecek makale seçilmeli.")]
		public string Slug { get; set; }
	}
}
