using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.QuestionModel
{
    public class QuestionDetailModel
    {
		public int ID { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime CreateAt { get; set; }
		
		public int LikeCount { get; set; }

		public int DisLikeCount { get; set; }

		public string Slug { get; set; }

		public int Hit { get; set; }

		public bool IsSolved { get; set; }

		public byte Status { get; set; }
		
		public string ActionResult { get; set; }
		public UserModel.UserInfoModel UserInfo { get; set; }

		public List<TicketModel.TicketListModel> Tickets { get; set; }

		public List<AnswerModel.QuestionAnswerModel> Answers { get; set; }
		
	}
}
