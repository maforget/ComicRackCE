using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class NewsDialog
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
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.listNewItems = new System.Windows.Forms.ListView();
			this.chTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.webBrowser = new System.Windows.Forms.WebBrowser();
			this.btClose = new System.Windows.Forms.Button();
			this.btMarkRead = new System.Windows.Forms.Button();
			this.chkNewsStartup = new System.Windows.Forms.CheckBox();
			this.btRefresh = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer.Location = new System.Drawing.Point(11, 13);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.listNewItems);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.webBrowser);
			this.splitContainer.Size = new System.Drawing.Size(1162, 512);
			this.splitContainer.SplitterDistance = 380;
			this.splitContainer.TabIndex = 0;
			// 
			// listNewItems
			// 
			this.listNewItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listNewItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTitle});
			this.listNewItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listNewItems.FullRowSelect = true;
			this.listNewItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listNewItems.HideSelection = false;
			this.listNewItems.Location = new System.Drawing.Point(0, 0);
			this.listNewItems.MultiSelect = false;
			this.listNewItems.Name = "listNewItems";
			this.listNewItems.Size = new System.Drawing.Size(378, 510);
			this.listNewItems.TabIndex = 0;
			this.listNewItems.UseCompatibleStateImageBehavior = false;
			this.listNewItems.View = System.Windows.Forms.View.Details;
			this.listNewItems.SelectedIndexChanged += new System.EventHandler(this.listNewItems_SelectedIndexChanged);
			// 
			// chTitle
			// 
			this.chTitle.Text = "Title";
			this.chTitle.Width = 360;
			// 
			// webBrowser
			// 
			this.webBrowser.AllowWebBrowserDrop = false;
			this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser.Location = new System.Drawing.Point(0, 0);
			this.webBrowser.MinimumSize = new System.Drawing.Size(600, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.ScriptErrorsSuppressed = true;
			this.webBrowser.Size = new System.Drawing.Size(776, 510);
			this.webBrowser.TabIndex = 0;
			this.webBrowser.WebBrowserShortcutsEnabled = false;
			this.webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
			// 
			// btClose
			// 
			this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btClose.Location = new System.Drawing.Point(1096, 531);
			this.btClose.Name = "btClose";
			this.btClose.Size = new System.Drawing.Size(76, 24);
			this.btClose.TabIndex = 2;
			this.btClose.Text = "&Close";
			// 
			// btMarkRead
			// 
			this.btMarkRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btMarkRead.Location = new System.Drawing.Point(987, 532);
			this.btMarkRead.Name = "btMarkRead";
			this.btMarkRead.Size = new System.Drawing.Size(103, 23);
			this.btMarkRead.TabIndex = 1;
			this.btMarkRead.Text = "Mark all as Read";
			this.btMarkRead.UseVisualStyleBackColor = true;
			this.btMarkRead.Click += new System.EventHandler(this.btMarkRead_Click);
			// 
			// chkNewsStartup
			// 
			this.chkNewsStartup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkNewsStartup.AutoSize = true;
			this.chkNewsStartup.Location = new System.Drawing.Point(12, 536);
			this.chkNewsStartup.Name = "chkNewsStartup";
			this.chkNewsStartup.Size = new System.Drawing.Size(154, 17);
			this.chkNewsStartup.TabIndex = 3;
			this.chkNewsStartup.Text = "Check for News on Startup";
			this.chkNewsStartup.UseVisualStyleBackColor = true;
			this.chkNewsStartup.CheckedChanged += new System.EventHandler(this.chkNewsStartup_CheckedChanged);
			// 
			// btRefresh
			// 
			this.btRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btRefresh.Location = new System.Drawing.Point(890, 532);
			this.btRefresh.Name = "btRefresh";
			this.btRefresh.Size = new System.Drawing.Size(91, 23);
			this.btRefresh.TabIndex = 0;
			this.btRefresh.Text = "Refresh";
			this.btRefresh.UseVisualStyleBackColor = true;
			this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
			// 
			// NewsDialog
			// 
			this.AcceptButton = this.btClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1184, 561);
			this.Controls.Add(this.btRefresh);
			this.Controls.Add(this.chkNewsStartup);
			this.Controls.Add(this.btMarkRead);
			this.Controls.Add(this.btClose);
			this.Controls.Add(this.splitContainer);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(400, 250);
			this.Name = "NewsDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Latest ComicRack News";
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private SplitContainer splitContainer;
		private ListView listNewItems;
		private WebBrowser webBrowser;
		private Button btClose;
		private ColumnHeader chTitle;
		private Button btMarkRead;
		private CheckBox chkNewsStartup;
		private Button btRefresh;
	}
}
