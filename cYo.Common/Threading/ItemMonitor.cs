using System;
using System.Threading;

namespace cYo.Common.Threading
{
	public struct ItemMonitor : IDisposable
	{
		public static bool CatchThreadInterruptException = true;

		private object lockItem;

		public object LockItem => lockItem;

		private ItemMonitor(object lockItem)
		{
			this.lockItem = lockItem;
			if (lockItem == null)
			{
				return;
			}
			try
			{
				Monitor.Enter(this.lockItem);
			}
			catch (ThreadInterruptedException)
			{
				if (CatchThreadInterruptException)
				{
					this.lockItem = null;
					return;
				}
				throw;
			}
		}

		public void Dispose()
		{
			try
			{
				if (lockItem != null)
				{
					Monitor.Exit(lockItem);
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				lockItem = null;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != typeof(ItemMonitor))
			{
				return false;
			}
			return lockItem == ((ItemMonitor)obj).lockItem;
		}

		public override int GetHashCode()
		{
			if (lockItem != null)
			{
				return lockItem.GetHashCode();
			}
			return 0;
		}

		public static bool operator ==(ItemMonitor a, ItemMonitor b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(ItemMonitor a, ItemMonitor b)
		{
			return !(a == b);
		}

		public static IDisposable Lock(object o)
		{
			return new ItemMonitor(o);
		}
	}
}
