using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.UserModel
{
	public class LoginModel
	{
		[DataType(DataType.EmailAddress)]
		[Required(ErrorMessage = "Email alanı zorunludur.")]
		[RegularExpression(@"^([a-z0-9-_\.]+)@([a-z0-9]+)\.([a-z\.]{2,8})$", ErrorMessage = "Geçerli bir email adresi giriniz.")]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre alanı zorunludur.")]
		public string Password { get; set; }
	}
}
