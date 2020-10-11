using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ListLayoutDialog
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				IdleProcess.Idle -= Application_Idle;
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			btOK = new System.Windows.Forms.Button();
			btCancel = new System.Windows.Forms.Button();
			tabDetails = new System.Windows.Forms.TabPage();
			lvColumns = new cYo.Common.Windows.Forms.ListViewEx();
			chName = new System.Windows.Forms.ColumnHeader();
			chDescription = new System.Windows.Forms.ColumnHeader();
			labelSelectColumn = new System.Windows.Forms.Label();
			btHideAll = new System.Windows.Forms.Button();
			btShowAll = new System.Windows.Forms.Button();
			labelHint = new System.Windows.Forms.Label();
			btMoveDown = new cYo.Common.Windows.Forms.AutoRepeatButton();
			btMoveUp = new cYo.Common.Windows.Forms.AutoRepeatButton();
			tab = new System.Windows.Forms.TabControl();
			tabThumbnails = new System.Windows.Forms.TabPage();
			chkHideCaption = new System.Windows.Forms.CheckBox();
			labelSelectThumbnailText = new System.Windows.Forms.Label();
			cbThirdLine = new System.Windows.Forms.ComboBox();
			labelThirdLine = new System.Windows.Forms.Label();
			cbSecondLine = new System.Windows.Forms.ComboBox();
			labelSecondLine = new System.Windows.Forms.Label();
			cbFirstLine = new System.Windows.Forms.ComboBox();
			labelFirstLine = new System.Windows.Forms.Label();
			tabTiles = new System.Windows.Forms.TabPage();
			btTilesDefault = new cYo.Common.Windows.Forms.AutoRepeatButton();
			lbTileItems = new System.Windows.Forms.CheckedListBox();
			btApply = new System.Windows.Forms.Button();
			tabDetails.SuspendLayout();
			tab.SuspendLayout();
			tabThumbnails.SuspendLayout();
			tabTiles.SuspendLayout();
			SuspendLayout();
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(252, 401);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 1;
			btOK.Text = "&OK";
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(338, 401);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 2;
			btCancel.Text = "&Cancel";
			tabDetails.Controls.Add(lvColumns);
			tabDetails.Controls.Add(labelSelectColumn);
			tabDetails.Controls.Add(btHideAll);
			tabDetails.Controls.Add(btShowAll);
			tabDetails.Controls.Add(labelHint);
			tabDetails.Controls.Add(btMoveDown);
			tabDetails.Controls.Add(btMoveUp);
			tabDetails.Location = new System.Drawing.Point(4, 22);
			tabDetails.Name = "tabDetails";
			tabDetails.Padding = new System.Windows.Forms.Padding(3);
			tabDetails.Size = new System.Drawing.Size(484, 357);
			tabDetails.TabIndex = 0;
			tabDetails.Text = "Details";
			tabDetails.UseVisualStyleBackColor = true;
			lvColumns.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lvColumns.CheckBoxes = true;
			lvColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2]
			{
				chName,
				chDescription
			});
			lvColumns.EnableMouseReorder = true;
			lvColumns.FullRowSelect = true;
			lvColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			lvColumns.HideSelection = false;
			lvColumns.Location = new System.Drawing.Point(12, 40);
			lvColumns.MultiSelect = false;
			lvColumns.Name = "lvColumns";
			lvColumns.Size = new System.Drawing.Size(378, 291);
			lvColumns.TabIndex = 7;
			lvColumns.UseCompatibleStateImageBehavior = false;
			lvColumns.View = System.Windows.Forms.View.Details;
			chName.Text = "Name";
			chName.Width = 127;
			chDescription.Text = "Description";
			chDescription.Width = 221;
			labelSelectColumn.AutoSize = true;
			labelSelectColumn.Location = new System.Drawing.Point(6, 21);
			labelSelectColumn.Name = "labelSelectColumn";
			labelSelectColumn.Size = new System.Drawing.Size(219, 13);
			labelSelectColumn.TabIndex = 0;
			labelSelectColumn.Text = "Please select the columns you want to show:";
			btHideAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btHideAll.Location = new System.Drawing.Point(398, 140);
			btHideAll.Name = "btHideAll";
			btHideAll.Size = new System.Drawing.Size(80, 24);
			btHideAll.TabIndex = 5;
			btHideAll.Text = "&Hide All";
			btHideAll.UseVisualStyleBackColor = true;
			btHideAll.Click += new System.EventHandler(btHideAll_Click);
			btShowAll.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btShowAll.Location = new System.Drawing.Point(398, 110);
			btShowAll.Name = "btShowAll";
			btShowAll.Size = new System.Drawing.Size(80, 24);
			btShowAll.TabIndex = 4;
			btShowAll.Text = "&Show All";
			btShowAll.UseVisualStyleBackColor = true;
			btShowAll.Click += new System.EventHandler(btShowAll_Click);
			labelHint.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			labelHint.Location = new System.Drawing.Point(6, 334);
			labelHint.Name = "labelHint";
			labelHint.Size = new System.Drawing.Size(469, 20);
			labelHint.TabIndex = 6;
			labelHint.Text = "Hint: You can also change the displayed columns by right clicking on a column header.";
			labelHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			btMoveDown.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btMoveDown.Location = new System.Drawing.Point(398, 66);
			btMoveDown.Name = "btMoveDown";
			btMoveDown.Size = new System.Drawing.Size(80, 23);
			btMoveDown.TabIndex = 3;
			btMoveDown.Text = "Move &Down";
			btMoveDown.UseVisualStyleBackColor = true;
			btMoveDown.Click += new System.EventHandler(btMoveDown_Click);
			btMoveUp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btMoveUp.Location = new System.Drawing.Point(398, 37);
			btMoveUp.Name = "btMoveUp";
			btMoveUp.Size = new System.Drawing.Size(80, 23);
			btMoveUp.TabIndex = 2;
			btMoveUp.Text = "Move &Up";
			btMoveUp.UseVisualStyleBackColor = true;
			btMoveUp.Click += new System.EventHandler(btMoveUp_Click);
			tab.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tab.Controls.Add(tabDetails);
			tab.Controls.Add(tabThumbnails);
			tab.Controls.Add(tabTiles);
			tab.Location = new System.Drawing.Point(12, 12);
			tab.Name = "tab";
			tab.SelectedIndex = 0;
			tab.Size = new System.Drawing.Size(492, 383);
			tab.TabIndex = 0;
			tabThumbnails.Controls.Add(chkHideCaption);
			tabThumbnails.Controls.Add(labelSelectThumbnailText);
			tabThumbnails.Controls.Add(cbThirdLine);
			tabThumbnails.Controls.Add(labelThirdLine);
			tabThumbnails.Controls.Add(cbSecondLine);
			tabThumbnails.Controls.Add(labelSecondLine);
			tabThumbnails.Controls.Add(cbFirstLine);
			tabThumbnails.Controls.Add(labelFirstLine);
			tabThumbnails.Location = new System.Drawing.Point(4, 22);
			tabThumbnails.Name = "tabThumbnails";
			tabThumbnails.Size = new System.Drawing.Size(484, 357);
			tabThumbnails.TabIndex = 1;
			tabThumbnails.Text = "Thumbnails";
			tabThumbnails.UseVisualStyleBackColor = true;
			chkHideCaption.AutoSize = true;
			chkHideCaption.Location = new System.Drawing.Point(186, 165);
			chkHideCaption.Name = "chkHideCaption";
			chkHideCaption.Size = new System.Drawing.Size(130, 17);
			chkHideCaption.TabIndex = 7;
			chkHideCaption.Text = "Do not show any Text";
			chkHideCaption.UseVisualStyleBackColor = true;
			labelSelectThumbnailText.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			labelSelectThumbnailText.Location = new System.Drawing.Point(17, 20);
			labelSelectThumbnailText.Name = "labelSelectThumbnailText";
			labelSelectThumbnailText.Size = new System.Drawing.Size(445, 26);
			labelSelectThumbnailText.TabIndex = 6;
			labelSelectThumbnailText.Text = "Please select the text you want to display below the thumbnails:";
			cbThirdLine.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbThirdLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbThirdLine.FormattingEnabled = true;
			cbThirdLine.Location = new System.Drawing.Point(91, 114);
			cbThirdLine.Name = "cbThirdLine";
			cbThirdLine.Size = new System.Drawing.Size(371, 21);
			cbThirdLine.Sorted = true;
			cbThirdLine.TabIndex = 5;
			labelThirdLine.AutoSize = true;
			labelThirdLine.Location = new System.Drawing.Point(15, 117);
			labelThirdLine.Name = "labelThirdLine";
			labelThirdLine.Size = new System.Drawing.Size(57, 13);
			labelThirdLine.TabIndex = 4;
			labelThirdLine.Text = "Third Line:";
			cbSecondLine.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbSecondLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbSecondLine.FormattingEnabled = true;
			cbSecondLine.Location = new System.Drawing.Point(91, 87);
			cbSecondLine.Name = "cbSecondLine";
			cbSecondLine.Size = new System.Drawing.Size(371, 21);
			cbSecondLine.Sorted = true;
			cbSecondLine.TabIndex = 3;
			labelSecondLine.AutoSize = true;
			labelSecondLine.Location = new System.Drawing.Point(15, 90);
			labelSecondLine.Name = "labelSecondLine";
			labelSecondLine.Size = new System.Drawing.Size(70, 13);
			labelSecondLine.TabIndex = 2;
			labelSecondLine.Text = "Second Line:";
			cbFirstLine.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbFirstLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbFirstLine.FormattingEnabled = true;
			cbFirstLine.Location = new System.Drawing.Point(91, 60);
			cbFirstLine.Name = "cbFirstLine";
			cbFirstLine.Size = new System.Drawing.Size(371, 21);
			cbFirstLine.Sorted = true;
			cbFirstLine.TabIndex = 1;
			labelFirstLine.AutoSize = true;
			labelFirstLine.Location = new System.Drawing.Point(15, 63);
			labelFirstLine.Name = "labelFirstLine";
			labelFirstLine.Size = new System.Drawing.Size(52, 13);
			labelFirstLine.TabIndex = 0;
			labelFirstLine.Text = "First Line:";
			tabTiles.Controls.Add(btTilesDefault);
			tabTiles.Controls.Add(lbTileItems);
			tabTiles.Location = new System.Drawing.Point(4, 22);
			tabTiles.Name = "tabTiles";
			tabTiles.Padding = new System.Windows.Forms.Padding(3);
			tabTiles.Size = new System.Drawing.Size(484, 357);
			tabTiles.TabIndex = 2;
			tabTiles.Text = "Tiles";
			tabTiles.UseVisualStyleBackColor = true;
			btTilesDefault.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btTilesDefault.Location = new System.Drawing.Point(397, 13);
			btTilesDefault.Name = "btTilesDefault";
			btTilesDefault.Size = new System.Drawing.Size(80, 23);
			btTilesDefault.TabIndex = 3;
			btTilesDefault.Text = "Default";
			btTilesDefault.UseVisualStyleBackColor = true;
			btTilesDefault.Click += new System.EventHandler(btDefaultTile_Click);
			lbTileItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lbTileItems.CheckOnClick = true;
			lbTileItems.FormattingEnabled = true;
			lbTileItems.IntegralHeight = false;
			lbTileItems.Location = new System.Drawing.Point(9, 13);
			lbTileItems.Name = "lbTileItems";
			lbTileItems.Size = new System.Drawing.Size(382, 334);
			lbTileItems.TabIndex = 0;
			btApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btApply.Location = new System.Drawing.Point(424, 401);
			btApply.Name = "btApply";
			btApply.Size = new System.Drawing.Size(80, 24);
			btApply.TabIndex = 3;
			btApply.Text = "&Apply";
			btApply.Click += new System.EventHandler(btApply_Click);
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(516, 432);
			base.Controls.Add(btApply);
			base.Controls.Add(tab);
			base.Controls.Add(btOK);
			base.Controls.Add(btCancel);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(355, 361);
			base.Name = "ListLayoutDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "List Options";
			tabDetails.ResumeLayout(false);
			tabDetails.PerformLayout();
			tab.ResumeLayout(false);
			tabThumbnails.ResumeLayout(false);
			tabThumbnails.PerformLayout();
			tabTiles.ResumeLayout(false);
			ResumeLayout(false);
		}
		
		private IContainer components;

		private Button btOK;

		private Button btCancel;

		private TabPage tabDetails;

		private Label labelSelectColumn;

		private Button btHideAll;

		private Button btShowAll;

		private Label labelHint;

		private AutoRepeatButton btMoveDown;

		private AutoRepeatButton btMoveUp;

		private TabControl tab;

		private TabPage tabThumbnails;

		private ComboBox cbSecondLine;

		private Label labelSecondLine;

		private ComboBox cbFirstLine;

		private Label labelFirstLine;

		private CheckBox chkHideCaption;

		private Label labelSelectThumbnailText;

		private ComboBox cbThirdLine;

		private Label labelThirdLine;

		private ListViewEx lvColumns;

		private ColumnHeader chName;

		private ColumnHeader chDescription;

		private TabPage tabTiles;

		private CheckedListBox lbTileItems;

		private AutoRepeatButton btTilesDefault;

		private Button btApply;
	}
}
