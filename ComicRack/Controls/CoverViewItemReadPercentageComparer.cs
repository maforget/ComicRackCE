namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class CoverViewItemReadPercentageComparer : CoverViewItemComparer
	{
		protected override int OnCompare(CoverViewItem x, CoverViewItem y)
		{
			return x.StackReadPercent.CompareTo(y.StackReadPercent);
		}
	}
}
