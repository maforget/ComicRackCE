using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	public class ThumbTileRenderer : ViewItemRenderer
	{
		private static readonly Font DefaultFont = SystemFonts.IconTitleFont;

		private Font font = DefaultFont;

		public Font Font
		{
			get
			{
				return font;
			}
			set
			{
				font = value;
			}
		}

		public bool ThreeD
		{
			get;
			set;
		}

		public IEnumerable<Image> Icons
		{
			get;
			set;
		}

		public int ScrollOffset
		{
			get;
			set;
		}

		public ThumbTileRenderer(Image image, ThumbnailDrawingOptions flags)
		{
			base.Image = image;
			base.Options = flags;
		}

		public void DrawBackground(Graphics graphics, Rectangle tileBounds)
		{
			if (base.SelectionAlphaState == StyledRenderer.AlphaStyle.None)
			{
				graphics.DrawStyledRectangle(tileBounds, 128, base.BackColor, StyledRenderer.Default.NoGradient());
			}
			else
			{
				graphics.DrawStyledRectangle(tileBounds, base.SelectionAlphaState, base.SelectionBackColor);
			}
		}

		public void DrawTile(Graphics graphics, Rectangle tileBounds)
		{
			if (!graphics.IsVisible(tileBounds))
			{
				return;
			}
			tileBounds.Inflate(-base.Border.Width, -base.Border.Height);
			if (base.BackgroundEnabled)
			{
				DrawBackground(graphics, tileBounds);
				tileBounds.Inflate(-4, -4);
			}
			ThumbnailDrawingOptions options = base.Options;
			if (base.RatingMode == ThumbnailRatingMode.Tags)
			{
				base.RatingEnabled = false;
			}
			Rectangle rectangle;
			if (ThreeD)
			{
				rectangle = tileBounds.Pad(tileBounds.Width / 3, 0);
			}
			else
			{
				Size safeScaledImageSize = ThumbRenderer.GetSafeScaledImageSize(base.Image, new Size(tileBounds.Width / 2, tileBounds.Height));
				rectangle = new Rectangle(tileBounds.X + safeScaledImageSize.Width + 4, tileBounds.Y, tileBounds.Width - safeScaledImageSize.Width - 4, tileBounds.Height);
				DrawThumbnail(graphics, new Rectangle(tileBounds.Location, safeScaledImageSize));
			}
			base.Options = options;
			Rectangle rectangle2 = rectangle;
			int specialTagsHeight = GetSpecialTagsHeight(tileBounds);
			bool flag = Icons != null && Icons.Count() > 0;
			if (base.HasTagRatingOverlay || base.HasStateOverlay || flag)
			{
				rectangle2.Height -= (int)((float)specialTagsHeight * 1.2f);
			}
			if (ThreeD)
			{
				DrawEmbossedText(graphics, base.TextLines, rectangle2);
			}
			else
			{
				SimpleTextRenderer.DrawText(graphics, base.TextLines, rectangle2);
			}
			if (base.RatingMode == ThumbnailRatingMode.Tags)
			{
				rectangle.Width -= DrawRating(graphics, rectangle, specialTagsHeight);
			}
			if (flag)
			{
				using (base.FastModeEnabled ? graphics.Fast() : graphics.HighQuality(enabled: true))
				{
					int top = rectangle.Height - specialTagsHeight;
					ThumbRenderer.DrawImageList(graphics, Icons, rectangle.Pad(0, top), ThreeD ? ContentAlignment.BottomRight : ContentAlignment.BottomLeft, -0.1f);
				}
			}
			if (ThreeD)
			{
				Draw3DComic(graphics, tileBounds);
			}
		}

		private void Draw3DComic(Graphics graphics, Rectangle tileBounds)
		{
			if (base.Image == null || !(base.Image is Bitmap))
			{
				return;
			}
			Bitmap cover = base.Image as Bitmap;
			Size size = new Size(512, 512);
			try
			{
				using (Bitmap bitmap = ComicBox3D.CreateDefaultBook(cover, null, size, base.PageCount))
				{
					Rectangle rect = bitmap.Size.ToRectangle().Scale(bitmap.Size.GetScale(tileBounds.Size));
					graphics.DrawImage(bitmap, rect);
				}
			}
			catch (Exception)
			{
			}
		}

		private void DrawEmbossedText(Graphics graphics, IEnumerable<TextLine> lines, Rectangle bounds)
		{
			base.TextLines.ForEach(delegate(TextLine tl)
			{
				tl.Format.Alignment = StringAlignment.Far;
			});
			bounds.Offset(1, 1);
			base.TextLines.ForEach(delegate(TextLine tl)
			{
				tl.ForeColor = ThemeColors.ThumbTileRenderer.Emboss;
			});
			SimpleTextRenderer.DrawText(graphics, base.TextLines, bounds);
			bounds.Offset(-1, -1);
			base.TextLines.ForEach(delegate(TextLine tl)
			{
				tl.ForeColor = ThemeColors.ThumbTileRenderer.TitleText;
            });
			base.TextLines.Where((TextLine tl) => tl.Font != null && !tl.Font.Bold).ForEach(delegate(TextLine tl)
			{
				tl.ForeColor = ThemeColors.ThumbTileRenderer.BodyText;
            });
			SimpleTextRenderer.DrawText(graphics, base.TextLines, bounds);
		}

		private int GetSpecialTagsHeight(Rectangle tileBounds)
		{
			return ThumbRenderer.GetTagHeight(tileBounds);
		}

		public static void DrawTile(Graphics graphics, Rectangle bounds, Image image, ComicBook comicBook, int page, Font font, Color foreColor, Color backColor, ThumbnailDrawingOptions options, ComicTextElements elements, bool threeD, IEnumerable<Image> icons = null)
		{
			ThumbTileRenderer thumbTileRenderer = new ThumbTileRenderer(image, options)
			{
				ForeColor = foreColor,
				BackColor = backColor,
				ThreeD = threeD
			};
			if (comicBook != null)
			{
				thumbTileRenderer.PageCount = comicBook.PageCount;
				thumbTileRenderer.Rating1 = comicBook.Rating;
				thumbTileRenderer.Rating2 = comicBook.CommunityRating;
				thumbTileRenderer.Bookmarks = new int[2]
				{
					comicBook.CurrentPage,
					comicBook.LastPageRead
				};
				thumbTileRenderer.Icons = icons;
				if (page >= 0)
				{
					thumbTileRenderer.PageNumber = page;
					thumbTileRenderer.TextLines.AddRange(ComicTextBuilder.GetTextBlocks(comicBook.GetPage(page), page, font, foreColor, elements));
				}
				else
				{
					thumbTileRenderer.TextLines.AddRange(ComicTextBuilder.GetTextBlocks(comicBook, font, foreColor, elements));
				}
				thumbTileRenderer.DrawTile(graphics, bounds);
				thumbTileRenderer.DisposeTextLines();
			}
		}

		public static void DrawTile(Graphics graphics, Rectangle bounds, Image image, ComicBook comicBook, Font font, Color foreColor, Color backColor, ThumbnailDrawingOptions options, ComicTextElements elements, bool threeD, IEnumerable<Image> icons = null)
		{
			DrawTile(graphics, bounds, image, comicBook, -1, font, foreColor, backColor, options, elements, threeD, icons);
		}
	}
}
