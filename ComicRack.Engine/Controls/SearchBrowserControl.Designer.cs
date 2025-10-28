using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine.Controls
{
    public partial class SearchBrowserControl : UserControlEx
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                books.Clear();
                books.Changed -= BooksChanged;
                IdleProcess.Idle -= IdleUpdate;
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listView2 = new cYo.Common.Windows.Forms.ListViewEx();
            this.listView3 = new cYo.Common.Windows.Forms.ListViewEx();
            this.listView1 = new cYo.Common.Windows.Forms.ListViewEx();
            this.cbType1 = new System.Windows.Forms.ComboBox();
            this.cbType2 = new System.Windows.Forms.ComboBox();
            this.cbType3 = new System.Windows.Forms.ComboBox();
            this.btNot1 = new System.Windows.Forms.CheckBox();
            this.btNot2 = new System.Windows.Forms.CheckBox();
            this.btNot3 = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // listView2
            // 
            this.listView2.FullRowSelect = true;
            this.listView2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(191, 32);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(178, 162);
            this.listView2.TabIndex = 3;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.VirtualMode = true;
            this.listView2.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView_ItemDrag);
            this.listView2.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListItemSelectionChanged);
            this.listView2.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.ListViewRetrieveVirtualItem);
            this.listView2.VirtualItemsSelectionRangeChanged += new System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventHandler(this.ListViewVirtualItemsSelectionRangeChanged);
            // 
            // listView3
            // 
            this.listView3.FullRowSelect = true;
            this.listView3.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView3.HideSelection = false;
            this.listView3.Location = new System.Drawing.Point(375, 32);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(202, 162);
            this.listView3.TabIndex = 5;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            this.listView3.VirtualMode = true;
            this.listView3.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView_ItemDrag);
            this.listView3.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListItemSelectionChanged);
            this.listView3.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.ListViewRetrieveVirtualItem);
            this.listView3.VirtualItemsSelectionRangeChanged += new System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventHandler(this.ListViewVirtualItemsSelectionRangeChanged);
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(16, 32);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(169, 162);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.VirtualMode = true;
            this.listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView_ItemDrag);
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ListItemSelectionChanged);
            this.listView1.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.ListViewRetrieveVirtualItem);
            this.listView1.VirtualItemsSelectionRangeChanged += new System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventHandler(this.ListViewVirtualItemsSelectionRangeChanged);
            // 
            // cbType1
            // 
            this.cbType1.BackColor = SystemColors.Window;
            this.cbType1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType1.FormattingEnabled = true;
            this.cbType1.Location = new System.Drawing.Point(15, 7);
            this.cbType1.Name = "cbType1";
            this.cbType1.Size = new System.Drawing.Size(145, 21);
            this.cbType1.TabIndex = 0;
            this.cbType1.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // cbType2
            // 
            this.cbType2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType2.FormattingEnabled = true;
            this.cbType2.Location = new System.Drawing.Point(190, 7);
            this.cbType2.Name = "cbType2";
            this.cbType2.Size = new System.Drawing.Size(157, 21);
            this.cbType2.TabIndex = 2;
            this.cbType2.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // cbType3
            // 
            this.cbType3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType3.FormattingEnabled = true;
            this.cbType3.Location = new System.Drawing.Point(375, 7);
            this.cbType3.Name = "cbType3";
            this.cbType3.Size = new System.Drawing.Size(172, 21);
            this.cbType3.TabIndex = 4;
            this.cbType3.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // btNot1
            // 
            this.btNot1.Appearance = System.Windows.Forms.Appearance.Button;
            this.btNot1.AutoSize = true;
            this.btNot1.Location = new System.Drawing.Point(166, 7);
            this.btNot1.Name = "btNot1";
            this.btNot1.Size = new System.Drawing.Size(20, 23);
            this.btNot1.TabIndex = 6;
            this.btNot1.Tag = "0";
            this.btNot1.Text = "!";
            this.btNot1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btNot1.UseVisualStyleBackColor = true;
            this.btNot1.CheckedChanged += new System.EventHandler(this.btNot_CheckedChanged);
            // 
            // btNot2
            // 
            this.btNot2.Appearance = System.Windows.Forms.Appearance.Button;
            this.btNot2.AutoSize = true;
            this.btNot2.Location = new System.Drawing.Point(349, 7);
            this.btNot2.Name = "btNot2";
            this.btNot2.Size = new System.Drawing.Size(20, 23);
            this.btNot2.TabIndex = 7;
            this.btNot2.Tag = "1";
            this.btNot2.Text = "!";
            this.btNot2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btNot2.UseVisualStyleBackColor = true;
            this.btNot2.CheckedChanged += new System.EventHandler(this.btNot_CheckedChanged);
            // 
            // btNot3
            // 
            this.btNot3.Appearance = System.Windows.Forms.Appearance.Button;
            this.btNot3.AutoSize = true;
            this.btNot3.Location = new System.Drawing.Point(557, 7);
            this.btNot3.Name = "btNot3";
            this.btNot3.Size = new System.Drawing.Size(20, 23);
            this.btNot3.TabIndex = 8;
            this.btNot3.Tag = "2";
            this.btNot3.Text = "!";
            this.btNot3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btNot3.UseVisualStyleBackColor = true;
            this.btNot3.CheckedChanged += new System.EventHandler(this.btNot_CheckedChanged);
            // 
            // SearchBrowserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btNot3);
            this.Controls.Add(this.btNot2);
            this.Controls.Add(this.btNot1);
            this.Controls.Add(this.cbType3);
            this.Controls.Add(this.cbType2);
            this.Controls.Add(this.cbType1);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView3);
            this.Controls.Add(this.listView1);
            this.DoubleBuffered = true;
            this.Name = "SearchBrowserControl";
            this.Size = new System.Drawing.Size(586, 272);
            this.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.GiveDragFeedback);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private ListViewEx listView1;
        private ListViewEx listView2;
        private ListViewEx listView3;
        private ComboBox cbType1;
        private ComboBox cbType2;
        private ComboBox cbType3;
        private CheckBox btNot1;
        private CheckBox btNot2;
        private CheckBox btNot3;
        private ToolTip toolTip;
    }
}
