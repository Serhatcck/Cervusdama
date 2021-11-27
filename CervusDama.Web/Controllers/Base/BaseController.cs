using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers.Base
{
	public class BaseController : Controller
	{
		// GET: Base
		private Data.CervusDamaContext _dbContext = null;

		public Data.CervusDamaContext dbContext
		{
			get
			{
				return _dbContext;
			}
		}
		/*Veri tabanı nesnesin var mı yok mu kontrol eder */
		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			if (_dbContext == null)
			{
				_dbContext = new Data.CervusDamaContext();
			}

			base.Initialize(requestContext);
		}
		/*Veri tabanı nesnesi kapanır*/
		protected override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			if (_dbContext != null)
			{
				_dbContext.Dispose();
			}
			base.OnResultExecuted(filterContext);
		}
	}
}
