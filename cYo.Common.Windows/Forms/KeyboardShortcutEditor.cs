using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class KeyboardShortcutEditor : UserControl
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

		private IContainer components;

		private ColumnHeader chCommand;

		private ColumnHeader chKeyboard;

		private ColumnHeader chKeyboard2;

		private ListView lvCommands;

		private ColumnHeader chAction;

		private ColumnHeader chKeys;

		private Panel panelKeyEditor;

		private CheckBox chkCtrl1;

		private ComboBox cbKey1;

		private Label labelMainKey;

		private CheckBox chkShift1;

		private CheckBox chkAlt1;

		private CheckBox chkCtrl2;

		private ComboBox cbKey2;

		private CheckBox chkShift2;

		private Label labelAlternateKey;

		private CheckBox chkAlt2;

		private ImageList imageList;

		private Label labelCurrentKey;

		private CheckBox chkCtrl3;

		private ComboBox cbKey3;

		private CheckBox chkShift3;

		private Label labelAlternate2Key;

		private CheckBox chkAlt3;

		private Button btPress3;

		private Button btPress2;

		private Button btPress1;

		private Button btPress4;

		private CheckBox chkCtrl4;

		private ComboBox cbKey4;

		private CheckBox chkShift4;

		private Label labelAlternate3Key;

		private CheckBox chkAlt4;

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

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			chCommand = new System.Windows.Forms.ColumnHeader();
			chKeyboard = new System.Windows.Forms.ColumnHeader();
			chKeyboard2 = new System.Windows.Forms.ColumnHeader();
			lvCommands = new System.Windows.Forms.ListView();
			chAction = new System.Windows.Forms.ColumnHeader();
			chKeys = new System.Windows.Forms.ColumnHeader();
			imageList = new System.Windows.Forms.ImageList(components);
			panelKeyEditor = new System.Windows.Forms.Panel();
			btPress4 = new System.Windows.Forms.Button();
			chkCtrl4 = new System.Windows.Forms.CheckBox();
			cbKey4 = new System.Windows.Forms.ComboBox();
			chkShift4 = new System.Windows.Forms.CheckBox();
			labelAlternate3Key = new System.Windows.Forms.Label();
			chkAlt4 = new System.Windows.Forms.CheckBox();
			btPress3 = new System.Windows.Forms.Button();
			btPress2 = new System.Windows.Forms.Button();
			btPress1 = new System.Windows.Forms.Button();
			chkCtrl3 = new System.Windows.Forms.CheckBox();
			cbKey3 = new System.Windows.Forms.ComboBox();
			chkShift3 = new System.Windows.Forms.CheckBox();
			labelAlternate2Key = new System.Windows.Forms.Label();
			chkAlt3 = new System.Windows.Forms.CheckBox();
			labelCurrentKey = new System.Windows.Forms.Label();
			chkCtrl2 = new System.Windows.Forms.CheckBox();
			cbKey2 = new System.Windows.Forms.ComboBox();
			chkShift2 = new System.Windows.Forms.CheckBox();
			labelAlternateKey = new System.Windows.Forms.Label();
			chkAlt2 = new System.Windows.Forms.CheckBox();
			chkCtrl1 = new System.Windows.Forms.CheckBox();
			cbKey1 = new System.Windows.Forms.ComboBox();
			chkShift1 = new System.Windows.Forms.CheckBox();
			labelMainKey = new System.Windows.Forms.Label();
			chkAlt1 = new System.Windows.Forms.CheckBox();
			panelKeyEditor.SuspendLayout();
			SuspendLayout();
			chCommand.Text = "Action";
			chCommand.Width = 250;
			chKeyboard.Text = "Key";
			chKeyboard.Width = 80;
			chKeyboard2.Text = "Alternate";
			chKeyboard2.Width = 80;
			lvCommands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2]
			{
				chAction,
				chKeys
			});
			lvCommands.Dock = System.Windows.Forms.DockStyle.Fill;
			lvCommands.FullRowSelect = true;
			lvCommands.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			lvCommands.HideSelection = false;
			lvCommands.Location = new System.Drawing.Point(0, 0);
			lvCommands.MultiSelect = false;
			lvCommands.Name = "lvCommands";
			lvCommands.Size = new System.Drawing.Size(467, 230);
			lvCommands.SmallImageList = imageList;
			lvCommands.TabIndex = 0;
			lvCommands.UseCompatibleStateImageBehavior = false;
			lvCommands.View = System.Windows.Forms.View.Details;
			lvCommands.SelectedIndexChanged += new System.EventHandler(lvCommands_SelectedIndexChanged);
			chAction.Text = "Action";
			chAction.Width = 268;
			chKeys.Text = "Keys";
			chKeys.Width = 158;
			imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			imageList.ImageSize = new System.Drawing.Size(16, 16);
			imageList.TransparentColor = System.Drawing.Color.Transparent;
			panelKeyEditor.Controls.Add(btPress4);
			panelKeyEditor.Controls.Add(chkCtrl4);
			panelKeyEditor.Controls.Add(cbKey4);
			panelKeyEditor.Controls.Add(chkShift4);
			panelKeyEditor.Controls.Add(labelAlternate3Key);
			panelKeyEditor.Controls.Add(chkAlt4);
			panelKeyEditor.Controls.Add(btPress3);
			panelKeyEditor.Controls.Add(btPress2);
			panelKeyEditor.Controls.Add(btPress1);
			panelKeyEditor.Controls.Add(chkCtrl3);
			panelKeyEditor.Controls.Add(cbKey3);
			panelKeyEditor.Controls.Add(chkShift3);
			panelKeyEditor.Controls.Add(labelAlternate2Key);
			panelKeyEditor.Controls.Add(chkAlt3);
			panelKeyEditor.Controls.Add(labelCurrentKey);
			panelKeyEditor.Controls.Add(chkCtrl2);
			panelKeyEditor.Controls.Add(cbKey2);
			panelKeyEditor.Controls.Add(chkShift2);
			panelKeyEditor.Controls.Add(labelAlternateKey);
			panelKeyEditor.Controls.Add(chkAlt2);
			panelKeyEditor.Controls.Add(chkCtrl1);
			panelKeyEditor.Controls.Add(cbKey1);
			panelKeyEditor.Controls.Add(chkShift1);
			panelKeyEditor.Controls.Add(labelMainKey);
			panelKeyEditor.Controls.Add(chkAlt1);
			panelKeyEditor.Dock = System.Windows.Forms.DockStyle.Bottom;
			panelKeyEditor.Location = new System.Drawing.Point(0, 230);
			panelKeyEditor.Name = "panelKeyEditor";
			panelKeyEditor.Size = new System.Drawing.Size(467, 142);
			panelKeyEditor.TabIndex = 1;
			btPress4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btPress4.Location = new System.Drawing.Point(439, 113);
			btPress4.Name = "btPress4";
			btPress4.Size = new System.Drawing.Size(25, 21);
			btPress4.TabIndex = 24;
			btPress4.Text = "...";
			btPress4.UseVisualStyleBackColor = true;
			chkCtrl4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkCtrl4.AutoSize = true;
			chkCtrl4.Location = new System.Drawing.Point(259, 117);
			chkCtrl4.Name = "chkCtrl4";
			chkCtrl4.Size = new System.Drawing.Size(41, 17);
			chkCtrl4.TabIndex = 21;
			chkCtrl4.Text = "Ctrl";
			chkCtrl4.UseVisualStyleBackColor = true;
			cbKey4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbKey4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbKey4.FormattingEnabled = true;
			cbKey4.Location = new System.Drawing.Point(81, 113);
			cbKey4.Name = "cbKey4";
			cbKey4.Size = new System.Drawing.Size(170, 21);
			cbKey4.TabIndex = 20;
			chkShift4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkShift4.AutoSize = true;
			chkShift4.Location = new System.Drawing.Point(314, 117);
			chkShift4.Name = "chkShift4";
			chkShift4.Size = new System.Drawing.Size(47, 17);
			chkShift4.TabIndex = 22;
			chkShift4.Text = "Shift";
			chkShift4.UseVisualStyleBackColor = true;
			labelAlternate3Key.AutoSize = true;
			labelAlternate3Key.Location = new System.Drawing.Point(12, 116);
			labelAlternate3Key.Name = "labelAlternate3Key";
			labelAlternate3Key.Size = new System.Drawing.Size(52, 13);
			labelAlternate3Key.TabIndex = 19;
			labelAlternate3Key.Text = "Alternate:";
			chkAlt4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkAlt4.AutoSize = true;
			chkAlt4.Location = new System.Drawing.Point(378, 117);
			chkAlt4.Name = "chkAlt4";
			chkAlt4.Size = new System.Drawing.Size(38, 17);
			chkAlt4.TabIndex = 23;
			chkAlt4.Text = "Alt";
			chkAlt4.UseVisualStyleBackColor = true;
			btPress3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btPress3.Location = new System.Drawing.Point(439, 86);
			btPress3.Name = "btPress3";
			btPress3.Size = new System.Drawing.Size(25, 21);
			btPress3.TabIndex = 18;
			btPress3.Text = "...";
			btPress3.UseVisualStyleBackColor = true;
			btPress2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btPress2.Location = new System.Drawing.Point(439, 60);
			btPress2.Name = "btPress2";
			btPress2.Size = new System.Drawing.Size(25, 21);
			btPress2.TabIndex = 12;
			btPress2.Text = "...";
			btPress2.UseVisualStyleBackColor = true;
			btPress1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btPress1.Location = new System.Drawing.Point(439, 34);
			btPress1.Name = "btPress1";
			btPress1.Size = new System.Drawing.Size(25, 21);
			btPress1.TabIndex = 6;
			btPress1.Text = "...";
			btPress1.UseVisualStyleBackColor = true;
			chkCtrl3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkCtrl3.AutoSize = true;
			chkCtrl3.Location = new System.Drawing.Point(259, 90);
			chkCtrl3.Name = "chkCtrl3";
			chkCtrl3.Size = new System.Drawing.Size(41, 17);
			chkCtrl3.TabIndex = 15;
			chkCtrl3.Text = "Ctrl";
			chkCtrl3.UseVisualStyleBackColor = true;
			cbKey3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbKey3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbKey3.FormattingEnabled = true;
			cbKey3.Location = new System.Drawing.Point(81, 86);
			cbKey3.Name = "cbKey3";
			cbKey3.Size = new System.Drawing.Size(170, 21);
			cbKey3.TabIndex = 14;
			chkShift3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkShift3.AutoSize = true;
			chkShift3.Location = new System.Drawing.Point(314, 90);
			chkShift3.Name = "chkShift3";
			chkShift3.Size = new System.Drawing.Size(47, 17);
			chkShift3.TabIndex = 16;
			chkShift3.Text = "Shift";
			chkShift3.UseVisualStyleBackColor = true;
			labelAlternate2Key.AutoSize = true;
			labelAlternate2Key.Location = new System.Drawing.Point(12, 89);
			labelAlternate2Key.Name = "labelAlternate2Key";
			labelAlternate2Key.Size = new System.Drawing.Size(52, 13);
			labelAlternate2Key.TabIndex = 13;
			labelAlternate2Key.Text = "Alternate:";
			chkAlt3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkAlt3.AutoSize = true;
			chkAlt3.Location = new System.Drawing.Point(378, 90);
			chkAlt3.Name = "chkAlt3";
			chkAlt3.Size = new System.Drawing.Size(38, 17);
			chkAlt3.TabIndex = 17;
			chkAlt3.Text = "Alt";
			chkAlt3.UseVisualStyleBackColor = true;
			labelCurrentKey.AutoSize = true;
			labelCurrentKey.Location = new System.Drawing.Point(12, 12);
			labelCurrentKey.Name = "labelCurrentKey";
			labelCurrentKey.Size = new System.Drawing.Size(67, 13);
			labelCurrentKey.TabIndex = 0;
			labelCurrentKey.Text = "Lorum Ipsum";
			chkCtrl2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkCtrl2.AutoSize = true;
			chkCtrl2.Location = new System.Drawing.Point(259, 63);
			chkCtrl2.Name = "chkCtrl2";
			chkCtrl2.Size = new System.Drawing.Size(41, 17);
			chkCtrl2.TabIndex = 9;
			chkCtrl2.Text = "Ctrl";
			chkCtrl2.UseVisualStyleBackColor = true;
			cbKey2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbKey2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbKey2.FormattingEnabled = true;
			cbKey2.Location = new System.Drawing.Point(81, 59);
			cbKey2.Name = "cbKey2";
			cbKey2.Size = new System.Drawing.Size(170, 21);
			cbKey2.TabIndex = 8;
			chkShift2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkShift2.AutoSize = true;
			chkShift2.Location = new System.Drawing.Point(314, 63);
			chkShift2.Name = "chkShift2";
			chkShift2.Size = new System.Drawing.Size(47, 17);
			chkShift2.TabIndex = 10;
			chkShift2.Text = "Shift";
			chkShift2.UseVisualStyleBackColor = true;
			labelAlternateKey.AutoSize = true;
			labelAlternateKey.Location = new System.Drawing.Point(12, 62);
			labelAlternateKey.Name = "labelAlternateKey";
			labelAlternateKey.Size = new System.Drawing.Size(52, 13);
			labelAlternateKey.TabIndex = 7;
			labelAlternateKey.Text = "Alternate:";
			chkAlt2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkAlt2.AutoSize = true;
			chkAlt2.Location = new System.Drawing.Point(378, 63);
			chkAlt2.Name = "chkAlt2";
			chkAlt2.Size = new System.Drawing.Size(38, 17);
			chkAlt2.TabIndex = 11;
			chkAlt2.Text = "Alt";
			chkAlt2.UseVisualStyleBackColor = true;
			chkCtrl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkCtrl1.AutoSize = true;
			chkCtrl1.Location = new System.Drawing.Point(259, 37);
			chkCtrl1.Name = "chkCtrl1";
			chkCtrl1.Size = new System.Drawing.Size(41, 17);
			chkCtrl1.TabIndex = 3;
			chkCtrl1.Text = "Ctrl";
			chkCtrl1.UseVisualStyleBackColor = true;
			cbKey1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbKey1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbKey1.FormattingEnabled = true;
			cbKey1.Location = new System.Drawing.Point(81, 34);
			cbKey1.Name = "cbKey1";
			cbKey1.Size = new System.Drawing.Size(170, 21);
			cbKey1.TabIndex = 2;
			chkShift1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkShift1.AutoSize = true;
			chkShift1.Location = new System.Drawing.Point(314, 37);
			chkShift1.Name = "chkShift1";
			chkShift1.Size = new System.Drawing.Size(47, 17);
			chkShift1.TabIndex = 4;
			chkShift1.Text = "Shift";
			chkShift1.UseVisualStyleBackColor = true;
			labelMainKey.AutoSize = true;
			labelMainKey.Location = new System.Drawing.Point(12, 38);
			labelMainKey.Name = "labelMainKey";
			labelMainKey.Size = new System.Drawing.Size(33, 13);
			labelMainKey.TabIndex = 1;
			labelMainKey.Text = "Main:";
			chkAlt1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			chkAlt1.AutoSize = true;
			chkAlt1.Location = new System.Drawing.Point(378, 37);
			chkAlt1.Name = "chkAlt1";
			chkAlt1.Size = new System.Drawing.Size(38, 17);
			chkAlt1.TabIndex = 5;
			chkAlt1.Text = "Alt";
			chkAlt1.UseVisualStyleBackColor = true;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			base.Controls.Add(lvCommands);
			base.Controls.Add(panelKeyEditor);
			base.Name = "KeyboardShortcutEditor";
			base.Size = new System.Drawing.Size(467, 372);
			panelKeyEditor.ResumeLayout(false);
			panelKeyEditor.PerformLayout();
			ResumeLayout(false);
		}
	}
}
