using CervusDama.Data.Model.ArticleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
    public class TicketController : Base.BaseController
    {
        // GET: Ticket
        public ActionResult Index(string slug)
        {
			List<ArticleBigListModel> articles = new List<ArticleBigListModel>();

			if (!String.IsNullOrEmpty(slug))
			{
				if (dbContext.Ticket.Any(t => t.Slug.Equals(slug)))
				{
					var ticket = dbContext.Ticket.FirstOrDefault(t => t.Slug.Equals(slug));

					articles = dbContext.Article.Where(a => a.Status == 1 && a.Tickets.Any(t => t.Slug.Equals(slug))).OrderByDescending(o => o.CreateAt).Take(20).Select(s => new ArticleBigListModel()
					{
						ID = s.ID,
						Title = s.Title,
						Description = s.Description,
						Hit = s.Hit,
						CreateAt = s.CreateAt,
						Image = s.Media.Title,
						Slug = s.Slug,
						SelectedTicket = ticket.Title,
						UserInfo = new Data.Model.UserModel.UserInfoModel
						{
							ID = s.User.ID,
							FirstName = s.User.FirstName,
							LastName = s.User.LastName,
							Slug = s.User.Slug
						}
					}).ToList();
				}
			}

            return View(articles);
        }
    }
}