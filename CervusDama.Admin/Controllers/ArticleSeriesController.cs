using CervusDama.Authorization;
using CervusDama.Data.Model.ArticleModel;
using CervusDama.Data.Model.ArticleSeriesModel;
using CervusDama.Utility.PagerHelper;
using CervusDama.Utility.StringHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Admin.Controllers
{
	[Authorization(Permission = "Administrator,Moderator")]
	public class ArticleSeriesController : Base.BaseController
	{
		// GET: ArticleSeries
		public ActionResult Index(int? page)
		{
			if (!page.HasValue)
				page = 1;

			int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);

			var articleSeries = dbContext.ArticleSeries.OrderByDescending(o => o.ID).ToPagedList(page.Value, getDataSize).Select(s => new ArticleSeriesListModel
			{
				ID = s.ID,
				Title = s.Title,
				CreateAt = s.CreateAt,
				ArticleCount = dbContext.Article.Count(a => dbContext.ArticleSeriesArticles.Any(c => c.ArticleID == a.ID && c.ArticleSeriesCategory.ArticleSeriesID == s.ID)),
				CategoryCount = dbContext.ArticleSeriesCategories.Count(c => c.ArticleSeriesID == s.ID),
				Slug = s.Slug,
				Status = s.Status
			}).ToList();

			ViewBag.currentPage = page.Value;
			ViewBag.totalData = dbContext.ArticleSeries.Count();

			return View(articleSeries);
		}

		public ActionResult Insert()
		{
			return View();
		}

		[HttpPost]
		public JsonResult Insert(ArticleSeriesInsertModel data)
		{
			if (ModelState.IsValid)
			{
				var articleSeries = dbContext.ArticleSeries.Create();
				articleSeries.CreateAt = DateTime.Now;
				articleSeries.Slug = StringHelper.Slug(data.SeriesName);
				articleSeries.Title = data.SeriesName;
				articleSeries.Description = data.Description;
				articleSeries.MediaID = data.ImageID;
				articleSeries.Status = 1;

				dbContext.ArticleSeries.Add(articleSeries);

				foreach (var item in data.ArticleSeriesData)
				{
					var category = dbContext.ArticleSeriesCategories.Create();
					category.ArticleSeries = articleSeries;
					category.Title = item.CategoryTitle;
					category.Slug = StringHelper.Slug(item.CategoryTitle);
					category.ArticleSeriesArticles = new List<Data.Entities.ArticleSeriesArticles>();


					foreach (int article in item.Articles)
					{
						category.ArticleSeriesArticles.Add(new Data.Entities.ArticleSeriesArticles
						{
							ArticleID = article,
							ArticleSeriesCategoryID = category.ID
						});
					}

					dbContext.ArticleSeriesCategories.Add(category);
				}

				dbContext.SaveChanges();

				return Json(new { status = true, message = "Seri makale ekleme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "Gönderilen verilerde eksiklikler var! Kontrol ederek tekrar gönderiniz." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult Search(string q)
		{
			if (String.IsNullOrEmpty(q))
			{
				return Json(new { status = false }, JsonRequestBehavior.AllowGet);
			}

			var article = dbContext.Article.Where(a => a.Title.Contains(q)).OrderBy(o => o.CreateAt).Select(s => new ArticleListModel()
			{
				Title = s.Title,
				ID = s.ID
			}).ToList();

			return Json(new { status = true, message = "basarili", data = article }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Delete(int id)
		{
			if(dbContext.ArticleSeries.Any(a => a.ID == id))
			{
				var articleSeries = dbContext.ArticleSeries.Single(a => a.ID == id);

				foreach (var item in dbContext.ArticleSeriesCategories.Where(c => c.ArticleSeriesID == id).ToList())
				{
					foreach (var article in dbContext.ArticleSeriesArticles.Where(a => a.ArticleSeriesCategoryID == item.ID).ToList())
					{
						dbContext.ArticleSeriesArticles.Remove(article);
					}

					dbContext.ArticleSeriesCategories.Remove(item);
				}

				dbContext.ArticleSeries.Remove(articleSeries);

				dbContext.SaveChanges();

				return Json(new { status = true, message = "Seri makale sistemden silindi." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "İşlem yapmak istenen makale serisi bulunamadı." });
			}
		}

		[HttpPost]
		public JsonResult Block(int id)
		{
			if(dbContext.ArticleSeries.Any(a => a.ID == id))
			{
				var articleSeries = dbContext.ArticleSeries.Single(a => a.ID == id);
				articleSeries.Status = (byte)((articleSeries.Status == 1) ? 0 : 1);
				dbContext.SaveChanges();

				if(articleSeries.Status == 1)
				{
					return Json(new { status = true, message = "Makale serisi tekrar yayında." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = true, message = "Makale serisi yayından kaldırıldı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "İşlem yapmak istenen makale serisi sitemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}