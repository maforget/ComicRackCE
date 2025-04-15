using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Net.Search;
using cYo.Common.Text;
using cYo.Common.Windows.Properties;

namespace cYo.Common.Windows.Forms
{
	public partial class ListSelectorControl : UserControl, Popup.INotifyClose
	{
		private static int lastTab;

		private static Size lastSize;

		private HashSet<string> pool;

		private int tab;

		private bool noUpdate;

		private bool checkShield;

		private bool transfered;

		private static readonly Image dropDownImage = Resources.Route;

		public HashSet<string> Pool
		{
			get
			{
				return pool;
			}
			set
			{
				pool = value;
				OnPoolChanged();
			}
		}

		public int Tab
		{
			get
			{
				return tab;
			}
			set
			{
				tab = (lastTab = value);
				switch (tab)
				{
				default:
					btLists.BackColor = SystemColors.Control;
					btCheck.BackColor = SystemColors.Window;
					btText.BackColor = SystemColors.Window;
					listPanel.Visible = true;
					lbCheckList.Visible = false;
					text.Visible = false;
					break;
				case 1:
					btLists.BackColor = SystemColors.Window;
					btCheck.BackColor = SystemColors.Control;
					btText.BackColor = SystemColors.Window;
					listPanel.Visible = false;
					lbCheckList.Visible = true;
					text.Visible = false;
					break;
				case 2:
					btLists.BackColor = SystemColors.Window;
					btCheck.BackColor = SystemColors.Window;
					btText.BackColor = SystemColors.Control;
					listPanel.Visible = false;
					lbCheckList.Visible = false;
					text.Visible = true;
					break;
				}
			}
		}

		[Browsable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		public ListSelectorControl()
		{
			InitializeComponent();
			MinimumSize = base.Size;
			MaximumSize = new Size(base.Width * 2, base.Height * 2);
			base.ResizeRedraw = true;
			if (!lastSize.IsEmpty)
			{
				base.Size = lastSize;
			}
			Tab = lastTab;
			LocalizeUtility.Localize(this, null);
		}

		private void RegisterSearch(IEnumerable<INetSearch> search)
		{
			if (search != null && !search.IsEmpty())
			{
				TextBoxContextMenu.Register(text, TextBoxContextMenu.AddSearchLinks(search));
			}
		}

		private void OnPoolChanged()
		{
			HashSet<string> hashSet = Text.ListStringToSet(',');
			if (pool == null)
			{
				pool = new HashSet<string>(hashSet);
			}
			else
			{
				pool.RemoveRange(hashSet);
			}
			lbOwn.BeginUpdate();
			lbPool.BeginUpdate();
			lbCheckList.BeginUpdate();
			lbOwn.Items.Clear();
			lbPool.Items.Clear();
			lbCheckList.Items.Clear();
			lbOwn.Items.AddRange(hashSet.ToArray());
			lbPool.Items.AddRange(pool.ToArray());
			lbCheckList.Items.AddRange(hashSet.ToArray());
			CheckAll(lbCheckList, state: true);
			lbCheckList.Items.AddRange(pool.ToArray());
			lbOwn.EndUpdate();
			lbPool.EndUpdate();
			lbCheckList.EndUpdate();
			UpdateButtonStates();
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			if (!noUpdate)
			{
				if (Tab == 2)
				{
					text.Text = Text;
				}
				OnPoolChanged();
			}
		}

		protected override void WndProc(ref Message m)
		{
			Popup popup = base.Parent as Popup;
			if (popup == null || !popup.ProcessResizing(ref m))
			{
				base.WndProc(ref m);
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			if (base.IsHandleCreated)
			{
				lastSize = base.Size;
			}
		}

		private void btAllToOwn_Click(object sender, EventArgs e)
		{
			CheckAll(lbCheckList, state: true);
			TransferAll(lbPool, lbOwn);
		}

		private void btSelectedToOwn_Click(object sender, EventArgs e)
		{
			CheckSelected(lbCheckList, lbPool, state: true);
			TransferSelected(lbPool, lbOwn);
		}

		private void btSelectedToPool_Click(object sender, EventArgs e)
		{
			CheckSelected(lbCheckList, lbOwn, state: false);
			TransferSelected(lbOwn, lbPool);
		}

		private void btAllToPool_Click(object sender, EventArgs e)
		{
			CheckAll(lbCheckList, state: false);
			TransferAll(lbOwn, lbPool);
		}

		private void lbOwn_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateButtonStates();
		}

		private void lbOwn_DoubleClick(object sender, EventArgs e)
		{
			CheckSelected(lbCheckList, lbOwn, state: false);
			TransferSelected(lbOwn, lbPool);
		}

		private void lbPool_DoubleClick(object sender, EventArgs e)
		{
			CheckSelected(lbCheckList, lbPool, state: true);
			TransferSelected(lbPool, lbOwn);
		}

		private void lbPool_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				lbPool_DoubleClick(sender, EventArgs.Empty);
			}
		}

