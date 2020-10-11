using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Localize;

namespace cYo.Common.Windows.Forms
{
	public class EnumMenuUtility
	{
		private readonly bool flagsMode;

		private readonly bool isFlags;

		private readonly Type enumType;

		private readonly ToolStripItem[] items;

		private int enumValue;

		public bool FlagsMode => flagsMode;

		public bool IsFlags => isFlags;

		public Type EnumType => enumType;

		public IEnumerable<ToolStripItem> Items => items;

		public string Text
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (object value in Enum.GetValues(enumType))
				{
					int num = Convert.ToInt32(value);
					if (((isFlags && BitUtility.GetBitCount(num) == 1) || !isFlags) && (Value & num) == num)
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(CultureInfo.CurrentCulture.TextInfo.ListSeparator);
						}
						stringBuilder.Append(LocalizeUtility.LocalizeEnum(enumType, num));
					}
				}
				return stringBuilder.ToString();
			}
		}

		public int Value
		{
			get
			{
				return enumValue;
			}
			set
			{
				if (enumValue != value)
				{
					enumValue = value;
					OnValueChanged();
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return items.All((ToolStripItem ti) => ti.Enabled);
			}
			set
			{
				items.ForEach(delegate(ToolStripItem ti)
				{
					ti.Enabled = value;
				});
			}
		}

		public event EventHandler ValueChanged;

		protected EnumMenuUtility(Type enumType, bool flagsMode, IDictionary<int, Image> images, Keys startKey)
		{
			this.enumType = enumType;
			isFlags = Attribute.IsDefined(enumType, typeof(FlagsAttribute));
			this.flagsMode = flagsMode && isFlags;
			items = MakeEnumMenu(images, startKey);
		}

		public EnumMenuUtility(ToolStripDropDownItem item, Type enumType, bool flagsMode, IDictionary<int, Image> images, Keys startKey)
			: this(enumType, flagsMode, images, startKey)
		{
			item.DropDownItems.AddRange(items);
			item.DropDownOpening += DropDownOpening;
		}

		public EnumMenuUtility(ToolStripDropDown cms, Type enumType, bool flagsMode, IDictionary<int, Image> images, Keys startKey)
			: this(enumType, flagsMode, images, startKey)
		{
			cms.Items.AddRange(items);
			cms.Opening += ContextMenuOpening;
		}

		protected virtual void OnValueChanged()
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, EventArgs.Empty);
			}
		}

		private ToolStripItem[] MakeEnumMenu(IDictionary<int, Image> images, Keys startKey)
		{
			List<ToolStripItem> list = new List<ToolStripItem>();
			foreach (object value in Enum.GetValues(enumType))
			{
				int num = Convert.ToInt32(value);
				string name = Enum.GetName(enumType, value);
				BrowsableAttribute browsableAttribute = enumType.GetField(name).GetCustomAttributes(typeof(BrowsableAttribute), inherit: true).OfType<BrowsableAttribute>()
					.FirstOrDefault();
				if ((browsableAttribute == null || browsableAttribute.Browsable) && ((isFlags && BitUtility.GetBitCount(num) == 1) || !isFlags))
				{
					ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem
					{
						Text = LocalizeUtility.LocalizeEnum(enumType, num),
						Tag = num
					};
					toolStripMenuItem.Click += EnumClicked;
					if (images != null)
					{
						toolStripMenuItem.Image = images[num];
					}
					if (startKey != 0)
					{
						toolStripMenuItem.ShortcutKeys = startKey + list.Count;
					}
					list.Add(toolStripMenuItem);
				}
			}
			if (flagsMode)
			{
				list.Add(new ToolStripSeparator());
				list.Add(new ToolStripMenuItem(TR.Default["SetAll", "Set all"], null, EnumAllClicked));
				list.Add(new ToolStripMenuItem(TR.Default["ClearAll", "Clear all"], null, EnumNoneClicked));
				list.Add(new ToolStripMenuItem(TR.Default["Invert", "Invert"], null, EnumInvertClicked));
			}
			return list.ToArray();
		}

		private int GetFlagValue()
		{
			int num = 0;
			foreach (object value in Enum.GetValues(enumType))
			{
				if (!value.Equals(uint.MaxValue))
				{
					num |= Convert.ToInt32(value);
				}
			}
			return num;
		}

		private void UpdateItems()
		{
			foreach (ToolStripMenuItem item in items.OfType<ToolStripMenuItem>())
			{
				if (item.Tag != null)
				{
					int num = (int)item.Tag;
					if (flagsMode)
					{
						item.Checked = (Value & num) == num;
					}
					else
					{
						item.Checked = Value.Equals(item.Tag);
					}
				}
			}
		}

		private void EnumClicked(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
			if (flagsMode)
			{
				Value = enumValue ^ Convert.ToInt32(toolStripMenuItem.Tag);
			}
			else
			{
				Value = (int)toolStripMenuItem.Tag;
			}
		}

		private void EnumAllClicked(object sender, EventArgs e)
		{
			Value = GetFlagValue();
		}

		private void EnumNoneClicked(object sender, EventArgs e)
		{
			Value = 0;
		}

		private void EnumInvertClicked(object sender, EventArgs e)
		{
			Value = ~Value;
		}

		private void DropDownOpening(object sender, EventArgs e)
		{
			UpdateItems();
		}

		private void ContextMenuOpening(object sender, CancelEventArgs e)
		{
			UpdateItems();
		}

		public override string ToString()
		{
			return Text;
		}
	}
}
