namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class CoverViewItemRatingComparer : CoverViewItemComparer
	{
		protected override int OnCompare(CoverViewItem x, CoverViewItem y)
		{
			return x.AverageRating.CompareTo(y.AverageRating);
		}
	}
}
