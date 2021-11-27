using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CervusDama.Data.Model.ProfileModel
{
	public class ProfileDeleteModel
	{
		[Required(ErrorMessage = "Hesap silme onay kutusunu doldurmalısınız.")]
		public bool IsAccept { get; set; }
	}
}
