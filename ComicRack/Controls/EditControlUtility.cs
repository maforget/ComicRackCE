using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public static class EditControlUtility
	{
		public static string GetText(TextBox control, string compare = null, bool trim = true)
		{
			string text = control.Text;
			if (trim)
			{
				text = text.Trim();
			}
			if (text != compare && control.AutoCompleteCustomSource != null)
			{
				control.AutoCompleteCustomSource.Add(text);
			}
			return text;
		}

		public static int GetNumber(Control control)
		{
			if (int.TryParse(control.Text, out var result))
			{
				if (result >= 0)
				{
					return result;
				}
				return -1;
			}
			return -1;
		}

		public static float GetRealNumber(Control control)
		{
			if (float.TryParse(control.Text, out var result))
			{
				if (!(result < 0f))
				{
					return result;
				}
				return -1f;
			}
			return -1f;
		}

		public static void SetLabel(Label label, string text, bool enabled)
		{
			label.Text = (string.IsNullOrEmpty(text) ? "Unknown" : text);
			label.Enabled = !string.IsNullOrEmpty(text) && enabled;
		}

		public static void SetLabel(Label label, string text)
		{
			SetLabel(label, text, enabled: true);
		}

		public static void SetText(Control control, string text)
		{
			if (text != null)
			{
				try
				{
					control.Text = text;
				}
				catch (Exception)
				{
				}
			}
		}

		public static void SetText(TextBox textBox, string text, Func<AutoCompleteStringCollection> autoCompletePredicate)
		{
			SetText(textBox, text);
			IDelayedAutoCompleteList delayedAutoCompleteList = textBox as IDelayedAutoCompleteList;
			if (delayedAutoCompleteList != null)
			{
				if (autoCompletePredicate != null)
				{
					delayedAutoCompleteList.SetLazyAutoComplete(autoCompletePredicate);
				}
				else
				{
					delayedAutoCompleteList.ResetLazyAutoComplete();
				}
			}
			else
			{
				textBox.AutoCompleteCustomSource = autoCompletePredicate();
				textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
				textBox.AutoCompleteMode = AutoCompleteMode.Append;
			}
		}

		public static void SetText(ComboBox c, string text, Func<AutoCompleteStringCollection> autoCompletePredicate, bool onlyDirectList = false)
		{
			if (autoCompletePredicate != null)
			{
				Func<IEnumerable<string>> allItems = delegate
				{
					HashSet<string> hashSet = new HashSet<string>
					{
						string.Empty
					};
					hashSet.AddRange(autoCompletePredicate().Cast<string>());
					return hashSet;
				};
				if (c.DataSource == null)
				{
					if (!onlyDirectList)
					{
						c.Enter += delegate
						{
							string text2 = c.Text;
							c.DataSource = ComboBoxSkinner.AutoSeparatorList(allItems());
							c.Text = text2;
						};
					}
				}
				else
				{
					c.DataSource = ComboBoxSkinner.AutoSeparatorList(allItems());
				}
			}
			SetText(c, text);
		}

		public static void SetText(Control control, int value)
		{
			control.Text = ((value == -1) ? string.Empty : value.ToString());
		}

		public static void SetText(Control control, float value)
		{
			control.Text = ((value == -1f) ? string.Empty : value.ToString());
		}

		public static void SetText(Control c, bool value)
		{
			c.Text = (value ? ComicInfo.YesText : ComicInfo.NoText);
		}

		public static void SetText(Control c, YesNo yn)
		{
			c.Text = ComicInfo.GetYesNoAsText(yn);
		}

		public static void SetText(Control c, MangaYesNo yn)
		{
			c.Text = ComicInfo.GetYesNoAsText(yn);
		}

		public static YesNo GetYesNo(string text)
		{
			if (string.Equals(text, ComicInfo.YesText, StringComparison.OrdinalIgnoreCase))
			{
				return YesNo.Yes;
			}
			if (!string.Equals(text, ComicInfo.NoText, StringComparison.OrdinalIgnoreCase))
			{
				return YesNo.Unknown;
			}
			return YesNo.No;
		}

		public static MangaYesNo GetMangaYesNo(string text)
		{
			if (string.Equals(text, ComicInfo.YesText, StringComparison.OrdinalIgnoreCase))
			{
				return MangaYesNo.Yes;
			}
			if (string.Equals(text, ComicInfo.YesRightToLeftText, StringComparison.OrdinalIgnoreCase))
			{
				return MangaYesNo.YesAndRightToLeft;
			}
			if (string.Equals(text, ComicInfo.NoText, StringComparison.OrdinalIgnoreCase))
			{
				return MangaYesNo.No;
			}
			return MangaYesNo.Unknown;
		}

		public static void InitializeYesNo(ComboBox cb, bool withEmpty = true)
		{
			if (withEmpty)
			{
				cb.Items.Add(string.Empty);
			}
			cb.Items.Add(ComicInfo.YesText);
			cb.Items.Add(ComicInfo.NoText);
		}

		public static void InitializeMangaYesNo(ComboBox cb, bool withEmpty = true)
		{
			if (withEmpty)
			{
				cb.Items.Add(string.Empty);
			}
			cb.Items.Add(ComicInfo.YesText);
			cb.Items.Add(ComicInfo.YesRightToLeftText);
			cb.Items.Add(ComicInfo.NoText);
		}
	}
}
