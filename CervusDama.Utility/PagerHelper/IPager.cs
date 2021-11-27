using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Utility.PagerHelper
{
	public interface IPager<out T> : IEnumerable<T>
	{
	}
}
