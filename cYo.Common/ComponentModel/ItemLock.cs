using System;
using cYo.Common.Threading;

namespace cYo.Common.ComponentModel
{
	public class ItemLock<T> : DisposableObject, IItemLock<T>, IDisposable where T : class
	{
		private readonly IDisposable monitor;

		private volatile T item;

		public T Item
		{
			get
			{
				return item;
			}
			set
			{
				item = value;
			}
		}

		public object LockObject
		{
			get;
			set;
		}

		public object Tag
		{
			get;
			set;
		}

		public ItemLock(T data, object lockObject)
		{
			Item = data;
			monitor = ItemMonitor.Lock(lockObject);
		}

		public ItemLock(T data, bool synchronized)
			: this(data, (object)(synchronized ? data : null))
		{
		}

		public ItemLock(T data)
			: this(data, synchronized: true)
		{
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				monitor.Dispose();
			}
			catch
			{
			}
			finally
			{
				Item = null;
			}
			base.Dispose(disposing);
		}
	}
}
