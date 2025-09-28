using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class DeleteItemDialog : FormEx
	{
		public DeleteItemDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
			new ComboBoxSkinner(cbItems);
		}

		public static List<T> GetList<T>(IWin32Window parent, string caption, IEnumerable<T> list) where T : class
		{
			using (DeleteItemDialog deleteItemDialog = new DeleteItemDialog())
			{
				deleteItemDialog.Text = StringUtility.Format(deleteItemDialog.Text, caption);
				deleteItemDialog.lbCaption.Text = $"{caption}:";
				List<T> list2 = new List<T>(list);
				list2.Sort();
				deleteItemDialog.cbItems.Items.AddRange(list2.ToArray());
				deleteItemDialog.cbItems.Items.Add(new ComboBoxSkinner.ComboBoxSeparator("All"));
				deleteItemDialog.cbItems.SelectedIndex = 0;
				if (deleteItemDialog.ShowDialog(parent) == DialogResult.Cancel)
				{
					return null;
				}
				List<T> list3 = new List<T>();
				if (deleteItemDialog.cbItems.SelectedItem is ComboBoxSkinner.ComboBoxSeparator)
				{
					list3.AddRange(list);
				}
				else
				{
					list3.Add(deleteItemDialog.cbItems.SelectedItem as T);
				}
				return list3;
			}
		}

	}
}