		private void lbOwn_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				lbOwn_DoubleClick(sender, EventArgs.Empty);
			}
		}

		private void lbCheckList_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (!checkShield)
			{
				if (e.NewValue == CheckState.Checked)
				{
					lbOwn.Items.Add(lbCheckList.Items[e.Index]);
					lbPool.Items.Remove(lbCheckList.Items[e.Index]);
				}
				else
				{
					lbOwn.Items.Remove(lbCheckList.Items[e.Index]);
					lbPool.Items.Add(lbCheckList.Items[e.Index]);
				}
				transfered = true;
				UpdateButtonStates();
			}
		}

		private void btLists_Click(object sender, EventArgs e)
		{
			if (Tab == 2)
			{
				Text = text.Text.Replace("\r\n", " ");
			}
			Tab = 0;
		}

		private void btCheck_Click(object sender, EventArgs e)
		{
			if (Tab == 2)
			{
				Text = text.Text.Replace("\r\n", " ");
			}
			Tab = 1;
		}

		private void btText_Click(object sender, EventArgs e)
		{
			text.Text = GetListboxItems(lbOwn, ", ");
			Tab = 2;
		}

		private void CheckAll(CheckedListBox clb, bool state)
		{
			checkShield = true;
			clb.BeginUpdate();
			for (int i = 0; i < clb.Items.Count; i++)
			{
				clb.SetItemChecked(i, state);
			}
			clb.EndUpdate();
			checkShield = false;
		}

		private void CheckSelected(CheckedListBox clb, ListBox lb, bool state)
		{
			checkShield = true;
			clb.BeginUpdate();
			foreach (string selectedItem in lb.SelectedItems)
			{
				clb.SetItemChecked(clb.Items.IndexOf(selectedItem), state);
			}
			clb.EndUpdate();
			checkShield = false;
		}

		private void UpdateButtonStates()
		{
			btAllToOwn.Enabled = lbPool.Items.Count > 0;
			btAllToPool.Enabled = lbOwn.Items.Count > 0;
			btSelectedToOwn.Enabled = lbPool.SelectedIndices.Count > 0;
			btSelectedToPool.Enabled = lbOwn.SelectedItems.Count > 0;
		}

		private static string GetListboxItems(ListBox lb, string separator)
		{
			return lb.Items.ToListString(separator);
		}

		private void TransferAll(ListBox a, ListBox b)
		{
			a.BeginUpdate();
			b.BeginUpdate();
			b.Items.AddRange(a.Items);
			a.Items.Clear();
			b.EndUpdate();
			a.EndUpdate();
			transfered = true;
			UpdateButtonStates();
		}

		private void TransferSelected(ListBox a, ListBox b)
		{
			a.BeginUpdate();
			b.BeginUpdate();
			foreach (string selectedItem in a.SelectedItems)
			{
				b.Items.Add(selectedItem);
			}
			b.ClearSelected();
			IEnumerator enumerator2 = a.SelectedItems.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string text = (string)(b.SelectedItem = (string)enumerator2.Current);
				}
			}
			finally
			{
				IDisposable disposable = enumerator2 as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			foreach (string selectedItem2 in b.SelectedItems)
			{
				a.Items.Remove(selectedItem2);
			}
			b.EndUpdate();
			a.EndUpdate();
			transfered = true;
			UpdateButtonStates();
		}

		public void PopupClosed()
		{
			noUpdate = true;
			if (Tab == 2)
			{
				Text = text.Text.Replace("\r\n", " ");
			}
			else if (transfered)
			{
				Text = GetListboxItems(lbOwn, ", ");
			}
		}

		public static void Register(TextBox textBox, IEnumerable<INetSearch> search = null)
		{
			int num = FormUtility.ScaleDpiX(16);
			textBox.Width -= num;
			Button bt = new Button();
			bt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			textBox.Parent.Controls.Add(bt);
			textBox.Parent.Controls.SetChildIndex(bt, 0);
			bt.Bounds = new Rectangle(textBox.Right, textBox.Top, num, textBox.Height);
			bt.BackgroundImage = dropDownImage;
			bt.BackgroundImageLayout = ImageLayout.Center;
			bt.TabStop = false;
			bt.Click += delegate
			{
				Popup popup2 = ShowPopup(textBox, search);
				popup2.Closed += delegate
				{
					bt.Enabled = true;
				};
				bt.Enabled = false;
			};
			if (!textBox.Multiline)
			{
				textBox.KeyUp += delegate(object s2, KeyEventArgs ea2)
				{
					if (ea2.KeyCode == Keys.Down)
					{
						Popup popup = ShowPopup(textBox);
						popup.Closed += delegate
						{
							bt.Enabled = true;
						};
						bt.Enabled = false;
					}
				};
			}
			bt.Visible = true;
		}

		public static void Register(IEnumerable<INetSearch> search = null, params TextBox[] textBoxes)
		{
			foreach (TextBox textBox in textBoxes)
			{
				Register(textBox, search);
			}
		}

		public static Popup ShowPopup(TextBox textBox, IEnumerable<INetSearch> search = null)
		{
			ListSelectorControl ls = new ListSelectorControl();
			string value = textBox.Text;
			if (string.IsNullOrEmpty(value) && textBox is IPromptText)
			{
				value = ((IPromptText)textBox).Text;
			}
			ls.Text = value;
			ls.Pool = SetFromAutoComplete(textBox);
			ls.RegisterSearch(search);
			Popup popup = new Popup(ls, autoDispose: true)
			{
				ShowingAnimation = (Popup.PopupAnimations.TopToBottom | Popup.PopupAnimations.Slide),
				Resizable = true
			};
			popup.PopupClosed += delegate
			{
				textBox.Text = ls.Text;
			};
			popup.Show(textBox);
			return popup;
		}

		private static HashSet<string> SetFromAutoComplete(TextBox textBox)
		{
			HashSet<string> hashSet = new HashSet<string>();
			if (textBox is IDelayedAutoCompleteList)
			{
				((IDelayedAutoCompleteList)textBox).BuildAutoComplete();
			}
			if (textBox.AutoCompleteCustomSource == null)
			{
				return hashSet;
			}
			foreach (string item in textBox.AutoCompleteCustomSource)
			{
				hashSet.AddRange(item.FromListString(','));
			}
			return hashSet;
		}
	}
}
