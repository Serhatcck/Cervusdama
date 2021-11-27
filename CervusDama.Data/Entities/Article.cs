using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("Article")]
	public class Article
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int UserID { get; set; }

		public int? MediaID { get; set; }

		[Required]
		[MaxLength(300)]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		[MaxLength(500)]
		public string Description { get; set; }

		[Required]
		public System.DateTime CreateAt { get; set; }

		public System.DateTime? EditAt { get; set; }

		[Required]
		public int Hit { get; set; }

		public byte? IsPinned { get; set; }

		public byte? IsComment { get; set; }

		[Required]
		public byte ViewState { get; set; }

		[Required]
		[MaxLength(300)]
		public string Slug { get; set; }

		[Required]
		public byte Status { get; set; }

		public virtual User User { get; set; }

		public virtual Media Media { get; set; }

		private ICollection<Category> _categories;

		public virtual ICollection<Category> Categories
		{
			get { return _categories ?? (_categories = new HashSet<Category>()); }
			protected set { _categories = value; }
		}

		private ICollection<Ticket> _tickets;

		public virtual ICollection<Ticket> Tickets
		{
			get { return _tickets ?? (_tickets = new HashSet<Ticket>()); }
			protected set { _tickets = value; }
		}
	}
}
