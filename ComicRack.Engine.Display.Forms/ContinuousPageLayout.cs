using System;
using System.Collections.Generic;
using System.Drawing;

namespace cYo.Projects.ComicRack.Engine.Display.Forms
{
	/// <summary>
	/// Deterministic geometry for an ordered sequence of vertically stacked comic
	/// pages. Virtual coordinates are kept as <see cref="long"/> values;
	/// GDI-facing rectangles and sizes are clipped to the limits of <see cref="int"/>.
	/// </summary>
	internal sealed class ContinuousPageLayout
	{
		/// <summary>Source metadata for one page in the ordered input sequence.</summary>
		public readonly struct SourcePage
		{
			public SourcePage(int page, Size sourceSize)
			{
				Page = page;
				SourceSize = sourceSize;
			}

			public int Page { get; }

			public Size SourceSize { get; }
		}

		/// <summary>
		/// A page's output geometry. Pages have no vertical spacing. Depending on the
		/// selected fit mode, source images either use the shared content width or
		/// preserve their native dimensions and are centered within that width.
		/// </summary>
		public sealed class PageEntry
		{
			private readonly long top;
			private readonly long height;

			internal PageEntry(int page, Size sourceSize, int left, long top, long height, int renderedWidth)
			{
				Page = page;
				SourceSize = sourceSize;
				this.top = top;
				this.height = height;
				Bounds = ToRectangle(left, top, renderedWidth, height);
			}

			public int Page { get; }

			public Size SourceSize { get; }

			public Rectangle Bounds { get; }

			internal long Top => top;

			internal long Height => height;

			internal long Bottom => SaturatingAdd(top, height);
		}

		/// <summary>
		/// A stable page-plus-relative-position anchor that can be carried across a
		/// layout rebuild, even when the page's rendered height changes.
		/// </summary>
		public readonly struct Anchor
		{
			public Anchor(int page, double relativeOffset)
			{
				Page = page;
				RelativeOffset = double.IsNaN(relativeOffset) ? 0d : Math.Max(0d, Math.Min(relativeOffset, 1d));
			}

			public int Page { get; }

			public double RelativeOffset { get; }
		}

		private readonly List<PageEntry> pages;
		private readonly Dictionary<int, PageEntry> pagesByNumber;
		private readonly int contentWidth;
		private readonly long totalHeight;

		public ContinuousPageLayout(IEnumerable<SourcePage> sourcePages, int contentWidth, bool preserveSourceSize = false)
		{
			if (sourcePages == null)
			{
				throw new ArgumentNullException(nameof(sourcePages));
			}
			if (contentWidth <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(contentWidth), contentWidth, "Content width must be positive.");
			}

			this.contentWidth = contentWidth;
			List<PageEntry> entries = new List<PageEntry>();
			pagesByNumber = new Dictionary<int, PageEntry>();
			long top = 0L;
			foreach (SourcePage sourcePage in sourcePages)
			{
				bool validSize = sourcePage.SourceSize.Width > 0 && sourcePage.SourceSize.Height > 0;
				int renderedWidth = validSize ? (preserveSourceSize ? sourcePage.SourceSize.Width : contentWidth) : 0;
				long height = preserveSourceSize ? (validSize ? sourcePage.SourceSize.Height : 0L) : GetScaledHeight(contentWidth, sourcePage.SourceSize);
				int left = preserveSourceSize ? Math.Max(0, (contentWidth - renderedWidth) / 2) : 0;
				PageEntry entry = new PageEntry(sourcePage.Page, sourcePage.SourceSize, left, top, height, renderedWidth);
				entries.Add(entry);
				if (!pagesByNumber.ContainsKey(sourcePage.Page))
				{
					pagesByNumber.Add(sourcePage.Page, entry);
				}
				top = SaturatingAdd(top, height);
			}

			pages = entries;
			totalHeight = top;
		}

		/// <summary>Full virtual height.  It may exceed <see cref="int.MaxValue"/>.</summary>
		public long TotalHeight => totalHeight;

		public Size TotalSize => new Size(contentWidth, ClampToInt(totalHeight));

		/// <summary>
		/// Ordered pages that intersect the viewport.  Bounds use half-open
		/// intervals, so a page ending exactly on an edge is not returned.
		/// </summary>
		public IEnumerable<PageEntry> GetVisible(Rectangle viewport)
		{
			if (pages.Count == 0 || viewport.Width <= 0 || viewport.Height <= 0)
			{
				yield break;
			}

			long viewportLeft = viewport.X;
			long viewportRight = SaturatingAdd(viewportLeft, viewport.Width);
			if (viewportRight <= 0L || viewportLeft >= contentWidth)
			{
				yield break;
			}

			long viewportTop = viewport.Y;
			long viewportBottom = SaturatingAdd(viewportTop, viewport.Height);
			int low = 0;
			int high = pages.Count;
			while (low < high)
			{
				int middle = low + ((high - low) / 2);
				if (pages[middle].Bottom <= viewportTop)
				{
					low = middle + 1;
				}
				else
				{
					high = middle;
				}
			}

			for (int i = low; i < pages.Count; i++)
			{
				PageEntry page = pages[i];
				if (page.Top >= viewportBottom)
				{
					yield break;
				}
				if (page.Bounds.Width <= 0 || page.Height <= 0L)
				{
					continue;
				}
				if (page.Bounds.Left < viewportRight && page.Bounds.Right > viewportLeft && page.Top < viewportBottom && page.Bottom > viewportTop)
				{
					yield return page;
				}
			}
		}

