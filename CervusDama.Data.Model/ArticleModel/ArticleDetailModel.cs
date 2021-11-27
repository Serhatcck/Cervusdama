using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.ArticleModel
{
	public class ArticleDetailModel
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime CreateAt { get; set; }

		public int Hit { get; set; }

		public int LikeCount { get; set; }

		public int DisLikeCount { get; set; }

		public string Slug { get; set; }

		public byte Status { get; set; }

		public string CategoryIcon { get; set; }

		public UserModel.UserInfoModel UserInfo { get; set; }

		public List<TicketModel.TicketListModel> Tickets { get; set; }

		public List<CommentModel.ArticleCommentModel> Comments { get; set; }
	}
}
