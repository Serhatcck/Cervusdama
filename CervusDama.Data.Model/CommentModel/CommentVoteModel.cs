using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.CommentModel
{
	public class CommentVoteModel
	{
		[Required(ErrorMessage = "Yorum bilgisi gönderilmeli.")]
		public int CommentID { get; set; }

		public bool VoteType { get; set; }
	}
}
