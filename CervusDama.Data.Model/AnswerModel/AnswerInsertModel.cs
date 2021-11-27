using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.AnswerModel
{
	public class AnswerInsertModel
    {
        [Required(ErrorMessage ="Cevap eklenecek soru seçilmeli.")]
        public int QuestionId { get; set; }
        
        [Required(ErrorMessage = "Boş Cevap Eklenemez.")]
        [MaxLength(5000, ErrorMessage ="Cevabınız en fazla 5000 karakter olabilir.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Cevap eklenecek Soru seçilmeli.")]
        public string Slug { get; set; }
    }
}
