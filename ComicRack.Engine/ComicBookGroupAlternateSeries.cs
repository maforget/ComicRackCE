using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupAlternateSeries : ComicBookStringGrouper<ComicBookAlternateSeriesMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return SingleComicGrouper.GetNameGroup(item.AlternateSeries);
		}
	}
}
