using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("Media")]
	public class Media
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int UserID { get; set; }

		[Required]
		[MaxLength(200)]
		public string Title { get; set; }

		[Required]
		[MaxLength(500)]
		public string Description { get; set; }

		[Required]
		[MaxLength(60)]
		public string MediaType { get; set; }

		[Required]
		public DateTime CreateAt { get; set; }

		public DateTime? EditAt { get; set; }

		[Required]
		public byte Status { get; set; }

		public virtual User User { get; set; }

	}
}
