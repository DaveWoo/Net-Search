using System;
using System.Runtime.Serialization;
using iBoxDB.LocalServer;

namespace Net.Models
{
	[DataContract]
	public abstract class KeyWord
	{
		[DataMember]
		public readonly static int MAX_WORD_LENGTH = 16;

		public static void config(DatabaseConfig c)
		{
			// English Language or Word (max=16)
			c.EnsureTable<KeyWordE>("/E", "K(" + MAX_WORD_LENGTH + ")", "I", "P");

			// Non-English Language or Character
			c.EnsureTable<KeyWordN>("/N", "K", "I", "P");
		}

		[NotColumn]
		[DataMember]
		public abstract object KWord
		{
			get;
			set;
		}

		//Position
		[DataMember]
		public int P;

		[NotColumn]
		[DataMember]
		public int Position
		{
			get { return P; }
			set { P = value; }
		}

		//Document ID
		[DataMember]
		public long I;

		[NotColumn]
		[DataMember]
		public long ID
		{
			get { return I; }
			set { I = value; }
		}

		[NotColumn]
		[DataMember]
		public KeyWord previous;

		public String ToFullString()
		{
			return (previous != null ? previous.ToFullString() + " -> " : "") + ToString();
		}
	}
}