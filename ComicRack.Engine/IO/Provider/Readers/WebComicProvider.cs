using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Net;
using cYo.Common.Text;
using cYo.Common.Xml;
using cYo.Projects.ComicRack.Engine.IO.Cache;
using static System.Net.Mime.MediaTypeNames;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	[FileFormat("eComic (WebComic)", KnownFileFormats.CBW, ".cbw", EnableUpdate = true, Dynamic = true)]
	public class WebComicProvider : ComicProvider, IDynamicImages
	{
		private readonly List<WebComic.WebComicImage> images = new List<WebComic.WebComicImage>();

		public override bool IsSlow => true;

		public override ImageProviderCapabilities Capabilities => ImageProviderCapabilities.FastFormatCheck;

		public bool RefreshMode
		{
			get;
			set;
		}

		public WebComicProvider()
		{
			base.DisableNtfs = true;
			base.DisableSidecar = true;
		}

		protected override bool IsSupportedImage(string file)
		{
			return true;
		}

		protected override bool OnFastFormatCheck(string source)
		{
			if (!source.EndsWith(".cbw", StringComparison.OrdinalIgnoreCase))
				return false;

			return Load(source) != null;
		}

		public override string CreateHash()
		{
			using (FileStream inputStream = File.OpenRead(base.Source))
			{
				return Base32.ToBase32String(new SHA1Managed().ComputeHash(inputStream));
			}
		}

		protected override void OnParse()
		{
			WebComic webComic = Load(base.Source);
			if (webComic == null)
			{
				return;
			}
			webComic.Variables.Add(new ValuePair<string, string>("ComicFileName", Path.GetFileName(base.Source)));
			webComic.Variables.Add(new ValuePair<string, string>("ComicFilePath", "file://" + Path.GetDirectoryName(base.Source)));
			images.Clear();
			foreach (WebComic.WebComicImage parsedImage in webComic.GetParsedImages(RefreshMode))
			{
				images.Add(parsedImage);
				if (!FireIndexReady(new ProviderImageInfo(images.Count - 1, parsedImage.Name, 0L)))
				{
					break;
				}
			}
		}

		protected override byte[] OnRetrieveSourceByteImage(int index)
		{
			WebComic.WebComicImage webComicImage = images[index];
			if (webComicImage.Compositing.IsEmpty)
			{
				return RetrieveSourceByteImage(webComicImage.Urls[0].Url, RefreshMode);
			}
			WebComic.PageCompositing compositing = webComicImage.Compositing;
			var array = (from uri in webComicImage.Urls
				select new
				{
					Uri = uri,
					Bitmap = BitmapFromBytes(uri.Url, RefreshMode)
				} into bmp
				where bmp.Bitmap != null
				select bmp).ToArray();
			Bitmap bitmap = null;
			try
			{
				if (compositing.PageSize.IsEmpty)
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					while (true)
					{
						if (num6 % compositing.Columns == 0 || num6 >= array.Length)
						{
							num = Math.Max(num, num4);
							num2 += num3;
							num3 = 0;
							num4 = 0;
							num5 = 0;
							if (num6 >= array.Length)
							{
								break;
							}
						}
						Bitmap bitmap2 = array[num6].Bitmap;
						num3 = Math.Max(num3, bitmap2.Height);
						num4 += bitmap2.Width;
						num6++;
						num5++;
					}
					int num7 = (int)(Math.Sqrt(num2 * num2 + num * num) * (double)compositing.BorderWidth / 100.0);
					bitmap = new Bitmap(num + num7 * 2, num2 + num7 * 2);
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.Clear(compositing.BackColor);
						int num8 = num7;
						int num9 = num7;
						num5 = 0;
						num3 = 0;
						var array2 = array;
						foreach (var anon in array2)
						{
							int x = (compositing.RightToLeft ? (num - anon.Bitmap.Width - num8) : num8);
							graphics.DrawImage(anon.Bitmap, x, num9, anon.Bitmap.Width, anon.Bitmap.Height);
							num8 += anon.Bitmap.Width;
							num3 = Math.Max(num3, anon.Bitmap.Height);
							if (++num5 >= compositing.Columns)
							{
								num8 = num7;
								num9 += num3;
								num3 = 0;
								num5 = 0;
							}
						}
					}
				}
				else
				{
					int pageWidth = compositing.PageWidth;
					int pageHeight = compositing.PageHeight;
					int num10 = (int)(Math.Sqrt(pageHeight * pageHeight + pageWidth * pageWidth) * (double)compositing.BorderWidth / 100.0);
					bitmap = new Bitmap(pageWidth + num10 * 2, pageHeight + num10 * 2);
					Rectangle rectangle = array.Select(bmp => new Rectangle(bmp.Uri.Left, bmp.Uri.Top, bmp.Bitmap.Width, bmp.Bitmap.Height)).Aggregate(default(Rectangle), (Rectangle current, Rectangle rb) => (!current.IsEmpty) ? Rectangle.Union(current, rb) : rb);
					int num11 = (bitmap.Width - rectangle.Width) / 2;
					int num12 = (bitmap.Height - rectangle.Height) / 2;
					using (Graphics graphics2 = Graphics.FromImage(bitmap))
					{
						graphics2.Clear(compositing.BackColor);
						var array3 = array;
						foreach (var anon2 in array3)
						{
							int x2 = (compositing.RightToLeft ? (pageWidth - num11 - anon2.Uri.Left - anon2.Bitmap.Width) : (num11 + anon2.Uri.Left));
							int y = num12 + anon2.Uri.Top;
							graphics2.DrawImage(anon2.Bitmap, x2, y, anon2.Bitmap.Width, anon2.Bitmap.Height);
						}
					}
				}
				return bitmap.ImageToBytes(ImageFormat.Jpeg);
			}
			catch (Exception)
			{
				return null;
			}
			finally
			{
				array.Select(bmp => bmp.Bitmap).Dispose();
				bitmap.SafeDispose();
			}
		}

		protected override ComicInfo OnLoadInfo()
		{
			try
			{
				return Load(base.Source).Info;
			}
			catch
			{
				return null;
			}
		}

		protected override bool OnStoreInfo(ComicInfo comicInfo)
		{
			try
			{
				WebComic webComic = Load(base.Source);
				webComic.Info = comicInfo;
				using (FileStream s = File.Create(base.Source))
				{
					XmlUtility.Store(s, webComic, compressed: false);
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static Bitmap BitmapFromBytes(string url, bool refresh)
		{
			try
			{
				return BitmapExtensions.BitmapFromBytes(RetrieveSourceByteImage(url, refresh));
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static byte[] RetrieveSourceByteImage(string uri, bool refreshMode)
		{
			try
			{
				byte[] item;
				if (FileCache.Default != null && !refreshMode)
				{
					item = FileCache.Default.GetItem(uri);
					if (item != null)
					{
						return item;
					}
				}
				item = HttpAccess.ReadBinary(uri);
				if (item != null && FileCache.Default != null)
				{
					FileCache.Default.AddItem(uri, item);
				}
				return item;
			}
			catch
			{
				return null;
			}
		}

		private static WebComic Load(string file)
		{
			try
			{
				return XmlUtility.Load<WebComic>(file);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
