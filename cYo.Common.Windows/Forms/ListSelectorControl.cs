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
	public class ListSelectorControl : UserControl, Popup.INotifyClose
	{
		private static int lastTab;

		private static Size lastSize;

		private HashSet<string> pool;

		private int tab;

		private bool noUpdate;

		private bool checkShield;

		private bool transfered;

		private static readonly Image dropDownImage = Resources.Route;

		private IContainer components;

		private ListBox lbOwn;

		private ListBox lbPool;

		private Button btAllToOwn;

		private Button btSelectedToOwn;

		private Button btSelectedToPool;

		private Button btAllToPool;

		private CheckedListBoxEx lbCheckList;

		private Panel listPanel;

		private Button btLists;

		private Button btCheck;

		private Button btText;

		private TextBox text;

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
			lbOwn = new System.Windows.Forms.ListBox();
			lbPool = new System.Windows.Forms.ListBox();
			btAllToOwn = new System.Windows.Forms.Button();
			btSelectedToOwn = new System.Windows.Forms.Button();
			btSelectedToPool = new System.Windows.Forms.Button();
			btAllToPool = new System.Windows.Forms.Button();
			listPanel = new System.Windows.Forms.Panel();
			btLists = new System.Windows.Forms.Button();
			btCheck = new System.Windows.Forms.Button();
			btText = new System.Windows.Forms.Button();
			text = new System.Windows.Forms.TextBox();
			lbCheckList = new cYo.Common.Windows.Forms.CheckedListBoxEx();
			listPanel.SuspendLayout();
			SuspendLayout();
			lbOwn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lbOwn.FormattingEnabled = true;
			lbOwn.IntegralHeight = false;
			lbOwn.Location = new System.Drawing.Point(0, 0);
			lbOwn.MultiColumn = true;
			lbOwn.Name = "lbOwn";
			lbOwn.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			lbOwn.Size = new System.Drawing.Size(245, 95);
			lbOwn.Sorted = true;
			lbOwn.TabIndex = 0;
			lbOwn.SelectedIndexChanged += new System.EventHandler(lbOwn_SelectedIndexChanged);
			lbOwn.DoubleClick += new System.EventHandler(lbOwn_DoubleClick);
			lbOwn.KeyDown += new System.Windows.Forms.KeyEventHandler(lbOwn_KeyDown);
			lbPool.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lbPool.FormattingEnabled = true;
			lbPool.IntegralHeight = false;
			lbPool.Location = new System.Drawing.Point(0, 101);
			lbPool.MultiColumn = true;
			lbPool.Name = "lbPool";
			lbPool.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			lbPool.Size = new System.Drawing.Size(291, 142);
			lbPool.Sorted = true;
			lbPool.TabIndex = 5;
			lbPool.SelectedIndexChanged += new System.EventHandler(lbOwn_SelectedIndexChanged);
			lbPool.DoubleClick += new System.EventHandler(lbPool_DoubleClick);
			lbPool.KeyDown += new System.Windows.Forms.KeyEventHandler(lbPool_KeyDown);
			btAllToOwn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btAllToOwn.Location = new System.Drawing.Point(251, 0);
			btAllToOwn.Name = "btAllToOwn";
			btAllToOwn.Size = new System.Drawing.Size(40, 23);
			btAllToOwn.TabIndex = 1;
			btAllToOwn.Text = "<<";
			btAllToOwn.UseVisualStyleBackColor = true;
			btAllToOwn.Click += new System.EventHandler(btAllToOwn_Click);
			btSelectedToOwn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btSelectedToOwn.Location = new System.Drawing.Point(251, 25);
			btSelectedToOwn.Name = "btSelectedToOwn";
			btSelectedToOwn.Size = new System.Drawing.Size(40, 23);
			btSelectedToOwn.TabIndex = 2;
			btSelectedToOwn.Text = "<";
			btSelectedToOwn.UseVisualStyleBackColor = true;
			btSelectedToOwn.Click += new System.EventHandler(btSelectedToOwn_Click);
			btSelectedToPool.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btSelectedToPool.Location = new System.Drawing.Point(251, 49);
			btSelectedToPool.Name = "btSelectedToPool";
			btSelectedToPool.Size = new System.Drawing.Size(40, 23);
			btSelectedToPool.TabIndex = 3;
			btSelectedToPool.Text = ">";
			btSelectedToPool.UseVisualStyleBackColor = true;
			btSelectedToPool.Click += new System.EventHandler(btSelectedToPool_Click);
			btAllToPool.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btAllToPool.Location = new System.Drawing.Point(251, 73);
			btAllToPool.Name = "btAllToPool";
			btAllToPool.Size = new System.Drawing.Size(40, 23);
			btAllToPool.TabIndex = 4;
			btAllToPool.Text = ">>";
			btAllToPool.UseVisualStyleBackColor = true;
			btAllToPool.Click += new System.EventHandler(btAllToPool_Click);
			listPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			listPanel.Controls.Add(lbOwn);
			listPanel.Controls.Add(btAllToPool);
			listPanel.Controls.Add(btSelectedToOwn);
			listPanel.Controls.Add(lbPool);
			listPanel.Controls.Add(btAllToOwn);
			listPanel.Controls.Add(btSelectedToPool);
			listPanel.Location = new System.Drawing.Point(7, 7);
			listPanel.Margin = new System.Windows.Forms.Padding(0);
			listPanel.Name = "listPanel";
			listPanel.Size = new System.Drawing.Size(291, 243);
			listPanel.TabIndex = 11;
			btLists.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btLists.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLight;
			btLists.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btLists.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f);
			btLists.Location = new System.Drawing.Point(11, 245);
			btLists.Name = "btLists";
			btLists.Size = new System.Drawing.Size(67, 24);
			btLists.TabIndex = 14;
			btLists.Text = "&Lists";
			btLists.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btLists.UseVisualStyleBackColor = true;
			btLists.Click += new System.EventHandler(btLists_Click);
			btCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btCheck.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLight;
			btCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f);
			btCheck.Location = new System.Drawing.Point(79, 245);
			btCheck.Name = "btCheck";
			btCheck.Size = new System.Drawing.Size(67, 24);
			btCheck.TabIndex = 15;
			btCheck.Text = "&Check";
			btCheck.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btCheck.UseVisualStyleBackColor = true;
			btCheck.Click += new System.EventHandler(btCheck_Click);
			btText.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btText.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlLight;
			btText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btText.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f);
			btText.Location = new System.Drawing.Point(147, 245);
			btText.Name = "btText";
			btText.Size = new System.Drawing.Size(67, 24);
			btText.TabIndex = 16;
			btText.Text = "&Text";
			btText.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btText.UseVisualStyleBackColor = true;
			btText.Click += new System.EventHandler(btText_Click);
			text.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			text.Location = new System.Drawing.Point(7, 7);
			text.Multiline = true;
			text.Name = "text";
			text.Size = new System.Drawing.Size(291, 243);
			text.TabIndex = 17;
			lbCheckList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lbCheckList.FormattingEnabled = true;
			lbCheckList.IntegralHeight = false;
			lbCheckList.Location = new System.Drawing.Point(7, 7);
			lbCheckList.MultiColumn = true;
			lbCheckList.Name = "lbCheckList";
			lbCheckList.Size = new System.Drawing.Size(291, 243);
			lbCheckList.Sorted = true;
			lbCheckList.TabIndex = 0;
			lbCheckList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(lbCheckList_ItemCheck);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.SystemColors.Window;
			base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			base.Controls.Add(text);
			base.Controls.Add(lbCheckList);
			base.Controls.Add(listPanel);
			base.Controls.Add(btLists);
			base.Controls.Add(btCheck);
			base.Controls.Add(btText);
			base.Margin = new System.Windows.Forms.Padding(0);
			base.Name = "ListSelectorControl";
			base.Padding = new System.Windows.Forms.Padding(4);
			base.Size = new System.Drawing.Size(308, 278);
			listPanel.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
