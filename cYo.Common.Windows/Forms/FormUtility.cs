using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Reflection;
using cYo.Common.Text;
using cYo.Common.Win32;

namespace cYo.Common.Windows.Forms
{
	public static class FormUtility
	{
		private class GroupHeaderComparer : IComparer
		{
			public int Compare(object x, object y)
			{
				ListViewGroup listViewGroup = x as ListViewGroup;
				ListViewGroup listViewGroup2 = y as ListViewGroup;
				return string.Compare(listViewGroup.Header, listViewGroup2.Header);
			}
		}

		private class PanelState
		{
			public string Name
			{
				get;
				set;
			}

			public Point AutoScrollPosition
			{
				get;
				set;
			}

			public bool[] Collapsed
			{
				get;
				set;
			}
		}

		public static readonly Dictionary<object, object> ServiceTranslation = new Dictionary<object, object>();

		private static Dictionary<string, Rectangle> formPositions;

		private static Dictionary<string, IEnumerable<PanelState>> panelStates;

		private const int LOGPIXELSX = 88;

		private const int LOGPIXELSY = 90;

		private static PointF dpiScale = PointF.Empty;

		public static Dictionary<string, Rectangle> FormPositions
		{
			get
			{
				if (formPositions == null)
				{
					formPositions = new Dictionary<string, Rectangle>();
				}
				return formPositions;
			}
		}

		public static PointF DpiScale
		{
			get
			{
				if (!dpiScale.IsEmpty)
				{
					return dpiScale;
				}
				if (!IsProcessDPIAware())
				{
					dpiScale = new PointF(1f, 1f);
				}
				else
				{
					IntPtr dC = GetDC(IntPtr.Zero);
					Size size = new Size(GetDeviceCaps(dC, LOGPIXELSX), GetDeviceCaps(dC, LOGPIXELSY));
					dpiScale = new PointF((float)size.Width / 96f, (float)size.Height / 96f);
				}
				return dpiScale;
			}
		}

		public static object FindActiveService(this Control root, Type service)
		{
			if (service != null && root != null)
			{
				ContainerControl containerControl = root as ContainerControl;
				if (containerControl != null)
				{
					Control activeControl = containerControl.ActiveControl;
					if (activeControl != null && activeControl.Visible)
					{
						object obj = activeControl.FindActiveService(service);
						if (obj != null)
						{
							return obj;
						}
					}
				}
				if (service.IsInstanceOfType(GetServiceObject(root)))
				{
					return GetServiceObject(root);
				}
				foreach (Control control in root.Controls)
				{
					if (control != null && control.Visible)
					{
						object obj2 = control.FindActiveService(service);
						if (obj2 != null)
						{
							return obj2;
						}
					}
				}
			}
			return null;
		}

		public static T FindActiveService<T>(this Control root) where T : class
		{
			return (T)root.FindActiveService(typeof(T));
		}

		public static K InvokeActiveService<T, K>(this Control root, Func<T, K> predicate, K defaultReturn = default(K)) where T : class
		{
			T val = root.FindActiveService<T>();
			if (val != null)
			{
				return predicate(val);
			}
			return defaultReturn;
		}

		public static void InvokeActiveService<T>(this Control root, Action<T> action) where T : class
		{
			T val = root.FindActiveService<T>();
			if (val != null)
			{
				action(val);
			}
		}

		public static IEnumerable<T> FindServices<T>(this Control root) where T : class
		{
			T val = GetServiceObject(root) as T;
			if (val != null)
			{
				yield return val;
			}
			foreach (Control control in root.Controls)
			{
				foreach (T item in control.FindServices<T>())
				{
					yield return item;
				}
			}
		}

		public static T FindFirstService<T>(this Control root) where T : class
		{
			return root.FindServices<T>().FirstOrDefault();
		}

		[DllImport("user32.dll")]
		private static extern IntPtr GetActiveWindow();

		public static T FindActiveService<T>() where T : class
		{
			return (Form.ActiveForm ?? Control.FromHandle(GetActiveWindow())).FindActiveService<T>();
		}

