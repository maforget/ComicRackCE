using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class EditListDialog : FormEx
	{
		public override UIComponent UIComponent => UIComponent.Content;

		public EditListDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			chkShowNotes.Image = ((Bitmap)chkShowNotes.Image).ScaleDpi();
			this.RestorePosition();
			LocalizeUtility.Localize(this, null);
			cbCombineMode.Items.AddRange(TR.Load(base.Name).GetStrings("CombineMode", "All Books from every list|Only Books existing in every list|Empty list", '|'));
		}

		private void chkShowNotes_CheckedChanged(object sender, EventArgs e)
		{
			panelNotes.Visible = chkShowNotes.Checked;
		}

		public static bool Edit(IWin32Window parent, ComicListItem item)
		{
			using (EditListDialog editListDialog = new EditListDialog())
			{
				ComicListItemFolder comicListItemFolder = item as ComicListItemFolder;
				ShareableComicListItem shareableComicListItem = item as ShareableComicListItem;
				editListDialog.txtName.Text = item.Name;
				editListDialog.txtNotes.Text = StringUtility.MakeEditBoxMultiline(item.Description);
				if (shareableComicListItem != null)
				{
					editListDialog.chkQuickOpen.Visible = true;
					editListDialog.chkQuickOpen.Checked = shareableComicListItem.QuickOpen;
				}
				else
				{
					editListDialog.chkQuickOpen.Visible = false;
				}
				if (comicListItemFolder != null)
				{
					editListDialog.panelBooks.Visible = true;
					editListDialog.cbCombineMode.SelectedIndex = (int)comicListItemFolder.CombineMode;
				}
				else
				{
					editListDialog.panelBooks.Visible = false;
				}
				CheckBox checkBox = editListDialog.chkShowNotes;
				bool @checked = (editListDialog.panelNotes.Visible = !string.IsNullOrEmpty(editListDialog.txtNotes.Text) || editListDialog.chkQuickOpen.Checked);
				checkBox.Checked = @checked;
				if (editListDialog.ShowDialog(parent) == DialogResult.Cancel)
				{
					return false;
				}
				item.Name = editListDialog.txtName.Text.Trim();
				item.Description = editListDialog.txtNotes.Text.Trim();
				if (shareableComicListItem != null)
				{
					shareableComicListItem.QuickOpen = editListDialog.chkQuickOpen.Checked;
				}
				if (comicListItemFolder != null)
				{
					comicListItemFolder.CombineMode = (ComicFolderCombineMode)editListDialog.cbCombineMode.SelectedIndex;
				}
				return true;
			}
		}


	}
}
