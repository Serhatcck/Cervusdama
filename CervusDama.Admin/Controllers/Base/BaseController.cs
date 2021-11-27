using System.Web.Mvc;

namespace CervusDama.Admin.Controllers.Base
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

		protected override void Initialize(System.Web.Routing.RequestContext requestContext)
		{
			if (_dbContext == null)
			{
				_dbContext = new Data.CervusDamaContext();
			}

			base.Initialize(requestContext);
		}

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