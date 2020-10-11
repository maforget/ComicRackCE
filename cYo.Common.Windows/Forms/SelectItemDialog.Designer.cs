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
			lblName = new System.Windows.Forms.Label();
			cbName = new System.Windows.Forms.ComboBox();
			txtName = new System.Windows.Forms.TextBox();
			chkOption = new System.Windows.Forms.CheckBox();
			SuspendLayout();
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(252, 94);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 5;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(166, 94);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 4;
			btOK.Text = "&OK";
			lblName.AutoSize = true;
			lblName.Location = new System.Drawing.Point(12, 24);
			lblName.Name = "lblName";
			lblName.Size = new System.Drawing.Size(38, 13);
			lblName.TabIndex = 0;
			lblName.Text = "Name:";
			cbName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cbName.FormattingEnabled = true;
			cbName.Location = new System.Drawing.Point(12, 40);
			cbName.Name = "cbName";
			cbName.Size = new System.Drawing.Size(320, 21);
			cbName.TabIndex = 1;
			cbName.TextChanged += new System.EventHandler(NameTextChanged);
			txtName.Location = new System.Drawing.Point(12, 95);
			txtName.Name = "txtName";
			txtName.Size = new System.Drawing.Size(100, 20);
			txtName.TabIndex = 2;
			txtName.TextChanged += new System.EventHandler(NameTextChanged);
			chkOption.AutoSize = true;
			chkOption.Location = new System.Drawing.Point(12, 67);
			chkOption.Name = "chkOption";
			chkOption.Size = new System.Drawing.Size(86, 17);
			chkOption.TabIndex = 3;
			chkOption.Text = "Lorem Ipsum";
			chkOption.UseVisualStyleBackColor = true;
			chkOption.Visible = false;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(344, 130);
			base.Controls.Add(chkOption);
			base.Controls.Add(txtName);
			base.Controls.Add(cbName);
			base.Controls.Add(lblName);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AddItemDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Add Item Caption";
			ResumeLayout(false);
			PerformLayout();
		}

		private IContainer components;

		private Button btCancel;

		private Button btOK;

		private Label lblName;

		private ComboBox cbName;

		private TextBox txtName;

		private CheckBox chkOption;
	}
}
