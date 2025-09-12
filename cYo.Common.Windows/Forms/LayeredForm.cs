using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using cYo.Common.Drawing;

namespace cYo.Common.Windows.Forms
{
	public partial class LayeredForm : Form
	{
		public static class LayeredApi
		{
			private enum Bool
			{
				False,
				True
			}

			private struct Point
			{
				public readonly int x;

				public readonly int y;

				public Point(int x, int y)
				{
					this.x = x;
					this.y = y;
				}
			}

			private struct Size
			{
				public readonly int cx;

				public readonly int cy;

				public Size(int cx, int cy)
				{
					this.cx = cx;
					this.cy = cy;
				}
			}

			[StructLayout(LayoutKind.Sequential, Pack = 1)]
			private struct ARGB
			{
				public readonly byte Blue;

				public readonly byte Green;

				public readonly byte Red;

				public readonly byte Alpha;
			}

			[StructLayout(LayoutKind.Sequential, Pack = 1)]
			private struct BLENDFUNCTION
			{
				public byte BlendOp;

				public byte BlendFlags;

				public byte SourceConstantAlpha;

				public byte AlphaFormat;
			}

			public const int WS_EX_LAYERED = 524288;

			public const int HTCAPTION = 2;

			public const int WM_NCHITTEST = 132;

			public const int ULW_ALPHA = 2;

			public const byte AC_SRC_OVER = 0;

			public const byte AC_SRC_ALPHA = 1;

			[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
			private static extern Bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

			[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
			private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

			[DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
			private static extern IntPtr GetDC(IntPtr hWnd);

			[DllImport("user32.dll", ExactSpelling = true)]
			private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

			[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
			private static extern Bool DeleteDC(IntPtr hdc);

			[DllImport("gdi32.dll", ExactSpelling = true)]
			private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

			[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
			private static extern Bool DeleteObject(IntPtr hObject);

			public static void SelectBitmap(Form form, Bitmap bitmap, int alpha)
			{
				IntPtr dC = GetDC(IntPtr.Zero);
				IntPtr intPtr = CreateCompatibleDC(dC);
				IntPtr intPtr2 = IntPtr.Zero;
				IntPtr hObject = IntPtr.Zero;
				try
				{
					intPtr2 = bitmap.GetHbitmap(Color.FromArgb(0));
					hObject = SelectObject(intPtr, intPtr2);
					Size psize = new Size(bitmap.Width, bitmap.Height);
					Point pprSrc = new Point(0, 0);
					Point pptDst = new Point(form.Left, form.Top);
					BLENDFUNCTION pblend = default(BLENDFUNCTION);
					pblend.BlendOp = 0;
					pblend.BlendFlags = 0;
					pblend.SourceConstantAlpha = (byte)alpha;
					pblend.AlphaFormat = 1;
					UpdateLayeredWindow(form.Handle, dC, ref pptDst, ref psize, intPtr, ref pprSrc, 0, ref pblend, 2);
				}
				finally
				{
					ReleaseDC(IntPtr.Zero, dC);
					if (intPtr2 != IntPtr.Zero)
					{
						SelectObject(intPtr, hObject);
						DeleteObject(intPtr2);
					}
					DeleteDC(intPtr);
				}
			}
		}

		private Bitmap surface;

		private int alpha;

		public Bitmap Surface
		{
			get
			{
				return surface;
			}
			set
			{
				if (surface != value)
				{
					surface = value;
					base.Width = surface.Width;
					base.Height = surface.Height;
					UpdateSurface();
				}
			}
		}

		public int Alpha
		{
			get
			{
				return alpha;
			}
			set
			{
				if (alpha != value)
				{
					alpha = value;
					Invalidate();
				}
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				if (!base.DesignMode)
				{
					createParams.ExStyle |= LayeredApi.WS_EX_LAYERED;
				}
				return createParams;
			}
		}

		protected override void OnInvalidated(InvalidateEventArgs e)
		{
			base.OnInvalidated(e);
			if (!base.DesignMode)
			{
				UpdateSurface();
			}
		}

		private void UpdateSurface()
		{
			try
			{
				if (this.InvokeIfRequired(UpdateSurface))
				{
					return;
				}
				using (Bitmap bitmap = new Bitmap(base.Width, base.Height))
				{
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.Clear(Color.Transparent);
						if (surface != null)
						{
							graphics.DrawImage(surface, surface.Size.ToRectangle());
						}
						OnPaint(new PaintEventArgs(graphics, bitmap.Size.ToRectangle()));
					}
					LayeredApi.SelectBitmap(this, bitmap, alpha);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
