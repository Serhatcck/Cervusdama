using CervusDama.Authorization;
using CervusDama.Data.Model.ArticleModel;
using CervusDama.Data.Model.ProfileModel;
using CervusDama.Data.Model.QuestionModel;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
	public class ProfileController : Base.BaseController
	{
		// GET: Profile
		public ActionResult Index(string slug)
		{
			Data.Entities.User user = null;

			if (String.IsNullOrEmpty(slug))
			{
				if (User.Identity.IsAuthenticated)
				{
					int userID = int.Parse(User.Identity.Name);
					user = dbContext.User.Single(u => u.ID == userID);
					slug = user.Slug;
				}
				else
				{
					return RedirectToRoute("ProfileAbsent");
				}
			}
			else
			{
				if (!dbContext.User.Any(u => u.Slug.Equals(slug)))
				{
					return RedirectToRoute("ProfileAbsent");
				}
			}

			user = dbContext.User.Single(u => u.Slug.Equals(slug));

			ProfileModel profileInfo = new ProfileModel()
			{
				ID = user.ID,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Bio = user.Bio,
				WebSite = user.WebSite,
				CreateAt = user.CreateAt,
				Slug = user.Slug,
				CityName = user.CityName,
				AnswerCount = dbContext.Answer.Count(a => a.UserID == user.ID),
				ArticleCount = dbContext.Article.Count(a => a.UserID == user.ID),
				CommentCount = dbContext.Comment.Count(c => c.UserID == user.ID),
				QuestionCount = dbContext.Question.Count(q => q.UserID == user.ID)
			};

			return View(profileInfo);
		}

		public ActionResult ProfileArticles(int id)
		{
			var articles = dbContext.Article.Where(a => a.UserID == id && a.Status > 0).OrderByDescending(o => o.CreateAt).Take(20).Select(s => new ArticleBigListModel()
			{
				ID = s.ID,
				CreateAt = s.CreateAt,
				Title = s.Title,
				Description = s.Description,
				Slug = s.Slug,
				Status = s.Status,
				UserInfo = new Data.Model.UserModel.UserInfoModel()
				{
					ID = s.UserID
				},
				BaseCategory = new Data.Model.CategoryModel.CategoryAllListModel()
				{
					Icon = s.Categories.FirstOrDefault().Icon,
					Color = s.Categories.FirstOrDefault().Color
				},
				Tickets = s.Tickets.Select(t => new Data.Model.TicketModel.TicketListModel()
				{
					ID = t.ID,
					Title = t.Title,
					Slug = t.Slug
				}).ToList()
			}).ToList();

			return View(articles);
		}

		public ActionResult ProfilePage()
		{
			return View();
		}

		public ActionResult ProfileAbsent()
		{
			return View();
		}

		[HttpPost]
		[Authorization(Permission = "")]
		public JsonResult ChangeProfileImage(HttpPostedFileBase profileImage)
		{
			if (profileImage == null)
			{
				return Json(new { status = false, message = "Bir resim seçmelisiniz." }, JsonRequestBehavior.AllowGet);
			}

			float fileSize = profileImage.ContentLength / 1024;

			if (fileSize > 150) //en fazla 150 KB dosya yüklenebilir.
			{
				return Json(new { status = false, message = "Seçilen dosya çok büyük, en fazla 150 KB olabilir." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				string[] mimeTypes = { "image/png", "image/jpg", "image/jpeg" };

				if (!mimeTypes.Any(a => a.Equals(profileImage.ContentType)))
				{
					return Json(new { status = false, message = "Sadece PNG ve JPG formatında resim seçebilirsiniz." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					int userID = int.Parse(User.Identity.Name);
					var user = dbContext.User.Single(u => u.ID == userID);

					profileImage.SaveAs(@"D:\Projects\TFS-Projects\CervusDama\CervusDama.Web\uploads\profile\" + user.Slug + ".png");

					return Json(new { status = true, message = "Profil resmi değiştirildi." }, JsonRequestBehavior.AllowGet);
				}
			}
		}

		[HttpPost]
		public JsonResult ProfileArticleSearch(string searchQuery, int userID)
		{
			if (String.IsNullOrEmpty(searchQuery))
			{
				return Json(new { status = false, message = "Arama için bir şeyler yazın.." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				Expression<Func<Data.Entities.Article, bool>> expr = PredicateBuilder.New<Data.Entities.Article>(true);

				expr = expr.And(a => a.UserID == userID && a.Title.Contains(searchQuery));
				int auhenticatedUserID = 0;
				if (User.Identity.IsAuthenticated)
				{
					auhenticatedUserID = int.Parse(User.Identity.Name);

					if(userID == auhenticatedUserID)
					{
						expr = expr.And(a => a.Status != 0);
					}
					else
					{
						expr = expr.And(a => a.Status == 1);
					}
				}
				else
				{
					expr = expr.And(a => a.Status == 1);
				}

				var articles = dbContext.Article.Where(expr).OrderByDescending(o => o.CreateAt).Take(20).Select(s => new ArticleBigListModel()
				{
					ID = s.ID,
					CreateAt = s.CreateAt,
					Title = s.Title,
					Slug = s.Slug,
					Status = s.Status,
					AuthID = auhenticatedUserID,
					UserInfo = new Data.Model.UserModel.UserInfoModel()
					{
						ID = s.UserID
					},
					BaseCategory = new Data.Model.CategoryModel.CategoryAllListModel()
					{
						Icon = s.Categories.FirstOrDefault().Parent.Icon,
						Color = s.Categories.FirstOrDefault().Parent.Color
					},
					Tickets = s.Tickets.Select(t => new Data.Model.TicketModel.TicketListModel()
					{
						ID = t.ID,
						Title = t.Title,
						Slug = t.Slug
					}).ToList()
				}).ToList();

				articles.ForEach(e => {
					e.CreateAtString = e.CreateAt.ToString("dd MMMM yyyy HH:mm");
				});

				return Json(new { status = true, data = articles, message = "Arama sonuçları." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		[Authorization(Permission = "")]
		public JsonResult DeleteAccount(ProfileDeleteModel data)
		{
			if (ModelState.IsValid)
			{
				if (data.IsAccept)
				{
					//TO:DO = kullanıcının makalelerini, tüm datalarını sil.
					return Json(new { status = true, message = "Hesabınız Cervus Dama sisteminden silinmiştir. 2 saniye içinde yönlendirileceksiniz." }, JsonRequestBehavior.AllowGet);
				}
			}

			return Json(new { status = false, message = "Hesap silme onay kutusunu doldurmalısınız." }, JsonRequestBehavior.AllowGet);
		}

		// Kişisel Bilgileri Güncelle ------------------
		[Authorization(Permission = "")]
		public ActionResult PersonalInfoUpdate()
		{
			int userID = int.Parse(User.Identity.Name);

			var user = dbContext.User.Single(u => u.ID == userID);

			ProfileInfoModel model = new ProfileInfoModel()
			{
				Bio = user.Bio,
				FirstName = user.FirstName,
				LastName = user.LastName,
				NickName = user.NickName
			};

			return View(model);
		}

		[HttpPost]
		[Authorization(Permission = "")]
		[ValidateAntiForgeryToken]
		public JsonResult PersonalInfoUpdate(ProfileInfoModel data)
		{
			if (ModelState.IsValid)
			{
				int userID = int.Parse(User.Identity.Name);

				if (dbContext.User.Any(u => u.NickName.Equals(data.NickName) && u.ID != userID))
				{
					return Json(new { status = false, message = "Seçilen kullanıcı adı kullanımda. Başka bir kullanıcı adı seçiniz." }, JsonRequestBehavior.AllowGet);
				}

				var user = dbContext.User.Single(u => u.ID == userID);
				user.FirstName = data.FirstName;
				user.LastName = data.LastName;
				user.NickName = data.NickName;
				user.Bio = data.Bio;

				try
				{
					dbContext.SaveChanges();
					return Json(new { status = true, message = "Kişisel bilgileri düzenleme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
				}
				catch
				{
					return Json(new { status = false, message = "Bilgiler düzenlenirken bir sistem hatası oluştu." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new
				{
					status = false,
					message = string.Join("<br/>", ModelState.Values
										.SelectMany(x => x.Errors)
										.Select(x => x.ErrorMessage))
				}, JsonRequestBehavior.AllowGet);
			}
		}

		// İletişim Bilgileri Güncelle ------------------
		[Authorization(Permission = "")]
		public ActionResult ConnectInfoUpdate()
		{
			int userID = int.Parse(User.Identity.Name);

			var user = dbContext.User.Single(u => u.ID == userID);

			ContactInfoModel model = new ContactInfoModel()
			{
				Email = user.Email,
				WebSite = user.WebSite,
				CityName = user.CityName
			};

			return View(model);
		}

		[HttpPost]
		[Authorization(Permission = "")]
		[ValidateAntiForgeryToken]
		public JsonResult ConnectInfoUpdate(ContactInfoModel data)
		{
			if (ModelState.IsValid)
			{
				int userID = int.Parse(User.Identity.Name);

				var user = dbContext.User.Single(u => u.ID == userID);
				user.Email = data.Email;
				user.WebSite = data.WebSite;
				user.CityName = data.CityName;

				try
				{
					dbContext.SaveChanges();
					return Json(new { status = true, message = "İletişim bilgileri düzenleme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
				}
				catch
				{
					return Json(new { status = false, message = "Bilgiler düzenlenirken bir sistem hatası oluştu." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new
				{
					status = false,
					message = string.Join("<br/>", ModelState.Values
										.SelectMany(x => x.Errors)
										.Select(x => x.ErrorMessage))
				}, JsonRequestBehavior.AllowGet);
			}
		}

		// Şifre Değiştir ------------------
		[Authorization(Permission = "")]
		public ActionResult PasswordUpdate()
		{
			return View();
		}

		[HttpPost]
		[Authorization(Permission = "")]
		[ValidateAntiForgeryToken]
		public JsonResult PasswordUpdate(PasswordChangeModel data)
		{
			if (ModelState.IsValid)
			{
				int userID = int.Parse(User.Identity.Name);

				var user = dbContext.User.Single(u => u.ID == userID);

				if (Crypto.VerifyHashedPassword(user.Password, data.OldPassword))
				{
					user.Password = Crypto.HashPassword(data.Password);

					try
					{
						dbContext.SaveChanges();
						return Json(new { status = true, message = "Şifre değiştirme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
					}
					catch
					{
						return Json(new { status = false, message = "Bilgiler düzenlenirken bir sistem hatası oluştu." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { status = false, message = "Eski şifrenizi hatalı girdiniz." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new
				{
					status = false,
					message = string.Join("<br/>", ModelState.Values
										.SelectMany(x => x.Errors)
										.Select(x => x.ErrorMessage))
				}, JsonRequestBehavior.AllowGet);
			}
		}

		//Profil soru listeleme
		public ActionResult QuestionList(int id)
		{
			var questions = dbContext.Question.Where(q => q.UserID == id).OrderByDescending(o => o.CreateAt).Take(20).Select(s => new QuestionListModel() {
				ID = s.ID,
				Title = s.Title,
				Slug = s.Slug,
				AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
				CreateAt = s.CreateAt,
				Status = s.Status,
				UserSlug = s.User.Slug,
				UserID = s.UserID,
				Tickets = s.Tickets.Select(t => new Data.Model.TicketModel.TicketListModel() { 
					ID = t.ID,
					Title = t.Title,
					Slug = t.Slug
				}).ToList()
			}).ToList();

			return View(questions);
		}
		
		public ActionResult AnsweredQuestionList(int id)
		{
			int[] answer = dbContext.Answer.Where(a => a.UserID == id && a.Question.UserID != id).GroupBy(g => g.QuestionID).Select(s => s.FirstOrDefault().QuestionID).ToArray();

			var questions = dbContext.Question.Where(q => answer.Contains(q.ID)).OrderByDescending(o => o.CreateAt).Take(20).Select(s => new QuestionListModel()
			{
				ID = s.ID,
				Title = s.Title,
				Slug = s.Slug,
				AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
				CreateAt = s.CreateAt,
				Status = s.Status,
				UserSlug = s.User.Slug,
				UserID = s.UserID,
				Tickets = s.Tickets.Select(t => new Data.Model.TicketModel.TicketListModel()
				{
					ID = t.ID,
					Title = t.Title,
					Slug = t.Slug
				}).ToList()
			}).ToList();

			return View(questions);
		}
	}
}