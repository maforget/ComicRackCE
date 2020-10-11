using System;
using System.Collections.Generic;

namespace cYo.Common.Reflection
{
	public static class DuckTyping
	{
		private class DuckTypeCache
		{
			private struct DictionaryEntry
			{
				public readonly Type InterfaceType;

				public readonly Type DuckedType;

				public DictionaryEntry(Type interfaceType, Type duckedType)
				{
					InterfaceType = interfaceType;
					DuckedType = duckedType;
				}

				public override bool Equals(object obj)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (InterfaceType == dictionaryEntry.InterfaceType)
					{
						return DuckedType == dictionaryEntry.DuckedType;
					}
					return false;
				}

				public override int GetHashCode()
				{
					return InterfaceType.GetHashCode() ^ DuckedType.GetHashCode();
				}
			}

			private readonly Dictionary<DictionaryEntry, Type> dict = new Dictionary<DictionaryEntry, Type>();

			private readonly object syncRoot = new object();

			public object SyncRoot => syncRoot;

			public bool Exists(Type interfaceType, Type duckedType)
			{
				return dict.ContainsKey(new DictionaryEntry(interfaceType, duckedType));
			}

			public Type Get(Type interfaceType, Type duckedType)
			{
				return dict[new DictionaryEntry(interfaceType, duckedType)];
			}

			public void Insert(Type interfaceType, Type duckedType, Type duckType)
			{
				dict[new DictionaryEntry(interfaceType, duckedType)] = duckType;
			}
		}

		private static readonly DuckTypeCache cache = new DuckTypeCache();

		private static readonly IDuckTypeGenerator generator = new CodeDomDuckTypeGenerator();

		public static T[] Implement<T>(params object[] objects)
		{
			if (objects == null)
			{
				throw new ArgumentNullException("objects");
			}
			Type typeFromHandle = typeof(T);
			ValidateInterfaceType(typeFromHandle);
			Type[] array = new Type[objects.Length];
			for (int i = 0; i < objects.Length; i++)
			{
				array[i] = objects[i].GetType();
			}
			Type[] duckTypes = GetDuckTypes(typeFromHandle, array);
			T[] array2 = new T[objects.Length];
			for (int j = 0; j < objects.Length; j++)
			{
				array2[j] = (T)Activator.CreateInstance(duckTypes[j], objects[j]);
			}
			return array2;
		}

		public static T[] Implement<T, K>(params K[] objects)
		{
			if (objects == null)
			{
				throw new ArgumentNullException("objects");
			}
			Type typeFromHandle = typeof(T);
			ValidateInterfaceType(typeFromHandle);
			Type typeFromHandle2 = typeof(K);
			Type duckType = GetDuckType(typeFromHandle, typeFromHandle2);
			T[] array = new T[objects.Length];
			for (int i = 0; i < objects.Length; i++)
			{
				array[i] = (T)Activator.CreateInstance(duckType, objects[i]);
			}
			return array;
		}

		public static T Implement<T, K>(K obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(K);
			ValidateInterfaceType(typeFromHandle);
			Type duckType = GetDuckType(typeFromHandle, typeFromHandle2);
			return (T)Activator.CreateInstance(duckType, obj);
		}

		public static T Implement<T>(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Type typeFromHandle = typeof(T);
			Type type = obj.GetType();
			ValidateInterfaceType(typeFromHandle);
			Type duckType = GetDuckType(typeFromHandle, type);
			return (T)Activator.CreateInstance(duckType, obj);
		}

		public static void PrepareDuckTypes<T>(params Type[] duckedTypes)
		{
			if (duckedTypes == null)
			{
				throw new ArgumentNullException("duckedTypes");
			}
			Type typeFromHandle = typeof(T);
			ValidateInterfaceType(typeFromHandle);
			GetDuckTypes(typeFromHandle, duckedTypes);
		}

		private static Type GetDuckType(Type interfaceType, Type duckedType)
		{
			lock (cache.SyncRoot)
			{
				if (cache.Exists(interfaceType, duckedType))
				{
					return cache.Get(interfaceType, duckedType);
				}
				Type type = CreateDuckTypes(interfaceType, new Type[1]
				{
					duckedType
				})[0];
				cache.Insert(interfaceType, duckedType, type);
				return type;
			}
		}

		private static Type[] GetDuckTypes(Type interfaceType, Type[] duckedTypes)
		{
			lock (cache.SyncRoot)
			{
				List<Type> list = new List<Type>();
				foreach (Type type in duckedTypes)
				{
					if (!cache.Exists(interfaceType, type) && !list.Contains(type))
					{
						list.Add(type);
					}
				}
				if (list.Count > 0)
				{
					Type[] array = CreateDuckTypes(interfaceType, list.ToArray());
					for (int j = 0; j < array.Length; j++)
					{
						cache.Insert(interfaceType, list[j], array[j]);
					}
				}
				Type[] array2 = new Type[duckedTypes.Length];
				for (int k = 0; k < duckedTypes.Length; k++)
				{
					array2[k] = GetDuckType(interfaceType, duckedTypes[k]);
				}
				return array2;
			}
		}

		private static Type[] CreateDuckTypes(Type interfaceType, Type[] duckedTypes)
		{
			return generator.CreateDuckTypes(interfaceType, duckedTypes);
		}

		private static void ValidateInterfaceType(Type interfaceType)
		{
			if (!interfaceType.IsInterface)
			{
				throw new Exception("T have to be an Interface - Type!");
			}
			if (!interfaceType.IsPublic)
			{
				throw new Exception("The Interface has to be public if you want to create a Duck - Type!");
			}
		}
	}
}
