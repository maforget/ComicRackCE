using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using cYo.Common.Drawing;
using OpenTK;
using OpenTK.Platform.Windows;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
//using Tao.OpenGl;
//using Tao.Platform.Windows;

namespace cYo.Common.Presentation.Tao
{
	public static class OpenGlImageProcessing
	{
		public static Bitmap Resize(Bitmap bmp, int width, int height)
		{
			InitializeOpenGl();
			Bitmap bitmap = null;
			if (bmp.PixelFormat != PixelFormat.Format32bppArgb && bmp.PixelFormat != PixelFormat.Format24bppRgb)
			{
				bmp = (bitmap = bmp.CreateCopy(PixelFormat.Format32bppArgb));
			}
			try
			{
				Bitmap bitmap2 = new Bitmap(width, height, PixelFormat.Format32bppArgb);
				bool flag;
				using (FastBitmapLock fastBitmapLock = new FastBitmapLock(bmp, bmp.Size.ToRectangle()))
				{
					using (FastBitmapLock fastBitmapLock2 = new FastBitmapLock(bitmap2, bitmap2.Size.ToRectangle(), allowWrite: true))
					{
						Glu.gluScaleImage(32993, fastBitmapLock.Width, fastBitmapLock.Height, 5121, fastBitmapLock.Data, fastBitmapLock2.Width, fastBitmapLock2.Height, 5121, fastBitmapLock2.Data);
						flag = GL.GetError() != ErrorCode.NoError;
					}
				}
				if (flag)
				{
					using (Graphics graphics = Graphics.FromImage(bitmap2))
					{
						using (graphics.HighQuality(enabled: true, new Size(width, height), bmp.Size))
						{
							graphics.DrawImage(bmp, new Rectangle(0, 0, width, height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel);
						}
					}
				}
				return bitmap2;
			}
			finally
			{
				if (bitmap == bmp)
				{
					bitmap.Dispose();
				}
			}
		}

		private static void InitializeOpenGl()
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			if (!(Wgl.wglGetCurrentContext() != IntPtr.Zero))
			{
				if (intPtr == IntPtr.Zero)
				{
					Gdi.PIXELFORMATDESCRIPTOR pixelFormatDescriptor = default(Gdi.PIXELFORMATDESCRIPTOR);
					pixelFormatDescriptor.nSize = (short)Marshal.SizeOf((object)pixelFormatDescriptor);
					pixelFormatDescriptor.nVersion = 1;
					pixelFormatDescriptor.dwFlags = 33;
					pixelFormatDescriptor.cColorBits = 8;
					intPtr = User.GetDC(IntPtr.Zero);
					int pixelFormat = Gdi.ChoosePixelFormat(intPtr, ref pixelFormatDescriptor);
					Gdi.SetPixelFormat(intPtr, pixelFormat, ref pixelFormatDescriptor);
				}
				if (intPtr2 == IntPtr.Zero)
				{
					intPtr2 = Wgl.wglCreateContext(intPtr);
				}
				Wgl.wglMakeCurrent(intPtr, intPtr2);
			}
		}
	}
}
