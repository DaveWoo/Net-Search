using System;
using System.IO;

namespace Net.Utils
{
	public class TraceLog
	{
		public static string log_name = "NetSearch_log.txt";
		public static bool bWriteToFile = false;

		private static void writeLog(string str)
		{
			if (bWriteToFile)
			{
				StreamWriter sw = new StreamWriter(log_name, true);
				sw.Write(str);
				sw.Flush();
				sw.Close();
			}
		}

		public static void Print(string format, params object[] args)
		{
			string msg = GetMessage(format, args);
			DateTime dt = System.DateTime.Now.ToLocalTime();

			ConsoleColor color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Gray;
			if (_newLine)
			{
				string str = string.Format("[{0}]: ", dt);
				Console.Write(str);
				writeLog(str);
				_newLine = false;
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(msg);
			writeLog(msg);
			Console.ForegroundColor = color;
		}

		public static void PrintLn(string format, params object[] args)
		{
			Print(format, args);
			writeLog("\r\n");
			Console.WriteLine();
			_newLine = true;
		}

		public static void Error(string format, params object[] args)
		{
			string msg = GetMessage(format, args);
			DateTime dt = System.DateTime.Now.ToLocalTime();

			ConsoleColor color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			if (!_newLine)
			{
				writeLog("\r\n");
				Console.WriteLine();
			}
			string str = string.Format("[{0}]: {1}", dt, msg);
			writeLog(str);
			Console.WriteLine(str);
			Console.ForegroundColor = color;
			_newLine = true;
		}

		private static string GetMessage(string format, object[] args)
		{
			if (args.Length > 0)
			{
				return string.Format(format, args);
			}
			else
			{
				return format;
			}
		}

		private static bool _newLine = true;
	}
}