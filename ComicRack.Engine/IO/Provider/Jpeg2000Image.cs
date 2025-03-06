using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.Drawing;
using cYo.Common.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using cYo.Common.Collections;
using CSJ2K;
using CSJ2K.Util;
using CSJ2K.j2k.util;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public static class Jpeg2000Image
	{
		static Jpeg2000Image()
		{
			BitmapImageCreator.Register();
		}

		public static byte[] ConvertToJpeg2000(Bitmap bmp, int quality = 40, bool isJp2 = true)
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
					Encode(bitmap, memoryStream, quality, isJp2);
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

		public static byte[] ConvertToJpeg(byte[] data)
		{
			if (!IsValidJpeg2000(data))
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

		private static bool IsValidJpeg2000(byte[] data)
		{
			if (data.Length >= 12)
			{
				// Check for JP2 header
				byte[] jp2Signature = { 0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20, 0x0D, 0x0A, 0x87, 0x0A };
				if (data.Take(12).SequenceEqual(jp2Signature))
					return true;
			}

			if (data.Length >= 2)
			{
				// Check for J2K codestream header
				byte[] j2kSignature = { 0xFF, 0x4F };
				if (data.Take(2).SequenceEqual(j2kSignature))
					return true;
			}

			return false;
		}


		private static Bitmap DecodeFromBytes(byte[] data)
		{
			return J2kImage.FromBytes(data).As<Bitmap>();
		}

		private static void Encode(Bitmap bitmap, MemoryStream memoryStream, int? quality, bool isJP2 = true)
		{
			//Image Gamma is off.
			ParameterList pList = J2kImage.GetDefaultEncoderParameterList(null);
			if (!quality.HasValue)
				pList["lossless"] = "on";
			else
				pList["rate"] = quality.ToString();

			pList["file_format"] = isJP2 ? "on" : "off"; //Sets the format to jp2 when "on" or j2k if "off"

			byte[] data = J2kImage.ToBytes(bitmap, pList);
			memoryStream.Write(data, 0, data.Length);
		}
	}
}