using System;
using iBoxDB.LocalServer;

namespace Net.Models
{
	public sealed class KeyWordN : KeyWord
	{
		//Key Word
		public long K;

		[NotColumn]
		public override object KWord
		{
			get { return K; }
			set { K = (long)value; }
		}

		public byte size()
		{
			if ((K & CMASK) != 0L)
			{
				return 3;
			}
			if ((K & (CMASK << 16)) != 0L)
			{
				return 2;
			}
			return 1;
		}

		private const long CMASK = 0xFFFF;

		private static String KtoString(long k)
		{
			char c0 = (char)((k & (CMASK << 32)) >> 32);
			char c1 = (char)((k & (CMASK << 16)) >> 16);
			char c2 = (char)(k & CMASK);

			if (c2 != 0)
			{
				return new String(new char[] { c0, c1, c2 });
			}
			if (c1 != 0)
			{
				return new String(new char[] { c0, c1 });
			}
			return c0.ToString();
		}

		public void longKeyWord(char c0, char c1, char c2)
		{
			long k = (0L | c0) << 32;
			if (c1 != 0)
			{
				k |= ((0L | c1) << 16);
				if (c2 != 0)
				{
					k |= (0L | c2);
				}
			}
			K = k;
		}

		public String toKString()
		{
			return KtoString(K);
		}

		public override String ToString()
		{
			return toKString() + " Pos=" + P + ", ID=" + I + " N";
		}
	}
}