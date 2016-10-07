using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Net.Server.Startup))]
namespace Net.Server
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
		}
	}
}
