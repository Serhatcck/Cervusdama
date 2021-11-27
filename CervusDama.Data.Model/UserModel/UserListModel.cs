using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.UserModel
{
	public class UserListModel
	{
		public int ID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string NickName { get; set; }

		public string Email { get; set; }

		public string Role { get; set; }

		public DateTime CreateAt { get; set; }

		public DateTime? LastLogin { get; set; }

		public string Slug { get; set; }

		public byte Stauts { get; set; }
	}
}
