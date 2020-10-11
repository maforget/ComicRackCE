using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class SaveWorkspaceDialog
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
			txtName = new System.Windows.Forms.TextBox();
			lblName = new System.Windows.Forms.Label();
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			chkWindowLayouts = new System.Windows.Forms.CheckBox();
			labelSettings = new System.Windows.Forms.Label();
			chkListLayouts = new System.Windows.Forms.CheckBox();
			chkComicDisplayLayout = new System.Windows.Forms.CheckBox();
			chkComicDisplaySettings = new System.Windows.Forms.CheckBox();
			SuspendLayout();
			txtName.Location = new System.Drawing.Point(73, 30);
			txtName.Name = "txtName";
			txtName.Size = new System.Drawing.Size(260, 20);
			txtName.TabIndex = 1;
			txtName.TextChanged += new System.EventHandler(ValidateData);
			lblName.AutoSize = true;
			lblName.Location = new System.Drawing.Point(29, 33);
			lblName.Name = "lblName";
			lblName.Size = new System.Drawing.Size(38, 13);
			lblName.TabIndex = 0;
			lblName.Text = "Name:";
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(270, 163);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 8;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(184, 163);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 7;
			btOK.Text = "&OK";
			chkWindowLayouts.AutoSize = true;
			chkWindowLayouts.Location = new System.Drawing.Point(32, 97);
			chkWindowLayouts.Name = "chkWindowLayouts";
			chkWindowLayouts.Size = new System.Drawing.Size(105, 17);
			chkWindowLayouts.TabIndex = 3;
			chkWindowLayouts.Text = "Window Layouts";
			chkWindowLayouts.UseVisualStyleBackColor = true;
			chkWindowLayouts.CheckedChanged += new System.EventHandler(ValidateData);
			labelSettings.AutoSize = true;
			labelSettings.Location = new System.Drawing.Point(29, 71);
			labelSettings.Name = "labelSettings";
			labelSettings.Size = new System.Drawing.Size(181, 13);
			labelSettings.TabIndex = 2;
			labelSettings.Text = "This workspace includes settings for:";
			chkListLayouts.AutoSize = true;
			chkListLayouts.Location = new System.Drawing.Point(32, 120);
			chkListLayouts.Name = "chkListLayouts";
			chkListLayouts.Size = new System.Drawing.Size(82, 17);
			chkListLayouts.TabIndex = 4;
			chkListLayouts.Text = "List Layouts";
			chkListLayouts.UseVisualStyleBackColor = true;
			chkListLayouts.CheckedChanged += new System.EventHandler(ValidateData);
			chkComicDisplayLayout.AutoSize = true;
			chkComicDisplayLayout.Location = new System.Drawing.Point(167, 97);
			chkComicDisplayLayout.Name = "chkComicDisplayLayout";
			chkComicDisplayLayout.Size = new System.Drawing.Size(127, 17);
			chkComicDisplayLayout.TabIndex = 5;
			chkComicDisplayLayout.Text = "Book Display Layout";
			chkComicDisplayLayout.UseVisualStyleBackColor = true;
			chkComicDisplayLayout.CheckedChanged += new System.EventHandler(ValidateData);
			chkComicDisplaySettings.AutoSize = true;
			chkComicDisplaySettings.Location = new System.Drawing.Point(167, 120);
			chkComicDisplaySettings.Name = "chkComicDisplaySettings";
			chkComicDisplaySettings.Size = new System.Drawing.Size(133, 17);
			chkComicDisplaySettings.TabIndex = 6;
			chkComicDisplaySettings.Text = "Book Display Settings";
			chkComicDisplaySettings.UseVisualStyleBackColor = true;
			chkComicDisplaySettings.CheckedChanged += new System.EventHandler(ValidateData);
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(362, 199);
			base.Controls.Add(chkComicDisplaySettings);
			base.Controls.Add(chkComicDisplayLayout);
			base.Controls.Add(chkListLayouts);
			base.Controls.Add(labelSettings);
			base.Controls.Add(chkWindowLayouts);
			base.Controls.Add(txtName);
			base.Controls.Add(lblName);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SaveWorkspaceDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Save Workspace";
			ResumeLayout(false);
			PerformLayout();
		}
		
		private IContainer components;

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
