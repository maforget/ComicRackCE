using System;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public interface IComicBookListProvider : ILiteComponent, IDisposable, IIdentity, IComicBookList
	{
		int BookCount
		{
			get;
			set;
		}

		event EventHandler BookListChanged;

		event EventHandler NameChanged;

		void Refresh();
	}
}
