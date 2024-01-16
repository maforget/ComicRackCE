using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public partial class SelectItemDialog
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
            this.lblName = new System.Windows.Forms.Label();
            this.cbName = new System.Windows.Forms.ComboBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkOption = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(252, 94);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 5;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(166, 94);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 4;
            this.btOK.Text = "&OK";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // cbName
            // 
            this.cbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbName.FormattingEnabled = true;
            this.cbName.Location = new System.Drawing.Point(12, 40);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(320, 21);
            this.cbName.TabIndex = 1;
            this.cbName.TextChanged += new System.EventHandler(this.NameTextChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 95);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 20);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.NameTextChanged);
            // 
            // chkOption
            // 
            this.chkOption.AutoSize = true;
            this.chkOption.Location = new System.Drawing.Point(12, 67);
            this.chkOption.Name = "chkOption";
            this.chkOption.Size = new System.Drawing.Size(86, 17);
            this.chkOption.TabIndex = 3;
            this.chkOption.Text = "Lorem Ipsum";
            this.chkOption.UseVisualStyleBackColor = true;
            this.chkOption.Visible = false;
            // 
            // SelectItemDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(344, 130);
            this.Controls.Add(this.chkOption);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.cbName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectItemDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Item Caption";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private Button btCancel;
		private Button btOK;
		private Label lblName;
		private ComboBox cbName;
		private TextBox txtName;
		private CheckBox chkOption;
	}
}
