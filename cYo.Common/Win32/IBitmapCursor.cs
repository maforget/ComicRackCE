using System;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Win32
{
	public interface IBitmapCursor : IDisposable
	{
		Bitmap Bitmap
		{
			get;
			set;
		}

		bool BitmapOwned
		{
			get;
			set;
		}

		Cursor Cursor
		{
			get;
		}

		Point HotSpot
		{
			get;
			set;
		}

		Cursor OverlayCursor
		{
			get;
			set;
		}

		BitmapCursorOverlayEffect OverlayEffect
		{
			get;
			set;
		}
	}
}
