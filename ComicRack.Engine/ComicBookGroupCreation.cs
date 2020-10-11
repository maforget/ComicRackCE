using cYo.Common;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupCreation : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return GroupInfo.GetDateGroup(item.FileCreationTime.SafeToLocalTime());
		}
	}
}
