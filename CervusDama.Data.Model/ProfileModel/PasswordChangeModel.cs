using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.ProfileModel
{
	public class PasswordChangeModel
	{
		[DisplayName("Geçerli şifreniz")]
		[Required(ErrorMessage = "Eski şifre alanı zorunludur.")]
		[MaxLength(30, ErrorMessage = "Şifreniz en fazla 30 karakter olabilir.")]
		public string OldPassword { get; set; }

		[DisplayName("Yeni şifreniz")]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Yeni şifre alanı zorunludur.")]
		[MaxLength(30, ErrorMessage = "Şifreniz en fazla 30 karakter olabilir.")]
		[MinLength(8, ErrorMessage = "Şifreniz en az 8 karakter olmalı.")]
		public string Password { get; set; }

		[DisplayName("Yeni şifreniz (tekrar)")]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre tekrar alanı zorunludur.")]
		[Compare("Password", ErrorMessage = "Yeni şifreniz tekrarı ile uyuşmuyor.")]
		public string RePassword { get; set; }
	}
}