		public static T FindParentService<T>(this Control c) where T : class
		{
			while ((c = c.Parent) != null)
			{
				T val = c as T;
				if (val != null)
				{
					return val;
				}
			}
			return null;
		}

		private static T GetAttribute<T>(this MemberInfo pi) where T : class
		{
			return Attribute.GetCustomAttribute(pi, typeof(T)) as T;
		}

		public static void FillPanelWithOptions(Control panel, object data, TR tr, bool rebuild = false)
		{
			if (panel.Tag is TabControl)
			{
				panel = (Control)panel.Tag;
			}
			if (rebuild)
			{
				panel.Clear(withDispose: true);
			}
			PropertyInfo[] properties = data.GetType().GetProperties();
			IEnumerable<string> enumerable = properties.Select((PropertyInfo p) => p.Category()).Distinct();
			foreach (string cat in enumerable)
			{
				string caption = tr[cat ?? "Other"];
				CollapsibleGroupBox collapsibleGroupBox = panel.Controls.OfType<CollapsibleGroupBox>().FirstOrDefault((CollapsibleGroupBox cp) => cp.Text == caption);
				var array = (from p in properties
					where p.PropertyType == typeof(bool) && p.Browsable() && p.Category() == cat
					select new
					{
						Property = p,
						Description = tr[p.Name, p.Description()]
					} into c
					where !string.IsNullOrEmpty(c.Description)
					orderby c.Description
					select c).ToArray();
				var array2 = array;
				foreach (var pi in array2)
				{
					CheckBox checkBox = panel.GetControls<CheckBox>().FirstOrDefault((CheckBox cb) => pi.Property.Name.Equals(cb.Tag));
					if (checkBox == null)
					{
						if (collapsibleGroupBox == null)
						{
							collapsibleGroupBox = new CollapsibleGroupBox
							{
								Dock = DockStyle.Top,
								Text = caption,
								Tag = cat
							};
							panel.Controls.Add(collapsibleGroupBox);
							panel.Controls.SetChildIndex(collapsibleGroupBox, 0);
						}
						checkBox = new CheckBox
						{
							AutoSize = true,
							Text = pi.Description,
							Tag = pi.Property.Name
						};
						collapsibleGroupBox.Controls.Add(checkBox);
						collapsibleGroupBox.Height = ScaleDpiY(30) + collapsibleGroupBox.Controls.Count * (checkBox.Height + ScaleDpiY(2)) + ScaleDpiY(10);
						checkBox.Left = ScaleDpiX(20);
						checkBox.Top = collapsibleGroupBox.Height - ScaleDpiY(10) - checkBox.Height - ScaleDpiY(1);
					}
					checkBox.Checked = (bool)pi.Property.GetValue(data, null);
				}
			}
		}

		public static void RetrieveOptionsFromPanel(Control panel, object data)
		{
			if (panel.Tag is TabControl)
			{
				panel = (Control)panel.Tag;
			}
			foreach (CheckBox control in panel.GetControls<CheckBox>())
			{
				try
				{
					data.GetType().GetProperty((string)control.Tag).SetValue(data, control.Checked, null);
				}
				catch
				{
				}
			}
		}

		public static void FillListWithOptions(ListView lv, object data, TR tr)
		{
			lv.BeginUpdate();
			try
			{
				lv.Items.Clear();
				lv.CheckBoxes = true;
				lv.ShowGroups = true;
				lv.View = View.Details;
				PropertyInfo[] properties = data.GetType().GetProperties();
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (!(propertyInfo.PropertyType != typeof(bool)) && (propertyInfo.GetAttribute<BrowsableAttribute>() == null || propertyInfo.GetAttribute<BrowsableAttribute>().Browsable))
					{
						DescriptionAttribute attribute = propertyInfo.GetAttribute<DescriptionAttribute>();
						CategoryAttribute attribute2 = propertyInfo.GetAttribute<CategoryAttribute>();
						string key = ((attribute2 == null || string.IsNullOrEmpty(attribute2.Category)) ? "Other" : attribute2.Category);
						if (attribute != null && !string.IsNullOrEmpty(attribute.Description))
						{
							ListViewItem listViewItem = lv.Items.Add(tr[propertyInfo.Name, attribute.Description]);
							listViewItem.Checked = (bool)propertyInfo.GetValue(data, null);
							listViewItem.Group = lv.Groups[key] ?? lv.Groups.Add(key, tr[key]);
							listViewItem.Tag = propertyInfo.Name;
						}
					}
				}
			}
			finally
			{
				lv.EndUpdate();
			}
		}

