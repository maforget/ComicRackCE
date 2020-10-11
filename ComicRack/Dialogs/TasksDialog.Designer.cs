using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class TasksDialog
	{
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
			components = new System.ComponentModel.Container();
			lvTasks = new cYo.Common.Windows.Forms.ListViewEx();
			colTask = new System.Windows.Forms.ColumnHeader();
			colState = new System.Windows.Forms.ColumnHeader();
			taskImages = new System.Windows.Forms.ImageList(components);
			updateTimer = new System.Windows.Forms.Timer(components);
			lblItemCount = new System.Windows.Forms.Label();
			btAbort = new cYo.Common.Windows.Forms.SplitButton();
			contextMenuAbort = new System.Windows.Forms.ContextMenuStrip(components);
			dummyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			tabs = new System.Windows.Forms.TabControl();
			tabTasks = new System.Windows.Forms.TabPage();
			tabNetwork = new System.Windows.Forms.TabPage();
			tvStats = new System.Windows.Forms.TreeView();
			networkImages = new System.Windows.Forms.ImageList(components);
			btClose = new System.Windows.Forms.Button();
			contextMenuAbort.SuspendLayout();
			tabs.SuspendLayout();
			tabTasks.SuspendLayout();
			tabNetwork.SuspendLayout();
			SuspendLayout();
			lvTasks.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lvTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2]
			{
				colTask,
				colState
			});
			lvTasks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			lvTasks.Location = new System.Drawing.Point(6, 6);
			lvTasks.Name = "lvTasks";
			lvTasks.OwnerDraw = true;
			lvTasks.Size = new System.Drawing.Size(588, 385);
			lvTasks.SmallImageList = taskImages;
			lvTasks.TabIndex = 0;
			lvTasks.UseCompatibleStateImageBehavior = false;
			lvTasks.View = System.Windows.Forms.View.Details;
			lvTasks.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(lvTasks_DrawColumnHeader);
			lvTasks.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(lvTasks_DrawSubItem);
			colTask.Text = "Task";
			colTask.Width = 437;
			colState.Text = "State";
			colState.Width = 87;
			taskImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			taskImages.ImageSize = new System.Drawing.Size(16, 16);
			taskImages.TransparentColor = System.Drawing.Color.Transparent;
			updateTimer.Enabled = true;
			updateTimer.Interval = 1000;
			updateTimer.Tick += new System.EventHandler(updateTimer_Tick);
			lblItemCount.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblItemCount.AutoSize = true;
			lblItemCount.Location = new System.Drawing.Point(6, 402);
			lblItemCount.Name = "lblItemCount";
			lblItemCount.Size = new System.Drawing.Size(112, 13);
			lblItemCount.TabIndex = 1;
			lblItemCount.Text = "{0} Tasks are pending";
			btAbort.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btAbort.AutoEllipsis = true;
			btAbort.ContextMenuStrip = contextMenuAbort;
			btAbort.Location = new System.Drawing.Point(413, 397);
			btAbort.Name = "btAbort";
			btAbort.Size = new System.Drawing.Size(181, 23);
			btAbort.TabIndex = 2;
			btAbort.Text = "Abort all User Tasks";
			btAbort.UseVisualStyleBackColor = true;
			btAbort.Click += new System.EventHandler(btAbort_Click);
			contextMenuAbort.Items.AddRange(new System.Windows.Forms.ToolStripItem[1]
			{
				dummyToolStripMenuItem
			});
			contextMenuAbort.Name = "contextMenuAbort";
			contextMenuAbort.Size = new System.Drawing.Size(118, 26);
			contextMenuAbort.Opening += new System.ComponentModel.CancelEventHandler(contextMenuAbort_Opening);
			dummyToolStripMenuItem.Name = "dummyToolStripMenuItem";
			dummyToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			dummyToolStripMenuItem.Text = "Dummy";
			tabs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tabs.Controls.Add(tabTasks);
			tabs.Controls.Add(tabNetwork);
			tabs.Location = new System.Drawing.Point(12, 6);
			tabs.Name = "tabs";
			tabs.SelectedIndex = 0;
			tabs.Size = new System.Drawing.Size(608, 452);
			tabs.TabIndex = 4;
			tabTasks.Controls.Add(lvTasks);
			tabTasks.Controls.Add(lblItemCount);
			tabTasks.Controls.Add(btAbort);
			tabTasks.Location = new System.Drawing.Point(4, 22);
			tabTasks.Name = "tabTasks";
			tabTasks.Padding = new System.Windows.Forms.Padding(3);
			tabTasks.Size = new System.Drawing.Size(600, 426);
			tabTasks.TabIndex = 0;
			tabTasks.Text = "Background Tasks";
			tabTasks.UseVisualStyleBackColor = true;
			tabNetwork.Controls.Add(tvStats);
			tabNetwork.Location = new System.Drawing.Point(4, 22);
			tabNetwork.Name = "tabNetwork";
			tabNetwork.Padding = new System.Windows.Forms.Padding(3);
			tabNetwork.Size = new System.Drawing.Size(600, 426);
			tabNetwork.TabIndex = 1;
			tabNetwork.Text = "Server Statistics";
			tabNetwork.UseVisualStyleBackColor = true;
			tvStats.Dock = System.Windows.Forms.DockStyle.Fill;
			tvStats.ImageIndex = 0;
			tvStats.ImageList = networkImages;
			tvStats.Location = new System.Drawing.Point(3, 3);
			tvStats.Name = "tvStats";
			tvStats.SelectedImageIndex = 0;
			tvStats.ShowLines = false;
			tvStats.ShowRootLines = false;
			tvStats.Size = new System.Drawing.Size(594, 420);
			tvStats.TabIndex = 1;
			networkImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			networkImages.ImageSize = new System.Drawing.Size(16, 16);
			networkImages.TransparentColor = System.Drawing.Color.Transparent;
			btClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			btClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btClose.Location = new System.Drawing.Point(524, 464);
			btClose.Name = "btClose";
			btClose.Size = new System.Drawing.Size(96, 24);
			btClose.TabIndex = 5;
			btClose.Text = "&Close";
			base.AcceptButton = btClose;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btClose;
			base.ClientSize = new System.Drawing.Size(632, 499);
			base.Controls.Add(btClose);
			base.Controls.Add(tabs);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(400, 200);
			base.Name = "TasksDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Tasks";
			contextMenuAbort.ResumeLayout(false);
			tabs.ResumeLayout(false);
			tabTasks.ResumeLayout(false);
			tabTasks.PerformLayout();
			tabNetwork.ResumeLayout(false);
			ResumeLayout(false);
		}
		
		private IContainer components;

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
