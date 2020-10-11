using System;
using System.Reflection;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Threading;

namespace cYo.Common.Reflection
{
	public static class PropertyCaller
	{
		private struct Key
		{
			public readonly string Name;

			public readonly Type Class;

			public readonly Type Return;

			public Key(string name, Type classType, Type returnType)
			{
				Name = name;
				Class = classType;
				Return = returnType;
			}

			public override bool Equals(object obj)
			{
				Key key = (Key)obj;
				if (Name == key.Name && Class == key.Class)
				{
					return Return == key.Return;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return Name.GetHashCode() ^ Class.GetHashCode() ^ Return.GetHashCode();
			}
		}

		private static readonly SimpleCache<Key, Delegate> dynamicGets = new SimpleCache<Key, Delegate>();

		private static readonly SimpleCache<Key, Delegate> dynamicSets = new SimpleCache<Key, Delegate>();

		public static Func<T, K> CreateGetMethod<T, K>(PropertyInfo pi) where T : class
		{
			Key key = new Key(pi.Name, typeof(T), typeof(K));
			using (ItemMonitor.Lock(dynamicGets))
			{
				return (Func<T, K>)dynamicGets.Get(key, delegate
				{
					MethodInfo getMethod = pi.GetGetMethod();
					if (getMethod == null)
					{
						return null;
					}
					if (pi.PropertyType == typeof(int))
					{
						return CreatePropertyDelegate<T, K, int>(pi.PropertyType, getMethod);
					}
					if (pi.PropertyType == typeof(long))
					{
						return CreatePropertyDelegate<T, K, long>(pi.PropertyType, getMethod);
					}
					if (pi.PropertyType == typeof(float))
					{
						return CreatePropertyDelegate<T, K, float>(pi.PropertyType, getMethod);
					}
					if (pi.PropertyType == typeof(double))
					{
						return CreatePropertyDelegate<T, K, double>(pi.PropertyType, getMethod);
					}
					if (pi.PropertyType == typeof(string))
					{
						return CreatePropertyDelegate<T, K, string>(pi.PropertyType, getMethod);
					}
					if (pi.PropertyType == typeof(DateTime))
					{
						return CreatePropertyDelegate<T, K, DateTime>(pi.PropertyType, getMethod);
					}
					try
					{
						return Delegate.CreateDelegate(typeof(Func<T, K>), getMethod);
					}
					catch (Exception)
					{
						return null;
					}
				});
			}
		}

		private static Delegate CreatePropertyDelegate<T, K, J>(Type propertyType, MethodInfo getMethod)
		{
			Func<T, J> fd = (Func<T, J>)Delegate.CreateDelegate(typeof(Func<T, J>), getMethod);
			if (propertyType == typeof(K))
			{
				return fd;
			}
			return (Func<T, K>)((T v) => (K)Convert.ChangeType(fd(v), typeof(K)));
		}

		public static Func<T, K> CreateGetMethod<T, K>(string name) where T : class
		{
			return CreateGetMethod<T, K>(typeof(T).GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public));
		}

		public static Action<T, K> CreateSetMethod<T, K>(PropertyInfo pi) where T : class
		{
			Key key = new Key(pi.Name, typeof(T), typeof(K));
			using (ItemMonitor.Lock(dynamicSets))
			{
				return (Action<T, K>)dynamicSets.Get(key, delegate
				{
					MethodInfo setMethod = pi.GetSetMethod();
					return (!(setMethod == null)) ? Delegate.CreateDelegate(typeof(Action<T, K>), setMethod) : null;
				});
			}
		}

		public static Action<T, K> CreateSetMethod<T, K>(string name) where T : class
		{
			return CreateSetMethod<T, K>(typeof(T).GetProperty(name));
		}

		public static IValueStore<K> CreateValueStore<T, K>(T data, string name) where T : class
		{
			Action<T, K> setMethod = CreateSetMethod<T, K>(name);
			Func<T, K> getMethod = CreateGetMethod<T, K>(name);
			return new ValueStore<K>(delegate(K v)
			{
				setMethod(data, v);
			}, () => getMethod(data));
		}

		public static IValueStore<bool> CreateFlagsValueStore<T, K>(T data, string name, K mask) where T : class where K : struct
		{
			Action<T, K> setMethod = CreateSetMethod<T, K>(name);
			Func<T, K> getMethod = CreateGetMethod<T, K>(name);
			int i = Convert.ToInt32(mask);
			return new ValueStore<bool>(delegate(bool v)
			{
				setMethod(data, (K)Convert.ChangeType(Convert.ToInt32(getMethod(data)).SetMask(i, v), Enum.GetUnderlyingType(typeof(K))));
			}, () => Convert.ToInt32(getMethod(data)).IsSet(i));
		}
	}
}