		public static void FillListWithOptions(ListView lv, object data)
		{
			FillListWithOptions(lv, data, new TR());
		}

		public static void RetrieveOptionsFromList(ListView lv, object data)
		{
			foreach (ListViewItem item in lv.Items)
			{
				try
				{
					data.GetType().GetProperty((string)item.Tag).SetValue(data, item.Checked, null);
				}
				catch
				{
				}
			}
		}

		public static void AutoResizeColumn(this ListView listView, int autoSizeColumnIndex, int spaces = 0)
		{
			// Do some rudimentary (parameter) validation.
			if (listView == null) throw new ArgumentNullException("listView");
			if (listView.View != View.Details || listView.Columns.Count <= 0 || autoSizeColumnIndex < 0) return;
			if (autoSizeColumnIndex >= listView.Columns.Count)
				throw new IndexOutOfRangeException("Parameter autoSizeColumnIndex is outside the range of column indices in the ListView.");

			// Sum up the width of all columns except the auto-resizing one.
			int otherColumnsWidth = 0;
			foreach (ColumnHeader header in listView.Columns)
				if (header.Index != autoSizeColumnIndex)
					otherColumnsWidth += header.Width;

			// Calculate the (possibly) new width of the auto-resizable column.
			int autoSizeColumnWidth = listView.ClientRectangle.Width - otherColumnsWidth - spaces;

			// Finally set the new width of the auto-resizing column, if it has changed.
			if (listView.Columns[autoSizeColumnIndex].Width != autoSizeColumnWidth)
				listView.Columns[autoSizeColumnIndex].Width = autoSizeColumnWidth;
		}

		public static IEnumerable<ListViewItem> Enumerate(this ListView listView)
		{
			for (int i = 0; i < listView.Items.Count; i++)
			{
				yield return listView.Items[i];
			}
		}

		public static void SortGroups(this ListView listView, IComparer comparer = null)
		{
			if (comparer == null)
			{
				comparer = new GroupHeaderComparer();
			}
			ArrayList arrayList = new ArrayList(listView.Groups);
			arrayList.Sort(comparer);
			listView.Groups.Clear();
			foreach (ListViewGroup item in arrayList)
			{
				listView.Groups.Add(item);
			}
		}

		public static IEnumerable<TreeNode> All(this TreeNodeCollection nodes)
		{
			foreach (TreeNode tn in nodes)
			{
				yield return tn;
				foreach (TreeNode item in tn.Nodes.All())
				{
					yield return item;
				}
			}
		}

		public static IEnumerable<TreeNode> AllNodes(this TreeView tree)
		{
			return tree.Nodes.All();
		}

		public static TreeNode Find(this TreeNodeCollection tnc, Predicate<TreeNode> pred, bool all = true)
		{
			foreach (TreeNode item in tnc)
			{
				if (pred(item))
				{
					return item;
				}
				if (all)
				{
					TreeNode treeNode2 = item.Nodes.Find(pred);
					if (treeNode2 != null)
					{
						return treeNode2;
					}
				}
			}
			return null;
		}

		public static void AddRange<T>(this ComboBox.ObjectCollection collection, IEnumerable<T> list)
		{
			foreach (T item in list)
			{
				collection.Add(item);
			}
		}

		public static IEnumerable<T> GetControls<T>(this Control container, bool all = true) where T : Control
		{
			if (!all)
			{
				return container.Controls.OfType<T>();
			}
			return container.Controls.Recurse<T>((object o) => ((Control)o).Controls);
		}

		public static void ForEachControl<T>(this Control container, Action<T> action, bool all = true) where T : Control
		{
			container.GetControls<T>(all).ForEach(action);
		}

