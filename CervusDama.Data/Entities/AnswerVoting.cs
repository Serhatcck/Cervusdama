using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("AnswerVoting")]
	public class AnswerVoting
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int UserID { get; set; }

		[Required]
		public int AnswerID { get; set; }

		[Required]
		public bool VoteType { get; set; }

		public virtual User User { get; set; }

		public virtual Answer Answer { get; set; }
	}
}
