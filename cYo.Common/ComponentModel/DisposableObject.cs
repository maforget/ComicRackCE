using System;
using System.Diagnostics;

namespace cYo.Common.ComponentModel
{
	[Serializable]
	public abstract class DisposableObject : IDisposable
	{
		private bool isDisposed;

		public bool IsDisposed => isDisposed;

		[field: NonSerialized]
		public event EventHandler Disposing;

		[field: NonSerialized]
		public event EventHandler Disposed;

		[Conditional("DEBUG")]
		public void FinalizerIsOk()
		{
		}

		public DisposableObject()
		{
		}

		~DisposableObject()
		{
			try
			{
				Dispose(disposing: false);
			}
			catch (Exception)
			{
			}
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		protected virtual void CheckDisposed()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException(GetType().ToString());
			}
		}

		public void Dispose()
		{
			try
			{
				try
				{
					if (this.Disposing != null)
					{
						this.Disposing(this, EventArgs.Empty);
					}
				}
				catch
				{
				}
				Dispose(disposing: true);
				isDisposed = true;
				try
				{
					if (this.Disposed != null)
					{
						this.Disposed(this, EventArgs.Empty);
					}
				}
				catch
				{
				}
			}
			catch
			{
			}
			finally
			{
				GC.SuppressFinalize(this);
			}
		}
	}
}
