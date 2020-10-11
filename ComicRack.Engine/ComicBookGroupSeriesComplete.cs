using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupSeriesComplete : SingleComicGrouper
	{
		private readonly string[] captions = GroupInfo.TRGroup.GetStrings("SeriesCompleteGroups", "Series complete|Series not complete|Unknown", '|');

		public override IGroupInfo GetGroup(ComicBook item)
		{
			switch (item.SeriesComplete)
			{
			case YesNo.Yes:
				return new GroupInfo(captions[0], 0);
			case YesNo.No:
				return new GroupInfo(captions[1], 1);
			default:
				return new GroupInfo(captions[2], 2);
			}
		}
	}
}
