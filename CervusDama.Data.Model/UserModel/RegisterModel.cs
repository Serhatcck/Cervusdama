using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.UserModel
{
	public class RegisterModel
	{
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "İsim alanı zorunludur.")]
		[MaxLength(16, ErrorMessage = "İsim alanı en fazla 16 karakter olabilir.")]
		[RegularExpression(@"^([a-züğöçşı\s]+)$", ErrorMessage = "İsminiz sadece küçükharf ve boşluk içerebilir.")]
		public string FirstName { get; set; }

		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Soyisim alanı zorunludur.")]
		[MaxLength(16, ErrorMessage = "Soyisim alanı en fazla 16 karakter olabilir.")]
		[RegularExpression(@"^([a-züğöçşı]+)$", ErrorMessage = "Soyisminiz sadece küçük harflerden oluşabilir.")]
		public string LastName { get; set; }

		[DataType(DataType.EmailAddress)]
		[Required(ErrorMessage = "Email alanı zorunludur.")]
		[MaxLength(100, ErrorMessage = "Email alanı en fazla 120 karakter olabilir.")]
		[RegularExpression(@"^([a-z0-9-_\.]+)@([a-z0-9]+)\.([a-z\.]{2,8})$", ErrorMessage = "Geçerli bir email adresi giriniz.")]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre alanı zorunludur.")]
		[MaxLength(30, ErrorMessage = "Şifreniz en fazla 30 karakter olabilir.")]
		[MinLength(8, ErrorMessage = "Şifreniz en az 8 karakter olmalı.")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre tekrar alanı zorunludur.")]
		[Compare("Password", ErrorMessage = "Şifreniz tekrarı ile uyuşmuyor.")]
		public string RePassword { get; set; }

		[Required(ErrorMessage = "Kullanıcı sözleşmesini kabul ettiğinizi onaylamalısınız.")]
		public bool UserAgreementIsCheck { get; set; }
	}
}
