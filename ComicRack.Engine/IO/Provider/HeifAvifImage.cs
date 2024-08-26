using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibHeifSharp;
using cYo.Common.Drawing;
using cYo.Common.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
    public static class HeifAvifImage
    {
        public static class NativeMethods
        {
            public enum heif_filetype_result
            {
                heif_filetype_no,
                heif_filetype_yes_supported,   // it is heif and can be read by libheif
                heif_filetype_yes_unsupported, // it is heif, but cannot be read by libheif
                heif_filetype_maybe // not sure whether it is an heif, try detection with more input data
            };

            [DllImport("libheif", CallingConvention = CallingConvention.Cdecl)]
            public static extern heif_filetype_result heif_check_filetype(IntPtr data, int len);
        }

		public static byte[] ConvertToJpeg(byte[] data)
        {
            if (!IsSupported(data) || !IsSupportedNative(data))
            {
                return data;
            }
            try
            {
                using (Bitmap image = DecodeFromBytes(data))
                {
                    return image.ImageToJpegBytes();
                }
            }
            catch (Exception)
            {
                return data;
            }
        }

        public static byte[] ConvertToHeif(Bitmap bmp, int quality = 40, bool avif = false)
        {
            if (bmp == null || !Environment.Is64BitProcess)
            {
                return null;
            }
            Bitmap bitmap = null;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    bitmap = ((bmp.PixelFormat == PixelFormat.Format24bppRgb) ? bmp : bmp.CreateCopy(PixelFormat.Format24bppRgb));
                    Encode(bitmap, memoryStream, quality, avif);
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

		#region Checking file header for identification
		private static bool IsSupported(byte[] data)
        {
			if (data.Length < 12 || !Environment.Is64BitProcess)
				return false;

			// Check bytes 4 to 7
			string typeTag = Encoding.ASCII.GetString(data, 4, 4);
			if (typeTag != "ftyp")
				return false;

			// Check bytes 8 to 11
			string fileType = Encoding.ASCII.GetString(data, 8, 4);
			var supportedTypes = new[] { "heic", "heix", "avif", "jpeg", "j2ki" };

			return Array.Exists(supportedTypes, t => t == fileType);
		}

		private static bool IsSupportedNative(byte[] data)
        {
            if (data.Length < 12 || !Environment.Is64BitProcess)
                return false;

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr dataPtr = handle.AddrOfPinnedObject();

            try
            {
                NativeMethods.heif_filetype_result result = NativeMethods.heif_check_filetype(dataPtr, data.Length);

                if (result == NativeMethods.heif_filetype_result.heif_filetype_yes_supported)
                    return true;

                return false;
            }
            finally
            {
                handle.Free();
            }
        } 
        #endregion

        #region HEIF // AVIF Decoding
        public static Bitmap DecodeFromBytes(byte[] data)
        {
            try
            {
                var decodingOptions = new HeifDecodingOptions
                {
                    ConvertHdrToEightBit = true,
                    Strict = false,
                    DecoderId = null
                };

                using HeifContext context = new HeifContext(data);
                using HeifImageHandle primaryImage = context.GetPrimaryImageHandle();

                bool hasAlpha = primaryImage.HasAlphaChannel;
                HeifChroma chroma = hasAlpha ? HeifChroma.InterleavedRgba32 : HeifChroma.InterleavedRgb24;

                using HeifImage image = primaryImage.Decode(HeifColorspace.Rgb, chroma, decodingOptions);
                return ConvertToBitmap(image);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        private static unsafe Bitmap ConvertToBitmap(HeifImage image)
        {
            Bitmap bmp = null;
            BitmapData bmpData = null;

            try
            {
                int width = image.Width;
                int height = image.Height;

                bool hasAlpha = image.HasAlphaChannel;
                PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
                bmp = new Bitmap(width, height, pixelFormat);
                bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);

                HeifPlaneData heifPlaneData = image.GetPlane(HeifChannel.Interleaved);
                byte* srcScan0 = (byte*)heifPlaneData.Scan0;
                byte* dstScan0 = (byte*)bmpData.Scan0;

                if (image.IsPremultipliedAlpha)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int srcPos = y * heifPlaneData.Stride + x * 4;
                            int dstPos = y * bmpData.Stride + x * 4;

                            byte alpha = srcScan0[srcPos + 3];

                            if (alpha == 0)
                            {
                                dstScan0[dstPos + 0] = 0;
                                dstScan0[dstPos + 1] = 0;
                                dstScan0[dstPos + 2] = 0;
                                dstScan0[dstPos + 3] = 0;
                            }
                            else if (alpha == 0xff)
                            {
                                dstScan0[dstPos + 0] = srcScan0[srcPos + 2];//B
                                dstScan0[dstPos + 1] = srcScan0[srcPos + 1];//G
                                dstScan0[dstPos + 2] = srcScan0[srcPos + 0];//R
                                dstScan0[dstPos + 3] = 0xff;// srcScan0[pos + 0];//A
                            }
                            else
                            {
                                dstScan0[dstPos + 0] = (byte)Math.Min(MathF.Round(srcScan0[srcPos + 2] * 255f / alpha), 255);
                                dstScan0[dstPos + 1] = (byte)Math.Min(MathF.Round(srcScan0[srcPos + 1] * 255f / alpha), 255);
                                dstScan0[dstPos + 2] = (byte)Math.Min(MathF.Round(srcScan0[srcPos + 0] * 255f / alpha), 255);
                                dstScan0[dstPos + 3] = alpha;
                            }
                        }
                    }
                }
                else
                {
                    CopyBits(width, height, heifPlaneData.Stride, bmpData.Stride, srcScan0, dstScan0, hasAlpha);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (bmp != null && bmpData != null)
                {
                    bmp.UnlockBits(bmpData);
                }
            }
            return bmp;
        }
        #endregion

        #region HEIF // ABIF Encoding
        public static void Encode(Bitmap bmp, MemoryStream stream, int quality, bool avif = false)
        {
            var format = avif ? HeifCompressionFormat.Av1 : HeifCompressionFormat.Hevc;

            var encodingOptions = new HeifEncodingOptions
            {
                SaveAlphaChannel = true,
                WriteTwoColorProfiles = false
            };

            using HeifContext context = new HeifContext();
            HeifEncoderDescriptor encoderDescriptor = context.GetEncoderDescriptors(format).FirstOrDefault();
            using HeifEncoder encoder = context.GetEncoder(encoderDescriptor);
            encoder.SetLossyQuality(quality);

            using HeifImage heifImage = CreateHeifImage(bmp);
            context.EncodeImage(heifImage, encoder, encodingOptions);
            context.WriteToStream(stream);
        }

        private static HeifImage CreateHeifImage(Bitmap image)
        {
            HeifColorspace colorspace = HeifColorspace.Rgb;

            HeifImage heifImage = null;
            HeifImage temp = null;

            try
            {
                HeifChroma chroma = HeifChroma.InterleavedRgba32;//It seems all output are 32bit anyway, and the CopyRgba assumes destination is 32bit

                temp = new HeifImage(image.Width, image.Height, colorspace, chroma);
                temp.AddPlane(HeifChannel.Interleaved, image.Width, image.Height, 8);

                CopyRgba(image, temp);

                heifImage = temp;
                temp = null;
            }
            finally
            {
                temp?.Dispose();
            }

            return heifImage;
        }

        private static unsafe void CopyRgba(Bitmap inputBitmap, HeifImage heifImage)
        {
            BitmapData bmpData = null;

            try
            {
                int width = inputBitmap.Width;
                int height = inputBitmap.Height;

                bool inputHasAlpha = ((ImageFlags)inputBitmap.Flags & ImageFlags.HasAlpha) != 0;
                PixelFormat pixelFormat = inputHasAlpha ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb;
                bmpData = inputBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);

                HeifPlaneData heifPlaneData = heifImage.GetPlane(HeifChannel.Interleaved);
                byte* srcScan0 = (byte*)bmpData.Scan0;
                byte* dstScan0 = (byte*)heifPlaneData.Scan0;

                CopyBits(width, height, bmpData.Stride, heifPlaneData.Stride, srcScan0, dstScan0, inputHasAlpha);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (inputBitmap != null && bmpData != null)
                {
                    inputBitmap.UnlockBits(bmpData);
                }
            }
        }
        #endregion

        #region Common
        private static unsafe void CopyBits(int width, int height, int srcStride, int dstStride, byte* srcScan0, byte* dstScan0, bool inputHasAlpha)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int srcBitDepth = inputHasAlpha ? 4 : 3;//Input bit depth
                    int srcPos = y * srcStride + x * srcBitDepth;
                    int dstPos = y * dstStride + x * 4;//Change to 3 if output isnt 32bit

                    dstScan0[dstPos + 0] = srcScan0[srcPos + 2];//B
                    dstScan0[dstPos + 1] = srcScan0[srcPos + 1];//G
                    dstScan0[dstPos + 2] = srcScan0[srcPos + 0];//R
                    dstScan0[dstPos + 3] = inputHasAlpha ? srcScan0[srcPos + 3] : (byte)0xff;//A //Remove if output isn't 32bit
                }
            }
        } 
        #endregion
    }
}
