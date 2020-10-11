using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Plugins.Automation
{
	public interface IOpenBooksManager
	{
		bool Open(ComicBook cb, bool inNewSlot, int page);

		bool OpenFile(string file, bool inNewSlot, int page);

		bool IsOpen(ComicBook cb);
	}
}
