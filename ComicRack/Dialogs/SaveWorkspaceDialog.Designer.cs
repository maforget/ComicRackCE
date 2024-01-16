using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class SaveWorkspaceDialog
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.chkWindowLayouts = new System.Windows.Forms.CheckBox();
            this.labelSettings = new System.Windows.Forms.Label();
            this.chkListLayouts = new System.Windows.Forms.CheckBox();
            this.chkComicDisplayLayout = new System.Windows.Forms.CheckBox();
            this.chkComicDisplaySettings = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(73, 30);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(260, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.ValidateData);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(29, 33);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(270, 163);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 8;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(184, 163);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 7;
            this.btOK.Text = "&OK";
            // 
            // chkWindowLayouts
            // 
            this.chkWindowLayouts.AutoSize = true;
            this.chkWindowLayouts.Location = new System.Drawing.Point(32, 97);
            this.chkWindowLayouts.Name = "chkWindowLayouts";
            this.chkWindowLayouts.Size = new System.Drawing.Size(105, 17);
            this.chkWindowLayouts.TabIndex = 3;
            this.chkWindowLayouts.Text = "Window Layouts";
            this.chkWindowLayouts.UseVisualStyleBackColor = true;
            this.chkWindowLayouts.CheckedChanged += new System.EventHandler(this.ValidateData);
            // 
            // labelSettings
            // 
            this.labelSettings.AutoSize = true;
            this.labelSettings.Location = new System.Drawing.Point(29, 71);
            this.labelSettings.Name = "labelSettings";
            this.labelSettings.Size = new System.Drawing.Size(181, 13);
            this.labelSettings.TabIndex = 2;
            this.labelSettings.Text = "This workspace includes settings for:";
            // 
            // chkListLayouts
            // 
            this.chkListLayouts.AutoSize = true;
            this.chkListLayouts.Location = new System.Drawing.Point(32, 120);
            this.chkListLayouts.Name = "chkListLayouts";
            this.chkListLayouts.Size = new System.Drawing.Size(82, 17);
            this.chkListLayouts.TabIndex = 4;
            this.chkListLayouts.Text = "List Layouts";
            this.chkListLayouts.UseVisualStyleBackColor = true;
            this.chkListLayouts.CheckedChanged += new System.EventHandler(this.ValidateData);
            // 
            // chkComicDisplayLayout
            // 
            this.chkComicDisplayLayout.AutoSize = true;
            this.chkComicDisplayLayout.Location = new System.Drawing.Point(167, 97);
            this.chkComicDisplayLayout.Name = "chkComicDisplayLayout";
            this.chkComicDisplayLayout.Size = new System.Drawing.Size(123, 17);
            this.chkComicDisplayLayout.TabIndex = 5;
            this.chkComicDisplayLayout.Text = "Book Display Layout";
            this.chkComicDisplayLayout.UseVisualStyleBackColor = true;
            this.chkComicDisplayLayout.CheckedChanged += new System.EventHandler(this.ValidateData);
            // 
            // chkComicDisplaySettings
            // 
            this.chkComicDisplaySettings.AutoSize = true;
            this.chkComicDisplaySettings.Location = new System.Drawing.Point(167, 120);
            this.chkComicDisplaySettings.Name = "chkComicDisplaySettings";
            this.chkComicDisplaySettings.Size = new System.Drawing.Size(129, 17);
            this.chkComicDisplaySettings.TabIndex = 6;
            this.chkComicDisplaySettings.Text = "Book Display Settings";
            this.chkComicDisplaySettings.UseVisualStyleBackColor = true;
            this.chkComicDisplaySettings.CheckedChanged += new System.EventHandler(this.ValidateData);
            // 
            // SaveWorkspaceDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(362, 199);
            this.Controls.Add(this.chkComicDisplaySettings);
            this.Controls.Add(this.chkComicDisplayLayout);
            this.Controls.Add(this.chkListLayouts);
            this.Controls.Add(this.labelSettings);
            this.Controls.Add(this.chkWindowLayouts);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveWorkspaceDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save Workspace";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		
		private TextBox txtName;
		private Label lblName;
		private Button btCancel;
		private Button btOK;
		private CheckBox chkWindowLayouts;
		private Label labelSettings;
		private CheckBox chkListLayouts;
		private CheckBox chkComicDisplayLayout;
		private CheckBox chkComicDisplaySettings;
	}
}
