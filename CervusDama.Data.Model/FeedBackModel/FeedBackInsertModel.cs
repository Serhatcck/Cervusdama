using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CervusDama.Data.Model.FeedBackModel
{
	public class FeedBackInsertModel
	{
		[Required(ErrorMessage = "Geri bildirim türü seçmelisiniz.")]
		[Range(1, 6, ErrorMessage = "Geçerli bir geri bildirim türü seçilmeli.")]
		public int FeedBackType { get; set; }

		[Required(ErrorMessage = "Geri bildirim mesajınızı girmelisiniz.")]
		[MaxLength(2000, ErrorMessage = "Girdiğiniz mesaj çok uzun en fazla 2000 karakterlik mesaj girilebilir.")]
		public string Content { get; set; }

		public SelectList FeedBackTypeList { get; set; }
	}
}
