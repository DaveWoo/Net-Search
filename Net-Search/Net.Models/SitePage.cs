using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using CsQuery;
using iBoxDB.LocalServer;

namespace Net.Models
{
	[DataContract]
	public class SitePage : IDInterface
	{
		[DataMember]
		public const int MAX_URL_LENGTH = 100;

		[DataMember]
		public long Id { get; set; }

		[DataMember]
		public String Url { get; set; }

		[DataMember]
		public String Title { get; set; }

		[DataMember]
		public String Description { get; set; }

		[DataMember]
		public String VerifiedSiteName { get; set; }

		[DataMember]
		public String Rank { get; set; }

		[DataMember]
		public String Content { get; set; }

		[DataMember]
		public DateTime CreatedTimeStamp { get; set; }

		[DataMember]
		public DateTime ModifiedTimeStamp { get; set; }

		[DataMember]
		public bool Enabled { get; set; }

		public string Host
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Url))
					return string.Empty;
				else
				{
					try
					{
						return new Uri(Url).Host;
					}
					catch (Exception ex)
					{
						return string.Empty;
					}
				}
			}
		}

		public object Tag { get; set; }

		public string Verified
		{
			get
			{
				string content = string.Empty;
				if (!string.IsNullOrWhiteSpace(VerifiedSiteName))
				{
					content += string.Format("<a href='#' id=\"{0}\" onmouseover=\"show(this)\" onmouseout=\"hide()\"class=\"mingpian\" target=\"_blank\"><span class=\"tip-v\"  ></span><span>{1}</span></a>", Id, VerifiedSiteName);
				}
				return content;
			}
		}

		[NotColumn]
		public long RankUpId()
		{
			return Id | (1L << 60);
		}

		[NotColumn]
		public static long RankDownId(long id)
		{
			return id & (~(1L << 60));
		}

		[NotColumn]
		public String RankUpDescription()
		{
			return Description + " " + Title;
		}

		private static readonly Random cran = new Random();

		[NotColumn]
		public String GetRandomContent()
		{
			int len = Content.ToString().Length - 100;
			if (len <= 20)
			{
				return Content.ToString();
			}
			int s = cran.Next(len);
			if (s < 0)
			{
				s = 0;
			}
			if (s > len)
			{
				s = len;
			}

			int count = Content.ToString().Length - s;
			if (count > 200)
			{
				count = 200;
			}
			return Content.ToString().Substring(s, count);
		}

		[NotColumn]
		public static SitePage Get(String url)
		{
			try
			{
				if (url == null || url.Length > MAX_URL_LENGTH || url.Length < 8)
				{
					return null;
				}
				SitePage page = new SitePage();
				page.Url = url;

				CQ doc = CQ.CreateFromUrl(url);
				//Console.WriteLine(doc.Html());
				doc["script"].Remove();
				doc["Script"].Remove();

				doc["style"].Remove();
				doc["Style"].Remove();

				doc["textarea"].Remove();
				doc["Textarea"].Remove();

				doc["noscript"].Remove();
				doc["Noscript"].Remove();

				page.Title = doc["title"].Text();
				if (page.Title == null)
				{
					page.Title = doc["Title"].Text();
				}
				if (page.Title == null)
				{
					page.Title = url;
				}
				page.Title = page.Title.Trim();
				if (page.Title.Length < 2)
				{
					page.Title = url;
				}
				if (page.Title.Length > 80)
				{
					page.Title = page.Title.Substring(0, 80);
				}
				page.Title = page.Title.Replace("<", " ")
					.Replace(">", " ").Replace("$", " ");
				doc["title"].Remove();
				doc["Title"].Remove();

				page.Description = doc["meta[name='description']"].Attr("content");
				if (page.Description == null)
				{
					page.Description = doc["meta[name='Description']"].Attr("content");
				}
				if (page.Description == null)
				{
					page.Description = "";
				}
				if (page.Description.Length > 200)
				{
					page.Description = page.Description.Substring(0, 200);
				}
				page.Description = page.Description.Replace("<", " ")
					.Replace(">", " ").Replace("$", " ");

				String content = doc.Text().Replace("　", " ");
				content = Regex.Replace(content, "\t|\r|\n|�|<|>", " ");
				content = Regex.Replace(content, "\\$", " ");
				content = Regex.Replace(content, "\\s+", " ");
				content = content.Trim();

				if (content.Length < 50)
				{
					return null;
				}
				if (content.Length > 5000)
				{
					content = content.Substring(0, 5000);
				}

				page.Content = content + " " + page.Url;
				page.CreatedTimeStamp = System.DateTime.Now;
				page.ModifiedTimeStamp = System.DateTime.Now;
				return page;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}
		}

		[NotColumn]
		public KeyWord keyWord;
	}
}