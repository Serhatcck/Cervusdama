using CervusDama.Authorization;
using CervusDama.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Admin.Controllers
{
	[Authorization(Permission = "Administrator")]
	public class RoleController : Base.BaseController
	{
		// GET: Role
		public ActionResult Index()
		{
			var roles = dbContext.Role.ToList();
			return View(roles);
		}

		[HttpPost]
		public JsonResult Insert(Role data)
		{
			if (!String.IsNullOrEmpty(data.Name))
			{
				if (!dbContext.Role.Any(r => r.Name.Equals(data.Name)))
				{
					var role = dbContext.Role.Create();
					role.Name = data.Name;

					dbContext.Role.Add(role);
					dbContext.SaveChanges();

					return Json(new { status = true, message = "Rol ekleme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "Sistemde aynı isimde rol zaten tanımlı. Farklı bir isim belirtin." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Rol için isim belirtilmeli." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult Edit(Role data)
		{
			if (!String.IsNullOrEmpty(data.Name) && data.ID > 0)
			{
				if (dbContext.Role.Any(r => r.ID == data.ID))
				{
					if (!dbContext.Role.Any(r => r.Name.Equals(data.Name)))
					{
						var role = dbContext.Role.Single(r=>r.ID == data.ID);
						role.Name = data.Name;

						dbContext.SaveChanges();

						return Json(new { status = true, message = "Rol düzenleme işlemi başarılı." }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json(new { status = false, message = "Sistemde aynı isimde rol zaten tanımlı. Farklı bir isim belirtin." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { status = false, message = "Düzenlemek istenen rol sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Rol için isim belirtilmeli." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult Delete(int id)
		{
			if (dbContext.Role.Any(r => r.ID == id))
			{
				if (!dbContext.User.Any(u => u.RoleID == id))
				{
					var role = dbContext.Role.Single(r => r.ID == id);
					dbContext.Role.Remove(role);
					dbContext.SaveChanges();

					return Json(new { status = true, message = "Rol sistemden silindi." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "Role ait kullanıcılar var, silme işlemi iptal edildi." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Silmek istenen rol sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}