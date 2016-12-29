namespace Net.Utils.Logging
{
#pragma warning disable 1591

	public class CallContextPredefinedProperties
	{
		public const string NoLogPrefix = "nolog_";
		public const string CallContextProperties = "NetProps";

		public const string ActivityId = "ActivityId";
		public const string SessionId = "SessionId";
		public const string UserId = "UserId";
		public const string UserName = "UserName";
		public const string UserSessionToken = NoLogPrefix + "UserSessionToken";
	}

#pragma warning restore 1591
}