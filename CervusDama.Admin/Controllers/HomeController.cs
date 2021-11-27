using CervusDama.Authorization;
using CervusDama.Data.Model.DashBoardModel;
using CervusDama.Data.Model.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace CervusDama.Admin.Controllers
{
	public class HomeController : Base.BaseController
	{
		// GET: Home
		public ActionResult Index()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Dashboard", "Home");
			}

			return View();
		}

		[HttpPost]
		public ActionResult Index(LoginModel data)
		{
			if (ModelState.IsValid)
			{
				if (dbContext.User.Any(u => u.Email == data.Email))
				{
					var user = dbContext.User.Single(u => u.Email == data.Email);

					if (Crypto.VerifyHashedPassword(user.Password, data.Password))
					{
						FormsAuthentication.SetAuthCookie(user.ID.ToString(), false);

						user.LastLogin = DateTime.Now;
						dbContext.SaveChanges();

						return RedirectToAction("Dashboard", "Home");
					}
					else
					{
						ModelState.AddModelError("error", "Girilen şifre hatalı.");
					}
				}
				else
				{
					ModelState.AddModelError("error", "Girilen email adresi hatalı.");
				}
			}
			else
			{
				ModelState.AddModelError("error", "Gerekli alanları doldurmalısınız.");
			}

			return View(data);
		}

		public ActionResult LogOut()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index");
		}

		[Authorization(Permission = "Administrator,Moderator")]
		public ActionResult Dashboard()
		{


			return View();
		}

		public ActionResult DashboardNumericData()
		{
			DateTime startDateTime = DateTime.Today;
			DateTime week = DateTime.Today.AddDays((-1 * (int)DateTime.Now.DayOfWeek)).AddTicks(-1);
			DateTime month = DateTime.Today.AddDays((-1 * (int)DateTime.Now.Day)).AddTicks(-1);

			DashboardNumericDataModel model = new DashboardNumericDataModel();
			model.Articles = new NumericDataModel()
			{
				ToDay = dbContext.Article.Count(a => a.CreateAt > startDateTime),
				ToWeek = dbContext.Article.Count(a => a.CreateAt > week),
				ToMonth = dbContext.Article.Count(a => a.CreateAt > month),
				Total = dbContext.Article.Count()
			};

			model.Users = new NumericDataModel()
			{
				ToDay = dbContext.User.Count(u => u.CreateAt > startDateTime),
				ToWeek = dbContext.User.Count(u => u.CreateAt > week),
				ToMonth = dbContext.User.Count(u => u.CreateAt > month),
				Total = dbContext.User.Count()
			};

			model.Questions = new NumericDataModel()
			{
				ToDay = dbContext.Question.Count(q => q.CreateAt > startDateTime),
				ToWeek = dbContext.Question.Count(q => q.CreateAt > week),
				ToMonth = dbContext.Question.Count(q => q.CreateAt > month),
				Total = dbContext.Question.Count()
			};

			model.CodeLibraries = new NumericDataModel()
			{
				ToDay = dbContext.CodeLibrary.Count(c => c.InsertAt > startDateTime),
				ToWeek = dbContext.CodeLibrary.Count(c => c.InsertAt > week),
				ToMonth = dbContext.CodeLibrary.Count(c => c.InsertAt > month),
				Total = dbContext.CodeLibrary.Count()
			};

			model.Categories = new NumericDataModel()
			{
				Total = dbContext.Category.Count()
			};

			model.Tickets = new NumericDataModel()
			{
				Total = dbContext.Ticket.Count()
			};

			model.Media = new NumericDataModel()
			{
				Total = dbContext.Media.Count()
			};

			model.ArticleSeries = new NumericDataModel()
			{
				Total = dbContext.ArticleSeries.Count()
			};

			return View(model);
		}

		[HttpPost]
		public JsonResult YearChartData(int? year)
		{
			if (!year.HasValue)
				year = DateTime.Now.Year;

			YearChartBigDataModel model = new YearChartBigDataModel();

			model.ArticleData = dbContext.Article.Where(a => a.CreateAt.Year == year.Value).GroupBy(g => g.CreateAt.Month).Select(
				s => new YearChartDataModel(){
					Year = year.Value,
					Month = s.FirstOrDefault().CreateAt.Month,
					Count = s.Count()
				}).ToList();

			model.UserData = dbContext.User.Where(u => u.CreateAt.Year == year.Value).GroupBy(g => g.CreateAt.Month).Select(
				s => new YearChartDataModel()
				{
					Year = year.Value,
					Month = s.FirstOrDefault().CreateAt.Month,
					Count = s.Count()
				}).ToList();

			model.QuestionData = dbContext.Question.Where(q => q.CreateAt.Year == year.Value).GroupBy(g => g.CreateAt.Month).Select(
				s => new YearChartDataModel()
				{
					Year = year.Value,
					Month = s.FirstOrDefault().CreateAt.Month,
					Count = s.Count()
				}).ToList();

			return Json(new { status = true, data = model }, JsonRequestBehavior.AllowGet);
		}
	}
}