using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupCoverArtist : ComicBookStringGrouper<ComicBookCoverArtistMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return SingleComicGrouper.GetNameGroup(item.CoverArtist);
		}
	}
}
