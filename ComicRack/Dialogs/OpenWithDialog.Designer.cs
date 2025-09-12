using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class OpenWithDialog
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
            this.btMoveDown = new System.Windows.Forms.Button();
            this.btMoveUp = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btNew = new System.Windows.Forms.Button();
            this.btEdit = new System.Windows.Forms.Button();
            this.btOverride = new System.Windows.Forms.Button();
            this.btMoveTop = new System.Windows.Forms.Button();
            this.btMoveBottom = new System.Windows.Forms.Button();
            this.lvItems = new cYo.Common.Windows.Forms.ListViewEx();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chOverride = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(546, 397);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(94, 24);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(546, 367);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(94, 24);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "&OK";
            // 
            // btMoveDown
            // 
            this.btMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btMoveDown.Location = new System.Drawing.Point(3, 195);
            this.btMoveDown.Name = "btMoveDown";
            this.btMoveDown.Size = new System.Drawing.Size(94, 23);
            this.btMoveDown.TabIndex = 6;
            this.btMoveDown.Text = "Move &Down";
            this.btMoveDown.UseVisualStyleBackColor = true;
            this.btMoveDown.Click += new System.EventHandler(this.btMoveDown_Click);
            // 
            // btMoveUp
            // 
            this.btMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btMoveUp.Location = new System.Drawing.Point(3, 166);
            this.btMoveUp.Name = "btMoveUp";
            this.btMoveUp.Size = new System.Drawing.Size(94, 23);
            this.btMoveUp.TabIndex = 5;
            this.btMoveUp.Text = "Move &Up";
            this.btMoveUp.UseVisualStyleBackColor = true;
            this.btMoveUp.Click += new System.EventHandler(this.btMoveUp_Click);
            // 
            // btDelete
            // 
            this.btDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDelete.Location = new System.Drawing.Point(3, 61);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(94, 23);
            this.btDelete.TabIndex = 3;
            this.btDelete.Text = "D&elete";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.btNew);
            this.flowLayoutPanel1.Controls.Add(this.btEdit);
            this.flowLayoutPanel1.Controls.Add(this.btDelete);
            this.flowLayoutPanel1.Controls.Add(this.btOverride);
            this.flowLayoutPanel1.Controls.Add(this.btMoveTop);
            this.flowLayoutPanel1.Controls.Add(this.btMoveUp);
            this.flowLayoutPanel1.Controls.Add(this.btMoveDown);
            this.flowLayoutPanel1.Controls.Add(this.btMoveBottom);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(543, 13);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(101, 304);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btNew
            // 
            this.btNew.Location = new System.Drawing.Point(3, 3);
            this.btNew.Name = "btNew";
            this.btNew.Size = new System.Drawing.Size(94, 23);
            this.btNew.TabIndex = 0;
            this.btNew.Text = "&New...";
            this.btNew.UseVisualStyleBackColor = true;
            this.btNew.Visible = false;
            this.btNew.Click += new System.EventHandler(this.btNew_Click);
            // 
            // btEdit
            // 
            this.btEdit.Location = new System.Drawing.Point(3, 32);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(94, 23);
            this.btEdit.TabIndex = 1;
            this.btEdit.Text = "&Edit...";
            this.btEdit.UseVisualStyleBackColor = true;
            this.btEdit.Visible = false;
            this.btEdit.Click += new System.EventHandler(this.btEdit_Click);
            // 
            // btOverride
            // 
            this.btOverride.Location = new System.Drawing.Point(3, 99);
            this.btOverride.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.btOverride.Name = "btOverride";
            this.btOverride.Size = new System.Drawing.Size(94, 23);
            this.btOverride.TabIndex = 2;
            this.btOverride.Text = "&Toggle Override";
            this.btOverride.UseVisualStyleBackColor = true;
            this.btOverride.Visible = false;
            this.btOverride.Click += new System.EventHandler(this.btOverride_Click);
            // 
            // btMoveTop
            // 
            this.btMoveTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btMoveTop.Location = new System.Drawing.Point(3, 137);
            this.btMoveTop.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.btMoveTop.Name = "btMoveTop";
            this.btMoveTop.Size = new System.Drawing.Size(94, 23);
            this.btMoveTop.TabIndex = 4;
            this.btMoveTop.Text = "Move &Top";
            this.btMoveTop.UseVisualStyleBackColor = true;
            this.btMoveTop.Click += new System.EventHandler(this.btMoveTop_Click);
            // 
            // btMoveBottom
            // 
            this.btMoveBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btMoveBottom.Location = new System.Drawing.Point(3, 224);
            this.btMoveBottom.Name = "btMoveBottom";
            this.btMoveBottom.Size = new System.Drawing.Size(94, 23);
            this.btMoveBottom.TabIndex = 7;
            this.btMoveBottom.Text = "Move &Bottom";
            this.btMoveBottom.UseVisualStyleBackColor = true;
            this.btMoveBottom.Click += new System.EventHandler(this.btMoveBottom_Click);
            // 
            // lvItems
            // 
            this.lvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chPath,
            this.chOverride});
            this.lvItems.EnableMouseReorder = true;
            this.lvItems.FullRowSelect = true;
            this.lvItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvItems.HideSelection = false;
            this.lvItems.Location = new System.Drawing.Point(12, 15);
            this.lvItems.MultiSelect = false;
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(528, 405);
            this.lvItems.TabIndex = 0;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.MouseReorder += new System.EventHandler<cYo.Common.Windows.Forms.ListViewEx.MouseReorderEventArgs>(this.lvItems_MouseReorder);
            this.lvItems.SelectedIndexChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
            this.lvItems.DoubleClick += new System.EventHandler(this.lvItems_DoubleClick);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 154;
            // 
            // chPath
            // 
            this.chPath.Text = "Path";
            this.chPath.Width = 275;
            // 
            // chOverride
            // 
            this.chOverride.Text = "Override Reader";
            this.chOverride.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chOverride.Width = 90;
            // 
            // OpenWithDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(652, 432);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.btCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 390);
            this.Name = "OpenWithDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OpenWithDialog";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		
		private Button btCancel;
		private Button btOK;
		private ListViewEx lvItems;
		private ColumnHeader chName;
		private ColumnHeader chPath;
		private Button btMoveDown;
		private Button btMoveUp;
		private Button btDelete;
		private FlowLayoutPanel flowLayoutPanel1;
		private Button btNew;
		private Button btEdit;
		private Button btMoveTop;
		private Button btMoveBottom;
		private Button btOverride;
        private ColumnHeader chOverride;
    }
}
