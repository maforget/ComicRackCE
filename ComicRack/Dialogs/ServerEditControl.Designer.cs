using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ServerEditControl : UserControlEx
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IdleProcess.Idle -= IdleProcess_Idle;
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
            this.chkClientsCanExport = new System.Windows.Forms.CheckBox();
            this.chkClientsCanEdit = new System.Windows.Forms.CheckBox();
            this.txPassword = new cYo.Common.Windows.Forms.PasswordTextBox();
            this.chkRequirePassword = new System.Windows.Forms.CheckBox();
            this.txSharedName = new System.Windows.Forms.TextBox();
            this.labelSharedName = new System.Windows.Forms.Label();
            this.txPublicServerComment = new System.Windows.Forms.TextBox();
            this.labelShareDescription = new System.Windows.Forms.Label();
            this.chkShareInternet = new System.Windows.Forms.CheckBox();
            this.chkPrivate = new System.Windows.Forms.CheckBox();
            this.tbPageQuality = new cYo.Common.Windows.Forms.TrackBarLite();
            this.labelPageQuality = new System.Windows.Forms.Label();
            this.labelThumbQuality = new System.Windows.Forms.Label();
            this.tbThumbQuality = new cYo.Common.Windows.Forms.TrackBarLite();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tvSharedLists = new cYo.Common.Windows.Forms.TreeViewEx();
            this.ilShares = new System.Windows.Forms.ImageList(this.components);
            this.cbShare = new System.Windows.Forms.ComboBox();
            this.grpShareSettings = new System.Windows.Forms.GroupBox();
            this.grpShareSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkClientsCanExport
            // 
            this.chkClientsCanExport.AutoSize = true;
            this.chkClientsCanExport.Location = new System.Drawing.Point(5, 138);
            this.chkClientsCanExport.Name = "chkClientsCanExport";
            this.chkClientsCanExport.Size = new System.Drawing.Size(143, 17);
            this.chkClientsCanExport.TabIndex = 11;
            this.chkClientsCanExport.Text = "Clients can export Books";
            this.chkClientsCanExport.UseVisualStyleBackColor = true;
            // 
            // chkClientsCanEdit
            // 
            this.chkClientsCanEdit.AutoSize = true;
            this.chkClientsCanEdit.Location = new System.Drawing.Point(5, 120);
            this.chkClientsCanEdit.Name = "chkClientsCanEdit";
            this.chkClientsCanEdit.Size = new System.Drawing.Size(131, 17);
            this.chkClientsCanEdit.TabIndex = 10;
            this.chkClientsCanEdit.Text = "Clients can edit Books";
            this.chkClientsCanEdit.UseVisualStyleBackColor = true;
            // 
            // txPassword
            // 
            this.txPassword.Location = new System.Drawing.Point(24, 45);
            this.txPassword.Name = "txPassword";
            this.txPassword.Password = null;
            this.txPassword.Size = new System.Drawing.Size(153, 20);
            this.txPassword.TabIndex = 7;
            this.txPassword.UseSystemPasswordChar = true;
            // 
            // chkRequirePassword
            // 
            this.chkRequirePassword.AutoSize = true;
            this.chkRequirePassword.Location = new System.Drawing.Point(6, 23);
            this.chkRequirePassword.Name = "chkRequirePassword";
            this.chkRequirePassword.Size = new System.Drawing.Size(115, 17);
            this.chkRequirePassword.TabIndex = 6;
            this.chkRequirePassword.Text = "Require Password:";
            this.chkRequirePassword.UseVisualStyleBackColor = true;
            // 
            // txSharedName
            // 
            this.txSharedName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txSharedName.Location = new System.Drawing.Point(106, 7);
            this.txSharedName.Name = "txSharedName";
            this.txSharedName.Size = new System.Drawing.Size(280, 20);
            this.txSharedName.TabIndex = 1;
            this.txSharedName.TextChanged += new System.EventHandler(this.txSharedName_TextChanged);
            // 
            // labelSharedName
            // 
            this.labelSharedName.AutoSize = true;
            this.labelSharedName.Location = new System.Drawing.Point(3, 10);
            this.labelSharedName.Name = "labelSharedName";
            this.labelSharedName.Size = new System.Drawing.Size(75, 13);
            this.labelSharedName.TabIndex = 0;
            this.labelSharedName.Text = "Shared Name:";
            // 
            // txPublicServerComment
            // 
            this.txPublicServerComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txPublicServerComment.Location = new System.Drawing.Point(106, 29);
            this.txPublicServerComment.Name = "txPublicServerComment";
            this.txPublicServerComment.Size = new System.Drawing.Size(280, 20);
            this.txPublicServerComment.TabIndex = 3;
            // 
            // labelShareDescription
            // 
            this.labelShareDescription.AutoSize = true;
            this.labelShareDescription.Location = new System.Drawing.Point(3, 32);
            this.labelShareDescription.Name = "labelShareDescription";
            this.labelShareDescription.Size = new System.Drawing.Size(63, 13);
            this.labelShareDescription.TabIndex = 2;
            this.labelShareDescription.Text = "Description:";
            // 
            // chkShareInternet
            // 
            this.chkShareInternet.AutoSize = true;
            this.chkShareInternet.Location = new System.Drawing.Point(6, 78);
            this.chkShareInternet.Name = "chkShareInternet";
            this.chkShareInternet.Size = new System.Drawing.Size(126, 17);
            this.chkShareInternet.TabIndex = 8;
            this.chkShareInternet.Text = "Share on the Internet";
            this.chkShareInternet.UseVisualStyleBackColor = true;
            // 
            // chkPrivate
            // 
            this.chkPrivate.AutoSize = true;
            this.chkPrivate.Location = new System.Drawing.Point(25, 97);
            this.chkPrivate.Name = "chkPrivate";
            this.chkPrivate.Size = new System.Drawing.Size(59, 17);
            this.chkPrivate.TabIndex = 9;
            this.chkPrivate.Text = "Private";
            this.chkPrivate.UseVisualStyleBackColor = true;
            // 
            // tbPageQuality
            // 
            this.tbPageQuality.Location = new System.Drawing.Point(82, 175);
            this.tbPageQuality.Minimum = 15;
            this.tbPageQuality.Name = "tbPageQuality";
            this.tbPageQuality.Size = new System.Drawing.Size(95, 19);
            this.tbPageQuality.TabIndex = 13;
            this.tbPageQuality.ThumbSize = new System.Drawing.Size(8, 12);
            this.tbPageQuality.Value = 15;
            this.tbPageQuality.ValueChanged += new System.EventHandler(this.tbPageQuality_ValueChanged);
            // 
            // labelPageQuality
            // 
            this.labelPageQuality.AutoSize = true;
            this.labelPageQuality.Location = new System.Drawing.Point(3, 175);
            this.labelPageQuality.Name = "labelPageQuality";
            this.labelPageQuality.Size = new System.Drawing.Size(70, 13);
            this.labelPageQuality.TabIndex = 12;
            this.labelPageQuality.Text = "Page Quality:";
            // 
            // labelThumbQuality
            // 
            this.labelThumbQuality.AutoSize = true;
            this.labelThumbQuality.Location = new System.Drawing.Point(3, 197);
            this.labelThumbQuality.Name = "labelThumbQuality";
            this.labelThumbQuality.Size = new System.Drawing.Size(78, 13);
            this.labelThumbQuality.TabIndex = 14;
            this.labelThumbQuality.Text = "Thumb Quality:";
            // 
            // tbThumbQuality
            // 
            this.tbThumbQuality.Location = new System.Drawing.Point(82, 197);
            this.tbThumbQuality.Minimum = 15;
            this.tbThumbQuality.Name = "tbThumbQuality";
            this.tbThumbQuality.Size = new System.Drawing.Size(95, 19);
            this.tbThumbQuality.TabIndex = 15;
            this.tbThumbQuality.ThumbSize = new System.Drawing.Size(8, 12);
            this.tbThumbQuality.Value = 15;
            this.tbThumbQuality.ValueChanged += new System.EventHandler(this.tbPageQuality_ValueChanged);
            // 
            // tvSharedLists
            // 
            this.tvSharedLists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvSharedLists.CheckBoxes = true;
            this.tvSharedLists.ImageIndex = 0;
            this.tvSharedLists.ImageList = this.ilShares;
            this.tvSharedLists.ItemHeight = 16;
            this.tvSharedLists.Location = new System.Drawing.Point(6, 90);
            this.tvSharedLists.Name = "tvSharedLists";
            this.tvSharedLists.SelectedImageIndex = 0;
            this.tvSharedLists.ShowLines = false;
            this.tvSharedLists.ShowPlusMinus = false;
            this.tvSharedLists.ShowRootLines = false;
            this.tvSharedLists.Size = new System.Drawing.Size(191, 240);
            this.tvSharedLists.TabIndex = 5;
            // 
            // ilShares
            // 
            this.ilShares.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilShares.ImageSize = new System.Drawing.Size(16, 16);
            this.ilShares.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cbShare
            // 
            this.cbShare.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShare.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShare.FormattingEnabled = true;
            this.cbShare.Items.AddRange(new object[] {
            "Share None",
            "Share All",
            "Share Selected"});
            this.cbShare.Location = new System.Drawing.Point(6, 67);
            this.cbShare.Name = "cbShare";
            this.cbShare.Size = new System.Drawing.Size(191, 21);
            this.cbShare.TabIndex = 4;
            // 
            // grpShareSettings
            // 
            this.grpShareSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpShareSettings.Controls.Add(this.chkRequirePassword);
            this.grpShareSettings.Controls.Add(this.txPassword);
            this.grpShareSettings.Controls.Add(this.chkClientsCanEdit);
            this.grpShareSettings.Controls.Add(this.labelThumbQuality);
            this.grpShareSettings.Controls.Add(this.chkClientsCanExport);
            this.grpShareSettings.Controls.Add(this.tbThumbQuality);
            this.grpShareSettings.Controls.Add(this.chkShareInternet);
            this.grpShareSettings.Controls.Add(this.labelPageQuality);
            this.grpShareSettings.Controls.Add(this.chkPrivate);
            this.grpShareSettings.Controls.Add(this.tbPageQuality);
            this.grpShareSettings.Location = new System.Drawing.Point(203, 67);
            this.grpShareSettings.Name = "grpShareSettings";
            this.grpShareSettings.Size = new System.Drawing.Size(183, 263);
            this.grpShareSettings.TabIndex = 16;
            this.grpShareSettings.TabStop = false;
            this.grpShareSettings.Text = "Settings";
            // 
            // ServerEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpShareSettings);
            this.Controls.Add(this.cbShare);
            this.Controls.Add(this.tvSharedLists);
            this.Controls.Add(this.txPublicServerComment);
            this.Controls.Add(this.labelShareDescription);
            this.Controls.Add(this.txSharedName);
            this.Controls.Add(this.labelSharedName);
            this.Name = "ServerEditControl";
            this.Size = new System.Drawing.Size(389, 333);
            this.grpShareSettings.ResumeLayout(false);
            this.grpShareSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private CheckBox chkClientsCanExport;
        private CheckBox chkClientsCanEdit;
        private PasswordTextBox txPassword;
        private CheckBox chkRequirePassword;
        private TextBox txSharedName;
        private Label labelSharedName;
        private TextBox txPublicServerComment;
        private Label labelShareDescription;
        private CheckBox chkShareInternet;
        private CheckBox chkPrivate;
        private TrackBarLite tbPageQuality;
        private Label labelPageQuality;
        private Label labelThumbQuality;
        private TrackBarLite tbThumbQuality;
        private ToolTip toolTip;
        private TreeViewEx tvSharedLists;
        private ComboBox cbShare;
        private ImageList ilShares;
        private GroupBox grpShareSettings;

    }
}
