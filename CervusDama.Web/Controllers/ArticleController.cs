using CervusDama.Authorization;
using CervusDama.Data.Model.ArticleModel;
using CervusDama.Utility.PagerHelper;
using CervusDama.Utility.SecurityHelper;
using CervusDama.Utility.StringHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
	public class ArticleController : Base.BaseController
	{
		// GET: Article
		public ActionResult Index(string slug)
		{
			ArticleCategoryListModel model = new ArticleCategoryListModel();

			if (String.IsNullOrEmpty(slug))
			{
				model.Articles = dbContext.Article.Where(a => a.Status == 1).OrderByDescending(o => o.CreateAt).Take(14).Select(s => new ArticleBigListModel()
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
			}
			else
			{
				if (!dbContext.Category.Any(c => c.Slug.Equals(slug)))
					return RedirectToRoute("CategoryAbsent");

				model.Articles = dbContext.Article.Where(a => a.Status == 1 && a.Categories.Any(c => c.Slug.Equals(slug))).OrderByDescending(o => o.CreateAt).Take(20).Select(s => new ArticleBigListModel()
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

				var category = dbContext.Category.FirstOrDefault(c => c.Slug.Equals(slug));
				model.Category = new Data.Model.CategoryModel.CategoryListModel()
				{
					ID = category.ID,
					Icon = category.Icon,
					Color = category.Color,
					Description = category.Description,
					Slug = category.Slug,
					Title = category.Title
				};
			}

			return View(model);
		}

		public ActionResult Detail(string slug)
		{
			if (String.IsNullOrEmpty(slug))
				return RedirectToRoute("Articles");

			if (!dbContext.Article.Any(a => a.Slug.Equals(slug)))
				return RedirectToRoute("ArticleAbsent");

			var article = dbContext.Article.Include("Tickets").FirstOrDefault(a => a.Slug.Equals(slug));
			article.Hit = article.Hit + 1;
			dbContext.SaveChanges();

			ArticleDetailModel model = new ArticleDetailModel();
			model.Content = article.Content;
			model.CreateAt = article.CreateAt;
			model.Hit = article.Hit;
			model.ID = article.ID;
			model.Slug = article.Slug;
			model.Status = article.Status;
			model.Title = article.Title;
			model.LikeCount = dbContext.ArticleVoting.Count(c => c.ArticleID == article.ID && c.VoteType == true);
			model.DisLikeCount = dbContext.ArticleVoting.Count(c => c.ArticleID == article.ID && c.VoteType == false);

			model.UserInfo = new Data.Model.UserModel.UserInfoModel()
			{
				ID = article.User.ID,
				FirstName = article.User.FirstName,
				LastName = article.User.LastName,
				Slug = article.User.Slug
			};

			model.Tickets = article.Tickets.Select(s => new Data.Model.TicketModel.TicketListModel()
			{
				ID = s.ID,
				Title = s.Title,
				Slug = s.Slug
			}).ToList();

			model.Comments = dbContext.Comment.Where(c => c.ArticleID == article.ID).OrderByDescending(o => o.CreateAt).Select(s => new Data.Model.CommentModel.ArticleCommentModel()
			{
				ID = s.ID,
				Content = s.Content,
				CreateAt = s.CreateAt,
				DisLikeCount = dbContext.CommentVoting.Count(c => c.CommentID == s.ID && c.VoteType == false),
				LikeCount = dbContext.CommentVoting.Count(c => c.CommentID == s.ID && c.VoteType == true),
				UserInfo = new Data.Model.UserModel.UserInfoModel()
				{
					ID = s.User.ID,
					FirstName = s.User.FirstName,
					LastName = s.User.LastName,
					Slug = s.User.Slug
				}
			}).ToList();


			return View(model);
		}

		[Authorization(Permission = "")]
		public ActionResult Insert()
		{
			return View();
		}

		[HttpPost]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		[Authorization(Permission = "")]
		public JsonResult Insert(ArticleInsertModel data)
		{
			if (ModelState.IsValid)
			{
				string titleSlug = StringHelper.Slug(data.Title);
				int userID = int.Parse(User.Identity.Name);

				SecurityHelper sch = new SecurityHelper();

				data.Content = data.Content.Replace(" style=\"text-align: start;\"", String.Empty);
				data.Content = data.Content.Replace("<br>", String.Empty);

				try
				{
					data.Content = sch.XSSFilter(data.Content);
				}
				catch (Exception ex)
				{
					return Json(new { status = true, message = "Gönderilen makale içeriği hatalı. " + ex.Message }, JsonRequestBehavior.AllowGet);
				}

				if (!dbContext.Article.Any(a => (a.Title.Equals(data.Title) || a.Slug.Equals(titleSlug)) && a.UserID == userID))
				{
					foreach (var item in data.CategoryID.Distinct().ToArray())
					{
						if (!dbContext.Category.Any(c => c.ID == item))
						{
							return Json(new { status = false, message = "Hatalı kategori seçimi." }, JsonRequestBehavior.AllowGet);
						}
					}

					var article = dbContext.Article.Create();
					article.IsPinned = 0;
					article.IsComment = 1;
					article.ViewState = 1;
					article.UserID = userID;
					article.Content = data.Content.Replace("<#text>", String.Empty);
					article.Description = StringHelper.StripTags(data.Description);
					article.CreateAt = DateTime.Now;
					article.Hit = 0;
					article.Title = data.Title;
					article.Slug = titleSlug;
					article.Status = (byte)Data.ArticleStatus.Active;

					//Makale için medya seç.
					if (data.ImageID.HasValue)
					{
						if (dbContext.Media.Any(m => m.ID == data.ImageID.Value))
						{
							article.MediaID = data.ImageID.Value;
						}
					}

					if (!article.MediaID.HasValue)
					{
						article.MediaID = dbContext.Media.Where(m => m.User.Slug.Equals("sistem")).OrderBy(r => Guid.NewGuid()).First().ID;
					}

					//Makale kategorileri ekle.
					foreach (var item in data.CategoryID.Distinct().ToArray())
					{
						article.Categories.Add(dbContext.Category.Single(c => c.ID == item));
					}

					//Makale etiketleri ekle.
					foreach (var item in data.Tickets.Split(';'))
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

					try
					{
						dbContext.Article.Add(article);
						dbContext.SaveChanges();

						return Json(new { status = true, message = "Makaleniz sisteme eklendi. Katkınız için teşekkürler..", slug = titleSlug }, JsonRequestBehavior.AllowGet);
					}
					catch (Exception ex)
					{
						return Json(new { status = false, message = "Kayıt işlemi sırasında bir hata oluştu." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { status = false, message = "Sistemde aynı isimde makale mevcut. Yeni bir isim belirleyiniz." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = string.Join("<br/>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) }, JsonRequestBehavior.AllowGet);
			}
		}

		[Authorization(Permission = "")]
		public ActionResult Edit(int id)
		{
			if (dbContext.Article.Any(a => a.ID == id))
			{
				var article = dbContext.Article.Single(a => a.ID == id);

				ArticleEditModel model = new ArticleEditModel();
				model.ID = article.ID;
				model.Content = article.Content;
				model.Description = article.Description;
				model.Slug = article.Slug;
				model.Title = article.Title;
				model.MediaID = article.MediaID;

				model.Tickets = String.Join(";", article.Tickets.Select(t => t.Title).ToArray());

				return View(model);
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public JsonResult Edit(ArticleEditModel data)
		{
			if (ModelState.IsValid)
			{
				int userID = int.Parse(User.Identity.Name);

				if(dbContext.Article.Any(a => a.ID == data.ID && a.Slug.Equals(data.Slug) && a.UserID == userID))
				{
					var article = dbContext.Article.Single(a => a.ID == data.ID);
					article.Title = data.Title;
					article.Description = data.Description;
					article.Slug = StringHelper.Slug(data.Title);


					string[] ticketTemp = data.Tickets.Split(';');

					foreach (var item in article.Tickets.ToList())
					{
						if(!ticketTemp.Any(t => t.Equals(item.Title)))
						{
							article.Tickets.Remove(item);
						}
					}

					foreach (string item in ticketTemp)
					{
						if(!article.Tickets.Any(t => t.Title.Equals(item)))
						{
							article.Tickets.Add(new Data.Entities.Ticket()
							{
								Title = item,
								Slug = StringHelper.Slug(item)
							});
						}
					}

					dbContext.SaveChanges();

					return Json(new { status = false, message = "Değişiklikler kaydedildi." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "İşlem yapmak istenen makale bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}

			return Json(new { status = false, message = string.Join("<br/>", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[Authorization(Permission = "")]
		public JsonResult Delete(int id)
		{
			if (dbContext.Article.Any(a => a.ID == id))
			{
				int userID = int.Parse(User.Identity.Name);
				if (dbContext.Article.Any(a => a.ID == id && a.UserID == userID))
				{
					var article = dbContext.Article.Single(a => a.ID == id);
					article.Status = (byte)Data.ArticleStatus.Deleted;

					dbContext.SaveChanges();

					return Json(new { status = true, message = "Makale silme işlemi başarıyla tamamlandı." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "Sadece kendi eklediğiniz makale üzerinde işlem yapabilirsiniz." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "İşlem yapmak istenen makale sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		[Authorization(Permission = "")]
		public JsonResult Block(int id)
		{
			if (dbContext.Article.Any(a => a.ID == id))
			{
				int userID = int.Parse(User.Identity.Name);
				if (dbContext.Article.Any(a => a.ID == id && a.UserID == userID))
				{
					var article = dbContext.Article.Single(a => a.ID == id);
					article.Status = (byte)((article.Status == 4) ? Data.ArticleStatus.Active : Data.ArticleStatus.Blockked);

					dbContext.SaveChanges();

					string message = (article.Status == (byte)Data.ArticleStatus.Active) ? "Makale yayına alındı." : "Makale yayından kaldırıldı.";

					return Json(new { status = true, message = message }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "Sadece kendi eklediğiniz makale üzerinde işlem yapabilirsiniz." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "İşlem yapmak istenen makale sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult ArticleAbsent()
		{
			return View();
		}

		public ActionResult CategoryAbsent()
		{
			return View();
		}

		[HttpPost]
		public JsonResult Like(ArticleVoteModel data)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return Json(new { status = false, message = "Oy vermek için kayıtlı hesabınız ile oturum açmalısınız." }, JsonRequestBehavior.AllowGet);
			}

			if (ModelState.IsValid)
			{
				int userID = int.Parse(User.Identity.Name);

				if (dbContext.Article.Any(a => a.ID == data.ArticleID))
				{
					if (!dbContext.Article.Any(a => a.ID == data.ArticleID && a.UserID == userID))
					{
						if (dbContext.ArticleVoting.Any(a => a.ArticleID == data.ArticleID && a.UserID == userID))
						{
							var voting = dbContext.ArticleVoting.Single(a => a.ArticleID == data.ArticleID && a.UserID == userID);
							voting.VoteType = !voting.VoteType;
							data.VoteType = voting.VoteType;

							dbContext.SaveChanges();
						}
						else
						{
							var voting = dbContext.ArticleVoting.Create();
							voting.ArticleID = data.ArticleID;
							voting.UserID = userID;
							voting.VoteType = data.VoteType;

							dbContext.ArticleVoting.Add(voting);
							dbContext.SaveChanges();
						}

						int likeCount = dbContext.ArticleVoting.Count(a => a.ArticleID == data.ArticleID && a.VoteType == true);
						int disLikeCount = dbContext.ArticleVoting.Count(a => a.ArticleID == data.ArticleID && a.VoteType == false);

						return Json(new { status = true, likeCount = likeCount, disLikeCount = disLikeCount }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json(new { status = false, message = "Kendi makalenize oy veremezsiniz." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { status = false, message = "Oy vermek istediğiniz makale bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Gerekli alanlar doldurulmamış." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult LazyLoading(int? page)
		{
			if (!page.HasValue)
				page = 2;

			ArticleCategoryListModel model = new ArticleCategoryListModel();
			model.Articles = dbContext.Article.Where(a => a.Status == 1).OrderByDescending(o => o.CreateAt).ToPagedList(page.Value, 14).Select(s => new ArticleBigListModel()
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

			return Json(new { status = true, data = model, message = "listelendi" }, JsonRequestBehavior.AllowGet);
		}
	}
}