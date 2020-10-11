using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ZoomDialog
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
			lblPercentage = new System.Windows.Forms.Label();
			numPercentage = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)numPercentage).BeginInit();
			SuspendLayout();
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(232, 58);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 3;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(146, 58);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 2;
			btOK.Text = "&OK";
			lblPercentage.AutoSize = true;
			lblPercentage.Location = new System.Drawing.Point(23, 21);
			lblPercentage.Name = "lblPercentage";
			lblPercentage.Size = new System.Drawing.Size(93, 13);
			lblPercentage.TabIndex = 0;
			lblPercentage.Text = "Percentage zoom:";
			numPercentage.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numPercentage.Location = new System.Drawing.Point(146, 19);
			numPercentage.Maximum = new decimal(new int[4]
			{
				800,
				0,
				0,
				0
			});
			numPercentage.Minimum = new decimal(new int[4]
			{
				100,
				0,
				0,
				0
			});
			numPercentage.Name = "numPercentage";
			numPercentage.Size = new System.Drawing.Size(80, 20);
			numPercentage.TabIndex = 1;
			numPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			numPercentage.Value = new decimal(new int[4]
			{
				100,
				0,
				0,
				0
			});
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(320, 92);
			base.Controls.Add(numPercentage);
			base.Controls.Add(lblPercentage);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ZoomDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Custom Zoom";
			((System.ComponentModel.ISupportInitialize)numPercentage).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
		
		private IContainer components;

		private Button btCancel;

		private Button btOK;

		private Label lblPercentage;

		private NumericUpDown numPercentage;
	}
}
