using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.ProfileModel
{
	public class ProfileModel
	{
		public int ID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string WebSite { get; set; }

		public string CityName { get; set; }

		public string Bio { get; set; }

		public DateTime CreateAt { get; set; }

		public string Slug { get; set; }

		public int ArticleCount { get; set; }

		public int QuestionCount { get; set; }

		public int AnswerCount { get; set; }

		public int CommentCount { get; set; }
	}
}
