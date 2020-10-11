using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupSeriesGroup : ComicBookStringGrouper<ComicBookSeriesGroupMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return SingleComicGrouper.GetNameGroup(item.SeriesGroup);
		}
	}
}
