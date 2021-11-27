using CervusDama.Data.Model.ArticleModel;
using CervusDama.Data.Model.SeacrhModel;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
    public class SearchController : Base.BaseController
    {
        // GET: Search
        public ActionResult Index(string query)
        {
			if (!String.IsNullOrEmpty(query))
			{
                SearchModel result = new SearchModel();
                result.SearchData = null;

                result.Articles = dbContext.Article.Where(a => a.Title.Contains(query)).OrderByDescending(o => o.CreateAt).Take(10).Select(s => new ArticleBigListModel() { 
                    ID = s.ID,
                    CreateAt = s.CreateAt,
                    Description = s.Description,
                    Title = s.Title,
                    Slug = s.Slug,
                    Hit = s.Hit,
                    UserInfo = new Data.Model.UserModel.UserInfoModel()
					{
                        FirstName = s.User.FirstName,
                        LastName = s.User.LastName,
                        Slug = s.User.Slug
					}
                }).ToList();

                return View(result);
			}

            return View();
        }

        [HttpPost]
        public ActionResult Index(DetailSearchModel data)
		{
			if (String.IsNullOrEmpty(data.SearchQuery))
			{
                //error : aranacak ifade boş olamaz.
			}

            Expression<Func<Data.Entities.Article, bool>> expr = PredicateBuilder.New<Data.Entities.Article>(false);

            if (data.InTitle)
			{
                expr = expr.Or(x => x.Title.Contains(data.SearchQuery));
			}

			if (data.InContent)
			{
                expr = expr.Or(x => x.Content.Contains(data.SearchQuery));
            }

			if (data.InCategory)
			{
                expr = expr.Or(x => x.Categories.Any(c=>c.Title.Contains(data.SearchQuery)));
			}

			if (data.InTicket)
			{
                expr = expr.Or(x => x.Tickets.Any(c => c.Title.Contains(data.SearchQuery)));
            }

			if (data.StartAt.HasValue)
			{
                expr = expr.And(x=>x.CreateAt >= data.StartAt.Value);
			}

			if (data.EndAt.HasValue)
			{
                expr = expr.And(x=>x.CreateAt <= data.EndAt.Value);
			}

            SearchModel result = new SearchModel();
            result.SearchData = data;

            result.Articles = dbContext.Article.Where(expr).OrderByDescending(o=>o.CreateAt).Take(10).Select(s => new ArticleBigListModel()
            {
                ID = s.ID,
                CreateAt = s.CreateAt,
                Description = s.Description,
                Title = s.Title,
                Slug = s.Slug,
                Hit = s.Hit,
                UserInfo = new Data.Model.UserModel.UserInfoModel()
                {
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    Slug = s.User.Slug
                }
            }).ToList();

            return View(result);
        }
    }
}