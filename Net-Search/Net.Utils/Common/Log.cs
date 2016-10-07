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

		public static void Info(string info, Exception ex = null)
		{
			if (loginfo.IsInfoEnabled)
			{
				loginfo.Info(info, ex);
			}
		}

		public static void Error(string info, Exception ex = null)
		{
			if (loginfo.IsErrorEnabled)
			{
				loginfo.Error(info, ex);
			}
		}

		public static void Warn(string info, Exception ex = null)
		{
			if (loginfo.IsWarnEnabled)
			{
				loginfo.Warn(info, ex);
			}
		}

		public static void Fatal(string info, Exception ex = null)
		{
			if (loginfo.IsFatalEnabled)
			{
				loginfo.Fatal(info, ex);
			}
		}
	}
}