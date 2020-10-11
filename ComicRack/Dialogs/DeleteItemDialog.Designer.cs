using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class DeleteItemDialog
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
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			lbCaption = new System.Windows.Forms.Label();
			cbItems = new System.Windows.Forms.ComboBox();
			SuspendLayout();
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(252, 73);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 3;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(166, 73);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 2;
			btOK.Text = "&OK";
			lbCaption.AutoSize = true;
			lbCaption.Location = new System.Drawing.Point(12, 23);
			lbCaption.Name = "lbCaption";
			lbCaption.Size = new System.Drawing.Size(24, 13);
			lbCaption.TabIndex = 0;
			lbCaption.Text = "{0}:";
			cbItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbItems.FormattingEnabled = true;
			cbItems.Location = new System.Drawing.Point(15, 39);
			cbItems.MaxDropDownItems = 10;
			cbItems.Name = "cbItems";
			cbItems.Size = new System.Drawing.Size(312, 21);
			cbItems.TabIndex = 1;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(344, 109);
			base.Controls.Add(cbItems);
			base.Controls.Add(lbCaption);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DeleteItemDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Delete Item {0}";
			ResumeLayout(false);
			PerformLayout();
		}
		
		private IContainer components;

		private Button btCancel;

		private Button btOK;

		private Label lbCaption;

		private ComboBox cbItems;
	}
}
