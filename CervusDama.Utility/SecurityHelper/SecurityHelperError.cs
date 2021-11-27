using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Utility.SecurityHelper
{
	[Serializable]
	public class SecurityHelperError : Exception
	{
		public SecurityHelperError() : base() { }

		public SecurityHelperError(string message) : base(message) { }
	}
}
