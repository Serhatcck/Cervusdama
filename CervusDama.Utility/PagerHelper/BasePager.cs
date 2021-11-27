using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Utility.PagerHelper
{
	public abstract class BasePager<T> : IPager<T>
	{
		protected readonly List<T> Subset = new List<T>();

		public IEnumerator<T> GetEnumerator()
		{
			return Subset.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
