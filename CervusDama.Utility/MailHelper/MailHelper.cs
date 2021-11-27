using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Utility.MailHelper
{
	public class MailHelper
	{
		public enum MailTemplates
		{
			NewMemeber,
			ForgotPassword
		}

		private string mailTemplateDir = "D:\\Projects\\CervusDama\\CervusDama.Web\\Content\\mailTemplates\\";

		public bool SendMail(string mailAddr, string subject)
		{
			try
			{
				MailMessage mail = new MailMessage();
				mail.From = new MailAddress("sup.cervusdama@gmail.com");
				mail.To.Add(mailAddr);
				mail.Subject = subject;
				mail.IsBodyHtml = true;
				mail.Body = "Kaydınız tamamlandı. Hesabınızı onaylamak için <a href=\"#\">Tıklayınız</a>";

				SmtpClient sc = new SmtpClient();
				sc.Port = 587;
				sc.Host = "smtp.gmail.com";
				sc.EnableSsl = true;
				sc.Credentials = new NetworkCredential("sup.cervusdama@gmail.com", "*Cd2105613324");
				sc.Send(mail);

				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public bool SendTemplateMail(string mailAddr, string subject, MailTemplates mailTemplate, Dictionary<string, string> replacements)
		{
			try
			{
				MailMessage mail = new MailMessage();
				mail.From = new MailAddress("sup.cervusdama@gmail.com");
				mail.To.Add(mailAddr);
				mail.Subject = subject;
				mail.IsBodyHtml = true;

				using (System.IO.StreamReader streamReader = new System.IO.StreamReader(mailTemplateDir + mailTemplate.ToString() + ".txt", Encoding.GetEncoding("UTF-8"))) // mesaj formu
				{
					mail.Body = streamReader.ReadToEnd();
				}

				foreach (var key in replacements)
				{
					mail.Body = mail.Body.Replace(key.Key, key.Value);
				}

				SmtpClient sc = new SmtpClient();
				sc.Port = 587;
				sc.Host = "smtp.gmail.com";
				sc.EnableSsl = true;
				sc.Credentials = new NetworkCredential("sup.cervusdama@gmail.com", "*Cd2105613324");
				sc.Send(mail);

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
