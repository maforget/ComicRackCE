using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer
{
    public partial class ReaderForm
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				base.Icon.Dispose();
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			SuspendLayout();
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(653, 621);
			base.KeyPreview = true;
			base.Name = "ReaderForm";
			Text = "Reader";
			ResumeLayout(false);
		}

		private IContainer components;

	}
}
