using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Net.Utils
{
	internal class SerializeHelper
	{
		public static void Serialize<T>(T instance)
		{
			Type type = typeof(T);
			using (FileStream fs = new FileStream(type.Name + ".txt", FileMode.OpenOrCreate))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(fs, instance);
			}
		}

		public static T Deserialize<T>(out bool bFromFile) where T : class, new()
		{
			bFromFile = false;
			Type type = typeof(T);
			if (File.Exists(type.Name + ".txt"))
			{
				try
				{
					using (FileStream fs = new FileStream(type.Name + ".txt", FileMode.Open))
					{
						BinaryFormatter formatter = new BinaryFormatter();
						T t = formatter.Deserialize(fs) as T;
						if (t != null)
						{
							bFromFile = true;
							return t;
						}
					}
				}
				catch
				{
					return new T();
				}
			}
			return new T();
		}
	}
}