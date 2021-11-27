using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CervusDama.Data.Entities
{
	[Table("User")]
	public class User
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public int RoleID { get; set; }

		[Required]
		[MaxLength(20)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(20)]
		public string LastName { get; set; }

		[Required]
		[MaxLength(20)]
		[MinLength(6)]
		public string NickName { get; set; }

		[Required]
		[MaxLength(100)]
		public string Email { get; set; }

		[Required]
		[MaxLength(68)]
		public string Password { get; set; }

		[Required]
		[MaxLength(20)]
		public string Tier { get; set; }

		[MaxLength(30)]
		public string CityName { get; set; }

		[MaxLength(50)]
		public string WebSite { get; set; }

		[MaxLength(800)]
		public string Bio { get; set; }

		[Required]
		public DateTime CreateAt { get; set; }

		public DateTime? LastLogin { get; set; }

		public Guid? RePassKey { get; set; }
		
		public Guid? MailCheckKey { get; set; }

		[Required]
		[MaxLength(40)]
		public string Slug { get; set; }

		[Required]
		public byte Status { get; set; }

		public virtual Role Role { get; set; }
	}
}
