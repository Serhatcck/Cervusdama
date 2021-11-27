using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.TicketModel
{
	public class TicketInsertModel
	{
		public int ID { get; set; }

		[Required(ErrorMessage = "Etiket ismi zorunludur.")]
		[MaxLength(20, ErrorMessage = "Etiket en fazla 20 karakter uzunluğunda olabilir.")]
		public string Title { get; set; }

		[MaxLength(20, ErrorMessage = "Slug değeri en fazla 20 karakter uzunluğunda olabilir.")]
		public string Slug { get; set; }
	}
}
