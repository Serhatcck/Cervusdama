using CervusDama.Authorization;
using CervusDama.Data.Model.AnswerModel;
using CervusDama.Data.Model.QuestionModel;
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
	public class QuestionController : Base.BaseController
	{
		// GET: Question
		public ActionResult Index(int? page)
		{
			if (!page.HasValue)
				page = 1;

			int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);

			var questions = dbContext.Question.OrderByDescending(o => o.ID).ToPagedList(page.Value, getDataSize).Select(s => new QuestionListModel
			{
				ID = s.ID,
				Title = s.Title,
				CreateAt = s.CreateAt,
				FirstName = s.User.FirstName,
				LastName = s.User.LastName,
				AnswerCount = dbContext.Answer.Count(a => a.QuestionID == s.ID),
				Status = s.Status,
				Slug = s.Slug
			}).ToList();

			ViewBag.currentPage = page.Value;
			ViewBag.totalData = dbContext.Question.Count();

			return View(questions);
		}

		public ActionResult Edit(int id)
		{
			if (dbContext.Question.Any(q => q.ID == id))
			{
				var question = dbContext.Question.Single(q => q.ID == id);
				var data = new QuestionInsertModel()
				{
					ID = question.ID,
					Content = question.Content,
					Title = question.Title,
					Tickets = String.Join(";", question.Tickets.Select(s => s.Title).ToList())
				};
				return View(data);
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

		[HttpPost]
		[ValidateInput(false)]
		public JsonResult Edit(QuestionInsertModel data)
		{
			if (ModelState.IsValid)
			{
				if (dbContext.Question.Any(q => q.ID == data.ID))
				{
					string slug = StringHelper.Slug(data.Title);
					var question = dbContext.Question.Single(q => q.ID == data.ID);

					if (dbContext.Question.Any(q => q.Slug.Equals(slug) && q.ID != data.ID && q.UserID != question.UserID))
					{
						return Json(new { status = false, message = "Aynı isimde makale var, farklı bir isim ile tekrar deneyiniz." }, JsonRequestBehavior.AllowGet);
					}

					question.Title = data.Title;
					question.Content = data.Content;
					question.Slug = StringHelper.Slug(data.Title);

					string[] tickets = data.Tickets.Split(';');

					foreach (var item in question.Tickets.ToList())
					{
						if (!tickets.Contains(item.Title))
						{
							question.Tickets.Remove(item);
						}
					}

					foreach (var item in tickets)
					{
						if (!question.Tickets.Any(t => t.Title.Equals(item.Trim())))
						{
							if (dbContext.Ticket.Any(t => t.Title.Equals(item.Trim())))
							{
								question.Tickets.Add(dbContext.Ticket.FirstOrDefault(t => t.Title.Equals(item.Trim())));
							}
							else
							{
								question.Tickets.Add(new Data.Entities.Ticket
								{
									Title = item.Trim(),
									Slug = StringHelper.Slug(item.Trim())
								});
							}
						}
					}

					dbContext.SaveChanges();

					return Json(new { status = true, message = "Soru güncelleme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "İşlem yapmak istene soru sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Boş gönderilen alanlar var. Tüm alanlar doldurulmalı." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult Delete(int id)
		{
			if (dbContext.Question.Any(a => a.ID == id))
			{
				var question = dbContext.Question.Single(a => a.ID == id);

				if (dbContext.Answer.Any(a => a.QuestionID == question.ID))
				{
					foreach (var item in dbContext.Answer.Where(a => a.QuestionID == question.ID).ToList())
					{
						dbContext.Answer.Remove(item);
					}
				}

				dbContext.Question.Remove(question);

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
			if (dbContext.Question.Any(a => a.ID == id))
			{
				var question = dbContext.Question.Single(a => a.ID == id);
				question.Status = (byte)((question.Status == 1) ? 0 : 1);
				dbContext.SaveChanges();

				if (question.Status == 1)
				{
					return Json(new { status = true, message = "Soru tekrar yayında." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = true, message = "Soru yayından kaldırıldı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "İşlem yapmak istenen soru sitemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult AnswerList(int questionID)
		{
			if (dbContext.Question.Any(q => q.ID == questionID))
			{
				var answer = dbContext.Answer.Where(a => a.QuestionID == questionID).OrderByDescending(o => o.CreateAt).Select(s => new QuestionAnswerModel()
				{
					ID = s.ID,
					Content = s.Content,
					CreateAt = s.CreateAt,
					UserInfo = new Data.Model.UserModel.UserInfoModel()
					{
						FirstName = s.User.FirstName,
						LastName = s.User.LastName
					}
				}).ToList();

				if(answer.Count() == 0)
				{
					return Json(new { status = false, message = "Gösterilecek cevap yok. Soru için henüz cevap eklenmemiş." }, JsonRequestBehavior.AllowGet);
				}

				return Json(new { status = true, data = answer, message = "Soru listesi alma başarılı." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "Cevapları alınacak soru sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}