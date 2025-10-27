using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Windows.Forms.Theme;
using cYo.Common.Windows.Forms.Theme.Resources;

namespace cYo.Common.Windows.Forms
{
	public class ComboBoxSkinner
	{
		public interface IComboBoxItem
		{
			bool IsSeparator
			{
				get;
				set;
			}

			bool IsOwnerDrawn
			{
				get;
				set;
			}

			Size Measure(Graphics gr, Font font);

			void Draw(Graphics gr, Rectangle bounds, Color foreColor, Font font);
		}

		public class ComboBoxItem<T> : IComboBoxItem
		{
			public T Item
			{
				get;
				set;
			}

			public bool IsSeparator
			{
				get;
				set;
			}

			public bool IsOwnerDrawn
			{
				get;
				set;
			}

			public ComboBoxItem(T item)
			{
				Item = item;
			}

			public override string ToString()
			{
				try
				{
					return Item.ToString();
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}

			public override int GetHashCode()
			{
				try
				{
					return Item.GetHashCode();
				}
				catch (Exception)
				{
					return 0;
				}
			}

			public static implicit operator T(ComboBoxItem<T> obj)
			{
				return obj.Item;
			}

			public virtual Size Measure(Graphics gr, Font font)
			{
				return new Size(0, gr.MeasureString("M", font).ToSize().Height);
			}

			public virtual void Draw(Graphics gr, Rectangle bounds, Color foreColor, Font font)
			{
			}
		}

		public class ComboBoxItem : ComboBoxItem<object>
		{
			public ComboBoxItem(object item)
				: base(item)
			{
			}
		}

		public class ComboBoxSeparator<T> : ComboBoxItem<T>
		{
			public ComboBoxSeparator(T item)
				: base(item)
			{
				base.IsSeparator = true;
			}
		}

		public class ComboBoxSeparator : ComboBoxSeparator<object>
		{
			public ComboBoxSeparator(object item)
				: base(item)
			{
			}
		}

		private static class Native
		{
			public const int CB_SETDROPPEDWIDTH = 352;

			[DllImport("user32.dll", CharSet = CharSet.Unicode)]
			public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
		}

		private ComboBox comboBox;

		private int verticalItemPadding = 4;

		private int separatorHeight = 3;

		private bool autoSizeDropDown = true;

		public ComboBox ComboBox
		{
			get
			{
				return comboBox;
			}
			set
			{
				if (comboBox != null)
				{
					comboBox.DrawMode = DrawMode.Normal;
					comboBox.MeasureItem -= comboBox_MeasureItem;
					comboBox.DrawItem -= comboBox_DrawItem;
					comboBox.DropDown -= comboBox_Enter;
				}
				comboBox = value;
				if (comboBox != null)
				{
					comboBox.IntegralHeight = false;
					comboBox.DrawMode = DrawMode.OwnerDrawVariable;
					comboBox.MeasureItem += comboBox_MeasureItem;
					comboBox.DrawItem += comboBox_DrawItem;
					comboBox.DropDown += comboBox_Enter;
				}
			}
		}

		[DefaultValue(4)]
		public int VerticalItemPadding
		{
			get
			{
				return verticalItemPadding;
			}
			set
			{
				if (verticalItemPadding != value)
				{
					verticalItemPadding = value;
					comboBox.Invalidate();
				}
			}
		}

		public int SeparatorHeight
		{
			get
			{
				return separatorHeight;
			}
			set
			{
				if (separatorHeight != value)
				{
					separatorHeight = value;
					comboBox.Invalidate();
				}
			}
		}

		[DefaultValue(true)]
		public bool AutoSizeDropDown
		{
			get
			{
				return autoSizeDropDown;
			}
			set
			{
				autoSizeDropDown = value;
			}
		}

		[DefaultValue(1)]
		public int MaxHeightScale { get; set; } = 1;

		[DefaultValue(null)]
		public IImagePackage IconPackage
		{
			get;
			set;
		}

		[DefaultValue(typeof(Size), "24, 24")]
		public Size IconSize
		{
			get;
			set;
		}

		public ComboBoxSkinner()
		{
			IconSize = new Size(24, 24);
		}

		public ComboBoxSkinner(ComboBox comboBox, IImagePackage icons = null)
			: this()
		{
			ComboBox = comboBox;
			IconPackage = icons;
		}

