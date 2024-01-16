using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
    public partial class QuestionDialog
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (iconBox.BackgroundImage != null)
				{
					iconBox.BackgroundImage.Dispose();
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.imageBox = new System.Windows.Forms.PictureBox();
            this.chkOption = new cYo.Common.Windows.Forms.WrappingCheckBox();
            this.chkOption2 = new cYo.Common.Windows.Forms.WrappingCheckBox();
            this.buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.flowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            this.buttonFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // iconBox
            // 
            this.iconBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.iconBox.Location = new System.Drawing.Point(12, 12);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(37, 34);
            this.iconBox.TabIndex = 0;
            this.iconBox.TabStop = false;
            // 
            // lblQuestion
            // 
            this.lblQuestion.AutoSize = true;
            this.lblQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestion.Location = new System.Drawing.Point(0, 8);
            this.lblQuestion.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.lblQuestion.MinimumSize = new System.Drawing.Size(300, 0);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(300, 13);
            this.lblQuestion.TabIndex = 1;
            this.lblQuestion.Text = "Question";
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(89, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.AutoSize = true;
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(3, 3);
            this.btOK.MinimumSize = new System.Drawing.Size(80, 0);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 5;
            this.btOK.Text = "&OK";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(0, 76);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(60, 13);
            this.lblDescription.TabIndex = 7;
            this.lblDescription.Text = "Description";
            // 
            // flowPanel
            // 
            this.flowPanel.AutoSize = true;
            this.flowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowPanel.Controls.Add(this.lblQuestion);
            this.flowPanel.Controls.Add(this.imageBox);
            this.flowPanel.Controls.Add(this.lblDescription);
            this.flowPanel.Controls.Add(this.chkOption);
            this.flowPanel.Controls.Add(this.chkOption2);
            this.flowPanel.Controls.Add(this.buttonFlow);
            this.flowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanel.Location = new System.Drawing.Point(55, 12);
            this.flowPanel.MaximumSize = new System.Drawing.Size(350, 0);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(300, 169);
            this.flowPanel.TabIndex = 8;
            // 
            // imageBox
            // 
            this.imageBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.imageBox.Location = new System.Drawing.Point(134, 29);
            this.imageBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(31, 39);
            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageBox.TabIndex = 9;
            this.imageBox.TabStop = false;
            // 
            // chkOption
            // 
            this.chkOption.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkOption.AutoSize = true;
            this.chkOption.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkOption.Location = new System.Drawing.Point(0, 97);
            this.chkOption.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.chkOption.Name = "chkOption";
            this.chkOption.Size = new System.Drawing.Size(116, 17);
            this.chkOption.TabIndex = 10;
            this.chkOption.Text = "Optional Checkbox";
            this.chkOption.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkOption.UseVisualStyleBackColor = true;
            this.chkOption.Visible = false;
            this.chkOption.CheckedChanged += new System.EventHandler(this.chkOption_CheckedChanged);
            // 
            // chkOption2
            // 
            this.chkOption2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkOption2.AutoSize = true;
            this.chkOption2.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkOption2.Location = new System.Drawing.Point(0, 114);
            this.chkOption2.Margin = new System.Windows.Forms.Padding(0);
            this.chkOption2.Name = "chkOption2";
            this.chkOption2.Size = new System.Drawing.Size(125, 17);
            this.chkOption2.TabIndex = 11;
            this.chkOption2.Text = "Optional Checkbox 2";
            this.chkOption2.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkOption2.UseVisualStyleBackColor = true;
            this.chkOption2.Visible = false;
            // 
            // buttonFlow
            // 
            this.buttonFlow.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonFlow.AutoSize = true;
            this.buttonFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonFlow.Controls.Add(this.btOK);
            this.buttonFlow.Controls.Add(this.btCancel);
            this.buttonFlow.Location = new System.Drawing.Point(128, 139);
            this.buttonFlow.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.buttonFlow.Name = "buttonFlow";
            this.buttonFlow.Size = new System.Drawing.Size(172, 30);
            this.buttonFlow.TabIndex = 9;
            this.buttonFlow.WrapContents = false;
            // 
            // QuestionDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(369, 198);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.iconBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuestionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QuestionDialog";
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.flowPanel.ResumeLayout(false);
            this.flowPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            this.buttonFlow.ResumeLayout(false);
            this.buttonFlow.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private PictureBox iconBox;
		private Label lblQuestion;
		private Button btCancel;
		private Button btOK;
		private Label lblDescription;
		private FlowLayoutPanel flowPanel;
		private FlowLayoutPanel buttonFlow;
		private WrappingCheckBox chkOption;
		private PictureBox imageBox;
		private WrappingCheckBox chkOption2;
	}
}
