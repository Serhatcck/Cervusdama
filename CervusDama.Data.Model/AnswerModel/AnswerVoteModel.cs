using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.AnswerModel
{
    public class AnswerVoteModel
    {
        [Required(ErrorMessage = "Cevap bilgisi gönderilmeli.")]
        public int AnswerID { get; set; }

        public bool VoteType { get; set; }
    }
}
