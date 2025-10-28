using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Viewer.Config;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class SaveWorkspaceDialog : FormEx
	{
		public SaveWorkspaceDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			this.RestorePosition();
			LocalizeUtility.Localize(this, null);
		}

		private void ValidateData(object sender, EventArgs e)
		{
			btOK.Enabled = (chkWindowLayouts.Checked || chkListLayouts.Checked || chkComicDisplayLayout.Checked || chkComicDisplaySettings.Checked) && !string.IsNullOrEmpty(txtName.Text);
		}

		public static bool Show(IWin32Window parent, DisplayWorkspace ws)
		{
			using (SaveWorkspaceDialog saveWorkspaceDialog = new SaveWorkspaceDialog())
			{
				saveWorkspaceDialog.txtName.Text = ws.Name;
				saveWorkspaceDialog.chkWindowLayouts.Checked = ws.IsWindowLayout;
				saveWorkspaceDialog.chkListLayouts.Checked = ws.IsViewsSetup;
				saveWorkspaceDialog.chkComicDisplayLayout.Checked = ws.IsComicPageLayout;
				saveWorkspaceDialog.chkComicDisplaySettings.Checked = ws.IsComicPageDisplay;
				if (saveWorkspaceDialog.ShowDialog(parent) != DialogResult.OK)
				{
					return false;
				}
				ws.Name = saveWorkspaceDialog.txtName.Text;
				ws.IsWindowLayout = saveWorkspaceDialog.chkWindowLayouts.Checked;
				ws.IsViewsSetup = saveWorkspaceDialog.chkListLayouts.Checked;
				ws.IsComicPageLayout = saveWorkspaceDialog.chkComicDisplayLayout.Checked;
				ws.IsComicPageDisplay = saveWorkspaceDialog.chkComicDisplaySettings.Checked;
				return true;
			}
		}

	}
}
