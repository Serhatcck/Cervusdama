using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Utility.PagerHelper
{
	[Serializable]
	public class Pager<T> : BasePager<T>
	{
		public Pager(IQueryable<T> baseList, int page, int perData)
		{
			int totalItemCount = baseList == null ? 0 : baseList.Count();
			int pageCount = totalItemCount == 0 ? 0 : (int)Math.Ceiling(totalItemCount / (double)perData);

			if (baseList != null && totalItemCount > 0)
			{
				Subset.AddRange(page == 1 ? baseList.Skip(0).Take(perData).ToList() : baseList.Skip((page - 1) * perData).Take(perData).ToList());
			}
		}

		public Pager(IEnumerable<T> baseList, int page, int perData)
			: this(baseList.AsQueryable<T>(), page, perData)
		{
		}
	}
}
