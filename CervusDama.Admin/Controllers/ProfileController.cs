using CervusDama.Authorization;
using CervusDama.Data.Model.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CervusDama.Admin.Controllers
{
    [Authorization(Permission = "Administrator,Moderator")]
    public class ProfileController : Base.BaseController
    {
        // GET: Profile
        public ActionResult Index()
        {
            int id = int.Parse(User.Identity.Name);

            var user = dbContext.User.Single(u => u.ID == id);

            UserListModel model = new UserListModel();
            model.CreateAt = user.CreateAt;
            model.Email = user.Email;
            model.FirstName = user.FirstName;
            model.ID = user.ID;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.NickName = user.NickName;
            model.Slug = user.Slug;

            return View(model);
        }

        [HttpPost]
        public JsonResult EditUserInfo(UserEditModel data)
		{
			if (ModelState.IsValid)
			{
                int id = int.Parse(User.Identity.Name);

                var user = dbContext.User.Single(u => u.ID == id);
                user.FirstName = data.FirstName;
                user.LastName = data.LastName;
                user.Email = data.Email;
                user.NickName = data.NickName;

                dbContext.SaveChanges();

                return Json(new { status = true, message = "Bilgileriniz güncellendi." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = false, message = string.Join(" ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) }, JsonRequestBehavior.AllowGet);
		}

        public JsonResult ChangePassword(UpdatePasswordModel data)
		{
			if (ModelState.IsValid)
			{
                int id = int.Parse(User.Identity.Name);
                data.OldPassword = Crypto.HashPassword(data.OldPassword);
                
                if (dbContext.User.Any(u => u.ID == id && u.Password.Equals(data.OldPassword)))
                {
                    var user = dbContext.User.Single(u => u.ID == id);
                    user.Password = Crypto.HashPassword(data.Password);
                    dbContext.SaveChanges();

                    return Json(new { status = true, message = "Şifreniz güncellendi." }, JsonRequestBehavior.AllowGet);
				}
				else
				{
                    return Json(new { status = false, message = "Eski şifreniz hatalı." }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { status = false, message = string.Join(" ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) }, JsonRequestBehavior.AllowGet);
        }
    }
}