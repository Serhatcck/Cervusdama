using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CervusDama.Utility.SecurityHelper
{
	public class SecurityHelper
	{
		public string XSSFilter(string text)
		{
			text = Regex.Replace(text, ">[\t\n\r]+", ">", RegexOptions.Multiline);
			text = Regex.Replace(text, "[\t\n\r]+<", "<", RegexOptions.Multiline);

			return _xssFilterParser(text);
		}

		private string[] acceptedTag = { "h2", "p", "ol", "ul", "li", "b", "u", "i", "pre", "code", "a", "img", "#text" };

		private string _xssFilterParser(string text)
		{
			HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
			doc.LoadHtml(text);

			StringBuilder clearHtml = new StringBuilder();

			foreach (var item in doc.DocumentNode.ChildNodes)
			{
				if (acceptedTag.Any(a => a.Equals(item.Name)))
				{
					StringBuilder subTag = new StringBuilder();

					if (item.Name == "pre")
					{
						if (item.HasAttributes)
						{
							if (item.Attributes.Count() == 1)
							{
								var attribute = item.Attributes.FirstOrDefault();

								if (attribute.Name == "class")
								{
									if (Regex.IsMatch(attribute.Value, "language-[a-z]{1,10}"))
									{
										subTag.Append("<").Append(item.Name).Append(" class=\"").Append(attribute.Value).Append("\">");
									}
									else
									{
										//kullanıcı attribute değerine müdehale etmiş, ekletme.
										throw new SecurityHelperError("Müdehale edilmiş attribute.");
									}
								}
								else
								{
									//kullanıcı classtan başka attribute eklemiş, xss olabilir.
									throw new SecurityHelperError("Bilinmeyen attribute.");
								}
							}
							else
							{
								//birden fazla attribute var, kullanıcı müdehale etmiş, izin verme.
								throw new SecurityHelperError("Beklenenden fazla attribute.");
							}
						}
						else
						{
							//Hata döndür kod için tür belirtilmemiş
							throw new SecurityHelperError("Kod için tür belirtilmeli.");
						}
					}
					else if (item.Name == "img")
					{
						if (item.HasAttributes)
						{
							if (item.Attributes.Count() == 1)
							{
								var attribute = item.Attributes.FirstOrDefault();

								if (attribute.Name == "src")
								{
									if (Regex.IsMatch(attribute.Value, @"\/uploads\/medium\/[a-z0-9\-]+\.(jpg|jpeg|png|gif|tiff)"))
									{
										subTag.Append("<").Append(item.Name).Append(" src=\"").Append(attribute.Value).Append("\" alt=\"Makale Görseli\"/>");
									}
									else
									{
										//girilen resim adı uygun değil.
										throw new SecurityHelperError("Girilen resim adı uygun değil.");
									}
								}
								else
								{
									//kabul edilenlerden başka attribute eklenmiş, müdehale var.
									throw new SecurityHelperError("Bilinmeyen attribute.");
								}
							}
							else
							{
								//img etiketi için eksik ya da fazla attribute var. müdehale edilmiş.
								throw new SecurityHelperError("Beklenen sayıda attribute yok.");
							}
						}
						else
						{
							//img etiketi için src yok, hata.
							throw new SecurityHelperError("Resim için SRC Attribute'u olmalı.");
						}
					}
					else
					{
						subTag.Append("<").Append(item.Name).Append(">");
					}

					if (item.HasChildNodes)
					{
						subTag.Append(rcNode(item.ChildNodes).ToString());
					}
					else
					{
						subTag.Append(item.InnerText);
					}

					if (item.Name != "img")
					{
						subTag.Append("</").Append(item.Name).Append(">");
					}

					clearHtml.Append(subTag);
				}
				else
				{
					//Kabul edilmeyen html etiketi var. müdehale edilmiş ekletme.
					throw new SecurityHelperError("Bilinmeyen html etiketi.");
				}
			}

			return clearHtml.ToString();
		}

		private StringBuilder rcNode(HtmlAgilityPack.HtmlNodeCollection parent)
		{
			StringBuilder sb = new StringBuilder();

			foreach (var item in parent)
			{
				if (item.Name != "#text" && !acceptedTag.Any(a => a.Equals(item.Name)))
				{
					throw new SecurityHelperError("Bilinmeyen html etiketi.");
				}

				if (item.HasChildNodes)
				{
					if (item.Name == "a")
					{
						if (item.HasAttributes)
						{
							if (item.Attributes.Count() == 1)
							{
								if (item.Attributes.Any(a => a.Name.Equals("href")))
								{
									var attribute = item.Attributes.FirstOrDefault();

									if (Regex.IsMatch(attribute.Value, @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)"))
									{
										sb.Append(" <a href=\"").Append(attribute.Value).Append("\">").Append(rcNode(item.ChildNodes).ToString());
									}
									else
									{
										throw new SecurityHelperError("Link için geçersiz url değeri.");
									}
								}
								else
								{
									throw new SecurityHelperError("Link için bağlatı adresi belirtilmemiş.");
								}
							}
							else
							{
								throw new SecurityHelperError("Bilinmeyen attribute değeri.");
							}
						}
						else
						{
							throw new SecurityHelperError("Link için bağlatı adresi belirtilmemiş.");
						}
					}
					else
					{
						sb.Append(" <").Append(item.Name).Append(">").Append(rcNode(item.ChildNodes).ToString());
					}

					sb.Append("</").Append(item.Name).Append("> ");
				}
				else
				{
					if (item.Name == "#text")
					{
						sb.Append(item.InnerText.Replace("<", "&lt;").Replace(">", "&gt;"));
					}
				}
			}

			return sb;
		}
	}
}
