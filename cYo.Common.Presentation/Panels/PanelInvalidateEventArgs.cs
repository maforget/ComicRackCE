using System;
using System.Drawing;

namespace cYo.Common.Presentation.Panels
{
	public class PanelInvalidateEventArgs : EventArgs
	{
		private readonly bool always;

		private readonly Rectangle bounds;

		public bool Always => always;

		public Rectangle Bounds => bounds;

		public PanelInvalidateEventArgs(Rectangle bounds, bool always)
		{
			this.bounds = bounds;
			this.always = always;
		}
	}
}
