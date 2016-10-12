
namespace Net.Server
{
	using System;
	using Net.Api;
	using Net.Utils.Common;
	using System.Net;
	using CsQuery.Web;
	using System.Reflection;

	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(Object sender, EventArgs e)
		{
			#region Web mvc

			//AreaRegistration.RegisterAllAreas();
			//FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			//RouteConfig.RegisterRoutes(RouteTable.Routes);
			//BundleConfig.RegisterBundles(BundleTable.Bundles);

			#endregion Web mvc

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
			SDB.Close();
		}
	}
}