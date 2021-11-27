using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Entities
{
	[Table("ArticleSeries")]
	public class ArticleSeries
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		public int? MediaID { get; set; }

		[Required]
		[MaxLength(200)]
		public string Title { get; set; }

		[Required]
		[MaxLength(1000)]
		public string Description { get; set; }

		[Required]
		public DateTime CreateAt { get; set; }

		[Required]
		[MaxLength(200)]
		public string Slug { get; set; }

		[Required]
		public byte Status { get; set; }

		public virtual List<ArticleSeriesCategory> ArticleSeriesCategories { get; set; }

		public virtual Media Media { get; set; }
	}
}
