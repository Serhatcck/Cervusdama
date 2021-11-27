using CervusDama.Authorization;
using CervusDama.Data.Model.CategoryModel;
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
	public class CategoryController : Base.BaseController
	{
		// GET: Category
		public ActionResult Index(int? page)
		{
			CategoryInsertModel model = new CategoryInsertModel();
			model.Parents = new List<SelectListItem>();

			model.Parents = dbContext.Category.Where(c => !c.ParentID.HasValue).Select(s => new SelectListItem
			{
				Text = s.Title,
				Value = s.ID.ToString()
			}).ToList();

			model.Parents.Insert(0, new SelectListItem
			{
				Text = "Üst Kategori Yok",
				Value = "",
				Selected = true
			});

			if (!page.HasValue)
				page = 1;

			ViewBag.currentPage = page.Value;

			return View(model);
		}

		[HttpPost]
		public ActionResult Index(CategoryInsertModel data)
		{
			ViewBag.currentPage = 1;

			if (ModelState.IsValid)
			{
				if (String.IsNullOrEmpty(data.Slug))
				{
					data.Slug = StringHelper.Slug(data.Title);
				}

				if (dbContext.Category.Any(c => c.Title.Equals(data.Title) || c.Slug.Equals(data.Slug)))
				{
					//Aynı isimde slugda kategori var.
				}
				else
				{
					var category = dbContext.Category.Create();
					category.Description = String.IsNullOrEmpty(data.Description) ? "-" : data.Description;
					category.Title = data.Title;
					category.Status = 1;
					category.Slug = data.Slug;
					category.Icon = data.Icon;
					category.Color = data.Color;

					if (data.ParentID.HasValue)
						category.ParentID = data.ParentID;

					dbContext.Category.Add(category);
					dbContext.SaveChanges();
				}

				return RedirectToAction("Index");
			}
			else
			{
				data.Parents = new List<SelectListItem>();

				data.Parents = dbContext.Category.Where(c => !c.ParentID.HasValue).Select(s => new SelectListItem
				{
					Text = s.Title,
					Value = s.ID.ToString()
				}).ToList();

				data.Parents.Insert(0, new SelectListItem
				{
					Text = "Üst Kategori Yok",
					Value = "",
					Selected = true
				});

				return View(data);
			}
		}

		public ActionResult List(int? page)
		{
			if (!page.HasValue)
				page = 1;

			int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);

			var category = dbContext.Category.Select(s => new CategoryListModel
			{
				ID = s.ID,
				Title = s.Title,
				Description = s.Description,
				Slug = s.Slug,
				Status = s.Status,
				ParentID = (s.ParentID.HasValue ? s.ParentID.Value : 0)
			}).OrderByDescending(o => o.ID).ToPagedList(page.Value, getDataSize).ToList();

			ViewBag.currentPage = page.Value;
			ViewBag.totalData = dbContext.Category.Count();

			return View(category);
		}

		[HttpPost]
		public JsonResult Edit(CategoryInsertModel data)
		{
			if (ModelState.IsValid)
			{
				var category = dbContext.Category.FirstOrDefault(c => c.ID == data.ID);

				if (category != null)
				{
					if (String.IsNullOrEmpty(data.Slug))
						data.Slug = StringHelper.Slug(data.Title);

					if (dbContext.Category.Any(c => ((c.Title.Equals(data.Title) && c.ID != data.ID) || (c.Slug.Equals(data.Slug) && c.ID != data.ID))))
					{
						return Json(new { status = false, message = "Aynı isimde kategori mevcut, güncelleme işlemi gerçekleştirilemedi." }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						category.Title = data.Title;
						category.Slug = data.Slug;
						category.Description = data.Description;
						category.Icon = data.Icon;
						category.Color = data.Color;

						if (data.ParentID.HasValue)
							category.ParentID = data.ParentID.Value;

						dbContext.SaveChanges();

						return Json(new { status = true, message = "Güncelleme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { status = false, message = "Güncellemek istenen kategori sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Kategori ismi boş olamaz." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult CategoryEditData(int id)
		{
			if(id > 0)
			{
				if(dbContext.Category.Any(c => c.ID == id))
				{
					var data = dbContext.Category.Single(c => c.ID == id);

					return Json(new { Status = true, Data = data }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { Status = false, Message = "Düzenlemek istenen kategori sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { Status = false, Message = "Geçersiz ID değeri." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public JsonResult Delete(int id)
		{
			if(dbContext.Category.Any(c=>c.ID == id))
			{
				if(!dbContext.Article.Any(a=>a.Categories.Any(c=>c.ID == id)))
				{
					var category = dbContext.Category.Single(c=>c.ID == id);

					dbContext.Category.Remove(category);
					dbContext.SaveChanges();

					return Json(new { status = true, message = "Kategori silme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "Kategoriye ait makaleler var. Silme işlemi iptal edildi." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Silmek istenen kategori sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public JsonResult Block(int id)
		{
			if (dbContext.Category.Any(c => c.ID == id))
			{
				var category = dbContext.Category.Single(c => c.ID == id);
				
				string messageText = "";

				if (category.Status == 1)
				{
					category.Status = 0;
					messageText = "Kategori engellendi, yayından kaldırıldı.";
				}
				else
				{
					category.Status = 1;
					messageText = "Kategori engeli kaldırıldı, tekrar yayında.";
				}

				dbContext.SaveChanges();

				return Json(new { status = true, message = messageText }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "Engellemek istenen kategori sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult Import()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Import(HttpPostedFileBase categoryFile)
		{
			categoryFile.SaveAs(@"D:\" + categoryFile.FileName);

			string[] lines = System.IO.File.ReadAllLines(@"D:\" + categoryFile.FileName);

			int parentID = 0;

			foreach (string item in lines)
			{
				if (item.StartsWith(">"))
				{
					string[] parts = item.Split('|');
					parts[0] = parts[0].Replace(">", String.Empty);
					
					var category = dbContext.Category.Create();
					category.Title = parts[0].Trim();
					category.Slug = StringHelper.Slug(parts[0].Trim());

					category.Icon = parts[1].Trim().Equals("?") ? "cdi cdi-arrow-right-alt2" : parts[1].Trim();
					category.Color = parts[2].Trim().Equals("?") ? "rgb(12, 56, 98)" : parts[2].Trim();
					category.Description = parts[3].Trim().Equals("?") ? "-" : parts[3].Trim();
					category.Status = 1;

					dbContext.Category.Add(category);
					dbContext.SaveChanges();

					parentID = category.ID;
				}
				else
				{
					string[] parts = item.Split('|');
					
					var category = dbContext.Category.Create();
					category.Title = parts[0].Trim();
					category.Slug = StringHelper.Slug(parts[0].Trim());
	
					category.Icon = parts[1].Trim().Equals("?") ? "cdi cdi-arrow-right-alt2" : parts[1].Trim();
					category.Color = parts[2].Trim().Equals("?") ? "rgb(12, 56, 98)" : parts[2].Trim();
					category.Description = parts[3].Trim().Equals("?") ? "-" : parts[3].Trim();
					category.ParentID = parentID;
					category.Status = 1;

					dbContext.Category.Add(category);
					dbContext.SaveChanges();
				}
			}

			return View();
		}
	}
}