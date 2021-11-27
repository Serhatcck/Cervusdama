using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("Category")]
	public class Category
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		public int? ParentID { get; set; }

		[Required]
		[MaxLength(50)]
		public string Title { get; set; }

		[Required]
		[MaxLength(1000)]
		public string Description { get; set; }

		[MaxLength(100)]
		public string Icon { get; set; }

		[MaxLength(100)]
		public string Color { get; set; }

		[Required]
		public byte Status { get; set; }

		[Required]
		[MaxLength(50)]
		public string Slug { get; set; }

		public virtual Category Parent { get; set; }

		private ICollection<Article> _articles;

		public virtual ICollection<Article> Articles
		{
			get { return _articles ?? (_articles = new HashSet<Article>()); }
			protected set { _articles = value; }
		}
	}
}
