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
		public static log4net.ILog Loginfo = null;
		static Log()
		{
			SetConfig();
			Loginfo = log4net.LogManager.GetLogger("loginfo");
		}

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
			if (Loginfo.IsInfoEnabled)
			{
				Loginfo.Info(message);
			}
		}

		public static void Info(string message, Exception ex)
		{
			if (Loginfo.IsInfoEnabled)
			{
				Loginfo.Info(message, ex);
			}
		}

		public static void Error(string message)
		{
			if (Loginfo.IsErrorEnabled)
			{
				Loginfo.Error(message);
			}
		}

		public static void Error(string message, Exception ex)
		{
			if (Loginfo.IsErrorEnabled)
			{
				Loginfo.Error(message, ex);
			}
		}

		public static void Warn(string message)
		{
			if (Loginfo.IsWarnEnabled)
			{
				Loginfo.Warn(message);
			}
		}

		public static void Warn(string message, Exception ex)
		{
			if (Loginfo.IsWarnEnabled)
			{
				Loginfo.Warn(message, ex);
			}
		}

		public static void Fatal(string message)
		{
			if (Loginfo.IsFatalEnabled)
			{
				Loginfo.Fatal(message);
			}
		}

		public static void Fatal(string message, Exception ex)
		{
			if (Loginfo.IsFatalEnabled)
			{
				Loginfo.Fatal(message, ex);
			}
		}
	}
}