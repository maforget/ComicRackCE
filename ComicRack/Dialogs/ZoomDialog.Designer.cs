using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ZoomDialog
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.lblPercentage = new System.Windows.Forms.Label();
            this.numPercentage = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numPercentage)).BeginInit();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(232, 58);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(146, 58);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "&OK";
            // 
            // lblPercentage
            // 
            this.lblPercentage.AutoSize = true;
            this.lblPercentage.Location = new System.Drawing.Point(23, 21);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(93, 13);
            this.lblPercentage.TabIndex = 0;
            this.lblPercentage.Text = "Percentage zoom:";
            // 
            // numPercentage
            // 
            this.numPercentage.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPercentage.Location = new System.Drawing.Point(146, 19);
            this.numPercentage.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.numPercentage.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numPercentage.Name = "numPercentage";
            this.numPercentage.Size = new System.Drawing.Size(80, 20);
            this.numPercentage.TabIndex = 1;
            this.numPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPercentage.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // ZoomDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(320, 92);
            this.Controls.Add(this.numPercentage);
            this.Controls.Add(this.lblPercentage);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ZoomDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Zoom";
            ((System.ComponentModel.ISupportInitialize)(this.numPercentage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		
		private Button btCancel;
		private Button btOK;
		private Label lblPercentage;
		private NumericUpDown numPercentage;
	}
}
