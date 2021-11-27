using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.AnswerModel
{
	public class AnswerEditModel
	{
		[Required(ErrorMessage = "ID değeri eksik.")]
		public int ID { get; set; }

		[Required(ErrorMessage = "Soru içeriği zorunludur.")]
		[MaxLength(5000, ErrorMessage = "Cevabınız en fazla 5000 karakter olabilir.")]
		public string Content { get; set; }
	}
}
