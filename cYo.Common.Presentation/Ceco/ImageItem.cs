using System.Drawing;

namespace cYo.Common.Presentation.Ceco
{
	public class ImageItem : Block
	{
		private Image image;

		private string source;

		private Size padding;

		private Size inflate;

		private VerticalAlignment vAlign;

		private HorizontalAlignment align;

		public Image Image
		{
			get
			{
				if (image == null && source != null)
				{
					image = GetImage(source);
				}
				return image;
			}
			set
			{
				if (image != value)
				{
					image = value;
					OnImageChanged();
				}
			}
		}

		public string Source
		{
			get
			{
				return source;
			}
			set
			{
				if (!(source == value))
				{
					source = value;
					image = null;
					OnImageChanged();
				}
			}
		}

		public Size Padding
		{
			get
			{
				return padding;
			}
			set
			{
				if (!(padding == value))
				{
					padding = value;
					inflate = new Size(padding.Width * 2, padding.Height * 2);
					OnPaddingChanged();
				}
			}
		}

		public override VerticalAlignment VAlign
		{
			get
			{
				return vAlign;
			}
			set
			{
				if (vAlign != value)
				{
					vAlign = value;
					OnVAlignChanged();
				}
			}
		}

		public override HorizontalAlignment Align
		{
			get
			{
				return align;
			}
			set
			{
				if (align != value)
				{
					align = value;
					OnAlignChanged();
				}
			}
		}

		public ImageItem()
		{
		}

		public ImageItem(string file)
		{
			image = Image.FromFile(file);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && image != null)
			{
				image.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void CoreMeasure(Graphics gr, int maxWidth, LayoutType tbl)
		{
			int num = ((!base.BlockWidth.IsAuto) ? base.BlockWidth.GetSize(maxWidth) : 0);
			int blockHeight = base.BlockHeight;
			Size size = Size.Empty;
			if (Image != null)
			{
				if (num <= 0 && blockHeight <= 0)
				{
					size = Image.Size;
				}
				else if (num > 0)
				{
					size = new Size(num, Image.Height * num / Image.Width);
				}
				else if (blockHeight > 0)
				{
					size = new Size(Image.Width * blockHeight / Image.Height, blockHeight);
				}
			}
			if (size.Width < num)
			{
				size.Width = num;
			}
			if (blockHeight != 0)
			{
				size.Height = blockHeight;
			}
			size += inflate + inflate;
			base.MinimumWidth = num + inflate.Width * 2;
			base.Size = size;
			switch (VAlign)
			{
			default:
				base.BaseLine = size.Height;
				break;
			case VerticalAlignment.Top:
				base.BaseLine = Font.Height;
				break;
			case VerticalAlignment.Middle:
				base.BaseLine = (Font.Height + size.Height) / 2 - base.DescentHeight;
				break;
			}
		}

		public override void Draw(Graphics gr, Point location)
		{
			Image image = Image;
			if (image != null)
			{
				Rectangle bounds = base.Bounds;
				bounds.Inflate(-padding.Width, -padding.Height);
				bounds.Offset(location);
				gr.DrawImage(image, bounds);
			}
		}

		protected virtual void OnImageChanged()
		{
			InvokeLayout(LayoutType.Full);
		}

		protected virtual void OnPaddingChanged()
		{
			InvokeLayout(LayoutType.Full);
		}
	}
}
