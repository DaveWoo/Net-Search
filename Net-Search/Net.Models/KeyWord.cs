using System;
using iBoxDB.LocalServer;

namespace Net.Models
{
	public abstract class KeyWord
	{
		public readonly static int MAX_WORD_LENGTH = 16;

		public static void config(DatabaseConfig c)
		{
			// English Language or Word (max=16)
			c.EnsureTable<KeyWordE>("/E", "K(" + MAX_WORD_LENGTH + ")", "I", "P");

			// Non-English Language or Character
			c.EnsureTable<KeyWordN>("/N", "K", "I", "P");
		}

		[NotColumn]
		public abstract object KWord
		{
			get;
			set;
		}

		//Position
		public int P;

		[NotColumn]
		public int Position
		{
			get { return P; }
			set { P = value; }
		}

		//Document ID
		public long I;

		[NotColumn]
		public long ID
		{
			get { return I; }
			set { I = value; }
		}

		[NotColumn]
		public KeyWord previous;

		public String ToFullString()
		{
			return (previous != null ? previous.ToFullString() + " -> " : "") + ToString();
		}
	}
}