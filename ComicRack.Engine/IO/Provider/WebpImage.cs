using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public static class WebpImage
	{
		private static class NativeMethods
		{
			[DllImport("Resources\\libwebp32.dll", EntryPoint = "WebPFree")]
			private static extern void WebPFree32(IntPtr toDeallocate);

			[DllImport("Resources\\libwebp64.dll", EntryPoint = "WebPFree")]
			private static extern void WebPFree64(IntPtr toDeallocate);

			[DllImport("Resources\\libwebp32.dll", EntryPoint = "WebPGetInfo")]
			private static extern int WebPGetInfo32([In] IntPtr data, UIntPtr dataSize, ref int width, ref int height);

			[DllImport("Resources\\libwebp64.dll", EntryPoint = "WebPGetInfo")]
			private static extern int WebPGetInfo64([In] IntPtr data, UIntPtr dataSize, ref int width, ref int height);

			[DllImport("Resources\\libwebp32.dll", EntryPoint = "WebPDecodeBGRAInto")]
			private static extern IntPtr WebPDecodeBGRAInto32([In] IntPtr data, UIntPtr dataSize, IntPtr outputBuffer, UIntPtr outputBufferSize, int outputStride);

			[DllImport("Resources\\libwebp64.dll", EntryPoint = "WebPDecodeBGRAInto")]
			private static extern IntPtr WebPDecodeBGRAInto64([In] IntPtr data, UIntPtr dataSize, IntPtr outputBuffer, UIntPtr outputBufferSize, int outputStride);

			[DllImport("Resources\\libwebp32.dll", EntryPoint = "WebPEncodeLosslessBGR")]
			private static extern UIntPtr WebPEncodeLosslessBGR32([In] IntPtr bgr, int width, int height, int stride, ref IntPtr output);

			[DllImport("Resources\\libwebp64.dll", EntryPoint = "WebPEncodeLosslessBGR")]
			private static extern UIntPtr WebPEncodeLosslessBGR64([In] IntPtr bgr, int width, int height, int stride, ref IntPtr output);

			[DllImport("Resources\\libwebp32.dll", EntryPoint = "WebPEncodeLosslessBGRA")]
			private static extern UIntPtr WebPEncodeLosslessBGRA32([In] IntPtr bgra, int width, int height, int stride, ref IntPtr output);

			[DllImport("Resources\\libwebp64.dll", EntryPoint = "WebPEncodeLosslessBGRA")]
			private static extern UIntPtr WebPEncodeLosslessBGRA64([In] IntPtr bgra, int width, int height, int stride, ref IntPtr output);

			[DllImport("Resources\\libwebp32.dll", EntryPoint = "WebPEncodeBGR")]
			private static extern UIntPtr WebPEncodeBGR32([In] IntPtr bgr, int width, int height, int stride, float qualityFactor, ref IntPtr output);

			[DllImport("Resources\\libwebp64.dll", EntryPoint = "WebPEncodeBGR")]
			private static extern UIntPtr WebPEncodeBGR64([In] IntPtr bgr, int width, int height, int stride, float qualityFactor, ref IntPtr output);

			[DllImport("Resources\\libwebp32.dll", EntryPoint = "WebPEncodeBGRA")]
			private static extern IntPtr WebPEncodeBGRA32([In] IntPtr bgra, int width, int height, int stride, float qualityFactor, ref IntPtr output);

			[DllImport("Resources\\libwebp64.dll", EntryPoint = "WebPEncodeBGRA")]
			private static extern IntPtr WebPEncodeBGRA64([In] IntPtr bgra, int width, int height, int stride, float qualityFactor, ref IntPtr output);

			public static int WebPGetInfo(IntPtr data, UIntPtr dataSize, ref int width, ref int height)
			{
				if (!Environment.Is64BitProcess)
				{
					return WebPGetInfo32(data, dataSize, ref width, ref height);
				}
				return WebPGetInfo64(data, dataSize, ref width, ref height);
			}

			public static void WebPFree(IntPtr toDeallocate)
			{
				if (Environment.Is64BitProcess)
				{
					WebPFree64(toDeallocate);
				}
				else
				{
					WebPFree32(toDeallocate);
				}
			}

			public static IntPtr WebPDecodeBGRAInto(IntPtr data, UIntPtr dataSize, IntPtr outputBuffer, UIntPtr outputBufferSize, int outputStride)
			{
				if (!Environment.Is64BitProcess)
				{
					return WebPDecodeBGRAInto32(data, dataSize, outputBuffer, outputBufferSize, outputStride);
				}
				return WebPDecodeBGRAInto64(data, dataSize, outputBuffer, outputBufferSize, outputStride);
			}

			public static UIntPtr WebPEncodeLosslessBGR(IntPtr bgr, int width, int height, int stride, ref IntPtr output)
			{
				if (!Environment.Is64BitProcess)
				{
					return WebPEncodeLosslessBGR32(bgr, width, height, stride, ref output);
				}
				return WebPEncodeLosslessBGR64(bgr, width, height, stride, ref output);
			}

			public static UIntPtr WebPEncodeLosslessBGRA(IntPtr bgra, int width, int height, int stride, ref IntPtr output)
			{
				if (!Environment.Is64BitProcess)
				{
					return WebPEncodeLosslessBGRA32(bgra, width, height, stride, ref output);
				}
				return WebPEncodeLosslessBGRA64(bgra, width, height, stride, ref output);
			}

			public static UIntPtr WebPEncodeBGR(IntPtr bgr, int width, int height, int stride, float qualityFactor, ref IntPtr output)
			{
				if (!Environment.Is64BitProcess)
				{
					return WebPEncodeBGR32(bgr, width, height, stride, qualityFactor, ref output);
				}
				return WebPEncodeBGR64(bgr, width, height, stride, qualityFactor, ref output);
			}

			public static IntPtr WebPEncodeBGRA(IntPtr bgra, int width, int height, int stride, float qualityFactor, ref IntPtr output)
			{
				if (!Environment.Is64BitProcess)
				{
					return WebPEncodeBGRA32(bgra, width, height, stride, qualityFactor, ref output);
				}
				return WebPEncodeBGRA64(bgra, width, height, stride, qualityFactor, ref output);
			}
		}

		private static readonly byte[] header1 = Encoding.ASCII.GetBytes("RIFF");

		private static readonly byte[] header2 = Encoding.ASCII.GetBytes("WEBP");

		public unsafe static Bitmap DecodeFromBytes(byte[] data, long length)
		{
			fixed (byte* value = data)
			{
				return DecodeFromPointer((IntPtr)value, length);
			}
		}

		private static Bitmap DecodeFromPointer(IntPtr data, long length)
		{
			int width = 0;
			int height = 0;
			if (NativeMethods.WebPGetInfo(data, (UIntPtr)(ulong)length, ref width, ref height) == 0)
			{
				throw new Exception("Invalid WebP header detected");
			}
			bool flag = false;
			Bitmap bitmap = null;
			BitmapData bitmapData = null;
			try
			{
				bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
				bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				IntPtr intPtr = NativeMethods.WebPDecodeBGRAInto(data, (UIntPtr)(ulong)length, bitmapData.Scan0, (UIntPtr)(ulong)(bitmapData.Stride * bitmapData.Height), bitmapData.Stride);
				if (bitmapData.Scan0 != intPtr)
				{
					throw new Exception("Failed to decode WebP image with error " + (long)intPtr);
				}
				flag = true;
			}
			finally
			{
				if (bitmapData != null)
				{
					bitmap.UnlockBits(bitmapData);
				}
				if (!flag)
				{
					bitmap?.Dispose();
				}
			}
			return bitmap;
		}

		private static void Encode(Bitmap from, Stream to, int quality)
		{
			Encode(from, quality, out var result, out var length);
			try
			{
				byte[] array = new byte[4096];
				for (int i = 0; i < length; i += array.Length)
				{
					int num = (int)Math.Min(array.Length, length - i);
					Marshal.Copy((IntPtr)((long)result + i), array, 0, num);
					to.Write(array, 0, num);
				}
			}
			finally
			{
				NativeMethods.WebPFree(result);
			}
		}

		private static void Encode(Bitmap b, float quality, out IntPtr result, out long length)
		{
			if (quality > 100f)
			{
				quality = 100f;
			}
			int width = b.Width;
			int height = b.Height;
			BitmapData bitmapData = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, b.PixelFormat);
			try
			{
				result = IntPtr.Zero;
				switch (b.PixelFormat)
				{
				case PixelFormat.Format32bppRgb:
				case PixelFormat.Format32bppArgb:
					if (quality <= 0f)
					{
						length = (long)(ulong)NativeMethods.WebPEncodeLosslessBGRA(bitmapData.Scan0, width, height, bitmapData.Stride, ref result);
					}
					else
					{
						length = (long)NativeMethods.WebPEncodeBGRA(bitmapData.Scan0, width, height, bitmapData.Stride, quality, ref result);
					}
					break;
				case PixelFormat.Format24bppRgb:
					if (quality <= 0f)
					{
						length = (long)(ulong)NativeMethods.WebPEncodeLosslessBGR(bitmapData.Scan0, width, height, bitmapData.Stride, ref result);
					}
					else
					{
						length = (long)(ulong)NativeMethods.WebPEncodeBGR(bitmapData.Scan0, width, height, bitmapData.Stride, quality, ref result);
					}
					break;
				default:
					throw new NotSupportedException("Only Format32bppArgb and Format32bppRgb bitmaps are supported");
				}
				if (length == 0L)
				{
					throw new Exception("WebP encode failed!");
				}
			}
			finally
			{
				b.UnlockBits(bitmapData);
			}
		}

		public static byte[] ConvertToJpeg(byte[] data)
		{
			if (!IsWebp(data))
			{
				return data;
			}
			try
			{
				using (Bitmap image = DecodeFromBytes(data, data.Length))
				{
					return image.ImageToJpegBytes();
				}
			}
			catch (Exception)
			{
				return data;
			}
		}

		public static bool IsWebp(byte[] data)
		{
			if (data.Length < 12)
			{
				return false;
			}
			for (int i = 0; i < header1.Length; i++)
			{
				if (data[i] != header1[i])
				{
					return false;
				}
			}
			for (int j = 0; j < header2.Length; j++)
			{
				if (data[j + 8] != header2[j])
				{
					return false;
				}
			}
			return true;
		}

		public static byte[] ConvertoToWebp(Bitmap bmp, int quality = 75)
		{
			if (bmp == null)
			{
				return null;
			}
			Bitmap bitmap = null;
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					bitmap = ((bmp.PixelFormat == PixelFormat.Format24bppRgb) ? bmp : bmp.CreateCopy(PixelFormat.Format24bppRgb));
					Encode(bitmap, memoryStream, quality);
					return memoryStream.ToArray();
				}
			}
			finally
			{
				if (bmp != bitmap)
				{
					bitmap.SafeDispose();
				}
			}
		}
	}
}
