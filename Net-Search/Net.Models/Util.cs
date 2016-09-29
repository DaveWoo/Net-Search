using System.Collections.Generic;

namespace Net.Models
{
	public class Util
	{
		private readonly StringUtil sUtil = new StringUtil();

		public List<KeyWord> fromString(long id, char[] str, bool includeOF)
		{
			List<KeyWord> kws = new List<KeyWord>();

			KeyWordE k = null;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c == ' ')
				{
					if (k != null)
					{
						kws.Add(k);
						if (includeOF)
						{
							k = k.getOriginalForm();
							if (k != null)
							{
								kws.Add(k);
							}
						}
					}
					k = null;
				}
				else if (sUtil.isWord(c))
				{
					if (k == null && c != '-' && c != '#')
					{
						k = new KeyWordE();
						k.ID = id;
						k.KWord = "";
						k.Position = i;
					}
					if (k != null)
					{
						k.KWord = k.KWord.ToString() + c;
					}
				}
				else
				{
					if (k != null)
					{
						kws.Add(k);
						if (includeOF)
						{
							k = k.getOriginalForm();
							if (k != null)
							{
								kws.Add(k);
							}
						}
					}
					k = null;
					KeyWordN n = new KeyWordN();
					n.ID = id;
					n.Position = i;
					n.longKeyWord(c, (char)0, (char)0);
					kws.Add(n);

					char c1 = str[i + 1];
					if ((c1 != ' ') && (!sUtil.isWord(c1)))
					{
						n = new KeyWordN();
						n.ID = id;
						n.Position = i;
						n.longKeyWord(c, c1, (char)0);
						kws.Add(n);
						if (!includeOF)
						{
							kws.RemoveAt(kws.Count - 2);
							i++;
						}
					}
				}
			}
			return kws;
		}
	}
}