using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using cYo.Common.ComponentModel;

namespace cYo.Common.Drawing
{
	public sealed class FastBitmapLock : DisposableObject
	{
		private readonly IntPtr scan = IntPtr.Zero;

		private readonly Bitmap bitmap;

		private readonly BitmapData bitmapData;

		private readonly bool bitmapOwned;

		private readonly IntPtr data;

		private readonly int size;

		private readonly int width;

		private readonly int height;

		public IntPtr Data => data;

		public int Size => size;

		public int Width => width;

		public int Height => height;

		public FastBitmapLock(Bitmap bmp, Rectangle rc)
			: this(bmp, rc, allowWrite: false)
		{
		}

		public unsafe FastBitmapLock(Bitmap bmp, Rectangle rc, bool allowWrite)
		{
			try
			{
				if (rc.Width <= 0 || rc.Height <= 0 || rc.X >= bmp.Width || rc.Y >= bmp.Height)
				{
					return;
				}
				bitmap = bmp;
				width = rc.Width;
				height = rc.Height;
				size = width * height * 4;
				if (bmp.PixelFormat != PixelFormat.Format32bppArgb && bmp.PixelFormat != PixelFormat.Format24bppRgb)
				{
					bitmap = bmp.CreateCopy(rc, PixelFormat.Format32bppArgb);
					bitmapOwned = true;
					rc.Location = Point.Empty;
				}
				bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), (!allowWrite) ? ImageLockMode.ReadOnly : ImageLockMode.ReadWrite, bitmap.PixelFormat);
				data = (scan = bitmapData.Scan0);
				if (rc.Location.IsEmpty && rc.Size == bitmap.Size && bitmap.PixelFormat == PixelFormat.Format32bppArgb)
				{
					return;
				}
				int num = Math.Min(width, bmp.Width - rc.X);
				int num2 = Math.Min(height, bmp.Height - rc.Y);
				int stride = bitmapData.Stride;
				int x = rc.X;
				int num3 = stride / bitmap.Width;
				data = Marshal.AllocHGlobal(size);
				try
				{
					byte* ptr = (byte*)(void*)scan + stride * rc.Y;
					switch (num3)
					{
					case 4:
					{
						int* ptr5 = (int*)(void*)data;
						for (int k = 0; k < num2; k++)
						{
							int* ptr6 = (int*)(ptr + (long)x * 4L);
							int* ptr7 = ptr5;
							for (int l = 0; l < num; l++)
							{
								int* intPtr = ptr7;
								ptr7 = intPtr + 1;
								int* intPtr2 = ptr6;
								ptr6 = intPtr2 + 1;
								*intPtr = *intPtr2;
							}
							ptr5 += width;
							ptr += stride;
						}
						break;
					}
					case 3:
					{
						byte* ptr2 = (byte*)(void*)data;
						for (int i = 0; i < num2; i++)
						{
							byte* ptr3 = ptr + x * num3;
							byte* ptr4 = ptr2;
							for (int j = 0; j < num; j++)
							{
								*(ptr4++) = *(ptr3++);
								*(ptr4++) = *(ptr3++);
								*(ptr4++) = *(ptr3++);
								*(ptr4++) = byte.MaxValue;
							}
							ptr2 += width << 2;
							ptr += stride;
						}
						break;
					}
					}
				}
				finally
				{
					BitmapData bitmapdata = bitmapData;
					bitmapData = null;
					bitmap.UnlockBits(bitmapdata);
				}
			}
			catch (Exception)
			{
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (data != scan)
			{
				Marshal.FreeHGlobal(data);
			}
			if (bitmapData != null)
			{
				bitmap.UnlockBits(bitmapData);
			}
			if (bitmapOwned && bitmap != null)
			{
				bitmap.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
