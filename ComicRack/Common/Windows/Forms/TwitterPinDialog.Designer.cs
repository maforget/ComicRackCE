using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public partial class TwitterPinDialog
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
			labelCaption = new System.Windows.Forms.Label();
			textPin = new System.Windows.Forms.TextBox();
			SuspendLayout();
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(217, 100);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 7;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(131, 100);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 6;
			btOK.Text = "&OK";
			labelCaption.AutoSize = true;
			labelCaption.Location = new System.Drawing.Point(12, 21);
			labelCaption.Name = "labelCaption";
			labelCaption.Size = new System.Drawing.Size(206, 13);
			labelCaption.TabIndex = 8;
			labelCaption.Text = "Please enter the Twitter authorization PIN:";
			textPin.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			textPin.Location = new System.Drawing.Point(15, 50);
			textPin.Name = "textPin";
			textPin.Size = new System.Drawing.Size(282, 38);
			textPin.TabIndex = 9;
			textPin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(309, 136);
			base.Controls.Add(textPin);
			base.Controls.Add(labelCaption);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "TwitterPinDialog";
			base.ShowIcon = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Twitter Authorization";
			ResumeLayout(false);
			PerformLayout();
		}

		private IContainer components;

		private Button btCancel;

		private Button btOK;

		private Label labelCaption;

		private TextBox textPin;
	}
}
