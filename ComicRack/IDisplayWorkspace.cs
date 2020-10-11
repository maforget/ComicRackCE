using cYo.Projects.ComicRack.Viewer.Config;

namespace cYo.Projects.ComicRack.Viewer
{
	public interface IDisplayWorkspace
	{
		void SetWorkspace(DisplayWorkspace ws);

		void StoreWorkspace(DisplayWorkspace ws);
	}
}
