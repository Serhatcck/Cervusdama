using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("Ticket")]
	public class Ticket
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		[MaxLength(20)]
		public string Title { get; set; }

		[Required]
		[MaxLength(20)]
		public string Slug { get; set; }

		private ICollection<Article> _articles;

		public virtual ICollection<Article> Articles
		{
			get { return _articles ?? (_articles = new HashSet<Article>()); }
			protected set { _articles = value; }
		}

		private ICollection<Question> _questions;

		public virtual ICollection<Question> Questions
		{
			get { return _questions ?? (_questions = new HashSet<Question>()); }
			protected set { _questions = value; }
		}

		private ICollection<CodeLibrary> _codeLibraries;

		public virtual ICollection<CodeLibrary> CodeLibraries
		{
			get { return _codeLibraries ?? (_codeLibraries = new HashSet<CodeLibrary>()); }
			protected set { _codeLibraries = value; }
		}
	}
}
