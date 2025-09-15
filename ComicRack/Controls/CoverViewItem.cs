using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Reflection;
using cYo.Common.Text;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class CoverViewItem : ThumbnailViewItem, IViewableItemHitTest, ISetCustomThumbnail
	{
		[Flags]
		private enum ItemGapType
		{
			Undefined = 0x1000,
			None = 0x0,
			Start = 0x1,
			End = 0x2
		}

		public delegate void DrawCustomThumbnailOverlayHandler(ComicBook comic, Graphics gr, Rectangle bounds, int flags);

		public const int GapInfoWidth = 16;

		public const int NumericSpinWidth = 11;

		public const int CheckBoxWidth = 22;

		public static readonly Bitmap MarkerLastOpenedImage = Resources.LastMarker;

		public static readonly Bitmap MarkerIsFileLessImage = Resources.FilelessMarker;

		public static readonly Bitmap MarkerIsOpenImage = Resources.OpenMarker;

		public static readonly Bitmap IsDirtyImage = Resources.UpdateBig;

		private int stackReadPercent = -1;

		private float averageRating = -1f;

		private float averageCommunityRating = -1f;

		private ItemGapType gapType = ItemGapType.Undefined;

		private string customThumbnailKey;

		private int thumbnailLabelHeight = 40;

		private float thumbnailLabelFontScale = 1f;

		private static readonly Image yellowStar = Resources.StarYellow;

		private static readonly Image blueStar = Resources.StarBlue;

		private static readonly Bitmap bitmapGapUp = Resources.GapUp.CropTransparent(width: true, height: false, 16).ScaleDpi();

		private static readonly Bitmap bitmapGapDown = Resources.GapDown.CropTransparent(width: true, height: false, 16).ScaleDpi();

		private static readonly Bitmap bitmapGapUpDown = Resources.GapUpDown.CropTransparent(width: true, height: false, 16).ScaleDpi();

		private const string SeriesStatsPrefix = "SeriesStat";

		private static readonly int SeriesStatsPrefixLength = SeriesStatsPrefix.Length;

		private static readonly Bitmap linkArrow = Resources.SmallArrowRight.ScaleDpi();

		private int labelLines = 3;

		private MarkerType marker;

		private Rectangle? drawnRect;

		private bool refreshed;

		public int StackReadPercent
		{
			get
			{
				if (base.View == null || !base.View.IsStack(this))
				{
					return Comic.ReadPercentage;
				}
				if (stackReadPercent < 0)
				{
					int stackCount = base.View.GetStackCount(this);
					int num = base.View.GetStackItems(this).OfType<CoverViewItem>().Count((CoverViewItem cvi) => cvi.Comic.HasBeenRead);
					stackReadPercent = num * 100 / stackCount;
				}
				return stackReadPercent;
			}
		}

		public float AverageRating
		{
			get
			{
				if (base.View == null || !base.View.IsStack(this))
				{
					return Comic.Rating;
				}
				if (averageRating < 0f)
				{
					try
					{
						averageRating = (from cvi in base.View.GetStackItems(this).OfType<CoverViewItem>()
							select cvi.Comic.Rating into r
							where r > 0f
							select r).Average((float r) => r);
					}
					catch (Exception)
					{
						averageRating = 0f;
					}
				}
				return averageRating;
			}
		}

		public float AverageCommunityRating
		{
			get
			{
				if (base.View == null || !base.View.IsStack(this))
				{
					return Comic.CommunityRating;
				}
				if (averageCommunityRating < 0f)
				{
					try
					{
						averageCommunityRating = (from cvi in base.View.GetStackItems(this).OfType<CoverViewItem>()
							select cvi.Comic.CommunityRating into r
							where r > 0f
							select r).Average((float r) => r);
					}
					catch (Exception)
					{
						averageCommunityRating = 0f;
					}
				}
				return averageCommunityRating;
			}
		}

		private ItemGapType GapType
		{
			get
			{
				if (gapType == ItemGapType.Undefined && SeriesStats != null)
				{
					gapType = ItemGapType.None;
					if (SeriesStats.IsGapStart(Comic))
					{
						gapType |= ItemGapType.Start;
					}
					if (SeriesStats.IsGapEnd(Comic))
					{
						gapType |= ItemGapType.End;
					}
				}
				return gapType;
			}
		}

		public override ThumbnailKey ThumbnailKey
		{
			get
			{
				if (string.IsNullOrEmpty(customThumbnailKey) || !base.View.IsStack(this))
				{
					return Comic.GetFrontCoverThumbnailKey();
				}
				return Comic.GetThumbnailKey(0, customThumbnailKey);
			}
		}

		public string CustomThumbnailKey
		{
			get
			{
				return customThumbnailKey;
			}
			set
			{
				if (!(customThumbnailKey == value))
				{
					customThumbnailKey = value;
					Update(sizeChanged: true);
				}
			}
		}

		public bool HasCustomThumbnail => !string.IsNullOrEmpty(customThumbnailKey);

		public override string Text
		{
			get
			{
				return Comic.Caption;
			}
			set
			{
				base.Text = value;
			}
		}

		public override string Name
		{
			get
			{
				return Comic.CaptionWithoutFormat;
			}
			set
			{
				base.Name = value;
			}
		}

		public int ThumbnailLabelHeight => thumbnailLabelHeight;

		public float ThumbnailLabelFontScale => thumbnailLabelFontScale;

		public ComicBook Comic
		{
			get;
			set;
		}

		public int LabelLines
		{
			get
			{
				return labelLines;
			}
			set
			{
				if (labelLines != value)
				{
					labelLines = value;
					Update(sizeChanged: true);
				}
			}
		}

		public int Position
		{
			get;
			set;
		}

		public ThumbnailConfig ThumbnailConfig
		{
			get;
			set;
		}

		public MarkerType Marker
		{
			get
			{
				return marker;
			}
			set
			{
				if (marker != value)
				{
					marker = value;
					Update();
				}
			}
		}

		public IGroupInfo CustomGroup
		{
			get;
			set;
		}

		public IComicBookStatsProvider StatsProvider
		{
			get;
			set;
		}

		public ComicBookSeriesStatistics SeriesStats
		{
			get
			{
				if (StatsProvider == null)
				{
					return null;
				}
				return StatsProvider.GetSeriesStats(Comic);
			}
		}

		public static CoverThumbnailSizing ThumbnailSizing
		{
			get;
			set;
		}

		public static event DrawCustomThumbnailOverlayHandler DrawCustomThumbnailOverlay;

		public static CoverViewItem Create(ComicBook comic, int position, IComicBookStatsProvider statsProvider)
		{
			CoverViewItem coverViewItem = new CoverViewItem
			{
				Position = position,
				Comic = comic,
				StatsProvider = statsProvider
			};
			coverViewItem.Comic.BookChanged += coverViewItem.Comic_PropertyChanged;
			coverViewItem.Tag = comic.FilePath;
			return coverViewItem;
		}

		protected virtual void OnComicPropertyChanged(string name)
		{
			if (name == "Rating")
			{
				averageRating = -1f;
			}
			else if (name == "CommunityRating")
			{
				averageCommunityRating = -1f;
			}
		}

		protected override void OnRefreshImage()
		{
			base.OnRefreshImage();
			Program.ImagePool.Thumbs.RefreshImage(Comic.GetThumbnailKey(Comic.FirstNonCoverPageIndex));
		}

		protected override Size GetEstimatedSize(Size canvasSize)
		{
			if (Comic == null || Comic.PageCount < 1)
			{
				return base.GetEstimatedSize(canvasSize);
			}
			ComicPageInfo page = Comic.GetPage(Comic.FrontCoverPageIndex);
			Size size = new Size(page.ImageWidth, page.ImageHeight);
			if (size.Width <= 0 || size.Height <= 0)
			{
				return base.GetEstimatedSize(canvasSize);
			}
			size = size.ToRectangle(new Size(0, 512), RectangleScaleMode.None).Size;
			return ThumbRenderer.GetSafeScaledImageSize(size, canvasSize);
		}

		public override ItemViewStates GetOwnerDrawnStates(ItemViewMode mode)
		{
			if (mode != ItemViewMode.Detail)
			{
				return ItemViewStates.All;
			}
			return base.GetOwnerDrawnStates(mode);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Comic.BookChanged -= Comic_PropertyChanged;
			}
			base.Dispose(disposing);
		}

		protected override Size MeasureItem(Graphics graphics, Size defaultSize, ItemViewMode displayType)
		{
			defaultSize = base.MeasureItem(graphics, defaultSize, displayType);
			if (displayType == ItemViewMode.Thumbnail)
			{
				Size thumbnailSizeSafe = GetThumbnailSizeSafe(defaultSize);
				if (ThumbnailSizing != 0)
				{
					thumbnailSizeSafe.Width = (int)((float)thumbnailSizeSafe.Height * ThumbRenderer.DefaultThumbnailAspect);
				}
				thumbnailSizeSafe = AddBorder(thumbnailSizeSafe);
				thumbnailLabelFontScale = ((float)defaultSize.Height / 192f).Clamp(0.7f, 1f);
				thumbnailLabelHeight = ((ThumbnailConfig == null || !ThumbnailConfig.HideCaptions) ? ((int)((float)LabelLines * ((float)base.View.CurrentFontHeight * ThumbnailLabelFontScale + 2f))) : 0);
				thumbnailSizeSafe.Height += thumbnailLabelHeight;
				return thumbnailSizeSafe;
			}
			return defaultSize;
		}

		protected override Size GetDefaultMaximumSize(Size defaultSize)
		{
			int height = defaultSize.Height;
			if (ThumbnailSizing == CoverThumbnailSizing.None)
			{
				return new Size(height * 2, height);
			}
			return new Size(height * 1000, height);
		}

		protected override Size MeasureColumn(Graphics graphics, IColumn header, Size defaultSize)
		{
			defaultSize = base.MeasureColumn(graphics, header, defaultSize);
			ComicListField comicListField = (ComicListField)header.Tag;
			switch (comicListField.DisplayProperty)
			{
			case "Cover":
				defaultSize.Width = FormUtility.ScaleDpiX(128);
				break;
			case "Icons":
				defaultSize.Width = FormUtility.ScaleDpiX(64);
				break;
			case "State":
				defaultSize.Width = FormUtility.ScaleDpiX(24);
				break;
			case "Rating":
			case "CommunityRating":
			case "SeriesStatAverageRating":
			case "SeriesStatAverageCommunityRating":
				defaultSize.Width = FormUtility.ScaleDpiX(80);
				break;
			case "SeriesCompleteAsText":
			case "BlackAndWhiteAsText":
			case "LinkedAsText":
			case "EnableProposedAsText":
			case "Checked":
			case "HasBeenReadAsText":
				defaultSize.Width = FormUtility.ScaleDpiX(22);
				break;
			case "GapInformation":
				defaultSize.Width = FormUtility.ScaleDpiX(16);
				break;
			default:
			{
				string text = ((comicListField.DisplayProperty == "Position") ? Position.ToString() : GetColumnStringValue(comicListField.DisplayProperty, header.FormatId, comicListField.ValueType, proposed: true, comicListField.DefaultText));
				if (!string.IsNullOrEmpty(text))
				{
					defaultSize = graphics.MeasureString(text, base.View.Font).ToSize();
					defaultSize.Width += 8;
				}
				break;
			}
			}
			switch (comicListField.DisplayProperty)
			{
			case "YearAsText":
			case "MonthAsText":
			case "DayAsText":
			case "NumberAsText":
			case "CountAsText":
			case "AlternateNumberAsText":
			case "AlternateCountAsText":
			case "VolumeAsText":
			case "PagesAsTextSimple":
				defaultSize.Width += FormUtility.ScaleDpiX(11);
				break;
			}
			return defaultSize;
		}

		public override void OnDraw(ItemDrawInformation drawInfo)
		{
			base.OnDraw(drawInfo);
			if (drawInfo.SubItem == -1)
			{
				OnRefreshComicData();
			}
			bool flag = base.View.IsStack(this);
			Color textColor = drawInfo.TextColor;
			Rectangle rectangle = drawInfo.Bounds;
			Font font = base.View.Font;
			List<Image> list = null;
			if (Comic.ComicInfoIsDirty && Comic.IsLinked && Program.Settings.UpdateComicFiles)
			{
				list = list.SafeAdd(IsDirtyImage);
			}
			switch (marker)
			{
			case MarkerType.IsOpen:
				list = list.SafeAdd(MarkerIsOpenImage);
				break;
			case MarkerType.IsLast:
				list = list.SafeAdd(MarkerLastOpenedImage);
				break;
			}
			if (Comic.NewPages > 0)
			{
				list = list.SafeAdd(ThumbRenderer.GetNewPageStatusImage(Comic.NewPages));
			}
			if (!Comic.IsLinked)
			{
				list = list.SafeAdd(MarkerIsFileLessImage);
			}
			if (Comic.EditMode.IsLocalComic() && Comic.IsLinked && Comic.FileIsMissing)
			{
				list = list.SafeAdd(ThumbnailViewItem.DeletedStateImage);
			}
			ThumbnailRatingMode ratingMode = ((!Program.Settings.NumericRatingThumbnails) ? ((!EngineConfiguration.Default.RatingStarsBelowThumbnails) ? ThumbnailRatingMode.StarsOverlay : ThumbnailRatingMode.StarsBelow) : ThumbnailRatingMode.Tags);
			using (StringFormat stringFormat = new StringFormat())
			{
				drawnRect = null;
				int height = ((drawInfo.DisplayType == ItemViewMode.Detail) ? 256 : rectangle.Height);
				using (IItemLock<ThumbnailImage> itemLock = GetThumbnail(drawInfo))
				{
					Image backImage = null;
					Image image = itemLock?.Item.GetThumbnail(height);
					IItemLock<ThumbnailImage> itemLock2 = null;
					ThumbnailDrawingOptions thumbnailDrawingOptions = ThumbnailDrawingOptions.Default;
					if (base.Selected)
					{
						thumbnailDrawingOptions |= ThumbnailDrawingOptions.Selected;
					}
					if (base.Hot)
					{
						thumbnailDrawingOptions |= ThumbnailDrawingOptions.Hot;
					}
					if (base.Focused)
					{
						thumbnailDrawingOptions |= ThumbnailDrawingOptions.Focused;
					}
					if (flag)
					{
						thumbnailDrawingOptions = ((!HasCustomThumbnail) ? (thumbnailDrawingOptions | ThumbnailDrawingOptions.Stacked) : (thumbnailDrawingOptions | ThumbnailDrawingOptions.NoOpaqueCover));
					}
					if (HasCustomThumbnail)
					{
						thumbnailDrawingOptions &= ~(ThumbnailDrawingOptions.EnableShadow | ThumbnailDrawingOptions.EnableBorder | ThumbnailDrawingOptions.EnableRating | ThumbnailDrawingOptions.EnableVerticalBookmarks | ThumbnailDrawingOptions.EnableHorizontalBookmarks);
					}
					if (!Comic.IsLinked)
					{
						thumbnailDrawingOptions &= ~(ThumbnailDrawingOptions.EnableVerticalBookmarks | ThumbnailDrawingOptions.EnableHorizontalBookmarks);
					}
					if (HasCustomThumbnail && flag)
					{
						thumbnailDrawingOptions &= ~ThumbnailDrawingOptions.EnableBowShadow;
					}
					if (base.View.InScrollOrResize)
					{
						thumbnailDrawingOptions |= ThumbnailDrawingOptions.FastMode;
					}
					try
					{
						if (!HasCustomThumbnail && itemLock != null && (base.Hot || base.Selected) && drawInfo.DisplayType != ItemViewMode.Detail && Comic.IsLinked && Program.Settings.DogEarThumbnails && Comic.PageCount > 1 && !Comic.FileIsMissing)
						{
							itemLock2 = GetBackThumbnail();
							backImage = itemLock2?.Item.GetThumbnail(height);
						}
						switch (drawInfo.DisplayType)
						{
						case ItemViewMode.Thumbnail:
						{
							Animate(image);
							ThumbIconRenderer thumbIconRenderer = new ThumbIconRenderer(image, thumbnailDrawingOptions);
							thumbIconRenderer.Border = base.Border;
							thumbIconRenderer.ForeColor = textColor;
							thumbIconRenderer.PageCount = Comic.PageCount;
							thumbIconRenderer.RatingMode = ratingMode;
							thumbIconRenderer.Rating1 = AverageRating;
							thumbIconRenderer.Rating2 = AverageCommunityRating;
							thumbIconRenderer.ComicCount = base.View.GetStackCount(this);
							thumbIconRenderer.SelectionBackColor = StyledRenderer.GetSelectionColor(drawInfo.ControlFocused);
							thumbIconRenderer.BookmarkPercentMode = flag;
							thumbIconRenderer.Bookmarks = ((!flag) ? new int[2]
							{
								Comic.CurrentPage,
								Comic.LastPageRead
							} : new int[1]
							{
								StackReadPercent
							});
							thumbIconRenderer.BackImage = backImage;
							thumbIconRenderer.ImageOpacity = base.Opacity;
							ThumbIconRenderer thumbIconRenderer2 = thumbIconRenderer;
							if (list != null)
							{
								thumbIconRenderer2.StateImages.AddRange(list);
							}
							if (ThumbnailConfig != null && ThumbnailConfig.HideCaptions)
							{
								thumbIconRenderer2.TextHeight = 0;
							}
							else
							{
								thumbIconRenderer2.TextHeight = ThumbnailLabelHeight;
								CreateThumbnailLines(thumbIconRenderer2.TextLines, FC.GetRelative(font, ThumbnailLabelFontScale), textColor);
							}
							drawnRect = thumbIconRenderer2.Draw(drawInfo.Graphics, rectangle);
							thumbIconRenderer2.DisposeTextLines();
							OnDrawCustomThumbnailOverlay(drawInfo.Graphics, thumbIconRenderer2.ThumbnailBounds);
							DrawFrontCoverButton(drawInfo.Graphics, thumbIconRenderer2.ThumbnailBounds);
							if (thumbIconRenderer2.RatingStripRenderer == null || !Comic.EditMode.CanEditProperties())
							{
								break;
							}
							RatingRenderer r = thumbIconRenderer2.RatingStripRenderer;
							AddClickRegion(r.Bounds, delegate(Rectangle rect, Point pt)
							{
								SetSelectedComics(delegate(ComicBook cb)
								{
									cb.Rating = r.GetRatingFromStrip(pt.Add(rect.Location));
								}, TR.Load("Columns")["Rating"]);
							});
							break;
						}
						case ItemViewMode.Tile:
						{
							Animate(image);
							ThumbTileRenderer thumbTileRenderer = new ThumbTileRenderer(image, thumbnailDrawingOptions);
							thumbTileRenderer.ImageOpacity = base.Opacity;
							thumbTileRenderer.Font = font;
							thumbTileRenderer.Border = base.Border;
							thumbTileRenderer.ForeColor = textColor;
							thumbTileRenderer.BackColor = Color.LightGray;
							thumbTileRenderer.SelectionBackColor = StyledRenderer.GetSelectionColor(drawInfo.ControlFocused);
							thumbTileRenderer.PageCount = Comic.PageCount;
							thumbTileRenderer.RatingMode = ratingMode;
							thumbTileRenderer.Rating1 = AverageRating;
							thumbTileRenderer.Rating2 = AverageCommunityRating;
							thumbTileRenderer.ComicCount = base.View.GetStackCount(this);
							thumbTileRenderer.BookmarkPercentMode = flag;
							thumbTileRenderer.Bookmarks = ((!flag) ? new int[2]
							{
								Comic.CurrentPage,
								Comic.LastPageRead
							} : new int[1]
							{
								StackReadPercent
							});
							thumbTileRenderer.BackImage = backImage;
							thumbTileRenderer.Icons = Comic.GetIcons();
							ThumbTileRenderer thumbTileRenderer2 = thumbTileRenderer;
							Font font2 = FC.Get(font, ((float)rectangle.Height * 0.07f).Clamp(font.Size * 0.8f, font.Size * 1f));
							ComicBook comicBook;
							ComicTextElements flags;
							if (flag)
							{
								comicBook = CreateStackInfo();
								flags = ComicTextElements.DefaultStack;
							}
							else
							{
								flags = ((ThumbnailConfig != null) ? ThumbnailConfig.TextElements : ComicTextElements.DefaultFileComic);
								comicBook = Comic;
							}
							thumbTileRenderer2.TextLines.AddRange(ComicTextBuilder.GetTextBlocks(comicBook, font2, textColor, flags));
							if (list != null)
							{
								thumbTileRenderer2.StateImages.AddRange(list);
							}
							thumbTileRenderer2.DrawTile(drawInfo.Graphics, rectangle);
							thumbTileRenderer2.DisposeTextLines();
							OnDrawCustomThumbnailOverlay(drawInfo.Graphics, thumbTileRenderer2.ThumbnailBounds);
							DrawFrontCoverButton(drawInfo.Graphics, thumbTileRenderer2.ThumbnailBounds);
							if (thumbTileRenderer2.RatingStripRenderer == null || !Comic.EditMode.CanEditProperties())
							{
								break;
							}
							RatingRenderer r2 = thumbTileRenderer2.RatingStripRenderer;
							AddClickRegion(r2.Bounds, delegate(Rectangle rect, Point pt)
							{
								SetSelectedComics(delegate(ComicBook cb)
								{
									cb.Rating = r2.GetRatingFromStrip(pt.Add(rect.Location));
								}, TR.Load("Columns")["Rating"]);
							});
							break;
						}
						case ItemViewMode.Detail:
							if (drawInfo.Header != null)
							{
								ComicListField comicListField = (ComicListField)drawInfo.Header.Tag;
								string displayProperty = comicListField.DisplayProperty;
								switch (displayProperty)
								{
								case "Cover":
									if (!drawInfo.ExpandedColumn)
									{
										Animate(image);
									}
									if (image != null)
									{
										rectangle.Height = rectangle.Width * image.Height / image.Width;
										rectangle.Inflate(-2, -2);
										drawInfo.Graphics.IntersectClip(rectangle);
										ThumbnailDrawingOptions thumbnailDrawingOptions2 = ThumbnailDrawingOptions.EnableShadow | ThumbnailDrawingOptions.EnableHorizontalBookmarks;
										if (base.View.InScrollOrResize)
										{
											thumbnailDrawingOptions2 |= ThumbnailDrawingOptions.FastMode;
										}
										ThumbRenderer.DrawThumbnail(drawInfo.Graphics, image, rectangle, thumbnailDrawingOptions2, Comic, base.Opacity);
									}
									break;
								case "Icons":
								{
									Graphics graphics = drawInfo.Graphics;
									using (base.View.InScrollOrResize ? null : graphics.HighQuality(enabled: true))
									{
										rectangle = drawInfo.Bounds;
										rectangle.Inflate(-2, -2);
										ThumbRenderer.DrawImageList(graphics, Comic.GetIcons(), rectangle, ContentAlignment.MiddleCenter, -0.1f);
									}
									break;
								}
								case "State":
									if (list != null && list.Count > 0)
									{
										rectangle = drawInfo.Bounds;
										rectangle.Inflate(-2, -2);
										ThumbRenderer.DrawImageList(drawInfo.Graphics, list, rectangle, ContentAlignment.MiddleCenter, ThumbRenderer.DefaultStateOverlap);
									}
									break;
								case "Rating":
									DrawRating(drawInfo.Graphics, comicListField, rectangle, Comic.Rating, yellowStar, delegate(ComicBook cb, float x)
									{
										cb.Rating = x;
									});
									break;
								case "SeriesStatAverageRating":
									if (SeriesStats != null)
									{
										DrawRating(drawInfo.Graphics, comicListField, rectangle, SeriesStats.AverageRating, yellowStar);
									}
									break;
								case "CommunityRating":
									DrawRating(drawInfo.Graphics, comicListField, rectangle, Comic.CommunityRating, blueStar, delegate(ComicBook cb, float x)
									{
										cb.CommunityRating = x;
									});
									break;
								case "SeriesStatAverageCommunityRating":
									if (SeriesStats != null)
									{
										DrawRating(drawInfo.Graphics, comicListField, rectangle, SeriesStats.AverageCommunityRating, blueStar);
									}
									break;
								case "SeriesCompleteAsText":
									DrawCheckBox(drawInfo.Graphics, comicListField, rectangle, Comic.SeriesComplete, delegate(ComicBook cb, YesNo x)
									{
										cb.SeriesComplete = x;
									});
									break;
								case "BlackAndWhiteAsText":
									DrawCheckBox(drawInfo.Graphics, comicListField, rectangle, Comic.BlackAndWhite, delegate(ComicBook cb, YesNo x)
									{
										cb.BlackAndWhite = x;
									});
									break;
								case "LinkedAsText":
									DrawCheckBox(drawInfo.Graphics, comicListField, rectangle, Comic.IsLinked ? YesNo.Yes : YesNo.No);
									break;
								case "Checked":
									DrawCheckBox(drawInfo.Graphics, comicListField, rectangle, Comic.Checked ? YesNo.Yes : YesNo.No, delegate(ComicBook cb, YesNo x)
									{
										cb.Checked = x == YesNo.Yes;
									}, onlyYesNo: true);
									break;
								case "EnableProposedAsText":
									DrawCheckBox(drawInfo.Graphics, comicListField, rectangle, Comic.EnableProposed ? YesNo.Yes : YesNo.No, delegate(ComicBook cb, YesNo x)
									{
										cb.EnableProposed = x == YesNo.Yes;
									}, onlyYesNo: true);
									break;
								case "HasBeenReadAsText":
									DrawCheckBox(drawInfo.Graphics, comicListField, rectangle, Comic.HasBeenRead ? YesNo.Yes : YesNo.No, delegate(ComicBook cb, YesNo x)
									{
										cb.HasBeenRead = x == YesNo.Yes;
									}, onlyYesNo: true);
									break;
								case "GapInformation":
									DrawGapInfo(drawInfo.Graphics, comicListField, rectangle, GapType);
									break;
								default:
								{
									stringFormat.Alignment = drawInfo.Header.Alignment;
									stringFormat.LineAlignment = StringAlignment.Center;
									stringFormat.Trimming = comicListField.Trimming;
									stringFormat.FormatFlags = StringFormatFlags.LineLimit;
									string text;
									string b;
									if (displayProperty == "Position")
									{
										text = (b = Position.ToString());
									}
									else
									{
										text = GetColumnStringValue(displayProperty, drawInfo.Header.FormatId, comicListField.ValueType, proposed: true, comicListField.DefaultText);
										b = GetColumnStringValue(displayProperty, drawInfo.Header.FormatId, comicListField.ValueType, proposed: false, comicListField.DefaultText);
									}
									if (base.Focused)
									{
										rectangle = rectangle.Pad(0, 0, DrawSearchLinkButton(drawInfo.Graphics, rectangle, displayProperty, text));
										if (Comic.EditMode.CanEditProperties() && !Program.ExtendedSettings.DisableListSpinButtons)
										{
											switch (displayProperty)
											{
											case "VolumeAsText":
												rectangle.Width -= DrawSpinUpDownButton(drawInfo.Graphics, comicListField, rectangle, Comic.Volume, delegate(ComicBook cb, int old, int x)
												{
													cb.Volume = x.Clamp(-1, int.MaxValue);
												});
												break;
											case "MonthAsText":
												rectangle.Width -= DrawSpinUpDownButton(drawInfo.Graphics, comicListField, rectangle, Comic.Month, delegate(ComicBook cb, int old, int x)
												{
													cb.Month = OneClamping(12, old, x);
												});
												break;
											case "DayAsText":
												rectangle.Width -= DrawSpinUpDownButton(drawInfo.Graphics, comicListField, rectangle, Comic.Day, delegate(ComicBook cb, int old, int x)
												{
													cb.Day = OneClamping(31, old, x);
												});
												break;
											case "YearAsText":
												rectangle.Width -= DrawSpinUpDownButton(drawInfo.Graphics, comicListField, rectangle, Comic.Year, delegate(ComicBook cb, int old, int x)
												{
													cb.Year = OneClamping(10000, old, x);
												});
												break;
											case "NumberAsText":
												rectangle.Width -= DrawSpinUpDownButton(drawInfo.Graphics, comicListField, rectangle, (int)Comic.CompareNumber.Number, delegate(ComicBook cb, int old, int x)
												{
													cb.Number = x.ToString();
												});
												break;
											case "CountAsText":
												rectangle.Width -= DrawSpinUpDownButton(drawInfo.Graphics, comicListField, rectangle, Comic.Count, delegate(ComicBook cb, int old, int x)
												{
													cb.Count = OneClamping(int.MaxValue, old, x);
												});
												break;
											case "AlternateNumberAsText":
												rectangle.Width -= DrawSpinUpDownButton(drawInfo.Graphics, comicListField, rectangle, (int)Comic.CompareAlternateNumber.Number, delegate(ComicBook cb, int old, int x)
												{
													cb.AlternateNumber = x.ToString();
												});
												break;
											case "AlternateCountAsText":
												rectangle.Width -= DrawSpinUpDownButton(drawInfo.Graphics, comicListField, rectangle, Comic.AlternateCount, delegate(ComicBook cb, int old, int x)
												{
													cb.AlternateCount = OneClamping(int.MaxValue, old, x);
												});
												break;
											case "PagesAsTextSimple":
												if (!Comic.IsLinked)
												{
													rectangle.Width -= DrawSpinUpDownButton(drawInfo.Graphics, comicListField, rectangle, Comic.PageCount, delegate(ComicBook cb, int old, int x)
													{
														cb.PageCount = x.Clamp(0, int.MaxValue);
													});
												}
												break;
											}
										}
									}
									rectangle.Inflate(-2, 0);
									using (Brush brush = new SolidBrush((text == b) ? textColor : Color.FromArgb(128, textColor)))
									{
										drawInfo.Graphics.DrawString(text, font, brush, rectangle, stringFormat);
									}
									break;
								}
								}
							}
							else if (drawInfo.GroupItem % 2 == 0 && (drawInfo.State & ItemViewStates.Selected) == 0)
							{
								using (Brush brush2 = new SolidBrush(Color.LightGray.Transparent(96)))
								{
									drawInfo.Graphics.FillRectangle(brush2, rectangle);
								}
							}
							break;
						}
					}
					finally
					{
						itemLock2?.Dispose();
					}
				}
			}
		}

		private static int OneClamping(int max, int oldValue, int newValue)
		{
			if (newValue > max)
			{
				return max;
			}
			if (newValue < 1)
			{
				if (oldValue <= newValue)
				{
					return 1;
				}
				return -1;
			}
			return newValue;
		}

		private static void AddColumnUndo(string columnName)
		{
			if (!string.IsNullOrEmpty(columnName))
			{
				Program.Database.Undo.SetMarker(StringUtility.Format(TR.Messages["UndoEditColumn", "Edit Column '{0}'"], columnName));
			}
		}

		private void SetSelectedComics(Action<ComicBook> setFunction, string columnName = null)
		{
			if (base.View != null)
			{
				AddColumnUndo(columnName);
				(from item in base.View.SelectedItems.Concat(ListExtensions.AsEnumerable<IViewableItem>(base.View.FocusedItem))
					where item != null
					select item).Distinct().OfType<CoverViewItem>().ToArray()
					.ForEach(delegate(CoverViewItem cvi)
					{
						setFunction(cvi.Comic);
					});
			}
		}

		private int DrawSpinUpDownButton(Graphics gr, ComicListField clf, Rectangle rc, int start, Action<ComicBook, int, int> setFunction)
		{
			if (rc.Width < 16)
			{
				return 0;
			}
			Rectangle src = new Rectangle(rc.Right - FormUtility.ScaleDpiX(11), rc.Top + 3, FormUtility.ScaleDpiX(11), rc.Height - 5);
			SpinButton.Draw(gr, src, styleMode: false);
			AddClickRegion(src, delegate(Rectangle rect, Point pt)
			{
				SetSelectedComics(delegate(ComicBook cb)
				{
					switch (SpinButton.HitTest(src, pt.Add(rect.Location)))
					{
					case SpinButton.SpinButtonType.Up:
						setFunction(cb, start, start + 1);
						break;
					case SpinButton.SpinButtonType.Down:
						setFunction(cb, start, start - 1);
						break;
					case SpinButton.SpinButtonType.None:
						break;
					}
				}, clf.Description);
			});
			return src.Width;
		}

		private void DrawRating(Graphics gr, ComicListField clf, Rectangle rc, float rating, Image image, Action<ComicBook, float> setFunction = null)
		{
			Rectangle bounds = rc.Pad(2, 4, 2, 4);
			RatingRenderer r = new RatingRenderer(image, bounds)
			{
				Fast = base.View.InScrollOrResize
			};
			if (setFunction != null && Comic.EditMode.CanEditProperties())
			{
				AddClickRegion(bounds, delegate(Rectangle rect, Point pt)
				{
					SetSelectedComics(delegate(ComicBook cb)
					{
						setFunction(cb, r.GetRatingFromStrip(pt.Add(rect.Location)));
					}, clf.Description);
				});
			}
			r.DrawRatingStrip(gr, rating, 1f, 0.1f);
		}

		private void DrawCheckBox(Graphics gr, ComicListField clf, Rectangle rc, YesNo yesNo, Action<ComicBook, YesNo> setFunction = null, bool onlyYesNo = false)
		{
			int num = Math.Min(rc.Width - 4, rc.Height - 6);
			if (num < 4)
			{
				return;
			}
			Rectangle rectangle = new Rectangle(0, 0, num, num).Align(rc, ContentAlignment.MiddleCenter);
			int num2;
			switch (yesNo)
			{
			default:
				num2 = -1;
				break;
			case YesNo.Yes:
				num2 = 0;
				break;
			case YesNo.Unknown:
				num2 = 1;
				break;
			}
			YesNo newState = (YesNo)num2;
			int num3;
			switch (yesNo)
			{
			default:
				num3 = 0;
				break;
			case YesNo.Yes:
				num3 = 1024;
				break;
			case YesNo.Unknown:
				num3 = 256;
				break;
			}
			ButtonState buttonState = (ButtonState)num3;
			ControlPaint.DrawCheckBox(gr, rectangle, ButtonState.Flat | buttonState);
			if (onlyYesNo && newState == YesNo.Unknown)
			{
				newState = YesNo.Yes;
			}
			if (setFunction == null || !Comic.EditMode.CanEditProperties())
			{
				return;
			}
			AddClickRegion(rectangle, delegate
			{
				SetSelectedComics(delegate(ComicBook cb)
				{
					setFunction(cb, newState);
				}, clf.Description);
			});
		}

		private void DrawGapInfo(Graphics graphics, ComicListField clf, Rectangle rc, ItemGapType gapType)
		{
			Bitmap bitmap = null;
			if (gapType.HasFlag(ItemGapType.End) && gapType.HasFlag(ItemGapType.Start))
			{
				bitmap = bitmapGapUpDown;
			}
			else if (gapType.HasFlag(ItemGapType.End))
			{
				bitmap = bitmapGapUp;
			}
			else if (gapType.HasFlag(ItemGapType.Start))
			{
				bitmap = bitmapGapDown;
			}
			if (bitmap != null)
			{
				using (base.View.InScrollOrResize ? null : graphics.HighQuality(enabled: true))
				{
					Rectangle bounds = rc.Pad(2, 4, 2, 4);
					float scale = bitmap.Size.GetScale(bounds.Size, ScaleMode.FitAll, allowOversize: false);
					graphics.DrawImage(bitmap, bitmap.Size.Scale(scale).Align(bounds, ContentAlignment.MiddleCenter));
				}
			}
		}

		private void CreateThumbnailLines(ICollection<TextLine> lines, Font f, Color textColor)
		{
			StringFormat stringFormat = new StringFormat
			{
				Alignment = StringAlignment.Center
			};
			if (base.View.IsStack(this))
			{
				int stackCount = base.View.GetStackCount(this);
				lines.Add(new TextLine(base.View.GetStackCaption(this), f, textColor, stringFormat));
				lines.Add(new TextLine(StringUtility.Format("{0} {1}", stackCount, (stackCount > 1) ? TR.Default["Books", "books"] : TR.Default["Book", "book"]), FC.GetRelative(f, 0.8f), textColor, stringFormat));
				return;
			}
			if (ThumbnailConfig == null || ThumbnailConfig.CaptionIds.Count == 0)
			{
				stringFormat.Trimming = StringTrimming.EllipsisWord;
				lines.Add(new TextLine(Text, f, textColor, stringFormat));
				return;
			}
			int count = ThumbnailConfig.CaptionIds.Count;
			for (int i = 0; i < count; i++)
			{
				int id = ThumbnailConfig.CaptionIds[i];
				IColumn column = base.View.Columns.FindById(id);
				if (column == null)
				{
					continue;
				}
				ComicListField comicListField = column.Tag as ComicListField;
				if (comicListField == null)
				{
					continue;
				}
				string columnStringValue = GetColumnStringValue(comicListField.DisplayProperty, 0, comicListField.ValueType, proposed: true, comicListField.DefaultText);
				if (!string.IsNullOrEmpty(columnStringValue))
				{
					stringFormat.FormatFlags = ((count != 1) ? StringFormatFlags.NoWrap : ((StringFormatFlags)0));
					stringFormat.Trimming = StringTrimming.EllipsisCharacter;
					if (lines.Count == 1)
					{
						f = FC.GetRelative(f, 0.8f);
					}
					lines.Add(new TextLine(columnStringValue, f, textColor, stringFormat));
				}
			}
		}

		private ComicBook CreateStackInfo()
		{
			ComicBook comicBook = new ComicBook
			{
				Series = base.View.GetStackCaption(this),
				Count = base.View.GetStackCount(this)
			};
			bool flag = true;
			foreach (ComicBook item in from ci in base.View.GetStackItems(this).OfType<CoverViewItem>()
				select ci.Comic)
			{
				if (flag)
				{
					comicBook.AppendArtistInfo(item);
					flag = string.IsNullOrEmpty(comicBook.ArtistInfo);
				}
				if (item.HasBeenOpened)
				{
					comicBook.LastPageRead++;
				}
				comicBook.FileSize += Math.Max(0L, item.FileSize);
				comicBook.PageCount += item.PageCount;
			}
			return comicBook;
		}

		protected override void CreateThumbnail(ThumbnailKey key)
		{
			using (Program.ImagePool.GetThumbnail(key, Comic))
			{
			}
		}

		private void Comic_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged("Comic");
			OnComicPropertyChanged(e.PropertyName);
			OnBookChanged(e);
		}

		public string GetColumnStringValue(string displayProperty, int formatId, Type type, bool proposed = false, string defaultText = null)
		{
			try
			{
				if (type != typeof(DateTime))
				{
					return GetColumnValue<string>(displayProperty, proposed) ?? defaultText ?? string.Empty;
				}
				DateTime columnValue = GetColumnValue<DateTime>(displayProperty, proposed);
				return ComicBook.FormatDate(columnValue, (ComicDateFormat)formatId, toLocal: false, defaultText);
			}
			catch (Exception)
			{
				return defaultText ?? string.Empty;
			}
		}

		public T GetColumnValue<T>(string displayProperty, bool proposed = false)
		{
			if (StatsProvider == null || !displayProperty.StartsWith(SeriesStatsPrefix))
			{
				return Comic.GetPropertyValue<T>(displayProperty, proposed);
			}
			ComicBookSeriesStatistics seriesStats = StatsProvider.GetSeriesStats(Comic);
			displayProperty = displayProperty.Substring(SeriesStatsPrefixLength);
			return PropertyCaller.CreateGetMethod<ComicBookSeriesStatistics, T>(displayProperty)(seriesStats);
		}

		public IItemLock<ThumbnailImage> GetBackThumbnail()
		{
			int page = ((Comic.CurrentPage <= 0) ? Comic.FirstNonCoverPageIndex : Comic.CurrentPage);
			ThumbnailKey tk = Comic.GetThumbnailKey(page);
			IItemLock<ThumbnailImage> image = Program.ImagePool.Thumbs.GetImage(tk, memoryOnly: true);
			if (image == null)
			{
				Program.ImagePool.AddThumbToQueue(tk, base.View, delegate
				{
					MakePageThumbnail(tk, updateSize: false);
				});
			}
			return image;
		}

		public void DrawFrontCoverButton(Graphics graphics, Rectangle thumbnailBounds)
		{
			if (HasCustomThumbnail || Comic.FrontCoverCount <= 1 || !base.Selected)
			{
				return;
			}
			string text = $"- {Comic.PreferredFrontCover + 1}/{Comic.FrontCoverCount} +";
			Font font = FC.Get("Arial", 7f);
			thumbnailBounds.Inflate(-2, -2);
			Rectangle rectangle = new Rectangle(Point.Empty, graphics.MeasureString(text, font).ToSize());
			rectangle.Inflate(2, 2);
			rectangle = rectangle.Align(thumbnailBounds, ContentAlignment.BottomCenter);
			using (graphics.AntiAlias())
			{
				using (GraphicsPath path = rectangle.ConvertToPath(4, 4))
				{
					using (StringFormat format = new StringFormat
					{
						LineAlignment = StringAlignment.Center,
						Alignment = StringAlignment.Center
					})
					{
						graphics.FillPath(Brushes.White, path);
						graphics.DrawString(text, font, Brushes.Black, rectangle, format);
						graphics.DrawPath(Pens.Black, path);
					}
				}
			}
			AddClickRegion(rectangle, ClickCoverButton);
		}

		private void ClickCoverButton(Rectangle bounds, Point pt)
		{
			int add = ((pt.X >= bounds.Width / 2) ? 1 : (-1));
			Comic.PreferredFrontCover = Numeric.Rollover(Comic.PreferredFrontCover, Comic.FrontCoverCount, add);
		}

		private int DrawSearchLinkButton(Graphics graphics, Rectangle rc, string field, string text)
		{
			if (!Program.Settings.ShowSearchLinks || string.IsNullOrEmpty(text) || !Comic.IsSearchable(field))
			{
				return 0;
			}
			Rectangle rectangle = linkArrow.Size.ToRectangle().Align(rc, ContentAlignment.MiddleRight);
			graphics.DrawImage(linkArrow, rectangle);
			AddClickRegion(rectangle, delegate(Rectangle ib, Point ipt)
			{
				Point location = base.View.Translate(base.View.GetItemBounds(this), fromClient: false).Location;
				location.Offset(ib.Right, ib.Top);
				ContextMenuStrip contextMenuStrip = new SearchContextMenuBuilder().CreateContextMenu(SearchEngines.Engines, field, text, delegate(ContextMenuStrip cms)
				{
					if (!string.IsNullOrEmpty(Comic.Web))
					{
						cms.Items.Add(new ToolStripSeparator());
						cms.Items.Add(Comic.Web, null, delegate
						{
							Program.StartDocument(Comic.Web);
						});
					}
				});
				contextMenuStrip.Show(base.View, location);
			});
			return rectangle.Width;
		}

		public override Control GetEditControl(int subItem)
		{
			try
			{
				IColumn column = base.View.Columns[subItem];
				ComicListField comicListField = (ComicListField)column.Tag;
				if (!Comic.EditMode.CanEditProperties() || string.IsNullOrEmpty(comicListField.EditProperty))
				{
					return null;
				}
				AddColumnUndo(comicListField.Description);
				AutoSizeTextBox autoSizeTextBox = null;
				Comic.RefreshInfoFromFile();
				switch (comicListField.EditProperty)
				{
				case "Series":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.ShadowSeries, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.ShadowSeries));
					break;
				case "Title":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.ShadowTitle, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.ShadowTitle));
					break;
				case "Format":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.ShadowFormat, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.ShadowFormat));
					break;
				case "Year":
					autoSizeTextBox = new AutoSizeTextBox();
					autoSizeTextBox.EnableOnlyNumberKeys();
					EditControlUtility.SetText(autoSizeTextBox, Comic.ShadowYear);
					break;
				case "Month":
					autoSizeTextBox = new AutoSizeTextBox();
					autoSizeTextBox.EnableOnlyNumberKeys();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Month);
					break;
				case "Day":
					autoSizeTextBox = new AutoSizeTextBox();
					autoSizeTextBox.EnableOnlyNumberKeys();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Day);
					break;
				case "Number":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.ShadowNumber);
					break;
				case "AlternateNumber":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.AlternateNumber);
					break;
				case "AlternateSeries":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.AlternateSeries, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.AlternateSeries));
					break;
				case "StoryArc":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.StoryArc, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.StoryArc));
					break;
				case "SeriesGroup":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.SeriesGroup, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.SeriesGroup));
					break;
				case "Count":
					autoSizeTextBox = new AutoSizeTextBox();
					autoSizeTextBox.EnableOnlyNumberKeys();
					EditControlUtility.SetText(autoSizeTextBox, Comic.ShadowCount);
					break;
				case "AlternateCount":
					autoSizeTextBox = new AutoSizeTextBox();
					autoSizeTextBox.EnableOnlyNumberKeys();
					EditControlUtility.SetText(autoSizeTextBox, Comic.AlternateCount);
					break;
				case "Volume":
					autoSizeTextBox = new AutoSizeTextBox();
					autoSizeTextBox.EnableOnlyNumberKeys();
					EditControlUtility.SetText(autoSizeTextBox, Comic.ShadowVolume);
					break;
				case "Writer":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Writer, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Writer));
					break;
				case "Inker":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Inker, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Inker));
					break;
				case "Letterer":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Letterer, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Letterer));
					break;
				case "CoverArtist":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.CoverArtist, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.CoverArtist));
					break;
				case "Editor":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Editor, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Editor));
					break;
				case "Translator":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Translator, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Translator));
					break;
				case "Colorist":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Colorist, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Colorist));
					break;
				case "Penciller":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Penciller, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Penciller));
					break;
				case "Genre":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Genre, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Genre));
					break;
				case "Publisher":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Publisher, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Publisher));
					break;
				case "Imprint":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Imprint, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Imprint));
					break;
				case "FileName":
					if (!Comic.IsLinked)
					{
						break;
					}
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.FileName, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.FileName));
					break;
				case "Tags":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Tags, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Tags));
					break;
				case "Review":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Review, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Review));
					break;
				case "Characters":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Characters, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Characters));
					break;
				case "Teams":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Teams, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Teams));
					break;
				case "MainCharacterOrTeam":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.MainCharacterOrTeam, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.MainCharacterOrTeam));
					break;
				case "Locations":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.Locations, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Locations));
					break;
				case "ScanInformation":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.ScanInformation, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.ScanInformation));
					break;
				case "BookAge":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.BookAge, () => Program.Lists.GetBookAgeList());
					break;
				case "BookStore":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.BookStore, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookStore));
					break;
				case "BookLocation":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.BookLocation, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookLocation));
					break;
				case "BookOwner":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.BookOwner, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookOwner));
					break;
				case "BookPriceAsText":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.BookPriceAsText, () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookPriceAsText));
					break;
				case "BookCondition":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.BookCondition, () => Program.Lists.GetBookConditionList());
					break;
				case "BookCollectionStatus":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.BookCollectionStatus, () => Program.Lists.GetBookCollectionStatusList());
					break;
				case "PagesAsTextSimple":
					if (!Comic.IsLinked)
					{
						autoSizeTextBox = new AutoSizeTextBox();
						autoSizeTextBox.EnableOnlyNumberKeys();
						EditControlUtility.SetText(autoSizeTextBox, Comic.PagesAsTextSimple);
					}
					break;
				case "ISBN":
					autoSizeTextBox = new AutoSizeTextBox();
					EditControlUtility.SetText(autoSizeTextBox, Comic.ISBN);
					break;
				default:
					if (comicListField.EditProperty.StartsWith("{"))
					{
						autoSizeTextBox = new AutoSizeTextBox();
						EditControlUtility.SetText(autoSizeTextBox, Comic.GetStringPropertyValue(comicListField.EditProperty));
					}
					break;
				}
				if (autoSizeTextBox == null)
				{
					return null;
				}
				TextBoxContextMenu.Register(autoSizeTextBox);
				autoSizeTextBox.AcceptsTab = false;
				autoSizeTextBox.Tag = comicListField.EditProperty;
				autoSizeTextBox.MinimumSize = new Size(40, 10);
				autoSizeTextBox.KeyDown += Editor_KeyDown;
				autoSizeTextBox.VisibleChanged += Editor_VisibleChanged;
				autoSizeTextBox.Multiline = false;
				autoSizeTextBox.AutoSizeEnabled = false;
				autoSizeTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
				autoSizeTextBox.HandleTab = true;
				return autoSizeTextBox;
			}
			catch
			{
				return null;
			}
		}

		private void Editor_VisibleChanged(object sender, EventArgs e)
		{
			Control c = sender as Control;
			if (c == null || c.Visible)
			{
				return;
			}
			string name = c.Tag as string;
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			string text = c.Text.Trim();
			switch (name)
			{
			case "Series":
				if (Comic.ShadowSeries != text)
				{
					SetSelectedComics(delegate(ComicBook cb)
					{
						cb.Series = text;
					});
				}
				return;
			case "Title":
				if (Comic.ShadowTitle != text)
				{
					SetSelectedComics(delegate(ComicBook cb)
					{
						cb.Title = text;
					});
				}
				return;
			case "Year":
				if (Comic.ShadowYear != EditControlUtility.GetNumber(c))
				{
					SetSelectedComics(delegate(ComicBook cb)
					{
						cb.Year = EditControlUtility.GetNumber(c);
					});
				}
				return;
			case "Number":
				if (Comic.ShadowNumber != text)
				{
					SetSelectedComics(delegate(ComicBook cb)
					{
						cb.Number = text;
					});
				}
				return;
			case "Volume":
				if (Comic.ShadowVolume != EditControlUtility.GetNumber(c))
				{
					SetSelectedComics(delegate(ComicBook cb)
					{
						cb.Volume = EditControlUtility.GetNumber(c);
					});
				}
				return;
			case "Count":
				if (Comic.ShadowCount != EditControlUtility.GetNumber(c))
				{
					SetSelectedComics(delegate(ComicBook cb)
					{
						cb.Count = EditControlUtility.GetNumber(c);
					});
				}
				return;
			case "AlternateCount":
				SetSelectedComics(delegate(ComicBook cb)
				{
					cb.AlternateCount = EditControlUtility.GetNumber(c);
				});
				return;
			case "Month":
				SetSelectedComics(delegate(ComicBook cb)
				{
					cb.Month = EditControlUtility.GetNumber(c);
				});
				return;
			case "Day":
				SetSelectedComics(delegate(ComicBook cb)
				{
					cb.Day = EditControlUtility.GetNumber(c);
				});
				return;
			case "FileName":
				Comic.RenameFile(text);
				return;
			}
			if (!name.StartsWith("{"))
			{
				SetSelectedComics(delegate(ComicBook cb)
				{
					cb.SetValue(name, text);
				});
				return;
			}
			name = name.Substring(1, name.Length - 2);
			SetSelectedComics(delegate(ComicBook cb)
			{
				cb.SetCustomValue(name, text);
			});
		}

		private static void Editor_KeyDown(object sender, KeyEventArgs e)
		{
			Control control = (Control)sender;
			switch (e.KeyCode)
			{
			case Keys.Escape:
				e.Handled = true;
				control.Tag = null;
				control.Hide();
				break;
			case Keys.Return:
				e.Handled = true;
				control.Hide();
				break;
			}
		}

		public bool Contains(Point pt)
		{
			if (drawnRect.HasValue)
			{
				return drawnRect.Value.Contains(pt);
			}
			return true;
		}

		public bool IntersectsWith(Rectangle rc)
		{
			if (drawnRect.HasValue)
			{
				return drawnRect.Value.IntersectsWith(rc);
			}
			return true;
		}

		protected virtual void OnRefreshComicData()
		{
			if (!refreshed)
			{
				Program.QueueManager.AddBookToRefreshComicData(Comic);
				refreshed = true;
			}
		}

		protected void OnDrawCustomThumbnailOverlay(Graphics gr, Rectangle bounds)
		{
			if (CoverViewItem.DrawCustomThumbnailOverlay != null)
			{
				using (gr.SaveState())
				{
					gr.SetClip(bounds, CombineMode.Intersect);
					gr.TranslateTransform(bounds.X, bounds.Y);
					Rectangle bounds2 = new Rectangle(0, 0, bounds.Width + 1, bounds.Height + 1);
					int flags = BitUtility.CreateMask(base.Hot, base.Selected, base.View.IsStack(this));
					CoverViewItem.DrawCustomThumbnailOverlay(Comic, gr, bounds2, flags);
				}
			}
		}
	}
}
