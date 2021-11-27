using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.ArticleModel
{
	public class ArticleEditModel
	{
		public int ID { get; set; }

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

		[Required(ErrorMessage = "Yazı için etiketler belirlemelisiniz.")]
		[RegularExpression(@"[a-zA-Z0-9;\sçöışüğÇÖİŞÜĞ]+", ErrorMessage = "Etiket ismi standartlara uygun değil.")]
		public string Tickets { get; set; }

		public int? MediaID { get; set; }

		[Required(ErrorMessage = "Makale slug alanı eksik.")]
		public string Slug { get; set; }
	}
}
