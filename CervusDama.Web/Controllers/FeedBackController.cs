using CervusDama.Data.Model.FeedBackModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
	public class FeedBackController : Base.BaseController
	{
		// GET: FeedBack
		public ActionResult Index()
		{
			FeedBackInsertModel model = new FeedBackInsertModel();

			model.FeedBackTypeList = new SelectList(
			new List<SelectListItem>
			{
				new SelectListItem { Text = "Geri bildirim türü seçiniz.", Value = null,  Selected = true },
				new SelectListItem { Text = "Bug bildirmek için.", Value = "1" },
				new SelectListItem { Text = "Öneride bulunmak istiyorum.", Value = "2" },
				new SelectListItem { Text = "Şikayetim var.", Value = "3" },
				new SelectListItem { Text = "Aklıma takılan bir soru var.", Value = "4" },
				new SelectListItem { Text = "Yeni bir makale kategorisi açılmalı.", Value = "5" },
				new SelectListItem { Text = "Diğer bir konuda.", Value = "6" },
				new SelectListItem { Text = "Kaldırılmasını istediğim içerik var.", Value = "7" },
				new SelectListItem { Text = "Telif hakkı ihlali bildirimi.", Value = "8" },
			}, "Value", "Text", 1);

			return View(model);
		}

		[HttpPost]
		[ValidateInput(true)]
		[ValidateAntiForgeryToken]
		public ActionResult Index(FeedBackInsertModel data)
		{
			if (ModelState.IsValid)
			{
				string userIP = Request.UserHostAddress;
				int? userID = null;
				bool isInserted = false;

				if (User.Identity.IsAuthenticated)
					userID = int.Parse(User.Identity.Name);

				DateTime olderTime = DateTime.Now.AddMinutes(-10);

				var lastFeedBack = dbContext.FeedBack.Where(f => f.SenderIP == userIP).OrderByDescending(o => o.CreateAt).FirstOrDefault();

				if(lastFeedBack != null)
				{
					if(lastFeedBack.CreateAt > olderTime)
					{
						isInserted = true;
					}
				}

				if (!isInserted)
				{
					var feedBack = dbContext.FeedBack.Create();
					feedBack.FeedBackType = data.FeedBackType;
					feedBack.Content = data.Content;
					feedBack.CreateAt = DateTime.Now;
					feedBack.SenderIP = userIP;
					feedBack.Status = 2;
					feedBack.UserID = userID;

					dbContext.FeedBack.Add(feedBack);

					dbContext.SaveChanges();

					ViewBag.Message = "Mesajınız alınmıştır, ilginiz için teşekkürler.";
				}
				else
				{
					ViewBag.Message = "Çok sık geri bildirim gönderemezsiniz.";
				}
			}

			data.FeedBackTypeList = new SelectList(
			new List<SelectListItem>
			{
				new SelectListItem { Text = "Geri bildirim türü seçiniz.", Value = null,  Selected = true },
				new SelectListItem { Text = "Bug bildirmek için.", Value = "1" },
				new SelectListItem { Text = "Öneride bulunmak istiyorum.", Value = "2" },
				new SelectListItem { Text = "Şikayetim var.", Value = "3" },
				new SelectListItem { Text = "Aklıma takılan bir soru var.", Value = "4" },
				new SelectListItem { Text = "Yeni bir makale kategorisi açılmalı.", Value = "5" },
				new SelectListItem { Text = "Diğer bir konuda.", Value = "6" }
			}, "Value", "Text", 1);

			return View(data);
		}
	}
}