using System.Drawing;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public interface IListDisplays
	{
		void AddListWindow(Image tabImage, IComicBookListProvider bookList);

		void AddListTab(Image tabImage, IComicBookListProvider bookList);

		void RemoveList(IComicBookListProvider bookList, object hint);
	}
}
