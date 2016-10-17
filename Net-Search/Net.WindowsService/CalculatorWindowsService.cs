using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceProcess;
using Net.Api;
using Net.Utils;

namespace Net.Services
{
	public class CalculatorWindowsService : ServiceBase
	{
		private System.Diagnostics.EventLog eventLog1;
		public ServiceHost serviceHost = null;

		public enum ServiceState
		{
			SERVICE_STOPPED = 0x00000001,
			SERVICE_START_PENDING = 0x00000002,
			SERVICE_STOP_PENDING = 0x00000003,
			SERVICE_RUNNING = 0x00000004,
			SERVICE_CONTINUE_PENDING = 0x00000005,
			SERVICE_PAUSE_PENDING = 0x00000006,
			SERVICE_PAUSED = 0x00000007,
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ServiceStatus
		{
			public long dwServiceType;
			public ServiceState dwCurrentState;
			public long dwControlsAccepted;
			public long dwWin32ExitCode;
			public long dwServiceSpecificExitCode;
			public long dwCheckPoint;
			public long dwWaitHint;
		};

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

		public CalculatorWindowsService(string[] args)
		{
			InitializeComponent();
			var source = Constants.SERVICE_NAME + "Source";
			var log = Constants.SERVICE_NAME + "Log";
			if (!System.Diagnostics.EventLog.SourceExists(source))
			{
				System.Diagnostics.EventLog.CreateEventSource(
					source, log);
			}
			eventLog1.Source = source;
			eventLog1.Log = log;
			eventLog1.WriteEntry("InitializeComponent");

			// Name the Windows Service
			ServiceName = Constants.SERVICE_NAME;
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		private static void Main(string[] args)
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

			// Set up a timer to trigger every minute.
			System.Timers.Timer timer = new System.Timers.Timer();
			timer.Interval = 60000; // 60 seconds
			timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
			timer.Start();
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

		private void UpdateServiceStatus()
		{
			// Update the service state to Start Pending.
			ServiceStatus serviceStatus = new ServiceStatus();
			serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
			serviceStatus.dwWaitHint = 100000;
			SetServiceStatus(this.ServiceHandle, ref serviceStatus);

			// Update the service state to Running.
			serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
			SetServiceStatus(this.ServiceHandle, ref serviceStatus);
		}
	}
}