using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.Threading;
using ICSharpCode.SharpZipLib.BZip2;

namespace cYo.Common.Xml
{
	public static class XmlUtility
	{
		private const int BufferSize = 131072;

		private static readonly SimpleCache<Type, XmlSerializer> cachedSerialzers = new SimpleCache<Type, XmlSerializer>();

		public static XmlSerializer GetSerializer(Type type)
		{
			using (ItemMonitor.Lock(cachedSerialzers))
			{
				return cachedSerialzers.Get(type, (Type k) => new XmlSerializer(type, GetExtraTypes(type)));
			}
		}

		public static XmlSerializer GetSerializer<T>()
		{
			return GetSerializer(typeof(T));
		}

		public static Type[] GetExtraTypes(Type type)
		{
			Type[] array = null;
			try
			{
				MethodInfo method = type.GetMethod("GetExtraXmlSerializationTypes", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
				if (method != null)
				{
					array = method.Invoke(null, new object[0]) as Type[];
				}
			}
			catch
			{
			}
			return array ?? new Type[0];
		}

		public static byte[] Store(object data, bool compressed)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.WriteByte((byte)(compressed ? 1 : 0));
				Store(memoryStream, data, compressed);
				return memoryStream.ToArray();
			}
		}

		public static string ToString(object data)
		{
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add("", "");
			StringBuilder stringBuilder = new StringBuilder();
			using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
			{
				OmitXmlDeclaration = true
			}))
			{
				GetSerializer(data.GetType()).Serialize(xmlWriter, data, xmlSerializerNamespaces);
			}
			return stringBuilder.ToString();
		}

		public static T FromString<T>(string text)
		{
			using (StringReader textReader = new StringReader(text))
			{
				return (T)GetSerializer(typeof(T)).Deserialize(textReader);
			}
		}

		public static object Load(Type dataType, byte[] bytes)
		{
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				bool compressed = memoryStream.ReadByte() != 0;
				return Load(memoryStream, dataType, compressed);
			}
		}

		public static object Load(Type dataType, string text)
		{
			using (StringReader textReader = new StringReader(text))
			{
				return GetSerializer(dataType).Deserialize(textReader);
			}
		}

		public static T Load<T>(byte[] bytes)
		{
			return (T)Load(typeof(T), bytes);
		}

		public static void Store(Stream s, object data, bool compressed)
		{
			using (BZip2OutputStream bZip2OutputStream = (compressed ? new BZip2OutputStream(s) : null))
			{
				XmlSerializer serializer = GetSerializer(data.GetType());
				serializer.Serialize(compressed ? bZip2OutputStream : s, data);
			}
		}

		public static object Load(Stream s, Type dataType, bool compressed)
		{
			using (BZip2InputStream bZip2InputStream = (compressed ? new BZip2InputStream(s) : null))
			{
				XmlSerializer serializer = GetSerializer(dataType);
				object obj = serializer.Deserialize(compressed ? bZip2InputStream : s);
				if (obj is IXmlDeserialized)
				{
					((IXmlDeserialized)obj).SerializationCompleted();
				}
				return obj;
			}
		}

		public static T Load<T>(Stream s, bool compressed)
		{
			return (T)Load(s, typeof(T), compressed);
		}

		public static T Load<T>(string file)
		{
			return Load<T>(file, compressed: false);
		}

		public static void Store(string newFile, object data, bool compressed)
		{
			using (Stream s = File.Create(newFile))
			{
				Store(s, data, compressed);
			}
		}

		public static void Store(string newFile, object data)
		{
			Store(newFile, data, compressed: false);
		}

		public static object Load(string file, Type dataType, bool compressed)
		{
			using (Stream s = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
			{
				return Load(s, dataType, compressed);
			}
		}

		public static T Load<T>(string file, bool compressed)
		{
			return (T)Load(file, typeof(T), compressed);
		}
	}
}
