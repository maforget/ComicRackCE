using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Presentation;
using cYo.Common.Presentation.Panels;
using cYo.Common.Threading;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Display.Forms.Properties;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Cache;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine.Display.Forms
{
	public class NavigationOverlay : OverlayPanel
	{
		private struct IndexRectangle
		{
			public Rectangle Bounds;

			public int Page;
		}

		private static readonly int ButtonSize = FormUtility.ScaleDpiX(28);

		private static readonly int PagePadding = FormUtility.ScaleDpiX(6);

		private static readonly int w1 = FormUtility.ScaleDpiX(1);

		private static readonly int w2 = FormUtility.ScaleDpiX(2);

		private static readonly int w3 = FormUtility.ScaleDpiX(3);

		private static readonly int w4 = FormUtility.ScaleDpiX(4);

		private readonly SimpleScrollbarPanel pageSlider;

		private readonly LabelPanel comicNameLabel;

		private readonly LabelPanel timeLabel;

		private readonly BatteryStatus batteryStatus;

		private readonly ScalableBitmap thumbBack = new ScalableBitmap(Resources.GradientFramedBackground, 0, 3, 0, 3);

		private readonly BitmapAdjustment adjustment = new BitmapAdjustment(-1f);

		private bool mirror;

		private int[] pages = new int[0];

		private bool isDoublePage;

		private IThumbnailPool pool;

		private IImageProvider provider;

		private int selectedPage = -1;

		private Point downPoint;

		private int downPage = -1;

		private bool thumbnailScroll;

		private readonly List<IndexRectangle> thumbnailAreas = new List<IndexRectangle>();

		public bool Mirror
		{
			get
			{
				return mirror;
			}
			set
			{
				if (mirror != value)
				{
					mirror = value;
					pageSlider.Mirror = mirror;
					Invalidate();
				}
			}
		}

		public int[] Pages
		{
			get
			{
				return pages;
			}
			set
			{
				pages = value;
				pageSlider.Maximum = pages.Length - 1;
			}
		}

		public int DisplayedPageIndex
		{
			get
			{
				return pageSlider.Value;
			}
			set
			{
				if (value != -1 && pageSlider.Value != value)
				{
					pageSlider.Value = value;
				}
			}
		}

		public bool IsDoublePage
		{
			get
			{
				return isDoublePage;
			}
			set
			{
				if (isDoublePage != value)
				{
					isDoublePage = value;
					Invalidate();
				}
			}
		}

		public IThumbnailPool Pool
		{
			get
			{
				return pool;
			}
			set
			{
				if (pool != value)
				{
					if (pool != null)
					{
						pool.ThumbnailCached -= MemoryThumbnailCacheItemAdded;
					}
					pool = value;
					if (pool != null)
					{
						pool.ThumbnailCached += MemoryThumbnailCacheItemAdded;
					}
					Invalidate();
				}
			}
		}

		public IImageProvider Provider
		{
			get
			{
				return provider;
			}
			set
			{
				if (provider != value)
				{
					provider = value;
					Invalidate();
				}
			}
		}

		public IImageKeyProvider ImageKeyProvider
		{
			get;
			set;
		}

		public string Caption
		{
			get
			{
				return comicNameLabel.Text;
			}
			set
			{
				comicNameLabel.Text = value;
			}
		}

		public int SelectedPage
		{
			get
			{
				return selectedPage;
			}
			set
			{
				if (selectedPage != value)
				{
					Invalidate(selectedPage);
					selectedPage = value;
					Invalidate(selectedPage);
				}
			}
		}

		public event EventHandler<BrowseEventArgs> Browse;

		public NavigationOverlay(Size size)
			: base(size)
		{
			base.Margin = PanelRenderer.GetMargin(base.ClientRectangle);
			AddButton(ButtonSize, ContentAlignment.BottomLeft, 0, Resources.GoFirst.ScaleDpi(), PageBrowseLeftDoubleClick);
			AddButton(ButtonSize, ContentAlignment.BottomLeft, ButtonSize + w4, Resources.GoPrevious.ScaleDpi(), PageBrowseLeftClick);
			AddButton(ButtonSize, ContentAlignment.BottomRight, -ButtonSize - w4, Resources.GoNext.ScaleDpi(), PageBrowseRightClick);
			AddButton(ButtonSize, ContentAlignment.BottomRight, 0, Resources.GoLast.ScaleDpi(), PageBrowseRightDoubleClick);
			pageSlider = new SimpleScrollbarPanel(GetScrollbarSize(size))
			{
				HitTestType = HitTestType.Bounds,
				BackColor = Color.Transparent,
				Visible = true,
				AutoAlign = true,
				Alignment = ContentAlignment.BottomCenter,
				AlignmentOffset = new Point(0, -w2),
				Background = new ScalableBitmap(Resources.BlackGlassScrollbarBack, new Padding(3)),
				Knob = Resources.GrayGlassButton.Resize(new Size(32, 32).ScaleDpi(), BitmapResampling.GdiPlusHQ)
			};
			pageSlider.BorderWidth = pageSlider.Height / 2 - w3;
			pageSlider.Scroll += PageSliderScroll;
			pageSlider.ValueChanged += delegate
			{
				Invalidate();
			};
			pageSlider.MinimumChanged += delegate
			{
				Invalidate();
			};
			pageSlider.MouseUp += PageSliderMouseUp;
			base.Panels.Add(pageSlider);
			comicNameLabel = new LabelPanel
			{
				Size = GetNameLabelSize(size),
				TextSize = 9f,
				Alignment = ContentAlignment.TopLeft,
				AlignmentOffset = new Point(0, -w1),
				AutoAlign = true,
				Visible = true
			};
			base.Panels.Add(comicNameLabel);
			int num = 0;
			if (SystemInformation.PowerStatus.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery)
			{
				batteryStatus = new BatteryStatus
				{
					Alignment = ContentAlignment.TopRight,
					AlignmentOffset = new Point(0, w3),
					AutoAlign = true,
					Visible = true
				};
				base.Panels.Add(batteryStatus);
				num = batteryStatus.Width + w4;
			}
			timeLabel = new LabelPanel
			{
				Size = GetTimeLabelSize(),
				TextSize = 9f,
				TextAlignment = ContentAlignment.MiddleRight,
				Alignment = ContentAlignment.TopRight,
				AlignmentOffset = new Point(-num, -w1),
				AutoAlign = true,
				Visible = true
			};
			timeLabel.Drawing += delegate
			{
				timeLabel.Text = DateTime.Now.ToString("t");
			};
			base.Panels.Add(timeLabel);
		}

		private Size GetScrollbarSize(Size size)
		{
			return new Size(size.Width - base.Margin.Horizontal - 4 * w4 - 2 * w4 - 4 * ButtonSize, ButtonSize - w4);
		}

		private static Size GetNameLabelSize(Size size)
		{
			return new Size(size.Width - FormUtility.ScaleDpiX(80), FormUtility.ScaleDpiY(15));
		}

		private static Size GetTimeLabelSize()
		{
			return new Size(50, 15).ScaleDpi();
		}

		private void AddButton(int buttonSize, ContentAlignment align, int offset, Bitmap bi, EventHandler click)
		{
			Size size = new Size(buttonSize, buttonSize);
			SimpleButtonPanel simpleButtonPanel = new SimpleButtonPanel(size)
			{
				Background = Resources.GrayGlassButton.Resize(size, BitmapResampling.GdiPlusHQ),
				Icon = bi.CreateAdjustedBitmap(adjustment, alwaysClone: true),
				Margin = new Padding(w4),
				Visible = true,
				AutoAlign = true,
				AlignmentOffset = new Point(offset, 0),
				Alignment = align
			};
			simpleButtonPanel.Click += click;
			base.Panels.Add(simpleButtonPanel);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				base.Panels.Dispose();
				Pool = null;
			}
			base.Dispose(disposing);
		}

		private void MemoryThumbnailCacheItemAdded(object sender, CacheItemEventArgs<ImageKey, ThumbnailImage> e)
		{
			Invalidate();
		}

		private void PageBrowseLeftClick(object sender, EventArgs e)
		{
			OnBrowse(new BrowseEventArgs(PageSeekOrigin.Current, -1));
		}

		private void PageBrowseRightClick(object sender, EventArgs e)
		{
			OnBrowse(new BrowseEventArgs(PageSeekOrigin.Current, 1));
		}

		private void PageBrowseRightDoubleClick(object sender, EventArgs e)
		{
			OnBrowse(new BrowseEventArgs(PageSeekOrigin.End, 0));
		}

		private void PageBrowseLeftDoubleClick(object sender, EventArgs e)
		{
			OnBrowse(new BrowseEventArgs(PageSeekOrigin.Beginning, 0));
		}

		private void PageSliderScroll(object sender, EventArgs e)
		{
			Invalidate();
		}

		private void PageSliderMouseUp(object sender, MouseEventArgs e)
		{
			OnBrowse(new BrowseEventArgs(PageSeekOrigin.Absolute, pages[pageSlider.Value]));
		}

		public void Invalidate(int page)
		{
			if (page < 0)
			{
				return;
			}
			using (ItemMonitor.Lock(thumbnailAreas))
			{
				Invalidate(thumbnailAreas.FirstOrDefault((IndexRectangle ta) => ta.Page == page).Bounds);
			}
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			pageSlider.Size = GetScrollbarSize(base.Size);
			comicNameLabel.Size = GetNameLabelSize(base.Size);
			timeLabel.Size = GetTimeLabelSize();
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			downPoint = e.Location;
			downPage = -1;
			thumbnailScroll = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (downPoint.IsEmpty)
			{
				SelectedPage = PageHitTest(e.Location);
				return;
			}
			if (downPage == -1)
			{
				downPage = PageHitTest(e.Location);
			}
			if (downPage == -1)
			{
				return;
			}
			SelectedPage = downPage;
			IndexRectangle indexRectangle = thumbnailAreas.FirstOrDefault((IndexRectangle ta) => ta.Page == downPage);
			if (indexRectangle.Bounds.Width == 0 || e.Y < indexRectangle.Bounds.Top || e.Y > indexRectangle.Bounds.Bottom)
			{
				return;
			}
			int num = 0;
			if (e.X > indexRectangle.Bounds.Right + PagePadding)
			{
				num = -1;
			}
			else if (e.X < indexRectangle.Bounds.Left - PagePadding)
			{
				num = 1;
			}
			if (num != 0)
			{
				thumbnailScroll = true;
				downPage = -1;
				if (Mirror)
				{
					pageSlider.Value -= num;
				}
				else
				{
					pageSlider.Value += num;
				}
			}
		}

		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			SelectedPage = -1;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			downPoint = Point.Empty;
			int num = (thumbnailScroll ? pages[pageSlider.Value] : PageHitTest(e.Location));
			if (num != -1)
			{
				OnBrowse(new BrowseEventArgs(PageSeekOrigin.Absolute, num));
			}
		}

		protected override void OnScaleChanged()
		{
			base.Panels.ForEach(delegate(OverlayPanel p)
			{
				p.Scale = base.Scale;
			});
			base.OnScaleChanged();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.ClientRectangle;
			bool flag = IsDoublePage;
			clientRectangle = Rectangle.Round(PanelRenderer.DrawGraphics(graphics, clientRectangle, 0.7f));
			clientRectangle = clientRectangle.Pad(0, comicNameLabel.Height, 0, ButtonSize + 4);
			clientRectangle = Rectangle.Round(thumbBack.Draw((BitmapGdiRenderer)graphics, clientRectangle));
			clientRectangle = clientRectangle.Pad(4, 2, 4);
			graphics.IntersectClip(clientRectangle);
			using (ItemMonitor.Lock(thumbnailAreas))
			{
				thumbnailAreas.Clear();
			}
			bool flag2 = false;
			int num = clientRectangle.X + clientRectangle.Width / 2;
			int num2 = num;
			int num3 = DisplayedPageIndex;
			int num4 = DisplayedPageIndex + 1;
			bool flag3 = true;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				Size pageSize = GetPageSize(num3, clientRectangle);
				Rectangle rectangle = pageSize.ToRectangle().AlignHorizontal(num, StringAlignment.Center);
				rectangle.Y = clientRectangle.Y;
				if (clientRectangle.IntersectsWith(rectangle))
				{
					DrawPage(graphics, rectangle, num3, isSelected: true);
					AddPageArea(rectangle, num3);
					flag2 = true;
				}
				num -= (rectangle.Width / 2 + PagePadding) * ((!mirror) ? 1 : (-1));
				num2 += (rectangle.Width / 2 + PagePadding) * ((!mirror) ? 1 : (-1));
				num3--;
				flag3 = false;
			}
			while (flag2)
			{
				flag2 = false;
				Size pageSize2 = GetPageSize(num3, clientRectangle);
				if (!pageSize2.IsEmpty)
				{
					Rectangle rectangle2 = pageSize2.ToRectangle().AlignHorizontal(num, mirror ? StringAlignment.Far : StringAlignment.Near);
					rectangle2.Y = clientRectangle.Y;
					if (clientRectangle.IntersectsWith(rectangle2))
					{
						DrawPage(graphics, rectangle2, num3, flag3 || num3 == SelectedPage);
						AddPageArea(rectangle2, num3);
						num -= (rectangle2.Width + PagePadding) * ((!mirror) ? 1 : (-1));
						num3--;
						flag2 = true;
					}
				}
				pageSize2 = GetPageSize(num4, clientRectangle);
				if (!pageSize2.IsEmpty)
				{
					Rectangle rectangle3 = pageSize2.ToRectangle().AlignHorizontal(num2, (!mirror) ? StringAlignment.Far : StringAlignment.Near);
					rectangle3.Y = clientRectangle.Y;
					if (clientRectangle.IntersectsWith(rectangle3))
					{
						DrawPage(graphics, rectangle3, num4, flag3 || num4 == SelectedPage);
						AddPageArea(rectangle3, num4);
						num2 += (rectangle3.Width + PagePadding) * ((!mirror) ? 1 : (-1));
						num4++;
						flag2 = true;
					}
				}
				flag3 = false;
			}
		}

		protected virtual void OnBrowse(BrowseEventArgs e)
		{
			if (this.Browse != null)
			{
				this.Browse(this, e);
			}
		}

		private int PageHitTest(Point pt)
		{
			return (from ir in thumbnailAreas.Lock()
				where ir.Bounds.Contains(pt)
				select ir.Page).FirstOrValue(-1);
		}

		private IItemLock<ThumbnailImage> GetThumbnail(int pageIndex)
		{
			if (pool == null || ImageKeyProvider == null || pageIndex >= pages.Length)
			{
				return new ItemLock<ThumbnailImage>(null);
			}
			ThumbnailKey key = new ThumbnailKey(ImageKeyProvider.GetImageKey(pages[pageIndex]));
			IItemLock<ThumbnailImage> thumbnail = pool.GetThumbnail(key, onlyMemory: true);
			if (thumbnail != null)
			{
				return thumbnail;
			}
			pool.CacheThumbnail(key, checkMemoryOnly: true, provider);
			return new ItemLock<ThumbnailImage>(null);
		}

		private void AddPageArea(Rectangle trc, int pageIndex)
		{
			using (ItemMonitor.Lock(thumbnailAreas))
			{
				thumbnailAreas.Add(new IndexRectangle
				{
					Bounds = trc,
					Page = pages[pageIndex]
				});
			}
		}

		private Size GetPageSize(int pageIndex, Rectangle rc)
		{
			if (pageIndex < 0 || pageIndex >= pages.Length)
			{
				return Size.Empty;
			}
			using (IItemLock<ThumbnailImage> itemLock = GetThumbnail(pageIndex))
			{
				if (itemLock.Item == null)
				{
					return new Size(rc.Height * 3 / 4, rc.Height);
				}
				return itemLock.Item.OriginalSize.ToRectangle(new Size(0, rc.Height), RectangleScaleMode.None).Size;
			}
		}

		private void DrawPage(Graphics gr, Rectangle trc, int pageIndex, bool isSelected)
		{
			if (!gr.IsVisible(trc) || pageIndex < 0 || pageIndex >= pages.Length)
			{
				return;
			}
			int num = pages[pageIndex];
			using (IItemLock<ThumbnailImage> itemLock = GetThumbnail(pageIndex))
			{
				Image image = ((itemLock.Item == null) ? null : itemLock.Item.GetThumbnail(trc.Height));
				float num2 = (isSelected ? 1f : 0.3f);
				if (image == null)
				{
					using (StringFormat format = new StringFormat
					{
						LineAlignment = StringAlignment.Center,
						Alignment = StringAlignment.Center
					})
					{
						using (Brush brush = new SolidBrush(Color.FromArgb((int)(255f * num2), PanelRenderer.GetForeColor())))
						{
							gr.DrawString((num + 1).ToString(), FC.Get("Times", 32f), brush, trc, format);
						}
					}
				}
				else
				{
					ThumbnailDrawingOptions thumbnailDrawingOptions = ThumbnailDrawingOptions.Default | ThumbnailDrawingOptions.EnablePageNumber | ThumbnailDrawingOptions.KeepAspect | ThumbnailDrawingOptions.DisableMissingThumbnail;
					thumbnailDrawingOptions &= ~ThumbnailDrawingOptions.EnableShadow;
					ThumbRenderer thumbRenderer = new ThumbRenderer(image, thumbnailDrawingOptions)
					{
						PageNumber = num + 1,
						ImageOpacity = num2
					};
					thumbRenderer.DrawThumbnail(gr, trc);
				}
			}
		}
	}
}
