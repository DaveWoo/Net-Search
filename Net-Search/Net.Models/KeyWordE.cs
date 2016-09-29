using System;
using iBoxDB.LocalServer;

namespace Net.Models
{
	public sealed class KeyWordE : KeyWord
	{
		//Key Word
		public String K;

		[NotColumn]
		public override object KWord
		{
			get { return K; }
			set
			{
				var t = (String)value;
				if (t.Length > KeyWord.MAX_WORD_LENGTH)
				{
					return;
				}
				K = t;
			}
		}

		public KeyWordE getOriginalForm()
		{
			String of;
			if (StringUtil.antetypes.TryGetValue(K, out of))
			{
				KeyWordE e = new KeyWordE();
				e.I = this.I;
				e.P = this.P;
				e.K = of;
				return e;
			}
			return null;
		}

		public override String ToString()
		{
			return K + " Pos=" + P + ", ID=" + I + " E";
		}
	}
}