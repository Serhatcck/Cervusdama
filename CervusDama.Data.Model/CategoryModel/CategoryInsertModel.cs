using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CervusDama.Data.Model.CategoryModel
{
	public class CategoryInsertModel
	{
		public int ID { get; set; }

		[Display(Name = "Kategori Adı")]
		[Required(ErrorMessage = "İsim alanı zorunludur.")]
		[MaxLength(60, ErrorMessage = "İsim en fazla 60 karakter olabilir.")]
		[RegularExpression(@"[a-zA-Z0-9\sçöışüğÇÖİŞÜĞ\-\+]+", ErrorMessage = "Kategori başlığı standartlara uygun değil.")]
		public string Title { get; set; }

		[Display(Name = "Açıklama")]
		[MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
		public string Description { get; set; }

		[Display(Name ="Üst Kategori")]
		public int? ParentID { get; set; }

		[Display(Name = "Icon")]
		[MaxLength(100, ErrorMessage = "Icon ismi en fazla 100 katakter olabilir.")]
		public string Icon { get; set; }

		[Display(Name = "Renk")]
		[MaxLength(100, ErrorMessage = "Renk değeri en fazla 100 katakter olabilir.")]
		public string Color { get; set; }

		[Display(Name = "Slug")]
		[MaxLength(60, ErrorMessage = "Slug alanı en fazla 60 karakter olabilir.")]
		[RegularExpression(@"^([A-Za-z\-]+)$", ErrorMessage = "Slug sadece küçük harf ve tire içerebilir.")]
		public string Slug { get; set; }

		public List<SelectListItem> Parents { get; set; }
	}
}
