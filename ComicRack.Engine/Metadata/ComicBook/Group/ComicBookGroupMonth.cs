using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupMonth : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return new GroupInfo((item.Month <= 0) ? GroupInfo.Unspecified : item.MonthAsText, item.Month);
		}
	}
}
