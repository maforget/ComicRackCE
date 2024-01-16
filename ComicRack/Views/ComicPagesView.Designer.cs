using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
    public partial class ComicPagesView : SubView, IDisplayWorkspace, IRefreshDisplay, IItemSize
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tbbView = new System.Windows.Forms.ToolStripSplitButton();
            this.miViewThumbnails = new System.Windows.Forms.ToolStripMenuItem();
            this.miViewTiles = new System.Windows.Forms.ToolStripMenuItem();
            this.miViewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miExpandAllGroups = new System.Windows.Forms.ToolStripMenuItem();
            this.tbbGroup = new System.Windows.Forms.ToolStripSplitButton();
            this.tbbSort = new System.Windows.Forms.ToolStripSplitButton();
            this.tbFilter = new System.Windows.Forms.ToolStripDropDownButton();
            this.pagesView = new cYo.Projects.ComicRack.Viewer.Controls.PagesView();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbbView,
            this.tbbGroup,
            this.tbbSort,
            this.tbFilter});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(656, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tbbView
            // 
            this.tbbView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miViewThumbnails,
            this.miViewTiles,
            this.miViewDetails,
            this.toolStripMenuItem1,
            this.miExpandAllGroups});
            this.tbbView.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.View;
            this.tbbView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbView.Name = "tbbView";
            this.tbbView.Size = new System.Drawing.Size(69, 22);
            this.tbbView.Text = "Views";
            this.tbbView.ToolTipText = "Change how Books are displayed";
            this.tbbView.ButtonClick += new System.EventHandler(this.tbbView_ButtonClick);
            // 
            // miViewThumbnails
            // 
            this.miViewThumbnails.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ThumbView;
            this.miViewThumbnails.Name = "miViewThumbnails";
            this.miViewThumbnails.Size = new System.Drawing.Size(219, 22);
            this.miViewThumbnails.Text = "T&humbnails";
            // 
            // miViewTiles
            // 
            this.miViewTiles.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.TileView;
            this.miViewTiles.Name = "miViewTiles";
            this.miViewTiles.Size = new System.Drawing.Size(219, 22);
            this.miViewTiles.Text = "&Tiles";
            // 
            // miViewDetails
            // 
            this.miViewDetails.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DetailView;
            this.miViewDetails.Name = "miViewDetails";
            this.miViewDetails.Size = new System.Drawing.Size(219, 22);
            this.miViewDetails.Text = "&Details";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(216, 6);
            // 
            // miExpandAllGroups
            // 
            this.miExpandAllGroups.Name = "miExpandAllGroups";
            this.miExpandAllGroups.Size = new System.Drawing.Size(219, 22);
            this.miExpandAllGroups.Text = "Collapse/Expand all Groups";
            // 
            // tbbGroup
            // 
            this.tbbGroup.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Group;
            this.tbbGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbGroup.Name = "tbbGroup";
            this.tbbGroup.Size = new System.Drawing.Size(72, 22);
            this.tbbGroup.Text = "Group";
            this.tbbGroup.ToolTipText = "Group Books by different criteria";
            this.tbbGroup.ButtonClick += new System.EventHandler(this.tbbGroup_ButtonClick);
            this.tbbGroup.DropDownOpening += new System.EventHandler(this.tbbGroup_DropDownOpening);
            // 
            // tbbSort
            // 
            this.tbbSort.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SortUp;
            this.tbbSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbbSort.Name = "tbbSort";
            this.tbbSort.Size = new System.Drawing.Size(81, 22);
            this.tbbSort.Text = "Arrange";
            this.tbbSort.ToolTipText = "Change the sort order of the Books";
            this.tbbSort.ButtonClick += new System.EventHandler(this.tbbSort_ButtonClick);
            this.tbbSort.DropDownOpening += new System.EventHandler(this.tbbSort_DropDownOpening);
            // 
            // tbFilter
            // 
            this.tbFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbFilter.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Search;
            this.tbFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(91, 22);
            this.tbFilter.Text = "Page Filter";
            // 
            // pagesView
            // 
            this.pagesView.Bookmark = null;
            this.pagesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagesView.Location = new System.Drawing.Point(0, 25);
            this.pagesView.Name = "pagesView";
            this.pagesView.Size = new System.Drawing.Size(656, 399);
            this.pagesView.TabIndex = 2;
            // 
            // ComicPagesView
            // 
            this.Controls.Add(this.pagesView);
            this.Controls.Add(this.toolStrip);
            this.Name = "ComicPagesView";
            this.Size = new System.Drawing.Size(656, 424);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private ToolStrip toolStrip;
        private ToolStripSplitButton tbbView;
        private ToolStripMenuItem miViewThumbnails;
        private ToolStripMenuItem miViewTiles;
        private ToolStripMenuItem miViewDetails;
        private ToolStripSplitButton tbbSort;
        private ToolStripSplitButton tbbGroup;
        private ToolStripDropDownButton tbFilter;
        private PagesView pagesView;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem miExpandAllGroups;
    }
}
