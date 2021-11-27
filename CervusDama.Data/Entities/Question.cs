using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("Question")]
	public class Question
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int UserID { get; set; }

		[Required]
		[MaxLength(200)]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		[Required]
		public DateTime CreateAt { get; set; }

		[Required]
		public int Hit { get; set; }

		[Required]
		public bool IsSolved { get; set; }

		[Required]
		[MaxLength(200)]
		public string Slug { get; set; }

		[Required]
		public byte Status { get; set; }

		public virtual User User { get; set; }

		//public virtual List<Answer> Answers { get; set; }

		private ICollection<Ticket> _tickets;

		public virtual ICollection<Ticket> Tickets
		{
			get { return _tickets ?? (_tickets = new HashSet<Ticket>()); }
			protected set { _tickets = value; }
		}
	}
}
