using cYo.Common.Windows.Forms;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ListEditorDialog
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
			lvItems = new cYo.Common.Windows.Forms.ListViewEx();
			chName = new System.Windows.Forms.ColumnHeader();
			chDescription = new System.Windows.Forms.ColumnHeader();
			btMoveDown = new System.Windows.Forms.Button();
			btMoveUp = new System.Windows.Forms.Button();
			btDelete = new System.Windows.Forms.Button();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			btNew = new System.Windows.Forms.Button();
			btEdit = new System.Windows.Forms.Button();
			btActivate = new System.Windows.Forms.Button();
			btSetAll = new System.Windows.Forms.Button();
			btMoveTop = new System.Windows.Forms.Button();
			btMoveBottom = new System.Windows.Forms.Button();
			flowLayoutPanel1.SuspendLayout();
			SuspendLayout();
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(407, 353);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(94, 24);
			btCancel.TabIndex = 3;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(407, 323);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(94, 24);
			btOK.TabIndex = 2;
			btOK.Text = "&OK";
			lvItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2]
			{
				chName,
				chDescription
			});
			lvItems.EnableMouseReorder = true;
			lvItems.FullRowSelect = true;
			lvItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			lvItems.HideSelection = false;
			lvItems.Location = new System.Drawing.Point(12, 15);
			lvItems.MultiSelect = false;
			lvItems.Name = "lvItems";
			lvItems.Size = new System.Drawing.Size(389, 361);
			lvItems.TabIndex = 0;
			lvItems.UseCompatibleStateImageBehavior = false;
			lvItems.View = System.Windows.Forms.View.Details;
			lvItems.MouseReorder += new System.EventHandler<cYo.Common.Windows.Forms.ListViewEx.MouseReorderEventArgs>(lvItems_MouseReorder);
			lvItems.SelectedIndexChanged += new System.EventHandler(lvItems_SelectedIndexChanged);
			lvItems.DoubleClick += new System.EventHandler(lvItems_DoubleClick);
			chName.Text = "Name";
			chName.Width = 154;
			chDescription.Text = "Description";
			chDescription.Width = 202;
			btMoveDown.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btMoveDown.Location = new System.Drawing.Point(3, 224);
			btMoveDown.Name = "btMoveDown";
			btMoveDown.Size = new System.Drawing.Size(94, 23);
			btMoveDown.TabIndex = 6;
			btMoveDown.Text = "Move &Down";
			btMoveDown.UseVisualStyleBackColor = true;
			btMoveDown.Click += new System.EventHandler(btMoveDown_Click);
			btMoveUp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btMoveUp.Location = new System.Drawing.Point(3, 195);
			btMoveUp.Name = "btMoveUp";
			btMoveUp.Size = new System.Drawing.Size(94, 23);
			btMoveUp.TabIndex = 5;
			btMoveUp.Text = "Move &Up";
			btMoveUp.UseVisualStyleBackColor = true;
			btMoveUp.Click += new System.EventHandler(btMoveUp_Click);
			btDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btDelete.Location = new System.Drawing.Point(3, 61);
			btDelete.Name = "btDelete";
			btDelete.Size = new System.Drawing.Size(94, 23);
			btDelete.TabIndex = 3;
			btDelete.Text = "D&elete";
			btDelete.UseVisualStyleBackColor = true;
			btDelete.Click += new System.EventHandler(btDelete_Click);
			flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			flowLayoutPanel1.Controls.Add(btNew);
			flowLayoutPanel1.Controls.Add(btEdit);
			flowLayoutPanel1.Controls.Add(btDelete);
			flowLayoutPanel1.Controls.Add(btActivate);
			flowLayoutPanel1.Controls.Add(btSetAll);
			flowLayoutPanel1.Controls.Add(btMoveTop);
			flowLayoutPanel1.Controls.Add(btMoveUp);
			flowLayoutPanel1.Controls.Add(btMoveDown);
			flowLayoutPanel1.Controls.Add(btMoveBottom);
			flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowLayoutPanel1.Location = new System.Drawing.Point(404, 13);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new System.Drawing.Size(101, 304);
			flowLayoutPanel1.TabIndex = 1;
			btNew.Location = new System.Drawing.Point(3, 3);
			btNew.Name = "btNew";
			btNew.Size = new System.Drawing.Size(94, 23);
			btNew.TabIndex = 0;
			btNew.Text = "&New...";
			btNew.UseVisualStyleBackColor = true;
			btNew.Visible = false;
			btNew.Click += new System.EventHandler(btNew_Click);
			btEdit.Location = new System.Drawing.Point(3, 32);
			btEdit.Name = "btEdit";
			btEdit.Size = new System.Drawing.Size(94, 23);
			btEdit.TabIndex = 1;
			btEdit.Text = "&Edit...";
			btEdit.UseVisualStyleBackColor = true;
			btEdit.Visible = false;
			btEdit.Click += new System.EventHandler(btEdit_Click);
			btActivate.Location = new System.Drawing.Point(3, 99);
			btActivate.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
			btActivate.Name = "btActivate";
			btActivate.Size = new System.Drawing.Size(94, 23);
			btActivate.TabIndex = 2;
			btActivate.Text = "&Activate";
			btActivate.UseVisualStyleBackColor = true;
			btActivate.Visible = false;
			btActivate.Click += new System.EventHandler(btActivate_Click);
			btSetAll.Location = new System.Drawing.Point(3, 128);
			btSetAll.Name = "btSetAll";
			btSetAll.Size = new System.Drawing.Size(94, 23);
			btSetAll.TabIndex = 8;
			btSetAll.Text = "&Set to All";
			btSetAll.UseVisualStyleBackColor = true;
			btSetAll.Visible = false;
			btSetAll.Click += new System.EventHandler(btSetAll_Click);
			btMoveTop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btMoveTop.Location = new System.Drawing.Point(3, 166);
			btMoveTop.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
			btMoveTop.Name = "btMoveTop";
			btMoveTop.Size = new System.Drawing.Size(94, 23);
			btMoveTop.TabIndex = 4;
			btMoveTop.Text = "Move &Top";
			btMoveTop.UseVisualStyleBackColor = true;
			btMoveTop.Click += new System.EventHandler(btMoveTop_Click);
			btMoveBottom.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btMoveBottom.Location = new System.Drawing.Point(3, 253);
			btMoveBottom.Name = "btMoveBottom";
			btMoveBottom.Size = new System.Drawing.Size(94, 23);
			btMoveBottom.TabIndex = 7;
			btMoveBottom.Text = "Move &Bottom";
			btMoveBottom.UseVisualStyleBackColor = true;
			btMoveBottom.Click += new System.EventHandler(btMoveBottom_Click);
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(513, 388);
			base.Controls.Add(flowLayoutPanel1);
			base.Controls.Add(lvItems);
			base.Controls.Add(btOK);
			base.Controls.Add(btCancel);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(400, 390);
			base.Name = "ListEditorDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "ListEditorDialog";
			flowLayoutPanel1.ResumeLayout(false);
			ResumeLayout(false);
		}
		
		private IContainer components;

		private Button btCancel;

		private Button btOK;

		private ListViewEx lvItems;

		private ColumnHeader chName;

		private ColumnHeader chDescription;

		private Button btMoveDown;

		private Button btMoveUp;

		private Button btDelete;

		private FlowLayoutPanel flowLayoutPanel1;

		private Button btNew;

		private Button btEdit;

		private Button btMoveTop;

		private Button btMoveBottom;

		private Button btActivate;

		private Button btSetAll;
	}
}
