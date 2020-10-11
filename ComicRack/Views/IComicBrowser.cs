using System;
using System.Collections.Generic;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public interface IComicBrowser : IGetBookList
	{
		string SelectionInfo
		{
			get;
		}

		ComicLibrary Library
		{
			get;
		}

		DisplayListConfig ListConfig
		{
			get;
			set;
		}

		bool SelectComic(ComicBook comic);

		bool SelectComics(IEnumerable<ComicBook> comics);

		Guid GetBookListId();
	}
}
