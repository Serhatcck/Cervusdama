using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("CommentVoting")]
	public class CommentVoting
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int UserID { get; set; }

		[Required]
		public int CommentID { get; set; }

		[Required]
		public bool VoteType { get; set; }

		public virtual User User { get; set; }

		public virtual Comment Comment { get; set; }
	}
}
