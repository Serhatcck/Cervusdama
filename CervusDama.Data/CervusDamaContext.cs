using CervusDama.Data.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CervusDama.Data
{
	public class CervusDamaContext : DbContext
	{
		public CervusDamaContext()
		{
			/*Caglar Abi */
			Database.Connection.ConnectionString = "Data Source=localhost,1432;Initial Catalog=CervusDama;Persist Security Info=True;User ID=sa;Password=Pwd12345!";                                                                            
			/* Cicek */
			//Database.Connection.ConnectionString = "Server=localhost;Database=cervusdama;Trusted_Connection=True;";
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

			modelBuilder.Entity<Category>()
						.HasOptional(c => c.Parent)
						.WithMany()
						.HasForeignKey(c => c.ParentID);

			modelBuilder.Entity<Article>()
						.HasMany<Category>(s => s.Categories)
						.WithMany(c => c.Articles)
						.Map(cs =>
						{
							cs.MapLeftKey("ArticleRefID");
							cs.MapRightKey("CategoryRefID");
							cs.ToTable("ArticleCategories");
						});

			modelBuilder.Entity<Article>()
						.HasMany<Ticket>(s => s.Tickets)
						.WithMany(c => c.Articles)
						.Map(cs =>
						{
							cs.MapLeftKey("ArticleRefID");
							cs.MapRightKey("TicketRefID");
							cs.ToTable("ArticleTickets");
						});

			modelBuilder.Entity<CodeLibrary>()
						.HasMany<Ticket>(s => s.Tickets)
						.WithMany(c => c.CodeLibraries)
						.Map(cs =>
						{
							cs.MapLeftKey("CodeLibraryRefID");
							cs.MapRightKey("TicketRefID");
							cs.ToTable("CodeLibraryTickets");
						});

			modelBuilder.Entity<Question>()
						.HasMany<Ticket>(s => s.Tickets)
						.WithMany(c => c.Questions)
						.Map(cs =>
						{
							cs.MapLeftKey("QuestionRefID");
							cs.MapRightKey("TicketRefID");
							cs.ToTable("QuestionTickets");
						});
		}

		public DbSet<Role> Role { get; set; }

		public DbSet<User> User { get; set; }

		public DbSet<Article> Article { get; set; }

		public DbSet<Comment> Comment { get; set; }

		public DbSet<Category> Category { get; set; }

		public DbSet<Ticket> Ticket { get; set; }

		public DbSet<Media> Media { get; set; }

		public DbSet<CodeLibrary> CodeLibrary { get; set; }

		public DbSet<Answer> Answer { get; set; }

		public DbSet<Question> Question { get; set; }

		public DbSet<ArticleSeries> ArticleSeries { get; set; }
		
		public DbSet<ArticleSeriesCategory> ArticleSeriesCategories { get; set; }

		public DbSet<ArticleSeriesArticles> ArticleSeriesArticles { get; set; }

		public DbSet<FeedBack> FeedBack { get; set; }

		public DbSet<ArticleVoting> ArticleVoting { get; set; }

		public DbSet<CommentVoting> CommentVoting { get; set; }

		public DbSet<QuestionVoting> QuestionVoting { get; set; }

		public DbSet<AnswerVoting> AnswerVoting { get; set; }
	}
}
