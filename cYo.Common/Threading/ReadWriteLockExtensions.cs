using System;
using System.Collections.Generic;
using System.Threading;
using cYo.Common.ComponentModel;

namespace cYo.Common.Threading
{
	public static class ReadWriteLockExtensions
	{
		public static readonly bool IgnoreUnlockErrors = true;

		public static IDisposable ReadLock(this ReaderWriterLockSlim rwLock)
		{
			rwLock.EnterReadLock();
			return new LeanDisposer(rwLock.ExitReadLock, IgnoreUnlockErrors);
		}

		public static IDisposable UpgradeableReadLock(this ReaderWriterLockSlim rwLock)
		{
			rwLock.EnterUpgradeableReadLock();
			return new LeanDisposer(rwLock.ExitUpgradeableReadLock, IgnoreUnlockErrors);
		}

		public static IDisposable WriteLock(this ReaderWriterLockSlim rwLock)
		{
			rwLock.EnterWriteLock();
			return new LeanDisposer(rwLock.ExitWriteLock, IgnoreUnlockErrors);
		}

		public static IEnumerable<T> ReadLock<T>(this IEnumerable<T> list, ReaderWriterLockSlim rwLock)
		{
			using (rwLock.ReadLock())
			{
				foreach (T item in list)
				{
					yield return item;
				}
			}
		}
	}
}
