using cYo.Common.Windows.Forms.Theme;
using cYo.Common.Windows.Forms.Theme.Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace cYo.Common.Windows.Forms
{
	[Designer(typeof(ControlDesigner))]
	public class SearchTextBox : ToolStrip
	{
		private class MyLayout : LayoutEngine
		{
			public override bool Layout(object container, LayoutEventArgs layoutEventArgs)
			{
				SearchTextBox searchTextBox = container as SearchTextBox;
				Rectangle clientRectangle = searchTextBox.ClientRectangle;
				Size size = new Size(clientRectangle.Height, clientRectangle.Height);
				if (searchTextBox.Items.Count == 3)
				{
					ToolStripItem searchButton = searchTextBox.searchButton;
					ToolStripItem clearButton = searchTextBox.clearButton;
					ToolStripItem textBox = searchTextBox.textBox;
					searchTextBox.SetItemLocation(searchButton, clientRectangle.Location);
					searchButton.Size = new Size((int)((double)size.Width * 1.5), size.Height);
					int num = (searchButton.Visible ? searchButton.Width : 2);
					clearButton.Size = size;
					searchTextBox.SetItemLocation(clearButton, new Point(clientRectangle.Width - clearButton.Width, clientRectangle.Y));
					int num2 = clientRectangle.Width - num - 2;
					if (clearButton.Visible)
					{
						num2 -= clearButton.Width;
					}
					textBox.Size = new Size(num2, size.Height);
					searchTextBox.SetItemLocation(textBox, new Point(clientRectangle.X + num + 1, clientRectangle.Y));
				}
				return false;
			}
		}

		public class MyRenderer : ThemeToolStripProRenderer
        {
			public MyRenderer()
				: base((ToolStripManager.Renderer is ToolStripProfessionalRenderer) ? ((ToolStripProfessionalRenderer)ToolStripManager.Renderer).ColorTable : null)
			{
			}

			protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
			{
				base.OnRenderToolStripBorder(e);
				// themed textbox doesn't fit in toolstrip height, so no border
				//ControlPaint.DrawBorder(e.Graphics, e.AffectedBounds, Color.Red, ButtonBorderStyle.Solid);
				ThemeExtensions.InvokeAction(() => ControlPaint.DrawBorder3D(e.Graphics, e.AffectedBounds, Border3DStyle.Flat), isDefaultAction: true);
            }

			protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
			{
				base.OnRenderToolStripBackground(e);
				e.Graphics.Clear(SystemColors.Window);
			}
        }

		private readonly ToolStripDropDownButton searchButton = new ToolStripDropDownButton();

		private readonly ToolStripTextBox textBox = new ToolStripTextBox();

		private readonly ToolStripButton clearButton = new ToolStripButton();

		private readonly LayoutEngine myLayout = new MyLayout();

		private bool quickSelectAll;

		private string cueText;

		public override LayoutEngine LayoutEngine => myLayout;

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				string text3 = (textBox.Text = (base.Text = value));
			}
		}

		public Image ClearButtonImage
		{
			get
			{
				return clearButton.Image;
			}
			set
			{
				clearButton.Image = value;
			}
		}

		public Image SearchButtonImage
		{
			get
			{
				return searchButton.Image;
			}
			set
			{
				searchButton.Image = value;
			}
		}

		public ToolStripDropDown SearchMenu
		{
			get
			{
				return searchButton.DropDown;
			}
			set
			{
				searchButton.DropDown = value;
			}
		}

		public bool SearchButtonVisible
		{
			get
			{
				return searchButton.Visible;
			}
			set
			{
				searchButton.Visible = value;
			}
		}

		public AutoCompleteStringCollection AutoCompleteList
		{
			get
			{
				return textBox.AutoCompleteCustomSource;
			}
			set
			{
				textBox.AutoCompleteCustomSource = value;
			}
		}

		public SearchTextBox()
		{
			base.GripStyle = ToolStripGripStyle.Hidden;
			textBox.BorderStyle = BorderStyle.None;
			textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
			textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
			clearButton.Visible = false;
			clearButton.ImageScaling = ToolStripItemImageScaling.None;
			clearButton.ImageAlign = ContentAlignment.MiddleCenter;
			searchButton.ImageScaling = ToolStripItemImageScaling.None;
			searchButton.ImageAlign = ContentAlignment.MiddleCenter;
			Items.Add(searchButton);
			Items.Add(textBox);
			Items.Add(clearButton);
			base.Renderer = new MyRenderer();
			textBox.TextChanged += textBox_TextChanged;
			textBox.Enter += textBox_Enter;
			textBox.Leave += textBox_Leave;
			textBox.MouseDown += textBox_MouseDown;
			textBox.MouseUp += textBox_MouseUp;
			textBox.KeyDown += textBox_KeyDown;
			clearButton.Click += clearButton_Click;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				clearButton.Dispose();
				searchButton.Dispose();
				textBox.Dispose();
			}
			base.Dispose(disposing);
		}

		private void textBox_TextChanged(object sender, EventArgs e)
		{
			Text = textBox.Text;
			clearButton.Visible = !string.IsNullOrEmpty(Text);
		}

		private void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			quickSelectAll = false;
			if (e.KeyCode == Keys.Down)
			{
				searchButton.ShowDropDown();
			}
		}

		private void textBox_MouseUp(object sender, MouseEventArgs e)
		{
			quickSelectAll = false;
		}

		private void textBox_Leave(object sender, EventArgs e)
		{
			quickSelectAll = false;
			UpdateAutoComplete();
		}

		private void textBox_MouseDown(object sender, MouseEventArgs e)
		{
			if (quickSelectAll)
			{
				textBox.SelectAll();
			}
		}

		private void textBox_Enter(object sender, EventArgs e)
		{
			if (textBox.Text.Length > 0)
			{
				textBox.SelectAll();
				quickSelectAll = true;
			}
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			UpdateAutoComplete();
			Text = string.Empty;
			textBox.Focus();
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			textBox.Focus();
		}

		public void SetCueText(string text)
		{
			if (!(text == cueText))
			{
				cueText = text;
				textBox.TextBox.SetCueText(text);
			}
		}

		private void UpdateAutoComplete()
		{
			string text = textBox.Text;
			if (!string.IsNullOrEmpty(text))
			{
				textBox.AutoCompleteCustomSource.Add(text);
			}
		}
	}
}
