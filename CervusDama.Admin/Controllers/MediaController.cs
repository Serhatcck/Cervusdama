using CervusDama.Authorization;
using CervusDama.Data.Model.MediaModel;
using CervusDama.Utility.ImageHelper;
using CervusDama.Utility.StringHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Admin.Controllers
{
	[Authorization(Permission = "Administrator,Moderator")]
	public class MediaController : Base.BaseController
	{
		// GET: Media
		public ActionResult Index()
		{
			var media = dbContext.Media.OrderByDescending(o => o.ID).Select(s => new MediaListModel
			{
				ID = s.ID,
				Title = s.Title,
				MediaType = s.MediaType
			}).ToList();


			return View(media);
		}

		public ActionResult Upload()
		{
			return View();
		}

		[HttpPost]
		public JsonResult DoUpload(HttpPostedFileBase file)
		{
			if (file != null)
			{
				if (file.ContentType == "image/jpeg" || file.ContentType == "image/png")
				{
					string uploadDir = System.Configuration.ConfigurationManager.AppSettings.Get("uploadDir");
					string fileName = StringHelper.GenerateFileName(file.FileName, uploadDir + "original\\");

					file.SaveAs(uploadDir + "original\\" + fileName);

					ImageHelper.ImageCrop(uploadDir + "original\\" + fileName, uploadDir + "thumbnail\\" + fileName, 180, 180);
					ImageHelper.ImageCrop(uploadDir + "original\\" + fileName, uploadDir + "medium\\" + fileName, 300, 300);
					ImageHelper.ImageCrop(uploadDir + "original\\" + fileName, uploadDir + "large\\" + fileName, 600, 600);

					var media = dbContext.Media.Create();
					media.CreateAt = DateTime.Now;
					media.Description = "-";
					media.MediaType = file.ContentType;
					media.Status = 1;
					media.Title = fileName;
					media.UserID = dbContext.User.Single(u => u.Slug.Equals("sistem")).ID;

					dbContext.Media.Add(media);
					dbContext.SaveChanges();

					return Json(new { status = true, message = "Dosya yükleme işlemi başarılı.", fileName = fileName }, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(new { status = false, message = "Sadece .jpg ve .png formatında resim dosyası yükleyebilirsiniz." }, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				return Json(new { status = false, message = "Lütfen dosya seçiniz." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult ImageDetail(int id)
		{
			if (dbContext.Media.Any(m => m.ID == id))
			{
				return Json(new { status = true, message = "başarılı", mediaData = dbContext.Media.Single(m => m.ID == id) }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "Medya sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult ImageInfoUpdate(int id, string description)
		{
			if (dbContext.Media.Any(m => m.ID == id))
			{
				var media = dbContext.Media.Single(m => m.ID == id);
				media.Description = description;
				media.EditAt = DateTime.Now;

				dbContext.SaveChanges();

				return Json(new { status = true, message = "Medya bilgisi düzenlendi." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "Düzenlemek istenen Medya sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult Delete(int id)
		{
			if (dbContext.Media.Any(m => m.ID == id))
			{
				string uploadDir = System.Configuration.ConfigurationManager.AppSettings.Get("uploadDir");

				var media = dbContext.Media.Single(m => m.ID == id);

				if (ImageHelper.ImageDelete(uploadDir + "thumbnail\\" + media.Title))
				{
					if (ImageHelper.ImageDelete(uploadDir + "medium\\" + media.Title))
					{
						if (ImageHelper.ImageDelete(uploadDir + "large\\" + media.Title))
						{
							if (ImageHelper.ImageDelete(uploadDir + "original\\" + media.Title))
							{
								dbContext.Media.Remove(media);
								dbContext.SaveChanges();

								return Json(new { status = true, message = "Medya sistemden silindi." }, JsonRequestBehavior.AllowGet);
							}
						}
					}
				}

				return Json(new { status = false, message = "Silme işlemi sırasında bir hata oluştu." }, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json(new { status = false, message = "Silmek istenen Medya sistemde bulunamadı." }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpPost]
		public JsonResult EditorImageList()
		{
			var images = dbContext.Media.Where(m => m.UserID == 1 || m.UserID == 2).OrderByDescending(o => o.CreateAt).Select(s => new MediaListModel
			{
				ID = s.ID,
				Title = s.Title
			}).ToList();

			return Json(images, JsonRequestBehavior.AllowGet);
		}
	}
}