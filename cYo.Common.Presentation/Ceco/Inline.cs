using System;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.ComponentModel;

namespace cYo.Common.Presentation.Ceco
{
	public abstract class Inline : DisposableObject
	{
		private string fontFamily;

		private FontSize fontSize = FontSize.Empty;

		private float fontSizeEM;

		private float fontScale = 1f;

		private FontStyle fontStyle;

		private Color foreColor = Color.Empty;

		private Color backColor = Color.Empty;

		private const float normSize = 12f;

		private static readonly float[] sizeFactors = new float[7]
		{
			7f / normSize,
			19f / 24f,
			1f,
			1.16666663f,
			1.5f,
			2f,
			3f
		};

		private Cursor mouseCursor;

		private Inline parentInline;

		private Rectangle bounds;

		private BaseAlignment baseAlign;

		private HorizontalAlignment align;

		private LayoutType pendingLayout;

		private bool visible = true;

		private bool isHot;

		public virtual string FontFamily
		{
			get
			{
				string text = fontFamily;
				if (string.IsNullOrEmpty(text) && parentInline != null)
				{
					text = parentInline.FontFamily;
				}
				return text;
			}
			set
			{
				if (!(fontFamily == value))
				{
					fontFamily = value;
					OnFontChanged();
				}
			}
		}

		public virtual FontSize FontSize
		{
			get
			{
				return fontSize;
			}
			set
			{
				if (!(fontSize == value))
				{
					fontSize = value;
					OnFontChanged();
				}
			}
		}

		public virtual float FontSizeEM
		{
			get
			{
				float num = fontSizeEM;
				if (num == 0f && parentInline != null)
				{
					num = parentInline.FontSizeEM;
				}
				return num;
			}
			set
			{
				if (fontSizeEM != value)
				{
					fontSizeEM = value;
					OnFontChanged();
				}
			}
		}

		public virtual float FontScale
		{
			get
			{
				float num = fontScale;
				if (num == 0f && parentInline != null)
				{
					num = parentInline.FontScale;
				}
				return num;
			}
			set
			{
				if (fontScale != value)
				{
					fontScale = value;
					OnFontChanged();
				}
			}
		}

		public virtual FontStyle FontStyle
		{
			get
			{
				FontStyle fontStyle = this.fontStyle;
				if (fontStyle == FontStyle.Regular && parentInline != null)
				{
					fontStyle = parentInline.FontStyle;
				}
				return fontStyle;
			}
			set
			{
				if (fontStyle != value)
				{
					fontStyle = value;
					OnFontChanged();
				}
			}
		}

		public virtual Color ForeColor
		{
			get
			{
				Color result = foreColor;
				if (result.IsEmpty && parentInline != null)
				{
					result = parentInline.ForeColor;
				}
				return result;
			}
			set
			{
				if (!(foreColor == value))
				{
					foreColor = value;
					OnForeColorChanged();
				}
			}
		}

		public virtual Color BackColor
		{
			get
			{
				Color result = backColor;
				if (result.IsEmpty && parentInline != null)
				{
					result = parentInline.BackColor;
				}
				return result;
			}
			set
			{
				if (!(backColor == value))
				{
					backColor = value;
					OnBackColorChanged();
				}
			}
		}

		public virtual Font Font
		{
			get
			{
				float num = sizeFactors[Math.Max(Math.Min(GetFontSize(), 7), 1) - 1];
				return GetFont(FontFamily, FontSizeEM * FontScale * num, FontStyle);
			}
			set
			{
				FontFamily = value.FontFamily.Name;
				FontSizeEM = value.SizeInPoints;
				FontStyle = value.Style;
			}
		}

		public virtual Cursor MouseCursor
		{
			get
			{
				Cursor result = mouseCursor;
				if (mouseCursor == null && parentInline != null)
				{
					result = parentInline.MouseCursor;
				}
				return result;
			}
			set
			{
				if (!(mouseCursor == value))
				{
					mouseCursor = value;
					OnMouseCursorChanged();
				}
			}
		}

		public Inline ParentInline
		{
			get
			{
				return parentInline;
			}
			set
			{
				parentInline = value;
			}
		}

		public virtual bool IsNode => false;

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

