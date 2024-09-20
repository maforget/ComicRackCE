using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Reflection;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Provider;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class ExportComicsDialog : Form
	{
		private readonly EnumMenuUtility enumUtil;

		private ExportSettingCollection defaultPresets = new ExportSettingCollection();

		private ExportSettingCollection userPresets = new ExportSettingCollection();

		private static List<bool> expandedStates;

		public ExportSettingCollection DefaultPresets
		{
			get
			{
				return defaultPresets;
			}
			set
			{
				defaultPresets = value;
				BuildPresetsList();
			}
		}

		public ExportSettingCollection UserPresets
		{
			get
			{
				return userPresets;
			}
			set
			{
				userPresets = value;
				BuildPresetsList();
			}
		}

		public int FormatId
		{
			get
			{
				object selectedItem = cbComicFormat.SelectedItem;
				if (!(selectedItem is string))
				{
					return ((FileFormat)selectedItem).Id;
				}
				return 0;
			}
			set
			{
				foreach (object item in cbComicFormat.Items)
				{
					int num = ((!(item is string)) ? ((FileFormat)item).Id : 0);
					if (num == value)
					{
						cbComicFormat.SelectedItem = item;
						return;
					}
				}
				cbComicFormat.SelectedIndex = 0;
			}
		}

		public string SettingName
		{
			get;
			set;
		}

		public ExportSetting Setting
		{
			get
			{
				return new ExportSetting
				{
					Name = SettingName,
					Target = (ExportTarget)cbExport.SelectedIndex,
					TargetFolder = txFolder.Text,
					DeleteOriginal = chkDeleteOriginal.Checked,
					AddToLibrary = chkAddNewToLibrary.Checked,
					Overwrite = chkOverwrite.Checked,
					Combine = chkCombine.Checked,
					Naming = (ExportNaming)cbNamingTemplate.SelectedIndex,
					CustomName = txCustomName.Text.Trim(),
					CustomNamingStart = (int)txCustomStartIndex.Value,
					FormatId = FormatId,
					ComicCompression = (ExportCompression)cbCompression.SelectedIndex,
					EmbedComicInfo = chkEmbedComicInfo.Checked,
					RemovePageFilter = (ComicPageType)enumUtil.Value,
					IncludePages = txIncludePages.Text,
					PageType = (StoragePageType)cbPageFormat.SelectedIndex,
					PageCompression = tbQuality.Value,
					PageResize = (StoragePageResize)cbPageResize.SelectedIndex,
					PageWidth = (int)txWidth.Value,
					PageHeight = (int)txHeight.Value,
					DontEnlarge = chkDontEnlarge.Checked,
					DoublePages = (DoublePageHandling)cbDoublePages.SelectedIndex,
					IgnoreErrorPages = chkIgnoreErrorPages.Checked,
					KeepOriginalImageNames = chkKeepOriginalNames.Checked,
					ImageProcessingSource = (ExportImageProcessingSource)cbImageProcessingSource.SelectedIndex,
					ImageProcessing = new BitmapAdjustment((float)tbSaturation.Value / 100f, (float)tbBrightness.Value / 100f, (float)tbContrast.Value / 100f, (float)tbGamma.Value / 100f, Color.White, chkAutoContrast.Checked ? BitmapAdjustmentOptions.AutoContrast : BitmapAdjustmentOptions.None, tbSharpening.Value)
				};
			}
			set
			{
				SettingName = value.Name;
				cbExport.SelectedIndex = (int)value.Target;
				txFolder.Text = (string.IsNullOrEmpty(value.TargetFolder) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop) : value.TargetFolder);
				chkDeleteOriginal.Checked = value.DeleteOriginal;
				chkAddNewToLibrary.Checked = value.AddToLibrary;
				chkOverwrite.Checked = value.Overwrite;
				chkCombine.Checked = value.Combine;
				cbNamingTemplate.SelectedIndex = (int)value.Naming;
				txCustomName.Text = value.CustomName;
				txCustomStartIndex.Value = value.CustomNamingStart;
				FormatId = value.FormatId;
				cbCompression.SelectedIndex = (int)value.ComicCompression;
				chkEmbedComicInfo.Checked = value.EmbedComicInfo;
				enumUtil.Value = (int)value.RemovePageFilter;
				txIncludePages.Text = value.IncludePages;
				cbPageFormat.SelectedIndex = (int)value.PageType < cbPageFormat.Items.Count ? (int)value.PageType : 0;
				tbQuality.Value = value.PageCompression;
				cbPageResize.SelectedIndex = (int)value.PageResize;
				txWidth.Value = value.PageWidth;
				txHeight.Value = value.PageHeight;
				chkDontEnlarge.Checked = value.DontEnlarge;
				cbDoublePages.SelectedIndex = (int)value.DoublePages;
				chkIgnoreErrorPages.Checked = value.IgnoreErrorPages;
				chkKeepOriginalNames.Checked = value.KeepOriginalImageNames;
				cbImageProcessingSource.SelectedIndex = (int)value.ImageProcessingSource;
				tbSaturation.Value = (int)(value.ImageProcessing.Saturation * 100f);
				tbBrightness.Value = (int)(value.ImageProcessing.Brightness * 100f);
				tbContrast.Value = (int)(value.ImageProcessing.Contrast * 100f);
				tbGamma.Value = (int)(value.ImageProcessing.Gamma * 100f);
				tbSharpening.Value = value.ImageProcessing.Sharpen;
				chkAutoContrast.Checked = (value.ImageProcessing.Options & BitmapAdjustmentOptions.AutoContrast) != 0;
			}
		}

		public ExportComicsDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			if (Environment.Is64BitProcess) this.cbPageFormat.Items.AddRange(new object[] { "HEIF", "AVIF" });
			LocalizeUtility.Localize(this, null);
			foreach (ComboBox control in this.GetControls<ComboBox>())
			{
				LocalizeUtility.Localize(TR.Load(base.Name), control);
			}
			this.RestorePosition();
			this.RestorePanelStates();
			FormUtility.RegisterPanelToTabToggle(exportSettings, PropertyCaller.CreateFlagsValueStore(Program.Settings, "TabLayouts", TabLayouts.Export));
			foreach (FileFormat item in from f in Providers.Writers.GetSourceFormats()
				orderby f
				select f)
			{
				cbComicFormat.Items.Add(item);
			}
			enumUtil = new EnumMenuUtility(contextRemovePageFilter, typeof(ComicPageType), flagsMode: true, null, Keys.None);
			enumUtil.ValueChanged += enumUtil_ValueChanged;
			new NiceTreeSkin(tvPresets);
			IdleProcess.Idle += OnIdle;
		}

		private void OnIdle(object sender, EventArgs e)
		{
			ExportSetting setting = Setting;
			btChooseFolder.Enabled = setting.Target == ExportTarget.NewFolder;
			CheckBox checkBox = chkOverwrite;
			CheckBox checkBox2 = chkDeleteOriginal;
			bool flag2 = (chkAddNewToLibrary.Enabled = setting.Target != ExportTarget.ReplaceSource);
			bool enabled = (checkBox2.Enabled = flag2);
			checkBox.Enabled = enabled;
			txCustomStartIndex.Enabled = setting.Naming == ExportNaming.Custom;
			txCustomName.Enabled = setting.Naming == ExportNaming.Custom || setting.Naming == ExportNaming.Caption;
			tbQuality.Enabled = setting.PageType == StoragePageType.Jpeg || setting.PageType == StoragePageType.Webp || setting.PageType == StoragePageType.Heif || setting.PageType == StoragePageType.Avif;
			txWidth.Enabled = setting.PageResize != StoragePageResize.Height && setting.PageResize != StoragePageResize.Original;
			txHeight.Enabled = setting.PageResize != StoragePageResize.Width && setting.PageResize != StoragePageResize.Original;
			chkDontEnlarge.Enabled = setting.PageResize != StoragePageResize.Original;
			btRemovePreset.Enabled = tvPresets.SelectedNode != null && tvPresets.SelectedNode.Parent != null && (bool)tvPresets.SelectedNode.Parent.Tag;
			grpCustomProcessing.Enabled = setting.ImageProcessingSource == ExportImageProcessingSource.Custom;
		}

		private void tbQuality_ValueChanged(object sender, EventArgs e)
		{
			toolTip.SetToolTip(tbQuality, tbQuality.Value.ToString());
		}

		private void btRemovePreset_Click(object sender, EventArgs e)
		{
			OnIdle(this, EventArgs.Empty);
			if (btRemovePreset.Enabled)
			{
				userPresets.Remove(tvPresets.SelectedNode.Tag as ExportSetting);
				tvPresets.SelectedNode.Remove();
			}
		}

		private void btAddPreset_Click(object sender, EventArgs e)
		{
			string itemValue = (string.IsNullOrEmpty(Setting.Name) ? ExportSetting.DefaultName : Setting.Name);
			string name = SelectItemDialog.GetName(this, TR.Load(base.Name)["AddConvertPreset", "Add Export Preset"], itemValue);
			if (!string.IsNullOrEmpty(name))
			{
				SettingName = name;
				UserPresets.RemoveAll((ExportSetting x) => x.Name == name);
				UserPresets.Add(Setting);
				BuildPresetsList();
			}
		}

		private void enumUtil_ValueChanged(object sender, EventArgs e)
		{
			txRemovedPages.Text = enumUtil.Text;
		}

		private void btRemovePageFilter_Click(object sender, EventArgs e)
		{
			contextRemovePageFilter.Show(btRemovePageFilter, 0, btRemovePageFilter.Height);
		}

		private void tvPresets_AfterSelect(object sender, TreeViewEventArgs e)
		{
			ExportSetting exportSetting = tvPresets.SelectedNode.Tag as ExportSetting;
			if (exportSetting != null)
			{
				Setting = exportSetting;
			}
		}

		private void btChooseFolder_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.Description = TR.Load(base.Name)["SelectExportFolder", "Please select the Export Folder"];
				folderBrowserDialog.SelectedPath = txFolder.Text;
				folderBrowserDialog.ShowNewFolderButton = true;
				if (folderBrowserDialog.ShowDialog(this) != DialogResult.Cancel)
				{
					txFolder.Text = folderBrowserDialog.SelectedPath;
				}
			}
		}

		private void btResetColors_Click(object sender, EventArgs e)
		{
			TrackBarLite trackBarLite = tbSaturation;
			TrackBarLite trackBarLite2 = tbBrightness;
			TrackBarLite trackBarLite3 = tbContrast;
			int num2 = (tbGamma.Value = 0);
			int num4 = (trackBarLite3.Value = num2);
			int num7 = (trackBarLite.Value = (trackBarLite2.Value = num4));
			tbSharpening.Value = 0;
		}

		private void AdjustmentSliderChanged(object sender, EventArgs e)
		{
			TrackBarLite trackBarLite = (TrackBarLite)sender;
			toolTip.SetToolTip(trackBarLite, string.Format("{1}{0}%", trackBarLite.Value, (trackBarLite.Value > 0) ? "+" : string.Empty));
		}

		private void cbNamingTemplate_SelectedIndexChanged(object sender, EventArgs e)
		{
			int selectedIndex = cbNamingTemplate.SelectedIndex;
			if (selectedIndex != 1)
			{
				txCustomName.Text = string.Empty;
			}
			else
			{
				txCustomName.Text = EngineConfiguration.Default.ComicExportFileNameFormat;
			}
		}

		private void BuildPresetsList()
		{
			tvPresets.Nodes.Clear();
			AddNodes(tvPresets, canDelete: false, TR.Load(base.Name)["ComicRackExportPresets", "ComicRack Presets"], DefaultPresets);
			AddNodes(tvPresets, canDelete: true, TR.Load(base.Name)["UserExportPresets", "User Presets"], UserPresets);
			tvPresets.ExpandAll();
		}

		private static void AddNodes(TreeView tvPresets, bool canDelete, string groupName, ICollection<ExportSetting> settings)
		{
			if (settings.Count == 0)
			{
				return;
			}
			TreeNode treeNode = tvPresets.Nodes.Add(groupName);
			treeNode.Tag = canDelete;
			foreach (ExportSetting setting in settings)
			{
				TreeNode treeNode2 = treeNode.Nodes.Add(setting.Name);
				treeNode2.Tag = setting;
			}
		}

		public static ExportSetting Show(IWin32Window parent, ExportSettingCollection defaultPresets, ExportSettingCollection userPresets, ExportSetting setting)
		{
			using (ExportComicsDialog exportComicsDialog = new ExportComicsDialog())
			{
				exportComicsDialog.DefaultPresets = defaultPresets;
				exportComicsDialog.UserPresets = userPresets;
				exportComicsDialog.Setting = setting;
				if (expandedStates != null)
				{
					int i = 0;
					exportComicsDialog.ForEachControl(delegate (CollapsibleGroupBox x)
					{
						x.Collapsed = expandedStates[i++];
					});
				}
				ExportSetting result = ((exportComicsDialog.ShowDialog(parent) == DialogResult.OK) ? exportComicsDialog.Setting : null);
				expandedStates = new List<bool>();
				exportComicsDialog.ForEachControl(delegate (CollapsibleGroupBox x)
				{
					expandedStates.Add(x.Collapsed);
				});
				return result;
			}
		}

	}
}
