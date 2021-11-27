using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.ArticleModel
{
	public class ArticleListModel
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public DateTime CreateAt { get; set; }

		public int CommentCount { get; set; }

		public string UserName { get; set; }

		public int LikeCount { get; set; }

		public int DislikeCount { get; set; }

		public int Hit { get; set; }

		public int Status { get; set; }

		public string Slug { get; set; }

		public byte? IsPinned { get; set; }
	}
}
