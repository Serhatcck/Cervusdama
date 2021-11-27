using CervusDama.Data.Model.UserModel;
using CervusDama.Utility.HashHelper;
using CervusDama.Utility.MailHelper;
using CervusDama.Utility.StringHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace CervusDama.Web.Controllers
{
	public class UserController : Base.BaseController
	{
		// GET: User
		[HttpPost]
		[ValidateInput(true)]
		[ValidateAntiForgeryToken]
		public JsonResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				if (dbContext.User.Any(u => u.Email == model.Email))
				{
					var user = dbContext.User.Single(u => u.Email == model.Email);

					if (user.Status == 1)
					{
						if(Crypto.VerifyHashedPassword(user.Password, model.Password))
						{
							FormsAuthentication.SetAuthCookie(user.ID.ToString(), false);
							user.LastLogin = DateTime.Now;
							dbContext.SaveChanges();

							return Json(new { Status = true, Message = "Kullanıcı giriş işlemi başarılı." }, JsonRequestBehavior.AllowGet);
						}
						else
						{
							return Json(new { Status = false, Message = "Girilen şifre hatalı." }, JsonRequestBehavior.AllowGet);
						}
					}
					else if (user.Status == 2)
					{
						return Json(new { Status = false, Message = "Hesabınızı aktif etmelisiniz. Aktivasyon mailini tekrar almak için <a href=\"#\">Tıklayınız</a>" }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json(new { Status = false, Message = "Giriş yapmak istenen hesap sistemde engellenmiştir, giriş yapılamaz." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { Status = false, Message = "Girilen email adresi hatalı." }, JsonRequestBehavior.AllowGet);
				}
			}

			var errorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();

			return Json(new { Status = false, Message = errorList }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[ValidateInput(true)]
		[ValidateAntiForgeryToken]
		public JsonResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				if (model.UserAgreementIsCheck)
				{
					if (!dbContext.User.Any(u => u.Email == model.Email))
					{
						var user = dbContext.User.Create();
						user.CreateAt = DateTime.Now;
						user.Email = model.Email;
						user.FirstName = model.FirstName.Trim();
						user.LastLogin = null;
						user.LastName = model.LastName.Trim();
						user.MailCheckKey = Guid.NewGuid();
						user.NickName = "-------";
						user.Password = Crypto.HashPassword(model.Password);
						user.RePassKey = null;
						user.RoleID = 4;
						user.Slug = StringHelper.Slug(model.FirstName);
						user.Status = 2;
						user.WebSite = "-";
						user.CityName = "-";
						user.Bio = "-";
						user.Tier = "1";

						DrawProfileImage(user.Slug, user.FirstName.Substring(0, 1) + user.LastName.Substring(0, 1));

						dbContext.User.Add(user);
						dbContext.SaveChanges();

						MailHelper mail = new MailHelper();

						Dictionary<string, string> replecament = new Dictionary<string, string>();
						replecament.Add("{link}", "http://cervusdama.com/uyelik-onay/" + user.ID + "/" + user.MailCheckKey);
						replecament.Add("{nameSurname}", model.FirstName + " " + model.LastName);

						if (mail.SendTemplateMail(model.Email, "Cervus Dama Üyelik Onayı", MailHelper.MailTemplates.NewMemeber, replecament))
						{
							return Json(new { Status = false, Message = "Kayıt işlemi başarıyla tamamlandı. Mail adresinize gönderilen onay maili ile hesabınızı aktif hale getirerek giriş yapabilirsiniz.." }, JsonRequestBehavior.AllowGet);
						}
						else
						{
							return Json(new { Status = false, Message = "Heabınız oluşturulurken bir sorun oluştu." }, JsonRequestBehavior.AllowGet);
						}
					}
					else
					{
						return Json(new { Status = false, Message = "Girilen mail adresine ait üyelik zaten sistemde var." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { Status = false, Message = "Kullanıcı sözleşmesini okuyup kabul etmeniz gerekiyor." }, JsonRequestBehavior.AllowGet);
				}
			}

			var errorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();

			return Json(new { Status = false, Message = errorList }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult CheckMail(int userKey, Guid key)
		{
			if (key != Guid.Empty && key != null)
			{
				if (dbContext.User.Any(u => u.MailCheckKey == key && u.Status == 2))
				{
					var user = dbContext.User.Single(u => u.MailCheckKey == key && u.Status == 2);
					user.Status = 1;

					dbContext.SaveChanges();

					ViewBag.Message = "Hesabınız onaylanmıştır. Hesap bilgileriniz ile giriş yapabilirsiniz..";
				}
				else
				{
					ViewBag.Message = "Geçersiz onay kodu.";
				}
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		public ActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[ValidateInput(true)]
		[ValidateAntiForgeryToken]
		public JsonResult ForgotPassword(ForgotPasswordModel model)
		{
			if (ModelState.IsValid)
			{
				if (dbContext.User.Any(u => u.Email == model.Email))
				{
					var user = dbContext.User.Single(u => u.Email == model.Email);
					user.RePassKey = Guid.NewGuid();

					dbContext.SaveChanges();

					MailHelper mail = new MailHelper();

					Dictionary<string, string> replecament = new Dictionary<string, string>();
					replecament.Add("{link}", "http://cervusdama.com/yeni-sifre/" + user.ID + "/" + user.RePassKey);
					replecament.Add("{nameSurname}", user.FirstName + " " + user.LastName);

					if (mail.SendTemplateMail(model.Email, "Cervus Dama Şifre Sıfırlama", MailHelper.MailTemplates.ForgotPassword, replecament))
					{
						return Json(new { Status = true, Message = "Şifre sıfırlama bağlantınız mail adresinize gönderildi." }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json(new { Status = false, Message = "Şifre sıfırlama bağlantısı gönderilirken bir hata oluştu." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { Status = false, Message = "Girilen Email adresi ile ilişkili hesap bulunamadı." }, JsonRequestBehavior.AllowGet);
				}
			}

			var errorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();

			return Json(new { Status = false, Message = errorList }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ForgotPasswordCallBack(int userKey, Guid rePasswordKey)
		{
			ChangePasswordModel model = new ChangePasswordModel();

			if (userKey > 0 && rePasswordKey != Guid.Empty)
			{
				model.UserKey = userKey;
				model.RePasswordKey = rePasswordKey;
			}
			else
			{
				ModelState.AddModelError("ErrorMessage", "Geçeriz bilgiler.");
			}

			return View(model);
		}

		[HttpPost]
		[ValidateInput(true)]
		[ValidateAntiForgeryToken]
		public JsonResult ForgotPasswordCallBack(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{
				if (model.UserKey > 0 && model.RePasswordKey != Guid.Empty)
				{
					if (dbContext.User.Any(u => u.ID == model.UserKey && u.RePassKey == model.RePasswordKey))
					{
						var user = dbContext.User.Single(u => u.ID == model.UserKey && u.RePassKey == model.RePasswordKey);
						user.Password = HashHelper.Md5(model.Password);
						user.RePassKey = null;

						dbContext.SaveChanges();

						return Json(new { Status = true, Message = "Şifreniz değiştirildi." }, JsonRequestBehavior.AllowGet);
					}
					else
					{
						return Json(new { Status = false, Message = "Gönderilen bilgiler hatalı." }, JsonRequestBehavior.AllowGet);
					}
				}
				else
				{
					return Json(new { Status = false, Message = "Gönderilen bilgiler hatalı." }, JsonRequestBehavior.AllowGet);
				}
			}

			var errorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();

			return Json(new { Status = false, Message = errorList }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult LogOut()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
		}

		public ActionResult DrawImg()
		{
			DrawProfileImage("avatar", "ÇÖ");

			return View();
		}

		private void DrawProfileImage(string fileName, string text)
		{
			string[] color = { "#0088CC", "#79b32f", "#d95b44", "#1077b9", "#5f6368", "#E5C3A0", "#B7E5A0", "#A0E5E2", "#E5A0AE", "#C3A0E5", "#A0A4E5", "#0D72B2" };

			Random rnd = new Random();

			Image img = new Bitmap(150, 150);

			Graphics drawing = Graphics.FromImage(img);
			drawing.Clear(ColorTranslator.FromHtml(color[rnd.Next(0, color.Count())]));
 
			Brush textBrush = new SolidBrush(ColorTranslator.FromHtml("#FFFFFF"));

			drawing.TextRenderingHint = TextRenderingHint.AntiAlias;
			drawing.DrawString(text, new Font(FontFamily.GenericSansSerif, 45, FontStyle.Bold), textBrush, new Rectangle(15, 40, 0, 130));

			drawing.Save();

			textBrush.Dispose();
			drawing.Dispose();

			img.Save("D:/Projects/TFS-Projects/CervusDama/CervusDama.Web/uploads/profile/" + fileName + ".png");
		}
	}
}