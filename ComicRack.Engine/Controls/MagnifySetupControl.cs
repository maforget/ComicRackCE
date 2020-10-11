using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Display;

namespace cYo.Projects.ComicRack.Engine.Controls
{
	public class MagnifySetupControl : UserControl
	{
		private IContainer components;

		private TrackBarLite tbWidth;

		private Label labelWidth;

		private TrackBarLite tbHeight;

		private Label labelHeight;

		private TrackBarLite tbOpaque;

		private Label labelOpacity;

		private TrackBarLite tbZoom;

		private Label labelZoom;

		private CheckBox chkSimpleStyle;

		private CheckBox chkAutoHideMagnifier;

		private CheckBox chkAutoMagnifier;

		private GroupBox groupBox1;

		private GroupBox groupBox2;

		public int MagnifyWidth
		{
			get
			{
				return tbWidth.Value;
			}
			set
			{
				tbWidth.Value = value;
			}
		}

		public int MagnifyHeight
		{
			get
			{
				return tbHeight.Value;
			}
			set
			{
				tbHeight.Value = value;
			}
		}

		public float MagnifyOpaque
		{
			get
			{
				return (float)tbOpaque.Value / 100f;
			}
			set
			{
				tbOpaque.Value = (int)(value * 100f);
			}
		}

		public float MagnifyZoom
		{
			get
			{
				return (float)tbZoom.Value / 100f;
			}
			set
			{
				tbZoom.Value = (int)(value * 100f);
			}
		}

		public Size MagnifySize
		{
			get
			{
				return new Size(MagnifyWidth, MagnifyHeight);
			}
			set
			{
				MagnifyWidth = value.Width;
				MagnifyHeight = value.Height;
			}
		}

		public MagnifierStyle MagnifyStyle
		{
			get
			{
				if (!chkSimpleStyle.Checked)
				{
					return MagnifierStyle.Glass;
				}
				return MagnifierStyle.Simple;
			}
			set
			{
				chkSimpleStyle.Checked = value == MagnifierStyle.Simple;
			}
		}

		public bool AutoHideMagnifier
		{
			get
			{
				return chkAutoHideMagnifier.Checked;
			}
			set
			{
				chkAutoHideMagnifier.Checked = value;
			}
		}

		public bool AutoMagnifier
		{
			get
			{
				return chkAutoMagnifier.Checked;
			}
			set
			{
				chkAutoMagnifier.Checked = value;
			}
		}

		public event EventHandler ValuesChanged;

		public MagnifySetupControl()
		{
			InitializeComponent();
			LocalizeUtility.Localize(this, components);
		}

		private void ControlValuesChanged(object sender, EventArgs e)
		{
			OnValuesChanged();
		}

