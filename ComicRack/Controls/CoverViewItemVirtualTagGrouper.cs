using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Metadata.VirtualTags;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class CoverViewItemVirtualTagGrouper: IGrouper<IViewableItem>, IBookGrouper
	{
		private readonly IGrouper<ComicBook> bookGrouper;
		public CoverViewItemVirtualTagGrouper(IVirtualTag vtag)
		{
			bookGrouper = new ComicBookVirtualTagGrouper(vtag);
		}

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
