using System;
using System.Drawing;
using cYo.Common.ComponentModel;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine.IO
{
	public class ComicBookImageProvider : DisposableObject, IImageProvider, IDisposable
	{
		private IImageProvider provider;

		private IImageProvider ownProvider;

		private readonly ComicBook comic;

		private readonly int lastPageIndex;

		public bool IsSlow
		{
			get
			{
				CheckProvider();
				return provider.IsSlow;
			}
		}

		public string Source
		{
			get
			{
				CheckProvider();
				return provider.Source;
			}
		}

		public int Count
		{
			get
			{
				CheckProvider();
				return provider.Count;
			}
		}

		public ComicBookImageProvider(ComicBook comic, IImageProvider provider, int lastPageIndex)
		{
			this.comic = comic;
			this.provider = provider;
			this.lastPageIndex = lastPageIndex;
		}

		public Bitmap GetImage(int index)
		{
			CheckProvider();
			return provider.GetImage(index);
		}

		public byte[] GetByteImage(int index)
		{
			CheckProvider();
			return provider.GetByteImage(index);
		}

		public ExportImageContainer GetByteImageForExport(int index)
		{
			CheckProvider();
			return provider.GetByteImageForExport(index);
		}

		public ProviderImageInfo GetImageInfo(int index)
		{
			CheckProvider();
			return provider.GetImageInfo(index);
		}

		public ThumbnailImage GetThumbnail(int index)
		{
			CheckProvider();
			return provider.GetThumbnail(index);
		}

		private void CheckProvider()
		{
			if (provider == null && ownProvider == null)
			{
				provider = (ownProvider = comic.OpenProvider(lastPageIndex));
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && ownProvider != null)
			{
				ownProvider.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
