using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;
using cYo.Common.Runtime;

namespace cYo.Projects.ComicRack.Engine
{
	public class EngineConfiguration
	{
		public enum CbEngines
		{
			SevenZip,
			SevenZipExe,
			SharpCompress,
			SharpZip
		}

		private string tempPath = Path.GetTempPath();

		private float pageBowWidth = 0.07f;

		private int pageBowFromAlpha = 92;

		private int pageBowToAlpha;

		private int maximumQueueThreads = 4;

		private static EngineConfiguration defaultConfig;

		[DefaultValue(true)]
		public bool EnableParallelQueries
		{
			get;
			set;
		}

		public bool IsEnableParallelQueriesDefault => EnableParallelQueries;

		[DefaultValue(null)]
		public string IgnoredArticles
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string OfValues
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool LegacyFilenameParser
		{
			get;
			set;
		}

		[DefaultValue(1000)]
		public int PageScrollingDuration
		{
			get;
			set;
		}

		[DefaultValue(300)]
		public int AnimationDuration
		{
			get;
			set;
		}

		[DefaultValue(250)]
		public int BlendDuration
		{
			get;
			set;
		}

		[DefaultValue(1000)]
		public int SoftwareFilterDelay
		{
			get;
			set;
		}

		[DefaultValue(typeof(Size), "512, \u00b4512")]
		public Size ListCoverSize
		{
			get;
			set;
		}

		[DefaultValue(0.3f)]
		public float ListCoverAlpha
		{
			get;
			set;
		}

		[DefaultValue(0.9f)]
		public float NavigationPanelWidth
		{
			get;
			set;
		}

		public string TempPath
		{
			get
			{
				return tempPath;
			}
			set
			{
				if (Directory.Exists(value))
				{
					tempPath = value;
				}
			}
		}

		[TypeConverter(typeof(ArrayConverter<Color>))]
		[DefaultValue(typeof(Color[]), "Orange, Green, Red, Blue")]
		public Color[] BookmarkColors
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool CacheThumbnailPages
		{
			get;
			set;
		}

		[DefaultValue(BitmapResampling.FastBilinear)]
		public BitmapResampling ThumbnailResampling
		{
			get;
			set;
		}

		[DefaultValue(60)]
		public int ThumbnailQuality
		{
			get;
			set;
		}

		[DefaultValue(BitmapResampling.GdiPlusHQ)]
		public BitmapResampling ExportResampling
		{
			get;
			set;
		}

		[DefaultValue(BitmapResampling.GdiPlus)]
		public BitmapResampling SyncResamping
		{
			get;
			set;
		}

		[DefaultValue(BitmapResampling.GdiPlusHQ)]
		public BitmapResampling SoftwareFilter
		{
			get;
			set;
		}

		[DefaultValue(ComicBook.DefaultCaptionFormat)]
		public string ComicCaptionFormat
		{
			get;
			set;
		}

		[DefaultValue(ComicBook.DefaultComicExportFileNameFormat)]
		public string ComicExportFileNameFormat
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool DisableGhostscript
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string GhostscriptExecutable
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string DjVuLibreInstall
		{
			get;
			set;
		}

		[DefaultValue(typeof(Size), "2000, 2000")]
		public Size DjVuSizeLimit
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool MirroredPageTurnAnimation
		{
			get;
			set;
		}

		[DefaultValue(0.07f)]
		public float PageBowWidth
		{
			get
			{
				return pageBowWidth;
			}
			set
			{
				pageBowWidth = value.Clamp(0.01f, 0.5f);
			}
		}

		[DefaultValue(92)]
		public int PageBowFromAlpha
		{
			get
			{
				return pageBowFromAlpha;
			}
			set
			{
				pageBowFromAlpha = value.Clamp(0, 255);
			}
		}

		[DefaultValue(0)]
		public int PageBowToAlpha
		{
			get
			{
				return pageBowToAlpha;
			}
			set
			{
				pageBowToAlpha = value.Clamp(0, 255);
			}
		}

		[DefaultValue(typeof(Color), "Black")]
		public Color PageBowColor
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool PageBowCenter
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool PageBowBorder
		{
			get;
			set;
		}

		[DefaultValue(0.05f)]
		public float SoftwareFilterMinScale
		{
			get;
			set;
		}

		[DefaultValue(4)]
		public int MaximumQueueThreads
		{
			get
			{
				return maximumQueueThreads;
			}
			set
			{
				maximumQueueThreads = value.Clamp(1, 32);
			}
		}

		[DefaultValue(1)]
		public float PageShadowWidthPercentage
		{
			get;
			set;
		}

		[DefaultValue(0.6f)]
		public float PageShadowOpacity
		{
			get;
			set;
		}

		[DefaultValue(14)]
		public int IsRecentInDays
		{
			get;
			set;
		}

		[DefaultValue(95)]
		public int IsReadCompletionPercentage
		{
			get;
			set;
		}

		[DefaultValue(10)]
		public int IsNotReadCompletionPercentage
		{
			get;
			set;
		}

		[DefaultValue(300)]
		public int OperationTimeout
		{
			get;
			set;
		}

		[DefaultValue(100)]
		public int ServerProviderCacheSize
		{
			get;
			set;
		}

