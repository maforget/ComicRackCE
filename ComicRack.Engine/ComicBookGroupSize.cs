using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupSize : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return GroupInfo.GetFileSizeGroup(item.FileSize);
		}
	}
}
