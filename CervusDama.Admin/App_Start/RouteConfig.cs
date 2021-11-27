using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CervusDama.Admin
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			#region Home
			routes.MapRoute(
				name: "Dashboard",
				url: "anasayfa",
				defaults: new { controller = "Home", action = "Dashboard" }
			);

			routes.MapRoute(
				name: "LogOut",
				url: "cikis",
				defaults: new { controller = "Home", action = "LogOut" }
			);

			routes.MapRoute(
				name: "YearChartData",
				url: "chart-data/{year}",
				defaults: new { controller = "Home", action = "YearChartData", year = UrlParameter.Optional }
			);
			#endregion

			#region Media
			routes.MapRoute(
				name: "Media",
				url: "medya",
				defaults: new { controller = "Media", action = "Index" }
			);

			routes.MapRoute(
				name: "Upload",
				url: "medya-yukle",
				defaults: new { controller = "Media", action = "Upload" }
			);
			#endregion

			#region Article
			routes.MapRoute(
				name: "Articles",
				url: "yazilar/{page}",
				defaults: new { controller = "Article", action = "Index", page = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "ArticleInsert",
				url: "yazi-ekle",
				defaults: new { controller = "Article", action = "Insert" }
			);

			routes.MapRoute(
				name: "ArticleEdit",
				url: "yazi-duzenle/{id}",
				defaults: new { controller = "Article", action = "Edit", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "ArticlePin",
				url: "yazi-sabitle",
				defaults: new { controller = "Article", action = "AddPin" }
			);

			routes.MapRoute(
				name: "ArticleBan",
				url: "yazi-engelle",
				defaults: new { controller = "Article", action = "ArticleBan" }
			);

			routes.MapRoute(
				name: "ArticleDelete",
				url: "yazi-sil",
				defaults: new { controller = "Article", action = "Delete" }
			);
			#endregion

			#region ArticleSeries
			routes.MapRoute(
				name: "ArticleSeries",
				url: "seri-yazilar/{page}",
				defaults: new { controller = "ArticleSeries", action = "Index", page = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "ArticleSeriesInsert",
				url: "yazi-serisi-olustur",
				defaults: new { controller = "ArticleSeries", action = "Insert", page = UrlParameter.Optional }
			);
			#endregion;

			#region Users
			routes.MapRoute(
				name: "Users",
				url: "kullanicilar/{page}",
				defaults: new { controller = "Users", action = "Index", page = UrlParameter.Optional }
			);
			#endregion

			#region Category
			routes.MapRoute(
				name: "Categories",
				url: "kategori/{page}",
				defaults: new { controller = "Category", action = "Index", page = UrlParameter.Optional }
			);
			#endregion

			#region Ticket
			routes.MapRoute(
				name: "Tickets",
				url: "etiket/{page}",
				defaults: new { controller = "Ticket", action = "Index", page = UrlParameter.Optional }
			);
			#endregion

			#region FeedBack
			routes.MapRoute(
				name: "FeedBack",
				url: "geri-bildirimler/{page}",
				defaults: new { controller = "FeedBack", action = "Index", page = UrlParameter.Optional }
			);
			#endregion

			#region Role
			routes.MapRoute(
				name: "Roles",
				url: "kullanici-rolleri",
				defaults: new { controller = "Role", action = "Index" }
			);
			#endregion;

			#region Question
			routes.MapRoute(
				name: "Questions",
				url: "sorular",
				defaults: new { controller = "Question", action = "Index" }
			);

			routes.MapRoute(
				name: "QuestionEdit",
				url: "soru-duzenle/{id}",
				defaults: new { controller = "Question", action = "Edit", id = UrlParameter.Optional }
			);
			#endregion

			#region Answer
			routes.MapRoute(
				name: "Answers",
				url: "cevaplar",
				defaults: new { controller = "Answer", action = "Index" }
			);

			routes.MapRoute(
				name: "AnswerEdit",
				url: "cevap-duzenle/{id}",
				defaults: new { controller = "Answer", action = "Edit", id = UrlParameter.Optional }
			);
			#endregion

			#region CodeLibrary
			routes.MapRoute(
				name: "CodeLibrary",
				url: "kod-kutuphanesi",
				defaults: new { controller = "CodeLibrary", action = "Index" }
			);
			#endregion

			#region Profile
			routes.MapRoute(
				name: "Profile",
				url: "profil",
				defaults: new { controller = "Profile", action = "Index" }
			);
			#endregion

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}",
				defaults: new { controller = "Home", action = "Index" }
			);
		}
	}
}
