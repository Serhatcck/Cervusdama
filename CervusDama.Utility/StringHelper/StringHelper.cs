using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;

namespace CervusDama.Utility.StringHelper
{
	public class StringHelper
	{
		public static string Slug(string text)
		{
			text = Regex.Replace(HttpUtility.HtmlDecode(text.Replace("&nbsp;", string.Empty)), "<.*?>", string.Empty);

			text = text.ToLower().Trim()
				.Replace('ı', 'i')
				.Replace('ü', 'u')
				.Replace('ö', 'o')
				.Replace('ç', 'c')
				.Replace('ğ', 'g')
				.Replace('ş', 's');

			text = Regex.Replace(text, @"[^0-9a-z\s]+", string.Empty);
			text = Regex.Replace(text, @"\s+", "-");

			return text;
		}

		public static string StripTags(string text)
		{
			return Regex.Replace(HttpUtility.HtmlDecode(text.Replace("&nbsp;", string.Empty)), "<.*?>", string.Empty);
		}

		public static string GenerateFileName(string name, string path)
		{
			//string newFileName = Guid.NewGuid().ToString();

			int counter = 1;
			string newName = name, extension = "", tempName = "";

			string[] splitName = name.Split('.');

			extension = splitName.Last();

			tempName = Slug(String.Join("", splitName.Take(splitName.Length - 1)));

			newName = tempName;

			while (System.IO.File.Exists(path + newName + "." + extension))
			{
				newName = tempName + "-" + (counter++).ToString();
			}

			return newName + "." + extension;
		}
	}
}
