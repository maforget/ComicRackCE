using System;
using System.Drawing;
using System.Runtime.InteropServices;
using cYo.Common.ComponentModel;

namespace cYo.Common.Win32
{
	public class FontDC : DisposableObject, IDeviceContext, IDisposable
	{
		private static class UnsafeNativeMethods
		{
			[DllImport("Gdi32", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
			public static extern IntPtr SelectObject(HandleRef hdc, HandleRef obj);

			[DllImport("Gdi32", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
			public static extern bool DeleteObject(HandleRef hObject);
		}

		private readonly Graphics graphics;

		private IntPtr hdc = IntPtr.Zero;

		private IntPtr hfont = IntPtr.Zero;

		private Font font;

		public Font Font
		{
			get
			{
				return font;
			}
			set
			{
				if (font == value)
				{
					return;
				}
				font = value;
				if (font != null)
				{
					hfont = font.ToHfont();
					IntPtr intPtr = UnsafeNativeMethods.SelectObject(new HandleRef(this, GetHdc()), new HandleRef(this, hfont));
					if (intPtr != IntPtr.Zero)
					{
						UnsafeNativeMethods.DeleteObject(new HandleRef(this, intPtr));
					}
				}
				else if (hfont != IntPtr.Zero)
				{
					UnsafeNativeMethods.DeleteObject(new HandleRef(this, hfont));
				}
			}
		}

		public FontDC(Graphics g, Font font)
		{
			graphics = g;
			Font = font;
		}

		public FontDC(Graphics g)
			: this(g, null)
		{
		}

		protected override void Dispose(bool disposing)
		{
			ReleaseHdc();
			base.Dispose(disposing);
		}

		public IntPtr GetHdc()
		{
			if (hdc == IntPtr.Zero)
			{
				hdc = graphics.GetHdc();
			}
			return hdc;
		}

		public void ReleaseHdc()
		{
			Font = null;
			if (hdc != IntPtr.Zero)
			{
				graphics.ReleaseHdc();
				hdc = IntPtr.Zero;
			}
		}
	}
}
