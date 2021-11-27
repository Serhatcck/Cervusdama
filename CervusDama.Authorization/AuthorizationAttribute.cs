using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CervusDama.Authorization
{
	public class AuthorizationAttribute : AuthorizeAttribute
	{
		public string Permission { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			if (httpContext == null)
			{
				throw new ArgumentNullException("httpContext");
			}

			IPrincipal user = httpContext.User;
			if (!user.Identity.IsAuthenticated)
			{
				return false;
			}

			if (Permission.Count() > 0)
			{
				if (!user.IsInRole(Permission))
				{
					return false;
				}
			}

			return true;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			filterContext.Result = new RedirectToRouteResult(
			new RouteValueDictionary(
				new
				{
					controller = "Home",
					action = "Index"
				})
			);
		}
	}
}
