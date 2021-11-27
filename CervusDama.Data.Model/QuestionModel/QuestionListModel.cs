using CervusDama.Data.Model.TicketModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.QuestionModel
{
	public class QuestionListModel
	{
		public int ID { get; set; }

		public int UserID { get; set; }
		
		public string FirstName { get; set; }
		
		public string LastName { get; set; }
		
		public string UserSlug { get; set; }

		public string Title { get; set; }
		
		public string Content { get; set; }
		
		public DateTime CreateAt { get; set; }

		public int AnswerCount { get; set; }

		public int Hit { get; set; }

		public bool  IsSolved { get; set; }
		
		public string Slug { get; set; }
		
		public byte Status { get; set; }
		
		public List<TicketListModel> Tickets { get; set; }


	}
}
