using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using cYo.Common.Drawing;

namespace cYo.Common.Net
{
	public class WebImage : Component
	{
		public static string DefaultCacheLocation;

		private bool isLoading;

		private string name = "Image";

		private Uri uri;

		private string cacheLocation;

		private TimeSpan checkIntervall = new TimeSpan(7, 0, 0, 0);

		private volatile Bitmap image;

		[DefaultValue("Image")]
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		[DefaultValue(null)]
		public Uri Uri
		{
			get
			{
				return uri;
			}
			set
			{
				uri = value;
			}
		}

		[DefaultValue(null)]
		public string CacheLocation
		{
			get
			{
				return cacheLocation;
			}
			set
			{
				cacheLocation = value;
			}
		}

		[DefaultValue(null)]
		public Image DefaultImage
		{
			get;
			set;
		}

		public TimeSpan CheckIntervall
		{
			get
			{
				return checkIntervall;
			}
			set
			{
				checkIntervall = value;
			}
		}

		public Bitmap Image
		{
			get
			{
				return image;
			}
			protected set
			{
				image = value;
			}
		}

		public event EventHandler ImageLoaded;

		public WebImage()
		{
		}

		public WebImage(IContainer container)
		{
			container.Add(this);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && Image != null)
			{
				Image.Dispose();
			}
			base.Dispose(disposing);
		}

		public void LoadImage(string uri)
		{
			Uri = new Uri(uri);
			LoadImage();
		}

		public void LoadImage()
		{
			if (!CacheValid())
			{
				if (!isLoading)
				{
					isLoading = true;
					ThreadPool.QueueUserWorkItem(LoadWebImage);
				}
			}
			else
			{
				Image = GetCachedImage();
				OnImageLoaded();
			}
		}

		protected virtual void OnImageLoaded()
		{
			if (this.ImageLoaded != null)
			{
				this.ImageLoaded(this, EventArgs.Empty);
			}
		}

		private void LoadWebImage(object state)
		{
			try
			{
				HttpAccess httpAccess = new HttpAccess
				{
					AskProxyCredentials = false
				};
				using (Bitmap bitmap = (Bitmap)System.Drawing.Image.FromStream(httpAccess.GetStream(Uri)))
				{
					Bitmap bitmap2 = bitmap.CreateCopy(PixelFormat.Format32bppArgb);
					CacheImage(bitmap2);
					Image = bitmap2;
				}
				OnImageLoaded();
			}
			catch
			{
			}
			finally
			{
				isLoading = false;
			}
		}

		private string GetCacheLocation()
		{
			if (string.IsNullOrEmpty(cacheLocation))
			{
				return DefaultCacheLocation;
			}
			return cacheLocation;
		}

		private string GetCacheFilename()
		{
			return Path.Combine(GetCacheLocation(), Name);
		}

		private Bitmap GetCachedImage()
		{
			try
			{
				return (Bitmap)System.Drawing.Image.FromFile(GetCacheFilename());
			}
			catch
			{
				return null;
			}
		}

		private bool CacheValid()
		{
			try
			{
				FileInfo fileInfo = new FileInfo(GetCacheFilename());
				return DateTime.Now - fileInfo.LastWriteTime < checkIntervall;
			}
			catch
			{
				return false;
			}
		}

		private void CacheImage(Image image)
		{
			try
			{
				image.Save(GetCacheFilename(), ImageFormat.Png);
			}
			catch
			{
			}
		}
	}
}
