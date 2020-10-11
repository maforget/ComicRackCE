using cYo.Common.ComponentModel;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public interface IBookGrouper
	{
		IGrouper<ComicBook> BookGrouper
		{
			get;
		}
	}
}
