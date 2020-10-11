using System;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public interface IEditableComicBookListProvider : IComicBookListProvider, ILiteComponent, IDisposable, IIdentity, IComicBookList
	{
		bool IsLibrary
		{
			get;
		}

		int Add(ComicBook comicBook);

		int Insert(int index, ComicBook comicBook);

		bool Remove(ComicBook comicBook);
	}
}
