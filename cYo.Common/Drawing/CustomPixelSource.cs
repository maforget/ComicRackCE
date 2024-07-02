using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using PhotoSauce.MagicScaler;

namespace cYo.Common.Drawing
{
    public class CustomPixelSource : BitmapPixelSource
    {
        private readonly Bitmap _bitmap;
        private readonly BitmapData _bitmapData;
        private readonly int dataLength;
        private byte[] byteArray = null;

        public CustomPixelSource(Bitmap bitmap, PixelFormat format = PixelFormat.Format32bppArgb)
            : base(ConvertPixelFormat(format), bitmap.Width, bitmap.Height, GetStride(bitmap))
        {
            _bitmap = bitmap;
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            _bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            dataLength = Math.Abs(base.Stride) * bitmap.Height;
        }

		private static Guid ConvertPixelFormat(PixelFormat format)
		{
			switch (format)
			{
				case PixelFormat.Format24bppRgb:
					return PixelFormats.Bgr24bpp;
                case PixelFormat.Format8bppIndexed:
                    return PixelFormats.Grey8bpp;
				default:
					return PixelFormats.Bgra32bpp;
			}
		}

        private static int GetStride(Bitmap bitmap)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData _bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                return _bitmapData.Stride;
            }
            finally
            {
                bitmap.UnlockBits(_bitmapData);
            }
        }

        private ReadOnlySpan<byte> GetSpan()
        {
            if (byteArray == null)
            {
                // Store bitmap data inside byte array
                byteArray = new byte[dataLength];

                // Copy the bitmap data to the byte array.
                System.Runtime.InteropServices.Marshal.Copy(_bitmapData.Scan0, byteArray, 0, dataLength);

                // Return a ReadOnlySpan<byte> from the byte array.
                return new ReadOnlySpan<byte>(byteArray); 
            }
            return byteArray;
        }

        protected override ReadOnlySpan<byte> Span => GetSpan();
    }
}

