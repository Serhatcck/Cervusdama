using CervusDama.Authorization;
using CervusDama.Data.Model.ArticleModel;
using CervusDama.Data.Model.UtilModel;
using CervusDama.Utility.PagerHelper;
using CervusDama.Utility.StringHelper;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CervusDama.Admin.Controllers
{
	[Authorization(Permission = "Administrator,Moderator")]
	public class ArticleController : Base.BaseController
	{
		// GET: Article
		public ActionResult Index(int? page)
		{
			if (!page.HasValue)
				page = 1;

			int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);

			var articles = dbContext.Article.Where(a => a.Status != 0).OrderByDescending(o => o.ID).ToPagedList(page.Value, getDataSize).Select(s => new ArticleListModel
			{
				ID = s.ID,
				Title = s.Title,
				CreateAt = s.CreateAt,
				UserName = s.User.FirstName + " " + s.User.LastName,
				CommentCount = dbContext.Comment.Count(c => c.ArticleID == s.ID),
				Hit = s.Hit,
				LikeCount = dbContext.ArticleVoting.Count(c=> c.ArticleID == s.ID && c.VoteType == true),
				DislikeCount = dbContext.ArticleVoting.Count(c=> c.ArticleID == s.ID && c.VoteType == false),
				Status = s.Status,
				IsPinned = s.IsPinned
			}).ToList();

			ViewBag.currentPage = page.Value;
			ViewBag.totalData = dbContext.Article.Count();

			return View(articles);
		}

		public ActionResult Insert()
		{
			return View();
		}

		[HttpPost]
		[ValidateInput(false)]
		public JsonResult Insert(ArticleInsertModel data)
		{
			if (ModelState.IsValid)
			{
				string articleSlug = StringHelper.Slug(data.Title);

				if (!dbContext.Article.Any(a => a.Title.Equals(data.Title) || a.Slug.Equals(articleSlug)))
				{
					var article = dbContext.Article.Create();

					//Yazı kategorilerini ekle.
					foreach (var item in data.CategoryID)
					{
						if (dbContext.Category.Any(c => c.ID == item))
						{
							article.Categories.Add(dbContext.Category.Single(c => c.ID == item));
						}
					}

					//Yazı etiketlerini ekle
					string[] tickets = data.Tickets.Split(';');

					foreach (var item in tickets)
					{
						string ticketSlug = StringHelper.Slug(item);

						if (dbContext.Ticket.Any(t => t.Title.Equals(item) && t.Slug.Equals(ticketSlug)))
						{
							article.Tickets.Add(dbContext.Ticket.FirstOrDefault(t => t.Title.Equals(item) && t.Slug.Equals(ticketSlug)));
						}
						else
						{
							article.Tickets.Add(new Data.Entities.Ticket { Title = item, Slug = ticketSlug });
						}
					}

					//Diğer bilgiler.
					article.Content = data.Content;
					article.Title = data.Title;
					article.Slug = articleSlug;
					article.CreateAt = DateTime.Now;
					article.Hit = 0;
					article.IsComment = data.IsComment;
					article.IsPinned = data.IsPinned;
					article.MediaID = data.MediaID;
					article.Status = data.Status;
					article.UserID = int.Parse(User.Identity.Name);
					article.ViewState = data.ViewState;

					dbContext.Article.Add(article);

					try
					{
						dbContext.SaveChanges();
						return Json(new { status = true, message = "Yazı başarıyla eklendi." }, JsonRequestBehavior.AllowGet);
					}
					catch (Exception ex)
					{
						return Json(new { status = false, message = "Kayıt işlemi sırasında bir hata oluştu." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { status = false, message = "Sistemde aynı isimde yazı mevcut. Yazınıza başka bir isim veriniz." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Gerekli alanları doldurmalısınız." }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult Edit(int id)
		{
			return View();
		}

		[HttpPost]
		public JsonResult AddPin(int id)
		{
			if (dbContext.Article.Any(a => a.ID == id))
			{
				var article = dbContext.Article.Single(a => a.ID == id);

				if (article.IsPinned.HasValue)
					article.IsPinned = null;
				else
					article.IsPinned = 1;

				dbContext.SaveChanges();

				if (article.IsPinned.HasValue)
					return Json(new { status = true, message = "Yazı anasayfaya sabitlendi." }, JsonRequestBehavior.AllowGet);
				else
					return Json(new { status = true, message = "Yazı anasayfadan kaldırıldı." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "İşlem yapılmak istenen yazı sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}
		
		[HttpPost]
		public JsonResult ArticleBan(int id)
		{
			if (dbContext.Article.Any(a => a.ID == id))
			{
				var article = dbContext.Article.Single(a => a.ID == id);

				if (article.Status == 1)
					article.Status = 2;
				else
					article.Status = 1;

				dbContext.SaveChanges();

				if (article.Status == 1)
					return Json(new { status = true, message = "Yazı yayına alındı.." }, JsonRequestBehavior.AllowGet);
				else
					return Json(new { status = true, message = "Yazı yayından kaldırıldı." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "İşlem yapılmak istenen yazı sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult Delete(int id)
		{
			if(dbContext.Article.Any(a => a.ID == id))
			{
				var article = dbContext.Article.Single(a => a.ID == id);
				article.Status = 0;

				dbContext.SaveChanges();

				return Json(new { status = true, message = "Yazı sistemden silindi." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "İşlem yapılmak istenen yazı sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult CategoryList()
		{
			var categories = dbContext.Category.Select(s => new ListModel
			{
				Text = s.Title,
				ValueInt = s.ID
			}).ToList();

			return View(categories);
		}
	}
}