		private void OnValuesChanged()
		{
			if (this.ValuesChanged != null)
			{
				this.ValuesChanged(this, EventArgs.Empty);
			}
		}

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
			tbWidth = new cYo.Common.Windows.Forms.TrackBarLite();
			labelWidth = new System.Windows.Forms.Label();
			tbHeight = new cYo.Common.Windows.Forms.TrackBarLite();
			labelHeight = new System.Windows.Forms.Label();
			tbOpaque = new cYo.Common.Windows.Forms.TrackBarLite();
			labelOpacity = new System.Windows.Forms.Label();
			tbZoom = new cYo.Common.Windows.Forms.TrackBarLite();
			labelZoom = new System.Windows.Forms.Label();
			chkSimpleStyle = new System.Windows.Forms.CheckBox();
			chkAutoHideMagnifier = new System.Windows.Forms.CheckBox();
			chkAutoMagnifier = new System.Windows.Forms.CheckBox();
			groupBox1 = new System.Windows.Forms.GroupBox();
			groupBox2 = new System.Windows.Forms.GroupBox();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			SuspendLayout();
			tbWidth.Location = new System.Drawing.Point(6, 19);
			tbWidth.Maximum = 512;
			tbWidth.Minimum = 64;
			tbWidth.Name = "tbWidth";
			tbWidth.Size = new System.Drawing.Size(106, 17);
			tbWidth.TabIndex = 1;
			tbWidth.Text = "Width";
			tbWidth.ThumbSize = new System.Drawing.Size(6, 12);
			tbWidth.TickFrequency = 32;
			tbWidth.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbWidth.Value = 64;
			tbWidth.Scroll += new System.EventHandler(ControlValuesChanged);
			labelWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelWidth.Location = new System.Drawing.Point(9, 39);
			labelWidth.Name = "labelWidth";
			labelWidth.Size = new System.Drawing.Size(103, 12);
			labelWidth.TabIndex = 0;
			labelWidth.Text = "Width";
			labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			tbHeight.Location = new System.Drawing.Point(118, 19);
			tbHeight.Maximum = 512;
			tbHeight.Minimum = 64;
			tbHeight.Name = "tbHeight";
			tbHeight.Size = new System.Drawing.Size(106, 17);
			tbHeight.TabIndex = 3;
			tbHeight.Text = "Width";
			tbHeight.ThumbSize = new System.Drawing.Size(6, 12);
			tbHeight.TickFrequency = 32;
			tbHeight.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbHeight.Value = 64;
			tbHeight.Scroll += new System.EventHandler(ControlValuesChanged);
			labelHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelHeight.Location = new System.Drawing.Point(118, 39);
			labelHeight.Name = "labelHeight";
			labelHeight.Size = new System.Drawing.Size(106, 12);
			labelHeight.TabIndex = 2;
			labelHeight.Text = "Height";
			labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			tbOpaque.Location = new System.Drawing.Point(6, 56);
			tbOpaque.Minimum = 20;
			tbOpaque.Name = "tbOpaque";
			tbOpaque.Size = new System.Drawing.Size(106, 17);
			tbOpaque.TabIndex = 5;
			tbOpaque.Text = "Width";
			tbOpaque.ThumbSize = new System.Drawing.Size(6, 12);
			tbOpaque.TickFrequency = 5;
			tbOpaque.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbOpaque.Value = 20;
			tbOpaque.Scroll += new System.EventHandler(ControlValuesChanged);
			labelOpacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelOpacity.Location = new System.Drawing.Point(6, 76);
			labelOpacity.Name = "labelOpacity";
			labelOpacity.Size = new System.Drawing.Size(106, 12);
			labelOpacity.TabIndex = 4;
			labelOpacity.Text = "Opacity";
			labelOpacity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			tbZoom.Location = new System.Drawing.Point(118, 56);
			tbZoom.Maximum = 500;
			tbZoom.Minimum = 100;
			tbZoom.Name = "tbZoom";
			tbZoom.Size = new System.Drawing.Size(106, 17);
			tbZoom.TabIndex = 7;
			tbZoom.Text = "Width";
			tbZoom.ThumbSize = new System.Drawing.Size(6, 12);
			tbZoom.TickFrequency = 25;
			tbZoom.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbZoom.Value = 100;
			tbZoom.Scroll += new System.EventHandler(ControlValuesChanged);
			labelZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			labelZoom.Location = new System.Drawing.Point(120, 76);
			labelZoom.Name = "labelZoom";
			labelZoom.Size = new System.Drawing.Size(104, 12);
			labelZoom.TabIndex = 6;
			labelZoom.Text = "Zoom";
			labelZoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			chkSimpleStyle.AutoSize = true;
			chkSimpleStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			chkSimpleStyle.Location = new System.Drawing.Point(8, 19);
			chkSimpleStyle.Name = "chkSimpleStyle";
			chkSimpleStyle.Size = new System.Drawing.Size(87, 16);
			chkSimpleStyle.TabIndex = 8;
			chkSimpleStyle.Text = "Simple Style";
			chkSimpleStyle.UseVisualStyleBackColor = true;
			chkSimpleStyle.CheckedChanged += new System.EventHandler(ControlValuesChanged);
			chkAutoHideMagnifier.AutoSize = true;
			chkAutoHideMagnifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			chkAutoHideMagnifier.Location = new System.Drawing.Point(8, 41);
			chkAutoHideMagnifier.Name = "chkAutoHideMagnifier";
			chkAutoHideMagnifier.Size = new System.Drawing.Size(124, 16);
			chkAutoHideMagnifier.TabIndex = 9;
			chkAutoHideMagnifier.Text = "Hide at Page Border";
			chkAutoHideMagnifier.UseVisualStyleBackColor = true;
			chkAutoHideMagnifier.CheckedChanged += new System.EventHandler(ControlValuesChanged);
			chkAutoMagnifier.AutoSize = true;
			chkAutoMagnifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Bold);
			chkAutoMagnifier.Location = new System.Drawing.Point(8, 63);
			chkAutoMagnifier.Name = "chkAutoMagnifier";
			chkAutoMagnifier.Size = new System.Drawing.Size(152, 16);
			chkAutoMagnifier.TabIndex = 10;
			chkAutoMagnifier.Text = "Activate with 'long' Click";
			chkAutoMagnifier.UseVisualStyleBackColor = true;
			chkAutoMagnifier.CheckedChanged += new System.EventHandler(ControlValuesChanged);
			groupBox1.Controls.Add(chkSimpleStyle);
			groupBox1.Controls.Add(chkAutoMagnifier);
			groupBox1.Controls.Add(chkAutoHideMagnifier);
			groupBox1.Location = new System.Drawing.Point(11, 117);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(232, 87);
			groupBox1.TabIndex = 11;
			groupBox1.TabStop = false;
			groupBox2.Controls.Add(tbWidth);
			groupBox2.Controls.Add(tbHeight);
			groupBox2.Controls.Add(labelZoom);
			groupBox2.Controls.Add(labelWidth);
			groupBox2.Controls.Add(labelOpacity);
			groupBox2.Controls.Add(tbOpaque);
			groupBox2.Controls.Add(tbZoom);
			groupBox2.Controls.Add(labelHeight);
			groupBox2.Location = new System.Drawing.Point(11, 3);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(232, 108);
			groupBox2.TabIndex = 12;
			groupBox2.TabStop = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(groupBox2);
			base.Controls.Add(groupBox1);
			base.Name = "MagnifySetupControl";
			base.Size = new System.Drawing.Size(253, 215);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox2.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
