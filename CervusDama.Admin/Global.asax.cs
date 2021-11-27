using CervusDama.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CervusDama.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest()
        {
            if (Request.IsAuthenticated)
            {
                Context.User = System.Threading.Thread.CurrentPrincipal = new CustomPrincipal(Context.User.Identity);
            }
        }
    }
}
