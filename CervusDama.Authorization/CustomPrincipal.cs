using CervusDama.Data;
using CervusDama.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Authorization
{
	public class CustomPrincipal : IPrincipal
	{
		public CustomPrincipal(IIdentity identity)
		{
			_identity = identity;
		}

		private IIdentity _identity;

		public IIdentity Identity
		{
			get
			{
				return _identity;
			}
		}

		private User _user;

		public User User
		{
			get
			{
				if (_user == null)
				{
					using (var db = new CervusDamaContext())
					{
						if (db.User.Any(p => p.Email == Identity.Name))
							_user = db.User.Single(a => a.Email == Identity.Name);
					}
				}

				return _user;
			}
		}


		bool IPrincipal.IsInRole(string role)
		{
			using (var db = new CervusDamaContext())
			{
				int uID = int.Parse(Identity.Name);

				string Role = db.User.Single(u => u.ID == uID).Role.Name;

				if (role.Contains(Role))
					return true;
				else
					return false;
			}
		}
	}
}
