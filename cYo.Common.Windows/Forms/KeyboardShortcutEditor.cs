using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public partial class KeyboardShortcutEditor : UserControlEx
	{
		private class KeyItem
		{
			private readonly CommandKey key;

			public CommandKey Key => key;

			public KeyItem(CommandKey key)
			{
				this.key = key;
			}

			public override string ToString()
			{
				return KeyboardCommand.GetKeyName(key);
			}
		}

		private KeyboardShortcuts shortcuts;

		private ListViewItem currentItem;

		private KeyboardCommand currentCommand;

		public KeyboardShortcuts Shortcuts
		{
			get
			{
				return shortcuts;
			}
			set
			{
				if (shortcuts != value)
				{
					shortcuts = value;
					FillList();
				}
			}
		}

		public KeyboardShortcutEditor()
		{
			InitializeComponent();
			imageList.ImageSize = imageList.ImageSize.ScaleDpi();
			lvCommands.Columns.ScaleDpi();
			LocalizeUtility.Localize(this, components);
			FillKeys(cbKey1);
			FillKeys(cbKey2);
			FillKeys(cbKey3);
			FillKeys(cbKey4);
		}

		public void RefreshList()
		{
			foreach (ListViewItem item in lvCommands.Items)
			{
				item.SubItems[1].Text = ((KeyboardCommand)item.Tag).KeyNames;
			}
			UpdateSelectedItem();
		}

		private void FillList()
		{
			foreach (KeyboardCommand command in shortcuts.Commands)
			{
				int imageIndex = -1;
				ListViewGroup listViewGroup = lvCommands.Groups[command.Group];
				if (listViewGroup == null)
				{
					listViewGroup = lvCommands.Groups.Add(command.Group, command.Group);
				}
				if (command.Image != null)
				{
					imageList.Images.Add(command.Image);
					imageIndex = imageList.Images.Count - 1;
				}
				ListViewItem listViewItem = lvCommands.Items.Add(command.Text, imageIndex);
				listViewItem.SubItems.Add(command.KeyNames);
				listViewItem.Tag = command;
				listViewGroup.Items.Add(listViewItem);
			}
			lvCommands.Items[0].Selected = true;
		}

		private static void FillKeys(ComboBox cb)
		{
			foreach (CommandKey value in Enum.GetValues(typeof(CommandKey)))
			{
				if (KeyboardCommand.IsKeyValue(value))
				{
					cb.Items.Add(new KeyItem(value));
				}
			}
		}

		private static void SelectKey(CommandKey ck, ComboBox cb)
		{
			foreach (KeyItem item in cb.Items)
			{
				if (item.Key == (ck & (CommandKey)65535))
				{
					cb.SelectedItem = item;
					return;
				}
			}
			SelectKey(CommandKey.None, cb);
		}

		private void SelectKey(int n, ComboBox cb, CheckBox ctrl, CheckBox shift, CheckBox alt, Button button)
		{
			cb.Tag = -1;
			ctrl.Tag = -1;
			shift.Tag = -1;
			alt.Tag = -1;
			if (currentCommand == null)
			{
				if (panelKeyEditor.Enabled)
				{
					panelKeyEditor.Enabled = false;
				}
				return;
			}
			if (!panelKeyEditor.Enabled)
			{
				panelKeyEditor.Enabled = true;
			}
			CommandKey commandKey = currentCommand.Keyboard[n];
			SelectKey(commandKey, cb);
			ctrl.Checked = (commandKey & CommandKey.Ctrl) != 0;
			shift.Checked = (commandKey & CommandKey.Shift) != 0;
			alt.Checked = (commandKey & CommandKey.Alt) != 0;
			cb.Tag = n;
			ctrl.Tag = n;
			shift.Tag = n;
			alt.Tag = n;
			button.Tag = n;
			cb.SelectedIndexChanged -= cb_SelectedIndexChanged;
			cb.SelectedIndexChanged += cb_SelectedIndexChanged;
			ctrl.CheckedChanged -= ctrl_CheckedChanged;
			ctrl.CheckedChanged += ctrl_CheckedChanged;
			shift.CheckedChanged -= shift_CheckedChanged;
			shift.CheckedChanged += shift_CheckedChanged;
			alt.CheckedChanged -= alt_CheckedChanged;
			alt.CheckedChanged += alt_CheckedChanged;
			button.Click -= button_Click;
			button.Click += button_Click;
		}

		private static void FlipCommand(ref CommandKey ck, CommandKey mask, bool flag)
		{
			if (flag)
			{
				ck |= mask;
			}
			else
			{
				ck &= ~mask;
			}
		}

		private void ctrl_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = (CheckBox)sender;
			int num = (int)checkBox.Tag;
			if (num != -1)
			{
				FlipCommand(ref currentCommand.Keyboard[num], CommandKey.Ctrl, checkBox.Checked);
				UpdateCurrentItem();
			}
		}

		private void shift_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = (CheckBox)sender;
			int num = (int)checkBox.Tag;
			if (num != -1)
			{
				FlipCommand(ref currentCommand.Keyboard[num], CommandKey.Shift, checkBox.Checked);
				UpdateCurrentItem();
			}
		}

		private void alt_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = (CheckBox)sender;
			int num = (int)checkBox.Tag;
			if (num != -1)
			{
				FlipCommand(ref currentCommand.Keyboard[num], CommandKey.Alt, checkBox.Checked);
				UpdateCurrentItem();
			}
		}

		private void cb_SelectedIndexChanged(object sender, EventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			int num = (int)comboBox.Tag;
			if (num != -1)
			{
				CommandKey commandKey = currentCommand.Keyboard[num];
				currentCommand.Keyboard[num] = ((KeyItem)comboBox.SelectedItem).Key | (commandKey & CommandKey.Modifiers);
				UpdateCurrentItem();
			}
		}

		private void button_Click(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			int num = (int)button.Tag;
			CommandKey commandKey = KeyInputForm.Show(this, LocalizeUtility.GetText(this, "GetKeyCaption", "Press Key"), LocalizeUtility.GetText(this, "GetKeyDescription", "Press your key combination or close this window"));
			if (commandKey != 0)
			{
				currentCommand.Keyboard[num] = commandKey;
				UpdateCurrentItem();
				UpdateControls();
			}
		}

		private void UpdateCurrentItem()
		{
			if (currentItem != null)
			{
				currentItem.SubItems[1].Text = currentCommand.KeyNames;
			}
		}

		private void UpdateSelectedItem()
		{
			if (lvCommands.SelectedItems.Count == 0)
			{
				currentItem = null;
				currentCommand = null;
				labelCurrentKey.Text = LocalizeUtility.GetText(this, "NothingSelected", "No action selected");
			}
			else
			{
				currentItem = lvCommands.SelectedItems[0];
				currentCommand = (KeyboardCommand)currentItem.Tag;
				labelCurrentKey.Text = currentCommand.Text + ":";
			}
			UpdateControls();
		}

		private void UpdateControls()
		{
			SelectKey(0, cbKey1, chkCtrl1, chkShift1, chkAlt1, btPress1);
			SelectKey(1, cbKey2, chkCtrl2, chkShift2, chkAlt2, btPress2);
			SelectKey(2, cbKey3, chkCtrl3, chkShift3, chkAlt3, btPress3);
			SelectKey(3, cbKey4, chkCtrl4, chkShift4, chkAlt4, btPress4);
		}

		private void lvCommands_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateSelectedItem();
		}
	}
}
