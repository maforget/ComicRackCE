using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public partial class PagesView : UserControlEx, IEditBookmark, IEditPage
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IdleProcess.Idle -= Application_Idle;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PagesView));
            this.itemView = new cYo.Common.Windows.Forms.ItemView();
            this.contextPages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miPageType = new System.Windows.Forms.ToolStripMenuItem();
            this.cmPageRotate = new System.Windows.Forms.ToolStripMenuItem();
            this.miPagePosition = new System.Windows.Forms.ToolStripMenuItem();
            this.miPagePositionDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miPagePositionNear = new System.Windows.Forms.ToolStripMenuItem();
            this.miPagePositionFar = new System.Windows.Forms.ToolStripMenuItem();
            this.tsPageTypeSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.miSetBookmark = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemoveBookmark = new System.Windows.Forms.ToolStripMenuItem();
            this.tsBookmarkSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.miMoveToTop = new System.Windows.Forms.ToolStripMenuItem();
            this.miMoveToBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.miResetOriginalOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMovePagesSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.miMergePages = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.miInvertSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miRefreshThumbnail = new System.Windows.Forms.ToolStripMenuItem();
            this.miMarkDeleted = new System.Windows.Forms.ToolStripMenuItem();
            this.contextPages.SuspendLayout();
            this.SuspendLayout();
            // 
            // itemView
            // 
            this.itemView.AllowDrop = true;
            this.itemView.BackColor = ThemeColors.ItemView.MainBack;
            this.itemView.BackgroundImageAlignment = System.Drawing.ContentAlignment.BottomRight;
            this.itemView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemView.GroupColumns = new cYo.Common.Windows.Forms.IColumn[0];
            this.itemView.GroupColumnsKey = null;
            this.itemView.GroupsStatus = ((cYo.Common.Windows.Forms.ItemViewGroupsStatus)(resources.GetObject("itemView.GroupsStatus")));
            this.itemView.HideSelection = false;
            this.itemView.ItemContextMenuStrip = this.contextPages;
            this.itemView.Location = new System.Drawing.Point(0, 0);
            this.itemView.Name = "itemView";
            this.itemView.Size = new System.Drawing.Size(413, 406);
            this.itemView.SortColumn = null;
            this.itemView.SortColumns = new cYo.Common.Windows.Forms.IColumn[0];
            this.itemView.SortColumnsKey = null;
            this.itemView.StackColumns = new cYo.Common.Windows.Forms.IColumn[0];
            this.itemView.StackColumnsKey = null;
            this.itemView.TabIndex = 1;
            this.itemView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.itemView_ItemDrag);
            this.itemView.PostPaint += new System.Windows.Forms.PaintEventHandler(this.itemView_PostPaint);
            this.itemView.DragDrop += new System.Windows.Forms.DragEventHandler(this.itemView_DragDrop);
            this.itemView.DragEnter += new System.Windows.Forms.DragEventHandler(this.itemView_DragEnter);
            this.itemView.DragOver += new System.Windows.Forms.DragEventHandler(this.itemView_DragOver);
            this.itemView.DragLeave += new System.EventHandler(this.itemView_DragLeave);
            this.itemView.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.itemView_GiveFeedback);
            // 
            // contextPages
            // 
            this.contextPages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miPageType,
            this.cmPageRotate,
            this.miPagePosition,
            this.tsPageTypeSeparator,
            this.miSetBookmark,
            this.miRemoveBookmark,
            this.tsBookmarkSeparator,
            this.miMoveToTop,
            this.miMoveToBottom,
            this.miResetOriginalOrder,
            this.tsMovePagesSeparator,
            this.miCopy,
            this.miMergePages,
            this.toolStripMenuItem1,
            this.miSelectAll,
            this.miInvertSelection,
            this.miRefreshThumbnail,
            this.toolStripMenuItem3,
            this.miMarkDeleted});
            this.contextPages.Name = "cmPages";
            this.contextPages.Size = new System.Drawing.Size(249, 364);
            this.contextPages.Opening += new System.ComponentModel.CancelEventHandler(this.contextPages_Opening);
            // 
            // miPageType
            // 
            this.miPageType.Name = "miPageType";
            this.miPageType.Size = new System.Drawing.Size(248, 22);
            this.miPageType.Text = "Page Type";
            // 
            // cmPageRotate
            // 
            this.cmPageRotate.Name = "cmPageRotate";
            this.cmPageRotate.Size = new System.Drawing.Size(248, 22);
            this.cmPageRotate.Text = "Page Rotation";
            // 
            // miPagePosition
            // 
            this.miPagePosition.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miPagePositionDefault,
            this.toolStripMenuItem2,
            this.miPagePositionNear,
            this.miPagePositionFar});
            this.miPagePosition.Name = "miPagePosition";
            this.miPagePosition.Size = new System.Drawing.Size(248, 22);
            this.miPagePosition.Text = "Page Position";
            // 
            // miPagePositionDefault
            // 
            this.miPagePositionDefault.Name = "miPagePositionDefault";
            this.miPagePositionDefault.Size = new System.Drawing.Size(112, 22);
            this.miPagePositionDefault.Text = "Default";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(109, 6);
            // 
            // miPagePositionNear
            // 
            this.miPagePositionNear.Name = "miPagePositionNear";
            this.miPagePositionNear.Size = new System.Drawing.Size(112, 22);
            this.miPagePositionNear.Text = "Near";
            // 
            // miPagePositionFar
            // 
            this.miPagePositionFar.Name = "miPagePositionFar";
            this.miPagePositionFar.Size = new System.Drawing.Size(112, 22);
            this.miPagePositionFar.Text = "Far";
            // 
            // tsPageTypeSeparator
            // 
            this.tsPageTypeSeparator.Name = "tsPageTypeSeparator";
            this.tsPageTypeSeparator.Size = new System.Drawing.Size(245, 6);
            // 
            // miSetBookmark
            // 
            this.miSetBookmark.Name = "miSetBookmark";
            this.miSetBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
            this.miSetBookmark.Size = new System.Drawing.Size(248, 22);
            this.miSetBookmark.Text = "Set Bookmark...";
            // 
            // miRemoveBookmark
            // 
            this.miRemoveBookmark.Name = "miRemoveBookmark";
            this.miRemoveBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.miRemoveBookmark.Size = new System.Drawing.Size(248, 22);
            this.miRemoveBookmark.Text = "Remove Bookmark";
            // 
            // tsBookmarkSeparator
            // 
            this.tsBookmarkSeparator.Name = "tsBookmarkSeparator";
            this.tsBookmarkSeparator.Size = new System.Drawing.Size(245, 6);
            // 
            // miMoveToTop
            // 
            this.miMoveToTop.Name = "miMoveToTop";
            this.miMoveToTop.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.T)));
            this.miMoveToTop.Size = new System.Drawing.Size(248, 22);
            this.miMoveToTop.Text = "&Move to Top";
            // 
            // miMoveToBottom
            // 
            this.miMoveToBottom.Name = "miMoveToBottom";
            this.miMoveToBottom.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
            this.miMoveToBottom.Size = new System.Drawing.Size(248, 22);
            this.miMoveToBottom.Text = "Move to Bottom";
            // 
            // miResetOriginalOrder
            // 
            this.miResetOriginalOrder.Name = "miResetOriginalOrder";
            this.miResetOriginalOrder.Size = new System.Drawing.Size(248, 22);
            this.miResetOriginalOrder.Text = "Reset original Order";
            // 
            // tsMovePagesSeparator
            // 
            this.tsMovePagesSeparator.Name = "tsMovePagesSeparator";
            this.tsMovePagesSeparator.Size = new System.Drawing.Size(245, 6);
            // 
            // miMergePages
            // 
            this.miMergePages.Name = "miMergePages";
            this.miMergePages.Size = new System.Drawing.Size(248, 22);
            this.miMergePages.Text = "Merge Pages";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(245, 6);
            // 
            // miSelectAll
            // 
            this.miSelectAll.Name = "miSelectAll";
            this.miSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.miSelectAll.Size = new System.Drawing.Size(248, 22);
            this.miSelectAll.Text = "Select &All";
            // 
            // miInvertSelection
            // 
            this.miInvertSelection.Name = "miInvertSelection";
            this.miInvertSelection.Size = new System.Drawing.Size(248, 22);
            this.miInvertSelection.Text = "&Invert Selection";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(245, 6);
            // 
            // miCopy
            // 
            this.miCopy.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Copy;
            this.miCopy.Name = "miCopy";
            this.miCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.miCopy.Size = new System.Drawing.Size(248, 22);
            this.miCopy.Text = "&Copy Page";
            // 
            // miRefreshThumbnail
            // 
            this.miRefreshThumbnail.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RefreshThumbnail;
            this.miRefreshThumbnail.Name = "miRefreshThumbnail";
            this.miRefreshThumbnail.Size = new System.Drawing.Size(248, 22);
            this.miRefreshThumbnail.Text = "&Refresh";
            // 
            // miMarkDeleted
            // 
            this.miMarkDeleted.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDelete;
            this.miMarkDeleted.Name = "miMarkDeleted";
            this.miMarkDeleted.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.miMarkDeleted.Size = new System.Drawing.Size(248, 22);
            this.miMarkDeleted.Text = "Mark as &Deleted";
            // 
            // PagesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.itemView);
            this.Name = "PagesView";
            this.Size = new System.Drawing.Size(413, 406);
            this.contextPages.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        private ItemView itemView;
        private ContextMenuStrip contextPages;
        private ToolStripMenuItem miPageType;
        private ToolStripSeparator tsPageTypeSeparator;
        private ToolStripMenuItem miCopy;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem miSelectAll;
        private ToolStripMenuItem miInvertSelection;
        private ToolStripMenuItem miRefreshThumbnail;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem miMarkDeleted;
        private ToolStripSeparator tsMovePagesSeparator;
        private ToolStripMenuItem miSetBookmark;
        private ToolStripMenuItem miRemoveBookmark;
        private ToolStripSeparator tsBookmarkSeparator;
        private ToolStripMenuItem miMoveToTop;
        private ToolStripMenuItem miMoveToBottom;
        private ToolStripMenuItem miResetOriginalOrder;
        private ToolStripMenuItem cmPageRotate;
        private ToolStripMenuItem miPagePosition;
        private ToolStripMenuItem miPagePositionDefault;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem miPagePositionNear;
        private ToolStripMenuItem miPagePositionFar;
        private ToolStripMenuItem miMergePages;
    }
}
