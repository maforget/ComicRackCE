using System;
using System.Drawing;

namespace cYo.Common.Windows.Forms
{
	public interface IBitmapDisplayControl : IDisposable
	{
		Bitmap Bitmap
		{
			get;
			set;
		}

		object Tag
		{
			get;
			set;
		}

		void SetBitmap(Bitmap bitmap);
	}
}
