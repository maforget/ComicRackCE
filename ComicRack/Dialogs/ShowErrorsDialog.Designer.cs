using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;


namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ShowErrorsDialog
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
			lvErrors = new cYo.Common.Windows.Forms.ListViewEx();
			chItem = new System.Windows.Forms.ColumnHeader();
			chProblem = new System.Windows.Forms.ColumnHeader();
			btOk = new System.Windows.Forms.Button();
			SuspendLayout();
			lvErrors.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lvErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2]
			{
				chItem,
				chProblem
			});
			lvErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			lvErrors.Location = new System.Drawing.Point(12, 12);
			lvErrors.Name = "lvErrors";
			lvErrors.Size = new System.Drawing.Size(585, 317);
			lvErrors.TabIndex = 0;
			lvErrors.UseCompatibleStateImageBehavior = false;
			lvErrors.View = System.Windows.Forms.View.Details;
			chItem.Text = "Item";
			chItem.Width = 202;
			chProblem.Text = "Problem";
			chProblem.Width = 348;
			btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOk.Location = new System.Drawing.Point(506, 335);
			btOk.Name = "btOk";
			btOk.Size = new System.Drawing.Size(95, 23);
			btOk.TabIndex = 1;
			btOk.Text = "OK";
			btOk.UseVisualStyleBackColor = true;
			base.AcceptButton = btOk;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btOk;
			base.ClientSize = new System.Drawing.Size(609, 368);
			base.Controls.Add(btOk);
			base.Controls.Add(lvErrors);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(300, 200);
			base.Name = "ShowErrorsDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Errors";
			ResumeLayout(false);
		}
		
		private IContainer components;

		private ListViewEx lvErrors;

		private Button btOk;

		private ColumnHeader chItem;

		private ColumnHeader chProblem;
	}
}
