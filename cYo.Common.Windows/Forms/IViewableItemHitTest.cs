using System.Drawing;

namespace cYo.Common.Windows.Forms
{
	public interface IViewableItemHitTest
	{
		bool Contains(Point pt);

		bool IntersectsWith(Rectangle rc);
	}
}
