using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Net.HostServer.Startup))]
namespace Net.HostServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
