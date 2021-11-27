using CervusDama.Data.Model.ArticleModel;
using CervusDama.Data.Model.CategoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CervusDama.Web.Controllers
{
	public class CategoryController : Base.BaseController
	{
		// GET: Category
		public ActionResult Index()
		{
			var categories = dbContext.Category.Where(c => c.Status == 1).Select(s => new CategoryListModel()
			{
				ID = s.ID,
				Color = s.Color,
				Description = s.Description,
				Icon = s.Icon,
				ParentID = s.ParentID.HasValue ? s.ParentID.Value : 0,
				Slug = s.Slug,
				Status = s.Status,
				Title = s.Title,
				Hit = dbContext.Article.Count(a => a.Categories.Any(c => c.ID == s.ID))
			}).ToList();

			return View(categories);
		}

		public ActionResult ListCategory(int tip)
		{
			List<CategoryAllListModel> categoryList = new List<CategoryAllListModel>();

			foreach (var item in dbContext.Category.Where(c => c.ParentID == null && c.Status == 1).ToList())
			{
				CategoryAllListModel model = new CategoryAllListModel();
				model.Icon = item.Icon;
				model.Slug = item.Slug;
				model.Title = item.Title;
				model.Color = item.Color;
				model.SubCategories = new List<CategoryListModel>();

				foreach (var subItem in dbContext.Category.Where(c => c.ParentID == item.ID && c.Status == 1))
				{
					CategoryListModel ct = new CategoryListModel();
					ct.Title = subItem.Title;
					ct.Slug = subItem.Slug;
					ct.Icon = subItem.Icon;
					ct.Color = subItem.Color;

					model.SubCategories.Add(ct);
				}

				categoryList.Add(model);
			}

			ViewBag.Tip = tip;

			return View(categoryList);
		}

		public ActionResult ArticleListCategory()
		{
			var categories = dbContext.Category.Where(c => c.Status == 1).Select(s => new CategoryListModel() {
				ID = s.ID,
				Title = s.Title
			}).OrderByDescending(o => o.ID).ToList();

			return View(categories);
		}

		public ActionResult Detail(string slug)
		{
			if (String.IsNullOrEmpty(slug))
				return RedirectToRoute("CategoryAbsent");

			if (!dbContext.Category.Any(c => c.Slug.Equals(slug)))
				return RedirectToRoute("CategoryAbsent");

			ArticleCategoryListModel model = new ArticleCategoryListModel();

			model.Articles = dbContext.Article.Where(a => a.Status == 1 && a.Categories.Any(c => c.Slug.Equals(slug))).OrderByDescending(o => o.CreateAt).Take(20).Select(s => new ArticleBigListModel()
			{
				ID = s.ID,
				Title = s.Title,
				Description = s.Description,
				Hit = s.Hit,
				CreateAt = s.CreateAt,
				Image = s.Media.Title,
				Slug = s.Slug,
				UserInfo = new Data.Model.UserModel.UserInfoModel
				{
					ID = s.User.ID,
					FirstName = s.User.FirstName,
					LastName = s.User.LastName,
					Slug = s.User.Slug
				}
			}).ToList();

			var category = dbContext.Category.FirstOrDefault(c => c.Slug.Equals(slug));
			model.Category = new Data.Model.CategoryModel.CategoryListModel()
			{
				ID = category.ID,
				Icon = category.Icon,
				Color = category.Color,
				Description = category.Description,
				Slug = category.Slug,
				Title = category.Title
			};

			return View(model);
		}
	}
}