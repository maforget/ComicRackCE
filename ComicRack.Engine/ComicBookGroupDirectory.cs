using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupDirectory : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return new GroupInfo(item.FileDirectory.ToUpperInvariant());
		}
	}
}
