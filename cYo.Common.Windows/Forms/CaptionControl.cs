using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Windows.Forms.Theme.Resources;

namespace cYo.Common.Windows.Forms
{
	public partial class CaptionControl : UserControlEx
	{
		private Padding captionMargin = new Padding(2);

		private bool closeButton;

		private bool selected;

		[Category("Display")]
		[DefaultValue(null)]
		public string Caption
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		[Category("Display")]
		[DefaultValue(typeof(Padding), "2")]
		public Padding CaptionMargin
		{
			get
			{
				return captionMargin;
			}
			set
			{
				if (!(captionMargin == value))
				{
					captionMargin = value;
					Refresh();
				}
			}
		}

		[Category("Display")]
		[DefaultValue(false)]
		public bool CloseButton
		{
			get
			{
				return closeButton;
			}
			set
			{
				if (closeButton != value)
				{
					closeButton = value;
					InvalidateCaption();
				}
			}
		}

		private Rectangle CaptionRectangle
		{
			get
			{
				if (string.IsNullOrEmpty(Caption))
				{
					return Rectangle.Empty;
				}
				Size size = TextRenderer.MeasureText(Caption, SystemFonts.SmallCaptionFont);
				Rectangle clientRectangle = base.ClientRectangle;
				clientRectangle.Height = size.Height + captionMargin.Vertical;
				return clientRectangle;
			}
		}

		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle clientRectangle = base.ClientRectangle;
				Rectangle captionRectangle = CaptionRectangle;
				clientRectangle.Height -= captionRectangle.Height;
				clientRectangle.Y = captionRectangle.Height;
				return clientRectangle;
			}
		}

		public CaptionControl()
		{
			InitializeComponent();
			SetStyle(ControlStyles.ResizeRedraw, value: true);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			DrawCaption(e.Graphics);
			e.Graphics.SetClip(CaptionRectangle, CombineMode.Exclude);
			base.OnPaintBackground(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			selected = false;
			InvalidateCaption();
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			selected = true;
			InvalidateCaption();
		}

		private void DrawCaption(Graphics gr)
		{
			Rectangle captionRectangle = CaptionRectangle;
			if (captionRectangle.Height != 0)
			{
				gr.FillRectangle(ThemeBrushes.Caption.Back, captionRectangle);
				gr.DrawStyledRectangle(captionRectangle, selected ? 255 : 128, StyledRenderer.VistaColor, StyledRenderer.Default.Frame(0, 1));
				TextRenderer.DrawText(gr, Caption, SystemFonts.SmallCaptionFont, captionRectangle.Pad(captionMargin), ThemeColors.Caption.Text);
			}
		}

		private void InvalidateCaption()
		{
			Invalidate(CaptionRectangle);
		}
	}
}
