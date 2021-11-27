using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CervusDama.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			#region HomeController
			routes.MapRoute(
				name: "HomaPage",
				url: "anasayfa",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "RulesPage",
				url: "kurallar",
				defaults: new { controller = "Home", action = "Rules" }
			);
			#endregion

			#region PageController
			routes.MapRoute(
				name: "NoticesAndPolicies",
				url: "bildirimler-ve-yonergeler",
				defaults: new { controller = "Pages", action = "Index" }
			);

			routes.MapRoute(
				name: "UserAggrement",
				url: "kullanici-sozlesmesi",
				defaults: new { controller = "Pages", action = "UserAgreement" }
			);

			routes.MapRoute(
				name: "About",
				url: "hakkinda",
				defaults: new { controller = "Pages", action = "About" }
			);
			
			routes.MapRoute(
				name: "AskRightQuestion",
				url: "dogru-soru-sorma-teknikleri",
				defaults: new { controller = "Pages", action = "CorrectAskQuestion" }
			);
			
			routes.MapRoute(
				name: "Help",
				url: "yardim-konulari",
				defaults: new { controller = "Pages", action = "Help" }
			);
			
			routes.MapRoute(
				name: "SiteMap",
				url: "site-haritasi",
				defaults: new { controller = "Pages", action = "SiteMap" }
			);
			#endregion

			#region UserController
			routes.MapRoute(
				name: "LogOut",
				url: "cikis",
				defaults: new { controller = "User", action = "LogOut" }
			);

			routes.MapRoute(
				name: "CheckMail",
				url: "uyelik-onay/{userKey}/{key}",
				defaults: new { controller = "User", action = "CheckMail", userKey = UrlParameter.Optional, key = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "ForgotPassword",
				url: "sifremi-unuttum",
				defaults: new { controller = "User", action = "ForgotPassword" }
			);
			
			routes.MapRoute(
				name: "ForgotPasswordCallBack",
				url: "yeni-sifre-olustur/{userKey}/{rePasswordKey}",
				defaults: new { controller = "User", action = "ForgotPasswordCallBack", userKey = UrlParameter.Optional, rePasswordKey = UrlParameter.Optional }
			);
			#endregion

			#region ArticleController
			routes.MapRoute(
				name: "Articles",
				url: "makale/{slug}",
				defaults: new { controller = "Article", action = "Index", slug = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "ArticleInsert",
				url: "makale-ekle",
				defaults: new { controller = "Article", action = "Insert" }
			);
			
			routes.MapRoute(
				name: "ArticleEdit",
				url: "makale-duzenle/{id}",
				defaults: new { controller = "Article", action = "Edit", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "ArticleDetail",
				url: "makale-detay/{userName}/{slug}",
				defaults: new { controller = "Article", action = "Detail", userName = UrlParameter.Optional, slug = UrlParameter.Optional }
			);
			
			routes.MapRoute(
				name: "ArticleAbsent",
				url: "makale-bulunamadi",
				defaults: new { controller = "Article", action = "ArticleAbsent" }
			);
			
			routes.MapRoute(
				name: "CategoryAbsent",
				url: "kategori-bulunamadi",
				defaults: new { controller = "Article", action = "CategoryAbsent" }
			);

			routes.MapRoute(
				name: "ArticleLike",
				url: "makale-begeni",
				defaults: new { controller = "Article", action = "Like" }
			);
			#endregion

			#region ArticleSeriesController
			routes.MapRoute(
				name: "ArticleSeries",
				url: "seri-makaleler",
				defaults: new { controller = "ArticleSeries", action = "Index" }
			);

			routes.MapRoute(
				name: "ArticleSeriesAbsent",
				url: "makale-serisi-bulunamadi",
				defaults: new { controller = "ArticleSeries", action = "ArticleSeriesAbsent" }
			);

			routes.MapRoute(
				name: "ArticleSeriesDetail",
				url: "makale-serisi/{articleSeriesSlug}/{articleSlug}",
				defaults: new { controller = "ArticleSeries", action = "Detail", articleSeriesSlug = UrlParameter.Optional, articleSlug = UrlParameter.Optional }
			);
			#endregion

			#region FeedBackController
			routes.MapRoute(
				name: "FeedBack",
				url: "geri-bildirim-gonder",
				defaults: new { controller = "FeedBack", action = "Index" }
			);
			#endregion

			#region CategoryController
			routes.MapRoute(
				name: "Categories",
				url: "kategoriler",
				defaults: new { controller = "Category", action = "Index" }
			);

			routes.MapRoute(
				name: "CategoryDetail",
				url: "kategori-detay/{slug}",
				defaults: new { controller = "Category", action = "Detail", slug = UrlParameter.Optional }
			);
			#endregion

			#region QuestionController
			routes.MapRoute(
				name: "Questions",
				url: "soru-cevap",
				defaults: new { controller = "Question", action = "Index" }
			);

			routes.MapRoute(
				name: "QuestionInsert",
				url: "soru-ekle",
				defaults: new { controller = "Question", action = "Insert" }
			);

			routes.MapRoute(
				name: "QuestionDetail",
				url: "soru-detay/{slug}",
				defaults: new { controller = "Question", action = "Detail", slug = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "QuestionEdit",
				url: "soru-duzenle/{slug}",
				defaults: new { controller = "Question", action = "Edit", slug = UrlParameter.Optional }
			);
			#endregion

			#region SearchController
			routes.MapRoute(
				name: "Search",
				url: "arama",
				defaults: new { controller = "Search", action = "Index", query = UrlParameter.Optional }
			);
			#endregion

			#region TicketController
			routes.MapRoute(
				name: "TicketDetail",
				url: "etiket/{slug}",
				defaults: new { controller = "Ticket", action = "Index", slug = UrlParameter.Optional}
			);
			#endregion

			#region ProfileController
			routes.MapRoute(
				name: "UserProfile",
				url: "profil/{slug}",
				defaults: new { controller = "Profile", action = "Index", slug = UrlParameter.Optional }
			);
			
			routes.MapRoute(
				name: "ProfileAbsent",
				url: "kullanici-bulunamadi",
				defaults: new { controller = "Profile", action = "ProfileAbsent"}
			);
			#endregion

			#region CodeLibraryController
			routes.MapRoute(
				name: "CodeLibraries",
				url: "kod-kutuphanesi",
				defaults: new { controller = "CodeLibrary", action = "Index" }
			);
			#endregion

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
