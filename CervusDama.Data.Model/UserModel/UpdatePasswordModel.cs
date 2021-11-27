using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.UserModel
{
	public class UpdatePasswordModel
	{
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Eski şifre alanı zorunludur.")]
		public string OldPassword { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre alanı zorunludur.")]
		[MaxLength(30, ErrorMessage = "Şifreniz en fazla 30 karakter olabilir.")]
		[MinLength(8, ErrorMessage = "Şifreniz en az 8 karakter olmalı.")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre tekrar alanı zorunludur.")]
		[Compare("Password", ErrorMessage = "Şifreniz tekrarı ile uyuşmuyor.")]
		public string RePassword { get; set; }


	}
}
