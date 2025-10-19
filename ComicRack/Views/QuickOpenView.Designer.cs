using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class QuickOpenView : CaptionControl
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
				itemView.Items.Clear();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickOpenView));
            this.panelStatus = new System.Windows.Forms.Panel();
            this.btOpenFile = new System.Windows.Forms.Button();
            this.btBrowser = new System.Windows.Forms.Button();
            this.btOpen = new System.Windows.Forms.Button();
            this.comicPageContainer = new cYo.Projects.ComicRack.Engine.Controls.ComicPageContainerControl();
            this.itemView = new cYo.Common.Windows.Forms.ItemView();
            this.panelStatus.SuspendLayout();
            this.comicPageContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStatus
            // 
            this.panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatus.Controls.Add(this.btOpenFile);
            this.panelStatus.Controls.Add(this.btBrowser);
            this.panelStatus.Controls.Add(this.btOpen);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatus.Location = new System.Drawing.Point(0, 402);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(573, 37);
            this.panelStatus.TabIndex = 1;
            // 
            // btOpenFile
            // 
            this.btOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btOpenFile.Location = new System.Drawing.Point(3, 6);
            this.btOpenFile.Name = "btOpenFile";
            this.btOpenFile.Size = new System.Drawing.Size(90, 23);
            this.btOpenFile.TabIndex = 0;
            this.btOpenFile.Text = "Open File...";
            this.btOpenFile.UseVisualStyleBackColor = true;
            this.btOpenFile.Click += new System.EventHandler(this.btOpenFile_Click);
            // 
            // btBrowser
            // 
            this.btBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btBrowser.Location = new System.Drawing.Point(478, 6);
            this.btBrowser.Name = "btBrowser";
            this.btBrowser.Size = new System.Drawing.Size(90, 23);
            this.btBrowser.TabIndex = 2;
            this.btBrowser.Text = "Browser";
            this.btBrowser.UseVisualStyleBackColor = true;
            this.btBrowser.Click += new System.EventHandler(this.btBrowser_Click);
            // 
            // btOpen
            // 
            this.btOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btOpen.Location = new System.Drawing.Point(99, 6);
            this.btOpen.Name = "btOpen";
            this.btOpen.Size = new System.Drawing.Size(90, 23);
            this.btOpen.TabIndex = 1;
            this.btOpen.Text = "Open";
            this.btOpen.UseVisualStyleBackColor = true;
            this.btOpen.Click += new System.EventHandler(this.btOpen_Click);
            // 
            // comicPageContainer
            // 
            this.comicPageContainer.Controls.Add(this.itemView);
            this.comicPageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comicPageContainer.Location = new System.Drawing.Point(0, 19);
            this.comicPageContainer.Name = "comicPageContainer";
            this.comicPageContainer.Size = new System.Drawing.Size(573, 383);
            this.comicPageContainer.TabIndex = 2;
            this.comicPageContainer.Text = "Books";
            // 
            // itemView
            // 
            this.itemView.AutomaticHeaderMenu = false;
            this.itemView.AutomaticViewMenu = false;
            this.itemView.BackColor = ThemeColors.ItemView.MainBack;
            this.itemView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemView.EnableStick = false;
            this.itemView.GroupCollapsedImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ArrowRight;
            this.itemView.GroupColumns = new cYo.Common.Windows.Forms.IColumn[0];
            this.itemView.GroupColumnsKey = null;
            this.itemView.GroupDisplayEnabled = true;
            this.itemView.GroupExpandedImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ArrowDown;
            this.itemView.GroupsStatus = ((cYo.Common.Windows.Forms.ItemViewGroupsStatus)(resources.GetObject("itemView.GroupsStatus")));
            this.itemView.HorizontalItemAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.itemView.LabelEdit = false;
            this.itemView.Location = new System.Drawing.Point(0, 0);
            this.itemView.Multiselect = false;
            this.itemView.Name = "itemView";
            this.itemView.SelectionMode = System.Windows.Forms.SelectionMode.One;
            this.itemView.ShowGroupCount = false;
            this.itemView.Size = new System.Drawing.Size(573, 383);
            this.itemView.SortColumn = null;
            this.itemView.SortColumns = new cYo.Common.Windows.Forms.IColumn[0];
            this.itemView.SortColumnsKey = null;
            this.itemView.StackColumns = new cYo.Common.Windows.Forms.IColumn[0];
            this.itemView.StackColumnsKey = null;
            this.itemView.TabIndex = 0;
            this.itemView.ItemActivate += new System.EventHandler(this.itemView_ItemActivate);
            this.itemView.SelectedIndexChanged += new System.EventHandler(this.itemView_SelectedIndexChanged);
            this.itemView.PostPaint += new System.Windows.Forms.PaintEventHandler(this.itemView_PostPaint);
            this.itemView.VisibleChanged += new System.EventHandler(this.itemView_VisibleChanged);
            // 
            // QuickOpenView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = SystemColors.Window;
            this.Caption = "Quick Open";
            this.Controls.Add(this.comicPageContainer);
            this.Controls.Add(this.panelStatus);
            this.Name = "QuickOpenView";
            this.Size = new System.Drawing.Size(573, 439);
            this.panelStatus.ResumeLayout(false);
            this.comicPageContainer.ResumeLayout(false);
            this.comicPageContainer.PerformLayout();
            this.ResumeLayout(false);

		}

        private ItemView itemView;
        private Panel panelStatus;
        private Button btBrowser;
        private Button btOpen;
        private Button btOpenFile;
        private ComicPageContainerControl comicPageContainer;
    }
}