		[DefaultValue(80)]
		public int GestureAreaSize
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool ShowGestureHint
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool HtmlInfoContextMenu
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool EnableHtmlScriptErrors
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool HideVisiblePartOverlayClose
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool AeroFullScreenWorkaround
		{
			get;
			set;
		}

		[DefaultValue(typeof(Color), "Empty")]
		public Color ThumbnailPageCurlColor
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool ThumbnailPageBow
		{
			get;
			set;
		}

		[DefaultValue(typeof(Color), "White")]
		public Color BlankPageColor
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool SearchBrowserCaseSensitive
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool RatingStarsBelowThumbnails
		{
			get;
			set;
		}

		public int SyncOptimizeQuality
		{
			get;
			set;
		}

		public int SyncOptimizeMaxHeight
		{
			get;
			set;
		}

		public bool SyncOptimizeSharpen
		{
			get;
			set;
		}

		public bool SyncWebP
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool SyncOptimizeWebP
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool SyncCreateThumbnails
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string ExtraWifiDeviceAddresses
		{
			get;
			set;
		}

		[DefaultValue(50)]
		public int SyncQueueLength
		{
			get;
			set;
		}

		[DefaultValue(1)]
		public int SyncKeepReadComics
		{
			get;
			set;
		}

		[DefaultValue(1000)]
		public int PageCachingDelay
		{
			get;
			set;
		}

		[DefaultValue(CbEngines.SevenZip)]
		public CbEngines CbzUses
		{
			get;
			set;
		}

		[DefaultValue(CbEngines.SevenZip)]
		public CbEngines CbrUses
		{
			get;
			set;
		}

		[DefaultValue(CbEngines.SevenZip)]
		public CbEngines Cb7Uses
		{
			get;
			set;
		}

		[DefaultValue(CbEngines.SevenZip)]
		public CbEngines CbtUses
		{
			get;
			set;
		}

		[DefaultValue(128)]
		public int FreeDeviceMemoryMB
		{
			get;
			set;
		}

		[DefaultValue(4)]
		public int ParallelConversions
		{
			get;
			set;
		}

		[DefaultValue(5000)]
		public int WifiSyncReceiveTimeout
		{
			get;
			set;
		}

		[DefaultValue(5000)]
		public int WifiSyncSendTimeout
		{
			get;
			set;
		}

		[DefaultValue(2500)]
		public int WifiSyncConnectionTimeout
		{
			get;
			set;
		}

		[DefaultValue(1)]
		public int WifiSyncConnectionRetries
		{
			get;
			set;
		}

		public static EngineConfiguration Default => defaultConfig ?? (defaultConfig = IniFile.Default.Register<EngineConfiguration>());

		public EngineConfiguration()
		{
			PageScrollingDuration = 1000;
			AnimationDuration = 250;
			BlendDuration = 400;
			SoftwareFilterDelay = 1000;
			ListCoverSize = new Size(512, 512);
			ListCoverAlpha = 0.3f;
			NavigationPanelWidth = 0.9f;
			BookmarkColors = new Color[4]
			{
				Color.Orange,
				Color.Green,
				Color.Red,
				Color.Blue
			};
			ThumbnailResampling = BitmapResampling.FastBilinear;
			ThumbnailQuality = 60;
			ThumbnailPageBow = true;
			ExportResampling = BitmapResampling.GdiPlusHQ;
			SyncResamping = BitmapResampling.GdiPlus;
			SoftwareFilter = BitmapResampling.GdiPlusHQ;
			ComicCaptionFormat = ComicBook.DefaultCaptionFormat;
			ComicExportFileNameFormat = ComicBook.DefaultComicExportFileNameFormat;
			PageBowColor = Color.Black;
			PageBowCenter = true;
			PageBowBorder = true;
			SoftwareFilterMinScale = 0.05f;
			PageShadowWidthPercentage = 1f;
			PageShadowOpacity = 0.6f;
			IsRecentInDays = 14;
			IsReadCompletionPercentage = 95;
			IsNotReadCompletionPercentage = 10;
			OperationTimeout = 300;
			ServerProviderCacheSize = 100;
			DjVuSizeLimit = new Size(2000, 2000);
			GestureAreaSize = 80;
			ShowGestureHint = true;
			AeroFullScreenWorkaround = true;
			BlankPageColor = Color.White;
			RatingStarsBelowThumbnails = true;
			EnableParallelQueries = true;
			SyncOptimizeQuality = 65;
			SyncOptimizeMaxHeight = 1500;
			SyncOptimizeSharpen = false;
			SyncQueueLength = 50;
			SyncCreateThumbnails = true;
			SyncOptimizeWebP = true;
			SyncKeepReadComics = 1;
			PageCachingDelay = 1000;
			CbzUses = (CbrUses = (Cb7Uses = (CbtUses = CbEngines.SevenZip)));
			FreeDeviceMemoryMB = 128;
			ParallelConversions = 32;
			WifiSyncReceiveTimeout = 5000;
			WifiSyncSendTimeout = 5000;
			WifiSyncConnectionTimeout = 2500;
			WifiSyncConnectionRetries = 1;
		}

		public string GetTempFileName()
		{
			return Path.Combine(TempPath, string.Concat(Guid.NewGuid(), ".tmp"));
		}
	}
}
