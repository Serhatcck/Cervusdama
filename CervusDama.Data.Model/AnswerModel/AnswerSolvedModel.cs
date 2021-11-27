using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.AnswerModel
{
    public class AnswerSolvedModel
    {
        [Required(ErrorMessage = "Cevap bilgisi gönderilmeli.")]
        public int AnswerID { get; set; }

        [Required(ErrorMessage ="Soru bilgisi gönderilmeli.")]
        public int QuestionID{ get; set; }
    }
}
