using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Entities
{
	[Table("FeedBack")]
	public class FeedBack
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		public int? UserID { get; set; }

		[Required]
		[MaxLength(16)]
		public string SenderIP { get; set; }

		[Required]
		[Range(1,6)]
		public int FeedBackType { get; set; }

		[Required]
		[MaxLength(2000)]
		public string Content { get; set; }

		[Required]
		public DateTime CreateAt { get; set; }

		[Required]
		public byte Status { get; set; }

		public virtual User User { get; set; }
	}
}
