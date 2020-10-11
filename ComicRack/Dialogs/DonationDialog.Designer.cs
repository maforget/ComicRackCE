using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class DonationDialog
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
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(cYo.Projects.ComicRack.Viewer.Dialogs.DonationDialog));
			btOK = new System.Windows.Forms.Button();
			btCancel = new System.Windows.Forms.Button();
			labelDonationText = new System.Windows.Forms.Label();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			groupDonate = new System.Windows.Forms.GroupBox();
			validationIcon = new System.Windows.Forms.PictureBox();
			btValidate = new System.Windows.Forms.Button();
			textEmail = new System.Windows.Forms.TextBox();
			labelEmail = new System.Windows.Forms.Label();
			labelStepTwo = new System.Windows.Forms.Label();
			btDonate = new System.Windows.Forms.Button();
			labelStepOne = new System.Windows.Forms.Label();
			labelThankYou = new System.Windows.Forms.Label();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			panel1 = new System.Windows.Forms.Panel();
			panel2 = new System.Windows.Forms.Panel();
			timerValidation = new System.Windows.Forms.Timer(components);
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			groupDonate.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)validationIcon).BeginInit();
			flowLayoutPanel1.SuspendLayout();
			panel1.SuspendLayout();
			panel2.SuspendLayout();
			SuspendLayout();
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(104, 9);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(95, 24);
			btOK.TabIndex = 1;
			btOK.Text = "&OK";
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(3, 9);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(95, 24);
			btCancel.TabIndex = 0;
			btCancel.Text = "Skip for now";
			labelDonationText.AutoSize = true;
			labelDonationText.Location = new System.Drawing.Point(149, 0);
			labelDonationText.MaximumSize = new System.Drawing.Size(405, 0);
			labelDonationText.Name = "labelDonationText";
			labelDonationText.Padding = new System.Windows.Forms.Padding(4);
			labelDonationText.Size = new System.Drawing.Size(404, 125);
			labelDonationText.TabIndex = 0;
			labelDonationText.Text = resources.GetString("labelDonationText.Text");
			pictureBox1.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ComicRackApp256;
			pictureBox1.Location = new System.Drawing.Point(0, 0);
			pictureBox1.MinimumSize = new System.Drawing.Size(142, 142);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(142, 142);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 8;
			pictureBox1.TabStop = false;
			groupDonate.Controls.Add(validationIcon);
			groupDonate.Controls.Add(btValidate);
			groupDonate.Controls.Add(textEmail);
			groupDonate.Controls.Add(labelEmail);
			groupDonate.Controls.Add(labelStepTwo);
			groupDonate.Controls.Add(btDonate);
			groupDonate.Controls.Add(labelStepOne);
			groupDonate.Location = new System.Drawing.Point(3, 154);
			groupDonate.Name = "groupDonate";
			groupDonate.Size = new System.Drawing.Size(558, 194);
			groupDonate.TabIndex = 1;
			groupDonate.TabStop = false;
			groupDonate.Text = "Donate";
			validationIcon.Location = new System.Drawing.Point(360, 153);
			validationIcon.Name = "validationIcon";
			validationIcon.Size = new System.Drawing.Size(21, 20);
			validationIcon.TabIndex = 6;
			validationIcon.TabStop = false;
			btValidate.Location = new System.Drawing.Point(424, 150);
			btValidate.Name = "btValidate";
			btValidate.Size = new System.Drawing.Size(111, 23);
			btValidate.TabIndex = 5;
			btValidate.Text = "Validate";
			btValidate.UseVisualStyleBackColor = true;
			btValidate.Click += new System.EventHandler(btValidate_Click);
			textEmail.Location = new System.Drawing.Point(149, 153);
			textEmail.Name = "textEmail";
			textEmail.Size = new System.Drawing.Size(205, 20);
			textEmail.TabIndex = 4;
			textEmail.TextChanged += new System.EventHandler(textEmail_TextChanged);
			labelEmail.AutoSize = true;
			labelEmail.Location = new System.Drawing.Point(67, 156);
			labelEmail.Name = "labelEmail";
			labelEmail.Size = new System.Drawing.Size(76, 13);
			labelEmail.TabIndex = 3;
			labelEmail.Text = "Email Address:";
			labelStepTwo.Location = new System.Drawing.Point(23, 86);
			labelStepTwo.Name = "labelStepTwo";
			labelStepTwo.Size = new System.Drawing.Size(461, 57);
			labelStepTwo.TabIndex = 2;
			labelStepTwo.Text = "Step Two:\r\nEnter your donation email and validate it.\r\nIt may take a few minutes for your donation to be registered so you can do this step a little later.";
			btDonate.Location = new System.Drawing.Point(424, 34);
			btDonate.Name = "btDonate";
			btDonate.Size = new System.Drawing.Size(111, 23);
			btDonate.TabIndex = 1;
			btDonate.Text = "Donation Page";
			btDonate.UseVisualStyleBackColor = true;
			btDonate.Click += new System.EventHandler(btDonate_Click);
			labelStepOne.AutoSize = true;
			labelStepOne.Location = new System.Drawing.Point(23, 31);
			labelStepOne.Name = "labelStepOne";
			labelStepOne.Size = new System.Drawing.Size(188, 26);
			labelStepOne.TabIndex = 0;
			labelStepOne.Text = "Step One:\r\nVisit ComicRack on the web to donate";
			labelThankYou.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			labelThankYou.Location = new System.Drawing.Point(3, 351);
			labelThankYou.Name = "labelThankYou";
			labelThankYou.Size = new System.Drawing.Size(558, 32);
			labelThankYou.TabIndex = 2;
			labelThankYou.Text = "Thank you for supporting ComicRack!";
			labelThankYou.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			flowLayoutPanel1.AutoSize = true;
			flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			flowLayoutPanel1.Controls.Add(panel1);
			flowLayoutPanel1.Controls.Add(groupDonate);
			flowLayoutPanel1.Controls.Add(labelThankYou);
			flowLayoutPanel1.Controls.Add(panel2);
			flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowLayoutPanel1.Location = new System.Drawing.Point(2, 3);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new System.Drawing.Size(564, 425);
			flowLayoutPanel1.TabIndex = 0;
			panel1.AutoSize = true;
			panel1.Controls.Add(labelDonationText);
			panel1.Controls.Add(pictureBox1);
			panel1.Dock = System.Windows.Forms.DockStyle.Top;
			panel1.Location = new System.Drawing.Point(3, 3);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(558, 145);
			panel1.TabIndex = 0;
			panel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			panel2.AutoSize = true;
			panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel2.Controls.Add(btOK);
			panel2.Controls.Add(btCancel);
			panel2.Location = new System.Drawing.Point(359, 386);
			panel2.Name = "panel2";
			panel2.Size = new System.Drawing.Size(202, 36);
			panel2.TabIndex = 3;
			timerValidation.Interval = 5000;
			timerValidation.Tick += new System.EventHandler(timerValidation_Tick);
			base.AcceptButton = btOK;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.ClientSize = new System.Drawing.Size(571, 431);
			base.Controls.Add(flowLayoutPanel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DonationDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Support ComicRack...";
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			groupDonate.ResumeLayout(false);
			groupDonate.PerformLayout();
			((System.ComponentModel.ISupportInitialize)validationIcon).EndInit();
			flowLayoutPanel1.ResumeLayout(false);
			flowLayoutPanel1.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			panel2.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
		
		private IContainer components;

		private Button btOK;

		private Button btCancel;

		private Label labelDonationText;

		private PictureBox pictureBox1;

		private GroupBox groupDonate;

		private Label labelStepTwo;

		private Button btDonate;

		private Label labelStepOne;

		private Button btValidate;

		private TextBox textEmail;

		private Label labelEmail;

		private PictureBox validationIcon;

		private Label labelThankYou;

		private FlowLayoutPanel flowLayoutPanel1;

		private Panel panel1;

		private Panel panel2;

		private Timer timerValidation;
	}
}
