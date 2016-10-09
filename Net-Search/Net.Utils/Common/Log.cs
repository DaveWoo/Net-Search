using System;
using System.IO;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Net.Utils.Common
{
	/// <summary>
	/// LogHelper的摘要说明。
	/// </summary>
	public class Log
	{
		static Log()
		{
			SetConfig();
		}

		public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

		public static void SetConfig()
		{
			string path = AppDomain.CurrentDomain.BaseDirectory + @"\log4net_config.xml";
			log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
		}

		public static void SetConfig(FileInfo configFile)
		{
			log4net.Config.DOMConfigurator.Configure(configFile);
		}

        public static void Info(string message)
		{
			if (loginfo.IsInfoEnabled)
			{
                loginfo.Info(message);
			}
		}

        public static void Info(string message, Exception ex)
		{
			if (loginfo.IsInfoEnabled)
			{
                loginfo.Info(message, ex);
			}
		}

        public static void Error(string message)
		{
			if (loginfo.IsErrorEnabled)
			{
                loginfo.Error(message);
			}
		}

		public static void Error(string message, Exception ex)
		{
			if (loginfo.IsErrorEnabled)
			{
				loginfo.Error(message, ex);
			}
		}

		public static void Warn(string message)
		{
			if (loginfo.IsWarnEnabled)
			{
				loginfo.Warn(message);
			}
		}

		public static void Warn(string message, Exception ex)
		{
			if (loginfo.IsWarnEnabled)
			{
				loginfo.Warn(message, ex);
			}
		}

		public static void Fatal(string message)
		{
			if (loginfo.IsFatalEnabled)
			{
				loginfo.Fatal(message);
			}
		}

		public static void Fatal(string message, Exception ex)
		{
			if (loginfo.IsFatalEnabled)
			{
				loginfo.Fatal(message, ex);
			}
		}
	}
}