		public static void Clear(this Control control, bool withDispose)
		{
			for (int num = control.Controls.Count - 1; num >= 0; num--)
			{
				Control control2 = control.Controls[num];
				control2.Clear(withDispose);
				control.Controls.RemoveAt(num);
				if (withDispose)
				{
					control2.Dispose();
				}
			}
		}

		public static void AutoTabIndex(this Control control)
		{
			control.Controls.OfType<Control>().ForEach(delegate(Control c)
			{
				c.TabIndex = control.Controls.IndexOf(c);
			});
		}

		public static string FixAmpersand(string text)
		{
			return text?.Replace("&", "&&");
		}

		[DllImport("user32.dll")]
		private static extern IntPtr GetMessageExtraInfo();

		public static bool MessageHasExtraInfo(this Control control)
		{
			return GetMessageExtraInfo() != IntPtr.Zero;
		}

		public static bool IsTouchMessage(this Control control)
		{
			return (GetMessageExtraInfo().ToInt64() & 0xFFFFFF00u) == 4283520768u;
		}

		public static IEnumerable<ToolStripItem> GetTools(ToolStripItemCollection tic)
		{
			foreach (ToolStripItem t in tic)
			{
				yield return t;
				ToolStripDropDownItem toolStripDropDownItem = t as ToolStripDropDownItem;
				if (toolStripDropDownItem == null)
				{
					continue;
				}
				foreach (ToolStripItem tool in GetTools(toolStripDropDownItem.DropDownItems))
				{
					yield return tool;
				}
			}
		}

		public static void PrefixToolStrip(ToolStripItemCollection tsic)
		{
			List<ToolStripMenuItem> list = new List<ToolStripMenuItem>();
			List<string> list2 = new List<string>();
			foreach (ToolStripMenuItem item in tsic.OfType<ToolStripMenuItem>())
			{
				list.Add(item);
				list2.Add(item.Text);
			}
			StringUtility.Prefix(list2, '&');
			for (int i = 0; i < list2.Count; i++)
			{
				list[i].Text = list2[i];
			}
		}

		public static void PrefixToolStrip(ToolStrip ts)
		{
			PrefixToolStrip(ts.Items);
			foreach (ToolStripItem item in ts.Items)
			{
				ToolStripDropDownItem toolStripDropDownItem = item as ToolStripDropDownItem;
				if (toolStripDropDownItem != null)
				{
					PrefixToolStrip(toolStripDropDownItem.DropDownItems);
				}
			}
		}

		public static void PrefixToolStrips(Form f, ComponentCollection components)
		{
			foreach (ToolStrip item in f.Controls.OfType<ToolStrip>())
			{
				PrefixToolStrip(item);
			}
			if (components == null)
			{
				return;
			}
			foreach (ToolStrip item2 in components.OfType<ToolStrip>())
			{
				PrefixToolStrip(item2);
			}
		}

		public static void SafeToolStripClear(ToolStripItemCollection tsic, int startAt = 0)
		{
			foreach (ToolStripItem item in tsic.OfType<ToolStripItem>().ToArray().Skip(startAt))
			{
				tsic.Remove(item);
				item.Dispose();
			}
		}

		public static void EnableRightClickSplitButtons(ToolStripItemCollection tsic)
		{
			foreach (ToolStripSplitButton item in GetTools(tsic).OfType<ToolStripSplitButton>())
			{
				ToolStripSplitButton t = item;
				t.MouseUp += delegate(object s, MouseEventArgs e)
				{
					if (e.Button == MouseButtons.Right)
					{
						t.ShowDropDown();
					}
				};
			}
		}

		public static int Clamp(this NumericUpDown num, int n)
		{
			return n.Clamp((int)num.Minimum, (int)num.Maximum);
		}

		public static Rectangle GetSafeBounds(this Form form)
		{
			if (form.WindowState != 0)
			{
				return form.RestoreBounds;
			}
			return form.Bounds;
		}

