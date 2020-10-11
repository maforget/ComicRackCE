using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing;

namespace cYo.Common.Windows.Forms
{
	public class BitmapViewer : ScrollControl, IBitmapDisplayControl, IDisposable
	{
		private volatile Bitmap image;

		private volatile bool pendingImageChange;

		private ScaleMode scaleMode = ScaleMode.FitAll;

		private BitmapAdjustment colorAdjustment = BitmapAdjustment.Empty;

		private ContentAlignment textAlignment = ContentAlignment.TopCenter;

		[DefaultValue(null)]
		public Bitmap Bitmap
		{
			get
			{
				return image;
			}
			set
			{
				Image image = this.image;
				if (image != value)
				{
					this.image = value;
					OnImageChanged();
					pendingImageChange = true;
					Invalidate();
				}
			}
		}

		[DefaultValue(ScaleMode.FitAll)]
		public ScaleMode ScaleMode
		{
			get
			{
				return scaleMode;
			}
			set
			{
				if (scaleMode != value)
				{
					scaleMode = value;
					OnScaleModeChanged();
					pendingImageChange = true;
					Invalidate();
				}
			}
		}

		[DefaultValue(typeof(BitmapAdjustment), "0, 0, 0")]
		public BitmapAdjustment ColorAdjustment
		{
			get
			{
				return colorAdjustment;
			}
			set
			{
				if (!(colorAdjustment == value))
				{
					colorAdjustment = value;
					Invalidate();
				}
			}
		}

		[DefaultValue(ContentAlignment.TopCenter)]
		public ContentAlignment TextAlignment
		{
			get
			{
				return textAlignment;
			}
			set
			{
				if (textAlignment != value)
				{
					textAlignment = value;
					Invalidate();
				}
			}
		}

		public event EventHandler ImageChanged;

		public event EventHandler ScaleModeChanged;

		public BitmapViewer()
		{
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
		}

		public void SetBitmap(Bitmap image)
		{
			Bitmap bitmap = Bitmap;
			Bitmap = image;
			bitmap?.Dispose();
		}

		public Color GetPixel(Point location)
		{
			if (image == null)
			{
				throw new InvalidOperationException("No valid image for this method");
			}
			try
			{
				Point point = Translate(location, fromClient: true);
				Rectangle imageDisplayBounds = GetImageDisplayBounds();
				int x = point.X * image.Width / imageDisplayBounds.Width;
				int y = point.Y * image.Height / imageDisplayBounds.Height;
				return image.GetPixel(x, y);
			}
			catch
			{
				throw new ArgumentOutOfRangeException("location", "Location is not a valid position in the image");
			}
		}

		protected virtual void OnImageChanged()
		{
			if (this.ImageChanged != null)
			{
				this.ImageChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnScaleModeChanged()
		{
			if (this.ScaleModeChanged != null)
			{
				this.ScaleModeChanged(this, EventArgs.Empty);
			}
		}

		private Size GetImageSize()
		{
			Image image = this.image;
			if (image == null)
			{
				return Size.Empty;
			}
			float scale = image.Size.GetScale(ViewRectangle.Size, scaleMode);
			return new Size((int)((float)image.Width * scale), (int)((float)image.Height * scale));
		}

		private Point GetImageLeftTop()
		{
			return new Point(-base.ScrollPosition.X + DisplayRectangle.X, -base.ScrollPosition.Y + DisplayRectangle.Y);
		}

		private Rectangle GetImageDisplayBounds()
		{
			if (image == null)
			{
				return Rectangle.Empty;
			}
			return new Rectangle(GetImageLeftTop(), base.VirtualSize);
		}

		private void UpdateVirtualSize()
		{
			for (int i = 0; i < 2; i++)
			{
				base.VirtualSize = GetImageSize();
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			if (pendingImageChange)
			{
				pendingImageChange = false;
				UpdateVirtualSize();
			}
			if (image != null)
			{
				using (e.Graphics.SaveState())
				{
					e.Graphics.IntersectClip(DisplayRectangle);
					e.Graphics.DrawImage(image, GetImageDisplayBounds(), 0, 0, image.Width, image.Height, colorAdjustment);
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-4, -4);
			using (SolidBrush brush = new SolidBrush(ForeColor))
			{
				using (StringFormat format = new StringFormat
				{
					LineAlignment = TextAlignment.ToLineAlignment(),
					Alignment = TextAlignment.ToAlignment()
				})
				{
					e.Graphics.DrawString(Text, Font, brush, displayRectangle, format);
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (base.IsHandleCreated)
			{
				UpdateVirtualSize();
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			Point scrollPosition = base.ScrollPosition;
			scrollPosition.Y -= e.Delta;
			base.ScrollPosition = scrollPosition;
		}

        //Decompile Error
        //object IBitmapDisplayControl.get_Tag()
        //{
        //    return base.Tag;
        //}

        //void IBitmapDisplayControl.set_Tag(object value)
        //{
        //	base.Tag = value;
        //}
    }
}
