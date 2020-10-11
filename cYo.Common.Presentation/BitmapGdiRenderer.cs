using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using cYo.Common.Drawing;

namespace cYo.Common.Presentation
{
	public class BitmapGdiRenderer : IBitmapRenderer
	{
		private readonly bool fixedGraphics;

		private Stack<Graphics> graphicsStack;

		private float opacity = 1f;

		public Graphics Graphics
		{
			get;
			set;
		}

		public InterpolationMode LowQualityInterpolation
		{
			get;
			set;
		}

		public bool HighQuality
		{
			get;
			set;
		}

		public Matrix Transform
		{
			get
			{
				return Graphics.Transform;
			}
			set
			{
				Graphics.Transform = value;
			}
		}

		public bool IsHardware => false;

		public bool IsLocked => false;

		public float Opacity
		{
			get
			{
				return opacity;
			}
			set
			{
				opacity = value;
			}
		}

		public CompositingMode CompositingMode
		{
			get
			{
				return Graphics.CompositingMode;
			}
			set
			{
				Graphics.CompositingMode = value;
			}
		}

		public RectangleF Clip
		{
			get
			{
				return Graphics.ClipBounds;
			}
			set
			{
				Graphics.SetClip(value);
			}
		}

		public BitmapGdiRenderer(Graphics graphics, bool highQuality = false)
		{
			LowQualityInterpolation = InterpolationMode.Default;
			Graphics = graphics;
			HighQuality = highQuality;
			fixedGraphics = graphics != null;
		}

		public BitmapGdiRenderer()
			: this(null)
		{
		}

		public void Clear(Color color)
		{
			Graphics.Clear(color);
		}

		public void DrawImage(RendererImage image, RectangleF dest, RectangleF src, BitmapAdjustment transform, float opacity)
		{
			Rectangle rectangle = Rectangle.Truncate(src);
			Rectangle destination = Rectangle.Truncate(dest);
			opacity *= this.opacity;
			using (Graphics.HighQuality(HighQuality, dest.Size, src.Size))
			{
				if (!HighQuality)
				{
					Graphics.InterpolationMode = LowQualityInterpolation;
				}
				Graphics.DrawImage(image, destination, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, transform, opacity);
			}
		}

		public void FillRectangle(RectangleF bounds, Color color)
		{
			using (Brush brush = new SolidBrush(Color.FromArgb((int)(255f * opacity), color)))
			{
				Graphics.FillRectangle(brush, bounds);
			}
		}

		public void DrawLine(IEnumerable<PointF> points, Color color, float width)
		{
			using (Pen pen = new Pen(color, width))
			{
				bool flag = true;
				PointF pointF = PointF.Empty;
				foreach (PointF point in points)
				{
					if (flag)
					{
						pointF = point;
						flag = false;
					}
					else
					{
						PointF pt = pointF;
						pointF = point;
						Graphics.DrawLine(pen, pt, pointF);
					}
				}
			}
		}

		public bool IsVisible(RectangleF bounds)
		{
			return Graphics.IsVisible(bounds);
		}

		public IDisposable SaveState()
		{
			return Graphics.SaveState();
		}

		public void TranslateTransform(float dx, float dy)
		{
			Graphics.TranslateTransform(dx, dy);
		}

		public void ScaleTransform(float dx, float dy)
		{
			Graphics.ScaleTransform(dx, dy);
		}

		public void RotateTransform(float angel)
		{
			Graphics.RotateTransform(angel);
		}

		public bool BeginScene(Graphics gr)
		{
			if (fixedGraphics)
			{
				return false;
			}
			if (graphicsStack == null)
			{
				graphicsStack = new Stack<Graphics>();
			}
			graphicsStack.Push(Graphics);
			Graphics = gr;
			return true;
		}

		public void EndScene()
		{
			if (!fixedGraphics)
			{
				Graphics = ((graphicsStack != null) ? graphicsStack.Pop() : null);
			}
		}

		public void DrawBlurredImage(RendererImage image, RectangleF rd, RectangleF rs, float blur)
		{
			try
			{
				RectangleF src = new RectangleF(rs.X, rs.Y, rs.Width * blur, rs.Height * blur);
				RectangleF destRect = new RectangleF(PointF.Empty, src.Size);
				if (destRect.Width < 8f || destRect.Height < 8f)
				{
					return;
				}
				using (Graphics.SaveState())
				{
					using (Bitmap image2 = new Bitmap((int)destRect.Width, (int)destRect.Height))
					{
						using (Graphics graphics = Graphics.FromImage(image2))
						{
							graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
							graphics.DrawImage((Bitmap)image, destRect, rs, GraphicsUnit.Pixel);
						}
						DrawImage(image2, rd, src, BitmapAdjustment.Empty, opacity);
					}
				}
			}
			catch (Exception)
			{
				throw new ArgumentException(string.Concat("Failed to draw blurred image: ", rs, "/", blur));
			}
		}

		public static implicit operator BitmapGdiRenderer(Graphics gr)
		{
			return new BitmapGdiRenderer(gr);
		}
	}
}
