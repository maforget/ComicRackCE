using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class PreferencesDialog
	{
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.Windows.Forms.ListViewGroup listViewGroup = new System.Windows.Forms.ListViewGroup("Installed", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("To be removed (requires restart)", System.Windows.Forms.HorizontalAlignment.Left);
			System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("To be installed (requires restart)", System.Windows.Forms.HorizontalAlignment.Left);
			btOK = new System.Windows.Forms.Button();
			btCancel = new System.Windows.Forms.Button();
			imageList = new System.Windows.Forms.ImageList(components);
			btApply = new System.Windows.Forms.Button();
			toolTip = new System.Windows.Forms.ToolTip(components);
			pageBehavior = new System.Windows.Forms.Panel();
			pageReader = new System.Windows.Forms.Panel();
			groupHardwareAcceleration = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkEnableHardwareFiltering = new System.Windows.Forms.CheckBox();
			chkEnableSoftwareFiltering = new System.Windows.Forms.CheckBox();
			chkEnableHardware = new System.Windows.Forms.CheckBox();
			chkEnableDisplayChangeAnimation = new System.Windows.Forms.CheckBox();
			grpMouse = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkSmoothAutoScrolling = new System.Windows.Forms.CheckBox();
			lblFast = new System.Windows.Forms.Label();
			lblMouseWheel = new System.Windows.Forms.Label();
			chkEnableInertialMouseScrolling = new System.Windows.Forms.CheckBox();
			lblSlow = new System.Windows.Forms.Label();
			tbMouseWheel = new cYo.Common.Windows.Forms.TrackBarLite();
			groupOverlays = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			panelReaderOverlays = new System.Windows.Forms.Panel();
			labelVisiblePartOverlay = new System.Windows.Forms.Label();
			labelNavigationOverlay = new System.Windows.Forms.Label();
			labelStatusOverlay = new System.Windows.Forms.Label();
			labelPageOverlay = new System.Windows.Forms.Label();
			cbNavigationOverlayPosition = new System.Windows.Forms.ComboBox();
			labelNavigationOverlayPosition = new System.Windows.Forms.Label();
			chkShowPageNames = new System.Windows.Forms.CheckBox();
			tbOverlayScaling = new cYo.Common.Windows.Forms.TrackBarLite();
			chkShowCurrentPageOverlay = new System.Windows.Forms.CheckBox();
			chkShowStatusOverlay = new System.Windows.Forms.CheckBox();
			chkShowVisiblePartOverlay = new System.Windows.Forms.CheckBox();
			chkShowNavigationOverlay = new System.Windows.Forms.CheckBox();
			labelOverlaySize = new System.Windows.Forms.Label();
			grpKeyboard = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			btExportKeyboard = new System.Windows.Forms.Button();
			btImportKeyboard = new cYo.Common.Windows.Forms.SplitButton();
			cmKeyboardLayout = new System.Windows.Forms.ContextMenuStrip(components);
			miDefaultKeyboardLayout = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			keyboardShortcutEditor = new cYo.Common.Windows.Forms.KeyboardShortcutEditor();
			grpDisplay = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			tbGamma = new cYo.Common.Windows.Forms.TrackBarLite();
			labelGamma = new System.Windows.Forms.Label();
			chkAnamorphicScaling = new System.Windows.Forms.CheckBox();
			chkHighQualityDisplay = new System.Windows.Forms.CheckBox();
			labelSharpening = new System.Windows.Forms.Label();
			tbSharpening = new cYo.Common.Windows.Forms.TrackBarLite();
			btResetColor = new System.Windows.Forms.Button();
			chkAutoContrast = new System.Windows.Forms.CheckBox();
			labelSaturation = new System.Windows.Forms.Label();
			tbSaturation = new cYo.Common.Windows.Forms.TrackBarLite();
			labelBrightness = new System.Windows.Forms.Label();
			tbBrightness = new cYo.Common.Windows.Forms.TrackBarLite();
			tbContrast = new cYo.Common.Windows.Forms.TrackBarLite();
			labelContrast = new System.Windows.Forms.Label();
			pageAdvanced = new System.Windows.Forms.Panel();
			grpWirelessSetup = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			btTestWifi = new System.Windows.Forms.Button();
			lblWifiStatus = new System.Windows.Forms.Label();
			lblWifiAddresses = new System.Windows.Forms.Label();
			txWifiAddresses = new System.Windows.Forms.TextBox();
			grpIntegration = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			btAssociateExtensions = new System.Windows.Forms.Button();
			labelCheckedFormats = new System.Windows.Forms.Label();
			chkOverwriteAssociations = new System.Windows.Forms.CheckBox();
			lbFormats = new System.Windows.Forms.CheckedListBox();
			groupMessagesAndSocial = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			btResetMessages = new System.Windows.Forms.Button();
			labelReshowHidden = new System.Windows.Forms.Label();
			groupMemory = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			grpMaximumMemoryUsage = new System.Windows.Forms.GroupBox();
			lblMaximumMemoryUsageValue = new System.Windows.Forms.Label();
			tbMaximumMemoryUsage = new cYo.Common.Windows.Forms.TrackBarLite();
			lblMaximumMemoryUsage = new System.Windows.Forms.Label();
			grpMemoryCache = new System.Windows.Forms.GroupBox();
			lblPageMemCacheUsage = new System.Windows.Forms.Label();
			labelMemThumbSize = new System.Windows.Forms.Label();
			lblThumbMemCacheUsage = new System.Windows.Forms.Label();
			numMemPageCount = new System.Windows.Forms.NumericUpDown();
			labelMemPageCount = new System.Windows.Forms.Label();
			chkMemPageOptimized = new System.Windows.Forms.CheckBox();
			chkMemThumbOptimized = new System.Windows.Forms.CheckBox();
			numMemThumbSize = new System.Windows.Forms.NumericUpDown();
			grpDiskCache = new System.Windows.Forms.GroupBox();
			chkEnableInternetCache = new System.Windows.Forms.CheckBox();
			lblInternetCacheUsage = new System.Windows.Forms.Label();
			btClearPageCache = new System.Windows.Forms.Button();
			numPageCacheSize = new System.Windows.Forms.NumericUpDown();
			numInternetCacheSize = new System.Windows.Forms.NumericUpDown();
			btClearThumbnailCache = new System.Windows.Forms.Button();
			btClearInternetCache = new System.Windows.Forms.Button();
			chkEnablePageCache = new System.Windows.Forms.CheckBox();
			lblPageCacheUsage = new System.Windows.Forms.Label();
			numThumbnailCacheSize = new System.Windows.Forms.NumericUpDown();
			chkEnableThumbnailCache = new System.Windows.Forms.CheckBox();
			lblThumbCacheUsage = new System.Windows.Forms.Label();
			grpDatabaseBackup = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			btRestoreDatabase = new System.Windows.Forms.Button();
			btBackupDatabase = new System.Windows.Forms.Button();
			groupOtherComics = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkUpdateComicFiles = new System.Windows.Forms.CheckBox();
			labelExcludeCover = new System.Windows.Forms.Label();
			chkAutoUpdateComicFiles = new System.Windows.Forms.CheckBox();
			txCoverFilter = new System.Windows.Forms.TextBox();
			grpLanguages = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			btTranslate = new System.Windows.Forms.Button();
			labelLanguage = new System.Windows.Forms.Label();
			lbLanguages = new System.Windows.Forms.ListBox();
			pageLibrary = new System.Windows.Forms.Panel();
			grpServerSettings = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			txPrivateListingPassword = new cYo.Common.Windows.Forms.PasswordTextBox();
			labelPrivateListPassword = new System.Windows.Forms.Label();
			labelPublicServerAddress = new System.Windows.Forms.Label();
			txPublicServerAddress = new System.Windows.Forms.TextBox();
			grpSharing = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkAutoConnectShares = new System.Windows.Forms.CheckBox();
			btRemoveShare = new System.Windows.Forms.Button();
			btAddShare = new System.Windows.Forms.Button();
			tabShares = new System.Windows.Forms.TabControl();
			chkLookForShared = new System.Windows.Forms.CheckBox();
			groupLibraryDisplay = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkLibraryGaugesTotal = new System.Windows.Forms.CheckBox();
			chkLibraryGaugesUnread = new System.Windows.Forms.CheckBox();
			chkLibraryGaugesNumeric = new System.Windows.Forms.CheckBox();
			chkLibraryGaugesNew = new System.Windows.Forms.CheckBox();
			chkLibraryGauges = new System.Windows.Forms.CheckBox();
			grpScanning = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkDontAddRemovedFiles = new System.Windows.Forms.CheckBox();
			chkAutoRemoveMissing = new System.Windows.Forms.CheckBox();
			lblScan = new System.Windows.Forms.Label();
			btScan = new System.Windows.Forms.Button();
			groupComicFolders = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			btOpenFolder = new System.Windows.Forms.Button();
			btChangeFolder = new System.Windows.Forms.Button();
			lbPaths = new cYo.Common.Windows.Forms.CheckedListBoxEx();
			labelWatchedFolders = new System.Windows.Forms.Label();
			btRemoveFolder = new System.Windows.Forms.Button();
			btAddFolder = new System.Windows.Forms.Button();
			memCacheUpate = new System.Windows.Forms.Timer(components);
			pageScripts = new System.Windows.Forms.Panel();
			grpScriptSettings = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			btAddLibraryFolder = new System.Windows.Forms.Button();
			chkDisableScripting = new System.Windows.Forms.CheckBox();
			labelScriptPaths = new System.Windows.Forms.Label();
			txLibraries = new System.Windows.Forms.TextBox();
			grpScripts = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			chkHideSampleScripts = new System.Windows.Forms.CheckBox();
			btConfigScript = new System.Windows.Forms.Button();
			lvScripts = new System.Windows.Forms.ListView();
			chScriptName = new System.Windows.Forms.ColumnHeader();
			chScriptPackage = new System.Windows.Forms.ColumnHeader();
			grpPackages = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
			btRemovePackage = new System.Windows.Forms.Button();
			btInstallPackage = new System.Windows.Forms.Button();
			lvPackages = new System.Windows.Forms.ListView();
			chPackageName = new System.Windows.Forms.ColumnHeader();
			chPackageAuthor = new System.Windows.Forms.ColumnHeader();
			chPackageDescription = new System.Windows.Forms.ColumnHeader();
			packageImageList = new System.Windows.Forms.ImageList(components);
			tabReader = new System.Windows.Forms.CheckBox();
			tabLibraries = new System.Windows.Forms.CheckBox();
			tabBehavior = new System.Windows.Forms.CheckBox();
			tabScripts = new System.Windows.Forms.CheckBox();
			tabAdvanced = new System.Windows.Forms.CheckBox();
			btResetTwitter = new System.Windows.Forms.Button();
			labelResetTwitter = new System.Windows.Forms.Label();
			pageReader.SuspendLayout();
			groupHardwareAcceleration.SuspendLayout();
			grpMouse.SuspendLayout();
			groupOverlays.SuspendLayout();
			panelReaderOverlays.SuspendLayout();
			grpKeyboard.SuspendLayout();
			cmKeyboardLayout.SuspendLayout();
			grpDisplay.SuspendLayout();
			pageAdvanced.SuspendLayout();
			grpWirelessSetup.SuspendLayout();
			grpIntegration.SuspendLayout();
			groupMessagesAndSocial.SuspendLayout();
			groupMemory.SuspendLayout();
			grpMaximumMemoryUsage.SuspendLayout();
			grpMemoryCache.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)numMemPageCount).BeginInit();
			((System.ComponentModel.ISupportInitialize)numMemThumbSize).BeginInit();
			grpDiskCache.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)numPageCacheSize).BeginInit();
			((System.ComponentModel.ISupportInitialize)numInternetCacheSize).BeginInit();
			((System.ComponentModel.ISupportInitialize)numThumbnailCacheSize).BeginInit();
			grpDatabaseBackup.SuspendLayout();
			groupOtherComics.SuspendLayout();
			grpLanguages.SuspendLayout();
			pageLibrary.SuspendLayout();
			grpServerSettings.SuspendLayout();
			grpSharing.SuspendLayout();
			groupLibraryDisplay.SuspendLayout();
			grpScanning.SuspendLayout();
			groupComicFolders.SuspendLayout();
			pageScripts.SuspendLayout();
			grpScriptSettings.SuspendLayout();
			grpScripts.SuspendLayout();
			grpPackages.SuspendLayout();
			SuspendLayout();
			btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btOK.Location = new System.Drawing.Point(351, 422);
			btOK.Name = "btOK";
			btOK.Size = new System.Drawing.Size(80, 24);
			btOK.TabIndex = 1;
			btOK.Text = "&OK";
			btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(437, 422);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(80, 24);
			btCancel.TabIndex = 2;
			btCancel.Text = "&Cancel";
			imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			imageList.ImageSize = new System.Drawing.Size(16, 16);
			imageList.TransparentColor = System.Drawing.Color.Transparent;
			btApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btApply.Location = new System.Drawing.Point(523, 422);
			btApply.Name = "btApply";
			btApply.Size = new System.Drawing.Size(80, 24);
			btApply.TabIndex = 3;
			btApply.Text = "&Apply";
			btApply.Click += new System.EventHandler(btApply_Click);
			pageBehavior.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			pageBehavior.AutoScroll = true;
			pageBehavior.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			pageBehavior.Location = new System.Drawing.Point(84, 8);
			pageBehavior.Name = "pageBehavior";
			pageBehavior.Size = new System.Drawing.Size(517, 408);
			pageBehavior.TabIndex = 6;
			pageReader.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			pageReader.AutoScroll = true;
			pageReader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			pageReader.Controls.Add(groupHardwareAcceleration);
			pageReader.Controls.Add(grpMouse);
			pageReader.Controls.Add(groupOverlays);
			pageReader.Controls.Add(grpKeyboard);
			pageReader.Controls.Add(grpDisplay);
			pageReader.Location = new System.Drawing.Point(84, 8);
			pageReader.Name = "pageReader";
			pageReader.Size = new System.Drawing.Size(517, 408);
			pageReader.TabIndex = 8;
			groupHardwareAcceleration.Controls.Add(chkEnableHardwareFiltering);
			groupHardwareAcceleration.Controls.Add(chkEnableSoftwareFiltering);
			groupHardwareAcceleration.Controls.Add(chkEnableHardware);
			groupHardwareAcceleration.Controls.Add(chkEnableDisplayChangeAnimation);
			groupHardwareAcceleration.Dock = System.Windows.Forms.DockStyle.Top;
			groupHardwareAcceleration.Location = new System.Drawing.Point(0, 1180);
			groupHardwareAcceleration.Name = "groupHardwareAcceleration";
			groupHardwareAcceleration.Size = new System.Drawing.Size(498, 137);
			groupHardwareAcceleration.TabIndex = 3;
			groupHardwareAcceleration.Text = "Hardware Acceleration";
			chkEnableHardwareFiltering.AutoSize = true;
			chkEnableHardwareFiltering.Location = new System.Drawing.Point(33, 70);
			chkEnableHardwareFiltering.Name = "chkEnableHardwareFiltering";
			chkEnableHardwareFiltering.Size = new System.Drawing.Size(138, 17);
			chkEnableHardwareFiltering.TabIndex = 1;
			chkEnableHardwareFiltering.Text = "Enable Hardware Filters";
			chkEnableHardwareFiltering.UseVisualStyleBackColor = true;
			chkEnableSoftwareFiltering.AutoSize = true;
			chkEnableSoftwareFiltering.Location = new System.Drawing.Point(33, 88);
			chkEnableSoftwareFiltering.Name = "chkEnableSoftwareFiltering";
			chkEnableSoftwareFiltering.Size = new System.Drawing.Size(134, 17);
			chkEnableSoftwareFiltering.TabIndex = 2;
			chkEnableSoftwareFiltering.Text = "Enable Software Filters";
			chkEnableSoftwareFiltering.UseVisualStyleBackColor = true;
			chkEnableHardware.AutoSize = true;
			chkEnableHardware.Location = new System.Drawing.Point(12, 38);
			chkEnableHardware.Name = "chkEnableHardware";
			chkEnableHardware.Size = new System.Drawing.Size(170, 17);
			chkEnableHardware.TabIndex = 0;
			chkEnableHardware.Text = "Enable Hardware Acceleration";
			chkEnableHardware.UseVisualStyleBackColor = true;
			chkEnableDisplayChangeAnimation.AutoSize = true;
			chkEnableDisplayChangeAnimation.Location = new System.Drawing.Point(33, 108);
			chkEnableDisplayChangeAnimation.Name = "chkEnableDisplayChangeAnimation";
			chkEnableDisplayChangeAnimation.Size = new System.Drawing.Size(229, 17);
			chkEnableDisplayChangeAnimation.TabIndex = 3;
			chkEnableDisplayChangeAnimation.Text = "Enable Animation of Page Display changes";
			chkEnableDisplayChangeAnimation.UseVisualStyleBackColor = true;
			grpMouse.Controls.Add(chkSmoothAutoScrolling);
			grpMouse.Controls.Add(lblFast);
			grpMouse.Controls.Add(lblMouseWheel);
			grpMouse.Controls.Add(chkEnableInertialMouseScrolling);
			grpMouse.Controls.Add(lblSlow);
			grpMouse.Controls.Add(tbMouseWheel);
			grpMouse.Dock = System.Windows.Forms.DockStyle.Top;
			grpMouse.Location = new System.Drawing.Point(0, 1046);
			grpMouse.Name = "grpMouse";
			grpMouse.Size = new System.Drawing.Size(498, 134);
			grpMouse.TabIndex = 5;
			grpMouse.Text = "Mouse & Scrolling";
			chkSmoothAutoScrolling.AutoSize = true;
			chkSmoothAutoScrolling.Location = new System.Drawing.Point(9, 39);
			chkSmoothAutoScrolling.Name = "chkSmoothAutoScrolling";
			chkSmoothAutoScrolling.Size = new System.Drawing.Size(130, 17);
			chkSmoothAutoScrolling.TabIndex = 0;
			chkSmoothAutoScrolling.Text = "Smooth Auto Scrolling";
			chkSmoothAutoScrolling.UseVisualStyleBackColor = true;
			lblFast.Location = new System.Drawing.Point(426, 96);
			lblFast.Name = "lblFast";
			lblFast.Size = new System.Drawing.Size(56, 19);
			lblFast.TabIndex = 4;
			lblFast.Text = "fast";
			lblMouseWheel.AutoSize = true;
			lblMouseWheel.Location = new System.Drawing.Point(9, 97);
			lblMouseWheel.Name = "lblMouseWheel";
			lblMouseWheel.Size = new System.Drawing.Size(117, 13);
			lblMouseWheel.TabIndex = 0;
			lblMouseWheel.Text = "Mouse Wheel scrolling:";
			chkEnableInertialMouseScrolling.AutoSize = true;
			chkEnableInertialMouseScrolling.Location = new System.Drawing.Point(9, 62);
			chkEnableInertialMouseScrolling.Name = "chkEnableInertialMouseScrolling";
			chkEnableInertialMouseScrolling.Size = new System.Drawing.Size(169, 17);
			chkEnableInertialMouseScrolling.TabIndex = 1;
			chkEnableInertialMouseScrolling.Text = "Enable Inertial Mouse scrolling";
			chkEnableInertialMouseScrolling.UseVisualStyleBackColor = true;
			lblSlow.Location = new System.Drawing.Point(186, 97);
			lblSlow.Name = "lblSlow";
			lblSlow.Size = new System.Drawing.Size(55, 19);
			lblSlow.TabIndex = 2;
			lblSlow.Text = "slow";
			lblSlow.TextAlign = System.Drawing.ContentAlignment.TopRight;
			tbMouseWheel.Location = new System.Drawing.Point(247, 97);
			tbMouseWheel.Maximum = 50;
			tbMouseWheel.Minimum = 5;
			tbMouseWheel.Name = "tbMouseWheel";
			tbMouseWheel.Size = new System.Drawing.Size(173, 16);
			tbMouseWheel.TabIndex = 3;
			tbMouseWheel.ThumbSize = new System.Drawing.Size(8, 16);
			tbMouseWheel.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbMouseWheel.Value = 5;
			groupOverlays.Controls.Add(panelReaderOverlays);
			groupOverlays.Controls.Add(cbNavigationOverlayPosition);
			groupOverlays.Controls.Add(labelNavigationOverlayPosition);
			groupOverlays.Controls.Add(chkShowPageNames);
			groupOverlays.Controls.Add(tbOverlayScaling);
			groupOverlays.Controls.Add(chkShowCurrentPageOverlay);
			groupOverlays.Controls.Add(chkShowStatusOverlay);
			groupOverlays.Controls.Add(chkShowVisiblePartOverlay);
			groupOverlays.Controls.Add(chkShowNavigationOverlay);
			groupOverlays.Controls.Add(labelOverlaySize);
			groupOverlays.Dock = System.Windows.Forms.DockStyle.Top;
			groupOverlays.Location = new System.Drawing.Point(0, 690);
			groupOverlays.Name = "groupOverlays";
			groupOverlays.Size = new System.Drawing.Size(498, 356);
			groupOverlays.TabIndex = 2;
			groupOverlays.Text = "Overlays";
			panelReaderOverlays.BackColor = System.Drawing.Color.WhiteSmoke;
			panelReaderOverlays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			panelReaderOverlays.Controls.Add(labelVisiblePartOverlay);
			panelReaderOverlays.Controls.Add(labelNavigationOverlay);
			panelReaderOverlays.Controls.Add(labelStatusOverlay);
			panelReaderOverlays.Controls.Add(labelPageOverlay);
			panelReaderOverlays.Location = new System.Drawing.Point(118, 39);
			panelReaderOverlays.Name = "panelReaderOverlays";
			panelReaderOverlays.Size = new System.Drawing.Size(258, 134);
			panelReaderOverlays.TabIndex = 8;
			labelVisiblePartOverlay.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			labelVisiblePartOverlay.BackColor = System.Drawing.Color.Gainsboro;
			labelVisiblePartOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			labelVisiblePartOverlay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			labelVisiblePartOverlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			labelVisiblePartOverlay.Location = new System.Drawing.Point(204, 75);
			labelVisiblePartOverlay.Name = "labelVisiblePartOverlay";
			labelVisiblePartOverlay.Size = new System.Drawing.Size(49, 51);
			labelVisiblePartOverlay.TabIndex = 3;
			labelVisiblePartOverlay.Text = "Visible Part";
			labelVisiblePartOverlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			labelVisiblePartOverlay.UseMnemonic = false;
			labelNavigationOverlay.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			labelNavigationOverlay.BackColor = System.Drawing.Color.Gainsboro;
			labelNavigationOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			labelNavigationOverlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			labelNavigationOverlay.Location = new System.Drawing.Point(55, 100);
			labelNavigationOverlay.Name = "labelNavigationOverlay";
			labelNavigationOverlay.Size = new System.Drawing.Size(143, 26);
			labelNavigationOverlay.TabIndex = 2;
			labelNavigationOverlay.Text = "Navigation";
			labelNavigationOverlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			labelNavigationOverlay.UseMnemonic = false;
			labelStatusOverlay.BackColor = System.Drawing.Color.Gainsboro;
			labelStatusOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			labelStatusOverlay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			labelStatusOverlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			labelStatusOverlay.Location = new System.Drawing.Point(60, 49);
			labelStatusOverlay.Name = "labelStatusOverlay";
			labelStatusOverlay.Size = new System.Drawing.Size(134, 26);
			labelStatusOverlay.TabIndex = 1;
			labelStatusOverlay.Text = "Messages and Status";
			labelStatusOverlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			labelStatusOverlay.UseMnemonic = false;
			labelPageOverlay.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			labelPageOverlay.BackColor = System.Drawing.Color.Gainsboro;
			labelPageOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			labelPageOverlay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			labelPageOverlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			labelPageOverlay.Location = new System.Drawing.Point(204, 3);
			labelPageOverlay.Name = "labelPageOverlay";
			labelPageOverlay.Size = new System.Drawing.Size(49, 36);
			labelPageOverlay.TabIndex = 0;
			labelPageOverlay.Text = "Page";
			labelPageOverlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			labelPageOverlay.UseMnemonic = false;
			cbNavigationOverlayPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbNavigationOverlayPosition.FormattingEnabled = true;
			cbNavigationOverlayPosition.Items.AddRange(new object[2]
			{
				"at Bottom",
				"on Top"
			});
			cbNavigationOverlayPosition.Location = new System.Drawing.Point(84, 313);
			cbNavigationOverlayPosition.Name = "cbNavigationOverlayPosition";
			cbNavigationOverlayPosition.Size = new System.Drawing.Size(121, 21);
			cbNavigationOverlayPosition.TabIndex = 6;
			labelNavigationOverlayPosition.AutoSize = true;
			labelNavigationOverlayPosition.Location = new System.Drawing.Point(18, 316);
			labelNavigationOverlayPosition.Name = "labelNavigationOverlayPosition";
			labelNavigationOverlayPosition.Size = new System.Drawing.Size(61, 13);
			labelNavigationOverlayPosition.TabIndex = 5;
			labelNavigationOverlayPosition.Text = "Navigation:";
			chkShowPageNames.AutoSize = true;
			chkShowPageNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			chkShowPageNames.Location = new System.Drawing.Point(283, 193);
			chkShowPageNames.Name = "chkShowPageNames";
			chkShowPageNames.Size = new System.Drawing.Size(181, 17);
			chkShowPageNames.TabIndex = 4;
			chkShowPageNames.Text = "Current Page also displays Name";
			chkShowPageNames.UseVisualStyleBackColor = true;
			tbOverlayScaling.Location = new System.Drawing.Point(288, 316);
			tbOverlayScaling.Maximum = 150;
			tbOverlayScaling.Minimum = 40;
			tbOverlayScaling.Name = "tbOverlayScaling";
			tbOverlayScaling.Size = new System.Drawing.Size(184, 16);
			tbOverlayScaling.TabIndex = 8;
			tbOverlayScaling.ThumbSize = new System.Drawing.Size(8, 16);
			tbOverlayScaling.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbOverlayScaling.Value = 50;
			tbOverlayScaling.ValueChanged += new System.EventHandler(tbOverlayScalingChanged);
			chkShowCurrentPageOverlay.AutoSize = true;
			chkShowCurrentPageOverlay.Location = new System.Drawing.Point(58, 193);
			chkShowCurrentPageOverlay.Name = "chkShowCurrentPageOverlay";
			chkShowCurrentPageOverlay.Size = new System.Drawing.Size(117, 17);
			chkShowCurrentPageOverlay.TabIndex = 0;
			chkShowCurrentPageOverlay.Text = "Show current Page";
			chkShowCurrentPageOverlay.UseVisualStyleBackColor = true;
			chkShowStatusOverlay.AutoSize = true;
			chkShowStatusOverlay.Location = new System.Drawing.Point(58, 217);
			chkShowStatusOverlay.Name = "chkShowStatusOverlay";
			chkShowStatusOverlay.Size = new System.Drawing.Size(158, 17);
			chkShowStatusOverlay.TabIndex = 1;
			chkShowStatusOverlay.Text = "Show Messages and Status";
			chkShowStatusOverlay.UseVisualStyleBackColor = true;
			chkShowVisiblePartOverlay.AutoSize = true;
			chkShowVisiblePartOverlay.Location = new System.Drawing.Point(58, 241);
			chkShowVisiblePartOverlay.Name = "chkShowVisiblePartOverlay";
			chkShowVisiblePartOverlay.Size = new System.Drawing.Size(135, 17);
			chkShowVisiblePartOverlay.TabIndex = 2;
			chkShowVisiblePartOverlay.Text = "Show visible Page Part";
			chkShowVisiblePartOverlay.UseVisualStyleBackColor = true;
			chkShowNavigationOverlay.AutoSize = true;
			chkShowNavigationOverlay.Location = new System.Drawing.Point(58, 264);
			chkShowNavigationOverlay.Name = "chkShowNavigationOverlay";
			chkShowNavigationOverlay.Size = new System.Drawing.Size(171, 17);
			chkShowNavigationOverlay.TabIndex = 3;
			chkShowNavigationOverlay.Text = "Show Navigation automatically";
			chkShowNavigationOverlay.UseVisualStyleBackColor = true;
			labelOverlaySize.AutoSize = true;
			labelOverlaySize.Location = new System.Drawing.Point(244, 316);
			labelOverlaySize.Name = "labelOverlaySize";
			labelOverlaySize.Size = new System.Drawing.Size(38, 13);
			labelOverlaySize.TabIndex = 7;
			labelOverlaySize.Text = "Sizing:";
			grpKeyboard.Controls.Add(btExportKeyboard);
			grpKeyboard.Controls.Add(btImportKeyboard);
			grpKeyboard.Controls.Add(keyboardShortcutEditor);
			grpKeyboard.Dock = System.Windows.Forms.DockStyle.Top;
			grpKeyboard.Location = new System.Drawing.Point(0, 300);
			grpKeyboard.Name = "grpKeyboard";
			grpKeyboard.Size = new System.Drawing.Size(498, 390);
			grpKeyboard.TabIndex = 4;
			grpKeyboard.Text = "Keyboard";
			btExportKeyboard.Location = new System.Drawing.Point(274, 357);
			btExportKeyboard.Name = "btExportKeyboard";
			btExportKeyboard.Size = new System.Drawing.Size(102, 23);
			btExportKeyboard.TabIndex = 1;
			btExportKeyboard.Text = "Export...";
			btExportKeyboard.UseVisualStyleBackColor = true;
			btExportKeyboard.Click += new System.EventHandler(btExportKeyboard_Click);
			btImportKeyboard.ContextMenuStrip = cmKeyboardLayout;
			btImportKeyboard.Location = new System.Drawing.Point(382, 357);
			btImportKeyboard.Name = "btImportKeyboard";
			btImportKeyboard.Size = new System.Drawing.Size(102, 23);
			btImportKeyboard.TabIndex = 2;
			btImportKeyboard.Text = "Import...";
			btImportKeyboard.UseVisualStyleBackColor = true;
			btImportKeyboard.ShowContextMenu += new System.EventHandler(btImportKeyboard_ShowContextMenu);
			btImportKeyboard.Click += new System.EventHandler(btLoadKeyboard_Click);
			cmKeyboardLayout.Items.AddRange(new System.Windows.Forms.ToolStripItem[2]
			{
				miDefaultKeyboardLayout,
				toolStripMenuItem1
			});
			cmKeyboardLayout.Name = "cmKeyboardLayout";
			cmKeyboardLayout.Size = new System.Drawing.Size(113, 32);
			miDefaultKeyboardLayout.Name = "miDefaultKeyboardLayout";
			miDefaultKeyboardLayout.Size = new System.Drawing.Size(112, 22);
			miDefaultKeyboardLayout.Text = "&Default";
			miDefaultKeyboardLayout.Click += new System.EventHandler(miDefaultKeyboardLayout_Click);
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(109, 6);
			keyboardShortcutEditor.AllowDrop = true;
			keyboardShortcutEditor.Location = new System.Drawing.Point(12, 37);
			keyboardShortcutEditor.Name = "keyboardShortcutEditor";
			keyboardShortcutEditor.Shortcuts = null;
			keyboardShortcutEditor.Size = new System.Drawing.Size(472, 314);
			keyboardShortcutEditor.TabIndex = 0;
			keyboardShortcutEditor.DragDrop += new System.Windows.Forms.DragEventHandler(keyboardShortcutEditor_DragDrop);
			keyboardShortcutEditor.DragOver += new System.Windows.Forms.DragEventHandler(keyboardShortcutEditor_DragOver);
			grpDisplay.Controls.Add(tbGamma);
			grpDisplay.Controls.Add(labelGamma);
			grpDisplay.Controls.Add(chkAnamorphicScaling);
			grpDisplay.Controls.Add(chkHighQualityDisplay);
			grpDisplay.Controls.Add(labelSharpening);
			grpDisplay.Controls.Add(tbSharpening);
			grpDisplay.Controls.Add(btResetColor);
			grpDisplay.Controls.Add(chkAutoContrast);
			grpDisplay.Controls.Add(labelSaturation);
			grpDisplay.Controls.Add(tbSaturation);
			grpDisplay.Controls.Add(labelBrightness);
			grpDisplay.Controls.Add(tbBrightness);
			grpDisplay.Controls.Add(tbContrast);
			grpDisplay.Controls.Add(labelContrast);
			grpDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			grpDisplay.Location = new System.Drawing.Point(0, 0);
			grpDisplay.Name = "grpDisplay";
			grpDisplay.Size = new System.Drawing.Size(498, 300);
			grpDisplay.TabIndex = 1;
			grpDisplay.Text = "Display";
			tbGamma.Location = new System.Drawing.Point(150, 193);
			tbGamma.Minimum = -100;
			tbGamma.Name = "tbGamma";
			tbGamma.Size = new System.Drawing.Size(332, 16);
			tbGamma.TabIndex = 12;
			tbGamma.Text = "tbSaturation";
			tbGamma.ThumbSize = new System.Drawing.Size(8, 16);
			tbGamma.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbGamma.ValueChanged += new System.EventHandler(tbColorAdjustmentChanged);
			tbGamma.DoubleClick += new System.EventHandler(tbGamma_DoubleClick);
			labelGamma.Location = new System.Drawing.Point(14, 193);
			labelGamma.Name = "labelGamma";
			labelGamma.Size = new System.Drawing.Size(133, 13);
			labelGamma.TabIndex = 11;
			labelGamma.Text = "Gamma Adjustment:";
			labelGamma.TextAlign = System.Drawing.ContentAlignment.TopRight;
			chkAnamorphicScaling.AutoSize = true;
			chkAnamorphicScaling.Location = new System.Drawing.Point(12, 60);
			chkAnamorphicScaling.Name = "chkAnamorphicScaling";
			chkAnamorphicScaling.Size = new System.Drawing.Size(120, 17);
			chkAnamorphicScaling.TabIndex = 0;
			chkAnamorphicScaling.Text = "&Anamorphic Scaling";
			chkAnamorphicScaling.UseVisualStyleBackColor = true;
			chkHighQualityDisplay.AutoSize = true;
			chkHighQualityDisplay.Location = new System.Drawing.Point(12, 37);
			chkHighQualityDisplay.Name = "chkHighQualityDisplay";
			chkHighQualityDisplay.Size = new System.Drawing.Size(83, 17);
			chkHighQualityDisplay.TabIndex = 0;
			chkHighQualityDisplay.Text = "&High Quality";
			chkHighQualityDisplay.UseVisualStyleBackColor = true;
			labelSharpening.Location = new System.Drawing.Point(17, 225);
			labelSharpening.Name = "labelSharpening";
			labelSharpening.Size = new System.Drawing.Size(132, 13);
			labelSharpening.TabIndex = 8;
			labelSharpening.Text = "Sharpening:";
			labelSharpening.TextAlign = System.Drawing.ContentAlignment.TopRight;
			tbSharpening.LargeChange = 1;
			tbSharpening.Location = new System.Drawing.Point(149, 225);
			tbSharpening.Maximum = 3;
			tbSharpening.Name = "tbSharpening";
			tbSharpening.Size = new System.Drawing.Size(333, 18);
			tbSharpening.TabIndex = 9;
			tbSharpening.Text = "tbSaturation";
			tbSharpening.ThumbSize = new System.Drawing.Size(8, 16);
			tbSharpening.TickFrequency = 1;
			tbSharpening.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbSharpening.DoubleClick += new System.EventHandler(tbSharpening_DoubleClick);
			btResetColor.Location = new System.Drawing.Point(394, 265);
			btResetColor.Name = "btResetColor";
			btResetColor.Size = new System.Drawing.Size(91, 23);
			btResetColor.TabIndex = 10;
			btResetColor.Text = "&Reset";
			btResetColor.UseVisualStyleBackColor = true;
			btResetColor.Click += new System.EventHandler(btReset_Click);
			chkAutoContrast.AutoSize = true;
			chkAutoContrast.Location = new System.Drawing.Point(12, 95);
			chkAutoContrast.Name = "chkAutoContrast";
			chkAutoContrast.Size = new System.Drawing.Size(184, 17);
			chkAutoContrast.TabIndex = 1;
			chkAutoContrast.Text = "Automatic &Contrast Enhancement";
			chkAutoContrast.UseVisualStyleBackColor = true;
			labelSaturation.Location = new System.Drawing.Point(11, 122);
			labelSaturation.Name = "labelSaturation";
			labelSaturation.Size = new System.Drawing.Size(136, 13);
			labelSaturation.TabIndex = 2;
			labelSaturation.Text = "Saturation Adjustment:";
			labelSaturation.TextAlign = System.Drawing.ContentAlignment.TopRight;
			tbSaturation.Location = new System.Drawing.Point(148, 122);
			tbSaturation.Minimum = -100;
			tbSaturation.Name = "tbSaturation";
			tbSaturation.Size = new System.Drawing.Size(334, 16);
			tbSaturation.TabIndex = 3;
			tbSaturation.ThumbSize = new System.Drawing.Size(8, 16);
			tbSaturation.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbSaturation.ValueChanged += new System.EventHandler(tbColorAdjustmentChanged);
			tbSaturation.DoubleClick += new System.EventHandler(tbSaturation_DoubleClick);
			labelBrightness.Location = new System.Drawing.Point(14, 144);
			labelBrightness.Name = "labelBrightness";
			labelBrightness.Size = new System.Drawing.Size(133, 13);
			labelBrightness.TabIndex = 4;
			labelBrightness.Text = "Brightness Adjustment:";
			labelBrightness.TextAlign = System.Drawing.ContentAlignment.TopRight;
			tbBrightness.Location = new System.Drawing.Point(148, 144);
			tbBrightness.Minimum = -100;
			tbBrightness.Name = "tbBrightness";
			tbBrightness.Size = new System.Drawing.Size(334, 16);
			tbBrightness.TabIndex = 5;
			tbBrightness.Text = "tbBrightness";
			tbBrightness.ThumbSize = new System.Drawing.Size(8, 16);
			tbBrightness.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbBrightness.ValueChanged += new System.EventHandler(tbColorAdjustmentChanged);
			tbBrightness.DoubleClick += new System.EventHandler(tbBrightness_DoubleClick);
			tbContrast.Location = new System.Drawing.Point(148, 168);
			tbContrast.Minimum = -100;
			tbContrast.Name = "tbContrast";
			tbContrast.Size = new System.Drawing.Size(334, 16);
			tbContrast.TabIndex = 7;
			tbContrast.Text = "tbSaturation";
			tbContrast.ThumbSize = new System.Drawing.Size(8, 16);
			tbContrast.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbContrast.ValueChanged += new System.EventHandler(tbColorAdjustmentChanged);
			tbContrast.DoubleClick += new System.EventHandler(tbContrast_DoubleClick);
			labelContrast.Location = new System.Drawing.Point(14, 168);
			labelContrast.Name = "labelContrast";
			labelContrast.Size = new System.Drawing.Size(133, 13);
			labelContrast.TabIndex = 6;
			labelContrast.Text = "Contrast Adjustment:";
			labelContrast.TextAlign = System.Drawing.ContentAlignment.TopRight;
			pageAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			pageAdvanced.AutoScroll = true;
			pageAdvanced.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			pageAdvanced.Controls.Add(grpWirelessSetup);
			pageAdvanced.Controls.Add(grpIntegration);
			pageAdvanced.Controls.Add(groupMessagesAndSocial);
			pageAdvanced.Controls.Add(groupMemory);
			pageAdvanced.Controls.Add(grpDatabaseBackup);
			pageAdvanced.Controls.Add(groupOtherComics);
			pageAdvanced.Controls.Add(grpLanguages);
			pageAdvanced.Location = new System.Drawing.Point(84, 8);
			pageAdvanced.Name = "pageAdvanced";
			pageAdvanced.Size = new System.Drawing.Size(517, 408);
			pageAdvanced.TabIndex = 9;
			grpWirelessSetup.Controls.Add(btTestWifi);
			grpWirelessSetup.Controls.Add(lblWifiStatus);
			grpWirelessSetup.Controls.Add(lblWifiAddresses);
			grpWirelessSetup.Controls.Add(txWifiAddresses);
			grpWirelessSetup.Dock = System.Windows.Forms.DockStyle.Top;
			grpWirelessSetup.Location = new System.Drawing.Point(0, 1437);
			grpWirelessSetup.Name = "grpWirelessSetup";
			grpWirelessSetup.Size = new System.Drawing.Size(498, 136);
			grpWirelessSetup.TabIndex = 8;
			grpWirelessSetup.Text = "Wireless Setup";
			btTestWifi.Location = new System.Drawing.Point(382, 63);
			btTestWifi.Name = "btTestWifi";
			btTestWifi.Size = new System.Drawing.Size(104, 23);
			btTestWifi.TabIndex = 3;
			btTestWifi.Text = "Test";
			btTestWifi.UseVisualStyleBackColor = true;
			btTestWifi.Click += new System.EventHandler(btTestWifi_Click);
			lblWifiStatus.Location = new System.Drawing.Point(6, 93);
			lblWifiStatus.Name = "lblWifiStatus";
			lblWifiStatus.Size = new System.Drawing.Size(370, 21);
			lblWifiStatus.TabIndex = 2;
			lblWifiStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lblWifiAddresses.AutoSize = true;
			lblWifiAddresses.Location = new System.Drawing.Point(4, 41);
			lblWifiAddresses.Name = "lblWifiAddresses";
			lblWifiAddresses.Size = new System.Drawing.Size(490, 13);
			lblWifiAddresses.TabIndex = 1;
			lblWifiAddresses.Text = "Semicolon separated list of IP addresses for Wireless Devices which where not detected automatically:";
			txWifiAddresses.Location = new System.Drawing.Point(6, 65);
			txWifiAddresses.Name = "txWifiAddresses";
			txWifiAddresses.Size = new System.Drawing.Size(370, 20);
			txWifiAddresses.TabIndex = 0;
			grpIntegration.Controls.Add(btAssociateExtensions);
			grpIntegration.Controls.Add(labelCheckedFormats);
			grpIntegration.Controls.Add(chkOverwriteAssociations);
			grpIntegration.Controls.Add(lbFormats);
			grpIntegration.Dock = System.Windows.Forms.DockStyle.Top;
			grpIntegration.Location = new System.Drawing.Point(0, 1097);
			grpIntegration.Name = "grpIntegration";
			grpIntegration.Size = new System.Drawing.Size(498, 340);
			grpIntegration.TabIndex = 0;
			grpIntegration.Text = "Explorer Integration";
			btAssociateExtensions.Location = new System.Drawing.Point(382, 57);
			btAssociateExtensions.Name = "btAssociateExtensions";
			btAssociateExtensions.Size = new System.Drawing.Size(104, 23);
			btAssociateExtensions.TabIndex = 4;
			btAssociateExtensions.Text = "Change...";
			btAssociateExtensions.UseVisualStyleBackColor = true;
			btAssociateExtensions.Click += new System.EventHandler(btAssociateExtensions_Click);
			labelCheckedFormats.AutoSize = true;
			labelCheckedFormats.Location = new System.Drawing.Point(3, 35);
			labelCheckedFormats.Name = "labelCheckedFormats";
			labelCheckedFormats.Size = new System.Drawing.Size(253, 13);
			labelCheckedFormats.TabIndex = 0;
			labelCheckedFormats.Text = "Checked formats will be associated with ComicRack";
			chkOverwriteAssociations.AutoSize = true;
			chkOverwriteAssociations.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			chkOverwriteAssociations.Location = new System.Drawing.Point(6, 307);
			chkOverwriteAssociations.Name = "chkOverwriteAssociations";
			chkOverwriteAssociations.Size = new System.Drawing.Size(289, 17);
			chkOverwriteAssociations.TabIndex = 2;
			chkOverwriteAssociations.Text = "Overwrite existing associations instead of 'Open With ...'";
			chkOverwriteAssociations.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			chkOverwriteAssociations.UseVisualStyleBackColor = true;
			lbFormats.CheckOnClick = true;
			lbFormats.FormattingEnabled = true;
			lbFormats.Location = new System.Drawing.Point(6, 57);
			lbFormats.Name = "lbFormats";
			lbFormats.Size = new System.Drawing.Size(371, 244);
			lbFormats.TabIndex = 1;
			groupMessagesAndSocial.Controls.Add(btResetTwitter);
			groupMessagesAndSocial.Controls.Add(labelResetTwitter);
			groupMessagesAndSocial.Controls.Add(btResetMessages);
			groupMessagesAndSocial.Controls.Add(labelReshowHidden);
			groupMessagesAndSocial.Dock = System.Windows.Forms.DockStyle.Top;
			groupMessagesAndSocial.Location = new System.Drawing.Point(0, 996);
			groupMessagesAndSocial.Name = "groupMessagesAndSocial";
			groupMessagesAndSocial.Size = new System.Drawing.Size(498, 101);
			groupMessagesAndSocial.TabIndex = 6;
			groupMessagesAndSocial.Text = "Messages and Social";
			btResetMessages.Location = new System.Drawing.Point(382, 41);
			btResetMessages.Name = "btResetMessages";
			btResetMessages.Size = new System.Drawing.Size(104, 23);
			btResetMessages.TabIndex = 1;
			btResetMessages.Text = "Reset";
			btResetMessages.UseVisualStyleBackColor = true;
			btResetMessages.Click += new System.EventHandler(btResetMessages_Click);
			labelReshowHidden.Location = new System.Drawing.Point(6, 46);
			labelReshowHidden.Name = "labelReshowHidden";
			labelReshowHidden.Size = new System.Drawing.Size(370, 17);
			labelReshowHidden.TabIndex = 0;
			labelReshowHidden.Text = "To reshow hidden messages press";
			groupMemory.Controls.Add(grpMaximumMemoryUsage);
			groupMemory.Controls.Add(grpMemoryCache);
			groupMemory.Controls.Add(grpDiskCache);
			groupMemory.Dock = System.Windows.Forms.DockStyle.Top;
			groupMemory.Location = new System.Drawing.Point(0, 641);
			groupMemory.Name = "groupMemory";
			groupMemory.Size = new System.Drawing.Size(498, 355);
			groupMemory.TabIndex = 1;
			groupMemory.Text = "Caches & Memory Usage";
			grpMaximumMemoryUsage.Controls.Add(lblMaximumMemoryUsageValue);
			grpMaximumMemoryUsage.Controls.Add(tbMaximumMemoryUsage);
			grpMaximumMemoryUsage.Controls.Add(lblMaximumMemoryUsage);
			grpMaximumMemoryUsage.Location = new System.Drawing.Point(7, 255);
			grpMaximumMemoryUsage.Name = "grpMaximumMemoryUsage";
			grpMaximumMemoryUsage.Size = new System.Drawing.Size(476, 86);
			grpMaximumMemoryUsage.TabIndex = 14;
			grpMaximumMemoryUsage.TabStop = false;
			grpMaximumMemoryUsage.Text = "Maximum Memory Usage";
			lblMaximumMemoryUsageValue.AutoSize = true;
			lblMaximumMemoryUsageValue.Location = new System.Drawing.Point(397, 31);
			lblMaximumMemoryUsageValue.Name = "lblMaximumMemoryUsageValue";
			lblMaximumMemoryUsageValue.Size = new System.Drawing.Size(63, 13);
			lblMaximumMemoryUsageValue.TabIndex = 2;
			lblMaximumMemoryUsageValue.Text = "Slider Value";
			tbMaximumMemoryUsage.LargeChange = 4;
			tbMaximumMemoryUsage.Location = new System.Drawing.Point(7, 24);
			tbMaximumMemoryUsage.Maximum = 64;
			tbMaximumMemoryUsage.Name = "tbMaximumMemoryUsage";
			tbMaximumMemoryUsage.Size = new System.Drawing.Size(379, 29);
			tbMaximumMemoryUsage.TabIndex = 1;
			tbMaximumMemoryUsage.ThumbSize = new System.Drawing.Size(10, 20);
			tbMaximumMemoryUsage.TickFrequency = 8;
			tbMaximumMemoryUsage.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			tbMaximumMemoryUsage.TickThickness = 2;
			tbMaximumMemoryUsage.ValueChanged += new System.EventHandler(tbSystemMemory_ValueChanged);
			lblMaximumMemoryUsage.Dock = System.Windows.Forms.DockStyle.Bottom;
			lblMaximumMemoryUsage.Location = new System.Drawing.Point(3, 58);
			lblMaximumMemoryUsage.Name = "lblMaximumMemoryUsage";
			lblMaximumMemoryUsage.Size = new System.Drawing.Size(470, 25);
			lblMaximumMemoryUsage.TabIndex = 0;
			lblMaximumMemoryUsage.Text = "Limiting the memory can adversely affect the performance.";
			lblMaximumMemoryUsage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			grpMemoryCache.Controls.Add(lblPageMemCacheUsage);
			grpMemoryCache.Controls.Add(labelMemThumbSize);
			grpMemoryCache.Controls.Add(lblThumbMemCacheUsage);
			grpMemoryCache.Controls.Add(numMemPageCount);
			grpMemoryCache.Controls.Add(labelMemPageCount);
			grpMemoryCache.Controls.Add(chkMemPageOptimized);
			grpMemoryCache.Controls.Add(chkMemThumbOptimized);
			grpMemoryCache.Controls.Add(numMemThumbSize);
			grpMemoryCache.Location = new System.Drawing.Point(6, 162);
			grpMemoryCache.Name = "grpMemoryCache";
			grpMemoryCache.Size = new System.Drawing.Size(476, 85);
			grpMemoryCache.TabIndex = 13;
			grpMemoryCache.TabStop = false;
			grpMemoryCache.Text = "Memory Cache";
			lblPageMemCacheUsage.AutoSize = true;
			lblPageMemCacheUsage.Location = new System.Drawing.Point(299, 52);
			lblPageMemCacheUsage.Name = "lblPageMemCacheUsage";
			lblPageMemCacheUsage.Size = new System.Drawing.Size(124, 13);
			lblPageMemCacheUsage.TabIndex = 8;
			lblPageMemCacheUsage.Text = "usage Page Mem Cache";
			labelMemThumbSize.AutoSize = true;
			labelMemThumbSize.Location = new System.Drawing.Point(19, 29);
			labelMemThumbSize.Name = "labelMemThumbSize";
			labelMemThumbSize.Size = new System.Drawing.Size(86, 13);
			labelMemThumbSize.TabIndex = 0;
			labelMemThumbSize.Text = "Thumbnails [MB]";
			lblThumbMemCacheUsage.AutoSize = true;
			lblThumbMemCacheUsage.Location = new System.Drawing.Point(299, 26);
			lblThumbMemCacheUsage.Name = "lblThumbMemCacheUsage";
			lblThumbMemCacheUsage.Size = new System.Drawing.Size(132, 13);
			lblThumbMemCacheUsage.TabIndex = 7;
			lblThumbMemCacheUsage.Text = "usage Thumb Mem Cache";
			numMemPageCount.Location = new System.Drawing.Point(145, 51);
			numMemPageCount.Maximum = new decimal(new int[4]
			{
				25,
				0,
				0,
				0
			});
			numMemPageCount.Minimum = new decimal(new int[4]
			{
				5,
				0,
				0,
				0
			});
			numMemPageCount.Name = "numMemPageCount";
			numMemPageCount.Size = new System.Drawing.Size(67, 20);
			numMemPageCount.TabIndex = 4;
			numMemPageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			numMemPageCount.Value = new decimal(new int[4]
			{
				5,
				0,
				0,
				0
			});
			labelMemPageCount.AutoSize = true;
			labelMemPageCount.Location = new System.Drawing.Point(19, 52);
			labelMemPageCount.Name = "labelMemPageCount";
			labelMemPageCount.Size = new System.Drawing.Size(73, 13);
			labelMemPageCount.TabIndex = 3;
			labelMemPageCount.Text = "Pages [count]";
			chkMemPageOptimized.AutoSize = true;
			chkMemPageOptimized.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			chkMemPageOptimized.Location = new System.Drawing.Point(218, 51);
			chkMemPageOptimized.Name = "chkMemPageOptimized";
			chkMemPageOptimized.Size = new System.Drawing.Size(70, 17);
			chkMemPageOptimized.TabIndex = 5;
			chkMemPageOptimized.Text = "optimized";
			chkMemPageOptimized.UseVisualStyleBackColor = true;
			chkMemThumbOptimized.AutoSize = true;
			chkMemThumbOptimized.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			chkMemThumbOptimized.Location = new System.Drawing.Point(218, 25);
			chkMemThumbOptimized.Name = "chkMemThumbOptimized";
			chkMemThumbOptimized.Size = new System.Drawing.Size(70, 17);
			chkMemThumbOptimized.TabIndex = 2;
			chkMemThumbOptimized.Text = "optimized";
			chkMemThumbOptimized.UseVisualStyleBackColor = true;
			numMemThumbSize.Increment = new decimal(new int[4]
			{
				5,
				0,
				0,
				0
			});
			numMemThumbSize.Location = new System.Drawing.Point(145, 24);
			numMemThumbSize.Minimum = new decimal(new int[4]
			{
				20,
				0,
				0,
				0
			});
			numMemThumbSize.Name = "numMemThumbSize";
			numMemThumbSize.Size = new System.Drawing.Size(67, 20);
			numMemThumbSize.TabIndex = 1;
			numMemThumbSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			numMemThumbSize.Value = new decimal(new int[4]
			{
				25,
				0,
				0,
				0
			});
			grpDiskCache.Controls.Add(chkEnableInternetCache);
			grpDiskCache.Controls.Add(lblInternetCacheUsage);
			grpDiskCache.Controls.Add(btClearPageCache);
			grpDiskCache.Controls.Add(numPageCacheSize);
			grpDiskCache.Controls.Add(numInternetCacheSize);
			grpDiskCache.Controls.Add(btClearThumbnailCache);
			grpDiskCache.Controls.Add(btClearInternetCache);
			grpDiskCache.Controls.Add(chkEnablePageCache);
			grpDiskCache.Controls.Add(lblPageCacheUsage);
			grpDiskCache.Controls.Add(numThumbnailCacheSize);
			grpDiskCache.Controls.Add(chkEnableThumbnailCache);
			grpDiskCache.Controls.Add(lblThumbCacheUsage);
			grpDiskCache.Location = new System.Drawing.Point(6, 35);
			grpDiskCache.Name = "grpDiskCache";
			grpDiskCache.Size = new System.Drawing.Size(476, 120);
			grpDiskCache.TabIndex = 12;
			grpDiskCache.TabStop = false;
			grpDiskCache.Text = "Disk Cache";
			chkEnableInternetCache.AutoSize = true;
			chkEnableInternetCache.Location = new System.Drawing.Point(22, 31);
			chkEnableInternetCache.Name = "chkEnableInternetCache";
			chkEnableInternetCache.Size = new System.Drawing.Size(87, 17);
			chkEnableInternetCache.TabIndex = 0;
			chkEnableInternetCache.Text = "Internet [MB]";
			chkEnableInternetCache.UseVisualStyleBackColor = true;
			lblInternetCacheUsage.AutoSize = true;
			lblInternetCacheUsage.Location = new System.Drawing.Point(298, 31);
			lblInternetCacheUsage.Name = "lblInternetCacheUsage";
			lblInternetCacheUsage.Size = new System.Drawing.Size(109, 13);
			lblInternetCacheUsage.TabIndex = 3;
			lblInternetCacheUsage.Text = "usage Internet Cache";
			btClearPageCache.Location = new System.Drawing.Point(218, 80);
			btClearPageCache.Name = "btClearPageCache";
			btClearPageCache.Size = new System.Drawing.Size(74, 21);
			btClearPageCache.TabIndex = 10;
			btClearPageCache.Text = "Clear";
			btClearPageCache.UseVisualStyleBackColor = true;
			btClearPageCache.Click += new System.EventHandler(btClearPageCache_Click);
			numPageCacheSize.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numPageCacheSize.Location = new System.Drawing.Point(145, 82);
			numPageCacheSize.Maximum = new decimal(new int[4]
			{
				1000000,
				0,
				0,
				0
			});
			numPageCacheSize.Minimum = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numPageCacheSize.Name = "numPageCacheSize";
			numPageCacheSize.Size = new System.Drawing.Size(67, 20);
			numPageCacheSize.TabIndex = 9;
			numPageCacheSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			numPageCacheSize.Value = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numInternetCacheSize.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numInternetCacheSize.Location = new System.Drawing.Point(145, 29);
			numInternetCacheSize.Maximum = new decimal(new int[4]
			{
				1000000,
				0,
				0,
				0
			});
			numInternetCacheSize.Minimum = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numInternetCacheSize.Name = "numInternetCacheSize";
			numInternetCacheSize.Size = new System.Drawing.Size(67, 20);
			numInternetCacheSize.TabIndex = 1;
			numInternetCacheSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			numInternetCacheSize.Value = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			btClearThumbnailCache.Location = new System.Drawing.Point(218, 54);
			btClearThumbnailCache.Name = "btClearThumbnailCache";
			btClearThumbnailCache.Size = new System.Drawing.Size(74, 21);
			btClearThumbnailCache.TabIndex = 6;
			btClearThumbnailCache.Text = "Clear";
			btClearThumbnailCache.UseVisualStyleBackColor = true;
			btClearThumbnailCache.Click += new System.EventHandler(btClearThumbnailCache_Click);
			btClearInternetCache.Location = new System.Drawing.Point(218, 27);
			btClearInternetCache.Name = "btClearInternetCache";
			btClearInternetCache.Size = new System.Drawing.Size(74, 21);
			btClearInternetCache.TabIndex = 2;
			btClearInternetCache.Text = "Clear";
			btClearInternetCache.UseVisualStyleBackColor = true;
			btClearInternetCache.Click += new System.EventHandler(btClearInternetCache_Click);
			chkEnablePageCache.AutoSize = true;
			chkEnablePageCache.Location = new System.Drawing.Point(22, 84);
			chkEnablePageCache.Name = "chkEnablePageCache";
			chkEnablePageCache.Size = new System.Drawing.Size(81, 17);
			chkEnablePageCache.TabIndex = 8;
			chkEnablePageCache.Text = "&Pages [MB]";
			chkEnablePageCache.UseVisualStyleBackColor = true;
			lblPageCacheUsage.AutoSize = true;
			lblPageCacheUsage.Location = new System.Drawing.Point(298, 86);
			lblPageCacheUsage.Name = "lblPageCacheUsage";
			lblPageCacheUsage.Size = new System.Drawing.Size(98, 13);
			lblPageCacheUsage.TabIndex = 11;
			lblPageCacheUsage.Text = "usage Page Cache";
			numThumbnailCacheSize.Increment = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numThumbnailCacheSize.Location = new System.Drawing.Point(145, 55);
			numThumbnailCacheSize.Maximum = new decimal(new int[4]
			{
				1000000,
				0,
				0,
				0
			});
			numThumbnailCacheSize.Minimum = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			numThumbnailCacheSize.Name = "numThumbnailCacheSize";
			numThumbnailCacheSize.Size = new System.Drawing.Size(67, 20);
			numThumbnailCacheSize.TabIndex = 5;
			numThumbnailCacheSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			numThumbnailCacheSize.Value = new decimal(new int[4]
			{
				10,
				0,
				0,
				0
			});
			chkEnableThumbnailCache.AutoSize = true;
			chkEnableThumbnailCache.Location = new System.Drawing.Point(22, 57);
			chkEnableThumbnailCache.Name = "chkEnableThumbnailCache";
			chkEnableThumbnailCache.Size = new System.Drawing.Size(105, 17);
			chkEnableThumbnailCache.TabIndex = 4;
			chkEnableThumbnailCache.Text = "&Thumbnails [MB]";
			chkEnableThumbnailCache.UseVisualStyleBackColor = true;
			lblThumbCacheUsage.AutoSize = true;
			lblThumbCacheUsage.Location = new System.Drawing.Point(298, 58);
			lblThumbCacheUsage.Name = "lblThumbCacheUsage";
			lblThumbCacheUsage.Size = new System.Drawing.Size(106, 13);
			lblThumbCacheUsage.TabIndex = 7;
			lblThumbCacheUsage.Text = "usage Thumb Cache";
			grpDatabaseBackup.Controls.Add(btRestoreDatabase);
			grpDatabaseBackup.Controls.Add(btBackupDatabase);
			grpDatabaseBackup.Dock = System.Windows.Forms.DockStyle.Top;
			grpDatabaseBackup.Location = new System.Drawing.Point(0, 548);
			grpDatabaseBackup.Name = "grpDatabaseBackup";
			grpDatabaseBackup.Size = new System.Drawing.Size(498, 93);
			grpDatabaseBackup.TabIndex = 4;
			grpDatabaseBackup.Text = "Database Backup";
			btRestoreDatabase.Location = new System.Drawing.Point(259, 41);
			btRestoreDatabase.Name = "btRestoreDatabase";
			btRestoreDatabase.Size = new System.Drawing.Size(227, 23);
			btRestoreDatabase.TabIndex = 1;
			btRestoreDatabase.Text = "Restore Database...";
			btRestoreDatabase.UseVisualStyleBackColor = true;
			btRestoreDatabase.Click += new System.EventHandler(btRestoreDatabase_Click);
			btBackupDatabase.Location = new System.Drawing.Point(9, 41);
			btBackupDatabase.Name = "btBackupDatabase";
			btBackupDatabase.Size = new System.Drawing.Size(247, 23);
			btBackupDatabase.TabIndex = 0;
			btBackupDatabase.Text = "Backup Database...";
			btBackupDatabase.UseVisualStyleBackColor = true;
			btBackupDatabase.Click += new System.EventHandler(btBackupDatabase_Click);
			groupOtherComics.Controls.Add(chkUpdateComicFiles);
			groupOtherComics.Controls.Add(labelExcludeCover);
			groupOtherComics.Controls.Add(chkAutoUpdateComicFiles);
			groupOtherComics.Controls.Add(txCoverFilter);
			groupOtherComics.Dock = System.Windows.Forms.DockStyle.Top;
			groupOtherComics.Location = new System.Drawing.Point(0, 372);
			groupOtherComics.Name = "groupOtherComics";
			groupOtherComics.Size = new System.Drawing.Size(498, 176);
			groupOtherComics.TabIndex = 5;
			groupOtherComics.Text = "Books";
			chkUpdateComicFiles.AutoSize = true;
			chkUpdateComicFiles.Location = new System.Drawing.Point(9, 42);
			chkUpdateComicFiles.Name = "chkUpdateComicFiles";
			chkUpdateComicFiles.Size = new System.Drawing.Size(185, 17);
			chkUpdateComicFiles.TabIndex = 0;
			chkUpdateComicFiles.Text = "Allow writing of Book info into files";
			chkUpdateComicFiles.UseVisualStyleBackColor = true;
			labelExcludeCover.AutoSize = true;
			labelExcludeCover.Location = new System.Drawing.Point(6, 93);
			labelExcludeCover.Name = "labelExcludeCover";
			labelExcludeCover.Size = new System.Drawing.Size(381, 13);
			labelExcludeCover.TabIndex = 2;
			labelExcludeCover.Text = "Semicolon separated list of image names never to be used as cover thumbnails:";
			chkAutoUpdateComicFiles.AutoSize = true;
			chkAutoUpdateComicFiles.Location = new System.Drawing.Point(9, 65);
			chkAutoUpdateComicFiles.Name = "chkAutoUpdateComicFiles";
			chkAutoUpdateComicFiles.Size = new System.Drawing.Size(196, 17);
			chkAutoUpdateComicFiles.TabIndex = 1;
			chkAutoUpdateComicFiles.Text = "Book files are updated automatically";
			chkAutoUpdateComicFiles.UseVisualStyleBackColor = true;
			txCoverFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txCoverFilter.Location = new System.Drawing.Point(9, 112);
			txCoverFilter.Multiline = true;
			txCoverFilter.Name = "txCoverFilter";
			txCoverFilter.Size = new System.Drawing.Size(482, 54);
			txCoverFilter.TabIndex = 3;
			grpLanguages.Controls.Add(btTranslate);
			grpLanguages.Controls.Add(labelLanguage);
			grpLanguages.Controls.Add(lbLanguages);
			grpLanguages.Dock = System.Windows.Forms.DockStyle.Top;
			grpLanguages.Location = new System.Drawing.Point(0, 0);
			grpLanguages.Name = "grpLanguages";
			grpLanguages.Size = new System.Drawing.Size(498, 372);
			grpLanguages.TabIndex = 7;
			grpLanguages.Text = "Languages";
			btTranslate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btTranslate.Location = new System.Drawing.Point(207, 339);
			btTranslate.Name = "btTranslate";
			btTranslate.Size = new System.Drawing.Size(284, 23);
			btTranslate.TabIndex = 12;
			btTranslate.Text = "Help localizing ComicRack...";
			btTranslate.UseVisualStyleBackColor = true;
			btTranslate.Click += new System.EventHandler(btTranslate_Click);
			labelLanguage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			labelLanguage.Location = new System.Drawing.Point(6, 33);
			labelLanguage.Name = "labelLanguage";
			labelLanguage.Size = new System.Drawing.Size(485, 35);
			labelLanguage.TabIndex = 11;
			labelLanguage.Text = "Select the User Interface language for ComicRack (ComicRack must be restarted for any change to take effect):";
			lbLanguages.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lbLanguages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			lbLanguages.FormattingEnabled = true;
			lbLanguages.ItemHeight = 15;
			lbLanguages.Location = new System.Drawing.Point(6, 75);
			lbLanguages.Name = "lbLanguages";
			lbLanguages.Size = new System.Drawing.Size(485, 259);
			lbLanguages.TabIndex = 0;
			lbLanguages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(lbLanguages_DrawItem);
			pageLibrary.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			pageLibrary.AutoScroll = true;
			pageLibrary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			pageLibrary.Controls.Add(grpServerSettings);
			pageLibrary.Controls.Add(grpSharing);
			pageLibrary.Controls.Add(groupLibraryDisplay);
			pageLibrary.Controls.Add(grpScanning);
			pageLibrary.Controls.Add(groupComicFolders);
			pageLibrary.Location = new System.Drawing.Point(84, 8);
			pageLibrary.Name = "pageLibrary";
			pageLibrary.Size = new System.Drawing.Size(517, 408);
			pageLibrary.TabIndex = 10;
			grpServerSettings.Controls.Add(txPrivateListingPassword);
			grpServerSettings.Controls.Add(labelPrivateListPassword);
			grpServerSettings.Controls.Add(labelPublicServerAddress);
			grpServerSettings.Controls.Add(txPublicServerAddress);
			grpServerSettings.Dock = System.Windows.Forms.DockStyle.Top;
			grpServerSettings.Location = new System.Drawing.Point(0, 910);
			grpServerSettings.Name = "grpServerSettings";
			grpServerSettings.Size = new System.Drawing.Size(498, 148);
			grpServerSettings.TabIndex = 3;
			grpServerSettings.Text = "Server Settings";
			txPrivateListingPassword.Location = new System.Drawing.Point(12, 114);
			txPrivateListingPassword.Name = "txPrivateListingPassword";
			txPrivateListingPassword.Password = null;
			txPrivateListingPassword.Size = new System.Drawing.Size(379, 20);
			txPrivateListingPassword.TabIndex = 3;
			txPrivateListingPassword.UseSystemPasswordChar = true;
			labelPrivateListPassword.AutoSize = true;
			labelPrivateListPassword.Location = new System.Drawing.Point(13, 96);
			labelPrivateListPassword.Name = "labelPrivateListPassword";
			labelPrivateListPassword.Size = new System.Drawing.Size(307, 13);
			labelPrivateListPassword.TabIndex = 2;
			labelPrivateListPassword.Text = "Password used to protect your private Internet Share list entries:";
			labelPublicServerAddress.AutoSize = true;
			labelPublicServerAddress.Location = new System.Drawing.Point(14, 41);
			labelPublicServerAddress.Name = "labelPublicServerAddress";
			labelPublicServerAddress.Size = new System.Drawing.Size(368, 13);
			labelPublicServerAddress.TabIndex = 0;
			labelPublicServerAddress.Text = "External IP address of your server if ComicRack should not guess it correctly:";
			txPublicServerAddress.Location = new System.Drawing.Point(12, 60);
			txPublicServerAddress.Name = "txPublicServerAddress";
			txPublicServerAddress.Size = new System.Drawing.Size(379, 20);
			txPublicServerAddress.TabIndex = 1;
			grpSharing.Controls.Add(chkAutoConnectShares);
			grpSharing.Controls.Add(btRemoveShare);
			grpSharing.Controls.Add(btAddShare);
			grpSharing.Controls.Add(tabShares);
			grpSharing.Controls.Add(chkLookForShared);
			grpSharing.Dock = System.Windows.Forms.DockStyle.Top;
			grpSharing.Location = new System.Drawing.Point(0, 509);
			grpSharing.Name = "grpSharing";
			grpSharing.Size = new System.Drawing.Size(498, 401);
			grpSharing.TabIndex = 1;
			grpSharing.Text = "Sharing";
			chkAutoConnectShares.AutoSize = true;
			chkAutoConnectShares.Location = new System.Drawing.Point(261, 36);
			chkAutoConnectShares.Name = "chkAutoConnectShares";
			chkAutoConnectShares.Size = new System.Drawing.Size(130, 17);
			chkAutoConnectShares.TabIndex = 1;
			chkAutoConnectShares.Text = "Connect automatically";
			chkAutoConnectShares.UseVisualStyleBackColor = true;
			btRemoveShare.Location = new System.Drawing.Point(398, 89);
			btRemoveShare.Name = "btRemoveShare";
			btRemoveShare.Size = new System.Drawing.Size(92, 23);
			btRemoveShare.TabIndex = 4;
			btRemoveShare.Text = "Remove";
			btRemoveShare.UseVisualStyleBackColor = true;
			btRemoveShare.Click += new System.EventHandler(btRmoveShare_Click);
			btAddShare.Location = new System.Drawing.Point(398, 61);
			btAddShare.Name = "btAddShare";
			btAddShare.Size = new System.Drawing.Size(92, 23);
			btAddShare.TabIndex = 3;
			btAddShare.Text = "Add Share";
			btAddShare.UseVisualStyleBackColor = true;
			btAddShare.Click += new System.EventHandler(btAddShare_Click);
			tabShares.Location = new System.Drawing.Point(12, 59);
			tabShares.Name = "tabShares";
			tabShares.SelectedIndex = 0;
			tabShares.Size = new System.Drawing.Size(381, 336);
			tabShares.TabIndex = 2;
			chkLookForShared.AutoSize = true;
			chkLookForShared.Location = new System.Drawing.Point(12, 36);
			chkLookForShared.Name = "chkLookForShared";
			chkLookForShared.Size = new System.Drawing.Size(154, 17);
			chkLookForShared.TabIndex = 0;
			chkLookForShared.Text = "Look for local Book Shares";
			chkLookForShared.UseVisualStyleBackColor = true;
			groupLibraryDisplay.Controls.Add(chkLibraryGaugesTotal);
			groupLibraryDisplay.Controls.Add(chkLibraryGaugesUnread);
			groupLibraryDisplay.Controls.Add(chkLibraryGaugesNumeric);
			groupLibraryDisplay.Controls.Add(chkLibraryGaugesNew);
			groupLibraryDisplay.Controls.Add(chkLibraryGauges);
			groupLibraryDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			groupLibraryDisplay.Location = new System.Drawing.Point(0, 339);
			groupLibraryDisplay.Name = "groupLibraryDisplay";
			groupLibraryDisplay.Size = new System.Drawing.Size(498, 170);
			groupLibraryDisplay.TabIndex = 4;
			groupLibraryDisplay.Text = "Display";
			chkLibraryGaugesTotal.AutoSize = true;
			chkLibraryGaugesTotal.Location = new System.Drawing.Point(33, 111);
			chkLibraryGaugesTotal.Name = "chkLibraryGaugesTotal";
			chkLibraryGaugesTotal.Size = new System.Drawing.Size(113, 17);
			chkLibraryGaugesTotal.TabIndex = 1;
			chkLibraryGaugesTotal.Text = "For Total of Books";
			chkLibraryGaugesTotal.UseVisualStyleBackColor = true;
			chkLibraryGaugesUnread.AutoSize = true;
			chkLibraryGaugesUnread.Location = new System.Drawing.Point(33, 92);
			chkLibraryGaugesUnread.Name = "chkLibraryGaugesUnread";
			chkLibraryGaugesUnread.Size = new System.Drawing.Size(112, 17);
			chkLibraryGaugesUnread.TabIndex = 1;
			chkLibraryGaugesUnread.Text = "For Unread Books";
			chkLibraryGaugesUnread.UseVisualStyleBackColor = true;
			chkLibraryGaugesNumeric.AutoSize = true;
			chkLibraryGaugesNumeric.Location = new System.Drawing.Point(33, 131);
			chkLibraryGaugesNumeric.Name = "chkLibraryGaugesNumeric";
			chkLibraryGaugesNumeric.Size = new System.Drawing.Size(201, 17);
			chkLibraryGaugesNumeric.TabIndex = 1;
			chkLibraryGaugesNumeric.Text = "Also show numbers and not only bars";
			chkLibraryGaugesNumeric.UseVisualStyleBackColor = true;
			chkLibraryGaugesNew.AutoSize = true;
			chkLibraryGaugesNew.Location = new System.Drawing.Point(33, 72);
			chkLibraryGaugesNew.Name = "chkLibraryGaugesNew";
			chkLibraryGaugesNew.Size = new System.Drawing.Size(99, 17);
			chkLibraryGaugesNew.TabIndex = 1;
			chkLibraryGaugesNew.Text = "For New Books";
			chkLibraryGaugesNew.UseVisualStyleBackColor = true;
			chkLibraryGauges.AutoSize = true;
			chkLibraryGauges.Location = new System.Drawing.Point(12, 42);
			chkLibraryGauges.Name = "chkLibraryGauges";
			chkLibraryGauges.Size = new System.Drawing.Size(127, 17);
			chkLibraryGauges.TabIndex = 0;
			chkLibraryGauges.Text = "Enable Live Counters";
			chkLibraryGauges.UseVisualStyleBackColor = true;
			chkLibraryGauges.CheckedChanged += new System.EventHandler(chkLibraryGauges_CheckedChanged);
			grpScanning.Controls.Add(chkDontAddRemovedFiles);
			grpScanning.Controls.Add(chkAutoRemoveMissing);
			grpScanning.Controls.Add(lblScan);
			grpScanning.Controls.Add(btScan);
			grpScanning.Dock = System.Windows.Forms.DockStyle.Top;
			grpScanning.Location = new System.Drawing.Point(0, 203);
			grpScanning.Name = "grpScanning";
			grpScanning.Size = new System.Drawing.Size(498, 136);
			grpScanning.TabIndex = 0;
			grpScanning.Text = "Scanning";
			chkDontAddRemovedFiles.AutoSize = true;
			chkDontAddRemovedFiles.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			chkDontAddRemovedFiles.Location = new System.Drawing.Point(12, 58);
			chkDontAddRemovedFiles.Name = "chkDontAddRemovedFiles";
			chkDontAddRemovedFiles.Size = new System.Drawing.Size(322, 17);
			chkDontAddRemovedFiles.TabIndex = 1;
			chkDontAddRemovedFiles.Text = "Files manually removed from the Library will not be added again";
			chkDontAddRemovedFiles.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			chkDontAddRemovedFiles.UseVisualStyleBackColor = true;
			chkAutoRemoveMissing.AutoSize = true;
			chkAutoRemoveMissing.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			chkAutoRemoveMissing.Location = new System.Drawing.Point(12, 35);
			chkAutoRemoveMissing.Name = "chkAutoRemoveMissing";
			chkAutoRemoveMissing.Size = new System.Drawing.Size(301, 17);
			chkAutoRemoveMissing.TabIndex = 0;
			chkAutoRemoveMissing.Text = "Automatically remove missing files from Library during Scan";
			chkAutoRemoveMissing.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			chkAutoRemoveMissing.UseVisualStyleBackColor = true;
			lblScan.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lblScan.Location = new System.Drawing.Point(9, 83);
			lblScan.Name = "lblScan";
			lblScan.Size = new System.Drawing.Size(480, 43);
			lblScan.TabIndex = 8;
			lblScan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			btScan.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btScan.Location = new System.Drawing.Point(403, 33);
			btScan.Name = "btScan";
			btScan.Size = new System.Drawing.Size(88, 23);
			btScan.TabIndex = 2;
			btScan.Text = "Scan";
			btScan.UseVisualStyleBackColor = true;
			btScan.Click += new System.EventHandler(btScan_Click);
			groupComicFolders.Controls.Add(btOpenFolder);
			groupComicFolders.Controls.Add(btChangeFolder);
			groupComicFolders.Controls.Add(lbPaths);
			groupComicFolders.Controls.Add(labelWatchedFolders);
			groupComicFolders.Controls.Add(btRemoveFolder);
			groupComicFolders.Controls.Add(btAddFolder);
			groupComicFolders.Dock = System.Windows.Forms.DockStyle.Top;
			groupComicFolders.Location = new System.Drawing.Point(0, 0);
			groupComicFolders.Name = "groupComicFolders";
			groupComicFolders.Size = new System.Drawing.Size(498, 203);
			groupComicFolders.TabIndex = 0;
			groupComicFolders.Text = "Book Folders";
			btOpenFolder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btOpenFolder.Location = new System.Drawing.Point(400, 134);
			btOpenFolder.Name = "btOpenFolder";
			btOpenFolder.Size = new System.Drawing.Size(89, 23);
			btOpenFolder.TabIndex = 4;
			btOpenFolder.Text = "Open";
			btOpenFolder.UseVisualStyleBackColor = true;
			btOpenFolder.Click += new System.EventHandler(btOpenFolder_Click);
			btChangeFolder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btChangeFolder.Location = new System.Drawing.Point(400, 66);
			btChangeFolder.Name = "btChangeFolder";
			btChangeFolder.Size = new System.Drawing.Size(89, 23);
			btChangeFolder.TabIndex = 2;
			btChangeFolder.Text = "&Change...";
			btChangeFolder.UseVisualStyleBackColor = true;
			btChangeFolder.Click += new System.EventHandler(btChangeFolder_Click);
			lbPaths.AllowDrop = true;
			lbPaths.FormattingEnabled = true;
			lbPaths.IntegralHeight = false;
			lbPaths.Location = new System.Drawing.Point(12, 37);
			lbPaths.Name = "lbPaths";
			lbPaths.Size = new System.Drawing.Size(377, 120);
			lbPaths.TabIndex = 0;
			lbPaths.DrawItemText += new System.Windows.Forms.DrawItemEventHandler(lbPaths_DrawItemText);
			lbPaths.DrawItem += new System.Windows.Forms.DrawItemEventHandler(lbPaths_DrawItem);
			lbPaths.SelectedIndexChanged += new System.EventHandler(lbPaths_SelectedIndexChanged);
			lbPaths.DragDrop += new System.Windows.Forms.DragEventHandler(lbPaths_DragDrop);
			lbPaths.DragOver += new System.Windows.Forms.DragEventHandler(lbPaths_DragOver);
			labelWatchedFolders.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			labelWatchedFolders.Location = new System.Drawing.Point(9, 163);
			labelWatchedFolders.Name = "labelWatchedFolders";
			labelWatchedFolders.Size = new System.Drawing.Size(480, 26);
			labelWatchedFolders.TabIndex = 0;
			labelWatchedFolders.Text = "Checked folders will be watched for changes (rename, move) while the program is running.";
			labelWatchedFolders.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btRemoveFolder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btRemoveFolder.Location = new System.Drawing.Point(400, 95);
			btRemoveFolder.Name = "btRemoveFolder";
			btRemoveFolder.Size = new System.Drawing.Size(89, 23);
			btRemoveFolder.TabIndex = 3;
			btRemoveFolder.Text = "&Remove";
			btRemoveFolder.UseVisualStyleBackColor = true;
			btRemoveFolder.Click += new System.EventHandler(btRemoveFolder_Click);
			btAddFolder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btAddFolder.Location = new System.Drawing.Point(400, 37);
			btAddFolder.Name = "btAddFolder";
			btAddFolder.Size = new System.Drawing.Size(89, 23);
			btAddFolder.TabIndex = 1;
			btAddFolder.Text = "&Add...";
			btAddFolder.UseVisualStyleBackColor = true;
			btAddFolder.Click += new System.EventHandler(btAddFolder_Click);
			memCacheUpate.Enabled = true;
			memCacheUpate.Interval = 1000;
			memCacheUpate.Tick += new System.EventHandler(memCacheUpate_Tick);
			pageScripts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			pageScripts.AutoScroll = true;
			pageScripts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			pageScripts.Controls.Add(grpScriptSettings);
			pageScripts.Controls.Add(grpScripts);
			pageScripts.Controls.Add(grpPackages);
			pageScripts.Location = new System.Drawing.Point(84, 8);
			pageScripts.Name = "pageScripts";
			pageScripts.Size = new System.Drawing.Size(517, 408);
			pageScripts.TabIndex = 11;
			grpScriptSettings.Controls.Add(btAddLibraryFolder);
			grpScriptSettings.Controls.Add(chkDisableScripting);
			grpScriptSettings.Controls.Add(labelScriptPaths);
			grpScriptSettings.Controls.Add(txLibraries);
			grpScriptSettings.Dock = System.Windows.Forms.DockStyle.Top;
			grpScriptSettings.Location = new System.Drawing.Point(0, 752);
			grpScriptSettings.Name = "grpScriptSettings";
			grpScriptSettings.Size = new System.Drawing.Size(498, 192);
			grpScriptSettings.TabIndex = 5;
			grpScriptSettings.Text = "Script Settings";
			btAddLibraryFolder.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btAddLibraryFolder.Location = new System.Drawing.Point(369, 163);
			btAddLibraryFolder.Name = "btAddLibraryFolder";
			btAddLibraryFolder.Size = new System.Drawing.Size(121, 23);
			btAddLibraryFolder.TabIndex = 3;
			btAddLibraryFolder.Text = "Add Folder...";
			btAddLibraryFolder.UseVisualStyleBackColor = true;
			btAddLibraryFolder.Click += new System.EventHandler(btAddLibraryFolder_Click);
			chkDisableScripting.AutoSize = true;
			chkDisableScripting.Location = new System.Drawing.Point(9, 39);
			chkDisableScripting.Name = "chkDisableScripting";
			chkDisableScripting.Size = new System.Drawing.Size(109, 17);
			chkDisableScripting.TabIndex = 0;
			chkDisableScripting.Text = "Disable all Scripts";
			chkDisableScripting.UseVisualStyleBackColor = true;
			labelScriptPaths.Location = new System.Drawing.Point(6, 60);
			labelScriptPaths.Name = "labelScriptPaths";
			labelScriptPaths.Size = new System.Drawing.Size(478, 29);
			labelScriptPaths.TabIndex = 1;
			labelScriptPaths.Text = "Semicolon separated list of library paths for scripts (e.g. python libraries):";
			labelScriptPaths.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			txLibraries.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txLibraries.Location = new System.Drawing.Point(7, 92);
			txLibraries.Multiline = true;
			txLibraries.Name = "txLibraries";
			txLibraries.Size = new System.Drawing.Size(482, 63);
			txLibraries.TabIndex = 2;
			grpScripts.Controls.Add(chkHideSampleScripts);
			grpScripts.Controls.Add(btConfigScript);
			grpScripts.Controls.Add(lvScripts);
			grpScripts.Dock = System.Windows.Forms.DockStyle.Top;
			grpScripts.Location = new System.Drawing.Point(0, 378);
			grpScripts.Name = "grpScripts";
			grpScripts.Size = new System.Drawing.Size(498, 374);
			grpScripts.TabIndex = 4;
			grpScripts.Text = "Available Scripts";
			chkHideSampleScripts.AutoSize = true;
			chkHideSampleScripts.Location = new System.Drawing.Point(9, 345);
			chkHideSampleScripts.Name = "chkHideSampleScripts";
			chkHideSampleScripts.Size = new System.Drawing.Size(119, 17);
			chkHideSampleScripts.TabIndex = 8;
			chkHideSampleScripts.Text = "Hide sample Scripts";
			chkHideSampleScripts.UseVisualStyleBackColor = true;
			chkHideSampleScripts.CheckedChanged += new System.EventHandler(chkHideSampleScripts_CheckedChanged);
			btConfigScript.Enabled = false;
			btConfigScript.Location = new System.Drawing.Point(398, 339);
			btConfigScript.Name = "btConfigScript";
			btConfigScript.Size = new System.Drawing.Size(87, 23);
			btConfigScript.TabIndex = 7;
			btConfigScript.Text = "Configure...";
			btConfigScript.UseVisualStyleBackColor = true;
			btConfigScript.Click += new System.EventHandler(btConfigScript_Click);
			lvScripts.CheckBoxes = true;
			lvScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2]
			{
				chScriptName,
				chScriptPackage
			});
			lvScripts.FullRowSelect = true;
			lvScripts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			lvScripts.Location = new System.Drawing.Point(9, 42);
			lvScripts.MultiSelect = false;
			lvScripts.Name = "lvScripts";
			lvScripts.ShowItemToolTips = true;
			lvScripts.Size = new System.Drawing.Size(476, 291);
			lvScripts.SmallImageList = imageList;
			lvScripts.Sorting = System.Windows.Forms.SortOrder.Ascending;
			lvScripts.TabIndex = 6;
			lvScripts.UseCompatibleStateImageBehavior = false;
			lvScripts.View = System.Windows.Forms.View.Details;
			lvScripts.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(lvScripts_ItemChecked);
			lvScripts.SelectedIndexChanged += new System.EventHandler(lvScripts_SelectedIndexChanged);
			chScriptName.Text = "Name";
			chScriptName.Width = 250;
			chScriptPackage.Text = "Package";
			chScriptPackage.Width = 190;
			grpPackages.Controls.Add(btRemovePackage);
			grpPackages.Controls.Add(btInstallPackage);
			grpPackages.Controls.Add(lvPackages);
			grpPackages.Dock = System.Windows.Forms.DockStyle.Top;
			grpPackages.Location = new System.Drawing.Point(0, 0);
			grpPackages.Name = "grpPackages";
			grpPackages.Size = new System.Drawing.Size(498, 378);
			grpPackages.TabIndex = 13;
			grpPackages.Text = "Script Packages";
			btRemovePackage.Location = new System.Drawing.Point(398, 344);
			btRemovePackage.Name = "btRemovePackage";
			btRemovePackage.Size = new System.Drawing.Size(86, 23);
			btRemovePackage.TabIndex = 2;
			btRemovePackage.Text = "Remove";
			btRemovePackage.UseVisualStyleBackColor = true;
			btRemovePackage.Click += new System.EventHandler(btRemovePackage_Click);
			btInstallPackage.Location = new System.Drawing.Point(306, 344);
			btInstallPackage.Name = "btInstallPackage";
			btInstallPackage.Size = new System.Drawing.Size(86, 23);
			btInstallPackage.TabIndex = 1;
			btInstallPackage.Text = "Install...";
			btInstallPackage.UseVisualStyleBackColor = true;
			btInstallPackage.Click += new System.EventHandler(btInstallPackage_Click);
			lvPackages.AllowDrop = true;
			lvPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[3]
			{
				chPackageName,
				chPackageAuthor,
				chPackageDescription
			});
			listViewGroup.Header = "Installed";
			listViewGroup.Name = "packageGroupInstalled";
			listViewGroup2.Header = "To be removed (requires restart)";
			listViewGroup2.Name = "packageGroupRemove";
			listViewGroup3.Header = "To be installed (requires restart)";
			listViewGroup3.Name = "packageGroupInstall";
			lvPackages.Groups.AddRange(new System.Windows.Forms.ListViewGroup[3]
			{
				listViewGroup,
				listViewGroup2,
				listViewGroup3
			});
			lvPackages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			lvPackages.LargeImageList = packageImageList;
			lvPackages.Location = new System.Drawing.Point(16, 37);
			lvPackages.Name = "lvPackages";
			lvPackages.ShowItemToolTips = true;
			lvPackages.Size = new System.Drawing.Size(468, 301);
			lvPackages.SmallImageList = packageImageList;
			lvPackages.Sorting = System.Windows.Forms.SortOrder.Ascending;
			lvPackages.TabIndex = 0;
			lvPackages.UseCompatibleStateImageBehavior = false;
			lvPackages.View = System.Windows.Forms.View.Details;
			lvPackages.DragDrop += new System.Windows.Forms.DragEventHandler(lvPackages_DragDrop);
			lvPackages.DragOver += new System.Windows.Forms.DragEventHandler(lvPackages_DragOver);
			lvPackages.DoubleClick += new System.EventHandler(lvPackages_DoubleClick);
			chPackageName.Text = "Package";
			chPackageName.Width = 130;
			chPackageAuthor.Text = "Author";
			chPackageAuthor.Width = 89;
			chPackageDescription.Text = "Description";
			chPackageDescription.Width = 217;
			packageImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			packageImageList.ImageSize = new System.Drawing.Size(32, 32);
			packageImageList.TransparentColor = System.Drawing.Color.Transparent;
			tabReader.Appearance = System.Windows.Forms.Appearance.Button;
			tabReader.AutoEllipsis = true;
			tabReader.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ReaderPref;
			tabReader.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			tabReader.Location = new System.Drawing.Point(3, 7);
			tabReader.Name = "tabReader";
			tabReader.Size = new System.Drawing.Size(75, 56);
			tabReader.TabIndex = 13;
			tabReader.Text = "Reader";
			tabReader.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			tabReader.UseVisualStyleBackColor = true;
			tabReader.CheckedChanged += new System.EventHandler(chkAdvanced_CheckedChanged);
			tabLibraries.Appearance = System.Windows.Forms.Appearance.Button;
			tabLibraries.AutoEllipsis = true;
			tabLibraries.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.LibraryPref;
			tabLibraries.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			tabLibraries.Location = new System.Drawing.Point(3, 66);
			tabLibraries.Name = "tabLibraries";
			tabLibraries.Size = new System.Drawing.Size(75, 56);
			tabLibraries.TabIndex = 14;
			tabLibraries.Text = "Libraries";
			tabLibraries.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			tabLibraries.UseVisualStyleBackColor = true;
			tabLibraries.CheckedChanged += new System.EventHandler(chkAdvanced_CheckedChanged);
			tabBehavior.Appearance = System.Windows.Forms.Appearance.Button;
			tabBehavior.AutoEllipsis = true;
			tabBehavior.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.BehaviorPref;
			tabBehavior.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			tabBehavior.Location = new System.Drawing.Point(3, 126);
			tabBehavior.Name = "tabBehavior";
			tabBehavior.Size = new System.Drawing.Size(75, 56);
			tabBehavior.TabIndex = 15;
			tabBehavior.Text = "Behavior";
			tabBehavior.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			tabBehavior.UseVisualStyleBackColor = true;
			tabBehavior.CheckedChanged += new System.EventHandler(chkAdvanced_CheckedChanged);
			tabScripts.Appearance = System.Windows.Forms.Appearance.Button;
			tabScripts.AutoEllipsis = true;
			tabScripts.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ScriptingPref;
			tabScripts.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			tabScripts.Location = new System.Drawing.Point(3, 187);
			tabScripts.Name = "tabScripts";
			tabScripts.Size = new System.Drawing.Size(75, 56);
			tabScripts.TabIndex = 16;
			tabScripts.Text = "Scripts";
			tabScripts.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			tabScripts.UseVisualStyleBackColor = true;
			tabScripts.CheckedChanged += new System.EventHandler(chkAdvanced_CheckedChanged);
			tabAdvanced.Appearance = System.Windows.Forms.Appearance.Button;
			tabAdvanced.AutoEllipsis = true;
			tabAdvanced.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.AdvancedPref;
			tabAdvanced.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			tabAdvanced.Location = new System.Drawing.Point(3, 248);
			tabAdvanced.Name = "tabAdvanced";
			tabAdvanced.Size = new System.Drawing.Size(75, 56);
			tabAdvanced.TabIndex = 17;
			tabAdvanced.Text = "Advanced";
			tabAdvanced.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			tabAdvanced.UseVisualStyleBackColor = true;
			tabAdvanced.CheckedChanged += new System.EventHandler(chkAdvanced_CheckedChanged);
			btResetTwitter.Location = new System.Drawing.Point(382, 66);
			btResetTwitter.Name = "btResetTwitter";
			btResetTwitter.Size = new System.Drawing.Size(104, 23);
			btResetTwitter.TabIndex = 3;
			btResetTwitter.Text = "Reset";
			btResetTwitter.UseVisualStyleBackColor = true;
			btResetTwitter.Click += new System.EventHandler(btResetTwitter_Click);
			labelResetTwitter.Location = new System.Drawing.Point(6, 71);
			labelResetTwitter.Name = "labelResetTwitter";
			labelResetTwitter.Size = new System.Drawing.Size(370, 17);
			labelResetTwitter.TabIndex = 2;
			labelResetTwitter.Text = "To reset the Twitter authorization, press";
			base.AcceptButton = btOK;
			AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(610, 453);
			base.Controls.Add(pageAdvanced);
			base.Controls.Add(tabAdvanced);
			base.Controls.Add(tabScripts);
			base.Controls.Add(tabBehavior);
			base.Controls.Add(tabLibraries);
			base.Controls.Add(tabReader);
			base.Controls.Add(pageReader);
			base.Controls.Add(pageLibrary);
			base.Controls.Add(pageScripts);
			base.Controls.Add(pageBehavior);
			base.Controls.Add(btApply);
			base.Controls.Add(btCancel);
			base.Controls.Add(btOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "PreferencesDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Preferences";
			pageReader.ResumeLayout(false);
			groupHardwareAcceleration.ResumeLayout(false);
			groupHardwareAcceleration.PerformLayout();
			grpMouse.ResumeLayout(false);
			grpMouse.PerformLayout();
			groupOverlays.ResumeLayout(false);
			groupOverlays.PerformLayout();
			panelReaderOverlays.ResumeLayout(false);
			grpKeyboard.ResumeLayout(false);
			cmKeyboardLayout.ResumeLayout(false);
			grpDisplay.ResumeLayout(false);
			grpDisplay.PerformLayout();
			pageAdvanced.ResumeLayout(false);
			grpWirelessSetup.ResumeLayout(false);
			grpWirelessSetup.PerformLayout();
			grpIntegration.ResumeLayout(false);
			grpIntegration.PerformLayout();
			groupMessagesAndSocial.ResumeLayout(false);
			groupMemory.ResumeLayout(false);
			grpMaximumMemoryUsage.ResumeLayout(false);
			grpMaximumMemoryUsage.PerformLayout();
			grpMemoryCache.ResumeLayout(false);
			grpMemoryCache.PerformLayout();
			((System.ComponentModel.ISupportInitialize)numMemPageCount).EndInit();
			((System.ComponentModel.ISupportInitialize)numMemThumbSize).EndInit();
			grpDiskCache.ResumeLayout(false);
			grpDiskCache.PerformLayout();
			((System.ComponentModel.ISupportInitialize)numPageCacheSize).EndInit();
			((System.ComponentModel.ISupportInitialize)numInternetCacheSize).EndInit();
			((System.ComponentModel.ISupportInitialize)numThumbnailCacheSize).EndInit();
			grpDatabaseBackup.ResumeLayout(false);
			groupOtherComics.ResumeLayout(false);
			groupOtherComics.PerformLayout();
			grpLanguages.ResumeLayout(false);
			pageLibrary.ResumeLayout(false);
			grpServerSettings.ResumeLayout(false);
			grpServerSettings.PerformLayout();
			grpSharing.ResumeLayout(false);
			grpSharing.PerformLayout();
			groupLibraryDisplay.ResumeLayout(false);
			groupLibraryDisplay.PerformLayout();
			grpScanning.ResumeLayout(false);
			grpScanning.PerformLayout();
			groupComicFolders.ResumeLayout(false);
			pageScripts.ResumeLayout(false);
			grpScriptSettings.ResumeLayout(false);
			grpScriptSettings.PerformLayout();
			grpScripts.ResumeLayout(false);
			grpScripts.PerformLayout();
			grpPackages.ResumeLayout(false);
			ResumeLayout(false);
		}
		
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				activeTab = tabButtons.FindIndex((CheckBox c) => c.Checked);
				Program.Scanner.ScanNotify -= DatabaseScanNotify;
				IdleProcess.Idle -= ApplicationIdle;
				Program.InternetCache.SizeChanged -= UpdateDiskCacheStatus;
				Program.ImagePool.Pages.DiskCache.SizeChanged -= UpdateDiskCacheStatus;
				Program.ImagePool.Thumbs.DiskCache.SizeChanged -= UpdateDiskCacheStatus;
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private CheckBox chkHighQualityDisplay;

		private CheckBox chkEnableThumbnailCache;

		private NumericUpDown numThumbnailCacheSize;

		private Button btClearThumbnailCache;

		private Button btRemoveFolder;

		private Button btAddFolder;

		private CheckedListBoxEx lbPaths;

		private Button btOK;

		private Button btCancel;

		private Label lblScan;

		private Button btScan;

		private CheckBox chkAutoRemoveMissing;

		private Button btResetMessages;

		private Label labelReshowHidden;

		private Label labelWatchedFolders;

		private CheckedListBox lbFormats;

		private Label labelCheckedFormats;

		private CheckBox chkOverwriteAssociations;

		private CheckBox chkEnablePageCache;

		private Button btClearPageCache;

		private NumericUpDown numPageCacheSize;

		private CheckBox chkLookForShared;

		private Button btChangeFolder;

		private CheckBox chkAutoUpdateComicFiles;

		private TrackBarLite tbContrast;

		private TrackBarLite tbBrightness;

		private TrackBarLite tbSaturation;

		private Label labelContrast;

		private Label labelBrightness;

		private Label labelSaturation;

		private Button btApply;

		private CheckBox chkAutoContrast;

		private TextBox txCoverFilter;

		private Label labelExcludeCover;

		private ImageList imageList;

		private IContainer components;

		private Button btResetColor;

		private TrackBarLite tbOverlayScaling;

		private Label labelOverlaySize;

		private ToolTip toolTip;

		private ListBox lbLanguages;

		private Label labelLanguage;

		private KeyboardShortcutEditor keyboardShortcutEditor;

		private Label labelMemThumbSize;

		private NumericUpDown numMemPageCount;

		private Label labelMemPageCount;

		private NumericUpDown numMemThumbSize;

		private CheckBox chkMemPageOptimized;

		private CheckBox chkMemThumbOptimized;

		private Button btBackupDatabase;

		private Button btRestoreDatabase;

		private Button btTranslate;

		private CheckBox chkEnableHardware;

		private CheckBox chkShowStatusOverlay;

		private CheckBox chkShowNavigationOverlay;

		private CheckBox chkShowVisiblePartOverlay;

		private CheckBox chkShowCurrentPageOverlay;

		private CheckBox chkEnableDisplayChangeAnimation;

		private CheckBox chkEnableInertialMouseScrolling;

		private Panel pageBehavior;

		private Label lblMouseWheel;

		private Label lblFast;

		private Label lblSlow;

		private TrackBarLite tbMouseWheel;

		private Label lblPageCacheUsage;

		private Label lblThumbCacheUsage;

		private CheckBox chkUpdateComicFiles;

		private CheckBox chkAnamorphicScaling;

		private Panel pageReader;

		private CollapsibleGroupBox groupOverlays;

		private CollapsibleGroupBox grpDisplay;

		private CollapsibleGroupBox grpMouse;

		private CollapsibleGroupBox grpKeyboard;

		private CollapsibleGroupBox groupHardwareAcceleration;

		private Panel pageAdvanced;

		private CollapsibleGroupBox grpLanguages;

		private CollapsibleGroupBox groupMessagesAndSocial;

		private CollapsibleGroupBox groupOtherComics;

		private CollapsibleGroupBox grpDatabaseBackup;

		private CollapsibleGroupBox groupMemory;

		private CollapsibleGroupBox grpIntegration;

		private Panel pageLibrary;

		private CollapsibleGroupBox grpSharing;

		private CollapsibleGroupBox groupComicFolders;

		private CollapsibleGroupBox grpScanning;

		private CheckBox chkEnableSoftwareFiltering;

		private Label lblPageMemCacheUsage;

		private Label lblThumbMemCacheUsage;

		private Timer memCacheUpate;

		private CheckBox chkShowPageNames;

		private CheckBox chkEnableHardwareFiltering;

		private Panel pageScripts;

		private CollapsibleGroupBox grpScripts;

		private ListView lvScripts;

		private ColumnHeader chScriptName;

		private ColumnHeader chScriptPackage;

		private CheckBox chkDisableScripting;

		private CollapsibleGroupBox grpScriptSettings;

		private Button btAddLibraryFolder;

		private Label labelScriptPaths;

		private TextBox txLibraries;

		private CollapsibleGroupBox grpPackages;

		private Button btRemovePackage;

		private Button btInstallPackage;

		private ListView lvPackages;

		private ImageList packageImageList;

		private ColumnHeader chPackageName;

		private ColumnHeader chPackageAuthor;

		private ColumnHeader chPackageDescription;

		private Button btAssociateExtensions;

		private Label lblInternetCacheUsage;

		private CheckBox chkEnableInternetCache;

		private NumericUpDown numInternetCacheSize;

		private Button btClearInternetCache;

		private CollapsibleGroupBox grpServerSettings;

		private TextBox txPublicServerAddress;

		private Label labelPublicServerAddress;

		private TabControl tabShares;

		private Button btRemoveShare;

		private Button btAddShare;

		private PasswordTextBox txPrivateListingPassword;

		private Label labelPrivateListPassword;

		private Label labelSharpening;

		private TrackBarLite tbSharpening;

		private CheckBox chkDontAddRemovedFiles;

		private Button btConfigScript;

		private Button btOpenFolder;

		private CheckBox chkAutoConnectShares;

		private Button btExportKeyboard;

		private SplitButton btImportKeyboard;

		private ContextMenuStrip cmKeyboardLayout;

		private ToolStripMenuItem miDefaultKeyboardLayout;

		private ToolStripSeparator toolStripMenuItem1;

		private ComboBox cbNavigationOverlayPosition;

		private Label labelNavigationOverlayPosition;

		private Panel panelReaderOverlays;

		private Label labelVisiblePartOverlay;

		private Label labelNavigationOverlay;

		private Label labelStatusOverlay;

		private Label labelPageOverlay;

		private CheckBox chkHideSampleScripts;

		private CheckBox chkSmoothAutoScrolling;

		private TrackBarLite tbGamma;

		private Label labelGamma;

		private GroupBox grpDiskCache;

		private GroupBox grpMaximumMemoryUsage;

		private Label lblMaximumMemoryUsageValue;

		private TrackBarLite tbMaximumMemoryUsage;

		private Label lblMaximumMemoryUsage;

		private GroupBox grpMemoryCache;

		private CollapsibleGroupBox groupLibraryDisplay;

		private CheckBox chkLibraryGaugesTotal;

		private CheckBox chkLibraryGaugesUnread;

		private CheckBox chkLibraryGaugesNumeric;

		private CheckBox chkLibraryGaugesNew;

		private CheckBox chkLibraryGauges;

		private CheckBox tabReader;

		private CheckBox tabLibraries;

		private CheckBox tabBehavior;

		private CheckBox tabScripts;

		private CheckBox tabAdvanced;

		private CollapsibleGroupBox grpWirelessSetup;

		private Label lblWifiStatus;

		private Label lblWifiAddresses;

		private TextBox txWifiAddresses;

		private Button btTestWifi;

		private Button btResetTwitter;

		private Label labelResetTwitter;

		private static int activeTab = -1;

		private readonly List<CheckBox> tabButtons = new List<CheckBox>();
	}
}
