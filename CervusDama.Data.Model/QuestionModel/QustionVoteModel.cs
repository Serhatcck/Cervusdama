using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.QuestionModel
{
    public class QustionVoteModel
    {
        [Required(ErrorMessage = "Soru bilgisi gönderilmeli.")]
        public int QuestionId { get; set; }

        public bool VoteType { get; set; }
    }
}
