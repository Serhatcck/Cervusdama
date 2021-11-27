using CervusDama.Authorization;
using CervusDama.Data.Model.AnswerModel;
using CervusDama.Utility.PagerHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Admin.Controllers
{
    [Authorization(Permission = "Administrator,Moderator")]
    public class AnswerController : Base.BaseController
    {
        // GET: Answer
        public ActionResult Index(int? page)
        {
			if (!page.HasValue)
				page = 1;

			int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);

			var answer = dbContext.Answer.OrderByDescending(o => o.ID).ToPagedList(page.Value, getDataSize).Select(s => new QuestionAnswerModel
			{
				ID = s.ID,
				QuestionTitle = s.Question.Title,
				CreateAt = s.CreateAt,
				Content = s.Content,
				Status = s.Status,
				LikeCount = dbContext.AnswerVoting.Count(a => a.AnswerID == s.ID && a.VoteType == true),
				DislikeCount = dbContext.AnswerVoting.Count(a => a.AnswerID == s.ID && a.VoteType == false),
				UserInfo = new Data.Model.UserModel.UserInfoModel()
				{
					FirstName = s.User.FirstName,
					LastName = s.User.LastName
				}
			}).ToList();

			ViewBag.currentPage = page.Value;
			ViewBag.totalData = dbContext.Article.Count();

			return View(answer);
		}

		public ActionResult Edit(int id)
		{
			if(dbContext.Answer.Any(a => a.ID == id))
			{
				var answer = dbContext.Answer.Single(a => a.ID == id);

				var model = new AnswerEditModel()
				{
					ID = answer.ID,
					Content = answer.Content
				};

				return View(model);
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		public JsonResult Edit(AnswerEditModel data)
		{
			if (ModelState.IsValid)
			{
				if(dbContext.Answer.Any(a => a.ID == data.ID))
				{
					var answer = dbContext.Answer.Single(a => a.ID == data.ID);
					answer.Content = data.Content;

					dbContext.SaveChanges();

					return Json(new { status = true, message = "Cevap düzenleme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "İşlem yapılmak istenen cevap sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Eksik alanlar var. Kontrol ederek tekrar gönderiniz." }, JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult Detail(int id)
		{
			if(dbContext.Answer.Any(a => a.ID == id))
			{
				var answer = dbContext.Answer.Single(a => a.ID == id);

				var model = new QuestionAnswerModel()
				{
					ID = answer.ID,
					Content = answer.Content,
					CreateAt = answer.CreateAt,
					QuestionTitle = answer.Question.Title,
					Status = answer.Status,
					LikeCount = dbContext.AnswerVoting.Count(a => a.AnswerID == answer.ID && a.VoteType == true),
					DislikeCount = dbContext.AnswerVoting.Count(a => a.AnswerID == answer.ID && a.VoteType == false),
					UserInfo = new Data.Model.UserModel.UserInfoModel()
					{
						FirstName = answer.User.FirstName,
						LastName = answer.User.LastName
					}
				};

				return Json(new { status = true, data = model }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "Cevap sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult Delete(int id)
		{
			if (dbContext.Answer.Any(a => a.ID == id))
			{
				var answer = dbContext.Answer.Single(a => a.ID == id);

				foreach (var item in dbContext.AnswerVoting.Where(a => a.AnswerID == id).ToList())
				{
					dbContext.AnswerVoting.Remove(item);
				}

				dbContext.Answer.Remove(answer);

				dbContext.SaveChanges();

				return Json(new { status = true, message = "Cevap sistemden silindi." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "İşlem yapmak istenen cevap bulunamadı." });
			}
		}

		[HttpPost]
		public JsonResult Block(int id)
		{
			if (dbContext.Answer.Any(a => a.ID == id))
			{
				var answer = dbContext.Answer.Single(a => a.ID == id);
				answer.Status = (byte)((answer.Status == 1) ? 0 : 1);
				dbContext.SaveChanges();

				if (answer.Status == 1)
				{
					return Json(new { status = true, message = "Cevap tekrar yayında." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = true, message = "Cevap yayından kaldırıldı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "İşlem yapmak istenen cevap sitemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}