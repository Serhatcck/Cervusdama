using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.CommentModel
{
	public class ArticleCommentModel
	{
		public int ID { get; set; }

		public string Content { get; set; }

		public DateTime CreateAt { get; set; }

		public int LikeCount { get; set; }

		public int DisLikeCount { get; set; }

		public UserModel.UserInfoModel UserInfo { get; set; }
	}
}
