using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupBookOwner : ComicBookStringGrouper<ComicBookBookOwnerMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return SingleComicGrouper.GetNameGroup(item.BookOwner);
		}
	}
}
