using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Runtime;
using cYo.Common.Text;

namespace cYo.Common.Windows.Forms
{
	public class KeySearch : Component
	{
		private readonly Func<string, bool> select;

		private long lastTime;

		private string currentText = string.Empty;

		private int searchDelay = 2500;

		public string CurrentText => currentText;

		public int SearchDelay
		{
			get
			{
				return searchDelay;
			}
			set
			{
				searchDelay = value;
			}
		}

		public KeySearch(Func<string, bool> select)
		{
			this.select = select;
		}

		public bool Select(char c)
		{
			long ticks = Machine.Ticks;
			if (ticks > lastTime + SearchDelay)
			{
				currentText = string.Empty;
			}
			string arg = ((c != '\b') ? (CurrentText + c) : (string.IsNullOrEmpty(CurrentText) ? string.Empty : CurrentText.Substring(0, CurrentText.Length - 1)));
			bool flag = select(arg);
			if (flag)
			{
				currentText = arg;
			}
			lastTime = ticks;
			return flag;
		}

		public void Reset()
		{
			lastTime = 0L;
			currentText = string.Empty;
		}

		public static void Create(ListView listView)
		{
			Create(listView, ignoreAricles: true);
		}

		public static void Create(ListView listView, bool ignoreAricles)
		{
			KeySearch ks = new KeySearch(delegate(string s)
			{
				ListViewItem li = listView.Enumerate().FirstOrDefault((ListViewItem item) => item.Text.StartsWith(s, StringComparison.OrdinalIgnoreCase, ignoreAricles));
				if (li == null)
				{
					return false;
				}
				if (li == listView.FocusedItem)
				{
					return true;
				}
				try
				{
					listView.BeginUpdate();
					li.EnsureVisible();
					li.Focused = true;
					listView.Enumerate().ForEach(delegate(ListViewItem item)
					{
						item.Selected = item == li;
					});
				}
				finally
				{
					listView.EndUpdate();
				}
				return true;
			});
			RegisterKeys(listView, ks);
		}

		public static void Create(ItemView itemView)
		{
			Create(itemView, ignoreAricles: true);
		}

		public static void Create(ItemView itemView, bool ignoreAricles)
		{
			KeySearch ks = new KeySearch(delegate(string s)
			{
				IViewableItem viewableItem = itemView.DisplayedItems.FirstOrDefault((IViewableItem item) => item.Text.StartsWith(s, StringComparison.OrdinalIgnoreCase, ignoreAricles));
				if (viewableItem == null)
				{
					return false;
				}
				if (viewableItem == itemView.FocusedItem)
				{
					return true;
				}
				itemView.BeginUpdate();
				try
				{
					itemView.EnsureItemVisible(viewableItem);
					itemView.SelectAll(selectionState: false);
					itemView.Select(viewableItem);
					itemView.FocusedItem = viewableItem;
				}
				finally
				{
					itemView.EndUpdate();
				}
				return true;
			});
			RegisterKeys(itemView, ks);
		}

		private static void RegisterKeys(Control control, KeySearch ks)
		{
			ToolTip toolTip = new ToolTip();
			control.KeyPress += delegate(object s, KeyPressEventArgs e)
			{
				if (!e.Handled)
				{
					e.Handled = ks.Select(e.KeyChar);
					if (e.Handled)
					{
						toolTip.Show(TR.Default["Search", "Search"] + ": " + ks.CurrentText, control, 0, 0, 3000);
					}
				}
			};
			control.Disposed += delegate
			{
				toolTip.Dispose();
			};
		}
	}
}
