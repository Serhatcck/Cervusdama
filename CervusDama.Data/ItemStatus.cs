using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data
{
	public enum ArticleStatus : byte
	{
		[Display(Name = "Silinmiş")]
		Deleted = 0,
		[Display(Name = "Yayında")]
		Active = 1,
		[Display(Name = "Onaylanmadı")]
		NotApproved = 2,
		[Display(Name = "Düzeltme")]
		Correction = 3,
		[Display(Name = "Engelli")]
		Blockked = 4
	}

	public enum UserStatus : byte
	{
		[Display(Name = "Engelli")]
		Banned = 0,
		[Display(Name = "Aktif")]
		Active = 1,
		[Display(Name ="Email Onaylanmamış")]
		NotApproved = 2
	}

	public enum QuestionStatus : byte
	{
		[Display(Name = "Silinmiş")]
		Deleted = 0,
		[Display(Name = "Yayında")]
		Active = 1,
		[Display(Name = "Çözüldü")]
		Solved = 2,
		[Display(Name = "Engelli")]
		Blockked = 3
	}
}
