using CervusDama.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Admin.Controllers
{
    [Authorization(Permission = "Administrator,Moderator")]
    public class CodeLibraryController : Base.BaseController
    {
        // GET: CodeLibrary
        public ActionResult Index()
        {
            return View();
        }
    }
}