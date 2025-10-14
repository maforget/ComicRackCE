using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ValueEditorDialog
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
            this.panelMatchValue = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rtfMatchValue = new System.Windows.Forms.RichTextBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btInsertValue = new System.Windows.Forms.Button();
            this.panelMatchValue.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMatchValue
            // 
            this.panelMatchValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMatchValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMatchValue.Controls.Add(this.panel1);
            this.panelMatchValue.Location = new System.Drawing.Point(2, 3);
            this.panelMatchValue.Name = "panelMatchValue";
            this.panelMatchValue.Size = new System.Drawing.Size(375, 76);
            this.panelMatchValue.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BackColor = SystemColors.Window;
            this.panel1.Controls.Add(this.rtfMatchValue);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.panel1.Size = new System.Drawing.Size(373, 74);
            this.panel1.TabIndex = 0;
            // 
            // rtfMatchValue
            // 
            this.rtfMatchValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtfMatchValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfMatchValue.Location = new System.Drawing.Point(1, 2);
            this.rtfMatchValue.Multiline = false;
            this.rtfMatchValue.Name = "rtfMatchValue";
            this.rtfMatchValue.Size = new System.Drawing.Size(371, 70);
            this.rtfMatchValue.TabIndex = 0;
            this.rtfMatchValue.Text = "";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(211, 85);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "&OK";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(297, 85);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "&Cancel";
            // 
            // btInsertValue
            // 
            this.btInsertValue.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
            this.btInsertValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btInsertValue.Location = new System.Drawing.Point(4, 85);
            this.btInsertValue.Name = "btInsertValue";
            this.btInsertValue.Size = new System.Drawing.Size(113, 23);
            this.btInsertValue.TabIndex = 5;
            this.btInsertValue.Text = "Insert Value";
            this.btInsertValue.UseVisualStyleBackColor = true;
            // 
            // ValueEditorDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(380, 112);
            this.Controls.Add(this.btInsertValue);
            this.Controls.Add(this.panelMatchValue);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ValueEditorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Match Value";
            this.panelMatchValue.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		
		private Panel panelMatchValue;
		private Panel panel1;
		private RichTextBox rtfMatchValue;
		private Button btOK;
		private Button btCancel;
		private Button btInsertValue;
	}
}
