using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public interface IGetBookList
	{
		IEnumerable<ComicBook> GetBookList(ComicBookFilterType bookListType);
	}
}
