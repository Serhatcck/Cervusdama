using CervusDama.Authorization;
using CervusDama.Data.Model.CommentModel;
using CervusDama.Utility.StringHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
	public class CommentController : Base.BaseController
	{
		// GET: Comment
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult InsertArticleComment(string slug, int articleID)
		{
			CommentInsertModel model = new CommentInsertModel();
			model.ArticleID = articleID;
			model.Slug = slug;
			return View(model);
		}

		[HttpPost]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		[Authorization(Permission = "")]
		public JsonResult InsertComment(CommentInsertModel data)
		{
			if (ModelState.IsValid)
			{
				if (dbContext.Article.Any(a => a.ID == data.ArticleID && a.Slug.Equals(data.Slug)))
				{
					int userID = int.Parse(User.Identity.Name);

					if (dbContext.Comment.Any(c => c.ArticleID == data.ArticleID && c.UserID == userID))
					{
						DateTime lastInsertCommentTime = dbContext.Comment.FirstOrDefault(c => c.ArticleID == data.ArticleID && c.UserID == userID).CreateAt;
						if (DateTime.Now < lastInsertCommentTime.AddMinutes(5))
						{
							return Json(new { status = false, message = "5 dk içerisinde sadece 1 yorum ekleyebilirsiniz." }, JsonRequestBehavior.AllowGet);
						}
					}

					var comment = dbContext.Comment.Create();
					comment.UserID = userID;
					comment.ArticleID = data.ArticleID;
					comment.CreateAt = DateTime.Now;
					comment.Content = data.Content;
					comment.Status = 1;

					dbContext.Comment.Add(comment);
					dbContext.SaveChanges();

					return Json(new { status = true, message = "Yorumunuz kaydedildi. Katkınız için teşekkürler." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "Yorum eklenecek makale sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Gerekli alanları doldurmalısınız." }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult EditArticleComment(string slug, int articleID)
		{
			CommentEditModel model = new CommentEditModel();
			model.ArticleID = articleID;
			model.ArticleSlug = slug;

			return View(model);
		}
		
		[HttpPost]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		[Authorization(Permission = "")]
		public JsonResult EditArticleComment(CommentEditModel data)
		{
			if (ModelState.IsValid)
			{
				if(dbContext.Article.Any(a => a.ID == data.ArticleID && a.Slug.Equals(data.ArticleSlug)))
				{
					if(dbContext.Comment.Any(c => c.ArticleID == data.ArticleID && c.ID == data.CommentID))
					{
						int userID = int.Parse(User.Identity.Name);

						if(dbContext.Comment.Any(c => c.ID == data.CommentID && c.UserID == userID))
						{
							var comment = dbContext.Comment.Single(c => c.ID == data.CommentID);
							comment.Content = StringHelper.StripTags(data.CommentContent);

							dbContext.SaveChanges();

							return Json(new { status = true, message = "Yorum düzenleme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
						}
						else
						{
							return Json(new { status = false, message = "Sadece kendi eklediğiniz yorumu düzenleyebilirsiniz." }, JsonRequestBehavior.AllowGet);
						}
					}
					else
					{
						return Json(new { status = false, message = "Düzenlemek istediğiniz yorum bu makaleye ait değil." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { status = false, message = "Yorumun ait olduğu makale bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}
			
			return Json(new { status  = false, message = "Boş alanlar var kontrol ediniz." }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[Authorization(Permission = "")]
		public JsonResult Delete(int id)
		{
			if(dbContext.Comment.Any(c => c.ID == id))
			{
				int userID = int.Parse(User.Identity.Name);

				if(dbContext.Comment.Any(c => c.ID == id && c.UserID == userID))
				{
					var comment = dbContext.Comment.Single(c => c.ID == id);

					foreach (var item in dbContext.CommentVoting.Where(v => v.CommentID == id).ToList())
					{
						dbContext.CommentVoting.Remove(item);
					}

					dbContext.Comment.Remove(comment);
					dbContext.SaveChanges();

					return Json(new { status = true, message = "Yorum silme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "Sadece kendi eklediğiniz yorumu silebilirsiniz." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Silmek istenen yorum sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}

			return Json(new { status = false, message = "" }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Like(CommentVoteModel data)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return Json(new { status = false, message = "Oy vermek için kayıtlı hesabınız ile oturum açmalısınız." }, JsonRequestBehavior.AllowGet);
			}

			if (ModelState.IsValid)
			{
				int userID = int.Parse(User.Identity.Name);

				if (dbContext.Comment.Any(a => a.ID == data.CommentID))
				{
					if (!dbContext.Comment.Any(a => a.ID == data.CommentID && a.UserID == userID))
					{
						if (dbContext.CommentVoting.Any(a => a.CommentID == data.CommentID && a.UserID == userID))
						{
							var voting = dbContext.CommentVoting.Single(a => a.CommentID == data.CommentID && a.UserID == userID);
							voting.VoteType = !voting.VoteType;
							data.VoteType = voting.VoteType;

							dbContext.SaveChanges();
						}
						else
						{
							var voting = dbContext.CommentVoting.Create();
							voting.CommentID = data.CommentID;
							voting.UserID = userID;
							voting.VoteType = data.VoteType;

							dbContext.CommentVoting.Add(voting);
							dbContext.SaveChanges();
						}

						int likeCount = dbContext.CommentVoting.Count(a => a.CommentID == data.CommentID && a.VoteType == true);
						int disLikeCount = dbContext.CommentVoting.Count(a => a.CommentID == data.CommentID && a.VoteType == false);

						return Json(new { status = true, likeCount = likeCount, disLikeCount = disLikeCount }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json(new { status = false, message = "Kendi yorumunuza oy veremezsiniz." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { status = false, message = "Oy vermek istediğiniz yorum bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Gerekli alanlar doldurulmamış." }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}