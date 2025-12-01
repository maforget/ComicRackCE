using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Text;
using cYo.Common.Windows.Forms.Theme;

namespace cYo.Common.Windows.Forms
{
	public class SimpleColorPicker : ComboBox
	{
		private IContainer components;

		public string SelectedColorName
		{
			get
			{
				return SelectedColor.Name;
			}
			set
			{
				SelectedColor = Color.FromName(value);
			}
		}

		public Color SelectedColor
		{
			get
			{
				if (base.SelectedItem != null)
				{
					return (Color)base.SelectedItem;
				}
				return Color.Empty;
			}
			set
			{
				foreach (Color item in base.Items)
				{
					if (item == value)
					{
						base.SelectedItem = item;
						break;
					}
				}
			}
		}

		public SimpleColorPicker()
		{
			InitializeComponent();
			base.DrawMode = DrawMode.OwnerDrawFixed;
			base.Items.Clear();
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);
			Graphics graphics = e.Graphics;
            //e.DrawBackground();
            e.DrawThemeBackground();
            // does not draw a FocusRectangle in light mode, maintaining current functionality
            e.DrawThemeFocusRectangle();
            using (StringFormat format = new StringFormat
			{
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center
			})
			{
				try
				{
					Color color = (Color)base.Items[e.Index];
					Rectangle bounds = e.Bounds;
					bounds.Width = bounds.Height * 3 / 2;
					bounds.Inflate(-2, -2);
					using (Brush brush = new SolidBrush(color))
					{
						graphics.FillRectangle(brush, bounds);
					}
					using (Pen pen = new Pen(e.ForeColor))
					{
						graphics.DrawRectangle(pen, bounds);
					}
					Rectangle bounds2 = e.Bounds;
					bounds2.X = 5 + bounds.Right;
					bounds2.Width = e.Bounds.Width - bounds2.X;
					using (Brush brush2 = new SolidBrush(e.ForeColor))
					{
						string s = color.ToKnownColor().ToString().PascalToSpaced();
						graphics.DrawString(s, e.Font, brush2, bounds2, format);
					}
				}
				catch (Exception)
				{
					using (Brush brush3 = new SolidBrush(e.ForeColor))
					{
						graphics.DrawString("Unknown Color", e.Font, brush3, e.Bounds, format);
					}
				}
			}
		}

		public void FillKnownColors(bool includingSystem)
		{
			foreach (KnownColor value in Enum.GetValues(typeof(KnownColor)))
			{
				Color color2 = Color.FromKnownColor(value);
				if (color2.A == byte.MaxValue && (!color2.IsSystemColor || (color2.IsSystemColor && includingSystem)))
				{
					base.Items.Add(color2);
				}
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
			components = new System.ComponentModel.Container();
		}
	}
}
