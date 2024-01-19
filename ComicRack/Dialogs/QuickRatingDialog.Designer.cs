using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Controls;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class QuickRatingDialog
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
            this.txRating = new cYo.Projects.ComicRack.Engine.Controls.RatingControl();
            this.coverThumbnail = new cYo.Projects.ComicRack.Engine.Controls.ThumbnailControl();
            this.txReview = new cYo.Common.Windows.Forms.TextBoxEx();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.chkShow = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txRating
            // 
            this.txRating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txRating.DrawText = true;
            this.txRating.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txRating.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txRating.Location = new System.Drawing.Point(12, 245);
            this.txRating.Name = "txRating";
            this.txRating.Rating = 3F;
            this.txRating.RatingImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.StarYellow;
            this.txRating.Size = new System.Drawing.Size(170, 21);
            this.txRating.TabIndex = 2;
            this.txRating.Text = "3";
            // 
            // coverThumbnail
            // 
            this.coverThumbnail.AllowDrop = true;
            this.coverThumbnail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.coverThumbnail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coverThumbnail.Location = new System.Drawing.Point(12, 12);
            this.coverThumbnail.Name = "coverThumbnail";
            this.coverThumbnail.Size = new System.Drawing.Size(170, 227);
            this.coverThumbnail.TabIndex = 1;
            this.coverThumbnail.TextElements = cYo.Projects.ComicRack.Engine.Drawing.ComicTextElements.None;
            this.coverThumbnail.ThreeD = true;
            // 
            // txReview
            // 
            this.txReview.AcceptsReturn = true;
            this.txReview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txReview.FocusSelect = false;
            this.txReview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txReview.Location = new System.Drawing.Point(188, 12);
            this.txReview.Multiline = true;
            this.txReview.Name = "txReview";
            this.txReview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txReview.Size = new System.Drawing.Size(369, 254);
            this.txReview.TabIndex = 0;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(477, 272);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 6;
            this.btCancel.Text = "&Cancel";
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(391, 272);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 5;
            this.btOK.Text = "&OK";
            // 
            // chkShow
            // 
            this.chkShow.AutoSize = true;
            this.chkShow.Location = new System.Drawing.Point(12, 277);
            this.chkShow.Name = "chkShow";
            this.chkShow.Size = new System.Drawing.Size(134, 17);
            this.chkShow.TabIndex = 3;
            this.chkShow.Text = "Show when Book read";
            this.chkShow.UseVisualStyleBackColor = true;
            // 
            // QuickRatingDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(564, 304);
            this.Controls.Add(this.chkShow);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.txReview);
            this.Controls.Add(this.coverThumbnail);
            this.Controls.Add(this.txRating);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickRatingDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quick Rating";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private RatingControl txRating;

		private ThumbnailControl coverThumbnail;

		private TextBoxEx txReview;

		private Button btCancel;

		private Button btOK;

		private CheckBox chkShow;
    }
}
