using System.Collections.Generic;
using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class CoverViewItemBookGrouper<T> : IGrouper<IViewableItem>, IBookGrouper where T : IGrouper<ComicBook>, new()
	{
		private readonly IGrouper<ComicBook> bookGrouper = new T();

		public IGrouper<ComicBook> BookGrouper => bookGrouper;

		public bool IsMultiGroup => bookGrouper.IsMultiGroup;

		public IGroupInfo GetGroup(IViewableItem item)
		{
			return bookGrouper.GetGroup(((CoverViewItem)item).Comic);
		}

		public IEnumerable<IGroupInfo> GetGroups(IViewableItem item)
		{
			return bookGrouper.GetGroups(((CoverViewItem)item).Comic);
		}
	}
}
