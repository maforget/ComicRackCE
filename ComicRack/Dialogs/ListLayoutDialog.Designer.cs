using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ListLayoutDialog
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.tabDetails = new System.Windows.Forms.TabPage();
            this.lvColumns = new cYo.Common.Windows.Forms.ListViewEx();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelSelectColumn = new System.Windows.Forms.Label();
            this.btHideAll = new System.Windows.Forms.Button();
            this.btShowAll = new System.Windows.Forms.Button();
            this.labelHint = new System.Windows.Forms.Label();
            this.btMoveDown = new cYo.Common.Windows.Forms.AutoRepeatButton();
            this.btMoveUp = new cYo.Common.Windows.Forms.AutoRepeatButton();
            this.tab = new System.Windows.Forms.TabControl();
            this.tabThumbnails = new System.Windows.Forms.TabPage();
            this.chkHideCaption = new System.Windows.Forms.CheckBox();
            this.labelSelectThumbnailText = new System.Windows.Forms.Label();
            this.cbThirdLine = new System.Windows.Forms.ComboBox();
            this.labelThirdLine = new System.Windows.Forms.Label();
            this.cbSecondLine = new System.Windows.Forms.ComboBox();
            this.labelSecondLine = new System.Windows.Forms.Label();
            this.cbFirstLine = new System.Windows.Forms.ComboBox();
            this.labelFirstLine = new System.Windows.Forms.Label();
            this.tabTiles = new System.Windows.Forms.TabPage();
            this.btTilesDefault = new cYo.Common.Windows.Forms.AutoRepeatButton();
            this.lbTileItems = new System.Windows.Forms.CheckedListBox();
            this.btApply = new System.Windows.Forms.Button();
            this.tabDetails.SuspendLayout();
            this.tab.SuspendLayout();
            this.tabThumbnails.SuspendLayout();
            this.tabTiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(252, 401);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "&OK";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(338, 401);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "&Cancel";
            // 
            // tabDetails
            // 
            this.tabDetails.Controls.Add(this.lvColumns);
            this.tabDetails.Controls.Add(this.labelSelectColumn);
            this.tabDetails.Controls.Add(this.btHideAll);
            this.tabDetails.Controls.Add(this.btShowAll);
            this.tabDetails.Controls.Add(this.labelHint);
            this.tabDetails.Controls.Add(this.btMoveDown);
            this.tabDetails.Controls.Add(this.btMoveUp);
            this.tabDetails.Location = new System.Drawing.Point(4, 22);
            this.tabDetails.Name = "tabDetails";
            this.tabDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tabDetails.Size = new System.Drawing.Size(484, 357);
            this.tabDetails.TabIndex = 0;
            this.tabDetails.Text = "Details";
            this.tabDetails.UseVisualStyleBackColor = true;
            // 
            // lvColumns
            // 
            this.lvColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvColumns.CheckBoxes = true;
            this.lvColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chDescription});
            this.lvColumns.EnableMouseReorder = true;
            this.lvColumns.FullRowSelect = true;
            this.lvColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvColumns.HideSelection = false;
            this.lvColumns.Location = new System.Drawing.Point(12, 40);
            this.lvColumns.MultiSelect = false;
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(378, 291);
            this.lvColumns.TabIndex = 7;
            this.lvColumns.UseCompatibleStateImageBehavior = false;
            this.lvColumns.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 127;
            // 
            // chDescription
            // 
            this.chDescription.Text = "Description";
            this.chDescription.Width = 221;
            // 
            // labelSelectColumn
            // 
            this.labelSelectColumn.AutoSize = true;
            this.labelSelectColumn.Location = new System.Drawing.Point(6, 21);
            this.labelSelectColumn.Name = "labelSelectColumn";
            this.labelSelectColumn.Size = new System.Drawing.Size(219, 13);
            this.labelSelectColumn.TabIndex = 0;
            this.labelSelectColumn.Text = "Please select the columns you want to show:";
            // 
            // btHideAll
            // 
            this.btHideAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btHideAll.Location = new System.Drawing.Point(398, 140);
            this.btHideAll.Name = "btHideAll";
            this.btHideAll.Size = new System.Drawing.Size(80, 24);
            this.btHideAll.TabIndex = 5;
            this.btHideAll.Text = "&Hide All";
            this.btHideAll.UseVisualStyleBackColor = true;
            this.btHideAll.Click += new System.EventHandler(this.btHideAll_Click);
            // 
            // btShowAll
            // 
            this.btShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btShowAll.Location = new System.Drawing.Point(398, 110);
            this.btShowAll.Name = "btShowAll";
            this.btShowAll.Size = new System.Drawing.Size(80, 24);
            this.btShowAll.TabIndex = 4;
            this.btShowAll.Text = "&Show All";
            this.btShowAll.UseVisualStyleBackColor = true;
            this.btShowAll.Click += new System.EventHandler(this.btShowAll_Click);
            // 
            // labelHint
            // 
            this.labelHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelHint.Location = new System.Drawing.Point(6, 334);
            this.labelHint.Name = "labelHint";
            this.labelHint.Size = new System.Drawing.Size(469, 20);
            this.labelHint.TabIndex = 6;
            this.labelHint.Text = "Hint: You can also change the displayed columns by right clicking on a column hea" +
    "der.";
            this.labelHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btMoveDown
            // 
            this.btMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btMoveDown.Location = new System.Drawing.Point(398, 66);
            this.btMoveDown.Name = "btMoveDown";
            this.btMoveDown.Size = new System.Drawing.Size(80, 23);
            this.btMoveDown.TabIndex = 3;
            this.btMoveDown.Text = "Move &Down";
            this.btMoveDown.UseVisualStyleBackColor = true;
            this.btMoveDown.Click += new System.EventHandler(this.btMoveDown_Click);
            // 
            // btMoveUp
            // 
            this.btMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btMoveUp.Location = new System.Drawing.Point(398, 37);
            this.btMoveUp.Name = "btMoveUp";
            this.btMoveUp.Size = new System.Drawing.Size(80, 23);
            this.btMoveUp.TabIndex = 2;
            this.btMoveUp.Text = "Move &Up";
            this.btMoveUp.UseVisualStyleBackColor = true;
            this.btMoveUp.Click += new System.EventHandler(this.btMoveUp_Click);
            // 
            // tab
            // 
            this.tab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tab.Controls.Add(this.tabDetails);
            this.tab.Controls.Add(this.tabThumbnails);
            this.tab.Controls.Add(this.tabTiles);
            this.tab.Location = new System.Drawing.Point(12, 12);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(492, 383);
            this.tab.TabIndex = 0;
            // 
            // tabThumbnails
            // 
            this.tabThumbnails.Controls.Add(this.chkHideCaption);
            this.tabThumbnails.Controls.Add(this.labelSelectThumbnailText);
            this.tabThumbnails.Controls.Add(this.cbThirdLine);
            this.tabThumbnails.Controls.Add(this.labelThirdLine);
            this.tabThumbnails.Controls.Add(this.cbSecondLine);
            this.tabThumbnails.Controls.Add(this.labelSecondLine);
            this.tabThumbnails.Controls.Add(this.cbFirstLine);
            this.tabThumbnails.Controls.Add(this.labelFirstLine);
            this.tabThumbnails.Location = new System.Drawing.Point(4, 22);
            this.tabThumbnails.Name = "tabThumbnails";
            this.tabThumbnails.Size = new System.Drawing.Size(484, 357);
            this.tabThumbnails.TabIndex = 1;
            this.tabThumbnails.Text = "Thumbnails";
            this.tabThumbnails.UseVisualStyleBackColor = true;
            // 
            // chkHideCaption
            // 
            this.chkHideCaption.AutoSize = true;
            this.chkHideCaption.Location = new System.Drawing.Point(186, 165);
            this.chkHideCaption.Name = "chkHideCaption";
            this.chkHideCaption.Size = new System.Drawing.Size(130, 17);
            this.chkHideCaption.TabIndex = 7;
            this.chkHideCaption.Text = "Do not show any Text";
            this.chkHideCaption.UseVisualStyleBackColor = true;
            // 
            // labelSelectThumbnailText
            // 
            this.labelSelectThumbnailText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSelectThumbnailText.Location = new System.Drawing.Point(17, 20);
            this.labelSelectThumbnailText.Name = "labelSelectThumbnailText";
            this.labelSelectThumbnailText.Size = new System.Drawing.Size(445, 26);
            this.labelSelectThumbnailText.TabIndex = 6;
            this.labelSelectThumbnailText.Text = "Please select the text you want to display below the thumbnails:";
            // 
            // cbThirdLine
            // 
            this.cbThirdLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbThirdLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbThirdLine.FormattingEnabled = true;
            this.cbThirdLine.Location = new System.Drawing.Point(91, 114);
            this.cbThirdLine.Name = "cbThirdLine";
            this.cbThirdLine.Size = new System.Drawing.Size(371, 21);
            this.cbThirdLine.Sorted = true;
            this.cbThirdLine.TabIndex = 5;
            // 
            // labelThirdLine
            // 
            this.labelThirdLine.AutoSize = true;
            this.labelThirdLine.Location = new System.Drawing.Point(15, 117);
            this.labelThirdLine.Name = "labelThirdLine";
            this.labelThirdLine.Size = new System.Drawing.Size(57, 13);
            this.labelThirdLine.TabIndex = 4;
            this.labelThirdLine.Text = "Third Line:";
            // 
            // cbSecondLine
            // 
            this.cbSecondLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSecondLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSecondLine.FormattingEnabled = true;
            this.cbSecondLine.Location = new System.Drawing.Point(91, 87);
            this.cbSecondLine.Name = "cbSecondLine";
            this.cbSecondLine.Size = new System.Drawing.Size(371, 21);
            this.cbSecondLine.Sorted = true;
            this.cbSecondLine.TabIndex = 3;
            // 
            // labelSecondLine
            // 
            this.labelSecondLine.AutoSize = true;
            this.labelSecondLine.Location = new System.Drawing.Point(15, 90);
            this.labelSecondLine.Name = "labelSecondLine";
            this.labelSecondLine.Size = new System.Drawing.Size(70, 13);
            this.labelSecondLine.TabIndex = 2;
            this.labelSecondLine.Text = "Second Line:";
            // 
            // cbFirstLine
            // 
            this.cbFirstLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFirstLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFirstLine.FormattingEnabled = true;
            this.cbFirstLine.Location = new System.Drawing.Point(91, 60);
            this.cbFirstLine.Name = "cbFirstLine";
            this.cbFirstLine.Size = new System.Drawing.Size(371, 21);
            this.cbFirstLine.Sorted = true;
            this.cbFirstLine.TabIndex = 1;
            // 
            // labelFirstLine
            // 
            this.labelFirstLine.AutoSize = true;
            this.labelFirstLine.Location = new System.Drawing.Point(15, 63);
            this.labelFirstLine.Name = "labelFirstLine";
            this.labelFirstLine.Size = new System.Drawing.Size(52, 13);
            this.labelFirstLine.TabIndex = 0;
            this.labelFirstLine.Text = "First Line:";
            // 
            // tabTiles
            // 
            this.tabTiles.Controls.Add(this.btTilesDefault);
            this.tabTiles.Controls.Add(this.lbTileItems);
            this.tabTiles.Location = new System.Drawing.Point(4, 22);
            this.tabTiles.Name = "tabTiles";
            this.tabTiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabTiles.Size = new System.Drawing.Size(484, 357);
            this.tabTiles.TabIndex = 2;
            this.tabTiles.Text = "Tiles";
            this.tabTiles.UseVisualStyleBackColor = true;
            // 
            // btTilesDefault
            // 
            this.btTilesDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btTilesDefault.Location = new System.Drawing.Point(397, 13);
            this.btTilesDefault.Name = "btTilesDefault";
            this.btTilesDefault.Size = new System.Drawing.Size(80, 23);
            this.btTilesDefault.TabIndex = 3;
            this.btTilesDefault.Text = "Default";
            this.btTilesDefault.UseVisualStyleBackColor = true;
            this.btTilesDefault.Click += new System.EventHandler(this.btDefaultTile_Click);
            // 
            // lbTileItems
            // 
            this.lbTileItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTileItems.CheckOnClick = true;
            this.lbTileItems.FormattingEnabled = true;
            this.lbTileItems.IntegralHeight = false;
            this.lbTileItems.Location = new System.Drawing.Point(9, 13);
            this.lbTileItems.Name = "lbTileItems";
            this.lbTileItems.Size = new System.Drawing.Size(382, 334);
            this.lbTileItems.TabIndex = 0;
            // 
            // btApply
            // 
            this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btApply.Location = new System.Drawing.Point(424, 401);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(80, 24);
            this.btApply.TabIndex = 3;
            this.btApply.Text = "&Apply";
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // ListLayoutDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(516, 432);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.tab);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(355, 361);
            this.Name = "ListLayoutDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "List Options";
            this.tabDetails.ResumeLayout(false);
            this.tabDetails.PerformLayout();
            this.tab.ResumeLayout(false);
            this.tabThumbnails.ResumeLayout(false);
            this.tabThumbnails.PerformLayout();
            this.tabTiles.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		
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
