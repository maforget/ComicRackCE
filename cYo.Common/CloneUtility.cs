using System;
using System.IO;
using System.Runtime.Serialization;

namespace cYo.Common
{
	public static class CloneUtility
	{
		public static T Clone<T>(T data) where T : class
		{
			if (data == null)
			{
				return null;
			}

			DataContractSerializer serializer = new DataContractSerializer(typeof(T), new DataContractSerializerSettings()
			{
				PreserveObjectReferences = true,
			});
			using (MemoryStream memoryStream = new MemoryStream())
			{
				serializer.WriteObject(memoryStream, data);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				return(T)serializer.ReadObject(memoryStream);
			}
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			T val = a;
			a = b;
			b = val;
		}

		public static T Clone<T>(this ICloneable data) where T : class
		{
			if (data != null)
			{
				return data.Clone() as T;
			}
			return null;
		}
	}
}
