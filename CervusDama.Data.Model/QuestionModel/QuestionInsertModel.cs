using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.QuestionModel
{
    public class QuestionInsertModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur.")]
        [MaxLength(300, ErrorMessage = "Soru başlığı en fazla 300 karakter olabilir.")]
        [RegularExpression(@"[a-zA-Z0-9;\sçöışüğÇÖİŞÜĞ\-]+", ErrorMessage = "Soru başlığı standartlara uygun değil.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "İçerik zorunludur. ")]
        public string Content { get; set; }

        [RegularExpression(@"[a-zA-Z0-9;\sçöışüğÇÖİŞÜĞ]+", ErrorMessage = "Etiket ismi standartlara uygun değil.")]
        public string Tickets { get; set; }


    }
}
