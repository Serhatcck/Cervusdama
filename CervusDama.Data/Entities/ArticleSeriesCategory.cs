using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Entities
{
	[Table("ArticleSeriesCategory")]
	public class ArticleSeriesCategory
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int ArticleSeriesID { get; set; }

		[Required]
		[MaxLength(200)]
		public string Title { get; set; }

		[Required]
		[MaxLength(200)]
		public string Slug { get; set; }

		public virtual ArticleSeries ArticleSeries { get; set; }

		public virtual List<ArticleSeriesArticles> ArticleSeriesArticles { get; set; }
	}
}
