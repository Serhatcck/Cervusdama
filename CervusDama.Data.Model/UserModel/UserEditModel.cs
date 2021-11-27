using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.UserModel
{
	public class UserEditModel
	{
		[Required(ErrorMessage = "İsim alanı zorunludur.")]
		[MaxLength(ErrorMessage = "İsim alanı en fazla 16 karakter olabilir.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Soyisim alanı zorunludur.")]
		[MaxLength(ErrorMessage = "Soyisim alanı en fazla 16 karakter olabilir.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Email alanı zorunludur.")]
		[MaxLength(120, ErrorMessage = "Email alanı en fazla 120 karakter olabilir.")]
		[RegularExpression(@"^([a-z0-9-_\.]+)@([a-z0-9]+)\.([a-z\.]{2,8})$", ErrorMessage = "Geçerli bir email adresi giriniz.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "NickName alanı zorunludur.")]
		[MaxLength(ErrorMessage = "NickName alanı en fazla 16 karakter olabilir.")]
		public string NickName { get; set; }
	}
}
