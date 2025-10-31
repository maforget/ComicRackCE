using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class TasksDialog
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				FormUtility.SafeToolStripClear(contextMenuAbort.Items);
				if (!tabs.TabPages.Contains(tabNetwork))
				{
					tabs.TabPages.Add(tabNetwork);
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.lvTasks = new cYo.Common.Windows.Forms.ListViewEx();
            this.colTask = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.taskImages = new System.Windows.Forms.ImageList(this.components);
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.lblItemCount = new System.Windows.Forms.Label();
            this.btAbort = new cYo.Common.Windows.Forms.SplitButton();
            this.contextMenuAbort = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dummyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabTasks = new System.Windows.Forms.TabPage();
            this.tabNetwork = new System.Windows.Forms.TabPage();
            this.tvStats = new System.Windows.Forms.TreeView();
            this.networkImages = new System.Windows.Forms.ImageList(this.components);
            this.btClose = new System.Windows.Forms.Button();
            this.contextMenuAbort.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabTasks.SuspendLayout();
            this.tabNetwork.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvTasks
            // 
            this.lvTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTask,
            this.colState});
            this.lvTasks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTasks.HideSelection = false;
            this.lvTasks.Location = new System.Drawing.Point(6, 6);
            this.lvTasks.Name = "lvTasks";
            this.lvTasks.OwnerDraw = true;
            this.lvTasks.Size = new System.Drawing.Size(588, 385);
            this.lvTasks.SmallImageList = this.taskImages;
            this.lvTasks.TabIndex = 0;
            this.lvTasks.UseCompatibleStateImageBehavior = false;
            this.lvTasks.View = System.Windows.Forms.View.Details;
            this.lvTasks.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvTasks_DrawColumnHeader);
            this.lvTasks.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lvTasks_DrawSubItem);
            // 
            // colTask
            // 
            this.colTask.Text = "Task";
            this.colTask.Width = 437;
            // 
            // colState
            // 
            this.colState.Text = "State";
            this.colState.Width = 87;
            // 
            // taskImages
            // 
            this.taskImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.taskImages.ImageSize = new System.Drawing.Size(16, 16);
            this.taskImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 1000;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // lblItemCount
            // 
            this.lblItemCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblItemCount.AutoSize = true;
            this.lblItemCount.Location = new System.Drawing.Point(6, 402);
            this.lblItemCount.Name = "lblItemCount";
            this.lblItemCount.Size = new System.Drawing.Size(112, 13);
            this.lblItemCount.TabIndex = 1;
            this.lblItemCount.Text = "{0} Tasks are pending";
            // 
            // btAbort
            // 
            this.btAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btAbort.AutoEllipsis = true;
            this.btAbort.ContextMenuStrip = this.contextMenuAbort;
            this.btAbort.Location = new System.Drawing.Point(413, 397);
            this.btAbort.Name = "btAbort";
            this.btAbort.Size = new System.Drawing.Size(181, 23);
            this.btAbort.TabIndex = 2;
            this.btAbort.Text = "Abort all User Tasks";
            this.btAbort.UseVisualStyleBackColor = true;
            this.btAbort.Click += new System.EventHandler(this.btAbort_Click);
            // 
            // contextMenuAbort
            // 
            this.contextMenuAbort.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyToolStripMenuItem});
            this.contextMenuAbort.Name = "contextMenuAbort";
            this.contextMenuAbort.Size = new System.Drawing.Size(118, 26);
            this.contextMenuAbort.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuAbort_Opening);
            // 
            // dummyToolStripMenuItem
            // 
            this.dummyToolStripMenuItem.Name = "dummyToolStripMenuItem";
            this.dummyToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.dummyToolStripMenuItem.Text = "Dummy";
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabTasks);
            this.tabs.Controls.Add(this.tabNetwork);
            this.tabs.Location = new System.Drawing.Point(12, 6);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(608, 452);
            this.tabs.TabIndex = 4;
            // 
            // tabTasks
            // 
            this.tabTasks.Controls.Add(this.lvTasks);
            this.tabTasks.Controls.Add(this.lblItemCount);
            this.tabTasks.Controls.Add(this.btAbort);
            this.tabTasks.Location = new System.Drawing.Point(4, 22);
            this.tabTasks.Name = "tabTasks";
            this.tabTasks.Padding = new System.Windows.Forms.Padding(3);
            this.tabTasks.Size = new System.Drawing.Size(600, 426);
            this.tabTasks.TabIndex = 0;
            this.tabTasks.Text = "Background Tasks";
            this.tabTasks.UseVisualStyleBackColor = true;
            // 
            // tabNetwork
            // 
            this.tabNetwork.Controls.Add(this.tvStats);
            this.tabNetwork.Location = new System.Drawing.Point(4, 22);
            this.tabNetwork.Name = "tabNetwork";
            this.tabNetwork.Padding = new System.Windows.Forms.Padding(3);
            this.tabNetwork.Size = new System.Drawing.Size(600, 426);
            this.tabNetwork.TabIndex = 1;
            this.tabNetwork.Text = "Server Statistics";
            this.tabNetwork.UseVisualStyleBackColor = true;
            // 
            // tvStats
            // 
            this.tvStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvStats.ImageIndex = 0;
            this.tvStats.ImageList = this.networkImages;
            this.tvStats.Location = new System.Drawing.Point(3, 3);
            this.tvStats.Name = "tvStats";
            this.tvStats.SelectedImageIndex = 0;
            this.tvStats.ShowLines = false;
            this.tvStats.ShowRootLines = false;
            this.tvStats.Size = new System.Drawing.Size(594, 420);
            this.tvStats.TabIndex = 1;
            // 
            // networkImages
            // 
            this.networkImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.networkImages.ImageSize = new System.Drawing.Size(16, 16);
            this.networkImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btClose.Location = new System.Drawing.Point(524, 464);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(96, 24);
            this.btClose.TabIndex = 5;
            this.btClose.Text = "&Close";
            // 
            // TasksDialog
            // 
            this.AcceptButton = this.btClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btClose;
            this.ClientSize = new System.Drawing.Size(632, 499);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.tabs);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "TasksDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tasks";
            this.contextMenuAbort.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabTasks.ResumeLayout(false);
            this.tabTasks.PerformLayout();
            this.tabNetwork.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		
		private ListViewEx lvTasks;
		private ColumnHeader colTask;
		private Timer updateTimer;
		private Label lblItemCount;
		private ImageList taskImages;
		private SplitButton btAbort;
		private ContextMenuStrip contextMenuAbort;
		private ToolStripMenuItem dummyToolStripMenuItem;
		private ColumnHeader colState;
		private TabControl tabs;
		private TabPage tabTasks;
		private TabPage tabNetwork;
		private ImageList networkImages;
		private TreeView tvStats;
		private Button btClose;
	}
}