		public static void RestorePosition(this Form form, string key = null)
		{
			if (string.IsNullOrEmpty(key))
			{
				key = form.GetType().Name;
			}
			if (FormPositions.TryGetValue(key, out var bounds))
			{
				if (form.IsHandleCreated)
				{
					form.Bounds = bounds;
				}
				else
				{
					form.Load += delegate
					{
						form.Bounds = bounds;
					};
				}
			}
			form.Closing += delegate(object s, CancelEventArgs e)
			{
				FormPositions[key] = ((Form)s).GetSafeBounds();
			};
		}

		private static PanelState GetPanelState(ScrollableControl panel)
		{
			return new PanelState
			{
				Name = panel.Name,
				AutoScrollPosition = panel.AutoScrollPosition.Multiply(-1, -1),
				Collapsed = (from c in panel.Controls.OfType<CollapsibleGroupBox>()
					select c.Collapsed).ToArray()
			};
		}

		private static void SetPanelState(ScrollableControl panel, PanelState ps)
		{
			for (int i = 0; i < ps.Collapsed.Length; i++)
			{
				((CollapsibleGroupBox)panel.Controls[i]).Collapsed = ps.Collapsed[i];
			}
			panel.PerformLayout();
			panel.AutoScrollPosition = ps.AutoScrollPosition;
		}

		private static IEnumerable<PanelState> GetPanelStates(Control container)
		{
			return container.GetControls<Panel>().Select(GetPanelState).ToArray();
		}

		private static void SetPanelStates(Control container, IEnumerable<PanelState> psl)
		{
			if (psl == null)
			{
				return;
			}
			var enumerable = from panel in container.GetControls<Panel>()
				join ps in psl on panel.Name equals ps.Name
				select new
				{
					Panel = panel,
					State = ps
				};
			foreach (var item in enumerable)
			{
				SetPanelState(item.Panel, item.State);
			}
		}

		public static void RestorePanelStates(this Form control)
		{
			if (control == null)
			{
				return;
			}
			if (panelStates == null)
			{
				panelStates = new Dictionary<string, IEnumerable<PanelState>>();
			}
			if (panelStates.TryGetValue(control.Name, out var psl))
			{
				control.Load += delegate
				{
					SetPanelStates(control, psl);
				};
			}
			control.FormClosing += delegate
			{
				panelStates[control.Name] = GetPanelStates(control);
			};
		}

		public static bool PanelToTab(Control control, bool on, Action<bool> setState = null)
		{
			Control parent = control.Parent;
			if (control.Tag is TabControl)
			{
				if (on)
				{
					return true;
				}
				TabControl tabControl = control.Tag as TabControl;
				bool visible = tabControl.IsVisibleSet();
				foreach (TabPage tabPage3 in tabControl.TabPages)
				{
					Control control2 = tabPage3.Tag as Control;
					Control[] array = tabPage3.Controls.OfType<Control>().ToArray();
					foreach (Control control3 in array)
					{
						tabPage3.Controls.Remove(control3);
						if (control3 is Panel panel)
							panel.AutoScroll = false;

						if (control2 is CollapsibleGroupBox)
							control3.Top += ScaleDpiY((control2 as CollapsibleGroupBox).HeaderHeight);

						control2.Controls.Add(control3);
					}
				}
				control.Visible = visible;
				control.Tag = null;
				tabControl.Dispose();
				parent.Controls.Remove(tabControl);
			}
			else
			{
				if (!on)
				{
					return false;
				}
				TabControl tabControl2 = new TabControl();
				bool visible2 = control.IsVisibleSet();
				control.Visible = false;
				control.Tag = tabControl2;
				parent.Controls.Add(tabControl2);
				tabControl2.Bounds = control.Bounds;
				tabControl2.Visible = visible2;
				foreach (Control item in control.Controls.OfType<Control>().Reverse())
				{
					if (!item.IsVisibleSet())
					{
						continue;
					}
					TabPage tabPage2 = new TabPage(item.Text);
					tabControl2.TabPages.Add(tabPage2);
					tabPage2.Tag = item;
					tabPage2.BackColor = Color.Transparent;
					tabPage2.UseVisualStyleBackColor = true;
					tabPage2.DoubleClick += delegate
					{
						TogglePanelTab(control, setState);
					};
					Control[] array2 = item.Controls.OfType<Control>().ToArray();
					foreach (Control control4 in array2)
					{
						item.Controls.Remove(control4);
						if (control4 is Panel panel)
							panel.AutoScroll = true;

						if (item is CollapsibleGroupBox)
							control4.Top -= ScaleDpiY((item as CollapsibleGroupBox).HeaderHeight);

						tabPage2.Controls.Add(control4);
					}
				}
				tabControl2.SelectedIndex = 0;
				tabControl2.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			}
			return on;
		}

