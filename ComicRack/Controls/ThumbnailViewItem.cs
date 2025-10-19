using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;
using cYo.Common.Runtime;
using cYo.Common.Threading;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public abstract class ThumbnailViewItem : ItemViewItem, IDisposable
	{
		private class ClickRegion
		{
			public Rectangle Bounds;

			public Action<Rectangle, Point> Click;
		}

		public const int RowColumnBorder = 2;

		public const int FadeInTime = 300;

		public const int FadeInSteps = 25;

		public const float FontHeightPercent = 0.07f;

		public const float MinFontSize = 0.8f;

		public const float MaxFontSize = 1f;

		public static readonly Bitmap DeletedStateImage = Resources.Deleted;

		private volatile float opacity = 1f;

		private Size border = new Size(4, 4);

		private volatile bool disposed;

		private static Timer animationTimer;

		private static readonly LinkedList<ThumbnailViewItem> animatedItems = new LinkedList<ThumbnailViewItem>();

		private static long animationTime;

		private bool validImage = true;

		private List<ClickRegion> clickRegions;

		public float Opacity
		{
			get
			{
				return opacity;
			}
			set
			{
				value = value.Clamp(0f, 1f);
				if (!opacity.CompareTo(value, 0.01f))
				{
					opacity = value;
					Update();
				}
			}
		}

		public Size Border
		{
			get
			{
				return border;
			}
			set
			{
				if (!(border == value))
				{
					border = value;
					Update(sizeChanged: true);
				}
			}
		}

		public abstract ThumbnailKey ThumbnailKey
		{
			get;
		}

		public bool IsDisposed => disposed;

		~ThumbnailViewItem()
		{
			Dispose(disposing: false);
		}

		public void RefreshImage()
		{
			OnRefreshImage();
			Update(sizeChanged: true);
		}

		protected IItemLock<ThumbnailImage> GetThumbnail(ThumbnailKey key, bool memoryOnly)
		{
			IItemLock<ThumbnailImage> image = Program.ImagePool.Thumbs.GetImage(key, memoryOnly);
			if (image == null)
			{
				Program.ImagePool.AddThumbToQueue(key, base.View, delegate
				{
					MakePageThumbnail(key);
				});
			}
			return image;
		}

		public IItemLock<ThumbnailImage> GetThumbnail(bool memoryOnly)
		{
			return GetThumbnail(ThumbnailKey, memoryOnly);
		}

		public IItemLock<ThumbnailImage> GetThumbnail(ItemDrawInformation drawInfo)
		{
			if (drawInfo != null && drawInfo.DisplayType == ItemViewMode.Detail)
			{
				if (drawInfo.Header == null)
				{
					return null;
				}
				ComicListField comicListField = drawInfo.Header.Tag as ComicListField;
				if (comicListField != null && comicListField.DisplayProperty != "Cover" && comicListField.DisplayProperty != "Thumbnail")
				{
					return null;
				}
			}
			return GetThumbnail(memoryOnly: true);
		}

		private static void AddAnimation(ThumbnailViewItem item)
		{
			if (animationTimer == null)
			{
				animationTimer = new Timer(12.0)
				{
					AutoReset = true
				};
				animationTimer.Elapsed += animationTimer_Elapsed;
			}
			using (ItemMonitor.Lock(animatedItems))
			{
				if (!(item.Opacity >= 0.95f) && !animatedItems.Contains(item))
				{
					animatedItems.AddLast(item);
					if (!animationTimer.Enabled)
					{
						animationTime = 0L;
						animationTimer.Start();
					}
				}
			}
		}

		private static void animationTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			using (ItemMonitor.Lock(animatedItems))
			{
				long ticks = Machine.Ticks;
				long num = ((animationTime == 0L) ? 12 : (ticks - animationTime));
				animationTime = ticks;
				LinkedListNode<ThumbnailViewItem> linkedListNode = animatedItems.First;
				bool flag = false;
				while (linkedListNode != null)
				{
					LinkedListNode<ThumbnailViewItem> next = linkedListNode.Next;
					try
					{
						ThumbnailViewItem value = linkedListNode.Value;
						if (value.View == null || value.disposed)
						{
							animatedItems.Remove(linkedListNode);
							continue;
						}
						value.Opacity += (float)num / 300f;
						if (value.Opacity >= 1f)
						{
							animatedItems.Remove(linkedListNode);
						}
						else
						{
							flag = true;
						}
					}
					catch
					{
						animatedItems.Remove(linkedListNode);
					}
					finally
					{
						linkedListNode = next;
					}
				}
				if (!flag)
				{
					animationTimer.Stop();
				}
			}
		}

		public void Animate(Image image)
		{
			if (!Program.Settings.FadeInThumbnails)
			{
				return;
			}
			if (image == null)
			{
				validImage = false;
				return;
			}
			if (!validImage)
			{
				opacity = 0f;
				AddAnimation(this);
			}
			validImage = true;
		}

		protected virtual Size GetDefaultMaximumSize(Size defaultSize)
		{
			int height = defaultSize.Height;
			return new Size(height * 2, height);
		}

		protected virtual Size GetEstimatedSize(Size canvasSize)
		{
			return Size.Empty;
		}

		protected Size GetThumbnailSizeSafe(Size defaultSize)
		{
			using (IItemLock<ThumbnailImage> itemLock = Program.ImagePool.Thumbs.GetImage(ThumbnailKey, memoryOnly: true))
			{
				Size imageSize = itemLock?.Item.GetThumbnailSize(defaultSize.Height) ?? Size.Empty;
				Size defaultMaximumSize = GetDefaultMaximumSize(defaultSize);
				if (imageSize.IsEmpty)
				{
					Size estimatedSize = GetEstimatedSize(defaultMaximumSize);
					if (!estimatedSize.IsEmpty)
					{
						return estimatedSize;
					}
				}
				return ThumbRenderer.GetSafeScaledImageSize(imageSize, defaultMaximumSize);
			}
		}

		protected Size AddBorder(Size size)
		{
			return size + border + border;
		}

		public override void OnDraw(ItemDrawInformation drawInfo)
		{
			base.OnDraw(drawInfo);
			if (drawInfo.SubItem < 0)
			{
				clickRegions = null;
				return;
			}
			Rectangle bounds = drawInfo.Bounds;
			if (drawInfo.DrawBorder)
			{
				using (Pen pen = new Pen(ThemeColors.ThumbnailViewItem.Border, 1f))
				{
					drawInfo.Graphics.DrawLine(pen, bounds.Location, new Point(bounds.Left, bounds.Bottom));
				}
			}
		}

		protected void MakePageThumbnail(ThumbnailKey key)
		{
			MakePageThumbnail(key, updateSize: true);
		}

		protected void MakePageThumbnail(ThumbnailKey key, bool updateSize)
		{
			if (!disposed)
			{
				updateSize &= base.View.ItemViewMode == ItemViewMode.Thumbnail;
				if (!Program.ImagePool.Thumbs.IsAvailable(key))
				{
					CreateThumbnail(key);
				}
				else
				{
					updateSize = false;
				}
				if (base.View != null)
				{
					Update(updateSize);
				}
			}
		}

		protected abstract void CreateThumbnail(ThumbnailKey key);

		protected virtual void OnRefreshImage()
		{
			Program.ImagePool.Pages.RefreshImage(new PageKey(ThumbnailKey));
			Program.ImagePool.Thumbs.RefreshImage(ThumbnailKey);
		}

		protected void AddClickRegion(Rectangle bounds, Action<Rectangle, Point> click)
		{
			clickRegions = clickRegions.SafeAdd(new ClickRegion
			{
				Bounds = bounds,
				Click = click
			});
		}

		public override bool OnClick(Point pt)
		{
			ClickRegion clickRegion = clickRegions?.FirstOrDefault((ClickRegion c) => c.Bounds.Contains(pt));
			if (clickRegion == null)
			{
				return false;
			}
			clickRegion.Click(clickRegion.Bounds, pt.Subtract(clickRegion.Bounds.Location));
			return true;
		}

		public void Dispose()
		{
			if (!disposed)
			{
				try
				{
					Dispose(disposing: true);
					GC.SuppressFinalize(this);
				}
				finally
				{
					disposed = true;
				}
			}
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
