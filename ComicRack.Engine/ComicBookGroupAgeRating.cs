using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupAgeRating : ComicBookStringGrouper<ComicBookAgeRatingMatcher>
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			return new GroupInfo(string.IsNullOrEmpty(item.AgeRating) ? GroupInfo.Unspecified : item.AgeRating);
		}
	}
}
