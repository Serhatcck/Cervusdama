using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.ArticleModel
{
	public class ArticleInsertModel
	{
		[Required(ErrorMessage = "Başlık alanı zorunludur.")]
		[MaxLength(300, ErrorMessage = "Yazı başlığı en fazla 300 karakter olabilir.")]
		[RegularExpression(@"[a-zA-Z0-9;\sçöışüğÇÖİŞÜĞ#\-]+", ErrorMessage = "Yazı başlığı standartlara uygun değil.")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Yazı metni zorunludur.")]
		public string Content { get; set; }

		[Required(ErrorMessage = "Yazı için kısa bir açıklama giriniz.")]
		[MaxLength(500, ErrorMessage = "Yazı için en fazla 500 karakterlik açıklama eklenebilir.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Yazı için kategori seçmelisiniz.")]
		public List<int> CategoryID { get; set; }

		public int? ImageID { get; set; }

		public byte? IsPinned { get; set; }
		
		public byte? IsComment { get; set; }

		public byte? IsDraft { get; set; }

		public byte Status { get; set; }

		public byte ViewState { get; set; }

		[Required(ErrorMessage = "Yazı için etiketler belirlemelisiniz.")]
		[RegularExpression(@"[a-zA-Z0-9;\sçöışüğÇÖİŞÜĞ]+", ErrorMessage = "Etiket ismi standartlara uygun değil.")]
		public string Tickets { get; set; }

		public int? MediaID { get; set; }
	}
}
