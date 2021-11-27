using CervusDama.Data.Model.ArticleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
    public class HomeController : Base.BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rules()
		{
            return View();
		}

        public ActionResult LastArticles()
		{
            var articles = dbContext.Article.Where(a => a.Status == 1).OrderByDescending(o => o.CreateAt).Take(3).Select(s => new ArticleBigListModel()
			{
				ID = s.ID,
				Title = s.Title,
				Description = s.Description,
				Hit = s.Hit,
				CreateAt = s.CreateAt,
				Image = s.Media.Title,
				Slug = s.Slug,
				UserInfo = new Data.Model.UserModel.UserInfoModel
				{
					ID = s.User.ID,
					FirstName = s.User.FirstName,
					LastName = s.User.LastName,
					Slug = s.User.Slug
				}
			}).ToList();

			return View(articles);
		}
    }
}