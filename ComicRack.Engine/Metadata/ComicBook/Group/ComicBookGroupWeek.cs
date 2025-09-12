using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupWeek : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return new GroupInfo((item.Week <= 0) ? GroupInfo.Unspecified : item.WeekAsText, item.Week);
		}
	}
}
