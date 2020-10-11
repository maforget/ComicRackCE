using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupBookCollectionStatus : ComicBookStringGrouper<ComicBookBookCollectionStatusMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return SingleComicGrouper.GetNameGroup(item.BookCollectionStatus);
		}
	}
}
