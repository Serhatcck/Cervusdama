using CervusDama.Authorization;
using CervusDama.Data.Model.FeedBackModel;
using CervusDama.Utility.PagerHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Admin.Controllers
{
	[Authorization(Permission = "Administrator")]
	public class FeedBackController : Base.BaseController
	{
		// GET: FeedBack
		public ActionResult Index(int? page)
		{
			if (!page.HasValue)
				page = 1;

			int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);

			var feedBacks = dbContext.FeedBack.Select(s => new FeedBackListModel
			{
				ID = s.ID,
				Content = s.Content,
				CreateAt = s.CreateAt,
				FeedBackType = s.FeedBackType,
				FeedBackTypeText = s.FeedBackType == 1 ? "Bug bildirimi.." : s.FeedBackType == 2 ? "Öneride bulunma.." : s.FeedBackType == 3 ? "Şikayet bildirimi.." : s.FeedBackType == 4 ? "Soru sorma.." : s.FeedBackType == 5 ? "Yeni kategori isteği.." : s.FeedBackType == 6 ? "Diğer konularda.." : "Bilinmiyor..",
				Status = s.Status
			}).OrderByDescending(o => o.ID).ToPagedList(page.Value, getDataSize).ToList();

			ViewBag.currentPage = page.Value;
			ViewBag.totalData = dbContext.FeedBack.Count();

			return View(feedBacks);
		}

		[HttpPost]
		public JsonResult FeedbackDetail(int id)
		{
			if(dbContext.FeedBack.Any(f => f.ID == id))
			{
				var feedback = dbContext.FeedBack.Single(f => f.ID == id);

				FeedBackDetailModel model = new FeedBackDetailModel();
				model.ID = id;
				model.Content = feedback.Content;
				model.CreateAt = feedback.CreateAt.ToString("dd MMMM yyyy HH:mm");
				model.Status = feedback.Status;
				model.Title = getFeedBackType(feedback.FeedBackType);
				model.SenderType = feedback.UserID.HasValue;

				if (model.SenderType)
				{
					model.FirstName = feedback.User.FirstName;
					model.LastName = feedback.User.LastName;
				}

				return Json(new { status = true, data = model }, JsonRequestBehavior.AllowGet);
			}

			return Json(new { status = false, message = "Geri bildirim sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Delete(int id)
		{
			if(dbContext.FeedBack.Any(f=>f.ID == id))
			{
				var feedBack = dbContext.FeedBack.Single(f=>f.ID == id);
				dbContext.FeedBack.Remove(feedBack);
				dbContext.SaveChanges();

				return Json(new { status = true, message = "Bildirim sistemden silindi." }, JsonRequestBehavior.AllowGet);
			}

			return Json(new { status = false, message = "Bİldirim sistemden silinirken bir hata oluştu." }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult DashboardFeedBacks()
		{
			var feedBacks = dbContext.FeedBack.OrderByDescending(o => o.ID).Take(10).Select(s => new FeedBackListModel
			{
				ID = s.ID,
				Content = s.Content,
				CreateAt = s.CreateAt,
				FeedBackType = s.FeedBackType,
				FeedBackTypeText = s.FeedBackType == 1 ? "Bug bildirimi.." : s.FeedBackType == 2 ? "Öneride bulunma.." : s.FeedBackType == 3 ? "Şikayet bildirimi.." : s.FeedBackType == 4 ? "Soru sorma.." : s.FeedBackType == 5 ? "Yeni kategori isteği.." : s.FeedBackType == 6 ? "Diğer konularda.." : "Bilinmiyor..",
				Status = s.Status
			}).ToList();

			return View(feedBacks);
		}

		private string getFeedBackType(int type)
		{
			switch (type)
			{
				case 1:
					return "Bug bildirimi";
				case 2:
					return "Öneri.";
				case 3:
					return "Şikayet";
				case 4:
					return "Soru";
				case 5:
					return "Kategori isteği";
				case 6:
					return "Diğer";
				default:
					return "Diğer";
			}
		}
	}
}