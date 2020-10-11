using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public class ComicBrowserView : SubView
	{
		private ComicBrowserControl comicBrowser;

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

		public ComicBrowserView()
		{
			InitializeComponent();
		}

		protected override void OnMainFormChanged()
		{
			base.OnMainFormChanged();
			comicBrowser.Main = base.Main;
		}

		private void InitializeComponent()
		{
			comicBrowser = new cYo.Projects.ComicRack.Viewer.Views.ComicBrowserControl();
			SuspendLayout();
			comicBrowser.Caption = "";
			comicBrowser.CaptionMargin = new System.Windows.Forms.Padding(2);
			comicBrowser.DisableViewConfigUpdate = true;
			comicBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			comicBrowser.Location = new System.Drawing.Point(0, 0);
			comicBrowser.Name = "comicBrowser";
			comicBrowser.Size = new System.Drawing.Size(643, 552);
			comicBrowser.TabIndex = 0;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.Controls.Add(comicBrowser);
			base.Name = "ComicBrowserView";
			base.Size = new System.Drawing.Size(643, 552);
			ResumeLayout(false);
		}
	}
}
