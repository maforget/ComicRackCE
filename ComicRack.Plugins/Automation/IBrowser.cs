using System.Collections.Generic;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Plugins.Automation
{
	public interface IBrowser
	{
		bool OpenNextComic();

		bool OpenPrevComic();

		bool OpenRandomComic();

		void SelectComics(IEnumerable<ComicBook> books);
	}
}
