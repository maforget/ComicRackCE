using System.Collections.Generic;
using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class CoverViewItemStatsGrouper<T> : IGrouper<IViewableItem> where T : IGrouper<ComicBookSeriesStatistics>, new()
	{
		private readonly IGrouper<ComicBookSeriesStatistics> statsGrouper = new T();

		public bool IsMultiGroup => statsGrouper.IsMultiGroup;

		public IGroupInfo GetGroup(IViewableItem item)
		{
			return statsGrouper.GetGroup(((CoverViewItem)item).SeriesStats);
		}

		public IEnumerable<IGroupInfo> GetGroups(IViewableItem item)
		{
			return statsGrouper.GetGroups(((CoverViewItem)item).SeriesStats);
		}
	}
}
