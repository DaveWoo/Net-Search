using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Net.Api;

namespace Net.Services
{
	public class CalculatorWindowsService : ServiceBase
	{
		private System.Diagnostics.EventLog eventLog1;
		public ServiceHost serviceHost = null;
		public CalculatorWindowsService(string[] args)
		{
			InitializeComponent();
			if (!System.Diagnostics.EventLog.SourceExists("MySource2"))
			{
				System.Diagnostics.EventLog.CreateEventSource(
					"MySource2", "MyNewLog2");
			}
			eventLog1.Source = "MySource2";
			eventLog1.Log = "MyNewLog2";
			eventLog1.WriteEntry("InitializeComponent");

			// Name the Windows Service
			ServiceName = Constants.SERVICE_NAME;
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] { new CalculatorWindowsService(args) };
			ServiceBase.Run(ServicesToRun);
		}
		// Start the Windows service.
		protected override void OnStart(string[] args)
		{
			eventLog1.WriteEntry("In OnStart start");


			if (serviceHost != null)
			{
				serviceHost.Close();
			}

			// Create a ServiceHost for the CalculatorService type and 
			// provide the base address.
			serviceHost = new ServiceHost(typeof(Manager));

			// Open the ServiceHostBase to create listeners and start 
			// listening for messages.
			serviceHost.Open();
			eventLog1.WriteEntry("In OnStart end");

		}

		protected override void OnStop()
		{
			eventLog1.WriteEntry("In onStop start.");
			if (serviceHost != null)
			{
				serviceHost.Close();
				serviceHost = null;
			}
			eventLog1.WriteEntry("In onStop end.");
		}
		public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
		{
			// TODO: Insert monitoring activities here.
			eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, 1);
		}

		protected override void OnContinue()
		{
			eventLog1.WriteEntry("In OnContinue.");
		}

		private void InitializeComponent()
		{
			this.eventLog1 = new System.Diagnostics.EventLog();
			((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
		}
	}
}