		private void comboBox_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			if (e.Index < 0)
			{
				return;
			}
			ComboBox comboBox = (ComboBox)sender;
			object obj = comboBox.Items[e.Index];
			IComboBoxItem comboBoxItem = obj as IComboBoxItem;
			if (comboBoxItem != null && comboBoxItem.IsOwnerDrawn)
			{
				Size size = comboBoxItem.Measure(e.Graphics, comboBox.Font);
				e.ItemHeight = size.Height;
				e.ItemWidth = size.Width;
			}
			else
			{
				Size size2 = e.Graphics.MeasureString(comboBox.GetItemText(obj), comboBox.Font).ToSize();
				int num = e.Graphics.MeasureString("M", comboBox.Font).ToSize().Height;
				int num2 = 0;
				if (IconPackage != null)
				{
					if (IconPackage.ImageExists(comboBox.GetItemText(obj)))
					{
						num = Math.Max(num, IconSize.Height);
					}
					num2 = IconSize.Width;
				}
				e.ItemHeight = Math.Max(size2.Height, num);
				e.ItemWidth = size2.Width + num2;
			}
			e.ItemHeight += verticalItemPadding;
			if (comboBoxItem != null && comboBoxItem.IsSeparator)
			{
				e.ItemHeight += separatorHeight;
			}
		}

		private void comboBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
			{
				return;
			}
			ComboBox comboBox = (ComboBox)sender;
			object obj = comboBox.Items[e.Index];
			IComboBoxItem comboBoxItem = obj as IComboBoxItem;
			bool flag = (e.State & DrawItemState.ComboBoxEdit) != 0;
			bool flag2 = comboBoxItem != null && comboBoxItem.IsSeparator && !flag && e.Index > 0;

            e.DrawThemeBackground();
			//e.DrawFocusRectangle();
			//e.DrawThemeBackground();
			e.DrawThemeFocusRectangle(); // override SelectedText highlighting

			using (Brush brush = new SolidBrush(e.ForeColor))
			{
				Rectangle rectangle = e.Bounds;
				if (flag2)
				{
					rectangle = rectangle.Pad(0, separatorHeight);
				}
				if (IconPackage != null)
				{
					Image image = IconPackage.GetImage(comboBox.GetItemText(obj));
					if (image != null)
					{
						using (e.Graphics.HighQuality(enabled: true))
						{
							e.Graphics.DrawImage(image, new Rectangle(rectangle.Left, rectangle.Top, IconSize.Width, IconSize.Height));
						}
					}
					rectangle = rectangle.Pad(IconSize.Width, 0);
				}
				if (comboBoxItem != null && comboBoxItem.IsOwnerDrawn)
				{
					comboBoxItem.Draw(e.Graphics, rectangle, e.ForeColor, comboBox.Font);
				}
				else
				{
					using (StringFormat format = new StringFormat(StringFormatFlags.NoWrap)
					{
						LineAlignment = StringAlignment.Center,
						Alignment = StringAlignment.Near
					})
					{
						e.Graphics.DrawString(comboBox.GetItemText(obj), comboBox.Font, brush, rectangle, format);
					}
				}
				if (flag2)
				{
					Rectangle rect = new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, separatorHeight);
					using (Brush brush2 = new SolidBrush(comboBox.BackColor))
					{
						e.Graphics.FillRectangle(brush2, rect);
					}
					e.Graphics.DrawLine(ThemePens.ComboBox.Separator, rect.Left + 2, rect.Top + 1, rect.Right - 2, rect.Top + 1);
				}
			}
		}

		private void comboBox_Enter(object sender, EventArgs e)
		{
			if (autoSizeDropDown)
			{
				SizeDropDown();
			}
		}

		public static IList AutoSeparatorList(IEnumerable st)
		{
			ArrayList arrayList = new ArrayList();
			bool flag = false;
			foreach (object item in st)
			{
				if (item.ToString() == "-")
				{
					flag = true;
					continue;
				}
				if (flag)
				{
					arrayList.Add(new ComboBoxSeparator(item));
				}
				else
				{
					arrayList.Add(item);
				}
				flag = false;
			}
			return arrayList;
		}

		public void SizeDropDown()
		{
			if (ComboBox == null)
			{
				return;
			}
			int num = 0;
			using (Graphics graphics = ComboBox.CreateGraphics())
			{
				foreach (object item in ComboBox.Items)
				{
					num = Math.Max(num, graphics.MeasureString(ComboBox.GetItemText(item), ComboBox.Font).ToSize().Width);
				}
			}
			Native.SendMessage(ComboBox.Handle, 352u, num, 0);
			ComboBox.DropDownHeight = MaxHeightScale * 150;
		}
	}
}
