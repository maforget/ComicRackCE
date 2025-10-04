using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer
{
	public partial class ReaderForm : FormEx
	{
		private Rectangle safeBounds;

		public ComicDisplay ComicDisplay
		{
			get;
			set;
		}

		public Rectangle SafeBounds
		{
			get
			{
				return safeBounds;
			}
			set
			{
				base.StartPosition = FormStartPosition.Manual;
				base.Bounds = value;
				safeBounds = value;
			}
		}

		public ReaderForm(ComicDisplay comicDisplay)
		{
			InitializeComponent();
			base.Icon = Resources.ComicRackAppSmall;
			ComicDisplay = comicDisplay;
			LocalizeUtility.Localize(this, components);
		}


		protected override void OnResizeEnd(EventArgs e)
		{
			base.OnResizeEnd(e);
			UpdateSafeBounds();
		}

		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);
			UpdateSafeBounds();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing && !Program.AskQuestion(this, TR.Messages["CloseExternalReader", "This will only close the reader Window and not the open Book(s)!"], TR.Default["OK", "OK"], HiddenMessageBoxes.CloseExternalReader, TR.Messages["DontShowAgain", "Do not show this again"]))
			{
				e.Cancel = true;
			}
			if (!e.Cancel)
			{
				base.OnFormClosing(e);
			}
		}

		private void UpdateSafeBounds()
		{
			if (base.IsHandleCreated && base.WindowState == FormWindowState.Normal && base.FormBorderStyle != 0)
			{
				safeBounds = base.Bounds;
			}
		}


	}
}
