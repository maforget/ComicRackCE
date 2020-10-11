using System.ComponentModel;
using System.Drawing;

namespace cYo.Common.Windows.Forms
{
	public class AutoScrollEventArgs : CancelEventArgs
	{
		private Point delta;

		public Point Delta
		{
			get
			{
				return delta;
			}
			set
			{
				delta = value;
			}
		}

		public int X
		{
			get
			{
				return delta.X;
			}
			set
			{
				delta.X = value;
			}
		}

		public int Y
		{
			get
			{
				return delta.Y;
			}
			set
			{
				delta.Y = value;
			}
		}
	}
}
