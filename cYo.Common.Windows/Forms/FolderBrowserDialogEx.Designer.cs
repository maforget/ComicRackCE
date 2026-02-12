using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
    public partial class FolderBrowserDialogEx : FormEx
    {
        private System.ComponentModel.IContainer components;
        private Label lblDescription;
        private Button btCancel;
        private Button btOk;
        private Button btNewFolder;
        private CheckBox chkOption1;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.btNewFolder = new System.Windows.Forms.Button();
            this.chkOption1 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.folderTreeView = new cYo.Common.Windows.Forms.FolderTreeView();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(13, 13);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(359, 34);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Description";
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(297, 459);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btOk
            // 
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(216, 459);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "&Ok";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btNewFolder
            // 
            this.btNewFolder.Location = new System.Drawing.Point(12, 459);
            this.btNewFolder.Name = "btNewFolder";
            this.btNewFolder.Size = new System.Drawing.Size(160, 23);
            this.btNewFolder.TabIndex = 3;
            this.btNewFolder.Text = "Make New Folder";
            this.btNewFolder.UseVisualStyleBackColor = true;
            this.btNewFolder.Click += new System.EventHandler(this.btNewFolder_Click);
            // 
            // chkOption1
            // 
            this.chkOption1.AutoSize = true;
            this.chkOption1.Location = new System.Drawing.Point(12, 436);
            this.chkOption1.Name = "chkOption1";
            this.chkOption1.Size = new System.Drawing.Size(66, 17);
            this.chkOption1.TabIndex = 2;
            this.chkOption1.Text = "Option 1";
            this.chkOption1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.folderTreeView);
            this.panel1.Location = new System.Drawing.Point(12, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 372);
            this.panel1.TabIndex = 6;
            // 
            // folderTreeView
            // 
            this.folderTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.folderTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folderTreeView.FullRowSelect = true;
            this.folderTreeView.HideSelection = false;
            this.folderTreeView.ImageIndex = 0;
            this.folderTreeView.Indent = 15;
            this.folderTreeView.ItemHeight = 18;
            this.folderTreeView.Location = new System.Drawing.Point(0, 0);
            this.folderTreeView.Name = "folderTreeView";
            this.folderTreeView.SelectedImageIndex = 0;
            this.folderTreeView.Size = new System.Drawing.Size(358, 370);
            this.folderTreeView.TabIndex = 1;
            // 
            // FolderBrowserDialogEx
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(384, 494);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkOption1);
            this.Controls.Add(this.btNewFolder);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.lblDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FolderBrowserDialogEx";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Title";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel panel1;
        private FolderTreeView folderTreeView;
    }
}

