using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;


namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ShowErrorsDialog
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
            this.lvErrors = new cYo.Common.Windows.Forms.ListViewEx();
            this.chItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chProblem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvErrors
            // 
            this.lvErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chItem,
            this.chProblem});
            this.lvErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvErrors.HideSelection = false;
            this.lvErrors.Location = new System.Drawing.Point(12, 12);
            this.lvErrors.Name = "lvErrors";
            this.lvErrors.Size = new System.Drawing.Size(585, 317);
            this.lvErrors.TabIndex = 0;
            this.lvErrors.UseCompatibleStateImageBehavior = false;
            this.lvErrors.View = System.Windows.Forms.View.Details;
            // 
            // chItem
            // 
            this.chItem.Text = "Item";
            this.chItem.Width = 202;
            // 
            // chProblem
            // 
            this.chProblem.Text = "Problem";
            this.chProblem.Width = 348;
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(506, 335);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(95, 23);
            this.btOk.TabIndex = 1;
            this.btOk.Text = "OK";
            this.btOk.UseVisualStyleBackColor = true;
            // 
            // ShowErrorsDialog
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btOk;
            this.ClientSize = new System.Drawing.Size(609, 368);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.lvErrors);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "ShowErrorsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Errors";
            this.ResumeLayout(false);

		}

		private ListViewEx lvErrors;
		private Button btOk;
		private ColumnHeader chItem;
		private ColumnHeader chProblem;
	}
}
