using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("ArticleVoting")]
	public class ArticleVoting
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int UserID { get; set; }

		[Required]
		public int ArticleID { get; set; }

		[Required]
		public bool VoteType { get; set; }

		public virtual User User { get; set; }

		public virtual Article Article { get; set; }
	}
}
