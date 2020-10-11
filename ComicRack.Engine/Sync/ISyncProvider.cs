using System;
using System.Collections.Generic;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.IO.Cache;

namespace cYo.Projects.ComicRack.Engine.Sync
{
	public interface ISyncProvider
	{
		DeviceInfo Device
		{
			get;
		}

		void ValidateDevice(DeviceInfo device);

		void Start();

		IEnumerable<ComicBook> GetBooks();

		void Add(ComicBook book, bool optimize, IPagePool pagePool, Action working, Action start, Action completed);

		void Remove(ComicBook book);

		void SetLists(IEnumerable<ComicIdListItem> myBookLists);

		void WaitForWritesCompleted();

		bool Progress(int percent);

		void Completed();
	}
}
