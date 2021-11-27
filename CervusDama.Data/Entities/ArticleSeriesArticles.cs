using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Entities
{
	[Table("ArticleSeriesArticles")]
	public class ArticleSeriesArticles
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int ArticleSeriesCategoryID { get; set; }

		[Required]
		public int ArticleID { get; set; }

		public virtual ArticleSeriesCategory ArticleSeriesCategory { get; set; }

		public virtual Article Article { get; set; }
	}
}
