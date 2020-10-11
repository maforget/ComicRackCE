using System.Drawing;
using System.Drawing.Drawing2D;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	public class RatingRenderer
	{
		private Rectangle bounds;

		public bool Fast
		{
			get;
			set;
		}

		public Image RatingImage
		{
			get;
			set;
		}

		public Color RatingTextColor
		{
			get;
			set;
		}

		public int MaximumRating
		{
			get;
			set;
		}

		public Rectangle Bounds
		{
			get
			{
				return bounds;
			}
			set
			{
				bounds = value;
			}
		}

		public bool VerticalAlignment
		{
			get;
			set;
		}

		public RectangleScaleMode RatingScaleMode
		{
			get;
			set;
		}

		public int X
		{
			get
			{
				return bounds.X;
			}
			set
			{
				bounds.X = value;
			}
		}

		public int Y
		{
			get
			{
				return bounds.Y;
			}
			set
			{
				bounds.Y = value;
			}
		}

		public int Height
		{
			get
			{
				return bounds.Height;
			}
			set
			{
				bounds.Height = value;
			}
		}

		public int Width
		{
			get
			{
				return bounds.Width;
			}
			set
			{
				bounds.Width = value;
			}
		}

		public RatingRenderer(Image image, Rectangle bounds, int count = 5, bool vertical = false)
		{
			RatingImage = image;
			MaximumRating = count;
			VerticalAlignment = vertical;
			Bounds = bounds;
			RatingTextColor = Color.DimGray;
			RatingScaleMode = RectangleScaleMode.Center;
		}

		public RectangleF DrawRatingStrip(Graphics gr, float rating, float alpha1 = 1f, float alpha2 = 0.25f)
		{
			RectangleF stripDisplayBounds = GetStripDisplayBounds();
			RectangleF rect = stripDisplayBounds;
			if (VerticalAlignment)
			{
				stripDisplayBounds.Height *= rating / (float)MaximumRating;
				rect.Height -= stripDisplayBounds.Height;
				rect.Y += stripDisplayBounds.Height;
			}
			else
			{
				stripDisplayBounds.Width *= rating / (float)MaximumRating;
				rect.Width -= stripDisplayBounds.Width;
				rect.X += stripDisplayBounds.Width;
			}
			using (gr.Fast(Fast))
			{
				if (!stripDisplayBounds.IsEmpty && alpha1 > 0.05f)
				{
					using (gr.SaveState())
					{
						gr.SetClip(stripDisplayBounds, CombineMode.Intersect);
						DrawRatingStrip(gr, alpha1);
					}
				}
				if (!rect.IsEmpty)
				{
					if (alpha2 > 0.05f)
					{
						using (gr.SaveState())
						{
							gr.SetClip(rect, CombineMode.Intersect);
							DrawRatingStrip(gr, alpha2);
							return stripDisplayBounds;
						}
					}
					return stripDisplayBounds;
				}
				return stripDisplayBounds;
			}
		}

		public float GetRatingFromStrip(Point pt)
		{
			RectangleF stripDisplayBounds = GetStripDisplayBounds();
			if (VerticalAlignment)
			{
				if ((float)pt.Y < stripDisplayBounds.Y)
				{
					return 0f;
				}
				if ((float)pt.Y > stripDisplayBounds.Bottom)
				{
					return MaximumRating;
				}
				return ((float)pt.Y - stripDisplayBounds.Y) / stripDisplayBounds.Height * (float)MaximumRating;
			}
			if ((float)pt.X < stripDisplayBounds.X)
			{
				return 0f;
			}
			if ((float)pt.X > stripDisplayBounds.Right)
			{
				return MaximumRating;
			}
			return ((float)pt.X - stripDisplayBounds.X) / stripDisplayBounds.Width * (float)MaximumRating;
		}

		public RectangleF DrawRatingTag(Graphics gr, float rating, int ratingDigits = 1)
		{
			if (RatingImage == null || MaximumRating <= 0)
			{
				return RectangleF.Empty;
			}
			Size size = RatingImage.Size;
			Rectangle rectangle = size.ToRectangle(Bounds);
			using (gr.HighQuality(enabled: true))
			{
				gr.DrawImage(RatingImage, rectangle);
			}
			Color ratingTextColor = RatingTextColor;
			Font font = FC.Get("Arial", 12f, FontStyle.Bold | FontStyle.Italic);
			string text = rating.ToString("N" + ratingDigits);
			SizeF sizeF = gr.MeasureString(text, font);
			Rectangle rect = rectangle.Pad(2);
			float num = (float)rect.Width / sizeF.Width * 0.9f;
			using (gr.SaveState())
			{
				using (SolidBrush brush = new SolidBrush(ratingTextColor))
				{
					gr.ScaleTransform(num, num);
					rect = rect.Scale(1f / num);
					using (StringFormat format = new StringFormat
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Far
					})
					{
						gr.DrawString(text, font, brush, rect, format);
					}
				}
			}
			return rectangle;
		}

		public SizeF GetRenderSize()
		{
			return GetStripDisplayBounds().Size;
		}

		private RectangleF GetStripDisplayBounds()
		{
			if (RatingImage == null || MaximumRating <= 0)
			{
				return Rectangle.Empty;
			}
			SizeF size = ((!VerticalAlignment) ? ((SizeF)new Size(MaximumRating * RatingImage.Width, RatingImage.Height)) : ((SizeF)new Size(RatingImage.Width, MaximumRating * RatingImage.Height)));
			return size.ToRectangle(Bounds, RatingScaleMode);
		}

		private void DrawRatingStrip(Graphics gr, float alpha)
		{
			RectangleF stripDisplayBounds = GetStripDisplayBounds();
			float num;
			PointF pointF;
			if (VerticalAlignment)
			{
				num = stripDisplayBounds.Width / (float)RatingImage.Width;
				pointF = new PointF(0f, (float)RatingImage.Height * num);
			}
			else
			{
				num = stripDisplayBounds.Height / (float)RatingImage.Height;
				pointF = new PointF((float)RatingImage.Width * num, 0f);
			}
			RectangleF value = new RectangleF(stripDisplayBounds.Location, RatingImage.Size.Scale(num));
			for (int i = 0; i < MaximumRating; i++)
			{
				gr.DrawImage(RatingImage, Rectangle.Round(value), alpha);
				value.X += pointF.X;
				value.Y += pointF.Y;
			}
		}
	}
}
