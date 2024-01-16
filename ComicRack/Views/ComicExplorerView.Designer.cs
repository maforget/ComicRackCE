using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicExplorerView : SubView, ISidebar
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
            this.smallComicPreview = new cYo.Projects.ComicRack.Viewer.Views.SmallComicPreview();
            this.comicBrowser = new cYo.Projects.ComicRack.Viewer.Views.ComicBrowserControl();
            this.previewTimer = new System.Windows.Forms.Timer(this.components);
            this.sidePanel = new cYo.Common.Windows.Forms.SizableContainer();
            this.treePanel = new System.Windows.Forms.Panel();
            this.previewPane = new cYo.Common.Windows.Forms.SizableContainer();
            this.pluginContainer = new cYo.Common.Windows.Forms.SizableContainer();
            this.comicInfo = new cYo.Projects.ComicRack.Engine.Controls.ComicPageContainerControl();
            this.pluginPlaceholder = new System.Windows.Forms.Panel();
            this.sidePanel.SuspendLayout();
            this.previewPane.SuspendLayout();
            this.pluginContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // smallComicPreview
            // 
            this.smallComicPreview.Caption = "";
            this.smallComicPreview.CaptionMargin = new System.Windows.Forms.Padding(2);
            this.smallComicPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.smallComicPreview.Location = new System.Drawing.Point(2, 8);
            this.smallComicPreview.Name = "smallComicPreview";
            this.smallComicPreview.Size = new System.Drawing.Size(242, 197);
            this.smallComicPreview.TabIndex = 0;
            this.smallComicPreview.TwoPageDisplay = false;
            this.smallComicPreview.CloseClicked += new System.EventHandler(this.smallComicPreview_CloseClicked);
            // 
            // comicBrowser
            // 
            this.comicBrowser.Caption = "";
            this.comicBrowser.CaptionMargin = new System.Windows.Forms.Padding(2);
            this.comicBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comicBrowser.Location = new System.Drawing.Point(252, 0);
            this.comicBrowser.Name = "comicBrowser";
            this.comicBrowser.Size = new System.Drawing.Size(448, 370);
            this.comicBrowser.TabIndex = 0;
            // 
            // previewTimer
            // 
            this.previewTimer.Interval = 500;
            this.previewTimer.Tick += new System.EventHandler(this.previewTimer_Tick);
            // 
            // sidePanel
            // 
            this.sidePanel.AutoGripPosition = true;
            this.sidePanel.Controls.Add(this.treePanel);
            this.sidePanel.Controls.Add(this.previewPane);
            this.sidePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidePanel.Grip = cYo.Common.Windows.Forms.SizableContainer.GripPosition.Right;
            this.sidePanel.Location = new System.Drawing.Point(0, 0);
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(252, 538);
            this.sidePanel.TabIndex = 1;
            this.sidePanel.ExpandedChanged += new System.EventHandler(this.sidePanel_ExpandedChanged);
            // 
            // treePanel
            // 
            this.treePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treePanel.Location = new System.Drawing.Point(0, 0);
            this.treePanel.Name = "treePanel";
            this.treePanel.Size = new System.Drawing.Size(246, 331);
            this.treePanel.TabIndex = 0;
            // 
            // previewPane
            // 
            this.previewPane.AutoGripPosition = true;
            this.previewPane.BorderStyle = cYo.Common.Windows.Forms.ExtendedBorderStyle.Flat;
            this.previewPane.Controls.Add(this.smallComicPreview);
            this.previewPane.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.previewPane.Location = new System.Drawing.Point(0, 331);
            this.previewPane.Name = "previewPane";
            this.previewPane.Size = new System.Drawing.Size(246, 207);
            this.previewPane.TabIndex = 1;
            this.previewPane.Text = "sizableContainer1";
            this.previewPane.ExpandedChanged += new System.EventHandler(this.sidePanel_ExpandedChanged);
            // 
            // pluginContainer
            // 
            this.pluginContainer.AutoGripPosition = true;
            this.pluginContainer.Controls.Add(this.comicInfo);
            this.pluginContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pluginContainer.Location = new System.Drawing.Point(252, 370);
            this.pluginContainer.Name = "pluginContainer";
            this.pluginContainer.Size = new System.Drawing.Size(448, 162);
            this.pluginContainer.TabIndex = 2;
            this.pluginContainer.Visible = false;
            this.pluginContainer.ExpandedChanged += new System.EventHandler(this.pluginContainer_ExpandedChanged);
            // 
            // comicInfo
            // 
            this.comicInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comicInfo.Location = new System.Drawing.Point(0, 6);
            this.comicInfo.Name = "comicInfo";
            this.comicInfo.Size = new System.Drawing.Size(448, 156);
            this.comicInfo.TabIndex = 0;
            // 
            // pluginPlaceholder
            // 
            this.pluginPlaceholder.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pluginPlaceholder.Location = new System.Drawing.Point(252, 532);
            this.pluginPlaceholder.Name = "pluginPlaceholder";
            this.pluginPlaceholder.Size = new System.Drawing.Size(448, 6);
            this.pluginPlaceholder.TabIndex = 3;
            // 
            // ComicExplorerView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.comicBrowser);
            this.Controls.Add(this.pluginContainer);
            this.Controls.Add(this.pluginPlaceholder);
            this.Controls.Add(this.sidePanel);
            this.Name = "ComicExplorerView";
            this.Size = new System.Drawing.Size(700, 538);
            this.sidePanel.ResumeLayout(false);
            this.previewPane.ResumeLayout(false);
            this.pluginContainer.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        private ComicBrowserControl comicBrowser;
        private Timer previewTimer;
        private SmallComicPreview smallComicPreview;
        private SizableContainer sidePanel;
        private Panel treePanel;
        private SizableContainer previewPane;
        private SizableContainer pluginContainer;
        private ComicPageContainerControl comicInfo;
        private Panel pluginPlaceholder;
    }
}
