using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Mathematics;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class ListEditorDialog : Form
	{
		private Action newAction;

		private Action editAction;

		private Action activateAction;

		private Action setAllAction;

		public IList Items
		{
			get;
			set;
		}

		public object SelectedItem
		{
			get
			{
				if (lvItems.SelectedItems.Count != 0)
				{
					return lvItems.SelectedItems[0].Tag;
				}
				return null;
			}
		}

		public ListEditorDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
			this.RestorePosition();
		}

		private void FillList()
		{
			object selectedItem = SelectedItem;
			lvItems.BeginUpdate();
			lvItems.Items.Clear();
			foreach (object item in Items)
			{
				INamed named = item as INamed;
				IDescription description = item as IDescription;
				ListViewItem listViewItem = lvItems.Items.Add((named != null) ? named.Name : item.ToString());
				if (description != null)
				{
					listViewItem.SubItems.Add(description.Description);
				}
				listViewItem.Tag = item;
				listViewItem.Selected = item == selectedItem;
			}
			if (lvItems.SelectedIndices.Count == 0 && lvItems.Items.Count > 0)
			{
				lvItems.Items[0].Selected = true;
			}
			if (lvItems.SelectedIndices.Count > 0)
			{
				lvItems.EnsureVisible(lvItems.SelectedIndices[0]);
			}
			lvItems.EndUpdate();
			UpdateButtons();
		}

		private void MoveSelected(int offset, bool absolute = false)
		{
			if (SelectedItem != null)
			{
				int num = Items.IndexOf(SelectedItem);
				int newIndex = (absolute ? offset : (num + offset)).Clamp(0, Items.Count);
				Items.Move(num, newIndex);
				FillList();
			}
		}

        private void UpdateButtons()
        {
            bool flag = lvItems.SelectedItems.Count > 0;
            btMoveTop.Enabled = btMoveUp.Enabled = flag && lvItems.SelectedIndices[0] > 0;
            btMoveBottom.Enabled = btMoveDown.Enabled = flag && lvItems.SelectedIndices[0] < Items.Count - 1;
            btEdit.Enabled = btActivate.Enabled = btDelete.Enabled = flag;
        }

        private void lvItems_MouseReorder(object sender, ListViewEx.MouseReorderEventArgs e)
		{
			e.Cancel = true;
			MoveSelected(e.ToIndex, absolute: true);
		}

		private void btMoveTop_Click(object sender, EventArgs e)
		{
			MoveSelected(0, absolute: true);
		}

		private void btMoveUp_Click(object sender, EventArgs e)
		{
			MoveSelected(-1);
		}

		private void btMoveDown_Click(object sender, EventArgs e)
		{
			MoveSelected(1);
		}

		private void btMoveBottom_Click(object sender, EventArgs e)
		{
			MoveSelected(Items.Count - 1, absolute: true);
		}

		private void btDelete_Click(object sender, EventArgs e)
		{
			Items.Remove(SelectedItem);
			FillList();
		}

		private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateButtons();
		}

		private void btNew_Click(object sender, EventArgs e)
		{
			OnNew();
		}

		private void btEdit_Click(object sender, EventArgs e)
		{
			OnEdit();
		}

		private void btActivate_Click(object sender, EventArgs e)
		{
			OnActivate();
		}

		private void btSetAll_Click(object sender, EventArgs e)
		{
			OnSetAll();
		}

		private void lvItems_DoubleClick(object sender, EventArgs e)
		{
			if (editAction != null)
			{
				OnEdit();
			}
			else
			{
				OnActivate();
			}
		}

		protected virtual void OnNew()
		{
			if (newAction != null)
			{
				newAction();
			}
		}

		protected virtual void OnEdit()
		{
			if (editAction != null)
			{
				editAction();
			}
		}

		protected virtual void OnActivate()
		{
			if (activateAction != null)
			{
				activateAction();
			}
		}

		protected virtual void OnSetAll()
		{
			if (setAllAction != null)
			{
				setAllAction();
			}
		}

		public static IList<T> Show<T>(IWin32Window parent, string caption, IList<T> items, Func<T> newAction = null, Func<T, bool> editAction = null, Action<T> activateAction = null, Action<T> setAllAction = null) where T : class
		{
            items = (IList<T>)items.ToList<T>();
            using (ListEditorDialog dlg = new ListEditorDialog())
            {
                dlg.Text = caption;
                dlg.Items = (IList)items;
                dlg.FillList();
                if (newAction != null)
                {
                    dlg.btNew.Visible = true;
                    dlg.newAction = (Action)(() =>
                    {
                        T obj = newAction();
                        if ((object)obj == null)
                            return;
                        ((ICollection<T>)items).Add(obj);
                        dlg.FillList();
                    });
                }
                if (editAction != null)
                {
                    dlg.btEdit.Visible = true;
                    dlg.editAction = (Action)(() =>
                    {
                        if (!(dlg.SelectedItem is T selectedItem2) || !editAction(selectedItem2))
                            return;
                        dlg.FillList();
                    });
                }
                if (activateAction != null)
                {
                    dlg.btActivate.Visible = true;
                    dlg.activateAction = (Action)(() =>
                    {
                        if (!(dlg.SelectedItem is T selectedItem4))
                            return;
                        activateAction(selectedItem4);
                    });
                }
                if (setAllAction != null)
                {
                    dlg.btSetAll.Visible = true;
                    dlg.setAllAction = (Action)(() =>
                    {
                        if (!(dlg.SelectedItem is T selectedItem6))
                            return;
                        setAllAction(selectedItem6);
                    });
                }
                return dlg.ShowDialog(parent) == DialogResult.Cancel ? (IList<T>)null : items;
            }
        }
	}
}
