using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public interface IBookGrouper
	{
		IGrouper<ComicBook> BookGrouper
		{
			get;
		}
	}
}
