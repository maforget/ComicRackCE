using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ValueEditorDialog
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
			panelMatchValue = new System.Windows.Forms.Panel();
			panel1 = new System.Windows.Forms.Panel();
			rtfMatchValue = new System.Windows.Forms.RichTextBox();
			btOK = new System.Windows.Forms.Button();
			btCancel = new System.Windows.Forms.Button();
			btInsertValue = new System.Windows.Forms.Button();
			panelMatchValue.SuspendLayout();
			panel1.SuspendLayout();
			SuspendLayout();
			panelMatchValue.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			panelMatchValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			panelMatchValue.Controls.Add(panel1);
			panelMatchValue.Location = new System.Drawing.Point(2, 3);
			panelMatchValue.Name = "panelMatchValue";
			panelMatchValue.Size = new System.Drawing.Size(375, 76);
			panelMatchValue.TabIndex = 4;
			panel1.BackColor = System.Drawing.SystemColors.Window;
			panel1.Controls.Add(rtfMatchValue);
			panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			panel1.Location = new System.Drawing.Point(0, 0);
			panel1.Name = "panel1";
			panel1.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
			panel1.Size = new System.Drawing.Size(373, 74);
			panel1.TabIndex = 0;
			rtfMatchValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
			rtfMatchValue.Dock = System.Windows.Forms.DockStyle.Fill;
			rtfMatchValue.Location = new System.Drawing.Point(1, 2);
			rtfMatchValue.Multiline = false;
			rtfMatchValue.Name = "rtfMatchValue";
			rtfMatchValue.Size = new System.Drawing.Size(371, 70);
			rtfMatchValue.TabIndex = 0;
			rtfMatchValue.Text = "";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(211, 85);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 1;
			btOK.Text = "&OK";
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(297, 85);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 2;
			btCancel.Text = "&Cancel";
			btInsertValue.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
			btInsertValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			btInsertValue.Location = new System.Drawing.Point(4, 85);
			btInsertValue.Name = "btInsertValue";
			btInsertValue.Size = new System.Drawing.Size(113, 23);
			btInsertValue.TabIndex = 5;
			btInsertValue.Text = "Insert Value";
			btInsertValue.UseVisualStyleBackColor = true;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(380, 112);
			base.Controls.Add(btInsertValue);
			base.Controls.Add(panelMatchValue);
			base.Controls.Add(btOK);
			base.Controls.Add(btCancel);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "ValueEditorDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Match Value";
			panelMatchValue.ResumeLayout(false);
			panel1.ResumeLayout(false);
			ResumeLayout(false);
		}
		
		private IContainer components;

		private Panel panelMatchValue;

		private Panel panel1;

		private RichTextBox rtfMatchValue;

		private Button btOK;

		private Button btCancel;

		private Button btInsertValue;
	}
}
