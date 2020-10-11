namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class CoverViewItemCommunityRatingComparer : CoverViewItemComparer
	{
		protected override int OnCompare(CoverViewItem x, CoverViewItem y)
		{
			return x.AverageCommunityRating.CompareTo(y.AverageCommunityRating);
		}
	}
}
