using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class NewsDialog
	{
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
			splitContainer = new System.Windows.Forms.SplitContainer();
			listNewItems = new System.Windows.Forms.ListView();
			chTitle = new System.Windows.Forms.ColumnHeader();
			webBrowser = new System.Windows.Forms.WebBrowser();
			btClose = new System.Windows.Forms.Button();
			btMarkRead = new System.Windows.Forms.Button();
			chkNewsStartup = new System.Windows.Forms.CheckBox();
			btRefresh = new System.Windows.Forms.Button();
			splitContainer.Panel1.SuspendLayout();
			splitContainer.Panel2.SuspendLayout();
			splitContainer.SuspendLayout();
			SuspendLayout();
			splitContainer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			splitContainer.Location = new System.Drawing.Point(11, 13);
			splitContainer.Name = "splitContainer";
			splitContainer.Panel1.Controls.Add(listNewItems);
			splitContainer.Panel2.Controls.Add(webBrowser);
			splitContainer.Size = new System.Drawing.Size(623, 361);
			splitContainer.SplitterDistance = 214;
			splitContainer.TabIndex = 0;
			listNewItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
			listNewItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
			{
				chTitle
			});
			listNewItems.Dock = System.Windows.Forms.DockStyle.Fill;
			listNewItems.FullRowSelect = true;
			listNewItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			listNewItems.HideSelection = false;
			listNewItems.Location = new System.Drawing.Point(0, 0);
			listNewItems.MultiSelect = false;
			listNewItems.Name = "listNewItems";
			listNewItems.Size = new System.Drawing.Size(212, 359);
			listNewItems.TabIndex = 0;
			listNewItems.UseCompatibleStateImageBehavior = false;
			listNewItems.View = System.Windows.Forms.View.Details;
			listNewItems.SelectedIndexChanged += new System.EventHandler(listNewItems_SelectedIndexChanged);
			chTitle.Text = "Title";
			chTitle.Width = 186;
			webBrowser.AllowWebBrowserDrop = false;
			webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			webBrowser.Location = new System.Drawing.Point(0, 0);
			webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			webBrowser.Name = "webBrowser";
			webBrowser.ScriptErrorsSuppressed = true;
			webBrowser.Size = new System.Drawing.Size(403, 359);
			webBrowser.TabIndex = 0;
			webBrowser.WebBrowserShortcutsEnabled = false;
			webBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(webBrowser_Navigating);
			btClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			btClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btClose.Location = new System.Drawing.Point(557, 380);
			btClose.Name = "btClose";
			btClose.Size = new System.Drawing.Size(76, 24);
			btClose.TabIndex = 2;
			btClose.Text = "&Close";
			btMarkRead.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btMarkRead.Location = new System.Drawing.Point(448, 381);
			btMarkRead.Name = "btMarkRead";
			btMarkRead.Size = new System.Drawing.Size(103, 23);
			btMarkRead.TabIndex = 1;
			btMarkRead.Text = "Mark all as Read";
			btMarkRead.UseVisualStyleBackColor = true;
			btMarkRead.Click += new System.EventHandler(btMarkRead_Click);
			chkNewsStartup.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkNewsStartup.AutoSize = true;
			chkNewsStartup.Location = new System.Drawing.Point(12, 385);
			chkNewsStartup.Name = "chkNewsStartup";
			chkNewsStartup.Size = new System.Drawing.Size(154, 17);
			chkNewsStartup.TabIndex = 3;
			chkNewsStartup.Text = "Check for News on Startup";
			chkNewsStartup.UseVisualStyleBackColor = true;
			chkNewsStartup.CheckedChanged += new System.EventHandler(chkNewsStartup_CheckedChanged);
			btRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btRefresh.Location = new System.Drawing.Point(351, 381);
			btRefresh.Name = "btRefresh";
			btRefresh.Size = new System.Drawing.Size(91, 23);
			btRefresh.TabIndex = 0;
			btRefresh.Text = "Refresh";
			btRefresh.UseVisualStyleBackColor = true;
			btRefresh.Click += new System.EventHandler(btRefresh_Click);
			base.AcceptButton = btClose;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(645, 410);
			base.Controls.Add(btRefresh);
			base.Controls.Add(chkNewsStartup);
			base.Controls.Add(btMarkRead);
			base.Controls.Add(btClose);
			base.Controls.Add(splitContainer);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(400, 250);
			base.Name = "NewsDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Latest ComicRack News";
			splitContainer.Panel1.ResumeLayout(false);
			splitContainer.Panel2.ResumeLayout(false);
			splitContainer.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
		
		private IContainer components;

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
