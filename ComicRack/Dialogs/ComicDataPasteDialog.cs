using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Reflection;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Viewer.Config;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class ComicDataPasteDialog : FormEx
	{
		public ComicDataPasteDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			this.RestorePosition();
			this.RestorePanelStates();
			LocalizeUtility.Localize(this, null);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			FormUtility.RegisterPanelToTabToggle(pageData, PropertyCaller.CreateFlagsValueStore(Program.Settings, "TabLayouts", TabLayouts.Paste));
		}

		public void SetChecks(ComicBook data, IEnumerable<string> properties)
		{
			grpCatalog.Visible = !data.IsLinked || !Program.Settings.CatalogOnlyForFileless;
			foreach (CheckBox control in this.GetControls<CheckBox>())
			{
				string text = control.Tag as string;
				if (properties.Contains(text))
				{
					control.Checked = true;
				}
				try
				{
					if (!data.IsDefaultValue(text))
					{
						control.Font = FC.Get(control.Font, FontStyle.Bold);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		public IEnumerable<string> GetChecks()
		{
			return (from cb in this.GetControls<CheckBox>()
				where cb.Enabled
				where cb.Checked && cb.Tag != null
				select (string)cb.Tag).ToArray();
		}

		private void btMarkDefined_Click(object sender, EventArgs e)
		{
			foreach (CheckBox control in this.GetControls<CheckBox>())
			{
				control.Checked = control.Font.Bold;
			}
		}

		private void btMarkAll_Click(object sender, EventArgs e)
		{
			foreach (CheckBox control in this.GetControls<CheckBox>())
			{
				control.Checked = true;
			}
		}

		private void btMarkNone_Click(object sender, EventArgs e)
		{
			foreach (CheckBox control in this.GetControls<CheckBox>())
			{
				control.Checked = false;
			}
		}

		public static void ShowAndPaste(IWin32Window parent, ComicBook data, IEnumerable<ComicBook> books)
		{
			IEnumerable<string> checks = null;
			int count = books.Count();
			string[] array = (Program.Settings.ShowCustomBookFields ? Program.Database.CustomValues.Where((string k) => Program.ExtendedSettings.ShowCustomScriptValues || !k.Contains('.')).ToArray() : null);
			using (ComicDataPasteDialog comicDataPasteDialog = new ComicDataPasteDialog())
			{
				comicDataPasteDialog.SetChecks(data, Program.Settings.PasteProperties.Split(';'));
				comicDataPasteDialog.Text = StringUtility.Format(comicDataPasteDialog.Text, count);
				if (array == null || array.Length == 0)
				{
					comicDataPasteDialog.grpCustom.Visible = false;
				}
				else
				{
					int num = 0;
					Control control = null;
					string[] array2 = array;
					foreach (string text in array2)
					{
						CheckBox checkBox = (CheckBox)comicDataPasteDialog.chkBookStore.Clone();
						comicDataPasteDialog.grpCustom.Controls.Add(checkBox);
						checkBox.Top = comicDataPasteDialog.chkBookStore.Top + (comicDataPasteDialog.chkBookCondition.Top - comicDataPasteDialog.chkBookStore.Top) * (num / 4);
						switch (num % 4)
						{
						case 0:
							checkBox.Left = comicDataPasteDialog.chkBookStore.Left;
							break;
						case 1:
							checkBox.Left = comicDataPasteDialog.chkBookPrice.Left;
							break;
						case 2:
							checkBox.Left = comicDataPasteDialog.chkBookLocation.Left;
							break;
						case 3:
							checkBox.Left = comicDataPasteDialog.chkBookAge.Left;
							break;
						}
						checkBox.Visible = true;
						if (data.GetCustomValue(text) != null)
						{
							checkBox.Font = FC.Get(checkBox.Font, FontStyle.Bold);
						}
						checkBox.Text = text;
						checkBox.Tag = "{" + text + "}";
						num++;
						control = checkBox;
					}
					if (control != null)
					{
						comicDataPasteDialog.grpCustom.Height = control.Bottom + 8;
					}
				}
				if (!books.Any((ComicBook cb) => cb.IsLinked))
				{
					comicDataPasteDialog.chkScanInformation.Visible = false;
				}
				else
				{
					CheckBox checkBox2 = comicDataPasteDialog.chkOpenedTime;
					CheckBox checkBox3 = comicDataPasteDialog.chkPageCount;
					CheckBox checkBox4 = comicDataPasteDialog.chkOpenedTime;
					bool flag2 = (comicDataPasteDialog.chkPageCount.Enabled = false);
					bool flag4 = (checkBox4.Enabled = flag2);
					bool visible = (checkBox3.Visible = flag4);
					checkBox2.Visible = visible;
				}
				if (!books.Any((ComicBook cb) => cb.IsInContainer))
				{
					CheckBox checkBox5 = comicDataPasteDialog.chkCommunityRating;
					CheckBox checkBox6 = comicDataPasteDialog.chkRating;
					CheckBox checkBox7 = comicDataPasteDialog.chkTags;
					CheckBox checkBox8 = comicDataPasteDialog.chkColor;
					bool flag7 = (comicDataPasteDialog.chkSeriesComplete.Enabled = false);
					bool flag2 = (checkBox8.Enabled = flag7);
					bool flag4 = (checkBox7.Enabled = flag2);
					bool visible = (checkBox6.Enabled = flag4);
					checkBox5.Enabled = visible;
				}
				if (comicDataPasteDialog.ShowDialog(parent) == DialogResult.OK)
				{
					checks = comicDataPasteDialog.GetChecks();
				}
				Program.Settings.PasteProperties = comicDataPasteDialog.GetChecks().ToListString(";");
			}
			if (checks == null)
			{
				return;
			}
			AutomaticProgressDialog.Process(parent, TR.Messages["WriteData", "Pasting Data"], TR.Messages["WriteDataText", "Pasting Data into selected Books"], 1000, delegate
			{
				int num2 = 0;
				foreach (ComicBook book in books)
				{
					if (AutomaticProgressDialog.ShouldAbort)
					{
						break;
					}
					book.RefreshInfoFromFile(RefreshInfoOptions.None);
					book.CopyDataFrom(data, checks);
					AutomaticProgressDialog.Value = num2++ * 100 / count;
				}
			}, AutomaticProgressDialogOptions.EnableCancel);
		}
	}
}
