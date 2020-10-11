using System;

namespace cYo.Common.ComponentModel
{
	public interface IItemLock<T> : IDisposable
	{
		T Item
		{
			get;
			set;
		}

		object LockObject
		{
			get;
		}

		object Tag
		{
			get;
			set;
		}
	}
}
