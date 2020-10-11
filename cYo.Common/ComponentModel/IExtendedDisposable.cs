using System;

namespace cYo.Common.ComponentModel
{
	public interface IExtendedDisposable : IDisposable
	{
		bool IsDisposed
		{
			get;
		}

		event EventHandler Disposing;

		event EventHandler Disposed;
	}
}
