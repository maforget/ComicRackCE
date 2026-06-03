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

        /// <summary>
        /// Sets the checkboxes based on the properties that were pasted last time. The Tag property of each checkbox is used as the property name. If a property has a non-default value, its checkbox is shown in bold.
        /// </summary>
        public void SetChecks(ComicBook data, IEnumerable<string> properties)
        {
            grpCatalog.Visible = !data.IsLinked || !Program.Settings.CatalogOnlyForFileless;
            foreach (CheckBox control in this.GetControls<CheckBox>())
            {
                string text = control.Tag as string;
                if (properties.Contains(text))
                    control.Checked = true;

                try
                {
                    if (!data.IsDefaultValue(text))
                        control.Font = FC.Get(control.Font, FontStyle.Bold);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Returns the list of properties to paste, based on the enabled/checked checkboxes. The Tag property of each checkbox is used as the property name.
        /// </summary>
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

        /// <summary>
        /// Shows the dialog and pastes the data into the books if the user clicks OK.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ShowAndPaste(IWin32Window parent, ComicBook data, IEnumerable<ComicBook> books)
        {
            IEnumerable<string> checks = null;
            int count = books.Count();

            using (ComicDataPasteDialog comicDataPasteDialog = new ComicDataPasteDialog())
            {
                comicDataPasteDialog.Text = StringUtility.Format(comicDataPasteDialog.Text, count); // Set the number of books in the Title

                // Add checkboxes for custom values
                string[] customValues = Program.Settings.ShowCustomBookFields ? Program.Database.CustomValues.Where((string k) => Program.ExtendedSettings.ShowCustomScriptValues || !k.Contains('.')).ToArray() : null;
                if (customValues is not null && customValues.Length != 0) // Add checkboxes for custom values
                {
                    int num = 0;
                    Control control = null;
                    foreach (string text in customValues)
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
                            checkBox.Font = FC.Get(checkBox.Font, FontStyle.Bold);

                        checkBox.Text = text;
                        checkBox.Tag = "{" + text + "}";
                        num++;
                        control = checkBox;
                    }

                    if (control != null)
                        comicDataPasteDialog.grpCustom.Height = control.Bottom + 8;
                }
                else
                {
                    comicDataPasteDialog.grpCustom.Visible = false;
                }

                // Set the checkboxes based on the properties that were pasted last time
                comicDataPasteDialog.SetChecks(data, Program.Settings.PasteProperties.Split(';'));

                // If there are no fileless books, hide the "Scan Information" checkbox. If there are fileless books, disable the "Opened Time" and "Page Count" checkboxes, since these values are not relevant for fileless books.
                if (!books.Any((ComicBook cb) => cb.IsLinked))
                {
                    comicDataPasteDialog.chkScanInformation.Visible = false;
                }
                else
                {
                    CheckBox openedTime = comicDataPasteDialog.chkOpenedTime;
                    CheckBox pageCount = comicDataPasteDialog.chkPageCount;
                    openedTime.Visible = pageCount.Visible = openedTime.Enabled = pageCount.Enabled = false;
                }

                // If books aren't in the library, disable the "Rating", "Color" and "Series Complete" checkboxes. Unless the UpdateComicBookFiles is enabled
                if (!Program.Settings.UpdateComicBookFiles && !books.Any((ComicBook cb) => cb.IsInContainer))
                {
                    CheckBox rating = comicDataPasteDialog.chkRating;
                    CheckBox color = comicDataPasteDialog.chkColor;
                    CheckBox chkSeriesComplete = comicDataPasteDialog.chkSeriesComplete;
                    rating.Enabled = color.Enabled = chkSeriesComplete.Enabled = false;
                }

                // Show the dialog and get the list of properties to paste if the user clicks OK
                if (comicDataPasteDialog.ShowDialog(parent) == DialogResult.OK)
                    checks = comicDataPasteDialog.GetChecks();

                // Save the list of properties to paste for the next time
                Program.Settings.PasteProperties = comicDataPasteDialog.GetChecks().ToListString(";");
            }

            // Return if there is nothing to paste
            if (checks == null)
                return;

            // Paste the data into the books, showing a progress dialog
            AutomaticProgressDialog.Process(parent, TR.Messages["WriteData", "Pasting Data"], TR.Messages["WriteDataText", "Pasting Data into selected Books"], 1000, () =>
            {
                int num2 = 0;
                foreach (ComicBook book in books)
                {
                    if (AutomaticProgressDialog.ShouldAbort)
                        break;

                    book.RefreshInfoFromFile(RefreshInfoOptions.None);
                    book.CopyDataFrom(data, checks);
                    AutomaticProgressDialog.Value = num2++ * 100 / count;
                }
            }, AutomaticProgressDialogOptions.EnableCancel);
        }
    }
}
