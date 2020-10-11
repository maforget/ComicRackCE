using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupScanInformation : ComicBookStringGrouper<ComicBookScanInformationMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return SingleComicGrouper.GetNameGroup(item.ScanInformation);
		}
	}
}
