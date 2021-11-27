using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.ProfileModel
{
	public class ProfileInfoModel
	{
		[DisplayName("Adınız")]
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Ad alanı zorunludur.")]
		[MaxLength(20, ErrorMessage = "İsim alanı en fazla 20 karakter olabilir.")]
		[RegularExpression(@"^[A-Za-zöçşığüÖÇŞIĞÜ\s]+$", ErrorMessage = "İsim alanı sadece karakter ve boşluk içerebilir.")]
		public string FirstName { get; set; }

		[DisplayName("Soyadınız")]
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Soyad alanı zorunludur.")]
		[MaxLength(20, ErrorMessage = "Soyisim alanı en fazla 20 karakter olabilir.")]
		[RegularExpression(@"^[A-Za-zöçşığüÖÇŞIĞÜ\s]+$", ErrorMessage = "Soyisim alanı sadece karakter ve boşluk içerebilir.")]
		public string LastName { get; set; }

		[DisplayName("Kullanıcı Adı")]
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "Kullanıcı adı alanı zorunludur.")]
		[MaxLength(20, ErrorMessage = "Kullanıcı adı alanı en fazla 20 karakter olabilir.")]
		[MinLength(6, ErrorMessage = "Kullanıcı adı alanı en az 20 karakter olabilir.")]
		[RegularExpression(@"^[A-Za-zöçşığüÖÇŞIĞÜ\s]+$", ErrorMessage = "İsim alanı sadece karakter ve boşluk içerebilir.")]
		public string NickName { get; set; }

		[DisplayName("Kendinizden kısaca bahsedin..")]
		[DataType(DataType.MultilineText)]
		[MaxLength(800, ErrorMessage = "Biyografi alanı en fazla 800 karakter olabilir.")]
		public string Bio { get; set; }
	}
}
