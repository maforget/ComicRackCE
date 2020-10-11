using System.Drawing;

namespace cYo.Common.Presentation.Ceco
{
	public abstract class Block : Span, IRender
	{
		private int lastLayoutWidth;

		private int border = -1;

		private VerticalAlignment vAlign;

		private SizeValue blockWidth;

		private int blockHeight;

		private Size margin;

		public bool IsWhiteSpace => false;

		public int Border
		{
			get
			{
				if (border == -1)
				{
					Block block = base.ParentInline as Block;
					if (block != null)
					{
						return block.Border;
					}
				}
				return border;
			}
			set
			{
				if (border != value)
				{
					border = value;
					OnBorderChanged();
				}
			}
		}

		public virtual VerticalAlignment VAlign
		{
			get
			{
				if (vAlign == VerticalAlignment.None)
				{
					Block block = base.ParentInline as Block;
					if (block != null)
					{
						return block.VAlign;
					}
				}
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

		public SizeValue BlockWidth
		{
			get
			{
				return blockWidth;
			}
			set
			{
				if (!(blockWidth == value))
				{
					blockWidth = value;
					OnBlockWidthChanged();
				}
			}
		}

		public int BlockHeight
		{
			get
			{
				return blockHeight;
			}
			set
			{
				if (blockHeight != value)
				{
					blockHeight = value;
					OnBlockHeightChanged();
				}
			}
		}

		public virtual Size Margin
		{
			get
			{
				return margin;
			}
			set
			{
				if (!(margin == value))
				{
					margin = value;
					OnMarginChanged();
				}
			}
		}

		public override bool IsBlock => true;

		public int MinimumWidth
		{
			get;
			set;
		}

		public override bool IsNode => false;

		public virtual void Measure(Graphics gr, int maxWidth)
		{
			LayoutType layoutType = base.PendingLayout;
			maxWidth -= margin.Width * 2;
			if (layoutType == LayoutType.None && maxWidth != lastLayoutWidth)
			{
				layoutType = LayoutType.Position;
			}
			if (layoutType != 0)
			{
				CoreMeasure(gr, maxWidth, layoutType);
				lastLayoutWidth = maxWidth;
			}
			base.X += margin.Width;
			base.Y += margin.Height;
			base.PendingLayout = LayoutType.None;
		}

		public virtual void Draw(Graphics gr, Point location)
		{
			if (base.ParentInline == null)
			{
				Measure(gr, base.Size.Width);
			}
		}

		public void SetAlign(ContentAlignment contentAlignment)
		{
			switch (contentAlignment)
			{
			default:
				Align = HorizontalAlignment.Left;
				break;
			case ContentAlignment.TopCenter:
			case ContentAlignment.MiddleCenter:
			case ContentAlignment.BottomCenter:
				Align = HorizontalAlignment.Center;
				break;
			case ContentAlignment.TopRight:
			case ContentAlignment.MiddleRight:
			case ContentAlignment.BottomRight:
				Align = HorizontalAlignment.Right;
				break;
			}
			switch (contentAlignment)
			{
			case ContentAlignment.BottomLeft:
			case ContentAlignment.BottomCenter:
			case ContentAlignment.BottomRight:
				VAlign = VerticalAlignment.Bottom;
				break;
			case ContentAlignment.MiddleLeft:
			case ContentAlignment.MiddleCenter:
			case ContentAlignment.MiddleRight:
				VAlign = VerticalAlignment.Middle;
				break;
			default:
				VAlign = VerticalAlignment.Top;
				break;
			}
		}

		protected abstract void CoreMeasure(Graphics gr, int maxWidth, LayoutType tbl);

		protected virtual void OnVAlignChanged()
		{
			InvokeLayout(LayoutType.Position);
		}

		protected virtual void OnBorderChanged()
		{
			InvokeLayout(LayoutType.Full);
		}

		protected virtual void OnBlockWidthChanged()
		{
			InvokeLayout(LayoutType.Full);
		}

		protected virtual void OnBlockHeightChanged()
		{
			InvokeLayout(LayoutType.Full);
		}

		protected virtual void OnMarginChanged()
		{
			InvokeLayout(LayoutType.Full);
		}
	}
}
