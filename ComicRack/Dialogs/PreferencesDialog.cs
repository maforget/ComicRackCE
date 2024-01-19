using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Presentation;
using cYo.Common.Reflection;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Xml;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Engine.IO.Network;
using cYo.Projects.ComicRack.Engine.IO.Provider;
using cYo.Projects.ComicRack.Engine.Sync;
using cYo.Projects.ComicRack.Plugins;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class PreferencesDialog : Form
	{
		private const int MaximumMemoryStepSize = 32;

		private static readonly string DuplicatePackageText = TR.Messages["ScriptPackageExists", "A Script Package with the same name already exists! Do you want to overwrite this Package?"];

		private PluginEngine pluginEngine;

		private bool blockSetTab;

		public string AutoInstallPlugin
		{
			get;
			set;
		}

		public bool NeedsRestart
		{
			get;
			set;
		}

		public string BackupFile
		{
			get;
			set;
		}

		public PluginEngine Plugins
		{
			get
			{
				return pluginEngine;
			}
			set
			{
				pluginEngine = value;
				FillScriptsList();
			}
		}

		public PreferencesDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			lvPackages.Columns.ScaleDpi();
			lvScripts.Columns.ScaleDpi();
			this.RestorePosition();
			this.RestorePanelStates();
			tabReader.Tag = pageReader;
			tabBehavior.Tag = pageBehavior;
			tabLibraries.Tag = pageLibrary;
			tabScripts.Tag = pageScripts;
			tabAdvanced.Tag = pageAdvanced;
			tabButtons.AddRange(new CheckBox[5]
			{
				tabReader,
				tabBehavior,
				tabLibraries,
				tabScripts,
				tabAdvanced
			});
			lbLanguages.ItemHeight = FormUtility.ScaleDpiY(lbLanguages.ItemHeight);
			tabReader.Image = ((Bitmap)tabReader.Image).ScaleDpi();
			tabAdvanced.Image = ((Bitmap)tabAdvanced.Image).ScaleDpi();
			tabBehavior.Image = ((Bitmap)tabBehavior.Image).ScaleDpi();
			tabLibraries.Image = ((Bitmap)tabLibraries.Image).ScaleDpi();
			tabScripts.Image = ((Bitmap)tabScripts.Image).ScaleDpi();
			FormUtility.FillPanelWithOptions(pageBehavior, Program.Settings, TR.Load("Settings"));
			packageImageList.Images.Add(Resources.Package);
			LocalizeUtility.Localize(this, components);
			LocalizeUtility.Localize(TR.Load(base.Name), cbNavigationOverlayPosition);
			FormUtility.RegisterPanelToTabToggle(pageReader, PropertyCaller.CreateFlagsValueStore(Program.Settings, "TabLayouts", TabLayouts.ReaderSettings));
			FormUtility.RegisterPanelToTabToggle(pageBehavior, PropertyCaller.CreateFlagsValueStore(Program.Settings, "TabLayouts", TabLayouts.BehaviorSettings));
			FormUtility.RegisterPanelToTabToggle(pageLibrary, PropertyCaller.CreateFlagsValueStore(Program.Settings, "TabLayouts", TabLayouts.LibrarySettings));
			FormUtility.RegisterPanelToTabToggle(pageScripts, PropertyCaller.CreateFlagsValueStore(Program.Settings, "TabLayouts", TabLayouts.ScriptSettings));
			FormUtility.RegisterPanelToTabToggle(pageAdvanced, PropertyCaller.CreateFlagsValueStore(Program.Settings, "TabLayouts", TabLayouts.AdvancedSettings));
			Program.Scanner.ScanNotify += DatabaseScanNotify;
			IdleProcess.Idle += ApplicationIdle;
			numMemPageCount.Minimum = 20m;
			numMemPageCount.Maximum = 100m;
			numMemThumbSize.Minimum = 5m;
			numMemThumbSize.Maximum = 100m;
			lbLanguages.Items.Add(new TRInfo());
			lbLanguages.Items.Add(new TRInfo("en"));
			TRInfo[] installedLanguages = Program.InstalledLanguages;
			foreach (TRInfo item in installedLanguages)
			{
				lbLanguages.Items.Add(item);
			}
			SetSettings();
			foreach (WatchFolder watchFolder in Program.Database.WatchFolders)
			{
				lbPaths.Items.Add(watchFolder.Folder, watchFolder.Watch);
			}
			lbPaths_SelectedIndexChanged(lbPaths, EventArgs.Empty);
			SetScanButtonText();
			btResetMessages.Enabled = Program.Settings.HiddenMessageBoxes != HiddenMessageBoxes.None;
			FillExtensionsList();
			chkOverwriteAssociations.Checked = Program.Settings.OverwriteAssociations;
			if (!FileFormat.CanRegisterShell)
			{
				Win7.ShowShield(btAssociateExtensions);
			}
			else
			{
				btAssociateExtensions.Visible = false;
				lbFormats.Width = btAssociateExtensions.Right - lbFormats.Left;
			}
			Program.InternetCache.SizeChanged += UpdateDiskCacheStatus;
			Program.ImagePool.Pages.DiskCache.SizeChanged += UpdateDiskCacheStatus;
			Program.ImagePool.Thumbs.DiskCache.SizeChanged += UpdateDiskCacheStatus;
			UpdateDiskCacheStatus(this, null);
			UpdateMemoryCacheStatus();
			RefreshPackageList();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			SetTab((activeTab != -1) ? tabButtons[activeTab] : tabReader);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			if (!string.IsNullOrEmpty(AutoInstallPlugin))
			{
				SetTab(tabScripts);
				if (InstallPlugin(AutoInstallPlugin))
				{
					RefreshPackageList();
				}
			}
		}

		private void btTestWifi_Click(object sender, EventArgs e)
		{
			string text = string.Empty;
			using (new WaitCursor(this))
			{
				foreach (ISyncProvider item in DeviceSyncFactory.Discover(DeviceSyncFactory.ParseWifiAddressList(txWifiAddresses.Text)))
				{
					text = text.AppendWithSeparator(", ", item.Device.Model);
				}
			}
			lblWifiStatus.Text = (string.IsNullOrEmpty(text) ? TR.Load(base.Name)["msgNoDevicesFound", "No devices found!"] : TR.Load(base.Name)["msgDevicesFound", "{0} found!"].SafeFormat(text));
		}

		private void chkAdvanced_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox.Checked)
			{
				SetTab(checkBox);
			}
		}

		private void chkLibraryGauges_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = chkLibraryGaugesNew;
			CheckBox checkBox2 = chkLibraryGaugesUnread;
			CheckBox checkBox3 = chkLibraryGaugesTotal;
			bool flag = (chkLibraryGaugesNumeric.Enabled = chkLibraryGauges.Checked);
			bool flag3 = (checkBox3.Enabled = flag);
			bool enabled = (checkBox2.Enabled = flag3);
			checkBox.Enabled = enabled;
		}

		private void tbSystemMemory_ValueChanged(object sender, EventArgs e)
		{
			int num = tbMaximumMemoryUsage.Value * 32;
			if (num == 1024)
			{
				lblMaximumMemoryUsageValue.Text = TR.Default["Unlimited"];
			}
			else
			{
				lblMaximumMemoryUsageValue.Text = $"{num} MB";
			}
		}

		private void lbPaths_DragDrop(object sender, DragEventArgs e)
		{
			(from d in (e.Data.GetData(DataFormats.FileDrop) as string[])?.Select((string d) => (!Directory.Exists(d)) ? Path.GetDirectoryName(d) : d)
			 where !lbPaths.Items.Contains(d)
			 select d).ForEach(delegate (string d)
			 {
				 lbPaths.Items.Add(d);
			 });
		}

		private void lbPaths_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None);
		}

		private void lvScripts_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			Command command = e.Item.Tag as Command;
			if (command != null)
			{
				command.Enabled = e.Item.Checked;
			}
		}

		private void lvScripts_SelectedIndexChanged(object sender, EventArgs e)
		{
			Command command = (from lvi in lvScripts.SelectedItems.OfType<ListViewItem>()
							   select lvi.Tag as Command).FirstOrDefault();
			btConfigScript.Enabled = command != null && command.Configure != null;
			if (btConfigScript.Enabled)
			{
				btConfigScript.Tag = command.Configure;
			}
		}

		private void btConfigScript_Click(object sender, EventArgs e)
		{
			(btConfigScript.Tag as Command)?.Invoke(new object[0], catchErrors: true);
		}

		private void lbPaths_DrawItemText(object sender, DrawItemEventArgs e)
		{
			CheckedListBoxEx checkedListBoxEx = (CheckedListBoxEx)sender;
			using (StringFormat format = new StringFormat
			{
				LineAlignment = StringAlignment.Center,
				Trimming = StringTrimming.EllipsisPath
			})
			{
				checkedListBoxEx.DrawDefaultItemText(e, format);
			}
		}

		private void DatabaseScanNotify(object sender, ComicScanNotifyEventArgs e)
		{
			try
			{
				if (!this.BeginInvokeIfRequired(delegate
				{
					DatabaseScanNotify(sender, e);
				}))
				{
					if (string.IsNullOrEmpty(e.File))
					{
						lblScan.Text = string.Empty;
					}
					else
					{
						lblScan.Text = StringUtility.Format(LocalizeUtility.GetText(this, "Scanning", "Scanning '{0}' ..."), e.File);
					}
					SetScanButtonText();
				}
			}
			catch (Exception)
			{
			}
		}

		private void btClearThumbnailCache_Click(object sender, EventArgs e)
		{
			using (new WaitCursor())
			{
				Program.ImagePool.Thumbs.DiskCache.Clear();
			}
		}

		private void btClearPageCache_Click(object sender, EventArgs e)
		{
			using (new WaitCursor())
			{
				Program.ImagePool.Pages.DiskCache.Clear();
			}
		}

		private void btClearInternetCache_Click(object sender, EventArgs e)
		{
			using (new WaitCursor())
			{
				Program.InternetCache.Clear();
			}
		}

		private void btAddFolder_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.Description = LocalizeUtility.GetText(this, "SelectComicFolder", "Please select a folder containing Books");
				folderBrowserDialog.ShowNewFolderButton = true;
				if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
				{
					lbPaths.Items.Add(folderBrowserDialog.SelectedPath);
					if (lbPaths.SelectedIndex == -1)
					{
						lbPaths.SelectedIndex = 0;
					}
				}
			}
		}

		private void btChangeFolder_Click(object sender, EventArgs e)
		{
			string text = lbPaths.SelectedItem as string;
			if (text == null)
			{
				return;
			}
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.Description = LocalizeUtility.GetText(this, "SelectComicFolder", "Please select a folder containing Books");
				folderBrowserDialog.ShowNewFolderButton = true;
				folderBrowserDialog.SelectedPath = text;
				if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
				{
					lbPaths.Items[lbPaths.SelectedIndex] = folderBrowserDialog.SelectedPath;
				}
			}
		}

		private void btAddLibraryFolder_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.Description = LocalizeUtility.GetText(this, "SelectScriptFolder", "Please select a script library folder");
				folderBrowserDialog.ShowNewFolderButton = false;
				if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
				{
					if (txLibraries.Text.Trim().Length > 0)
					{
						txLibraries.Text += ";";
					}
					txLibraries.Text += folderBrowserDialog.SelectedPath;
				}
			}
		}

		private void btRemoveFolder_Click(object sender, EventArgs e)
		{
			int selectedIndex = lbPaths.SelectedIndex;
			if (selectedIndex != -1)
			{
				lbPaths.Items.RemoveAt(selectedIndex);
			}
		}

		private void btOpenFolder_Click(object sender, EventArgs e)
		{
			Program.ShowExplorer(lbPaths.SelectedItem as string);
		}

		private void btScan_Click(object sender, EventArgs e)
		{
			if (Program.Scanner.IsScanning)
			{
				Program.Scanner.Stop(clearQueue: true);
				return;
			}
			foreach (string item in lbPaths.Items)
			{
				Program.Scanner.ScanFileOrFolder(item, all: true, chkAutoRemoveMissing.Checked);
			}
		}

		private void lbPaths_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			string s = lbPaths.Items[e.Index] as string;
			using (StringFormat format = new StringFormat
			{
				Trimming = StringTrimming.EllipsisPath
			})
			{
				using (Brush brush = new SolidBrush(e.ForeColor))
				{
					e.Graphics.DrawString(s, e.Font, brush, e.Bounds, format);
				}
			}
			e.DrawFocusRectangle();
		}

		private void lbPaths_SelectedIndexChanged(object sender, EventArgs e)
		{
			Button button = btOpenFolder;
			bool enabled = (btRemoveFolder.Enabled = lbPaths.SelectedIndex != -1);
			button.Enabled = enabled;
			btScan.Enabled = lbPaths.Items.Count > 0;
		}

		private void btResetMessages_Click(object sender, EventArgs e)
		{
			Program.Settings.HiddenMessageBoxes = HiddenMessageBoxes.None;
			btResetMessages.Enabled = false;
		}

		private void ApplicationIdle(object sender, EventArgs e)
		{
			CheckBox checkBox = chkEnableDisplayChangeAnimation;
			CheckBox checkBox2 = chkEnableHardwareFiltering;
			CheckBox checkBox3 = chkEnableSoftwareFiltering;
			bool flag = (chkEnableInertialMouseScrolling.Enabled = chkEnableHardware.Checked);
			bool flag3 = (checkBox3.Enabled = flag);
			bool enabled = (checkBox2.Enabled = flag3);
			checkBox.Enabled = enabled;
			chkAutoConnectShares.Enabled = chkLookForShared.Checked;
			btRemovePackage.Enabled = lvPackages.SelectedItems.Count > 0;
			labelPageOverlay.Enabled = chkShowCurrentPageOverlay.Checked;
			labelVisiblePartOverlay.Enabled = chkShowVisiblePartOverlay.Checked;
			labelStatusOverlay.Enabled = chkShowStatusOverlay.Checked;
			labelNavigationOverlay.Top = ((cbNavigationOverlayPosition.SelectedIndex != 0) ? labelPageOverlay.Top : (labelVisiblePartOverlay.Bottom - labelNavigationOverlay.Height));
			labelPageOverlay.Text = (chkShowPageNames.Checked ? LocalizeUtility.GetText(this, "PageNumberAndName", "Page\nName") : LocalizeUtility.GetText(this, "PageNumberOnly", "Page"));
		}

		private void btApply_Click(object sender, EventArgs e)
		{
			Apply();
		}

		private void tbSaturation_DoubleClick(object sender, EventArgs e)
		{
			tbSaturation.Value = 0;
		}

		private void tbBrightness_DoubleClick(object sender, EventArgs e)
		{
			tbBrightness.Value = 0;
		}

		private void tbContrast_DoubleClick(object sender, EventArgs e)
		{
			tbContrast.Value = 0;
		}

		private void tbSharpening_DoubleClick(object sender, EventArgs e)
		{
			tbSharpening.Value = 0;
		}

		private void tbGamma_DoubleClick(object sender, EventArgs e)
		{
			tbGamma.Value = 0;
		}

		private void btReset_Click(object sender, EventArgs e)
		{
			TrackBarLite trackBarLite = tbSaturation;
			TrackBarLite trackBarLite2 = tbBrightness;
			TrackBarLite trackBarLite3 = tbContrast;
			int num2 = (tbGamma.Value = 0);
			int num4 = (trackBarLite3.Value = num2);
			int num7 = (trackBarLite.Value = (trackBarLite2.Value = num4));
		}

		private void tbOverlayScalingChanged(object sender, EventArgs e)
		{
			toolTip.SetToolTip(tbOverlayScaling, $"{tbOverlayScaling.Value}%");
		}

		private void tbColorAdjustmentChanged(object sender, EventArgs e)
		{
			TrackBarLite trackBarLite = (TrackBarLite)sender;
			toolTip.SetToolTip(trackBarLite, string.Format("{1}{0}%", trackBarLite.Value, (trackBarLite.Value > 0) ? "+" : string.Empty));
		}

		private void lbLanguages_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1)
			{
				return;
			}
			TRInfo tRInfo = (TRInfo)lbLanguages.Items[e.Index];
			e.DrawBackground();
			using (Brush brush = new SolidBrush((tRInfo.CompletionPercent > 95f) ? ForeColor : Color.Red))
			{
				Rectangle bounds = e.Bounds;
				string cultureCode = tRInfo.CultureName ?? CultureInfo.InstalledUICulture.Name;
				using (Image image = Flags.GetFlagFromCulture(cultureCode))
				{
					if (image != null)
					{
						float scale = image.Size.GetScale(bounds.Pad(2).Size);
						e.Graphics.DrawImage(image, bounds.X + 1, bounds.Y + 2, (float)image.Width * scale, (float)image.Height * scale);
					}
				}
				bounds.X += FormUtility.ScaleDpiX(20);
				bounds.Width -= FormUtility.ScaleDpiX(20);
				string[] array = tRInfo.ToString().Split('\t');
				using (StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap)
				{
					Trimming = StringTrimming.Character,
					LineAlignment = StringAlignment.Center
				})
				{
					e.Graphics.DrawString(array[0], e.Font, brush, bounds, stringFormat);
					if (array.Length > 1)
					{
						stringFormat.Alignment = StringAlignment.Far;
						e.Graphics.DrawString(array[1], e.Font, brush, bounds, stringFormat);
					}
				}
			}
			if ((e.State & DrawItemState.Focus) != 0)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds);
			}
		}

		private void btBackupDatabase_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			try
			{
				dlg.Title = btBackupDatabase.Text.Replace(".", string.Empty);
				dlg.FileName = string.Format("ComicDB Backup {0}.zip", DateTime.Now.ToString("yyyy-MM-dd"));
				dlg.Filter = TR.Load("FileFilter")["ComicRackBackup", "ComicRack Database|*.zip"];
				dlg.DefaultExt = ".zip";
				if (dlg.ShowDialog(this) != DialogResult.OK)
				{
					return;
				}
				try
				{
					AutomaticProgressDialog.Process(this, TR.Messages["DatabaseBackup", "Database Backup"], TR.Messages["DatabaseBackupText", "Creating and saving the Backup File"], 1000, delegate
					{
						Program.DatabaseManager.BackupTo(dlg.FileName, Program.Paths.CustomThumbnailPath);
					}, AutomaticProgressDialogOptions.None);
				}
				catch
				{
					MessageBox.Show(this, TR.Messages["DatabaseBackupError", "There was an error saving the Database backup"], TR.Messages["Attention", "Attention"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			finally
			{
				if (dlg != null)
				{
					((IDisposable)dlg).Dispose();
				}
			}
		}

		private void btRestoreDatabase_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Title = btRestoreDatabase.Text.Replace(".", string.Empty);
				openFileDialog.Filter = TR.Load("FileFilter")["ComicRackBackup", "ComicRack Backup|*.zip"];
				openFileDialog.CheckFileExists = true;
				if (openFileDialog.ShowDialog(this) == DialogResult.OK)
				{
					BackupFile = openFileDialog.FileName;
				}
			}
		}

		private void btTranslate_Click(object sender, EventArgs e)
		{
			Program.StartDocument("https://web.archive.org/web/20170528182733/http://comicrack.cyolito.com/faqs/12-how-to-create-language-packs");
		}

		private void memCacheUpate_Tick(object sender, EventArgs e)
		{
			UpdateMemoryCacheStatus();
		}

		private void btInstallPackage_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Title = btInstallPackage.Text.Replace(".", string.Empty);
				openFileDialog.Filter = TR.Load("FileFilter")["ScriptPackageOpen", "ComicRack Plugin|*.crplugin|Script Archive|*.zip"];
				openFileDialog.CheckFileExists = true;
				if (openFileDialog.ShowDialog(this) == DialogResult.OK && InstallPlugin(openFileDialog.FileName))
				{
					RefreshPackageList();
				}
			}
		}

		private void btRemovePackage_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem selectedItem in lvPackages.SelectedItems)
			{
				NeedsRestart = true;
				if (!Program.ScriptPackages.Uninstall(selectedItem.Tag as PackageManager.Package))
				{
					MessageBox.Show(this, TR.Messages["FailedRemovePackage", "Failed to uninstall package. Please restart ComicRack and try again!"], TR.Messages["Attention", "Attention"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			RefreshPackageList();
		}

		private void lvPackages_DoubleClick(object sender, EventArgs e)
		{
			ListViewItem listViewItem = lvPackages.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
			if (listViewItem != null)
			{
				PackageManager.Package package = listViewItem.Tag as PackageManager.Package;
				if (package.PackageType == PackageManager.PackageType.Installed)
				{
					Program.ShowExplorer(package.PackagePath);
				}
			}
		}

		private void lvPackages_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				string[] array = e.Data.GetData(DataFormats.FileDrop) as string[];
				string[] array2 = array;
				foreach (string f in array2)
				{
					if (!InstallPlugin(f))
					{
						break;
					}
				}
				RefreshPackageList();
			}
			catch (Exception)
			{
			}
		}

		private void lvPackages_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None);
		}

		private void keyboardShortcutEditor_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None);
		}

		private void keyboardShortcutEditor_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				LoadKeyboard(((string[])e.Data.GetData(DataFormats.FileDrop))[0]);
			}
			catch (Exception)
			{
			}
		}

		private void btExportKeyboard_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Title = btExportKeyboard.Text.Replace("&", string.Empty);
				saveFileDialog.FileName = TR.Load("FileFilter")["KeyboardLayout", "Keyboard Layout"] + ".xml";
				saveFileDialog.Filter = TR.Load("FileFilter")["KeyboardLayoutFilter", "Keyboard Layout|*.xml"];
				saveFileDialog.DefaultExt = ".xml";
				saveFileDialog.CheckPathExists = true;
				saveFileDialog.OverwritePrompt = true;
				if (saveFileDialog.ShowDialog(this) != DialogResult.Cancel)
				{
					try
					{
						List<StringPair> data = keyboardShortcutEditor.Shortcuts.GetKeyMapping().ToList();
						XmlUtility.Store(saveFileDialog.FileName, data);
						Program.Settings.KeyboardLayouts.UpdateMostRecent(saveFileDialog.FileName);
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, string.Format(TR.Messages["CouldNotExportKeyboardLayout", "Could not export Keyboard Layout!\nReason: {0}"], ex.Message), TR.Messages["Attention", "Attention"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
		}

		private void btLoadKeyboard_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Title = btImportKeyboard.Text.Replace("&", string.Empty);
				openFileDialog.Filter = TR.Load("FileFilter")["KeyboardLayoutFilter", "Keyboard Layout|*.xml"];
				openFileDialog.DefaultExt = ".xml";
				openFileDialog.CheckFileExists = true;
				if (openFileDialog.ShowDialog(this) != DialogResult.Cancel)
				{
					LoadKeyboard(openFileDialog.FileName);
				}
			}
		}

		private void btImportKeyboard_ShowContextMenu(object sender, EventArgs e)
		{
			FormUtility.SafeToolStripClear(cmKeyboardLayout.Items, 2);
			foreach (string keyboardLayout in Program.Settings.KeyboardLayouts)
			{
				string file = keyboardLayout;
				cmKeyboardLayout.Items.Add(keyboardLayout, null, delegate
				{
					LoadKeyboard(file);
				});
			}
			cmKeyboardLayout.Items[1].Visible = cmKeyboardLayout.Items.Count > 2;
		}

		private void miDefaultKeyboardLayout_Click(object sender, EventArgs e)
		{
			keyboardShortcutEditor.Shortcuts.SetKeyMapping(Program.DefaultKeyboardMapping);
			keyboardShortcutEditor.RefreshList();
		}

		private void chkHideSampleScripts_CheckedChanged(object sender, EventArgs e)
		{
			FillScriptsList();
		}

		private void FillScriptsList()
		{
			lvScripts.BeginUpdate();
			try
			{
				lvScripts.Items.Clear();
				if (pluginEngine == null)
				{
					return;
				}
				foreach (Command allCommand in pluginEngine.GetAllCommands())
				{
					if (!lvScripts.Items.ContainsKey(allCommand.Key) && (!allCommand.Name.Contains("[Code Sample]") || !chkHideSampleScripts.Checked))
					{
						string value = IniFile.GetValue(Path.Combine(allCommand.Environment.CommandPath, "package.ini"), "Name", "Other");
						string hookDescription = PluginEngine.GetHookDescription(allCommand.Hook);
						ListViewGroup group = lvScripts.Groups[hookDescription] ?? lvScripts.Groups.Add(hookDescription, hookDescription);
						ListViewItem listViewItem = lvScripts.Items.Add(allCommand.Key, allCommand.GetLocalizedName(), allCommand.Key);
						listViewItem.SubItems.Add(value);
						listViewItem.Tag = allCommand;
						listViewItem.Checked = allCommand.Enabled;
						listViewItem.ToolTipText = allCommand.GetLocalizedDescription();
						listViewItem.Group = group;
						Image commandImage = allCommand.CommandImage;
						if (commandImage != null)
						{
							imageList.Images.Add(allCommand.Key, commandImage);
						}
					}
				}
				lvScripts.SortGroups();
			}
			finally
			{
				lvScripts.EndUpdate();
			}
		}

		private void LoadKeyboard(string f)
		{
			try
			{
				keyboardShortcutEditor.Shortcuts.SetKeyMapping(XmlUtility.Load<List<StringPair>>(f));
				keyboardShortcutEditor.RefreshList();
				Program.Settings.KeyboardLayouts.UpdateMostRecent(f);
			}
			catch (Exception ex)
			{
				Program.Settings.KeyboardLayouts.Remove(f);
				MessageBox.Show(this, string.Format(TR.Messages["CouldNotImportKeyboardLayout", "Could not import Keyboard Layout!\nReason: {0}"], ex.Message), TR.Messages["Attention", "Attention"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private bool InstallPlugin(string f)
		{
			if (Program.ScriptPackages.PackageFileExists(f) && !QuestionDialog.Ask(this, DuplicatePackageText, TR.Default["Yes", "Yes"]))
			{
				return false;
			}
			bool flag = Program.ScriptPackages.Install(f);
			NeedsRestart |= flag;
			return flag;
		}

		private void FillExtensionsList()
		{
			lbFormats.Items.Clear();
			foreach (FileFormat item in from f in Providers.Readers.GetSourceFormats()
										orderby f
										select f)
			{
				int index = lbFormats.Items.Add(item);
				lbFormats.SetItemChecked(index, item.IsShellRegistered("cYo.ComicRack"));
			}
		}

		public void Apply()
		{
			string cultureName = ((TRInfo)lbLanguages.SelectedItem).CultureName;
			if (cultureName != Program.Settings.CultureName)
			{
				NeedsRestart = true;
				Program.Settings.CultureName = cultureName;
			}
			NeedsRestart |= Plugins != null && !string.IsNullOrEmpty(Program.Settings.PluginsStates) && Plugins.CommandStates != Program.Settings.PluginsStates;
			FormUtility.RetrieveOptionsFromPanel(pageBehavior, Program.Settings);
			Program.Settings.MouseWheelSpeed = (float)tbMouseWheel.Value / 10f;
			Program.Settings.LibraryGaugesFormat = LibraryGauges.None.SetMask(LibraryGauges.Unread, chkLibraryGaugesUnread.Checked).SetMask(LibraryGauges.New, chkLibraryGaugesNew.Checked).SetMask(LibraryGauges.Total, chkLibraryGaugesTotal.Checked)
				.SetMask(LibraryGauges.Numeric, chkLibraryGaugesNumeric.Checked);
			Program.Settings.DisplayLibraryGauges = chkLibraryGauges.Checked;
			Program.Settings.PageImageDisplayOptions = Program.Settings.PageImageDisplayOptions.SetMask(ImageDisplayOptions.AnamorphicScaling, chkAnamorphicScaling.Checked);
			Program.Settings.PageImageDisplayOptions = Program.Settings.PageImageDisplayOptions.SetMask(ImageDisplayOptions.HighQuality, chkHighQualityDisplay.Checked);
			Program.Settings.InternetCacheEnabled = chkEnableInternetCache.Checked;
			Program.Settings.InternetCacheSizeMB = (int)numInternetCacheSize.Value;
			Program.Settings.ThumbCacheEnabled = chkEnableThumbnailCache.Checked;
			Program.Settings.ThumbCacheSizeMB = (int)numThumbnailCacheSize.Value;
			Program.Settings.PageCacheEnabled = chkEnablePageCache.Checked;
			Program.Settings.PageCacheSizeMB = (int)numPageCacheSize.Value;
			Program.Settings.MaximumMemoryMB = tbMaximumMemoryUsage.Value * 32;
			Program.Settings.MemoryPageCacheOptimized = chkMemPageOptimized.Checked;
			Program.Settings.MemoryPageCacheCount = (int)numMemPageCount.Value;
			Program.Settings.MemoryThumbCacheOptimized = chkMemThumbOptimized.Checked;
			Program.Settings.MemoryThumbCacheSizeMB = (int)numMemThumbSize.Value;
			BitmapAdjustmentOptions options = (chkAutoContrast.Checked ? BitmapAdjustmentOptions.AutoContrast : BitmapAdjustmentOptions.None);
			Program.Settings.GlobalColorAdjustment = new BitmapAdjustment((float)tbSaturation.Value / 100f, (float)tbBrightness.Value / 100f, (float)tbContrast.Value / 100f, (float)tbGamma.Value / 100f, options, tbSharpening.Value);
			Program.Settings.ShowCurrentPageOverlay = chkShowCurrentPageOverlay.Checked;
			Program.Settings.ShowVisiblePagePartOverlay = chkShowVisiblePartOverlay.Checked;
			Program.Settings.ShowStatusOverlay = chkShowStatusOverlay.Checked;
			Program.Settings.ShowNavigationOverlay = chkShowNavigationOverlay.Checked;
			Program.Settings.NavigationOverlayOnTop = cbNavigationOverlayPosition.SelectedIndex == 1;
			Program.Settings.CurrentPageShowsName = chkShowPageNames.Checked;
			Program.Settings.HardwareAcceleration = chkEnableHardware.Checked;
			Program.Settings.SmoothScrolling = chkSmoothAutoScrolling.Checked;
			Program.Settings.DisplayChangeAnimation = chkEnableDisplayChangeAnimation.Checked;
			Program.Settings.SoftwareFiltering = chkEnableSoftwareFiltering.Checked;
			Program.Settings.HardwareFiltering = chkEnableHardwareFiltering.Checked;
			Program.Settings.FlowingMouseScrolling = chkEnableInertialMouseScrolling.Checked;
			Program.Settings.OverlayScaling = tbOverlayScaling.Value;
			Program.Settings.RemoveMissingFilesOnFullScan = chkAutoRemoveMissing.Checked;
			Program.Settings.DontAddRemoveFiles = chkDontAddRemovedFiles.Checked;
			if (!Program.Settings.DontAddRemoveFiles)
			{
				Program.Database.ClearBlackList();
			}
			Program.Settings.OverwriteAssociations = chkOverwriteAssociations.Checked;
			Program.Settings.LookForShared = chkLookForShared.Checked;
			Program.Settings.AutoConnectShares = chkAutoConnectShares.Checked;
			CopyWatchFoldersToDatabase();
			RegisterFileTypes();
			Program.Settings.UpdateComicFiles = chkUpdateComicFiles.Checked;
			Program.Settings.AutoUpdateComicsFiles = chkAutoUpdateComicFiles.Checked;
			Program.Settings.IgnoredCoverImages = (string.IsNullOrEmpty(txCoverFilter.Text) ? null : txCoverFilter.Text);
			Program.Settings.ScriptingLibraries = txLibraries.Text;
			Program.Settings.Scripting = !chkDisableScripting.Checked;
			Program.Settings.HideSampleScripts = chkHideSampleScripts.Checked;
			if (!string.IsNullOrEmpty(BackupFile))
			{
				try
				{
					try
					{
						AutomaticProgressDialog.Process(this, TR.Messages["DatabaseRestore", "Database Restore"], TR.Messages["DatabaseRestoreText", "Restoring database from Backup file"], 1000, delegate
						{
							Program.DatabaseManager.RestoreFrom(BackupFile, Program.Paths.CustomThumbnailPath);
						}, AutomaticProgressDialogOptions.None);
					}
					catch (Exception)
					{
						MessageBox.Show(this, TR.Messages["DatabaseRestoreError", "There was an error restoring the Database Backup"], TR.Messages["Attention"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					BackupFile = null;
					NeedsRestart = true;
				}
				catch
				{
				}
			}
			List<ComicLibraryServerConfig> list = (from p in tabShares.TabPages.OfType<Control>()
												   select p.Controls[0] as ServerEditControl into se
												   select se.Config).ToList();
			if (!list.SequenceEqual(Program.Settings.Shares, new ComicLibraryServerConfig.EqualityComparer()))
			{
				NeedsRestart = true;
				Program.Settings.Shares.Clear();
				Program.Settings.Shares.AddRange(list);
			}
			string text = txPublicServerAddress.Text.Trim();
			if (text != Program.Settings.ExternalServerAddress)
			{
				NeedsRestart = true;
				Program.Settings.ExternalServerAddress = text;
			}
			if (txPrivateListingPassword.Password != Program.Settings.PrivateListingPassword)
			{
				NeedsRestart = true;
				Program.Settings.PrivateListingPassword = txPrivateListingPassword.Password;
			}
			Program.Settings.ExtraWifiDeviceAddresses = txWifiAddresses.Text;
			Program.RefreshAllWindows();
			Program.ForAllForms(delegate (Form f)
			{
				f.FindServices<ISettingsChanged>().ForEach(delegate (ISettingsChanged s)
				{
					s.SettingsChanged();
				});
			});
		}

		private void SetScanButtonText()
		{
			btScan.Text = (Program.Scanner.IsScanning ? LocalizeUtility.GetText(this, "Stop", "Stop") : LocalizeUtility.GetText(this, "Scan", "Scan"));
		}

		private void SetSettings()
		{
			FormUtility.FillPanelWithOptions(pageBehavior, Program.Settings, TR.Load("Settings"));
			chkLibraryGauges.Checked = Program.Settings.DisplayLibraryGauges;
			chkLibraryGaugesUnread.Checked = Program.Settings.LibraryGaugesFormat.IsSet(LibraryGauges.Unread);
			chkLibraryGaugesNew.Checked = Program.Settings.LibraryGaugesFormat.IsSet(LibraryGauges.New);
			chkLibraryGaugesTotal.Checked = Program.Settings.LibraryGaugesFormat.IsSet(LibraryGauges.Total);
			chkLibraryGaugesNumeric.Checked = Program.Settings.LibraryGaugesFormat.IsSet(LibraryGauges.Numeric);
			tbMouseWheel.Value = (int)(Program.Settings.MouseWheelSpeed * 10f);
			chkHighQualityDisplay.Checked = Program.Settings.PageImageDisplayOptions.IsSet(ImageDisplayOptions.HighQuality);
			chkAnamorphicScaling.Checked = Program.Settings.PageImageDisplayOptions.IsSet(ImageDisplayOptions.AnamorphicScaling);
			chkAutoRemoveMissing.Checked = Program.Settings.RemoveMissingFilesOnFullScan;
			chkDontAddRemovedFiles.Checked = Program.Settings.DontAddRemoveFiles;
			tbSaturation.Value = (int)(Program.Settings.GlobalColorAdjustment.Saturation * 100f);
			tbBrightness.Value = (int)(Program.Settings.GlobalColorAdjustment.Brightness * 100f);
			tbContrast.Value = (int)(Program.Settings.GlobalColorAdjustment.Contrast * 100f);
			tbGamma.Value = (int)(Program.Settings.GlobalColorAdjustment.Gamma * 100f);
			tbSharpening.Value = Program.Settings.GlobalColorAdjustment.Sharpen;
			chkAutoContrast.Checked = Program.Settings.GlobalColorAdjustment.Options.IsSet(BitmapAdjustmentOptions.AutoContrast);
			chkShowCurrentPageOverlay.Checked = Program.Settings.ShowCurrentPageOverlay;
			chkShowVisiblePartOverlay.Checked = Program.Settings.ShowVisiblePagePartOverlay;
			chkShowStatusOverlay.Checked = Program.Settings.ShowStatusOverlay;
			chkShowNavigationOverlay.Checked = Program.Settings.ShowNavigationOverlay;
			cbNavigationOverlayPosition.SelectedIndex = (Program.Settings.NavigationOverlayOnTop ? 1 : 0);
			chkShowPageNames.Checked = Program.Settings.CurrentPageShowsName;
			chkEnableHardware.Checked = Program.Settings.HardwareAcceleration;
			chkSmoothAutoScrolling.Checked = Program.Settings.SmoothScrolling;
			chkEnableDisplayChangeAnimation.Checked = Program.Settings.DisplayChangeAnimation;
			chkEnableSoftwareFiltering.Checked = Program.Settings.SoftwareFiltering;
			chkEnableHardwareFiltering.Checked = Program.Settings.HardwareFiltering;
			chkEnableInertialMouseScrolling.Checked = Program.Settings.FlowingMouseScrolling;
			chkEnableInternetCache.Checked = Program.Settings.InternetCacheEnabled;
			numInternetCacheSize.Value = numInternetCacheSize.Clamp(Program.Settings.InternetCacheSizeMB);
			chkEnableThumbnailCache.Checked = Program.Settings.ThumbCacheEnabled;
			numThumbnailCacheSize.Value = numThumbnailCacheSize.Clamp(Program.Settings.ThumbCacheSizeMB);
			chkEnablePageCache.Checked = Program.Settings.PageCacheEnabled;
			numPageCacheSize.Value = numPageCacheSize.Clamp(Program.Settings.PageCacheSizeMB);
			chkMemPageOptimized.Checked = Program.Settings.MemoryPageCacheOptimized;
			numMemPageCount.Value = numMemPageCount.Clamp(Program.Settings.MemoryPageCacheCount);
			chkMemThumbOptimized.Checked = Program.Settings.MemoryThumbCacheOptimized;
			numMemThumbSize.Value = numMemThumbSize.Clamp(Program.Settings.MemoryThumbCacheSizeMB);
			tbMaximumMemoryUsage.SetRange(2, 32);
			tbMaximumMemoryUsage.Value = Program.Settings.MaximumMemoryMB / 32;
			tbOverlayScaling.Value = Program.Settings.OverlayScaling;
			chkOverwriteAssociations.Checked = Program.Settings.OverwriteAssociations;
			chkUpdateComicFiles.Checked = Program.Settings.UpdateComicFiles;
			chkAutoUpdateComicFiles.Checked = Program.Settings.AutoUpdateComicsFiles;
			txCoverFilter.Text = Program.Settings.IgnoredCoverImages;
			txLibraries.Text = Program.Settings.ScriptingLibraries;
			chkDisableScripting.Checked = !Program.Settings.Scripting;
			chkHideSampleScripts.Checked = Program.Settings.HideSampleScripts;
			lbLanguages.SelectedIndex = 0;
			foreach (TRInfo item in lbLanguages.Items)
			{
				if (item.CultureName == Program.Settings.CultureName)
				{
					lbLanguages.SelectedItem = item;
					break;
				}
			}
			txPublicServerAddress.Text = Program.Settings.ExternalServerAddress;
			txPrivateListingPassword.Password = Program.Settings.PrivateListingPassword;
			chkLookForShared.Checked = Program.Settings.LookForShared;
			chkAutoConnectShares.Checked = Program.Settings.AutoConnectShares;
			foreach (ComicLibraryServerConfig share in Program.Settings.Shares)
			{
				AddSharePage(share);
			}
			txWifiAddresses.Text = Program.Settings.ExtraWifiDeviceAddresses;
		}

		private void AddSharePage(ComicLibraryServerConfig cfg)
		{
			TabPage tab = new TabPage(cfg.Name)
			{
				UseVisualStyleBackColor = true
			};
			ServerEditControl sc = new ServerEditControl
			{
				Dock = DockStyle.Fill,
				Config = cfg,
				BackColor = Color.Transparent
			};
			sc.ShareNameChanged += delegate
			{
				tab.Text = sc.ShareName;
			};
			tab.Controls.Add(sc);
			tabShares.TabPages.Add(tab);
		}

		private void RemoveSharePage(TabPage tab)
		{
			Control control = tab.Controls[0];
			tab.Controls.Remove(control);
			control.Dispose();
			tabShares.TabPages.Remove(tab);
			tab.Dispose();
		}

		private void CopyWatchFoldersToDatabase()
		{
			Program.Database.WatchFolders.Clear();
			for (int i = 0; i < lbPaths.Items.Count; i++)
			{
				Program.Database.WatchFolders.Add(new WatchFolder(lbPaths.Items[i].ToString(), lbPaths.GetItemChecked(i)));
			}
		}

		private void RegisterFileTypes()
		{
			for (int i = 0; i < lbFormats.Items.Count; i++)
			{
				FileFormat fileFormat = (FileFormat)lbFormats.Items[i];
				if (lbFormats.GetItemChecked(i))
				{
					fileFormat.RegisterShell("cYo.ComicRack", "eComic", Program.Settings.OverwriteAssociations);
				}
				else
				{
					fileFormat.UnregisterShell("cYo.ComicRack");
				}
			}
		}

		private void SetTab(CheckBox cb)
		{
			if (blockSetTab)
			{
				return;
			}
			blockSetTab = true;
			try
			{
				foreach (CheckBox tabButton in tabButtons)
				{
					tabButton.Checked = tabButton == cb;
					Control control = tabButton.Tag as Control;
					if (control.Tag is Control)
					{
						control = control.Tag as Control;
					}
					control.Visible = tabButton.Checked;
				}
			}
			finally
			{
				blockSetTab = false;
			}
		}

		private void UpdateDiskCacheStatus(object sender, EventArgs e)
		{
			if (!this.BeginInvokeIfRequired(delegate
			{
				UpdateDiskCacheStatus(sender, e);
			}))
			{
				lblInternetCacheUsage.Text = string.Format("({0}/{1})", Program.InternetCache.Count, string.Format(new FileLengthFormat(), "{0}", new object[1]
				{
					Program.InternetCache.Size
				}));
				lblPageCacheUsage.Text = string.Format("({0}/{1})", Program.ImagePool.Pages.DiskCache.Count, string.Format(new FileLengthFormat(), "{0}", new object[1]
				{
					Program.ImagePool.Pages.DiskCache.Size
				}));
				lblThumbCacheUsage.Text = string.Format("({0}/{1})", Program.ImagePool.Thumbs.DiskCache.Count, string.Format(new FileLengthFormat(), "{0}", new object[1]
				{
					Program.ImagePool.Thumbs.DiskCache.Size
				}));
			}
		}

		private void UpdateMemoryCacheStatus()
		{
			if (!this.BeginInvokeIfRequired(UpdateMemoryCacheStatus))
			{
				lblPageMemCacheUsage.Text = string.Format("({0}/{1})", Program.ImagePool.Pages.MemoryCache.Count, string.Format(new FileLengthFormat(), "{0}", new object[1]
				{
					Program.ImagePool.Pages.MemoryCache.Size
				}));
				lblThumbMemCacheUsage.Text = string.Format("({0}/{1})", Program.ImagePool.Thumbs.MemoryCache.Count, string.Format(new FileLengthFormat(), "{0}", new object[1]
				{
					Program.ImagePool.Thumbs.MemoryCache.Size
				}));
			}
		}

		private void RefreshPackageList()
		{
			lvPackages.Items.Clear();
			foreach (PackageManager.Package item in (from p in Program.ScriptPackages.GetPackages()
													 orderby p.PackageType
													 select p).Reverse())
			{
				ListViewItem listViewItem = lvPackages.Items.Add(item.Name, item.Name, 0);
				if (item.Image != null)
				{
					packageImageList.Images.Add(item.Image);
					listViewItem.ImageIndex = packageImageList.Images.Count - 1;
				}
				listViewItem.Tag = item;
				if (!string.IsNullOrEmpty(item.Version))
				{
					listViewItem.Text = listViewItem.Text + " V" + item.Version;
				}
				listViewItem.SubItems.Add(item.Author);
				listViewItem.SubItems.Add(item.Description);
				switch (item.PackageType)
				{
					default:
						listViewItem.Group = lvPackages.Groups["packageGroupInstalled"];
						break;
					case PackageManager.PackageType.PendingInstall:
						listViewItem.Group = lvPackages.Groups["packageGroupInstall"];
						break;
					case PackageManager.PackageType.PendingRemove:
						listViewItem.Group = lvPackages.Groups["packageGroupRemove"];
						break;
				}
			}
		}

		private void btAssociateExtensions_Click(object sender, EventArgs e)
		{
			string str = "-rf \"" + (chkOverwriteAssociations.Checked ? "!" : string.Empty);
			for (int i = 0; i < lbFormats.Items.Count; i++)
			{
				FileFormat fileFormat = (FileFormat)lbFormats.Items[i];
				if (i != 0)
				{
					str += ",";
				}
				if (!lbFormats.GetItemChecked(i))
				{
					str += "-";
				}
				str += fileFormat.Name;
			}
			str += "\"";
			if (ProcessRunner.RunElevated(Application.ExecutablePath, str) == 0)
			{
				FillExtensionsList();
			}
		}

		private void btAddShare_Click(object sender, EventArgs e)
		{
			ComicLibraryServerConfig comicLibraryServerConfig = new ComicLibraryServerConfig();
			comicLibraryServerConfig.Name = $"{Environment.UserName}'s Library";
			if (tabShares.TabCount > 1)
			{
				comicLibraryServerConfig.Name += $" ({tabShares.TabCount + 1})";
			}
			AddSharePage(comicLibraryServerConfig);
			tabShares.SelectedIndex = tabShares.TabPages.Count - 1;
		}

		private void btRmoveShare_Click(object sender, EventArgs e)
		{
			TabPage selectedTab = tabShares.SelectedTab;
			if (selectedTab != null)
			{
				RemoveSharePage(selectedTab);
			}
		}

		public static bool Show(IWin32Window parent, KeyboardShortcuts commands, PluginEngine pe, string autoInstallPlugin = null)
		{
			using (PreferencesDialog preferencesDialog = new PreferencesDialog())
			{
				preferencesDialog.keyboardShortcutEditor.Shortcuts = commands;
				preferencesDialog.Plugins = pe;
				preferencesDialog.AutoInstallPlugin = (File.Exists(autoInstallPlugin) ? autoInstallPlugin : null);
				bool flag = preferencesDialog.ShowDialog(parent) == DialogResult.OK;
				if (flag)
				{
					preferencesDialog.Apply();
					if (preferencesDialog.NeedsRestart && QuestionDialog.Ask(parent, TR.Messages["RestartQuestion", "ComicRack needs to restart to complete the operation! Do you want to restart now?"], TR.Default["Restart", "Restart"]))
					{
						Program.MainForm.MenuRestart();
					}
				}
				else
				{
					Program.ScriptPackages.RemovePending();
				}
				return flag;
			}
		}

	}
}
