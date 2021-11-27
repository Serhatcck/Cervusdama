using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.ArticleModel
{
	public class ArticleVoteModel
	{
		[Required(ErrorMessage = "Makale bilgisi gönderilmeli.")]
		public int ArticleID { get; set; }

		public bool VoteType { get; set; }
	}
}
