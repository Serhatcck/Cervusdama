using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Utility.PagerHelper
{
	public static class PagerHelper
	{
		public static IPager<T> ToPagedList<T>(this IEnumerable<T> baseList, int page, int perData)
		{
			return new Pager<T>(baseList, page, perData);
		}

		public static IPager<T> ToPagedList<T>(this IQueryable<T> baseList, int page, int perData)
		{
			return new Pager<T>(baseList, page, perData);
		}
	}
}
