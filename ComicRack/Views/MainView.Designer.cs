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
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Display.Forms;
using cYo.Projects.ComicRack.Engine.IO.Network;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class MainView : SubView, IDisplayWorkspace, IListDisplays
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				openBrowsers.ToArray().Dispose();
				(from tsb in tabStrip.Items
					select tsb.Tag as ComicExplorerView into c
					where c != null && c.Tag != null && c.ComicBrowser.Library != null
					select c.ComicBrowser.Library).Dispose();
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.tabStrip = new cYo.Common.Windows.Forms.TabBar();
            this.tabToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbAlignment = new System.Windows.Forms.ToolStripSplitButton();
            this.tsbAlignBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbAlignLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbAlignRight = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbAlignFill = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbInfoPanelLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.tabStrip.SuspendLayout();
            this.tabToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabStrip
            // 
            this.tabStrip.AllowDrop = true;
            this.tabStrip.BackColor = SystemColors.Control;
            this.tabStrip.BottomPadding = 0;
            this.tabStrip.CloseImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Close;
            this.tabStrip.Controls.Add(this.tabToolStrip);
            this.tabStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabStrip.Location = new System.Drawing.Point(0, 0);
            this.tabStrip.Name = "tabStrip";
            this.tabStrip.OwnerDrawnTooltips = true;
            this.tabStrip.Size = new System.Drawing.Size(895, 25);
            this.tabStrip.TabIndex = 0;
            this.tabStrip.Text = "tabStrip";
            this.tabStrip.TopPadding = 0;
            this.tabStrip.SelectedTabChanged += new System.EventHandler<cYo.Common.Windows.Forms.TabBar.SelectedTabChangedEventArgs>(this.tabStrip_SelectedTabChanged);
            // 
            // tabToolStrip
            // 
            this.tabToolStrip.BackColor = System.Drawing.Color.Transparent;
            this.tabToolStrip.CanOverflow = false;
            this.tabToolStrip.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tabToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAlignment});
            this.tabToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.tabToolStrip.Location = new System.Drawing.Point(860, 1);
            this.tabToolStrip.Name = "tabToolStrip";
            this.tabToolStrip.Size = new System.Drawing.Size(35, 23);
            this.tabToolStrip.TabIndex = 1;
            // 
            // tsbAlignment
            // 
            this.tsbAlignment.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAlignment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAlignment.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAlignBottom,
            this.tsbAlignLeft,
            this.tsbAlignRight,
            this.tsbAlignFill,
            this.toolStripMenuItem1,
            this.tsbInfoPanelLeft});
            this.tsbAlignment.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AlignBottom;
            this.tsbAlignment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAlignment.Name = "tsbAlignment";
            this.tsbAlignment.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tsbAlignment.Size = new System.Drawing.Size(32, 20);
            this.tsbAlignment.Text = "Docking Mode";
            // 
            // tsbAlignBottom
            // 
            this.tsbAlignBottom.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAlignBottom.Checked = true;
            this.tsbAlignBottom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbAlignBottom.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AlignBottom;
            this.tsbAlignBottom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAlignBottom.Name = "tsbAlignBottom";
            this.tsbAlignBottom.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D1)));
            this.tsbAlignBottom.Size = new System.Drawing.Size(230, 22);
            this.tsbAlignBottom.Text = "Dock Bottom";
            // 
            // tsbAlignLeft
            // 
            this.tsbAlignLeft.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAlignLeft.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AlignLeft;
            this.tsbAlignLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAlignLeft.Name = "tsbAlignLeft";
            this.tsbAlignLeft.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D2)));
            this.tsbAlignLeft.Size = new System.Drawing.Size(230, 22);
            this.tsbAlignLeft.Text = "Dock Left";
            // 
            // tsbAlignRight
            // 
            this.tsbAlignRight.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAlignRight.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AlignRight;
            this.tsbAlignRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAlignRight.Name = "tsbAlignRight";
            this.tsbAlignRight.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D3)));
            this.tsbAlignRight.Size = new System.Drawing.Size(230, 22);
            this.tsbAlignRight.Text = "Dock Right";
            // 
            // tsbAlignFill
            // 
            this.tsbAlignFill.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAlignFill.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AlignFill;
            this.tsbAlignFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAlignFill.Name = "tsbAlignFill";
            this.tsbAlignFill.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D4)));
            this.tsbAlignFill.Size = new System.Drawing.Size(230, 22);
            this.tsbAlignFill.Text = "Fill";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(227, 6);
            // 
            // tsbInfoPanelLeft
            // 
            this.tsbInfoPanelLeft.Name = "tsbInfoPanelLeft";
            this.tsbInfoPanelLeft.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D5)));
            this.tsbInfoPanelLeft.Size = new System.Drawing.Size(230, 22);
            this.tsbInfoPanelLeft.Text = "Info Panel Right";
            // 
            // MainView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = SystemColors.Control;
            this.Controls.Add(this.tabStrip);
            this.Name = "MainView";
            this.Size = new System.Drawing.Size(895, 551);
            this.tabStrip.ResumeLayout(false);
            this.tabStrip.PerformLayout();
            this.tabToolStrip.ResumeLayout(false);
            this.tabToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private TabBar tabStrip;
        private ToolStrip tabToolStrip;
        private ToolStripSplitButton tsbAlignment;
        private ToolStripMenuItem tsbAlignBottom;
        private ToolStripMenuItem tsbAlignLeft;
        private ToolStripMenuItem tsbAlignRight;
        private ToolStripMenuItem tsbAlignFill;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem tsbInfoPanelLeft;
    }
}
