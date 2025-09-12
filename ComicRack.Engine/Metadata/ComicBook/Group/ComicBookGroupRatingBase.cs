using cYo.Common.ComponentModel;
using cYo.Common.Mathematics;

namespace cYo.Projects.ComicRack.Engine
{
	public abstract class ComicBookGroupRatingBase : SingleComicGrouper
	{
		private static readonly string[] captions = GroupInfo.TRGroup.GetStrings("RatingGroups", "Not Rated|No Stars|1 Star|2 Stars|3 Stars|4 Stars|5 Stars", '|');

		public override IGroupInfo GetGroup(ComicBook item)
		{
			return GetRatingGroup(GetRating(item));
		}

		protected abstract int GetRating(ComicBook item);

		public static IGroupInfo GetRatingGroup(int rating)
		{
			int num = rating.Clamp(-1, captions.Length - 2) + 1;
			return new GroupInfo(captions[num], captions.Length - num);
		}
	}
}
