using System.Windows.Forms;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;

namespace cYo.Projects.ComicRack.Engine.Controls
{
    public partial class MagnifySetupControl : UserControlEx
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
            this.tbWidth = new cYo.Common.Windows.Forms.TrackBarLite();
            this.labelWidth = new System.Windows.Forms.Label();
            this.tbHeight = new cYo.Common.Windows.Forms.TrackBarLite();
            this.labelHeight = new System.Windows.Forms.Label();
            this.tbOpaque = new cYo.Common.Windows.Forms.TrackBarLite();
            this.labelOpacity = new System.Windows.Forms.Label();
            this.tbZoom = new cYo.Common.Windows.Forms.TrackBarLite();
            this.labelZoom = new System.Windows.Forms.Label();
            this.chkSimpleStyle = new System.Windows.Forms.CheckBox();
            this.chkAutoHideMagnifier = new System.Windows.Forms.CheckBox();
            this.chkAutoMagnifier = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbWidth
            // 
            this.tbWidth.Location = new System.Drawing.Point(6, 19);
            this.tbWidth.Maximum = 1024;
            this.tbWidth.Minimum = 64;
            this.tbWidth.Name = "tbWidth";
            this.tbWidth.Size = new System.Drawing.Size(106, 17);
            this.tbWidth.TabIndex = 1;
            this.tbWidth.Text = "Width";
            this.tbWidth.ThumbSize = new System.Drawing.Size(6, 12);
            this.tbWidth.TickFrequency = 64;
            this.tbWidth.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbWidth.Value = 64;
            this.tbWidth.Scroll += new System.EventHandler(this.ControlValuesChanged);
            // 
            // labelWidth
            // 
            this.labelWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.labelWidth.Location = new System.Drawing.Point(9, 39);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(103, 12);
            this.labelWidth.TabIndex = 0;
            this.labelWidth.Text = "Width";
            this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbHeight
            // 
            this.tbHeight.Location = new System.Drawing.Point(118, 19);
            this.tbHeight.Maximum = 1024;
            this.tbHeight.Minimum = 64;
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new System.Drawing.Size(106, 17);
            this.tbHeight.TabIndex = 3;
            this.tbHeight.Text = "Width";
            this.tbHeight.ThumbSize = new System.Drawing.Size(6, 12);
            this.tbHeight.TickFrequency = 64;
            this.tbHeight.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbHeight.Value = 64;
            this.tbHeight.Scroll += new System.EventHandler(this.ControlValuesChanged);
            // 
            // labelHeight
            // 
            this.labelHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.labelHeight.Location = new System.Drawing.Point(118, 39);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(106, 12);
            this.labelHeight.TabIndex = 2;
            this.labelHeight.Text = "Height";
            this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbOpaque
            // 
            this.tbOpaque.Location = new System.Drawing.Point(6, 56);
            this.tbOpaque.Minimum = 20;
            this.tbOpaque.Name = "tbOpaque";
            this.tbOpaque.Size = new System.Drawing.Size(106, 17);
            this.tbOpaque.TabIndex = 5;
            this.tbOpaque.Text = "Width";
            this.tbOpaque.ThumbSize = new System.Drawing.Size(6, 12);
            this.tbOpaque.TickFrequency = 5;
            this.tbOpaque.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbOpaque.Value = 20;
            this.tbOpaque.Scroll += new System.EventHandler(this.ControlValuesChanged);
            // 
            // labelOpacity
            // 
            this.labelOpacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.labelOpacity.Location = new System.Drawing.Point(6, 76);
            this.labelOpacity.Name = "labelOpacity";
            this.labelOpacity.Size = new System.Drawing.Size(106, 12);
            this.labelOpacity.TabIndex = 4;
            this.labelOpacity.Text = "Opacity";
            this.labelOpacity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbZoom
            // 
            this.tbZoom.Location = new System.Drawing.Point(118, 56);
            this.tbZoom.Maximum = 500;
            this.tbZoom.Minimum = 100;
            this.tbZoom.Name = "tbZoom";
            this.tbZoom.Size = new System.Drawing.Size(106, 17);
            this.tbZoom.TabIndex = 7;
            this.tbZoom.Text = "Width";
            this.tbZoom.ThumbSize = new System.Drawing.Size(6, 12);
            this.tbZoom.TickFrequency = 25;
            this.tbZoom.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbZoom.Value = 100;
            this.tbZoom.Scroll += new System.EventHandler(this.ControlValuesChanged);
            // 
            // labelZoom
            // 
            this.labelZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.labelZoom.Location = new System.Drawing.Point(120, 76);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Size = new System.Drawing.Size(104, 12);
            this.labelZoom.TabIndex = 6;
            this.labelZoom.Text = "Zoom";
            this.labelZoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkSimpleStyle
            // 
            this.chkSimpleStyle.AutoSize = true;
            this.chkSimpleStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.chkSimpleStyle.Location = new System.Drawing.Point(8, 19);
            this.chkSimpleStyle.Name = "chkSimpleStyle";
            this.chkSimpleStyle.Size = new System.Drawing.Size(87, 16);
            this.chkSimpleStyle.TabIndex = 8;
            this.chkSimpleStyle.Text = "Simple Style";
            this.chkSimpleStyle.UseVisualStyleBackColor = true;
            this.chkSimpleStyle.CheckedChanged += new System.EventHandler(this.ControlValuesChanged);
            // 
            // chkAutoHideMagnifier
            // 
            this.chkAutoHideMagnifier.AutoSize = true;
            this.chkAutoHideMagnifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.chkAutoHideMagnifier.Location = new System.Drawing.Point(8, 41);
            this.chkAutoHideMagnifier.Name = "chkAutoHideMagnifier";
            this.chkAutoHideMagnifier.Size = new System.Drawing.Size(124, 16);
            this.chkAutoHideMagnifier.TabIndex = 9;
            this.chkAutoHideMagnifier.Text = "Hide at Page Border";
            this.chkAutoHideMagnifier.UseVisualStyleBackColor = true;
            this.chkAutoHideMagnifier.CheckedChanged += new System.EventHandler(this.ControlValuesChanged);
            // 
            // chkAutoMagnifier
            // 
            this.chkAutoMagnifier.AutoSize = true;
            this.chkAutoMagnifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold);
            this.chkAutoMagnifier.Location = new System.Drawing.Point(8, 63);
            this.chkAutoMagnifier.Name = "chkAutoMagnifier";
            this.chkAutoMagnifier.Size = new System.Drawing.Size(152, 16);
            this.chkAutoMagnifier.TabIndex = 10;
            this.chkAutoMagnifier.Text = "Activate with \'long\' Click";
            this.chkAutoMagnifier.UseVisualStyleBackColor = true;
            this.chkAutoMagnifier.CheckedChanged += new System.EventHandler(this.ControlValuesChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSimpleStyle);
            this.groupBox1.Controls.Add(this.chkAutoMagnifier);
            this.groupBox1.Controls.Add(this.chkAutoHideMagnifier);
            this.groupBox1.Location = new System.Drawing.Point(11, 117);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 87);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbWidth);
            this.groupBox2.Controls.Add(this.tbHeight);
            this.groupBox2.Controls.Add(this.labelZoom);
            this.groupBox2.Controls.Add(this.labelWidth);
            this.groupBox2.Controls.Add(this.labelOpacity);
            this.groupBox2.Controls.Add(this.tbOpaque);
            this.groupBox2.Controls.Add(this.tbZoom);
            this.groupBox2.Controls.Add(this.labelHeight);
            this.groupBox2.Location = new System.Drawing.Point(11, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(232, 108);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // MagnifySetupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = ThemeColors.MagnifySetup.Back;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MagnifySetupControl";
            this.Size = new System.Drawing.Size(253, 215);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        private Label labelWidth;
        private Label labelHeight;
        private Label labelOpacity;
        private Label labelZoom;
        private CheckBox chkSimpleStyle;
        private CheckBox chkAutoHideMagnifier;
        private CheckBox chkAutoMagnifier;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
    }
}

