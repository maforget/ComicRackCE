using System;
using System.Collections.Generic;
using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class PageViewBookmarkGrouper : IGrouper<IViewableItem>
	{
		public bool IsMultiGroup => false;

		public IGroupInfo GetGroup(IViewableItem item)
		{
			PageViewItem pageViewItem = (PageViewItem)item;
			return new GroupInfo(GetBookmark(pageViewItem), pageViewItem.Page);
		}

		public IEnumerable<IGroupInfo> GetGroups(IViewableItem item)
		{
			throw new NotImplementedException();
		}

		private static string GetBookmark(PageViewItem item)
		{
			int page = item.Page;
			ComicBook comic = item.Comic;
			while (page >= 0)
			{
				string bookmark = comic.GetPage(page--).Bookmark;
				if (!string.IsNullOrEmpty(bookmark))
				{
					return bookmark;
				}
			}
			return item.Comic.Caption;
		}
	}
}
