using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public partial class SelectItemDialog : FormEx
	{
		private string itemCaption;

		private string textValue;

		private readonly List<string> selectionItems = new List<string>();

		public string CheckOptionText
		{
			get;
			set;
		}

		public bool DefaultCheckResult
		{
			get;
			set;
		}

		public string TextCaption
		{
			get
			{
				return itemCaption;
			}
			set
			{
				itemCaption = value;
			}
		}

		public string TextValue
		{
			get
			{
				return textValue;
			}
			set
			{
				textValue = value;
			}
		}

		public List<string> SelectionItems => selectionItems;

		public SelectItemDialog()
		{
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
		}

		public string GetName(IWin32Window parent)
		{
			bool flag = selectionItems.Count > 0;
			if (!string.IsNullOrEmpty(TextCaption))
			{
				lblName.Text = itemCaption + ":";
			}
			if (!string.IsNullOrEmpty(CheckOptionText))
			{
				chkOption.Text = CheckOptionText;
				chkOption.Checked = DefaultCheckResult;
				chkOption.Visible = true;
			}
			if (flag)
			{
				txtName.Visible = false;
				cbName.Items.Clear();
				cbName.Items.AddRange(selectionItems.ToArray());
				cbName.Text = textValue ?? string.Empty;
				cbName.SelectAll();
			}
			else
			{
				cbName.Visible = false;
				txtName.TabIndex = 0;
				txtName.Text = textValue;
				txtName.Bounds = cbName.Bounds;
				txtName.SelectAll();
			}
			btOK.Enabled = !string.IsNullOrEmpty(TextValue);
			if (ShowDialog(parent) == DialogResult.Cancel)
			{
				return null;
			}
			DefaultCheckResult = chkOption.Checked;
			return (flag ? cbName.Text : txtName.Text).Trim();
		}

		private void NameTextChanged(object sender, EventArgs e)
		{
			btOK.Enabled = !string.IsNullOrEmpty(cbName.Visible ? cbName.Text : txtName.Text);
		}

		public static string GetName<T>(IWin32Window parent, string caption, string itemValue, IEnumerable<T> list, string itemCaption = null)
		{
			using (SelectItemDialog selectItemDialog = new SelectItemDialog())
			{
				selectItemDialog.Text = caption;
				selectItemDialog.TextValue = itemValue;
				selectItemDialog.TextCaption = itemCaption;
				if (list != null)
				{
					selectItemDialog.SelectionItems.AddRange(list.Select((T x) => x.ToString()).ToArray());
				}
				return selectItemDialog.GetName(parent);
			}
		}

		public static string GetName(IWin32Window parent, string caption, string itemValue)
		{
			return GetName<string>(parent, caption, itemValue, null);
		}
	}
}
