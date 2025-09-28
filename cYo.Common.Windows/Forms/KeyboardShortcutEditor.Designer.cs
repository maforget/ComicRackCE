using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public partial class KeyboardShortcutEditor : UserControlEx
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.chCommand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chKeyboard = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chKeyboard2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvCommands = new System.Windows.Forms.ListView();
            this.chAction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chKeys = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panelKeyEditor = new System.Windows.Forms.Panel();
            this.btPress4 = new System.Windows.Forms.Button();
            this.chkCtrl4 = new System.Windows.Forms.CheckBox();
            this.cbKey4 = new System.Windows.Forms.ComboBox();
            this.chkShift4 = new System.Windows.Forms.CheckBox();
            this.labelAlternate3Key = new System.Windows.Forms.Label();
            this.chkAlt4 = new System.Windows.Forms.CheckBox();
            this.btPress3 = new System.Windows.Forms.Button();
            this.btPress2 = new System.Windows.Forms.Button();
            this.btPress1 = new System.Windows.Forms.Button();
            this.chkCtrl3 = new System.Windows.Forms.CheckBox();
            this.cbKey3 = new System.Windows.Forms.ComboBox();
            this.chkShift3 = new System.Windows.Forms.CheckBox();
            this.labelAlternate2Key = new System.Windows.Forms.Label();
            this.chkAlt3 = new System.Windows.Forms.CheckBox();
            this.labelCurrentKey = new System.Windows.Forms.Label();
            this.chkCtrl2 = new System.Windows.Forms.CheckBox();
            this.cbKey2 = new System.Windows.Forms.ComboBox();
            this.chkShift2 = new System.Windows.Forms.CheckBox();
            this.labelAlternateKey = new System.Windows.Forms.Label();
            this.chkAlt2 = new System.Windows.Forms.CheckBox();
            this.chkCtrl1 = new System.Windows.Forms.CheckBox();
            this.cbKey1 = new System.Windows.Forms.ComboBox();
            this.chkShift1 = new System.Windows.Forms.CheckBox();
            this.labelMainKey = new System.Windows.Forms.Label();
            this.chkAlt1 = new System.Windows.Forms.CheckBox();
            this.panelKeyEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // chCommand
            // 
            this.chCommand.Text = "Action";
            this.chCommand.Width = 250;
            // 
            // chKeyboard
            // 
            this.chKeyboard.Text = "Key";
            this.chKeyboard.Width = 80;
            // 
            // chKeyboard2
            // 
            this.chKeyboard2.Text = "Alternate";
            this.chKeyboard2.Width = 80;
            // 
            // lvCommands
            // 
            this.lvCommands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAction,
            this.chKeys});
            this.lvCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvCommands.FullRowSelect = true;
            this.lvCommands.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvCommands.HideSelection = false;
            this.lvCommands.Location = new System.Drawing.Point(0, 0);
            this.lvCommands.MultiSelect = false;
            this.lvCommands.Name = "lvCommands";
            this.lvCommands.Size = new System.Drawing.Size(467, 230);
            this.lvCommands.SmallImageList = this.imageList;
            this.lvCommands.TabIndex = 0;
            this.lvCommands.UseCompatibleStateImageBehavior = false;
            this.lvCommands.View = System.Windows.Forms.View.Details;
            this.lvCommands.SelectedIndexChanged += new System.EventHandler(this.lvCommands_SelectedIndexChanged);
            // 
            // chAction
            // 
            this.chAction.Text = "Action";
            this.chAction.Width = 268;
            // 
            // chKeys
            // 
            this.chKeys.Text = "Keys";
            this.chKeys.Width = 158;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panelKeyEditor
            // 
            this.panelKeyEditor.Controls.Add(this.btPress4);
            this.panelKeyEditor.Controls.Add(this.chkCtrl4);
            this.panelKeyEditor.Controls.Add(this.cbKey4);
            this.panelKeyEditor.Controls.Add(this.chkShift4);
            this.panelKeyEditor.Controls.Add(this.labelAlternate3Key);
            this.panelKeyEditor.Controls.Add(this.chkAlt4);
            this.panelKeyEditor.Controls.Add(this.btPress3);
            this.panelKeyEditor.Controls.Add(this.btPress2);
            this.panelKeyEditor.Controls.Add(this.btPress1);
            this.panelKeyEditor.Controls.Add(this.chkCtrl3);
            this.panelKeyEditor.Controls.Add(this.cbKey3);
            this.panelKeyEditor.Controls.Add(this.chkShift3);
            this.panelKeyEditor.Controls.Add(this.labelAlternate2Key);
            this.panelKeyEditor.Controls.Add(this.chkAlt3);
            this.panelKeyEditor.Controls.Add(this.labelCurrentKey);
            this.panelKeyEditor.Controls.Add(this.chkCtrl2);
            this.panelKeyEditor.Controls.Add(this.cbKey2);
            this.panelKeyEditor.Controls.Add(this.chkShift2);
            this.panelKeyEditor.Controls.Add(this.labelAlternateKey);
            this.panelKeyEditor.Controls.Add(this.chkAlt2);
            this.panelKeyEditor.Controls.Add(this.chkCtrl1);
            this.panelKeyEditor.Controls.Add(this.cbKey1);
            this.panelKeyEditor.Controls.Add(this.chkShift1);
            this.panelKeyEditor.Controls.Add(this.labelMainKey);
            this.panelKeyEditor.Controls.Add(this.chkAlt1);
            this.panelKeyEditor.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelKeyEditor.Location = new System.Drawing.Point(0, 230);
            this.panelKeyEditor.Name = "panelKeyEditor";
            this.panelKeyEditor.Size = new System.Drawing.Size(467, 142);
            this.panelKeyEditor.TabIndex = 1;
            // 
            // btPress4
            // 
            this.btPress4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btPress4.Location = new System.Drawing.Point(439, 113);
            this.btPress4.Name = "btPress4";
            this.btPress4.Size = new System.Drawing.Size(25, 21);
            this.btPress4.TabIndex = 24;
            this.btPress4.Text = "...";
            this.btPress4.UseVisualStyleBackColor = true;
            // 
            // chkCtrl4
            // 
            this.chkCtrl4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCtrl4.AutoSize = true;
            this.chkCtrl4.Location = new System.Drawing.Point(259, 117);
            this.chkCtrl4.Name = "chkCtrl4";
            this.chkCtrl4.Size = new System.Drawing.Size(41, 17);
            this.chkCtrl4.TabIndex = 21;
            this.chkCtrl4.Text = "Ctrl";
            this.chkCtrl4.UseVisualStyleBackColor = true;
            // 
            // cbKey4
            // 
            this.cbKey4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbKey4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKey4.FormattingEnabled = true;
            this.cbKey4.Location = new System.Drawing.Point(81, 113);
            this.cbKey4.Name = "cbKey4";
            this.cbKey4.Size = new System.Drawing.Size(170, 21);
            this.cbKey4.TabIndex = 20;
            // 
            // chkShift4
            // 
            this.chkShift4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShift4.AutoSize = true;
            this.chkShift4.Location = new System.Drawing.Point(314, 117);
            this.chkShift4.Name = "chkShift4";
            this.chkShift4.Size = new System.Drawing.Size(47, 17);
            this.chkShift4.TabIndex = 22;
            this.chkShift4.Text = "Shift";
            this.chkShift4.UseVisualStyleBackColor = true;
            // 
            // labelAlternate3Key
            // 
            this.labelAlternate3Key.AutoSize = true;
            this.labelAlternate3Key.Location = new System.Drawing.Point(12, 116);
            this.labelAlternate3Key.Name = "labelAlternate3Key";
            this.labelAlternate3Key.Size = new System.Drawing.Size(52, 13);
            this.labelAlternate3Key.TabIndex = 19;
            this.labelAlternate3Key.Text = "Alternate:";
            // 
            // chkAlt4
            // 
            this.chkAlt4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAlt4.AutoSize = true;
            this.chkAlt4.Location = new System.Drawing.Point(378, 117);
            this.chkAlt4.Name = "chkAlt4";
            this.chkAlt4.Size = new System.Drawing.Size(38, 17);
            this.chkAlt4.TabIndex = 23;
            this.chkAlt4.Text = "Alt";
            this.chkAlt4.UseVisualStyleBackColor = true;
            // 
            // btPress3
            // 
            this.btPress3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btPress3.Location = new System.Drawing.Point(439, 86);
            this.btPress3.Name = "btPress3";
            this.btPress3.Size = new System.Drawing.Size(25, 21);
            this.btPress3.TabIndex = 18;
            this.btPress3.Text = "...";
            this.btPress3.UseVisualStyleBackColor = true;
            // 
            // btPress2
            // 
            this.btPress2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btPress2.Location = new System.Drawing.Point(439, 60);
            this.btPress2.Name = "btPress2";
            this.btPress2.Size = new System.Drawing.Size(25, 21);
            this.btPress2.TabIndex = 12;
            this.btPress2.Text = "...";
            this.btPress2.UseVisualStyleBackColor = true;
            // 
            // btPress1
            // 
            this.btPress1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btPress1.Location = new System.Drawing.Point(439, 34);
            this.btPress1.Name = "btPress1";
            this.btPress1.Size = new System.Drawing.Size(25, 21);
            this.btPress1.TabIndex = 6;
            this.btPress1.Text = "...";
            this.btPress1.UseVisualStyleBackColor = true;
            // 
            // chkCtrl3
            // 
            this.chkCtrl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCtrl3.AutoSize = true;
            this.chkCtrl3.Location = new System.Drawing.Point(259, 90);
            this.chkCtrl3.Name = "chkCtrl3";
            this.chkCtrl3.Size = new System.Drawing.Size(41, 17);
            this.chkCtrl3.TabIndex = 15;
            this.chkCtrl3.Text = "Ctrl";
            this.chkCtrl3.UseVisualStyleBackColor = true;
            // 
            // cbKey3
            // 
            this.cbKey3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbKey3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKey3.FormattingEnabled = true;
            this.cbKey3.Location = new System.Drawing.Point(81, 86);
            this.cbKey3.Name = "cbKey3";
            this.cbKey3.Size = new System.Drawing.Size(170, 21);
            this.cbKey3.TabIndex = 14;
            // 
            // chkShift3
            // 
            this.chkShift3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShift3.AutoSize = true;
            this.chkShift3.Location = new System.Drawing.Point(314, 90);
            this.chkShift3.Name = "chkShift3";
            this.chkShift3.Size = new System.Drawing.Size(47, 17);
            this.chkShift3.TabIndex = 16;
            this.chkShift3.Text = "Shift";
            this.chkShift3.UseVisualStyleBackColor = true;
            // 
            // labelAlternate2Key
            // 
            this.labelAlternate2Key.AutoSize = true;
            this.labelAlternate2Key.Location = new System.Drawing.Point(12, 89);
            this.labelAlternate2Key.Name = "labelAlternate2Key";
            this.labelAlternate2Key.Size = new System.Drawing.Size(52, 13);
            this.labelAlternate2Key.TabIndex = 13;
            this.labelAlternate2Key.Text = "Alternate:";
            // 
            // chkAlt3
            // 
            this.chkAlt3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAlt3.AutoSize = true;
            this.chkAlt3.Location = new System.Drawing.Point(378, 90);
            this.chkAlt3.Name = "chkAlt3";
            this.chkAlt3.Size = new System.Drawing.Size(38, 17);
            this.chkAlt3.TabIndex = 17;
            this.chkAlt3.Text = "Alt";
            this.chkAlt3.UseVisualStyleBackColor = true;
            // 
            // labelCurrentKey
            // 
            this.labelCurrentKey.AutoSize = true;
            this.labelCurrentKey.Location = new System.Drawing.Point(12, 12);
            this.labelCurrentKey.Name = "labelCurrentKey";
            this.labelCurrentKey.Size = new System.Drawing.Size(67, 13);
            this.labelCurrentKey.TabIndex = 0;
            this.labelCurrentKey.Text = "Lorum Ipsum";
            // 
            // chkCtrl2
            // 
            this.chkCtrl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCtrl2.AutoSize = true;
            this.chkCtrl2.Location = new System.Drawing.Point(259, 63);
            this.chkCtrl2.Name = "chkCtrl2";
            this.chkCtrl2.Size = new System.Drawing.Size(41, 17);
            this.chkCtrl2.TabIndex = 9;
            this.chkCtrl2.Text = "Ctrl";
            this.chkCtrl2.UseVisualStyleBackColor = true;
            // 
            // cbKey2
            // 
            this.cbKey2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbKey2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKey2.FormattingEnabled = true;
            this.cbKey2.Location = new System.Drawing.Point(81, 59);
            this.cbKey2.Name = "cbKey2";
            this.cbKey2.Size = new System.Drawing.Size(170, 21);
            this.cbKey2.TabIndex = 8;
            // 
            // chkShift2
            // 
            this.chkShift2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShift2.AutoSize = true;
            this.chkShift2.Location = new System.Drawing.Point(314, 63);
            this.chkShift2.Name = "chkShift2";
            this.chkShift2.Size = new System.Drawing.Size(47, 17);
            this.chkShift2.TabIndex = 10;
            this.chkShift2.Text = "Shift";
            this.chkShift2.UseVisualStyleBackColor = true;
            // 
            // labelAlternateKey
            // 
            this.labelAlternateKey.AutoSize = true;
            this.labelAlternateKey.Location = new System.Drawing.Point(12, 62);
            this.labelAlternateKey.Name = "labelAlternateKey";
            this.labelAlternateKey.Size = new System.Drawing.Size(52, 13);
            this.labelAlternateKey.TabIndex = 7;
            this.labelAlternateKey.Text = "Alternate:";
            // 
            // chkAlt2
            // 
            this.chkAlt2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAlt2.AutoSize = true;
            this.chkAlt2.Location = new System.Drawing.Point(378, 63);
            this.chkAlt2.Name = "chkAlt2";
            this.chkAlt2.Size = new System.Drawing.Size(38, 17);
            this.chkAlt2.TabIndex = 11;
            this.chkAlt2.Text = "Alt";
            this.chkAlt2.UseVisualStyleBackColor = true;
            // 
            // chkCtrl1
            // 
            this.chkCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCtrl1.AutoSize = true;
            this.chkCtrl1.Location = new System.Drawing.Point(259, 37);
            this.chkCtrl1.Name = "chkCtrl1";
            this.chkCtrl1.Size = new System.Drawing.Size(41, 17);
            this.chkCtrl1.TabIndex = 3;
            this.chkCtrl1.Text = "Ctrl";
            this.chkCtrl1.UseVisualStyleBackColor = true;
            // 
            // cbKey1
            // 
            this.cbKey1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbKey1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKey1.FormattingEnabled = true;
            this.cbKey1.Location = new System.Drawing.Point(81, 34);
            this.cbKey1.Name = "cbKey1";
            this.cbKey1.Size = new System.Drawing.Size(170, 21);
            this.cbKey1.TabIndex = 2;
            // 
            // chkShift1
            // 
            this.chkShift1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShift1.AutoSize = true;
            this.chkShift1.Location = new System.Drawing.Point(314, 37);
            this.chkShift1.Name = "chkShift1";
            this.chkShift1.Size = new System.Drawing.Size(47, 17);
            this.chkShift1.TabIndex = 4;
            this.chkShift1.Text = "Shift";
            this.chkShift1.UseVisualStyleBackColor = true;
            // 
            // labelMainKey
            // 
            this.labelMainKey.AutoSize = true;
            this.labelMainKey.Location = new System.Drawing.Point(12, 38);
            this.labelMainKey.Name = "labelMainKey";
            this.labelMainKey.Size = new System.Drawing.Size(33, 13);
            this.labelMainKey.TabIndex = 1;
            this.labelMainKey.Text = "Main:";
            // 
            // chkAlt1
            // 
            this.chkAlt1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAlt1.AutoSize = true;
            this.chkAlt1.Location = new System.Drawing.Point(378, 37);
            this.chkAlt1.Name = "chkAlt1";
            this.chkAlt1.Size = new System.Drawing.Size(38, 17);
            this.chkAlt1.TabIndex = 5;
            this.chkAlt1.Text = "Alt";
            this.chkAlt1.UseVisualStyleBackColor = true;
            // 
            // KeyboardShortcutEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.lvCommands);
            this.Controls.Add(this.panelKeyEditor);
            this.Name = "KeyboardShortcutEditor";
            this.Size = new System.Drawing.Size(467, 372);
            this.panelKeyEditor.ResumeLayout(false);
            this.panelKeyEditor.PerformLayout();
            this.ResumeLayout(false);

		}

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
    }
}
