using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using cYo.Common;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine.Properties;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	public class ThumbRenderer
	{
		public static float DefaultStateOverlap = 0.8f;

		public static Bitmap DefaultRatingImage1 = new Bitmap(16, 16);

		public static Bitmap DefaultRatingImage2 = new Bitmap(16, 16);

		public static Bitmap DefaultTagRatingImage1 = new Bitmap(16, 16);

		public static Bitmap DefaultTagRatingImage2 = new Bitmap(16, 16);

		public static Image DefaultPageCurlImage = Resources.PageCurl;

		public static Image DefaultPageCurlShadowImage = Resources.PageCurlShadow.ToOptimized();

		public static float DefaultThumbnailAspect = 2f / 3f;

		public static Color[] DefaultBookmarkColors = new Color[4]
		{
			Color.Orange,
			Color.Green,
			Color.Red,
			Color.Blue
		};

		public static Bitmap[] DefaultNewPagesImages = new Bitmap[6]
		{
			Resources.NewPages1.ToOptimized(),
			Resources.NewPages2.ToOptimized(),
			Resources.NewPages3.ToOptimized(),
			Resources.NewPages4.ToOptimized(),
			Resources.NewPages5.ToOptimized(),
			Resources.NewPages5Plus.ToOptimized()
		};

		private Color selectionBackColor = ThemeColors.ThumbRenderer.SelectionBack;

		private List<Image> stateImages;

		private Rectangle thumbnailBounds = Rectangle.Empty;

		private RatingRenderer ratingStripRenderer;

		private static readonly Bitmap pageBowLeft = PageRendering.CreatePageBow(new Size(32, 9), 180f).ToOptimized();

		private static readonly Bitmap pageBowRight = PageRendering.CreatePageBow(new Size(32, 9), 0f).ToOptimized();

		private static readonly string loadingThumbnail = TR.Messages["loadingThumbnail", "Loading Thumbnail..."];

		private static Color cachedPageCurlColor;

		private static Image cachedPageCurl;

		private static Image cachedPageCurlShadow;

		private static Bitmap coloredPageCurl;

		public Color SelectionBackColor
		{
			get
			{
				return selectionBackColor;
			}
			set
			{
				selectionBackColor = value;
			}
		}

		public ThumbnailDrawingOptions Options
		{
			get;
			set;
		}

		public Image Image
		{
			get;
			set;
		}

		public Image BackImage
		{
			get;
			set;
		}

		public Color MissingBackColor
		{
			get;
			set;
		}

		public float Rating1
		{
			get;
			set;
		}

		public float Rating2
		{
			get;
			set;
		}

		public ThumbnailRatingMode RatingMode
		{
			get;
			set;
		}

		public int ComicCount
		{
			get;
			set;
		}

		public int PageCount
		{
			get;
			set;
		}

		public bool BookmarkPercentMode
		{
			get;
			set;
		}

		public int[] Bookmarks
		{
			get;
			set;
		}

		public Color[] BookmarkColors
		{
			get;
			set;
		}

		public Image RatingImage1
		{
			get;
			set;
		}

		public Image RatingImage2
		{
			get;
			set;
		}

		public Image TagRatingImage1
		{
			get;
			set;
		}

		public Image TagRatingImage2
		{
			get;
			set;
		}

		public int PageNumber
		{
			get;
			set;
		}

		public ContentAlignment PageNumberAlignment
		{
			get;
			set;
		}

		public float ImageOpacity
		{
			get;
			set;
		}

		public float StateOverlap
		{
			get;
			set;
		}

		public List<Image> StateImages
		{
			get
			{
				if (stateImages == null)
				{
					stateImages = new List<Image>();
				}
				return stateImages;
			}
		}

		public Rectangle ThumbnailBounds => thumbnailBounds;

		public RatingRenderer RatingStripRenderer => ratingStripRenderer;

		public StyledRenderer.AlphaStyle SelectionAlphaState => StyledRenderer.GetAlphaStyle(Selected, Hot, Focused);

		public bool FastModeEnabled => (Options & ThumbnailDrawingOptions.FastMode) != 0;

		public bool BackImageEnabled => (Options & ThumbnailDrawingOptions.EnableBackImage) != 0;

		public bool BackgroundEnabled => (Options & ThumbnailDrawingOptions.EnableBackground) != 0;

		public bool ShadowEnabled => (Options & ThumbnailDrawingOptions.EnableShadow) != 0;

		public bool BorderEnabled => (Options & ThumbnailDrawingOptions.EnableBorder) != 0;

		public bool BowShadowEnabled
		{
			get
			{
				if ((Options & ThumbnailDrawingOptions.EnableBowShadow) != 0)
				{
					return EngineConfiguration.Default.ThumbnailPageBow;
				}
				return false;
			}
		}

		public bool RatingEnabled
		{
			get
			{
				return (Options & ThumbnailDrawingOptions.EnableRating) != 0;
			}
			set
			{
				Options = Options.SetMask(ThumbnailDrawingOptions.EnableRating, value);
			}
		}

		public bool StatesEnabled
		{
			get
			{
				return (Options & ThumbnailDrawingOptions.EnableStates) != 0;
			}
			set
			{
				Options = Options.SetMask(ThumbnailDrawingOptions.EnableStates, value);
			}
		}

		public bool VerticalBookmarksEnabled => (Options & ThumbnailDrawingOptions.EnableVerticalBookmarks) != 0;

		public bool HorizontalBookmarksEnabled => (Options & ThumbnailDrawingOptions.EnableHorizontalBookmarks) != 0;

		public bool PageNumberEnabled => (Options & ThumbnailDrawingOptions.EnablePageNumber) != 0;

		public bool KeepAspect => (Options & ThumbnailDrawingOptions.KeepAspect) != 0;

		public bool FillAspect => (Options & ThumbnailDrawingOptions.AspectFill) != 0;

		public bool Selected => (Options & ThumbnailDrawingOptions.Selected) != 0;

		public bool Hot => (Options & ThumbnailDrawingOptions.Hot) != 0;

		public bool Focused => (Options & ThumbnailDrawingOptions.Focused) != 0;

		public bool Bookmarked
		{
			get
			{
				return (Options & ThumbnailDrawingOptions.Bookmarked) != 0;
			}
			set
			{
				Options = (value ? (Options | ThumbnailDrawingOptions.Bookmarked) : (Options & ~ThumbnailDrawingOptions.Bookmarked));
			}
		}

		public bool MissingThumbnailDisabled => (Options & ThumbnailDrawingOptions.DisableMissingThumbnail) != 0;

		public bool HasStateOverlay
		{
			get
			{
				if (StatesEnabled && stateImages != null)
				{
					return stateImages.Count > 0;
				}
				return false;
			}
		}

		public bool HasTagRatingOverlay
		{
			get
			{
				if (RatingEnabled && RatingMode == ThumbnailRatingMode.Tags)
				{
					if (!(Rating1 > 0f))
					{
						return Rating2 > 0f;
					}
					return true;
				}
				return false;
			}
		}

		public ThumbRenderer()
		{
			ImageOpacity = 1f;
			PageNumberAlignment = ContentAlignment.TopRight;
			MissingBackColor = Color.White;
			ComicCount = 1;
			RatingImage1 = DefaultRatingImage1;
			RatingImage2 = DefaultRatingImage2;
			TagRatingImage1 = DefaultTagRatingImage1;
			TagRatingImage2 = DefaultTagRatingImage2;
			StateOverlap = DefaultStateOverlap;
			BookmarkColors = EngineConfiguration.Default.BookmarkColors;
			if (BookmarkColors == null || BookmarkColors.Length == 0)
			{
				BookmarkColors = DefaultBookmarkColors;
			}
		}

		public ThumbRenderer(Image image, ThumbnailDrawingOptions flags)
			: this()
		{
			Image = image;
			Options = flags;
		}

		public void DrawPageNumber(Graphics graphics, Rectangle thumbnailBounds, ContentAlignment align)
		{
			if (!PageNumberEnabled)
			{
				return;
			}
			thumbnailBounds.Inflate(-2, -2);
			string text = PageNumber.ToString();
			Font font = FC.Get("Arial", 7f);
			Rectangle rectangle = new Rectangle(Point.Empty, graphics.MeasureString(text, font).ToSize());
			rectangle.Width = Math.Max(rectangle.Width, 20);
			rectangle.Inflate(2, 2);
			rectangle = rectangle.Align(thumbnailBounds, align);
			using (graphics.AntiAlias())
			{
				using (GraphicsPath path = rectangle.ConvertToPath(3, 3))
				{
					using (StringFormat format = new StringFormat
					{
						LineAlignment = StringAlignment.Center,
						Alignment = StringAlignment.Center
					})
					{
						using (SolidBrush brush = new SolidBrush(Color.FromArgb(192, Color.Black)))
						{
							graphics.FillPath(brush, path);
						}
						graphics.DrawString(text, font, Brushes.White, rectangle, format);
						graphics.DrawPath(Pens.Black, path);
					}
				}
			}
		}

		public void DrawPageNumber(Graphics graphics, Rectangle thumbnailBounds)
		{
			DrawPageNumber(graphics, thumbnailBounds, PageNumberAlignment);
		}

		public static void DrawRatingStrip(Graphics graphics, Rectangle thumbnailBounds, Image image, float rating, PointF offset, int height = -1)
		{
			if (!(rating <= 0f))
			{
				Point location = thumbnailBounds.Location;
				location.Offset(2, 2);
				if (height == -1)
				{
					height = GetOverlaysHeight(thumbnailBounds);
				}
				int num = height / 5;
				location.Offset((int)((float)num * offset.X), (int)((float)height * offset.Y));
				new RatingRenderer(image, new Rectangle(location, new Size(num, height)), 5, vertical: true).DrawRatingStrip(graphics, rating);
			}
		}

		public static int GetOverlaysHeight(Rectangle thumbnailBounds)
		{
			return (thumbnailBounds.Height / 10).Clamp(16, 32);
		}

		public static int GetTagHeight(Rectangle thumbnailBounds)
		{
			return (thumbnailBounds.Height / 5).Clamp(16, 64);
		}

		public static int DrawTagRating(Graphics graphics, Rectangle thumbnailBounds, Image image, float rating, int height = -1)
		{
			if (rating <= 0f)
			{
				return 0;
			}
			Point location = thumbnailBounds.BottomRight();
			if (height == -1)
			{
				height = GetTagHeight(thumbnailBounds);
			}
			location.Offset(-height, -height);
			Rectangle bounds = new Rectangle(location, new Size(height, height));
			new RatingRenderer(image, bounds).DrawRatingTag(graphics, rating);
			return bounds.Width;
		}

		public int DrawRating(Graphics graphics, Rectangle thumbnailBounds, int height = -1)
		{
			if (!RatingEnabled || RatingMode == ThumbnailRatingMode.StarsBelow)
			{
				return 0;
			}
			using (graphics.Fast(FastModeEnabled))
			{
				switch (RatingMode)
				{
				case ThumbnailRatingMode.Tags:
				{
					int num = DrawTagRating(graphics, thumbnailBounds, TagRatingImage2, Rating2, height);
					thumbnailBounds.Width -= num;
					return num + DrawTagRating(graphics, thumbnailBounds, TagRatingImage1, Rating1, height);
				}
				case ThumbnailRatingMode.StarsOverlay:
					DrawRatingStrip(graphics, thumbnailBounds, RatingImage2, Rating2, PointF.Empty, height);
					DrawRatingStrip(graphics, thumbnailBounds, RatingImage1, Rating1, new PointF(0.25f, 0f), height);
					return thumbnailBounds.Width;
				default:
					return 0;
				}
			}
		}

		public int DrawStateImage(Graphics graphics, Rectangle thumbnailBounds)
		{
			if (!HasStateOverlay)
			{
				return 0;
			}
			int top = thumbnailBounds.Height - GetOverlaysHeight(thumbnailBounds);
			using (graphics.Fast(FastModeEnabled))
			{
				return DrawImageList(graphics, stateImages, thumbnailBounds.Pad(0, top), ContentAlignment.MiddleLeft, StateOverlap);
			}
		}

		public void DrawBookmarks(Graphics graphics, Rectangle thumbnailBounds)
		{
			int num = (BookmarkPercentMode ? 100 : PageCount);
			if (Bookmarks == null || num == 0 || (!HorizontalBookmarksEnabled && !VerticalBookmarksEnabled))
			{
				return;
			}
			for (int i = 0; i < Bookmarks.Length; i++)
			{
				try
				{
					float num2 = Bookmarks[i];
					Color col = BookmarkColors[i % BookmarkColors.Length];
					Color col2 = Color.FromArgb((int)col.R / 2, (int)col.G / 2, (int)col.B / 2);
					if (VerticalBookmarksEnabled)
					{
						DrawBookmarkV(graphics, thumbnailBounds, num2 / (float)num, col, col2, withShadow: true);
					}
					if (HorizontalBookmarksEnabled)
					{
						DrawBookmarkH(graphics, thumbnailBounds, num2 / (float)num, col, col2, withShadow: true);
					}
				}
				catch
				{
				}
			}
		}

		public void DrawThumbnailOverlays(Graphics graphics, Rectangle thumbnailBounds)
		{
			DrawPageNumber(graphics, thumbnailBounds);
			DrawBookmarks(graphics, thumbnailBounds);
			if (Bookmarked)
			{
				Color red = Color.Red;
				Color col = Color.FromArgb((int)red.R / 2, (int)red.G / 2, (int)red.B / 2);
				DrawBookmarkH(graphics, thumbnailBounds, 100f, red, col, withShadow: true);
			}
			DrawStateImage(graphics, thumbnailBounds);
			DrawRating(graphics, thumbnailBounds);
		}

		private static void DrawShadow(Graphics graphics, Rectangle bounds, int width)
		{
			bounds.Width += width;
			bounds.Height += width;
			graphics.DrawShadow(bounds, width, Color.Black, 0.33f, BlurShadowType.Outside, BlurShadowParts.Default);
		}

		private void DrawStack(Graphics graphics, Rectangle thumbnailBounds, int stackRotation, int shadowSize)
		{
			using (graphics.SaveState())
			{
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.TranslateTransform(thumbnailBounds.Left + thumbnailBounds.Width / 2, thumbnailBounds.Top + thumbnailBounds.Height / 2);
				graphics.RotateTransform(stackRotation);
				graphics.TranslateTransform(-thumbnailBounds.Left - thumbnailBounds.Width / 2, -thumbnailBounds.Top - thumbnailBounds.Height / 2);
				if (ShadowEnabled)
				{
					DrawShadow(graphics, thumbnailBounds, shadowSize);
				}
				graphics.FillRectangle(ThemeBrushes.Stack.Fill, thumbnailBounds);
				graphics.DrawRectangle(ThemePens.Stack.Border, thumbnailBounds);
			}
		}

		public Rectangle DrawThumbnail(Graphics graphics, Rectangle thumbnailBounds)
		{
			bool flag = Options.HasFlag(ThumbnailDrawingOptions.Stacked);
			bool flag2 = Options.HasFlag(ThumbnailDrawingOptions.NoOpaqueCover);
			Rectangle rect = thumbnailBounds;
			RatingRenderer ratingRenderer = null;
			RatingRenderer ratingRenderer2 = null;
			if (RatingEnabled && RatingMode == ThumbnailRatingMode.StarsBelow)
			{
				int overlaysHeight = GetOverlaysHeight(thumbnailBounds);
				Rectangle bounds = new Rectangle(0, 0, thumbnailBounds.Width - 4, overlaysHeight - 4);
				ratingRenderer = new RatingRenderer(RatingImage1, bounds)
				{
					Fast = FastModeEnabled
				};
				ratingRenderer2 = new RatingRenderer(RatingImage2, bounds)
				{
					Fast = FastModeEnabled
				};
				overlaysHeight = (int)ratingRenderer.GetRenderSize().Height;
				int num3 = (ratingRenderer.X = (ratingRenderer2.X = thumbnailBounds.Left + 2));
				num3 = (ratingRenderer.Height = (ratingRenderer2.Height = overlaysHeight));
				thumbnailBounds = thumbnailBounds.Fit(thumbnailBounds.Pad(0, 0, 0, overlaysHeight + 4));
			}
			if (KeepAspect && Image != null)
			{
				thumbnailBounds = Image.Size.ToRectangle(thumbnailBounds);
			}
			if (ShadowEnabled)
			{
				thumbnailBounds.Width -= 4;
				thumbnailBounds.Height -= 4;
			}
			if (flag)
			{
				thumbnailBounds.Inflate(-8, -8);
			}
			if (!graphics.IsVisible(rect))
			{
				this.thumbnailBounds = thumbnailBounds;
				return thumbnailBounds;
			}
			using (graphics.Fast(FastModeEnabled))
			{
				if (flag)
				{
					if (ComicCount > 2)
					{
						DrawStack(graphics, thumbnailBounds, -4, 4);
					}
					if (ComicCount > 1)
					{
						DrawStack(graphics, thumbnailBounds, 2, 4);
					}
				}
				if (ShadowEnabled)
				{
					DrawShadow(graphics, thumbnailBounds, 4);
				}
				if (!MissingThumbnailDisabled && (Image == null || ImageOpacity < 0.95f))
				{
					DrawMissingThumbnail(graphics, thumbnailBounds, MissingBackColor);
				}
				if (Image != null)
				{
					if (ImageOpacity > 0.95f && !flag2)
					{
						graphics.CompositingMode = CompositingMode.SourceCopy;
					}
					if (KeepAspect)
					{
						graphics.DrawImage(Image, thumbnailBounds, ImageOpacity);
					}
					else
					{
						Rectangle src = Image.Size.ToRectangle();
						if (Image.Width > Image.Height && thumbnailBounds.Height > thumbnailBounds.Width)
						{
							int width = src.Width;
							src.Width = Math.Min(width, src.Height * thumbnailBounds.Width / thumbnailBounds.Height);
							src.X = width - src.Width;
						}
						graphics.DrawImage(Image, thumbnailBounds, src, ImageOpacity);
					}
					graphics.CompositingMode = CompositingMode.SourceOver;
					if (BowShadowEnabled)
					{
						Rectangle bounds2 = thumbnailBounds;
						bounds2.Width /= 10;
						graphics.DrawImage(pageBowLeft, bounds2, new Rectangle(0, 0, 32, 8), ImageOpacity);
						bounds2.X = thumbnailBounds.Right - bounds2.Width;
						graphics.DrawImage(pageBowRight, bounds2, new Rectangle(0, 0, 32, 8), ImageOpacity);
					}
					if (BackImageEnabled)
					{
						if (Selected)
						{
							DrawPageCurlOverlay(graphics, BackImage, thumbnailBounds);
						}
						else if (Hot)
						{
							DrawPageCurlOverlay(graphics, BackImage, thumbnailBounds, 0.5f);
						}
					}
					else
					{
						graphics.DrawStyledRectangle(thumbnailBounds, SelectionAlphaState, SelectionBackColor, StyledRenderer.Default.Frame(0, 0));
					}
				}
			}
			if (BorderEnabled)
			{
				graphics.DrawRectangle(Pens.Black, thumbnailBounds);
			}
			DrawThumbnailOverlays(graphics, thumbnailBounds);
			if (ratingRenderer == null)
			{
				ratingStripRenderer = null;
			}
			else
			{
				ratingRenderer.Y = thumbnailBounds.Bottom + 4;
				ratingRenderer2.Y = ratingRenderer.Y + 2;
				ratingRenderer2.DrawRatingStrip(graphics, Rating2, 1f, (Rating2 > 0f) ? 0.25f : 0f);
				ratingRenderer.DrawRatingStrip(graphics, Rating1, 1f, (Rating2 > 0f) ? 0f : 0.25f);
				ratingStripRenderer = ratingRenderer;
			}
			this.thumbnailBounds = thumbnailBounds;
			return thumbnailBounds;
		}

		private static GraphicsPath GetBookmark(Rectangle rect)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.AddLines(new Point[5]
			{
				rect.Location,
				new Point(rect.Right, rect.Top),
				new Point(rect.Right, rect.Bottom),
				new Point(rect.Left, rect.Bottom),
				new Point(rect.Left + rect.Height, rect.Top + rect.Height / 2)
			});
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		private static void DrawBookmarkV(Graphics gr, Rectangle tr, float percent, Color col1, Color col2, bool withShadow)
		{
			int num = Math.Min(8, tr.Height - 2);
			int num2 = Math.Min(16, tr.Width - 2);
			int num3 = tr.Height - num;
			int y = tr.Top + (int)((float)num3 * percent);
			Rectangle rect = new Rectangle(tr.Right - num2, y, num2, num);
			using (gr.SaveState())
			{
				gr.SmoothingMode = SmoothingMode.AntiAlias;
				using (GraphicsPath path = GetBookmark(rect))
				{
					if (withShadow)
					{
						gr.TranslateTransform(1f, 1f);
						gr.FillPath(Brushes.Black, path);
					}
					gr.TranslateTransform(-1f, -1f);
					using (Brush brush = new LinearGradientBrush(rect, col1, col2, 90f))
					{
						gr.FillPath(brush, path);
					}
					gr.DrawPath(Pens.Black, path);
				}
			}
		}

		private static void DrawBookmarkH(Graphics gr, Rectangle tr, float percent, Color col1, Color col2, bool withShadow)
		{
			using (gr.SaveState())
			{
				gr.RotateTransform(-90f);
				using (System.Drawing.Drawing2D.Matrix rotationMatrix = MatrixUtility.GetRotationMatrix(Point.Empty, 90))
				{
					Rectangle tr2 = tr.Rotate(rotationMatrix);
					DrawBookmarkV(gr, tr2, percent, col1, col2, withShadow);
				}
			}
		}

		public static Size GetSafeScaledImageSize(Image image, Size canvasSize)
		{
			return GetSafeScaledImageSize(image, canvasSize, DefaultThumbnailAspect);
		}

		public static Size GetSafeScaledImageSize(Image image, Size canvasSize, float defaultAspect)
		{
			return GetSafeScaledImageSize(image?.Size ?? Size.Empty, canvasSize, defaultAspect);
		}

		public static Size GetSafeScaledImageSize(Size imageSize, Size canvasSize, float defaultAspect)
		{
			if (!imageSize.IsEmpty)
			{
				return imageSize.ToRectangle(canvasSize, RectangleScaleMode.None).Size;
			}
			return new Size((int)((float)canvasSize.Height * defaultAspect), canvasSize.Height);
		}

		public static Size GetSafeScaledImageSize(Size imageSize, Size canvasSize)
		{
			return GetSafeScaledImageSize(imageSize, canvasSize, DefaultThumbnailAspect);
		}

		public static void DrawMissingThumbnail(Graphics graphics, Rectangle bounds, Color backColor)
		{
			using (Brush brush = new SolidBrush(backColor))
			{
				graphics.FillRectangle(brush, bounds);
			}
			Rectangle r = bounds;
			r.Inflate(-4, -4);
			using (StringFormat format = new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			})
			{
				graphics.DrawString(loadingThumbnail, FC.Get("Arial", 6f), Brushes.LightGray, r, format);
			}
		}

		public static Rectangle DrawThumbnail(Graphics graphics, Image image, Rectangle bounds, ThumbnailDrawingOptions flags, ComicBook comicBook, float opacity = 1f)
		{
			ThumbRenderer thumbRenderer = new ThumbRenderer(image, flags);
			if (comicBook != null)
			{
				thumbRenderer.PageCount = comicBook.PageCount;
				thumbRenderer.Rating1 = comicBook.Rating;
				thumbRenderer.Rating2 = comicBook.CommunityRating;
				thumbRenderer.Bookmarks = new int[2]
				{
					comicBook.CurrentPage,
					comicBook.LastPageRead
				};
			}
			thumbRenderer.ImageOpacity = opacity;
			return thumbRenderer.DrawThumbnail(graphics, bounds);
		}

		public static void DrawPageCurlOverlay(Graphics graphics, Image uncoveredImage, Image pageCurl, Image pageCurlShadow, Rectangle rc, float width, Color pageCurlColor)
		{
			if (uncoveredImage == null || rc.IsEmpty)
			{
				return;
			}
			float num = (float)rc.Height / (float)uncoveredImage.Height;
			RectangleF rect = new RectangleF(rc.Left, rc.Top, num * (float)uncoveredImage.Width, rc.Height);
			int num2 = rc.Width;
			if (rc.Width > rc.Height)
			{
				num2 /= 2;
			}
			if (rect.Width < (float)num2)
			{
				num2 = (int)rect.Width;
			}
			rc = rc.Pad(rc.Width - num2, 0);
			float num3 = (float)Math.Min(rc.Width, rc.Height) * width;
			float num4 = num3;
			rect.X = (float)rc.Right - rect.Width;
			using (GraphicsPath graphicsPath = new GraphicsPath())
			{
				graphicsPath.AddPolygon(new PointF[3]
				{
					new PointF(rc.Right, (float)rc.Bottom - num4),
					new PointF(rc.Right, rc.Bottom),
					new PointF((float)rc.Right - num3, rc.Bottom)
				});
				RectangleF rect2 = new RectangleF((float)rc.Right - num3, (float)rc.Bottom - num4, num3, num4);
				using (graphics.SaveState())
				{
					using (Region region = new Region(graphicsPath))
					{
						graphics.IntersectClip(region);
						graphics.DrawImage(uncoveredImage, rect);
					}
				}
				if (coloredPageCurl == null || cachedPageCurlColor != pageCurlColor || cachedPageCurl != pageCurl || cachedPageCurlShadow != pageCurlShadow)
				{
					Bitmap bitmap = new Bitmap(pageCurl);
					if (!pageCurlColor.IsEmpty)
					{
						bitmap.ToGrayScale();
						bitmap.Colorize(pageCurlColor);
					}
					coloredPageCurl = bitmap.ToOptimized();
					cachedPageCurl = pageCurl;
					cachedPageCurlShadow = pageCurlShadow;
					cachedPageCurlColor = pageCurlColor;
				}
				graphics.DrawImage(pageCurlShadow, rect2);
				graphics.DrawImage(coloredPageCurl, rect2);
			}
		}

		public static void DrawPageCurlOverlay(Graphics graphics, Image uncoveredImage, Rectangle rc, float width = 0.9f)
		{
			DrawPageCurlOverlay(graphics, uncoveredImage, DefaultPageCurlImage, DefaultPageCurlShadowImage, rc, width, EngineConfiguration.Default.ThumbnailPageCurlColor);
		}

		public static int DrawImageList(Graphics graphics, IEnumerable<Image> images, Rectangle bounds, ContentAlignment alignment, float overlapPercent = 0f, bool allowOversize = true)
		{
			if (images.Count() == 0)
			{
				return 0;
			}
			var enumerable = images.Select((Image img) => new
			{
				Size = img.Size.Scale(img.Size.GetScale(bounds.Size, ScaleMode.FitAll, allowOversize)),
				Image = img
			});
			Func<int, int> getWidth = (int w) => w - (int)(overlapPercent * (float)w);
			int num = enumerable.Max(s => s.Size.Height);
			int num2 = enumerable.Last().Size.Width + enumerable.Reverse().Skip(1).Sum(s => getWidth(s.Size.Width));
			float num3 = Math.Min(1f, (float)bounds.Width / (float)num2);
			Rectangle rectangle = new Rectangle(0, 0, (int)(num3 * (float)num2), (int)(num3 * (float)num));
			Rectangle rectangle2 = rectangle.Align(bounds, alignment);
			int num4 = rectangle2.Left;
			int top = rectangle2.Top;
			foreach (var item in enumerable)
			{
				Size size = item.Size.Scale(num3);
				int num5 = (rectangle2.Height - size.Height) / 2;
				graphics.DrawImage(item.Image, num4, top + num5, size.Width, size.Height);
				num4 += getWidth(size.Width);
			}
			return rectangle2.Width;
		}

		public static Bitmap GetNewPageStatusImage(int newPages)
		{
			if (newPages <= 0)
			{
				return null;
			}
			return DefaultNewPagesImages[(newPages - 1).Clamp(0, DefaultNewPagesImages.Length - 1)];
		}
	}
}
