using cYo.Common;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupModified : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return GroupInfo.GetDateGroup(item.FileModifiedTime.SafeToLocalTime());
		}
	}
}
