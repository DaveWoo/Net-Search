using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using Net.Utils;

namespace Net.Services
{
	// Provide the ProjectInstaller class which allows
	// the service to be installed by the Installutil.exe tool
	[RunInstaller(true)]
	public class ProjectInstaller : Installer
	{
		private ServiceProcessInstaller process;
		private ServiceInstaller service;
		private string source;
		private string log;

		public ProjectInstaller()
		{
			source = Constants.SERVICE_NAME + "Source";
			log = Constants.SERVICE_NAME + "Log";

			process = new ServiceProcessInstaller();
			process.Account = ServiceAccount.LocalSystem;
			service = new ServiceInstaller();
			service.ServiceName = Constants.SERVICE_NAME;
			service.Description = Constants.SERVICE_DESCRIPTION;
			service.StartType = ServiceStartMode.Automatic;
			Installers.Add(process);
			Installers.Add(service);
		}

		protected override void OnBeforeInstall(IDictionary savedState)
		{
			string parameter = source + "\" \"" + log;
			Context.Parameters["assemblypath"] = "\"" + Context.Parameters["assemblypath"] + "\" \"" + parameter + "\"";
			base.OnBeforeInstall(savedState);
		}
	}
}