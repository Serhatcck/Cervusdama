using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("Answer")]
	public class Answer
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int UserID { get; set; }

		[Required]
		public int QuestionID { get; set; }

		[Required]
		public string Content { get; set; }

		[Required]
		public DateTime CreateAt { get; set; }

		[Required]
		public byte Status { get; set; }

		public bool isSolved { get; set; }
		public virtual User User { get; set; }

		public virtual Question Question { get; set; }
	}
}
