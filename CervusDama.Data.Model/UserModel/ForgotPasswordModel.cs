using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.UserModel
{
	public class ForgotPasswordModel
	{
		[DataType(DataType.EmailAddress)]
		[Required(ErrorMessage = "Email alanı zorunludur.")]
		[MaxLength(120, ErrorMessage = "İsim alanı en fazla 120 karakter olabilir.")]
		[RegularExpression(@"^([a-z0-9-_\.]+)@([a-z0-9]+)\.([a-z\.]{2,8})$", ErrorMessage = "Geçerli bir email adresi giriniz.")]
		public string Email { get; set; }
	}
}
