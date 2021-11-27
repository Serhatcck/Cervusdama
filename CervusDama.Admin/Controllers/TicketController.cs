using CervusDama.Authorization;
using CervusDama.Data.Model.TicketModel;
using CervusDama.Utility.PagerHelper;
using CervusDama.Utility.StringHelper;
using System.Linq;
using System.Web.Mvc;

namespace CervusDama.Admin.Controllers
{
	[Authorization(Permission = "Administrator,Moderator")]
	public class TicketController : Base.BaseController
    {
        // GET: Ticket
        public ActionResult Index(int? page)
        {
			if (!page.HasValue)
				page = 1;

			int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);

			var tickets = dbContext.Ticket.Select(s => new TicketListModel
			{
				ID = s.ID,
				Title = s.Title,
				Slug = s.Slug,
			}).OrderByDescending(o => o.ID).ToPagedList(page.Value, getDataSize).ToList();

			ViewBag.currentPage = page.Value;
			ViewBag.totalData = dbContext.Ticket.Count();

			return View(tickets);
        }

		[HttpPost]
		public JsonResult Insert(TicketInsertModel data)
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrEmpty(data.Slug))
					data.Slug = StringHelper.Slug(data.Title);

				if(!dbContext.Ticket.Any(t=> t.Title.Equals(data.Title) || t.Slug.Equals(data.Slug)))
				{
					var ticket = dbContext.Ticket.Create();
					ticket.Title = data.Title;
					ticket.Slug = data.Slug;

					dbContext.Ticket.Add(ticket);
					dbContext.SaveChanges();

					return Json(new { status = true, message = "Etiket oluşturma işlemi başarılı." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "Aynı isimde etiket mevcut, oluşturma işlemi gerçekleştirilemedi." }, JsonRequestBehavior.AllowGet);
				}

			}
			else
			{
				return Json(new { status = false, message = "Etiket ismi gereklidir. Boş olamaz!" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult Edit(TicketInsertModel data)
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrEmpty(data.Slug))
					data.Slug = StringHelper.Slug(data.Title);

				var ticket = dbContext.Ticket.FirstOrDefault(t => t.ID == data.ID);

				if(ticket != null)
				{
					if(!dbContext.Ticket.Any(t=>t.ID != data.ID && (t.Title.Equals(data.Title) || t.Slug.Equals(data.Slug))))
					{
						ticket.Title = data.Title;
						ticket.Slug = data.Slug;

						dbContext.SaveChanges();

						return Json(new { status = true, message = "Etiket düzenleme işlemi başarılı!" }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json(new { status = false, message = "Sistemde aynı isimde etiket mevcut. Düzenleme işlemi iptal edildi.!" }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { status = false, message = "Düzenlemek istenen etiket sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Etiket ismi gereklidir. Boş olamaz!" }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public JsonResult Delete(int id)
		{
			if(dbContext.Ticket.Any(t=>t.ID == id))
			{
				if(!dbContext.Article.Any(a=>a.Tickets.Any(t=>t.ID == id)))
				{
					var ticket = dbContext.Ticket.Single(t=> t.ID == id);
					dbContext.Ticket.Remove(ticket);
					dbContext.SaveChanges();

					return Json(new { status = true, message = "Etiket sistemden silindi." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "Etiket bir makalede tanımlı. Silme işlemi iptal edildi." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Silmek istenen etiket sistemde bulunamadı!" }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}