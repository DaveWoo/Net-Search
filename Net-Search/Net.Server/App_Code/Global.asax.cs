namespace Net.Server
{
	using System;
	using System.Net;
	using System.Reflection;
	using CsQuery.Web;
	using Net.Utils.Common;

	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(Object sender, EventArgs e)
		{
			#region Search engine

			ServicePointManager.ServerCertificateValidationCallback
				+= (ssender, cert, chain, sslPolicyErrors) => true;
			ServerConfig.Default.TimeoutSeconds = 20.0;

			Log.Loginfo = log4net.LogManager.GetLogger(Assembly.GetAssembly(typeof(Global)), "Net.Server");

			#endregion Search engine
		}

		protected void Session_Start(Object sender, EventArgs e)
		{
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{
		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
		}

		protected void Application_Error(Object sender, EventArgs e)
		{
		}

		protected void Session_End(Object sender, EventArgs e)
		{
		}

		protected void Application_End(Object sender, EventArgs e)
		{
		}
	}
}