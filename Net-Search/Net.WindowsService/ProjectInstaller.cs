using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using Net.Api;

namespace Net.Services
{
	// Provide the ProjectInstaller class which allows 
	// the service to be installed by the Installutil.exe tool
	[RunInstaller(true)]
	public class ProjectInstaller : Installer
	{
		private ServiceProcessInstaller process;
		private ServiceInstaller service;

		public ProjectInstaller()
		{
			process = new ServiceProcessInstaller();
			process.Account = ServiceAccount.LocalSystem;
			service = new ServiceInstaller();
			service.ServiceName = Constants.SERVICE_NAME;
			service.Description = Constants.SERVICE_DESCRIPTION;
			Installers.Add(process);
			Installers.Add(service);
		}
	}
}
