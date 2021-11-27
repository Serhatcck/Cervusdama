using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("QuestionVoting")]
	public class QuestionVoting
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int UserID { get; set; }

		[Required]
		public int QuestionID { get; set; }

		[Required]
		public bool VoteType { get; set; }

		public virtual User User { get; set; }

		public virtual Question Question { get; set; }
	}
}
