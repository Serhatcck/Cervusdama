using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("Comment")]
	public class Comment
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int UserID { get; set; }

		[Required]
		public int ArticleID { get; set; }

		[Required]
		[MaxLength(5000)]
		public string Content { get; set; }

		[Required]
		public DateTime CreateAt { get; set; }

		[Required]
		public byte Status { get; set; }

		public virtual User User { get; set; }

		public virtual Article Article { get; set; }
	}
}
