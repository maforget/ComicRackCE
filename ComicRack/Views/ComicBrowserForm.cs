using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicBrowserForm : FormEx
	{
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IComicBookListProvider BookList
		{
			get
			{
				return comicBrowser.BookList;
			}
			set
			{
				comicBrowser.BookList = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IMain Main
		{
			get
			{
				return comicBrowser.Main;
			}
			set
			{
				comicBrowser.Main = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool BookListOwned
		{
			get;
			set;
		}

		public ComicBrowserForm()
		{
			InitializeComponent();
			base.Icon = Resources.ComicRackAppSmall;
			statusStrip.Text = TR.Default["Ready", statusStrip.Text];
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			IdleProcess.Idle += OnIdle;
			comicBrowser.HideNavigation = true;
		}

		private void OnIdle(object sender, EventArgs e)
		{
			Text = comicBrowser.BookList.Name;
			tsText.Text = FormUtility.FixAmpersand(comicBrowser.SelectionInfo);
		}
	}
}
