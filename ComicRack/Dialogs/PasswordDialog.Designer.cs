using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class PasswordDialog
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
			lblDescription = new System.Windows.Forms.Label();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			panel1 = new System.Windows.Forms.Panel();
			chkRemember = new System.Windows.Forms.CheckBox();
			labelPassword = new System.Windows.Forms.Label();
			txPassword = new System.Windows.Forms.TextBox();
			flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			btOK = new System.Windows.Forms.Button();
			btCancel = new System.Windows.Forms.Button();
			flowLayoutPanel1.SuspendLayout();
			panel1.SuspendLayout();
			flowLayoutPanel2.SuspendLayout();
			SuspendLayout();
			lblDescription.AutoSize = true;
			lblDescription.Location = new System.Drawing.Point(8, 8);
			lblDescription.Margin = new System.Windows.Forms.Padding(4);
			lblDescription.Name = "lblDescription";
			lblDescription.Size = new System.Drawing.Size(212, 13);
			lblDescription.TabIndex = 0;
			lblDescription.Text = "A password is needed for the remote library:";
			flowLayoutPanel1.AutoSize = true;
			flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowLayoutPanel1.Controls.Add(lblDescription);
			flowLayoutPanel1.Controls.Add(panel1);
			flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
			flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			flowLayoutPanel1.MaximumSize = new System.Drawing.Size(310, 0);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(4);
			flowLayoutPanel1.Size = new System.Drawing.Size(309, 132);
			flowLayoutPanel1.TabIndex = 0;
			panel1.Controls.Add(chkRemember);
			panel1.Controls.Add(labelPassword);
			panel1.Controls.Add(txPassword);
			panel1.Location = new System.Drawing.Point(8, 29);
			panel1.Margin = new System.Windows.Forms.Padding(4);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(293, 59);
			panel1.TabIndex = 1;
			chkRemember.AutoSize = true;
			chkRemember.Location = new System.Drawing.Point(62, 33);
			chkRemember.Name = "chkRemember";
			chkRemember.Size = new System.Drawing.Size(143, 17);
			chkRemember.TabIndex = 2;
			chkRemember.Text = "Remember for this server";
			chkRemember.UseVisualStyleBackColor = true;
			labelPassword.AutoSize = true;
			labelPassword.Location = new System.Drawing.Point(0, 10);
			labelPassword.Name = "labelPassword";
			labelPassword.Size = new System.Drawing.Size(56, 13);
			labelPassword.TabIndex = 0;
			labelPassword.Text = "Password:";
			txPassword.Location = new System.Drawing.Point(62, 7);
			txPassword.Name = "txPassword";
			txPassword.Size = new System.Drawing.Size(224, 20);
			txPassword.TabIndex = 1;
			txPassword.UseSystemPasswordChar = true;
			flowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			flowLayoutPanel2.AutoSize = true;
			flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowLayoutPanel2.Controls.Add(btOK);
			flowLayoutPanel2.Controls.Add(btCancel);
			flowLayoutPanel2.Location = new System.Drawing.Point(130, 95);
			flowLayoutPanel2.Name = "flowLayoutPanel2";
			flowLayoutPanel2.Size = new System.Drawing.Size(172, 30);
			flowLayoutPanel2.TabIndex = 9;
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(3, 3);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 0;
			btOK.Text = "&OK";
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(89, 3);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 1;
			btCancel.Text = "&Cancel";
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(315, 138);
			base.Controls.Add(flowLayoutPanel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "PasswordDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Password";
			flowLayoutPanel1.ResumeLayout(false);
			flowLayoutPanel1.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			flowLayoutPanel2.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
		
		private IContainer components;

		private Label lblDescription;

		private FlowLayoutPanel flowLayoutPanel1;

		private Panel panel1;

		private CheckBox chkRemember;

		private Label labelPassword;

		private TextBox txPassword;

		private FlowLayoutPanel flowLayoutPanel2;

		private Button btOK;

		private Button btCancel;
	}
}
