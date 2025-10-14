using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Net.Search;
using cYo.Common.Text;
using cYo.Common.Windows.Properties;

namespace cYo.Common.Windows.Forms
{
	public partial class ListSelectorControl : UserControlEx, Popup.INotifyClose
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
            this.lbOwn = new System.Windows.Forms.ListBox();
            this.lbPool = new System.Windows.Forms.ListBox();
            this.btAllToOwn = new System.Windows.Forms.Button();
            this.btSelectedToOwn = new System.Windows.Forms.Button();
            this.btSelectedToPool = new System.Windows.Forms.Button();
            this.btAllToPool = new System.Windows.Forms.Button();
            this.listPanel = new System.Windows.Forms.Panel();
            this.btLists = new System.Windows.Forms.Button();
            this.btCheck = new System.Windows.Forms.Button();
            this.btText = new System.Windows.Forms.Button();
            this.text = new System.Windows.Forms.TextBox();
            this.lbCheckList = new cYo.Common.Windows.Forms.CheckedListBoxEx();
            this.listPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbOwn
            // 
            this.lbOwn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbOwn.FormattingEnabled = true;
            this.lbOwn.IntegralHeight = false;
            this.lbOwn.Location = new System.Drawing.Point(0, 0);
            this.lbOwn.MultiColumn = true;
            this.lbOwn.Name = "lbOwn";
            this.lbOwn.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbOwn.Size = new System.Drawing.Size(245, 95);
            this.lbOwn.Sorted = true;
            this.lbOwn.TabIndex = 0;
            this.lbOwn.SelectedIndexChanged += new System.EventHandler(this.lbOwn_SelectedIndexChanged);
            this.lbOwn.DoubleClick += new System.EventHandler(this.lbOwn_DoubleClick);
            this.lbOwn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbOwn_KeyDown);
            // 
            // lbPool
            // 
            this.lbPool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPool.FormattingEnabled = true;
            this.lbPool.IntegralHeight = false;
            this.lbPool.Location = new System.Drawing.Point(0, 101);
            this.lbPool.MultiColumn = true;
            this.lbPool.Name = "lbPool";
            this.lbPool.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbPool.Size = new System.Drawing.Size(291, 142);
            this.lbPool.Sorted = true;
            this.lbPool.TabIndex = 5;
            this.lbPool.SelectedIndexChanged += new System.EventHandler(this.lbOwn_SelectedIndexChanged);
            this.lbPool.DoubleClick += new System.EventHandler(this.lbPool_DoubleClick);
            this.lbPool.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbPool_KeyDown);
            // 
            // btAllToOwn
            // 
            this.btAllToOwn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAllToOwn.Location = new System.Drawing.Point(251, 0);
            this.btAllToOwn.Name = "btAllToOwn";
            this.btAllToOwn.Size = new System.Drawing.Size(40, 23);
            this.btAllToOwn.TabIndex = 1;
            this.btAllToOwn.Text = "<<";
            this.btAllToOwn.UseVisualStyleBackColor = true;
            this.btAllToOwn.Click += new System.EventHandler(this.btAllToOwn_Click);
            // 
            // btSelectedToOwn
            // 
            this.btSelectedToOwn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelectedToOwn.Location = new System.Drawing.Point(251, 25);
            this.btSelectedToOwn.Name = "btSelectedToOwn";
            this.btSelectedToOwn.Size = new System.Drawing.Size(40, 23);
            this.btSelectedToOwn.TabIndex = 2;
            this.btSelectedToOwn.Text = "<";
            this.btSelectedToOwn.UseVisualStyleBackColor = true;
            this.btSelectedToOwn.Click += new System.EventHandler(this.btSelectedToOwn_Click);
            // 
            // btSelectedToPool
            // 
            this.btSelectedToPool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSelectedToPool.Location = new System.Drawing.Point(251, 49);
            this.btSelectedToPool.Name = "btSelectedToPool";
            this.btSelectedToPool.Size = new System.Drawing.Size(40, 23);
            this.btSelectedToPool.TabIndex = 3;
            this.btSelectedToPool.Text = ">";
            this.btSelectedToPool.UseVisualStyleBackColor = true;
            this.btSelectedToPool.Click += new System.EventHandler(this.btSelectedToPool_Click);
            // 
            // btAllToPool
            // 
            this.btAllToPool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAllToPool.Location = new System.Drawing.Point(251, 73);
            this.btAllToPool.Name = "btAllToPool";
            this.btAllToPool.Size = new System.Drawing.Size(40, 23);
            this.btAllToPool.TabIndex = 4;
            this.btAllToPool.Text = ">>";
            this.btAllToPool.UseVisualStyleBackColor = true;
            this.btAllToPool.Click += new System.EventHandler(this.btAllToPool_Click);
            // 
            // listPanel
            // 
            this.listPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listPanel.Controls.Add(this.lbOwn);
            this.listPanel.Controls.Add(this.btAllToPool);
            this.listPanel.Controls.Add(this.btSelectedToOwn);
            this.listPanel.Controls.Add(this.lbPool);
            this.listPanel.Controls.Add(this.btAllToOwn);
            this.listPanel.Controls.Add(this.btSelectedToPool);
            this.listPanel.Location = new System.Drawing.Point(7, 7);
            this.listPanel.Margin = new System.Windows.Forms.Padding(0);
            this.listPanel.Name = "listPanel";
            this.listPanel.Size = new System.Drawing.Size(291, 243);
            this.listPanel.TabIndex = 11;
            // 
            // btLists
            // 
            this.btLists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btLists.FlatAppearance.CheckedBackColor = SystemColors.ControlLight;
            this.btLists.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLists.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btLists.Location = new System.Drawing.Point(11, 245);
            this.btLists.Name = "btLists";
            this.btLists.Size = new System.Drawing.Size(67, 24);
            this.btLists.TabIndex = 14;
            this.btLists.Text = "&Lists";
            this.btLists.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btLists.UseVisualStyleBackColor = true;
            this.btLists.Click += new System.EventHandler(this.btLists_Click);
            // 
            // btCheck
            // 
            this.btCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCheck.FlatAppearance.CheckedBackColor = SystemColors.ControlLight;
            this.btCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btCheck.Location = new System.Drawing.Point(79, 245);
            this.btCheck.Name = "btCheck";
            this.btCheck.Size = new System.Drawing.Size(67, 24);
            this.btCheck.TabIndex = 15;
            this.btCheck.Text = "&Check";
            this.btCheck.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btCheck.UseVisualStyleBackColor = true;
            this.btCheck.Click += new System.EventHandler(this.btCheck_Click);
            // 
            // btText
            // 
            this.btText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btText.FlatAppearance.CheckedBackColor = SystemColors.ControlLight;
            this.btText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btText.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btText.Location = new System.Drawing.Point(147, 245);
            this.btText.Name = "btText";
            this.btText.Size = new System.Drawing.Size(67, 24);
            this.btText.TabIndex = 16;
            this.btText.Text = "&Text";
            this.btText.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btText.UseVisualStyleBackColor = true;
            this.btText.Click += new System.EventHandler(this.btText_Click);
            // 
            // text
            // 
            this.text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.text.Location = new System.Drawing.Point(7, 7);
            this.text.Multiline = true;
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(291, 243);
            this.text.TabIndex = 17;
            // 
            // lbCheckList
            // 
            this.lbCheckList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCheckList.FormattingEnabled = true;
            this.lbCheckList.IntegralHeight = false;
            this.lbCheckList.Location = new System.Drawing.Point(7, 7);
            this.lbCheckList.MultiColumn = true;
            this.lbCheckList.Name = "lbCheckList";
            this.lbCheckList.Size = new System.Drawing.Size(291, 243);
            this.lbCheckList.Sorted = true;
            this.lbCheckList.TabIndex = 0;
            this.lbCheckList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbCheckList_ItemCheck);
            // 
            // ListSelectorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.text);
            this.Controls.Add(this.lbCheckList);
            this.Controls.Add(this.listPanel);
            this.Controls.Add(this.btLists);
            this.Controls.Add(this.btCheck);
            this.Controls.Add(this.btText);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ListSelectorControl";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(308, 278);
            this.listPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private ListBox lbOwn;
        private ListBox lbPool;
        private Button btAllToOwn;
        private Button btSelectedToOwn;
        private Button btSelectedToPool;
        private Button btAllToPool;
        private CheckedListBoxEx lbCheckList;
        private Panel listPanel;
        private Button btLists;
        private Button btCheck;
        private Button btText;
        private TextBox text;
    }
}
