using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Controls;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class QuickRatingDialog
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
			txRating = new cYo.Projects.ComicRack.Engine.Controls.RatingControl();
			coverThumbnail = new cYo.Projects.ComicRack.Engine.Controls.ThumbnailControl();
			txReview = new cYo.Common.Windows.Forms.TextBoxEx();
			btCancel = new System.Windows.Forms.Button();
			btOK = new System.Windows.Forms.Button();
			chkTweet = new System.Windows.Forms.CheckBox();
			chkShow = new System.Windows.Forms.CheckBox();
			SuspendLayout();
			txRating.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			txRating.DrawText = true;
			txRating.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			txRating.ForeColor = System.Drawing.SystemColors.GrayText;
			txRating.Location = new System.Drawing.Point(12, 245);
			txRating.Name = "txRating";
			txRating.Rating = 3f;
			txRating.RatingImage = cYo.Projects.ComicRack.Viewer.Properties.Resources.StarYellow;
			txRating.Size = new System.Drawing.Size(170, 21);
			txRating.TabIndex = 2;
			txRating.Text = "3";
			coverThumbnail.AllowDrop = true;
			coverThumbnail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			coverThumbnail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			coverThumbnail.Location = new System.Drawing.Point(12, 12);
			coverThumbnail.Name = "coverThumbnail";
			coverThumbnail.Size = new System.Drawing.Size(170, 227);
			coverThumbnail.TabIndex = 1;
			coverThumbnail.TextElements = cYo.Projects.ComicRack.Engine.Drawing.ComicTextElements.None;
			coverThumbnail.ThreeD = true;
			txReview.AcceptsReturn = true;
			txReview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txReview.FocusSelect = false;
			txReview.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
			txReview.Location = new System.Drawing.Point(188, 12);
			txReview.Multiline = true;
			txReview.Name = "txReview";
			txReview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			txReview.Size = new System.Drawing.Size(369, 254);
			txReview.TabIndex = 0;
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(477, 272);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 6;
			btCancel.Text = "&Cancel";
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(391, 272);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 5;
			btOK.Text = "&OK";
			chkTweet.AutoSize = true;
			chkTweet.Location = new System.Drawing.Point(188, 277);
			chkTweet.Name = "chkTweet";
			chkTweet.Size = new System.Drawing.Size(56, 17);
			chkTweet.TabIndex = 4;
			chkTweet.Text = "Tweet";
			chkTweet.UseVisualStyleBackColor = true;
			chkShow.AutoSize = true;
			chkShow.Location = new System.Drawing.Point(12, 277);
			chkShow.Name = "chkShow";
			chkShow.Size = new System.Drawing.Size(134, 17);
			chkShow.TabIndex = 3;
			chkShow.Text = "Show when Book read";
			chkShow.UseVisualStyleBackColor = true;
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(564, 304);
			base.Controls.Add(chkShow);
			base.Controls.Add(chkTweet);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.Controls.Add(txReview);
			base.Controls.Add(coverThumbnail);
			base.Controls.Add(txRating);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "QuickRatingDialog";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Quick Rating";
			ResumeLayout(false);
			PerformLayout();
		}

		private IContainer components;

		private RatingControl txRating;

		private ThumbnailControl coverThumbnail;

		private TextBoxEx txReview;

		private Button btCancel;

		private Button btOK;

		private CheckBox chkTweet;

		private CheckBox chkShow;
	}
}
