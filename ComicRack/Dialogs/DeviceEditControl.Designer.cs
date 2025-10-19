using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class DeviceEditControl : UserControlEx
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components.SafeDispose();
                if (library != null)
                {
                    library.ComicListCachesUpdated -= Database_ComicListCachesUpdated;
                    library.ComicListsChanged -= library_ComicListsChanged;
                }
                library = null;
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.ilShares = new System.Windows.Forms.ImageList(this.components);
            this.chkOptimizeSize = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btSelectAll = new System.Windows.Forms.Button();
            this.btSelectNone = new System.Windows.Forms.Button();
            this.chkOnlyUnread = new System.Windows.Forms.CheckBox();
            this.grpListOptions = new System.Windows.Forms.GroupBox();
            this.chkSortBy = new System.Windows.Forms.CheckBox();
            this.chkKeepLastRead = new System.Windows.Forms.CheckBox();
            this.cbLimitSort = new System.Windows.Forms.ComboBox();
            this.txLimit = new System.Windows.Forms.TextBox();
            this.chkLimit = new System.Windows.Forms.CheckBox();
            this.cbLimitType = new System.Windows.Forms.ComboBox();
            this.chkOnlyChecked = new System.Windows.Forms.CheckBox();
            this.tvSharedLists = new cYo.Common.Windows.Forms.TreeViewEx();
            this.btSelectList = new System.Windows.Forms.Button();
            this.btDeselectList = new System.Windows.Forms.Button();
            this.chkOnlyShowSelected = new cYo.Common.Windows.Forms.WrappingCheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpListOptions.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ilShares
            // 
            this.ilShares.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilShares.ImageSize = new System.Drawing.Size(16, 16);
            this.ilShares.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // chkOptimizeSize
            // 
            this.chkOptimizeSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkOptimizeSize.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkOptimizeSize.Location = new System.Drawing.Point(398, 17);
            this.chkOptimizeSize.Name = "chkOptimizeSize";
            this.chkOptimizeSize.Size = new System.Drawing.Size(112, 20);
            this.chkOptimizeSize.TabIndex = 8;
            this.chkOptimizeSize.Text = "Optimize Size";
            this.chkOptimizeSize.UseVisualStyleBackColor = true;
            this.chkOptimizeSize.CheckedChanged += new System.EventHandler(this.chkOptimizeSize_CheckedChanged);
            // 
            // btSelectAll
            // 
            this.btSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btSelectAll.Location = new System.Drawing.Point(3, 3);
            this.btSelectAll.Name = "btSelectAll";
            this.btSelectAll.Size = new System.Drawing.Size(113, 24);
            this.btSelectAll.TabIndex = 1;
            this.btSelectAll.Text = "Select All";
            this.btSelectAll.Click += new System.EventHandler(this.btSelectAll_Click);
            // 
            // btSelectNone
            // 
            this.btSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelectNone.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btSelectNone.Location = new System.Drawing.Point(3, 33);
            this.btSelectNone.Name = "btSelectNone";
            this.btSelectNone.Size = new System.Drawing.Size(113, 24);
            this.btSelectNone.TabIndex = 2;
            this.btSelectNone.Text = "Select None";
            this.btSelectNone.Click += new System.EventHandler(this.btSelectNone_Click);
            // 
            // chkOnlyUnread
            // 
            this.chkOnlyUnread.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkOnlyUnread.Location = new System.Drawing.Point(15, 68);
            this.chkOnlyUnread.Name = "chkOnlyUnread";
            this.chkOnlyUnread.Size = new System.Drawing.Size(102, 17);
            this.chkOnlyUnread.TabIndex = 5;
            this.chkOnlyUnread.Text = "Only Unread";
            this.chkOnlyUnread.UseVisualStyleBackColor = true;
            this.chkOnlyUnread.CheckedChanged += new System.EventHandler(this.chkOnlyUnread_CheckedChanged);
            // 
            // grpListOptions
            // 
            this.grpListOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpListOptions.Controls.Add(this.chkSortBy);
            this.grpListOptions.Controls.Add(this.chkKeepLastRead);
            this.grpListOptions.Controls.Add(this.cbLimitSort);
            this.grpListOptions.Controls.Add(this.txLimit);
            this.grpListOptions.Controls.Add(this.chkLimit);
            this.grpListOptions.Controls.Add(this.cbLimitType);
            this.grpListOptions.Controls.Add(this.chkOnlyChecked);
            this.grpListOptions.Controls.Add(this.chkOptimizeSize);
            this.grpListOptions.Controls.Add(this.chkOnlyUnread);
            this.grpListOptions.Enabled = false;
            this.grpListOptions.Location = new System.Drawing.Point(0, 301);
            this.grpListOptions.Name = "grpListOptions";
            this.grpListOptions.Size = new System.Drawing.Size(516, 122);
            this.grpListOptions.TabIndex = 6;
            this.grpListOptions.TabStop = false;
            // 
            // chkSortBy
            // 
            this.chkSortBy.Location = new System.Drawing.Point(15, 19);
            this.chkSortBy.Name = "chkSortBy";
            this.chkSortBy.Size = new System.Drawing.Size(102, 18);
            this.chkSortBy.TabIndex = 0;
            this.chkSortBy.Text = "Sort by";
            this.chkSortBy.UseVisualStyleBackColor = true;
            this.chkSortBy.CheckedChanged += new System.EventHandler(this.chkSortBy_CheckedChanged);
            // 
            // chkKeepLastRead
            // 
            this.chkKeepLastRead.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkKeepLastRead.Location = new System.Drawing.Point(126, 68);
            this.chkKeepLastRead.Name = "chkKeepLastRead";
            this.chkKeepLastRead.Size = new System.Drawing.Size(144, 17);
            this.chkKeepLastRead.TabIndex = 6;
            this.chkKeepLastRead.Text = "But keep last read";
            this.chkKeepLastRead.UseVisualStyleBackColor = true;
            this.chkKeepLastRead.CheckedChanged += new System.EventHandler(this.chkKeepLastRead_CheckedChanged);
            // 
            // cbLimitSort
            // 
            this.cbLimitSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLimitSort.Enabled = false;
            this.cbLimitSort.FormattingEnabled = true;
            this.cbLimitSort.Items.AddRange(new object[] {
            "Random",
            "Series",
            "Alternate Series",
            "Published",
            "Added",
            "Story Arc",
            "List Order"});
            this.cbLimitSort.Location = new System.Drawing.Point(126, 18);
            this.cbLimitSort.Name = "cbLimitSort";
            this.cbLimitSort.Size = new System.Drawing.Size(144, 21);
            this.cbLimitSort.TabIndex = 1;
            this.cbLimitSort.SelectedIndexChanged += new System.EventHandler(this.cbLimitSort_SelectedIndexChanged);
            // 
            // txLimit
            // 
            this.txLimit.Enabled = false;
            this.txLimit.Location = new System.Drawing.Point(126, 45);
            this.txLimit.Name = "txLimit";
            this.txLimit.Size = new System.Drawing.Size(61, 20);
            this.txLimit.TabIndex = 3;
            this.txLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txLimit.TextChanged += new System.EventHandler(this.txLimit_TextChanged);
            // 
            // chkLimit
            // 
            this.chkLimit.Location = new System.Drawing.Point(15, 44);
            this.chkLimit.Name = "chkLimit";
            this.chkLimit.Size = new System.Drawing.Size(102, 18);
            this.chkLimit.TabIndex = 2;
            this.chkLimit.Text = "Limit to ";
            this.chkLimit.UseVisualStyleBackColor = true;
            this.chkLimit.CheckedChanged += new System.EventHandler(this.chkLimit_CheckedChanged);
            // 
            // cbLimitType
            // 
            this.cbLimitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLimitType.Enabled = false;
            this.cbLimitType.FormattingEnabled = true;
            this.cbLimitType.Items.AddRange(new object[] {
            "Books",
            "MB",
            "GB"});
            this.cbLimitType.Location = new System.Drawing.Point(193, 45);
            this.cbLimitType.Name = "cbLimitType";
            this.cbLimitType.Size = new System.Drawing.Size(77, 21);
            this.cbLimitType.TabIndex = 4;
            this.cbLimitType.SelectedIndexChanged += new System.EventHandler(this.cbLimitType_SelectedIndexChanged);
            // 
            // chkOnlyChecked
            // 
            this.chkOnlyChecked.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkOnlyChecked.Location = new System.Drawing.Point(15, 91);
            this.chkOnlyChecked.Name = "chkOnlyChecked";
            this.chkOnlyChecked.Size = new System.Drawing.Size(102, 17);
            this.chkOnlyChecked.TabIndex = 7;
            this.chkOnlyChecked.Text = "Only Checked";
            this.chkOnlyChecked.UseVisualStyleBackColor = true;
            this.chkOnlyChecked.CheckedChanged += new System.EventHandler(this.chkOnlyChecked_CheckedChanged);
            // 
            // tvSharedLists
            // 
            this.tvSharedLists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvSharedLists.CheckBoxes = true;
            this.tvSharedLists.FullRowSelect = true;
            this.tvSharedLists.HideSelection = false;
            this.tvSharedLists.ImageIndex = 0;
            this.tvSharedLists.ImageList = this.ilShares;
            this.tvSharedLists.ItemHeight = 16;
            this.tvSharedLists.Location = new System.Drawing.Point(0, 3);
            this.tvSharedLists.Name = "tvSharedLists";
            this.tvSharedLists.SelectedImageIndex = 0;
            this.tvSharedLists.ShowLines = false;
            this.tvSharedLists.ShowPlusMinus = false;
            this.tvSharedLists.ShowRootLines = false;
            this.tvSharedLists.Size = new System.Drawing.Size(391, 292);
            this.tvSharedLists.TabIndex = 0;
            this.tvSharedLists.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvSharedLists_AfterCheck);
            this.tvSharedLists.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSharedLists_AfterSelect);
            // 
            // btSelectList
            // 
            this.btSelectList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelectList.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btSelectList.Location = new System.Drawing.Point(3, 72);
            this.btSelectList.Name = "btSelectList";
            this.btSelectList.Size = new System.Drawing.Size(113, 24);
            this.btSelectList.TabIndex = 3;
            this.btSelectList.Text = "Select List";
            this.btSelectList.Click += new System.EventHandler(this.btSelectList_Click);
            // 
            // btDeselectList
            // 
            this.btDeselectList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btDeselectList.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btDeselectList.Location = new System.Drawing.Point(3, 102);
            this.btDeselectList.Name = "btDeselectList";
            this.btDeselectList.Size = new System.Drawing.Size(113, 24);
            this.btDeselectList.TabIndex = 4;
            this.btDeselectList.Text = "Deselect List";
            this.btDeselectList.Click += new System.EventHandler(this.btUnselectList_Click);
            // 
            // chkOnlyShowSelected
            // 
            this.chkOnlyShowSelected.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkOnlyShowSelected.AutoSize = true;
            this.chkOnlyShowSelected.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkOnlyShowSelected.Location = new System.Drawing.Point(0, 269);
            this.chkOnlyShowSelected.Name = "chkOnlyShowSelected";
            this.chkOnlyShowSelected.Size = new System.Drawing.Size(119, 23);
            this.chkOnlyShowSelected.TabIndex = 7;
            this.chkOnlyShowSelected.Text = "Only show selected";
            this.chkOnlyShowSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkOnlyShowSelected.UseVisualStyleBackColor = true;
            this.chkOnlyShowSelected.CheckedChanged += new System.EventHandler(this.chkOnlyShowSelected_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btSelectAll);
            this.panel1.Controls.Add(this.chkOnlyShowSelected);
            this.panel1.Controls.Add(this.btSelectNone);
            this.panel1.Controls.Add(this.btDeselectList);
            this.panel1.Controls.Add(this.btSelectList);
            this.panel1.Location = new System.Drawing.Point(397, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(119, 292);
            this.panel1.TabIndex = 8;
            // 
            // DeviceEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = ThemeColors.DeviceEditControl.Back;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpListOptions);
            this.Controls.Add(this.tvSharedLists);
            this.MinimumSize = new System.Drawing.Size(410, 320);
            this.Name = "DeviceEditControl";
            this.Size = new System.Drawing.Size(519, 426);
            this.grpListOptions.ResumeLayout(false);
            this.grpListOptions.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}

        private TreeViewEx tvSharedLists;
        private CheckBox chkOptimizeSize;
        private ImageList ilShares;
        private ToolTip toolTip;
        private Button btSelectAll;
        private Button btSelectNone;
        private CheckBox chkOnlyUnread;
        private GroupBox grpListOptions;
        private CheckBox chkOnlyChecked;
        private Button btSelectList;
        private Button btDeselectList;
        private ComboBox cbLimitSort;
        private TextBox txLimit;
        private CheckBox chkLimit;
        private ComboBox cbLimitType;
        private CheckBox chkKeepLastRead;
        private CheckBox chkSortBy;
        private WrappingCheckBox chkOnlyShowSelected;
        private Panel panel1;
    }
}
