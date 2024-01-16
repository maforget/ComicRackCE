using System.ComponentModel;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicListFilesBrowser : ComicListBrowser
	{

		private IContainer components;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
				folderBooks.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // ComicListFilesBrowser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "ComicListFilesBrowser";
            this.ResumeLayout(false);

		}
	}
}

