using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using cYo.Common.Properties;

namespace cYo.Common.Win32
{
	public static class BitmapCursor
	{
		private class NativeBitmapCursor : Component, IBitmapCursor, IDisposable
		{
			private static class Native
			{
				public struct ICONINFO
				{
					public bool fIcon;

					public uint xHotspot;

					public uint yHotspot;

					public IntPtr hbmMask;

					public IntPtr hbmColor;
				}

				[DllImport("USER32.DLL")]
				public static extern IntPtr CreateIconIndirect(ref ICONINFO iconinfo);

				[DllImport("USER32.DLL")]
				[return: MarshalAs(UnmanagedType.Bool)]
				public static extern bool DestroyIcon(IntPtr hIcon);

				[DllImport("gdi32.dll")]
				public static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, IntPtr lpvBits);

				[DllImport("gdi32.dll")]
				[return: MarshalAs(UnmanagedType.Bool)]
				public static extern bool DeleteObject(IntPtr hObject);
			}

			private IntPtr hIcon = IntPtr.Zero;

			private volatile bool bitmapOwned;

			private Bitmap bitmap;

			private Cursor overlayCursor;

			private BitmapCursorOverlayEffect overlayEffect;

			private Point hotSpot;

			private Cursor cursor;

			public bool BitmapOwned
			{
				get
				{
					return bitmapOwned;
				}
				set
				{
					bitmapOwned = value;
				}
			}

			public Bitmap Bitmap
			{
				get
				{
					return bitmap;
				}
				set
				{
					SetBitmap(value);
				}
			}

			public Cursor OverlayCursor
			{
				get
				{
					return overlayCursor;
				}
				set
				{
					if (!(overlayCursor == value))
					{
						overlayCursor = value;
						InvalidateCursor();
					}
				}
			}

			public BitmapCursorOverlayEffect OverlayEffect
			{
				get
				{
					return overlayEffect;
				}
				set
				{
					if (overlayEffect != value)
					{
						overlayEffect = value;
						InvalidateCursor();
					}
				}
			}

			public Point HotSpot
			{
				get
				{
					return hotSpot;
				}
				set
				{
					if (!(hotSpot == value))
					{
						hotSpot = value;
						InvalidateCursor();
					}
				}
			}

			public Cursor Cursor
			{
				get
				{
					if (Screen.PrimaryScreen.BitsPerPixel != 32 || bitmap == null)
					{
						return null;
					}
					if (cursor == null)
					{
						hIcon = CreateCustomCursor(bitmap, overlayCursor, hotSpot, overlayEffect);
						cursor = new Cursor(hIcon);
					}
					return cursor;
				}
			}

			private static IntPtr CreateHBitmap(Bitmap bmp)
			{
				BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
				uint cBitsPerPel = (uint)(bitmapData.Stride / bmp.Width * 8);
				try
				{
					return Native.CreateBitmap(bmp.Width, bmp.Height, 1u, cBitsPerPel, bitmapData.Scan0);
				}
				finally
				{
					bmp.UnlockBits(bitmapData);
				}
			}

			public NativeBitmapCursor(Bitmap bitmap, Cursor overlayCursor, Point hotSpot)
			{
				Bitmap = bitmap;
				OverlayCursor = overlayCursor;
				HotSpot = hotSpot;
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					SetBitmap(null);
				}
				base.Dispose(disposing);
			}

			private void SetBitmap(Bitmap value)
			{
				if (this.bitmap != value)
				{
					Bitmap bitmap = this.bitmap;
					this.bitmap = value;
					if (bitmapOwned)
					{
						bitmap?.Dispose();
					}
					InvalidateCursor();
				}
			}

			private void InvalidateCursor()
			{
				_ = hIcon;
				Native.DestroyIcon(hIcon);
				cursor = null;
			}

			private static IntPtr CreateCustomCursor(Bitmap bitmap, Cursor overlayCursor, Point hotSpot, BitmapCursorOverlayEffect effect)
			{
				IntPtr intPtr = IntPtr.Zero;
				if (overlayCursor == null)
				{
					intPtr = CreateHBitmap(bitmap);
				}
				else
				{
					Point point = overlayCursor.HotSpot;
					Size size = overlayCursor.Size;
					Point p = new Point(Math.Min(0, hotSpot.X - point.X), Math.Min(0, hotSpot.Y - point.Y));
					Size size2 = new Size(Math.Max(bitmap.Width, hotSpot.X - point.X + size.Width), Math.Max(bitmap.Height, hotSpot.Y - point.Y + size.Height));
					point.Offset(p);
					using (Bitmap bitmap2 = new Bitmap(size2.Width - p.X, size2.Height - p.Y))
					{
						hotSpot.Offset(-point.X, -point.Y);
						using (Graphics graphics = Graphics.FromImage(bitmap2))
						{
							Rectangle targetRect = new Rectangle(hotSpot, size);
							graphics.DrawImage(bitmap, -p.X, -p.Y);
							if (effect != 0 && effect == BitmapCursorOverlayEffect.Plus)
							{
								using (Bitmap bitmap3 = Resources.PlusOverlay)
								{
									graphics.DrawImage(bitmap3, targetRect.Left + point.X + 24 - bitmap3.Width, targetRect.Top + point.Y + 24 - bitmap3.Height);
								}
							}
							overlayCursor.Draw(graphics, targetRect);
						}
						intPtr = CreateHBitmap(bitmap2);
					}
				}
				try
				{
					Native.ICONINFO iconinfo = default(Native.ICONINFO);
					iconinfo.fIcon = false;
					iconinfo.xHotspot = (uint)hotSpot.X;
					iconinfo.yHotspot = (uint)hotSpot.Y;
					iconinfo.hbmMask = intPtr;
					iconinfo.hbmColor = intPtr;
					return Native.CreateIconIndirect(ref iconinfo);
				}
				finally
				{
					Native.DeleteObject(intPtr);
				}
			}
		}

		public static IBitmapCursor Create(Bitmap bitmap, Cursor overlayCursor, Point hotSpot)
		{
			return new NativeBitmapCursor(bitmap, overlayCursor, hotSpot);
		}

		public static IBitmapCursor Create(Bitmap bitmap, Point hotSpot)
		{
			return Create(bitmap, null, hotSpot);
		}

		public static IBitmapCursor Create(Bitmap bitmap)
		{
			return Create(bitmap, null, Point.Empty);
		}
	}
}
