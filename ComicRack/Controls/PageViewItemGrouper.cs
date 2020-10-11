using System.Collections.Generic;
using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class PageViewItemGrouper<T> : IGrouper<IViewableItem> where T : IGrouper<ComicPageInfo>, new()
	{
		private readonly T grouper = new T();

		public bool IsMultiGroup
		{
			get
			{
				T val = grouper;
				return val.IsMultiGroup;
			}
		}

		public IGroupInfo GetGroup(IViewableItem item)
		{
			PageViewItem pageViewItem = (PageViewItem)item;
			T val = grouper;
			return val.GetGroup(pageViewItem.PageInfo);
		}

		public IEnumerable<IGroupInfo> GetGroups(IViewableItem item)
		{
			PageViewItem pageViewItem = (PageViewItem)item;
			T val = grouper;
			return val.GetGroups(pageViewItem.PageInfo);
		}
	}
}
