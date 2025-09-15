#define TRACE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Reflection;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class MultipleComicBooksDialog : Form
	{
		private class StoreInfo
		{
			public string PropertyName;

			public bool ListMode;

			public object Value;

			public HashSet<string> NewList;

			public HashSet<string> OldList;

			public PropertyInfo PropertyInfo;
		}

		private readonly IEnumerable<ComicBook> books;

		private readonly List<TextBox> listFields = new List<TextBox>();

		private readonly List<TextBox> customFields = new List<TextBox>();

		private readonly Dictionary<Control, HashSet<string>> oldLists = new Dictionary<Control, HashSet<string>>();

		public MultipleComicBooksDialog(IEnumerable<ComicBook> books)
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			this.RestorePosition();
			this.RestorePanelStates();
			LocalizeUtility.Localize(this, null);
			EditControlUtility.InitializeMangaYesNo(cbManga);
			EditControlUtility.InitializeYesNo(cbBlackAndWhite);
			EditControlUtility.InitializeYesNo(cbEnableProposed);
			EditControlUtility.InitializeYesNo(cbSeriesComplete);
			new ComboBoxSkinner(cbImprint);
			new ComboBoxSkinner(cbPublisher, ComicBook.PublisherIcons) { MaxHeightScale = 2 };
			new ComboBoxSkinner(cbImprint, ComicBook.PublisherIcons) { MaxHeightScale = 2 };
			new ComboBoxSkinner(cbAgeRating, ComicBook.AgeRatingIcons) { MaxHeightScale = 2 };
			new ComboBoxSkinner(cbFormat, ComicBook.FormatIcons) { MaxHeightScale = 2 };
			new ComboBoxSkinner(cbBookStore);
			new ComboBoxSkinner(cbBookCondition);
			new ComboBoxSkinner(cbBookLocation);
			new ComboBoxSkinner(cbBookOwner);
			new ComboBoxSkinner(cbBookPrice);
			listFields.AddRange(new TextBoxEx[14]
			{
				txWriter,
				txPenciller,
				txInker,
				txColorist,
				txEditor,
				txCoverArtist,
				txTranslator,
				txLetterer,
				txGenre,
				txTags,
				txCharacters,
				txTeams,
				txLocations,
				txCollectionStatus
			});
			ListSelectorControl.Register(SearchEngines.Engines, listFields.ToArray());
			cbLanguage.TopISOLanguages = Program.Lists.GetComicFieldList((ComicBook cb) => cb.LanguageISO).Cast<string>().Distinct();
			this.books = books.ToArray();
			Text = StringUtility.Format(Text, books.Count());
			labelOpenedTime.Visible = (dtpOpenedTime.Visible = (dtpOpenedTime.Enabled = (labelPagesAsTextSimple.Visible = (txPagesAsTextSimple.Visible = (txPagesAsTextSimple.Enabled = !books.Any((ComicBook cb) => cb.IsLinked))))));
			txCommunityRating.Enabled = txRating.Enabled = cbEnableProposed.Enabled = cbSeriesComplete.Enabled = books.All((ComicBook cb) => cb.IsInContainer);
			SpinButton.AddUpDown(txVolume);
			SpinButton.AddUpDown(txCount, 1, 0);
			SpinButton.AddUpDown(txNumber);
			SpinButton.AddUpDown(txYear, DateTime.Now.Year);
			SpinButton.AddUpDown(txMonth, DateTime.Now.Month, 1, 12);
			SpinButton.AddUpDown(txDay, DateTime.Now.Day, 1, 31);
			SpinButton.AddUpDown(txAlternateCount, 1, 0);
			SpinButton.AddUpDown(txAlternateNumber);
			SpinButton.AddUpDown(txPagesAsTextSimple, 1, 1);
			txVolume.EnableOnlyNumberKeys();
			txCount.EnableOnlyNumberKeys();
			txYear.EnableOnlyNumberKeys();
			txMonth.EnableOnlyNumberKeys();
			txAlternateCount.EnableOnlyNumberKeys();
			txPagesAsTextSimple.EnableOnlyNumberKeys();
			string[] array = Program.Database.CustomValues.Where((string k) => Program.ExtendedSettings.ShowCustomScriptValues || !k.Contains('.')).ToArray();
			if (!Program.Settings.ShowCustomBookFields || array.Length == 0)
			{
				grpCustom.Visible = false;
			}
			else
			{
				AddCustomFields(array);
			}
			TextBoxContextMenu.Register(this, TextBoxContextMenu.AddSearchLinks(SearchEngines.Engines));
		}

		private void AddCustomFields(string[] customValues)
		{
			Label label = null;
			TextBox textBox = null;
			foreach (string text in customValues)
			{
				Label label2;
				TextBox textBox2;
				if (label == null)
				{
					label2 = labelCustomField;
					textBox2 = textCustomField;
				}
				else
				{
					label2 = (Label)label.Clone();
					textBox2 = (TextBox)textBox.Clone();
					customGroupsPanel.Controls.Add(label2);
					customGroupsPanel.Controls.Add(textBox2);
					label2.Visible = true;
					label2.Top = textBox.Bottom + 8;
					textBox2.Visible = true;
					textBox2.Top = label2.Bottom + 4;
					Trace.WriteLine("Textbox: " + textBox2.Bounds);
				}
				label2.Text = text + ":";
				textBox2.Tag = text;
				customFields.Add(textBox2);
				label = label2;
				textBox = textBox2;
			}
			grpCustom.Height = textBox.Bottom + 32;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Init();
		}

		private void RefreshBooksInfoFromFiles()
		{
			int num = 0;
			int num2 = books.Count();
			foreach (ComicBook book in books)
			{
				book.RefreshInfoFromFile(RefreshInfoOptions.None);
				AutomaticProgressDialog.Value = num++ * 100 / num2;
			}
		}

		private void Init()
		{
			AutomaticProgressDialog.Process(this, TR.Messages["RefreshInfo", "Refreshing Information"], TR.Messages["RefreshInfoText", "Refreshing information for selected Books"], 1000, RefreshBooksInfoFromFiles, AutomaticProgressDialogOptions.None);
			bool flag = books.Any((ComicBook b) => b.IsLinked);
			SetText(txSeries, "Series", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.ShadowSeries));
			SetText(txNumber, "Number");
			SetText(txCount, "Count");
			SetText(txYear, "Year");
			SetText(txVolume, "Volume");
			SetText(cbSeriesComplete, "SeriesComplete");
			SetText(txTitle, "Title", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Title));
			SetText(txAlternateSeries, "AlternateSeries", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.AlternateSeries));
			SetText(txAlternateNumber, "AlternateNumber");
			SetText(txAlternateCount, "AlternateCount");
			SetText(txSeriesGroup, "SeriesGroup", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.SeriesGroup));
			SetText(txStoryArc, "StoryArc", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.StoryArc));
			SetText(txMonth, "Month");
			SetText(txDay, "Day");
			SetText(txWriter, "Writer", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Writer));
			SetText(txPenciller, "Penciller", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Penciller));
			SetText(txColorist, "Colorist", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Colorist));
			SetText(txInker, "Inker", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Inker));
			SetText(txLetterer, "Letterer", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Letterer));
			SetText(txCoverArtist, "CoverArtist", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.CoverArtist));
			SetText(txEditor, "Editor", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Editor));
			SetText(txTranslator, "Translator", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Translator));
			SetText(cbPublisher, "Publisher", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Publisher, sort: true));
			SetText(cbImprint, "Imprint", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Imprint, sort: true));
			SetText(txGenre, "Genre", () => Program.Lists.GetGenreList(withSeparator: false));
			SetText(cbFormat, "Format", () => Program.Lists.GetFormatList());
			SetText(cbAgeRating, "AgeRating", () => Program.Lists.GetAgeRatingList());
			SetText(txRating, "Rating");
			SetText(txCommunityRating, "CommunityRating");
			SetText(cbLanguage, "LanguageISO");
			SetText(cbManga, "Manga");
			SetText(cbBlackAndWhite, "BlackAndWhite");
			SetText(cbEnableProposed, "EnableProposed");
			SetText(txSummary, "Summary");
			SetText(txNotes, "Notes");
			SetText(txReview, "Review");
			SetText(txWeb, "Web");
			SetText(txTags, "Tags", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Tags));
			SetText(txCharacters, "Characters", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Characters));
			SetText(txTeams, "Teams", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Teams));
			SetText(txMainCharacterOrTeam, "MainCharacterOrTeam", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.MainCharacterOrTeam));
			SetText(txLocations, "Locations", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.Locations));
			SetText(txScanInformation, "ScanInformation", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.ScanInformation));
			SetGrayText(txSeries, "ProposedSeries");
			SetGrayText(txNumber, "ProposedNumber");
			SetGrayText(txCount, "ProposedCountAsText");
			SetGrayText(txYear, "ProposedYearAsText");
			SetGrayText(txVolume, "ProposedNakedVolumeAsText");
			SetGrayText(cbFormat, "ProposedFormat");
			grpCatalog.Visible = !flag || !Program.Settings.CatalogOnlyForFileless;
			Label label = labelScanInformation;
			bool visible = (txScanInformation.Visible = flag);
			label.Visible = visible;
			SetText(cbBookStore, "BookStore", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookStore, sort: true));
			SetText(cbBookOwner, "BookOwner", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookOwner, sort: true));
			SetText(cbBookLocation, "BookLocation", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookLocation, sort: true));
			SetText(cbBookPrice, "BookPriceAsText", () => Program.Lists.GetComicFieldList((ComicBook cb) => cb.BookPriceAsText, sort: true));
			SetText(cbBookAge, "BookAge", () => Program.Lists.GetBookAgeList());
			SetText(cbBookCondition, "BookCondition", () => Program.Lists.GetBookConditionList());
			SetText(txCollectionStatus, "BookCollectionStatus", () => Program.Lists.GetBookCollectionStatusList());
			SetText(txISBN, "ISBN");
			SetText(txBookNotes, "BookNotes");
			if (txPagesAsTextSimple.Enabled)
			{
				SetText(txPagesAsTextSimple, "PagesAsTextSimple");
				SetText(dtpOpenedTime, "OpenedTime");
			}
			SetText(dtpAddedTime, "AddedTime");
			SetText(dtpReleasedTime, "ReleasedTime");
			foreach (TextBox customField in customFields)
			{
				string key = customField.Tag.ToString();
				SetText(customField, "{" + key + "}", delegate
				{
					AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
					autoCompleteStringCollection.AddRange((from p in Program.Database.GetBooks().SelectMany((ComicBook cb) => cb.GetCustomValues())
						where p.Key.Equals(key, StringComparison.OrdinalIgnoreCase)
						select p.Value).ToArray());
					return autoCompleteStringCollection;
				});
			}
			FormUtility.RegisterPanelToTabToggle(pageData, PropertyCaller.CreateFlagsValueStore(Program.Settings, "TabLayouts", TabLayouts.Multiple));
		}

		private void Store()
		{
			List<StoreInfo> list = new List<StoreInfo>();
			foreach (CheckBox control2 in this.GetControls<CheckBox>())
			{
				if (control2.CheckState == CheckState.Unchecked)
				{
					continue;
				}
				Control control = control2.Tag as Control;
				if (!control.Enabled)
				{
					continue;
				}
				string text = control.Text;
				string text2 = control.Tag as string;
				PropertyInfo propertyInfo = (text2.StartsWith("{") ? null : typeof(ComicBook).GetProperty(text2));
				StoreInfo storeInfo = new StoreInfo
				{
					PropertyName = text2,
					PropertyInfo = propertyInfo,
					ListMode = (control2.CheckState == CheckState.Indeterminate)
				};
				if (propertyInfo == null)
				{
					storeInfo.Value = text;
				}
				else if (propertyInfo.PropertyType == typeof(bool))
				{
					storeInfo.Value = string.Compare(text, ComicInfo.YesText, ignoreCase: true) == 0;
				}
				else if (propertyInfo.PropertyType == typeof(YesNo))
				{
					storeInfo.Value = EditControlUtility.GetYesNo(text);
				}
				else if (propertyInfo.PropertyType == typeof(MangaYesNo))
				{
					storeInfo.Value = EditControlUtility.GetMangaYesNo(text);
				}
				else if (propertyInfo.PropertyType == typeof(int))
				{
					if (!int.TryParse(text, out var result) || result < 0)
					{
						result = propertyInfo.DefaultValue(-1);
					}
					storeInfo.Value = result;
				}
				else if (propertyInfo.PropertyType == typeof(float))
				{
					if (!float.TryParse(text, out var result2))
					{
						result2 = propertyInfo.DefaultValue(0f);
					}
					storeInfo.Value = result2;
				}
				else if (propertyInfo.PropertyType == typeof(DateTime))
				{
					storeInfo.Value = ((NullableDateTimePicker)control).Value;
				}
				else
				{
					storeInfo.Value = text;
					if (storeInfo.ListMode)
					{
						storeInfo.NewList = text.ListStringToSet(',');
						storeInfo.OldList = oldLists[control];
						if (object.Equals(storeInfo.OldList, storeInfo.NewList))
						{
							storeInfo = null;
						}
					}
				}
				if (storeInfo != null)
				{
					list.Add(storeInfo);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = books.Count();
			foreach (ComicBook book in books)
			{
				foreach (StoreInfo item in list)
				{
					if (item.PropertyInfo == null)
					{
						book.SetCustomValue(item.PropertyName.Substring(1, item.PropertyName.Length - 2), item.Value.ToString());
						continue;
					}
					if (!item.ListMode)
					{
						item.PropertyInfo.SetValue(book, item.Value, null);
						continue;
					}
					HashSet<string> list2 = book.GetStringPropertyValue(item.PropertyName).ListStringToSet(',');
					list2.RemoveRange(item.OldList);
					list2.AddRange(item.NewList);
					item.PropertyInfo.SetValue(book, list2.ToListString(", "), null);
				}
				AutomaticProgressDialog.Value = num++ * 100 / num2;
			}
		}

		private string GetSameValue(string propName)
		{
			string text = null;
			foreach (ComicBook book in books)
			{
				string stringPropertyValue = book.GetStringPropertyValue(propName);
				if (text == null)
				{
					text = stringPropertyValue;
				}
				else if (text != stringPropertyValue)
				{
					return string.Empty;
				}
			}
			return text;
		}

		private CheckBox MakeCheckBox(Control c)
		{
			CheckBox checkBox = new CheckBox();
			c.Parent.Controls.Add(checkBox);
			checkBox.AutoSize = true;
			checkBox.Visible = true;
			checkBox.TabStop = false;
			checkBox.Name = "chk" + c.Name;
			checkBox.Enabled = c.Enabled;
			checkBox.Left = c.Left;
			checkBox.Top = c.Top + (c.Height - checkBox.Height) / 2;
			c.Left += checkBox.Width + 2;
			c.Width -= checkBox.Width + 2;
			Label label = base.Controls[c.Name.Replace("tx", "label").Replace("cb", "label")] as Label;
			if (label != null)
			{
				label.Left = c.Left;
			}
			return checkBox;
		}

		private void SetGrayText(IPromptText tx, string property)
		{
			Control control = tx as Control;
			string s = "chk" + control.Name;
			CheckBox chk = this.GetControls<CheckBox>().FirstOrDefault((CheckBox cb) => cb.Name == s);
			if (chk == null)
			{
				return;
			}
			tx.PromptText = GetSameValue(property);
			chk.CheckedChanged += delegate
			{
				if (chk.Checked)
				{
					if (string.IsNullOrEmpty(tx.Text))
					{
						tx.Text = tx.PromptText;
					}
				}
				else if (tx.Text == tx.PromptText)
				{
					tx.Text = string.Empty;
				}
			};
		}

		private void SetText(Control c, string propName)
		{
			CheckBox chk = MakeCheckBox(c);
			c.Tag = propName;
			chk.Tag = c;
			if (listFields.Contains(c as TextBox))
			{
				if (SetListText(c as TextBox, propName))
				{
					chk.CheckState = CheckState.Indeterminate;
					chk.CheckedChanged += delegate
					{
						if (!chk.Checked)
						{
							c.Text = string.Empty;
						}
					};
				}
				else
				{
					chk.Checked = true;
				}
			}
			else
			{
				SetSimpleText(c, chk, propName);
			}
			c.TextChanged += delegate
			{
				if (!string.IsNullOrEmpty(c.Text))
				{
					chk.Checked = true;
				}
			};
		}

		private bool SetListText(TextBox c, string propName)
		{
			HashSet<string> hashSet = null;
			string text = null;
			bool flag = true;
			foreach (ComicBook book in books)
			{
				string stringPropertyValue = book.GetStringPropertyValue(propName);
				if (text == null)
				{
					text = stringPropertyValue;
				}
				else
				{
					flag &= text == stringPropertyValue;
				}
				HashSet<string> hashSet2 = stringPropertyValue.ListStringToSet(',');
				hashSet = ((hashSet != null) ? new HashSet<string>(hashSet.Intersect(hashSet2)) : hashSet2);
			}
			c.Text = hashSet.ToListString(", ");
			oldLists[c] = hashSet;
			return !flag;
		}

		private void SetSimpleText(Control c, CheckBox chk, string propName)
		{
			bool flag = propName.StartsWith("{");
			bool flag2 = true;
			object obj = null;
			PropertyInfo propertyInfo = (flag ? null : typeof(ComicBook).GetProperty(propName));
			foreach (ComicBook book in books)
			{
				object obj2 = ((propertyInfo == null) ? book.GetStringPropertyValue(propName) : propertyInfo.GetValue(book, null));
				if (obj == null)
				{
					obj = obj2;
				}
				else if (!object.Equals(obj2, obj))
				{
					flag2 = false;
					break;
				}
			}
			if (flag2)
			{
				if (chk != null)
				{
					chk.Checked = true;
				}
				try
				{
					if (obj is bool)
					{
						EditControlUtility.SetText(c, (bool)obj);
					}
					else if (obj is YesNo)
					{
						EditControlUtility.SetText(c, (YesNo)obj);
					}
					else if (obj is MangaYesNo)
					{
						EditControlUtility.SetText(c, (MangaYesNo)obj);
					}
					else if (obj is int)
					{
						int num = (int)obj;
						if (num != -1)
						{
							c.Text = num.ToString();
						}
					}
					else if (obj is DateTime)
					{
						((NullableDateTimePicker)c).Value = (DateTime)obj;
					}
					else
					{
						c.Text = obj.ToString();
					}
				}
				catch (Exception)
				{
				}
			}
			else if (c is NullableDateTimePicker)
			{
				((NullableDateTimePicker)c).Value = DateTime.MinValue;
			}
			else
			{
				c.Text = string.Empty;
			}
		}

		private void SetText(TextBox textBox, string propName, Func<AutoCompleteStringCollection> autoCompletePredicate)
		{
			SetText(textBox, propName);
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

		private void SetText(ComboBox c, string propName, Func<AutoCompleteStringCollection> autoCompletePredicate)
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
					c.Enter += delegate
					{
						string text = c.Text;
						c.DataSource = ComboBoxSkinner.AutoSeparatorList(allItems());
						c.Text = text;
					};
				}
				else
				{
					c.DataSource = ComboBoxSkinner.AutoSeparatorList(allItems());
				}
			}
			SetText(c, propName);
		}

		private void btOK_Click(object sender, EventArgs e)
		{
			Store();
		}
	}
}
