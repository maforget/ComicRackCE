using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows.Forms;

namespace cYo.Common.Windows
{
	public static class LocalizeUtility
	{
		public static void Localize(TR tr, ToolStripItemCollection tsic)
		{
			foreach (ToolStripItem item in tsic)
			{
				Localize(tr, item);
			}
			FormUtility.PrefixToolStrip(tsic);
		}

		public static void Localize(TR tr, ToolStripItem tsi)
		{
			if (tsi is ToolStripTextBox || tsi is ToolStripComboBox)
			{
				return;
			}
			bool flag = tsi.ToolTipText != tsi.Text;
			tsi.Text = tr[tsi.Name, tsi.Text];
			tsi.ToolTipText = (flag ? tr[tsi.Name + ".Tooltip", tsi.ToolTipText] : tsi.Text);
			ToolStripDropDownItem toolStripDropDownItem = tsi as ToolStripDropDownItem;
			if (toolStripDropDownItem != null && toolStripDropDownItem.HasDropDownItems)
			{
				Localize(tr, toolStripDropDownItem.DropDownItems);
				return;
			}
			ToolStripDropDownButton toolStripDropDownButton = tsi as ToolStripDropDownButton;
			if (toolStripDropDownButton != null && toolStripDropDownButton.HasDropDownItems)
			{
				Localize(tr, toolStripDropDownButton.DropDownItems);
			}
		}

		public static void Localize(TR tr, ToolStrip ts)
		{
			Localize(tr, ts.Items);
		}

		public static void Localize(TR tr, Control c)
		{
			if (c is Label || c is GroupBox || c is CollapsibleGroupBox || c is ButtonBase || c is Form)
			{
				c.Text = tr[c.Name, c.Text];
			}
			else if (c is ToolStrip)
			{
				Localize(tr, (ToolStrip)c);
			}
			else if (c is TabControl)
			{
				TabControl tabControl = c as TabControl;
				foreach (TabPage tabPage in tabControl.TabPages)
				{
					tabPage.Text = tr[tabPage.Name, tabPage.Text];
				}
			}
			else if (c is TabBar)
			{
				TabBar tabBar = c as TabBar;
				foreach (TabBar.TabBarItem item in tabBar.Items)
				{
					item.Text = tr[item.Name, item.Text];
					item.ToolTipText = tr[item.Name + ".Tooltip", item.ToolTipText];
				}
			}
			else if (c is ListView)
			{
				ListView listView = (ListView)c;
				foreach (ColumnHeader column in listView.Columns)
				{
					column.Text = tr["col" + column.Text, column.Text];
				}
				foreach (ListViewGroup group in listView.Groups)
				{
					group.Header = tr[group.Name, group.Header];
				}
			}
			else if (c is DataGridView)
			{
				DataGridView dataGridView = (DataGridView)c;
				foreach (DataGridViewColumn column2 in dataGridView.Columns)
				{
					column2.HeaderText = tr[column2.Name, column2.HeaderText];
				}
			}
			foreach (Control control in c.Controls)
			{
				if (!(control is UserControl))
				{
					Localize(tr, control);
				}
			}
		}

		public static void Localize(TR tr, ComboBox cb)
		{
			for (int i = 0; i < cb.Items.Count; i++)
			{
				cb.Items[i] = tr[cb.Name + ".Item" + i, cb.Items[i] as string];
			}
		}

		public static void Localize(Control control, string controlName, IContainer components)
		{
			TR tr = TR.Load(controlName);
			Localize(tr, control);
			if (components == null || components.Components == null)
			{
				return;
			}
			foreach (ToolStrip item in components.Components.OfType<ToolStrip>())
			{
				Localize(tr, item);
			}
		}

		public static void Localize(Control control, IContainer components)
		{
			Localize(control, control.Name, components);
		}

		public static string GetText(Control control, string key, string value)
		{
			return TR.Load(control.GetType().Name)[key, value];
		}

		public static void UpdateRightToLeft(Form f)
		{
			if (TR.Info.RightToLeft)
			{
				f.RightToLeft = RightToLeft.Yes;
			}
		}

		public static string LocalizeEnum(Type enumType, int value)
		{
			string name = Enum.GetName(enumType, value);
			return TR.Load(enumType.Name)[name, GetEnumDescription(enumType, name).PascalToSpaced()];
		}

		private static string GetEnumDescription(Type enumType, string name)
		{
			FieldInfo field = enumType.GetField(name);
			DescriptionAttribute descriptionAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).Cast<DescriptionAttribute>().FirstOrDefault();
			if (descriptionAttribute != null)
			{
				return descriptionAttribute.Description;
			}
			return name;
		}
	}
}