		public Point Location
		{
			get
			{
				return bounds.Location;
			}
			set
			{
				bounds.Location = value;
			}
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

		public Size Size
		{
			get
			{
				return bounds.Size;
			}
			set
			{
				bounds.Size = value;
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

		public int BaseLine
		{
			get;
			set;
		}

		public BaseAlignment BaseAlign
		{
			get
			{
				if (baseAlign == BaseAlignment.None && parentInline != null)
				{
					return parentInline.BaseAlign;
				}
				return baseAlign;
			}
			set
			{
				baseAlign = value;
			}
		}

		public virtual HorizontalAlignment Align
		{
			get
			{
				if (align == HorizontalAlignment.None && parentInline != null)
				{
					return parentInline.Align;
				}
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

		public virtual int FlowBreakOffset => 0;

		public virtual bool IsBlock => false;

		public virtual FlowBreak FlowBreak => FlowBreak.None;

		public LayoutType PendingLayout
		{
			get
			{
				return pendingLayout;
			}
			set
			{
				if (pendingLayout != value)
				{
					pendingLayout = value;
					OnPendingLayoutChanged();
				}
			}
		}

		public bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				visible = value;
			}
		}

		public int FontCellDescent => Font.FontFamily.GetCellDescent(Font.Style);

		public int FontEmHeight => Font.FontFamily.GetEmHeight(Font.Style);

		public int FontCellAscent => Font.FontFamily.GetCellAscent(Font.Style);

		public int FontLineSpacing => Font.FontFamily.GetLineSpacing(Font.Style);

		public int DescentHeight => DesignToPixel(FontCellDescent);

		public int AscentHeight => DesignToPixel(FontCellAscent);

		public event EventHandler PendingLayoutChanged;

		public virtual Inline GetHitItem(Point location, Point hitPoint)
		{
			if (IsNode || !Visible || IsBlock)
			{
				return null;
			}
			Rectangle rectangle = Bounds;
			rectangle.Offset(location);
			if (!rectangle.Contains(hitPoint))
			{
				return null;
			}
			return this;
		}

		public void Layout(LayoutType type)
		{
			if (type > PendingLayout)
			{
				PendingLayout = type;
			}
			if (ParentInline != null)
			{
				ParentInline.Layout(type);
			}
		}

		public virtual int GetFontSize()
		{
			int num = FontSize.Size;
			if (FontSize.Relative && parentInline != null)
			{
				num += parentInline.GetFontSize();
			}
			return num;
		}

		public virtual Font GetFont(string fontFamily, float fontSize, FontStyle fontStyle)
		{
			return GetService<IResources>(withException: true).GetFont(fontFamily, fontSize, fontStyle);
		}

		public virtual Image GetImage(string source)
		{
			return GetService<IResources>(withException: true).GetImage(source);
		}

		protected virtual void OnForeColorChanged()
		{
		}

		protected virtual void OnBackColorChanged()
		{
		}

		protected virtual void OnAlignChanged()
		{
			InvokeLayout(LayoutType.Position);
		}

		protected virtual void OnFontChanged()
		{
			InvokeLayout(LayoutType.Full);
		}

		protected virtual void OnPendingLayoutChanged()
		{
			if (this.PendingLayoutChanged != null)
			{
				this.PendingLayoutChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void InvokeLayout(LayoutType type)
		{
			Layout(type);
		}

		protected virtual void OnMouseCursorChanged()
		{
		}

		public virtual void MouseEnter()
		{
			if (!isHot)
			{
				isHot = true;
				OnMouseEnter();
				if (parentInline != null)
				{
					parentInline.MouseEnter();
				}
			}
		}

		public virtual void MouseLeave()
		{
			if (isHot)
			{
				isHot = false;
				OnMouseLeave();
				if (parentInline != null)
				{
					parentInline.MouseLeave();
				}
			}
		}

		public virtual void OnMouseClick()
		{
		}

		protected virtual void OnMouseEnter()
		{
		}

		protected virtual void OnMouseLeave()
		{
		}

		public virtual T GetService<T>(bool withException) where T : class
		{
			T val = this as T;
			if (val == null && parentInline != null)
			{
				val = parentInline.GetService<T>(withException: false);
			}
			if (val == null && withException)
			{
				throw new InvalidOperationException("Service not found");
			}
			return val;
		}

		private int DesignToPixel(int design)
		{
			return (int)Math.Round((float)Font.Height / (float)FontEmHeight * (float)design);
		}
	}
}