		/// <summary>
		/// Finds the page containing y.  For a nonempty layout y is clamped to the
		/// strip, which makes this suitable for capturing anchors at either edge.
		/// </summary>
		public PageEntry HitTest(int y)
		{
			if (pages.Count == 0)
			{
				return null;
			}
			if (totalHeight <= 0L)
			{
				return pages[0];
			}

			long coordinate = y;
			if (coordinate < 0L)
			{
				coordinate = 0L;
			}
			else if (coordinate >= totalHeight)
			{
				coordinate = totalHeight - 1L;
			}

			int low = 0;
			int high = pages.Count - 1;
			while (low <= high)
			{
				int middle = low + ((high - low) / 2);
				PageEntry page = pages[middle];
				if (coordinate < page.Top)
				{
					high = middle - 1;
				}
				else if (coordinate >= page.Bottom || page.Height <= 0L)
				{
					low = middle + 1;
				}
				else
				{
					return page;
				}
			}

			// Zero-sized metadata pages have no hit area.  Return the nearest page
			// in input order so anchor capture remains deterministic.
			if (low >= pages.Count)
			{
				return pages[pages.Count - 1];
			}
			if (high < 0)
			{
				return pages[0];
			}
			return pages[low].Top > coordinate ? pages[Math.Max(0, high)] : pages[Math.Min(pages.Count - 1, low)];
		}

		public Anchor CaptureAnchor(int y)
		{
			PageEntry page = HitTest(y);
			if (page == null)
			{
				return default;
			}

			long coordinate = y;
			if (coordinate < 0L)
			{
				coordinate = 0L;
			}
			else if (totalHeight > 0L && coordinate >= totalHeight)
			{
				coordinate = totalHeight - 1L;
			}

			long offset = Clamp(coordinate - page.Top, 0L, Math.Max(0L, page.Height - 1L));
			double relativeOffset = page.Height > 0L ? (double)offset / page.Height : 0d;
			return new Anchor(page.Page, relativeOffset);
		}

		/// <summary>
		/// Resolves an anchor to a clamped virtual y coordinate.  If the anchored
		/// page is unavailable, the nearest available page by page number is used;
		/// relative positions are clamped within that page's rendered height.
		/// </summary>
		public int ResolveAnchor(Anchor anchor)
		{
			if (pages.Count == 0)
			{
				return 0;
			}

			PageEntry page = FindPage(anchor.Page);
			long offset = (long)(anchor.RelativeOffset * page.Height);
			offset = Clamp(offset, 0L, Math.Max(0L, page.Height - 1L));
			long coordinate = SaturatingAdd(page.Top, offset);
			if (totalHeight > 0L)
			{
				coordinate = Clamp(coordinate, 0L, totalHeight - 1L);
			}
			return ClampToInt(coordinate);
		}

		private PageEntry FindPage(int pageNumber)
		{
			if (pagesByNumber.TryGetValue(pageNumber, out PageEntry exact))
			{
				return exact;
			}

			PageEntry nearest = pages[0];
			long distance = Distance(nearest.Page, pageNumber);
			for (int i = 1; i < pages.Count; i++)
			{
				PageEntry candidate = pages[i];
				long candidateDistance = Distance(candidate.Page, pageNumber);
				if (candidateDistance < distance)
				{
					nearest = candidate;
					distance = candidateDistance;
				}
			}
			return nearest;
		}

		private static long Distance(int left, int right)
		{
			return Math.Abs((long)left - right);
		}

		private static long GetScaledHeight(int width, Size sourceSize)
		{
			if (sourceSize.Width <= 0 || sourceSize.Height <= 0)
			{
				return 0L;
			}

			long numerator = SaturatingMultiply(width, sourceSize.Height);
			long rounded = SaturatingAdd(numerator, sourceSize.Width / 2L);
			long height = rounded / sourceSize.Width;
			return height > 0L ? height : 1L;
		}

		private static long SaturatingMultiply(long left, long right)
		{
			if (left <= 0L || right <= 0L)
			{
				return 0L;
			}
			if (left > long.MaxValue / right)
			{
				return long.MaxValue;
			}
			return left * right;
		}

		private static long SaturatingAdd(long left, long right)
		{
			if (right > 0L && left > long.MaxValue - right)
			{
				return long.MaxValue;
			}
			if (right < 0L && left < long.MinValue - right)
			{
				return long.MinValue;
			}
			return left + right;
		}

		private static long Clamp(long value, long minimum, long maximum)
		{
			if (value < minimum)
			{
				return minimum;
			}
			if (value > maximum)
			{
				return maximum;
			}
			return value;
		}

		private static int ClampToInt(long value)
		{
			if (value <= 0L)
			{
				return 0;
			}
			return value >= int.MaxValue ? int.MaxValue : (int)value;
		}

		private static Rectangle ToRectangle(int left, long top, int width, long height)
		{
			int y = ClampToInt(top);
			long availableHeight = int.MaxValue - (long)y;
			long clippedHeight = Math.Min(height, availableHeight);
			return new Rectangle(left, y, width, ClampToInt(clippedHeight));
		}
	}
}
