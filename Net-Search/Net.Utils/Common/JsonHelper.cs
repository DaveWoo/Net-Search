using System;
using System.Net.Json;

namespace Net.Utils
{
	public class JsonHelper
	{
		public static int GetIntegerValue(JsonObject field)
		{
			string value = GetStringValue(field);
			if (null == value)
			{
				return -1;
			}

			try
			{
				return Int32.Parse(value);
			}
			catch (OverflowException)
			{
				return -1;
			}
			catch (FormatException)
			{
				return -1;
			}
		}

		public static string GetStringValue(JsonObject field)
		{
			if (null == field || null == field.GetValue())
			{
				return null;
			}
			string value = null;
			string type = field.GetValue().GetType().Name;

			// try to get value.
			switch (type)
			{
				case "String":
					value = (string)field.GetValue();
					break;

				case "Double":
					value = field.GetValue().ToString();
					break;

				case "Boolean":
					value = field.GetValue().ToString();
					break;
			}

			return value;
		}

		        
	}
}