		public static bool TogglePanelTab(Control control, Action<bool> setState = null)
		{
			bool flag = PanelToTab(control, !(control.Tag is TabControl), setState);
			setState?.Invoke(flag);
			return flag;
		}

		public static void RegisterPanelToTabToggle(Control control, Func<bool> getState = null, Action<bool> setState = null)
		{
			foreach (Control control2 in control.Controls)
			{
				// Set up double-click handlers on all panels and the container control itself
				control2.Controls.OfType<Panel>().ForEach(c =>
				{
					c.AutoScroll = getState != null && getState(); // Set AutoScroll based on state, it should only be true when in tab mode
					c.DoubleClick += delegate
					{
						TogglePanelTab(control, setState);
					};
				});
				control2.DoubleClick += delegate
				{
					TogglePanelTab(control, setState);
				};
				control2.RegisterTouchScrolling();
			}
			if (getState != null && getState())
			{
				TogglePanelTab(control, setState);
			}
		}

		public static void RegisterPanelToTabToggle(Control control, IValueStore<bool> setting)
		{
			RegisterPanelToTabToggle(control, setting.GetValue, setting.SetValue);
		}

		public static bool RegisterTouchScrolling(this Control control)
		{
			return false;
		}

		[DllImport("user32.dll")]
		private static extern bool IsProcessDPIAware();

		[DllImport("gdi32.dll")]
		private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		[DllImport("user32.dll")]
		private static extern IntPtr GetDC(IntPtr hWnd);

		public static Size ScaleDpi(this Size size)
		{
			return new Size((int)((float)size.Width * DpiScale.X), (int)((float)size.Height * DpiScale.Y));
		}

		public static Point ScaleDpi(this Point pt)
		{
			return new Point((int)((float)pt.X * DpiScale.X), (int)((float)pt.Y * DpiScale.Y));
		}

		public static Padding ScaleDpi(this Padding pd)
		{
			pd.Left = ScaleDpiX(pd.Left);
			pd.Right = ScaleDpiX(pd.Right);
			pd.Top = ScaleDpiY(pd.Top);
			pd.Bottom = ScaleDpiY(pd.Bottom);
			return pd;
		}

		public static RectangleF ScaleDpi(this RectangleF rect)
		{
			rect.X = ScaleDpiX(rect.X);
			rect.Width = ScaleDpiX(rect.Width);
			rect.Y = ScaleDpiX(rect.Y);
			rect.Height = ScaleDpiY(rect.Height);
			return rect;
		}

		public static float ScaleDpiX(float v)
		{
			return v * DpiScale.X;
		}

		public static float ScaleDpiY(float v)
		{
			return v * DpiScale.Y;
		}

		public static int ScaleDpiX(int v)
		{
			return (int)((float)v * DpiScale.X);
		}

		public static int ScaleDpiY(int v)
		{
			return (int)((float)v * DpiScale.Y);
		}

		public static Bitmap ScaleDpi(this Bitmap bitmap)
		{
			return bitmap.Scale(DpiScale.Y);
		}

		public static Rectangle ScaleDpi(this Rectangle rect)
		{
			return new Rectangle(ScaleDpiX(rect.X), ScaleDpiY(rect.Y), ScaleDpiX(rect.Width), ScaleDpiY(rect.Height));
		}

		public static void ScaleDpi(this ListView.ColumnHeaderCollection chc)
		{
			foreach (ColumnHeader item in chc)
			{
				item.Width = ScaleDpiX(item.Width);
			}
		}

		private static object GetServiceObject(object obj)
		{
			if (!ServiceTranslation.TryGetValue(obj, out var value))
			{
				return obj;
			}
			return value;
		}
	}
}
