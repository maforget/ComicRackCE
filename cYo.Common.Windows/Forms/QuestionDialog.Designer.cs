using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
    public partial class QuestionDialog
	{
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
			iconBox = new System.Windows.Forms.PictureBox();
			lblQuestion = new System.Windows.Forms.Label();
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			lblDescription = new System.Windows.Forms.Label();
			flowPanel = new System.Windows.Forms.FlowLayoutPanel();
			imageBox = new System.Windows.Forms.PictureBox();
			chkOption = new cYo.Common.Windows.Forms.WrappingCheckBox();
			chkOption2 = new cYo.Common.Windows.Forms.WrappingCheckBox();
			buttonFlow = new System.Windows.Forms.FlowLayoutPanel();
			((System.ComponentModel.ISupportInitialize)iconBox).BeginInit();
			flowPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)imageBox).BeginInit();
			buttonFlow.SuspendLayout();
			SuspendLayout();
			iconBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			iconBox.Location = new System.Drawing.Point(12, 12);
			iconBox.Name = "iconBox";
			iconBox.Size = new System.Drawing.Size(37, 34);
			iconBox.TabIndex = 0;
			iconBox.TabStop = false;
			lblQuestion.AutoSize = true;
			lblQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblQuestion.Location = new System.Drawing.Point(0, 8);
			lblQuestion.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
			lblQuestion.MinimumSize = new System.Drawing.Size(300, 0);
			lblQuestion.Name = "lblQuestion";
			lblQuestion.Size = new System.Drawing.Size(300, 13);
			lblQuestion.TabIndex = 1;
			lblQuestion.Text = "Question";
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(89, 3);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 6;
			btCancel.Text = "&Cancel";
			btOK.AutoSize = true;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(3, 3);
			btOK.MinimumSize = new System.Drawing.Size(80, 0);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 5;
			btOK.Text = "&OK";
			lblDescription.AutoSize = true;
			lblDescription.Location = new System.Drawing.Point(0, 76);
			lblDescription.Margin = new System.Windows.Forms.Padding(0);
			lblDescription.Name = "lblDescription";
			lblDescription.Size = new System.Drawing.Size(60, 13);
			lblDescription.TabIndex = 7;
			lblDescription.Text = "Description";
			flowPanel.AutoSize = true;
			flowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowPanel.Controls.Add(lblQuestion);
			flowPanel.Controls.Add(imageBox);
			flowPanel.Controls.Add(lblDescription);
			flowPanel.Controls.Add(chkOption);
			flowPanel.Controls.Add(chkOption2);
			flowPanel.Controls.Add(buttonFlow);
			flowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowPanel.Location = new System.Drawing.Point(55, 12);
			flowPanel.MaximumSize = new System.Drawing.Size(350, 0);
			flowPanel.Name = "flowPanel";
			flowPanel.Size = new System.Drawing.Size(300, 169);
			flowPanel.TabIndex = 8;
			imageBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			imageBox.Location = new System.Drawing.Point(134, 29);
			imageBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
			imageBox.Name = "imageBox";
			imageBox.Size = new System.Drawing.Size(31, 39);
			imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			imageBox.TabIndex = 9;
			imageBox.TabStop = false;
			chkOption.Anchor = System.Windows.Forms.AnchorStyles.Left;
			chkOption.AutoSize = true;
			chkOption.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			chkOption.Location = new System.Drawing.Point(0, 97);
			chkOption.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			chkOption.Name = "chkOption";
			chkOption.Size = new System.Drawing.Size(116, 17);
			chkOption.TabIndex = 10;
			chkOption.Text = "Optional Checkbox";
			chkOption.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			chkOption.UseVisualStyleBackColor = true;
			chkOption.Visible = false;
			chkOption.CheckedChanged += new System.EventHandler(chkOption_CheckedChanged);
			chkOption2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			chkOption2.AutoSize = true;
			chkOption2.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			chkOption2.Location = new System.Drawing.Point(0, 114);
			chkOption2.Margin = new System.Windows.Forms.Padding(0);
			chkOption2.Name = "chkOption2";
			chkOption2.Size = new System.Drawing.Size(125, 17);
			chkOption2.TabIndex = 11;
			chkOption2.Text = "Optional Checkbox 2";
			chkOption2.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			chkOption2.UseVisualStyleBackColor = true;
			chkOption2.Visible = false;
			buttonFlow.Anchor = System.Windows.Forms.AnchorStyles.Right;
			buttonFlow.AutoSize = true;
			buttonFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			buttonFlow.Controls.Add(btOK);
			buttonFlow.Controls.Add(btCancel);
			buttonFlow.Location = new System.Drawing.Point(128, 139);
			buttonFlow.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			buttonFlow.Name = "buttonFlow";
			buttonFlow.Size = new System.Drawing.Size(172, 30);
			buttonFlow.TabIndex = 9;
			buttonFlow.WrapContents = false;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(369, 198);
			base.Controls.Add(flowPanel);
			base.Controls.Add(iconBox);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "QuestionDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "QuestionDialog";
			((System.ComponentModel.ISupportInitialize)iconBox).EndInit();
			flowPanel.ResumeLayout(false);
			flowPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)imageBox).EndInit();
			buttonFlow.ResumeLayout(false);
			buttonFlow.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		private IContainer components;

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
