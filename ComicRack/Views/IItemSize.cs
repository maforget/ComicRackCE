using cYo.Projects.ComicRack.Viewer.Controls;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public interface IItemSize
	{
		ItemSizeInfo GetItemSize();

		void SetItemSize(int height);
	}
}
