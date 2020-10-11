using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public interface IComicBookList
	{
		string Name
		{
			get;
		}

		IEnumerable<ComicBook> GetBooks();
	}
}
