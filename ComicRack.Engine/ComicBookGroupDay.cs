using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupDay : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return new GroupInfo((item.Day <= 0) ? GroupInfo.Unspecified : item.DayAsText, item.Day);
		}
	}
}
