using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Win32;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public static class DjVuImage
	{
		private static readonly string encodeExe = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\c44.exe");

		private static readonly string unpackExe = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\ddjvu.exe");

		private static Size SizeLimit => EngineConfiguration.Default.DjVuSizeLimit;

		public static byte[] ConvertToJpeg(byte[] data)
		{
			if (!IsDjvu(data))
			{
				return data;
			}
			string tempFileName = Path.GetTempFileName();
			try
			{
				File.WriteAllBytes(tempFileName, data);
				using (Bitmap image = GetBitmap(tempFileName))
				{
					return image.ImageToJpegBytes();
				}
			}
			catch (Exception)
			{
				return data;
			}
			finally
			{
				FileUtility.SafeDelete(tempFileName);
			}
		}

		public static byte[] ConvertToDjVu(Bitmap bmp)
		{
			string tempFileName = Path.GetTempFileName();
			string tempFileName2 = Path.GetTempFileName();
			try
			{
				SaveDjVu(bmp, tempFileName2);
				return File.ReadAllBytes(tempFileName2);
			}
			finally
			{
				FileUtility.SafeDelete(tempFileName);
				FileUtility.SafeDelete(tempFileName2);
			}
		}

		public static void SaveDjVu(Bitmap bmp, string encodedFile)
		{
			string tempFileName = Path.GetTempFileName();
			try
			{
				bmp.SaveJpeg(tempFileName);
				if (ExecuteProcess.Execute(encodeExe, $"\"{tempFileName}\" \"{encodedFile}\"", ExecuteProcess.Options.None).ExitCode != 0)
				{
					throw new InvalidDataException();
				}
			}
			catch (Exception)
			{
				FileUtility.SafeDelete(encodedFile);
				throw;
			}
			finally
			{
				FileUtility.SafeDelete(tempFileName);
			}
		}

		public static Bitmap GetBitmap(string source, int index = 0)
		{
			string tempFileName = Path.GetTempFileName();
			try
			{
				ExecuteProcess.Result result = ExecuteProcess.Execute(unpackExe, $"-format=tiff -size={SizeLimit.Width}x{SizeLimit.Height} -page={index + 1} \"{source}\" \"{tempFileName}\"", ExecuteProcess.Options.None);
				if (result.ExitCode != 0)
				{
					throw new FileLoadException();
				}
				return BitmapExtensions.BitmapFromFile(tempFileName);
			}
			finally
			{
				FileUtility.SafeDelete(tempFileName);
			}
		}

		public static bool IsDjvu(string uri)
		{
			return uri.EndsWith(".djvu", StringComparison.OrdinalIgnoreCase);
		}

		public static bool IsDjvu(byte[] sig)
		{
			if (sig == null)
			{
				return false;
			}
			try
			{
				return sig[0] == 65 && sig[1] == 84 && sig[2] == 38 && sig[3] == 84 && sig[4] == 70;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
