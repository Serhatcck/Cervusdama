using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages

        public ActionResult Index()
		{
            return View();
		}
        
        public ActionResult UserAgreement()
        {
            return View();
        }
        
        public ActionResult UsageAgreement()
        {
            return View();
        }

        public ActionResult LegalNotices()
		{
            return View();
		}

        public ActionResult About()
		{
            return View();
		}

        public ActionResult CorrectAskQuestion()
		{
            return View();
		}
    }
}