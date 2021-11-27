using CervusDama.Data.Model.MediaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
	public class MediaController : Base.BaseController
	{
		// GET: Media
		public JsonResult EditorImageList()
		{
			var images = dbContext.Media.Where(m => m.UserID == 1 || m.UserID == 2).OrderByDescending(o => o.CreateAt).Select(s => new MediaListModel{
				ID = s.ID,
				Title = s.Title
			}).ToList();

			return Json(images, JsonRequestBehavior.AllowGet);
		}
	}
}