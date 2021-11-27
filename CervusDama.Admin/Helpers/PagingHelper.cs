using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Admin.Helpers
{
	public static class PagingHelper
	{
		//Sayfalama helper.
		public static MvcHtmlString Pager(this HtmlHelper helper, int currentPage, int totalData, string url)
		{
			StringBuilder html = new StringBuilder();

			int getDataSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["paging"]);

			int totalPage = (int)Math.Ceiling((decimal)totalData / (decimal)getDataSize);

			for (int i = 1; i <= totalPage; i++)
			{
				html.Append("<a href=\"");
				html.Append(url);
				html.Append("/");
				html.Append(i.ToString());
				html.Append("\">");
				html.Append(i.ToString());
				html.Append("</a>");
			}

			return MvcHtmlString.Create(html.ToString());
		}
	}
}