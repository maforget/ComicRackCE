using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Presentation.Ceco.Controls
{
	public class XHtmlLabel : Control
	{
		private ContentAlignment textAlign = ContentAlignment.MiddleLeft;

		private Size textMargin;

		private XHtmlControlRenderer renderer;

		[Browsable(true)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		[DefaultValue(typeof(Color), "Transparent")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		[Category("Appearance")]
		[Description("The alignment of the text")]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		public ContentAlignment TextAlign
		{
			get
			{
				return textAlign;
			}
			set
			{
				if (textAlign != value)
				{
					textAlign = value;
					OnTextAlignChanged();
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[Description("The margin between content and border")]
		[DefaultValue(typeof(Size), "0,0")]
		public Size TextMargin
		{
			get
			{
				return textMargin;
			}
			set
			{
				if (!(textMargin == value))
				{
					textMargin = value;
					OnTextMarginChanged();
					Recalculate();
					Invalidate();
				}
			}
		}

		public event EventHandler TextAlignChanged;

		public event EventHandler TextMarginChanged;

		public XHtmlLabel()
		{
			InitializeComponent();
			SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
			DoubleBuffered = true;
			BackColor = Color.Transparent;
			renderer.Control = this;
		}

		protected virtual void OnTextMarginChanged()
		{
			if (this.TextMarginChanged != null)
			{
				this.TextMarginChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnTextAlignChanged()
		{
			renderer.Body.SetAlign(textAlign);
			if (this.TextAlignChanged != null)
			{
				this.TextAlignChanged(this, EventArgs.Empty);
			}
		}

		private void Recalculate()
		{
			if (AutoSize)
			{
				base.Size = GetPreferredSize(base.PreferredSize);
			}
		}

		private void InitializeComponent()
		{
			renderer = new cYo.Common.Presentation.Ceco.Controls.XHtmlControlRenderer();
			SuspendLayout();
			renderer.Control = null;
			ResumeLayout(false);
		}
	}
}
