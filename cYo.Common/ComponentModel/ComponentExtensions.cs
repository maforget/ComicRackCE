using System;

namespace cYo.Common.ComponentModel
{
	public static class ComponentExtensions
	{
		public static void SafeDispose(this IDisposable obj)
		{
			if (obj != null)
			{
				try
				{
					obj.Dispose();
				}
				catch (Exception)
				{
				}
			}
		}

		public static bool IsAlive<T>(this WeakReference<T> obj) where T : class
		{
			T target;
			return obj.TryGetTarget(out target);
		}

		public static T GetData<T>(this WeakReference<T> obj) where T : class
		{
			if (!obj.TryGetTarget(out var target))
			{
				return null;
			}
			return target;
		}
	}
}
