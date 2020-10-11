using System.Collections.Generic;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public interface IRemoveBooks
	{
		void RemoveBooks(IEnumerable<ComicBook> books, bool ask);
	}
}
