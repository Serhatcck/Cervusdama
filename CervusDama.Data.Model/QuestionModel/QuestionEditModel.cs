using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.QuestionModel
{
    public class QuestionEditModel
    {
		public int ID { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }


		public string Slug { get; set; }

		public List<TicketModel.TicketListModel> Tickets { get; set; }

		public string TicketList { get; set; }
	}
}
