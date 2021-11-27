using CervusDama.Authorization;
using CervusDama.Data.Model.UserModel;
using CervusDama.Utility.PagerHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Admin.Controllers
{
	[Authorization(Permission = "Administrator")]
	public class UsersController : Base.BaseController
	{
		// GET: Users
		public ActionResult Index(int? page)
		{
			if (!page.HasValue)
				page = 1;

			int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);

			var users = dbContext.User.OrderByDescending(o => o.ID).ToPagedList(page.Value, getDataSize).Select(s => new UserListModel
			{
				ID = s.ID,
				FirstName = s.FirstName,
				LastName = s.LastName,
				NickName = s.NickName,
				Role = s.Role.Name,
				LastLogin = s.LastLogin,
				CreateAt = s.CreateAt,
				Stauts = s.Status,
				Slug = s.Slug
			}).ToList();

			ViewBag.currentPage = page.Value;
			ViewBag.totalData = dbContext.User.Count();

			return View(users);
		}

		[HttpPost]
		public JsonResult UserDetailInfo(int id)
		{
			if (dbContext.User.Any(u => u.ID == id))
			{
				var user = dbContext.User.Include("Role").Single(u => u.ID == id);

				UserDetailModel model = new UserDetailModel();
				model.ID = user.ID;
				model.FirstName = user.FirstName;
				model.LastName = user.LastName;
				model.NickName = user.NickName;
				model.Email = user.Email;
				model.RoleName = user.Role.Name;
				model.CreateAt = user.CreateAt.ToString("dd MMMM yyyy HH:mm");
				model.LastLogin = user.LastLogin.HasValue ? user.LastLogin.Value.ToString("dd MMMM yyyy HH:mm") : "-";
				model.Status = user.Status;
				model.Slug = user.Slug;
				model.ArticleCount = dbContext.Article.Count(a => a.UserID == id);
				model.ArticleLikeCount = dbContext.ArticleVoting.Count(c => c.UserID == id && c.VoteType == true);
				model.ArticleDisLikeCount = dbContext.ArticleVoting.Count(c => c.UserID == id && c.VoteType == false);
				model.CommentCount = dbContext.Comment.Count(c => c.UserID == id);
				model.QuestionCount = dbContext.Question.Count(c => c.UserID == id);
				model.CodeCount = dbContext.CodeLibrary.Count(c => c.UserID == id);

				return Json(new { status = true, data = model }, JsonRequestBehavior.AllowGet);
			}

			return Json(new { status = false, message = "Kullanıcı sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult UserBan(int id)
		{
			if(dbContext.User.Any(u => u.ID == id))
			{
				var user = dbContext.User.Single(u => u.ID == id);

				if(user.Status == 1)
				{
					user.Status = 0;
					dbContext.SaveChanges();
					return Json(new { status = true, message = "Kullanıcı hesabı pasif hale gitirildi." });
				}
				else
				{
					user.Status = 1;
					dbContext.SaveChanges();
					return Json(new { status = true, message = "Kullanıcı hesabı aktif hale gitirildi." });
				}
			}

			return Json(new { status = false, message = "İşlem sırasında bir hata oluştu." });
		}
	}
}