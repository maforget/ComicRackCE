using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;

namespace cYo.Common.Windows.Forms
{
	public class LanguageComboBox : ComboBox
	{
		private class LanguageItem : ComboBoxSkinner.ComboBoxItem<CultureInfo>, IComparable<LanguageItem>
		{
			public LanguageItem(CultureInfo ci)
				: base(ci)
			{
			}

			public override string ToString()
			{
				if (string.IsNullOrEmpty(base.Item.Name))
				{
					return string.Empty;
				}
				return base.Item.DisplayName;
			}

			public int CompareTo(LanguageItem other)
			{
				return string.Compare(ToString(), other.ToString());
			}
		}

		private CultureTypes cultureTypes = CultureTypes.NeutralCultures;

		private IEnumerable<string> topISOLanguages;

		public override string Text
		{
			get
			{
				return SelectedCulture;
			}
			set
			{
				SelectedCulture = value;
			}
		}

		public CultureTypes CultureTypes
		{
			get
			{
				return cultureTypes;
			}
			set
			{
				if (value != cultureTypes)
				{
					cultureTypes = value;
					OnCultureTypesChanged();
				}
			}
		}

		public IEnumerable<string> TopISOLanguages
		{
			get
			{
				return topISOLanguages;
			}
			set
			{
				topISOLanguages = value;
				string selectedCulture = SelectedCulture;
				FillList();
				SelectedCulture = selectedCulture;
			}
		}

		public string SelectedTwoLetterISOLanguage
		{
			get
			{
				LanguageItem languageItem = (LanguageItem)base.SelectedItem;
				if (languageItem != null && !string.IsNullOrEmpty(languageItem.Item.Name))
				{
					return languageItem.Item.TwoLetterISOLanguageName;
				}
				return string.Empty;
			}
			set
			{
				foreach (LanguageItem item in base.Items)
				{
					if (item.Item.TwoLetterISOLanguageName == value || (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(item.Item.Name)))
					{
						base.SelectedItem = item;
						break;
					}
				}
			}
		}

		public string SelectedCulture
		{
			get
			{
				LanguageItem languageItem = base.SelectedItem as LanguageItem;
				if (languageItem == null)
				{
					return string.Empty;
				}
				if (!string.IsNullOrEmpty(languageItem.Item.Name))
				{
					return languageItem.Item.Name;
				}
				return string.Empty;
			}
			set
			{
				foreach (LanguageItem item in base.Items)
				{
					if (item.Item.Name == value)
					{
						base.SelectedItem = item;
						break;
					}
				}
			}
		}

		public LanguageComboBox()
		{
			base.DropDownStyle = ComboBoxStyle.DropDownList;
			SelectedCulture = string.Empty;
			new ComboBoxSkinner(this);
			FillList();
		}

		private void OnCultureTypesChanged()
		{
			if (!base.DesignMode)
			{
				FillList();
			}
		}

		private bool IsValidCulture(string iso)
		{
			if (string.IsNullOrEmpty(iso))
			{
				return false;
			}
			try
			{
				new CultureInfo(iso);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void FillList()
		{
			base.Items.Clear();
			base.Items.Add(new LanguageItem(new CultureInfo(string.Empty)));
			base.Sorted = false;
			LanguageItem[] array = ((topISOLanguages == null) ? null : (from iso in topISOLanguages.Where(IsValidCulture)
				select new LanguageItem(new CultureInfo(iso))).ToArray().Sort());
			bool hasTop = array != null && array.Length != 0;
			if (hasTop)
			{
				base.Items.AddRange(array);
			}
			LanguageItem[] array2 = (from ci in CultureInfo.GetCultures(cultureTypes)
				select new LanguageItem(ci) into li
				where !hasTop || !string.IsNullOrEmpty(li.ToString())
				select li).ToArray().Sort();
			if (hasTop)
			{
				((ComboBoxSkinner.IComboBoxItem)array2[0]).IsSeparator = true;
			}
			base.Items.AddRange(array2);
		}
	}
}
