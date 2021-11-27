using System;
using System.Collections.Generic;

namespace CervusDama.Data.Model.ArticleModel
{
	public class ArticleBigListModel
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public int Hit { get; set; }

		public string Image { get; set; }

		public DateTime CreateAt { get; set; }

		public string CreateAtString { get; set; }

		public string Slug { get; set; }

		public byte Status { get; set; }

		public int AuthID { get; set; }

		public string SelectedTicket { get; set; }

		public UserModel.UserInfoModel UserInfo { get; set; }

		public List<TicketModel.TicketListModel> Tickets { get; set; }

		public CategoryModel.CategoryAllListModel BaseCategory { get; set; }

		public List<CategoryModel.CategoryAllListModel> Categories { get; set; }
	}
}
