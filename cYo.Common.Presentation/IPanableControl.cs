using System;
using System.Drawing;

namespace cYo.Common.Presentation
{
	public interface IPanableControl
	{
		Point PanLocation
		{
			get;
		}

		event EventHandler PanStart;

		event EventHandler PanEnd;

		event EventHandler Pan;
	}
}
