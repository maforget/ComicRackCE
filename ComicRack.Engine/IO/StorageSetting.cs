using System;
using System.ComponentModel;
using System.Drawing;
using cYo.Common.Drawing;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine.IO
{
	[Serializable]
	public class StorageSetting
	{
		[DefaultValue(0)]
		public int FormatId
		{
			get;
			set;
		}

		[DefaultValue(ExportCompression.None)]
		public ExportCompression ComicCompression
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool EmbedComicInfo
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool RemovePages
		{
			get;
			set;
		}

		[DefaultValue(ComicPageType.Deleted)]
		public ComicPageType RemovePageFilter
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string IncludePages
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool IgnoreErrorPages
		{
			get;
			set;
		}

		[DefaultValue(StoragePageType.Original)]
		public StoragePageType PageType
		{
			get;
			set;
		}

		[DefaultValue(75)]
		public int PageCompression
		{
			get;
			set;
		}

		[DefaultValue(StoragePageResize.Original)]
		public StoragePageResize PageResize
		{
			get;
			set;
		}

		[DefaultValue(1000)]
		public int PageWidth
		{
			get;
			set;
		}

		[DefaultValue(1000)]
		public int PageHeight
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool DontEnlarge
		{
			get;
			set;
		}

		[DefaultValue(DoublePageHandling.Keep)]
		public DoublePageHandling DoublePages
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool AddKeyToPageInfo
		{
			get;
			set;
		}

		[DefaultValue(BitmapResampling.GdiPlusHQ)]
		public BitmapResampling Resampling
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool KeepOriginalImageNames
		{
			get;
			set;
		}

		[DefaultValue(typeof(BitmapAdjustment), "0,0,0")]
		public BitmapAdjustment ImageProcessing
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool CreateThumbnails
		{
			get;
			set;
		}

		[DefaultValue(typeof(Size), "0, 256")]
		public Size ThumbnailSize
		{
			get;
			set;
		}

		public StorageSetting()
		{
			DontEnlarge = true;
			PageHeight = 1000;
			PageWidth = 1000;
			PageResize = StoragePageResize.Original;
			PageCompression = 75;
			PageType = StoragePageType.Original;
			RemovePages = true;
			RemovePageFilter = ComicPageType.Deleted;
			EmbedComicInfo = true;
			ComicCompression = ExportCompression.None;
			ThumbnailSize = new Size(0, 256);
			DoublePages = DoublePageHandling.Keep;
			Resampling = EngineConfiguration.Default.ExportResampling;
		}

		public bool IsValidPage(int page)
		{
			return IncludePages.TestRangeString(page + 1);
		}
	}
}
