using cYo.Projects.ComicRack.Engine.Database;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public interface IImportComicList
	{
		ComicListItem ImportList(string file);
	}
}
