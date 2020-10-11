using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine.Database.Storage
{
	public class ComicStorage : DisposableObject
	{
		private readonly IComicStorage storage;

		private readonly ComicBookContainer container;

		private readonly Thread updateThread;

		private readonly HashSet<ComicBook> deleteSet = new HashSet<ComicBook>();

		private readonly HashSet<ComicBook> writeSet = new HashSet<ComicBook>();

		private readonly AutoResetEvent stop = new AutoResetEvent(initialState: false);

		public bool IsConnected
		{
			get
			{
				if (storage != null)
				{
					return storage.IsConnected;
				}
				return false;
			}
		}

		public string LastConnectionError
		{
			get;
			private set;
		}

		public event CancelEventHandler OnShouldRefresh;

		private ComicStorage(ComicBookContainer library, IComicStorage storage, bool copyLocal, Action<int> progress)
		{
			this.storage = storage;
			container = library;
			if (copyLocal && library.Books.Count > 0)
			{
				storage.BeginTransaction();
				try
				{
					int count = library.Books.Count;
					int num = 0;
					foreach (ComicBook book in library.Books)
					{
						storage.Write(book);
						progress?.Invoke(++num * 100 / count);
					}
					storage.CommitTransaction();
				}
				catch
				{
					storage.RollbackTransaction();
					throw;
				}
			}
			library.Books.Clear();
			ComicBook[] list = storage.Load().ToArray();
			library.Books.AddRange(list);
			library.Books.Changed += Books_Changed;
			library.BookChanged += library_BookChanged;
			updateThread = ThreadUtility.CreateWorkerThread("Database Update", UpdateDatabase, ThreadPriority.BelowNormal);
			updateThread.Start();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				container.Books.Changed -= Books_Changed;
				container.BookChanged -= library_BookChanged;
				try
				{
					stop.Set();
					updateThread.Join();
					storage.Close();
				}
				catch (Exception)
				{
				}
			}
			base.Dispose(disposing);
		}

		private void UpdateDatabase()
		{
			while (!stop.WaitOne(1000))
			{
				WorkQueue();
			}
			WorkQueue();
		}

		private void WorkQueue()
		{
			try
			{
				using (ItemMonitor.Lock(writeSet))
				{
					ComicBook[] array = writeSet.ToArray();
					foreach (ComicBook comicBook in array)
					{
						storage.Refresh(container);
						storage.Write(comicBook);
						writeSet.Remove(comicBook);
					}
				}
				using (ItemMonitor.Lock(deleteSet))
				{
					ComicBook[] array2 = deleteSet.ToArray();
					foreach (ComicBook comicBook2 in array2)
					{
						storage.Delete(comicBook2);
						deleteSet.Remove(comicBook2);
					}
				}
				bool flag = true;
				if (this.OnShouldRefresh != null)
				{
					CancelEventArgs cancelEventArgs = new CancelEventArgs();
					this.OnShouldRefresh(this, cancelEventArgs);
					flag = !cancelEventArgs.Cancel;
				}
				if (flag)
				{
					storage.Refresh(container);
				}
			}
			catch (Exception ex)
			{
				LastConnectionError = ex.Message;
			}
		}

		private void library_BookChanged(object sender, ContainerBookChangedEventArgs e)
		{
			AddWriteToQueue(e.Book);
		}

		private void AddWriteToQueue(ComicBook book)
		{
			using (ItemMonitor.Lock(writeSet))
			{
				writeSet.Add(book);
			}
		}

		private void Books_Changed(object sender, SmartListChangedEventArgs<ComicBook> e)
		{
			switch (e.Action)
			{
			case SmartListAction.Insert:
				AddWriteToQueue(e.Item);
				break;
			case SmartListAction.Remove:
				using (ItemMonitor.Lock(deleteSet))
				{
					deleteSet.Add(e.Item);
				}
				break;
			}
		}

		public static ComicStorage Create(ComicBookContainer books, string connection, Action<int> progress)
		{
			IComicStorage comicStorage = null;
			if (connection.StartsWith("mysql:", StringComparison.OrdinalIgnoreCase))
			{
				connection = connection.Substring("mysql:".Length);
				comicStorage = new ComicStorageMySql();
			}
			else if (connection.StartsWith("mssql:", StringComparison.OrdinalIgnoreCase))
			{
				connection = connection.Substring("mssql:".Length);
				comicStorage = new ComicStorageMsSql();
			}
			if (comicStorage == null)
			{
				return null;
			}
			comicStorage.Open(connection);
			return new ComicStorage(books, comicStorage, copyLocal: true, progress);
		}
	}
}
