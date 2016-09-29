using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Models
{
	public class StringUtil
	{
		public static Dictionary<String, String> correctKW = new Dictionary<String, String>() {
			{"databae", "database"},
			{"beby", "baby"},
			{"androd", "android"},
			{"canguan", "餐馆"},
			{"meishi", "美食"}
		};

		public static Dictionary<String, String> antetypes = new Dictionary<String, String>() {
			{"dogs", "dog"},
			{"houses", "house"},
			{"grams", "gram"},

			{"kisses", "kiss"},
			{"watches", "watch"},
			{"boxes", "box"},
			{"bushes", "bush"},

			{"tomatoes", "tomato"},
			{"potatoes", "potato"},

			{"babies", "baby"},
			{"universities", "university"},
			{"flies", "fly"},
			{"impurities", "impurity"}
		};

		private HashSet<char> set;
		public HashSet<String> mvends;

		public StringUtil()
		{
			String s = "!\"@$%&'()*+,./:;<=>?[\\]^_`{|}~\r\n"; //@-
			s += "， 　，《。》、？；：‘’“”【｛】｝——=+、｜·～！￥%……&*（）"; //@-#
			s += "｀～！＠￥％……—×（）——＋－＝【】｛｝：；’＇”＂，．／＜＞？’‘”“";//＃
			s += " � ★☆,。？,　！";
			s += "©»¥「」";

			set = new HashSet<char>();
			foreach (char c in s)
			{
				set.Add(c);
			}
			set.Add((char)0);

			String[] ms = new String[] {
				"are", "were", "have", "has", "had",
				"you", "she", "her", "him", "like", "will", "would", "should",
				"when", "than", "then", "that", "this", "there", "who", "those", "these",
				"with", "which", "where", "they", "them", "one",
				"does", "doesn", "did", "gave", "give",
				"something", "someone", "about", "come"
			};
			mvends = new HashSet<String>();
			foreach (String c in ms)
			{
				mvends.Add(c);
			}
		}

		//Chinese  [\u2E80-\u9fa5]
		//Japanese [\u0800-\u4e00]|
		//Korean   [\uAC00-\uD7A3] [\u3130-\u318F]
		public bool isWord(char c)
		{
			//English
			if (c >= 'a' && c <= 'z')
			{
				return true;
			}
			if (c >= '0' && c <= '9')
			{
				return true;
			}
			//Russian
			if (c >= 0x0400 && c <= 0x052f)
			{
				return true;
			}
			//Germen
			if (c >= 0xc0 && c <= 0xff)
			{
				return true;
			}
			return c == '-' || c == '#';
		}

		public char[] clear(String str)
		{
			char[] cs = (str + "   ").ToLower().ToCharArray();
			for (int i = 0; i < cs.Length; i++)
			{
				if (set.Contains(cs[i]))
				{
					cs[i] = ' ';
				}
			}
			return cs;
		}

		public String getDesc(String str, KeyWord kw, int length)
		{
			List<KeyWord> list = new List<KeyWord>();
			while (kw != null)
			{
				list.Add(kw);
				kw = kw.previous;
			}
			list.Sort(
				(o1, o2) =>
				{
					return o1.Position - o2.Position;
				}
			);
			KeyWord[] ps = list.ToArray();

			int start = -1;
			int end = -1;
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < ps.Length; i++)
			{
				int len = ps[i] is KeyWordE ? ps[i].KWord
					.ToString().Length : ((KeyWordN)ps[i]).size();
				if ((ps[i].Position + len) <= end)
				{
					continue;
				}
				start = ps[i].Position;
				end = ps[i].Position + length;
				if (end > str.Length)
				{
					end = str.Length;
				}
				sb.Append(str.Substring(start, end - start))
					.Append("...");
			}
			return sb.ToString();
		}

		public void correctInput(List<KeyWord> kws)
		{
			for (int i = 0; i < kws.Count; i++)
			{
				KeyWord kw = (KeyWord)kws[i];
				if (kw is KeyWordE)
				{
					String str = kw.KWord.ToString();
					if (correctKW.TryGetValue(str, out str))
					{
						if (isWord(str[0]))
						{
							kw.KWord = str;
						}
						else
						{
							KeyWordN kwn = new KeyWordN();
							kwn.I = kw.I;
							kwn.P = kw.P + 1;
							switch (str.Length)
							{
								case 1:
									kwn.longKeyWord(str[0], (char)0, (char)0);
									break;
								case 2:
									kwn.longKeyWord(str[0], str[1], (char)0);
									break;
								default:
									continue;
							}
							kws[i] = kwn;
						}
					}
				}
			}
		}
	}
}