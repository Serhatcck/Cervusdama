using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.ProfileModel
{
	public class ContactInfoModel
	{
		[DisplayName("Email adresi")]
		[DataType(DataType.EmailAddress)]
		[Required(ErrorMessage = "Eamil alanı zorunludur.")]
		[MaxLength(100, ErrorMessage = "Email alanı en fazla 100 karakter olabilir.")]
		[RegularExpression(@"^([a-z0-9-_\.]+)@([a-z0-9]+)\.([a-z\.]{2,8})$", ErrorMessage = "Geçerli bir email adresi giriniz.")]
		public string Email { get; set; }

		[DisplayName("Web sitesi")]
		[DataType(DataType.Text)]
		[MaxLength(50, ErrorMessage = "Web site adresi en fazla 50 karakter olabilir.")]
		[RegularExpression(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$", ErrorMessage = "Geçerli bir url adresi giriniz.")]
		public string WebSite { get; set; }

		[DisplayName("Nereden katılıyorsunuz?")]
		[DataType(DataType.Text)]
		[MaxLength(30, ErrorMessage = "Lokasyon alanı en fazla 30 karakter olabilir.")]
		[RegularExpression(@"^[A-Za-zöçşığüÖÇŞİĞÜ\s]+\/[A-Za-zöçşığüÖÇŞİĞÜ\s]+$", ErrorMessage = "Ülke ismi/Şehir ismi formatında doldurunuz.")]
		public string CityName { get; set; }
	}
}
