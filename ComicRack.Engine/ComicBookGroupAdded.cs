using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupAdded : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return GroupInfo.GetDateGroup(item.AddedTime);
		}
	}
}
