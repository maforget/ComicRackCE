using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupImprint : ComicBookStringGrouper<ComicBookImprintMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return SingleComicGrouper.GetNameGroup(item.Imprint);
		}
	}
}
