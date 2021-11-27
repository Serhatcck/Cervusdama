using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CervusDama.Data.Model.DashBoardModel
{
	public class YearChartBigDataModel
	{
		public List<YearChartDataModel> ArticleData { get; set; }

		public List<YearChartDataModel> UserData { get; set; }

		public List<YearChartDataModel> QuestionData { get; set; }
	}
}
