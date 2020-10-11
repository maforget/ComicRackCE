using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Presentation.Ceco.Controls
{
	public class XHtmlDisplay : ScrollableControl
	{
		private IContainer components;

		private XHtmlControlRenderer renderer;

		[Browsable(true)]
		public override string Text
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

		[Browsable(true)]
		public Size DisplayMargin
		{
			get
			{
				return renderer.Body.Margin;
			}
			set
			{
				renderer.Body.Margin = value;
			}
		}

		public XHtmlDisplay()
		{
			InitializeComponent();
			SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
			DoubleBuffered = true;
			AutoScroll = true;
			renderer.Body.ActualSizeChanged += TextBlock_ActualSizeChanged;
		}

		private void TextBlock_ActualSizeChanged(object sender, EventArgs e)
		{
			base.AutoScrollMinSize = renderer.Body.ActualSize;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			renderer = new cYo.Common.Presentation.Ceco.Controls.XHtmlControlRenderer();
			SuspendLayout();
			renderer.Control = this;
			base.Name = "HtmlDisplay";
			base.Size = new System.Drawing.Size(248, 252);
			ResumeLayout(false);
		}
	}
}
