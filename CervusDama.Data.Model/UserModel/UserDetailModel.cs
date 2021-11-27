using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.UserModel
{
	public class UserDetailModel
	{
		public int ID { get; set; }

		public string RoleName { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string NickName { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string Tier { get; set; }

		public string CreateAt { get; set; }

		public string LastLogin { get; set; }

		public string Slug { get; set; }

		public byte Status { get; set; }

		public int ArticleCount { get; set; }

		public int ArticleLikeCount { get; set; }
		
		public int ArticleDisLikeCount { get; set; }
		
		public int CommentCount { get; set; }
		
		public int QuestionCount { get; set; }
		
		public int CodeCount { get; set; }
	}
}
