using CervusDama.Data.Model.ArticleSeriesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
	public class ArticleSeriesController : Base.BaseController
	{
		public ActionResult Index()
		{
			var articleSeries = dbContext.ArticleSeries.Where(s => s.Status == 1).OrderByDescending(o => o.CreateAt).Take(20).Select(s => new ArticleSeriesMainModel()
			{
				ID = s.ID,
				CreateAt = s.CreateAt,
				Description = s.Description,
				Title = s.Title,
				Slug = s.Slug,
				MediaName = s.Media.Title
			}).ToList();

			return View(articleSeries);
		}

		public ActionResult Detail(string articleSeriesSlug, string articleSlug)
		{
			if (String.IsNullOrEmpty(articleSeriesSlug))
			{
				return RedirectToRoute("ArticleSeriesAbsent");
			}
			else
			{
				if (!dbContext.ArticleSeries.Any(a => a.Slug.Equals(articleSeriesSlug)))
				{
					return RedirectToRoute("ArticleSeriesAbsent");
				}
				else
				{
					var articleSeries = dbContext.ArticleSeries.Single(a => a.Slug.Equals(articleSeriesSlug));

					Data.Entities.Article selectedArticle = new Data.Entities.Article();

					if (!String.IsNullOrEmpty(articleSlug)) {
						if (dbContext.Article.Any(a => a.Slug.Equals(articleSlug)))
						{
							selectedArticle = dbContext.Article.Single(a => a.Slug.Equals(articleSlug));
						}
						else
						{
							return RedirectToRoute("ArticleSeriesAbsent");
						}
					}
					else
					{
						selectedArticle = dbContext.ArticleSeriesCategories.FirstOrDefault(c => c.ArticleSeriesID == articleSeries.ID).ArticleSeriesArticles.FirstOrDefault().Article;
					}

					ArticleSeriesModel model = new ArticleSeriesModel();
					model.Title = articleSeries.Title;
					model.Slug = articleSeries.Slug;

					model.Article = new Data.Model.ArticleModel.ArticleDetailModel();

					model.Article.Content = selectedArticle.Content;
					model.Article.Title = selectedArticle.Title;
					model.Article.CreateAt = selectedArticle.CreateAt;
					model.Article.CategoryIcon = dbContext.Category.FirstOrDefault(c => c.Articles.Any(a => a.ID == selectedArticle.ID)).Icon;

					return View(model);
				}
			}


			return View();
		}

		public ActionResult SeriesCategoryList(string slug)
		{
			if (String.IsNullOrEmpty(slug))
			{
				return RedirectToRoute("ArticleSeriesAbsent");
			}
			else
			{
				if (!dbContext.ArticleSeries.Any(a => a.Slug.Equals(slug)))
				{
					return RedirectToRoute("ArticleSeriesAbsent");
				}
				else
				{
					var articleSeries = dbContext.ArticleSeries.Single(a => a.Slug.Equals(slug));

					ArticleSeriesCategoryListModel model = new ArticleSeriesCategoryListModel();
					model.Title = articleSeries.Title;
					model.Slug = articleSeries.Slug;
					model.Categories = (from s in dbContext.Article
										join d in dbContext.ArticleSeriesArticles on s.ID equals d.ArticleID
										join e in dbContext.ArticleSeriesCategories on d.ArticleSeriesCategoryID equals e.ID
										where s.Status == 1 && e.ArticleSeries.Slug.Equals(slug)
										select new ArticleSeriesListModel
										{
											ID = s.ID,
											Title = s.Title,
											Slug = s.Slug,
											CategoryTitle = e.Title,
											CategorySlug = e.Slug
										}).ToList();

					return View(model);
				}
			}
		}

		public ActionResult ArticleSeriesAbsent()
		{
			return View();
		}
	}
}