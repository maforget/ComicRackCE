using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class PreferencesDialog
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesDialog));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Installed", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("To be removed (requires restart)", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("To be installed (requires restart)", System.Windows.Forms.HorizontalAlignment.Left);
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btApply = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pageBehavior = new System.Windows.Forms.Panel();
            this.pageReader = new System.Windows.Forms.Panel();
            this.groupHardwareAcceleration = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.chkEnableHardwareFiltering = new System.Windows.Forms.CheckBox();
            this.chkEnableSoftwareFiltering = new System.Windows.Forms.CheckBox();
            this.chkEnableHardware = new System.Windows.Forms.CheckBox();
            this.chkEnableDisplayChangeAnimation = new System.Windows.Forms.CheckBox();
            this.grpMouse = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.chkSmoothAutoScrolling = new System.Windows.Forms.CheckBox();
            this.lblFast = new System.Windows.Forms.Label();
            this.lblMouseWheel = new System.Windows.Forms.Label();
            this.chkEnableInertialMouseScrolling = new System.Windows.Forms.CheckBox();
            this.lblSlow = new System.Windows.Forms.Label();
            this.tbMouseWheel = new cYo.Common.Windows.Forms.TrackBarLite();
            this.groupOverlays = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.panelReaderOverlays = new System.Windows.Forms.Panel();
            this.labelVisiblePartOverlay = new System.Windows.Forms.Label();
            this.labelNavigationOverlay = new System.Windows.Forms.Label();
            this.labelStatusOverlay = new System.Windows.Forms.Label();
            this.labelPageOverlay = new System.Windows.Forms.Label();
            this.cbNavigationOverlayPosition = new System.Windows.Forms.ComboBox();
            this.labelNavigationOverlayPosition = new System.Windows.Forms.Label();
            this.chkShowPageNames = new System.Windows.Forms.CheckBox();
            this.tbOverlayScaling = new cYo.Common.Windows.Forms.TrackBarLite();
            this.chkShowCurrentPageOverlay = new System.Windows.Forms.CheckBox();
            this.chkShowStatusOverlay = new System.Windows.Forms.CheckBox();
            this.chkShowVisiblePartOverlay = new System.Windows.Forms.CheckBox();
            this.chkShowNavigationOverlay = new System.Windows.Forms.CheckBox();
            this.labelOverlaySize = new System.Windows.Forms.Label();
            this.grpKeyboard = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.btExportKeyboard = new System.Windows.Forms.Button();
            this.btImportKeyboard = new cYo.Common.Windows.Forms.SplitButton();
            this.cmKeyboardLayout = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miDefaultKeyboardLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.keyboardShortcutEditor = new cYo.Common.Windows.Forms.KeyboardShortcutEditor();
            this.grpDisplay = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.tbGamma = new cYo.Common.Windows.Forms.TrackBarLite();
            this.labelGamma = new System.Windows.Forms.Label();
            this.chkAnamorphicScaling = new System.Windows.Forms.CheckBox();
            this.chkHighQualityDisplay = new System.Windows.Forms.CheckBox();
            this.labelSharpening = new System.Windows.Forms.Label();
            this.tbSharpening = new cYo.Common.Windows.Forms.TrackBarLite();
            this.btResetColor = new System.Windows.Forms.Button();
            this.chkAutoContrast = new System.Windows.Forms.CheckBox();
            this.labelSaturation = new System.Windows.Forms.Label();
            this.tbSaturation = new cYo.Common.Windows.Forms.TrackBarLite();
            this.labelBrightness = new System.Windows.Forms.Label();
            this.tbBrightness = new cYo.Common.Windows.Forms.TrackBarLite();
            this.tbContrast = new cYo.Common.Windows.Forms.TrackBarLite();
            this.labelContrast = new System.Windows.Forms.Label();
            this.pageAdvanced = new System.Windows.Forms.Panel();
            this.grpWirelessSetup = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.btTestWifi = new System.Windows.Forms.Button();
            this.lblWifiStatus = new System.Windows.Forms.Label();
            this.lblWifiAddresses = new System.Windows.Forms.Label();
            this.txWifiAddresses = new System.Windows.Forms.TextBox();
            this.grpIntegration = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.btAssociateExtensions = new System.Windows.Forms.Button();
            this.labelCheckedFormats = new System.Windows.Forms.Label();
            this.chkOverwriteAssociations = new System.Windows.Forms.CheckBox();
            this.lbFormats = new System.Windows.Forms.CheckedListBox();
            this.groupMessagesAndSocial = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.btResetMessages = new System.Windows.Forms.Button();
            this.labelReshowHidden = new System.Windows.Forms.Label();
            this.groupMemory = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.grpMaximumMemoryUsage = new System.Windows.Forms.GroupBox();
            this.lblMaximumMemoryUsageValue = new System.Windows.Forms.Label();
            this.tbMaximumMemoryUsage = new cYo.Common.Windows.Forms.TrackBarLite();
            this.lblMaximumMemoryUsage = new System.Windows.Forms.Label();
            this.grpMemoryCache = new System.Windows.Forms.GroupBox();
            this.lblPageMemCacheUsage = new System.Windows.Forms.Label();
            this.labelMemThumbSize = new System.Windows.Forms.Label();
            this.lblThumbMemCacheUsage = new System.Windows.Forms.Label();
            this.numMemPageCount = new System.Windows.Forms.NumericUpDown();
            this.labelMemPageCount = new System.Windows.Forms.Label();
            this.chkMemPageOptimized = new System.Windows.Forms.CheckBox();
            this.chkMemThumbOptimized = new System.Windows.Forms.CheckBox();
            this.numMemThumbSize = new System.Windows.Forms.NumericUpDown();
            this.grpDiskCache = new System.Windows.Forms.GroupBox();
            this.chkEnableInternetCache = new System.Windows.Forms.CheckBox();
            this.lblInternetCacheUsage = new System.Windows.Forms.Label();
            this.btClearPageCache = new System.Windows.Forms.Button();
            this.numPageCacheSize = new System.Windows.Forms.NumericUpDown();
            this.numInternetCacheSize = new System.Windows.Forms.NumericUpDown();
            this.btClearThumbnailCache = new System.Windows.Forms.Button();
            this.btClearInternetCache = new System.Windows.Forms.Button();
            this.chkEnablePageCache = new System.Windows.Forms.CheckBox();
            this.lblPageCacheUsage = new System.Windows.Forms.Label();
            this.numThumbnailCacheSize = new System.Windows.Forms.NumericUpDown();
            this.chkEnableThumbnailCache = new System.Windows.Forms.CheckBox();
            this.lblThumbCacheUsage = new System.Windows.Forms.Label();
            this.grpDatabaseBackup = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.btRestoreDatabase = new System.Windows.Forms.Button();
            this.btBackupDatabase = new System.Windows.Forms.Button();
            this.groupOtherComics = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.chkUpdateComicFiles = new System.Windows.Forms.CheckBox();
            this.labelExcludeCover = new System.Windows.Forms.Label();
            this.chkAutoUpdateComicFiles = new System.Windows.Forms.CheckBox();
            this.txCoverFilter = new System.Windows.Forms.TextBox();
            this.grpLanguages = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.btTranslate = new System.Windows.Forms.Button();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.lbLanguages = new System.Windows.Forms.ListBox();
            this.pageLibrary = new System.Windows.Forms.Panel();
            this.grpVirtualTags = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.grpVtagConfig = new System.Windows.Forms.GroupBox();
            this.lblCaptionFormat = new System.Windows.Forms.Label();
            this.txtCaptionFormat = new System.Windows.Forms.TextBox();
            this.btInsertValue = new System.Windows.Forms.Button();
            this.lblVirtualTagDescription = new System.Windows.Forms.Label();
            this.lblVirtualTagName = new System.Windows.Forms.Label();
            this.txtVirtualTagDescription = new System.Windows.Forms.TextBox();
            this.txtVirtualTagName = new System.Windows.Forms.TextBox();
            this.chkVirtualTagEnable = new System.Windows.Forms.CheckBox();
            this.lblCaptionText = new System.Windows.Forms.Label();
            this.lblCaptionSuffix = new System.Windows.Forms.Label();
            this.rtfVirtualTagCaption = new System.Windows.Forms.RichTextBox();
            this.lblFieldConfig = new System.Windows.Forms.Label();
            this.txtCaptionPrefix = new System.Windows.Forms.TextBox();
            this.lblCaptionPrefix = new System.Windows.Forms.Label();
            this.btnCaptionInsert = new System.Windows.Forms.Button();
            this.txtCaptionSuffix = new System.Windows.Forms.TextBox();
            this.lblVirtualTags = new System.Windows.Forms.Label();
            this.cbVirtualTags = new System.Windows.Forms.ComboBox();
            this.grpServerSettings = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.txPrivateListingPassword = new cYo.Common.Windows.Forms.PasswordTextBox();
            this.labelPrivateListPassword = new System.Windows.Forms.Label();
            this.labelPublicServerAddress = new System.Windows.Forms.Label();
            this.txPublicServerAddress = new System.Windows.Forms.TextBox();
            this.grpSharing = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.chkAutoConnectShares = new System.Windows.Forms.CheckBox();
            this.btRemoveShare = new System.Windows.Forms.Button();
            this.btAddShare = new System.Windows.Forms.Button();
            this.tabShares = new System.Windows.Forms.TabControl();
            this.chkLookForShared = new System.Windows.Forms.CheckBox();
            this.groupLibraryDisplay = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.chkLibraryGaugesTotal = new System.Windows.Forms.CheckBox();
            this.chkLibraryGaugesUnread = new System.Windows.Forms.CheckBox();
            this.chkLibraryGaugesNumeric = new System.Windows.Forms.CheckBox();
            this.chkLibraryGaugesNew = new System.Windows.Forms.CheckBox();
            this.chkLibraryGauges = new System.Windows.Forms.CheckBox();
            this.grpScanning = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.chkDontAddRemovedFiles = new System.Windows.Forms.CheckBox();
            this.chkAutoRemoveMissing = new System.Windows.Forms.CheckBox();
            this.lblScan = new System.Windows.Forms.Label();
            this.btScan = new System.Windows.Forms.Button();
            this.groupComicFolders = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.btOpenFolder = new System.Windows.Forms.Button();
            this.btChangeFolder = new System.Windows.Forms.Button();
            this.lbPaths = new cYo.Common.Windows.Forms.CheckedListBoxEx();
            this.labelWatchedFolders = new System.Windows.Forms.Label();
            this.btRemoveFolder = new System.Windows.Forms.Button();
            this.btAddFolder = new System.Windows.Forms.Button();
            this.memCacheUpate = new System.Windows.Forms.Timer(this.components);
            this.pageScripts = new System.Windows.Forms.Panel();
            this.grpScriptSettings = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.btAddLibraryFolder = new System.Windows.Forms.Button();
            this.chkDisableScripting = new System.Windows.Forms.CheckBox();
            this.labelScriptPaths = new System.Windows.Forms.Label();
            this.txLibraries = new System.Windows.Forms.TextBox();
            this.grpScripts = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.chkHideSampleScripts = new System.Windows.Forms.CheckBox();
            this.btConfigScript = new System.Windows.Forms.Button();
            this.lvScripts = new System.Windows.Forms.ListView();
            this.chScriptName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chScriptPackage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpPackages = new cYo.Common.Windows.Forms.CollapsibleGroupBox();
            this.btRemovePackage = new System.Windows.Forms.Button();
            this.btInstallPackage = new System.Windows.Forms.Button();
            this.lvPackages = new System.Windows.Forms.ListView();
            this.chPackageName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPackageAuthor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPackageDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.packageImageList = new System.Windows.Forms.ImageList(this.components);
            this.tabReader = new System.Windows.Forms.CheckBox();
            this.tabLibraries = new System.Windows.Forms.CheckBox();
            this.tabBehavior = new System.Windows.Forms.CheckBox();
            this.tabScripts = new System.Windows.Forms.CheckBox();
            this.tabAdvanced = new System.Windows.Forms.CheckBox();
            this.pageReader.SuspendLayout();
            this.groupHardwareAcceleration.SuspendLayout();
            this.grpMouse.SuspendLayout();
            this.groupOverlays.SuspendLayout();
            this.panelReaderOverlays.SuspendLayout();
            this.grpKeyboard.SuspendLayout();
            this.cmKeyboardLayout.SuspendLayout();
            this.grpDisplay.SuspendLayout();
            this.pageAdvanced.SuspendLayout();
            this.grpWirelessSetup.SuspendLayout();
            this.grpIntegration.SuspendLayout();
            this.groupMessagesAndSocial.SuspendLayout();
            this.groupMemory.SuspendLayout();
            this.grpMaximumMemoryUsage.SuspendLayout();
            this.grpMemoryCache.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMemPageCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMemThumbSize)).BeginInit();
            this.grpDiskCache.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageCacheSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInternetCacheSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThumbnailCacheSize)).BeginInit();
            this.grpDatabaseBackup.SuspendLayout();
            this.groupOtherComics.SuspendLayout();
            this.grpLanguages.SuspendLayout();
            this.pageLibrary.SuspendLayout();
            this.grpVirtualTags.SuspendLayout();
            this.grpVtagConfig.SuspendLayout();
            this.grpServerSettings.SuspendLayout();
            this.grpSharing.SuspendLayout();
            this.groupLibraryDisplay.SuspendLayout();
            this.grpScanning.SuspendLayout();
            this.groupComicFolders.SuspendLayout();
            this.pageScripts.SuspendLayout();
            this.grpScriptSettings.SuspendLayout();
            this.grpScripts.SuspendLayout();
            this.grpPackages.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btOK.Location = new System.Drawing.Point(351, 422);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(80, 24);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "&OK";
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btCancel.Location = new System.Drawing.Point(437, 422);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(80, 24);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "&Cancel";
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btApply
            // 
            this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btApply.Location = new System.Drawing.Point(523, 422);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(80, 24);
            this.btApply.TabIndex = 3;
            this.btApply.Text = "&Apply";
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // pageBehavior
            // 
            this.pageBehavior.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pageBehavior.AutoScroll = true;
            this.pageBehavior.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageBehavior.Location = new System.Drawing.Point(84, 8);
            this.pageBehavior.Name = "pageBehavior";
            this.pageBehavior.Size = new System.Drawing.Size(517, 408);
            this.pageBehavior.TabIndex = 6;
            // 
            // pageReader
            // 
            this.pageReader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pageReader.AutoScroll = true;
            this.pageReader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageReader.Controls.Add(this.groupHardwareAcceleration);
            this.pageReader.Controls.Add(this.grpMouse);
            this.pageReader.Controls.Add(this.groupOverlays);
            this.pageReader.Controls.Add(this.grpKeyboard);
            this.pageReader.Controls.Add(this.grpDisplay);
            this.pageReader.Location = new System.Drawing.Point(84, 8);
            this.pageReader.Name = "pageReader";
            this.pageReader.Size = new System.Drawing.Size(517, 408);
            this.pageReader.TabIndex = 8;
            // 
            // groupHardwareAcceleration
            // 
            this.groupHardwareAcceleration.Controls.Add(this.chkEnableHardwareFiltering);
            this.groupHardwareAcceleration.Controls.Add(this.chkEnableSoftwareFiltering);
            this.groupHardwareAcceleration.Controls.Add(this.chkEnableHardware);
            this.groupHardwareAcceleration.Controls.Add(this.chkEnableDisplayChangeAnimation);
            this.groupHardwareAcceleration.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupHardwareAcceleration.Location = new System.Drawing.Point(0, 1180);
            this.groupHardwareAcceleration.Name = "groupHardwareAcceleration";
            this.groupHardwareAcceleration.Size = new System.Drawing.Size(498, 137);
            this.groupHardwareAcceleration.TabIndex = 3;
            this.groupHardwareAcceleration.Text = "Hardware Acceleration";
            // 
            // chkEnableHardwareFiltering
            // 
            this.chkEnableHardwareFiltering.AutoSize = true;
            this.chkEnableHardwareFiltering.Location = new System.Drawing.Point(33, 70);
            this.chkEnableHardwareFiltering.Name = "chkEnableHardwareFiltering";
            this.chkEnableHardwareFiltering.Size = new System.Drawing.Size(138, 17);
            this.chkEnableHardwareFiltering.TabIndex = 1;
            this.chkEnableHardwareFiltering.Text = "Enable Hardware Filters";
            this.chkEnableHardwareFiltering.UseVisualStyleBackColor = true;
            // 
            // chkEnableSoftwareFiltering
            // 
            this.chkEnableSoftwareFiltering.AutoSize = true;
            this.chkEnableSoftwareFiltering.Location = new System.Drawing.Point(33, 88);
            this.chkEnableSoftwareFiltering.Name = "chkEnableSoftwareFiltering";
            this.chkEnableSoftwareFiltering.Size = new System.Drawing.Size(134, 17);
            this.chkEnableSoftwareFiltering.TabIndex = 2;
            this.chkEnableSoftwareFiltering.Text = "Enable Software Filters";
            this.chkEnableSoftwareFiltering.UseVisualStyleBackColor = true;
            // 
            // chkEnableHardware
            // 
            this.chkEnableHardware.AutoSize = true;
            this.chkEnableHardware.Location = new System.Drawing.Point(12, 38);
            this.chkEnableHardware.Name = "chkEnableHardware";
            this.chkEnableHardware.Size = new System.Drawing.Size(170, 17);
            this.chkEnableHardware.TabIndex = 0;
            this.chkEnableHardware.Text = "Enable Hardware Acceleration";
            this.chkEnableHardware.UseVisualStyleBackColor = true;
            // 
            // chkEnableDisplayChangeAnimation
            // 
            this.chkEnableDisplayChangeAnimation.AutoSize = true;
            this.chkEnableDisplayChangeAnimation.Location = new System.Drawing.Point(33, 108);
            this.chkEnableDisplayChangeAnimation.Name = "chkEnableDisplayChangeAnimation";
            this.chkEnableDisplayChangeAnimation.Size = new System.Drawing.Size(229, 17);
            this.chkEnableDisplayChangeAnimation.TabIndex = 3;
            this.chkEnableDisplayChangeAnimation.Text = "Enable Animation of Page Display changes";
            this.chkEnableDisplayChangeAnimation.UseVisualStyleBackColor = true;
            // 
            // grpMouse
            // 
            this.grpMouse.Controls.Add(this.chkSmoothAutoScrolling);
            this.grpMouse.Controls.Add(this.lblFast);
            this.grpMouse.Controls.Add(this.lblMouseWheel);
            this.grpMouse.Controls.Add(this.chkEnableInertialMouseScrolling);
            this.grpMouse.Controls.Add(this.lblSlow);
            this.grpMouse.Controls.Add(this.tbMouseWheel);
            this.grpMouse.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpMouse.Location = new System.Drawing.Point(0, 1046);
            this.grpMouse.Name = "grpMouse";
            this.grpMouse.Size = new System.Drawing.Size(498, 134);
            this.grpMouse.TabIndex = 5;
            this.grpMouse.Text = "Mouse & Scrolling";
            // 
            // chkSmoothAutoScrolling
            // 
            this.chkSmoothAutoScrolling.AutoSize = true;
            this.chkSmoothAutoScrolling.Location = new System.Drawing.Point(9, 39);
            this.chkSmoothAutoScrolling.Name = "chkSmoothAutoScrolling";
            this.chkSmoothAutoScrolling.Size = new System.Drawing.Size(130, 17);
            this.chkSmoothAutoScrolling.TabIndex = 0;
            this.chkSmoothAutoScrolling.Text = "Smooth Auto Scrolling";
            this.chkSmoothAutoScrolling.UseVisualStyleBackColor = true;
            // 
            // lblFast
            // 
            this.lblFast.Location = new System.Drawing.Point(426, 96);
            this.lblFast.Name = "lblFast";
            this.lblFast.Size = new System.Drawing.Size(56, 19);
            this.lblFast.TabIndex = 4;
            this.lblFast.Text = "fast";
            // 
            // lblMouseWheel
            // 
            this.lblMouseWheel.AutoSize = true;
            this.lblMouseWheel.Location = new System.Drawing.Point(9, 97);
            this.lblMouseWheel.Name = "lblMouseWheel";
            this.lblMouseWheel.Size = new System.Drawing.Size(117, 13);
            this.lblMouseWheel.TabIndex = 0;
            this.lblMouseWheel.Text = "Mouse Wheel scrolling:";
            // 
            // chkEnableInertialMouseScrolling
            // 
            this.chkEnableInertialMouseScrolling.AutoSize = true;
            this.chkEnableInertialMouseScrolling.Location = new System.Drawing.Point(9, 62);
            this.chkEnableInertialMouseScrolling.Name = "chkEnableInertialMouseScrolling";
            this.chkEnableInertialMouseScrolling.Size = new System.Drawing.Size(169, 17);
            this.chkEnableInertialMouseScrolling.TabIndex = 1;
            this.chkEnableInertialMouseScrolling.Text = "Enable Inertial Mouse scrolling";
            this.chkEnableInertialMouseScrolling.UseVisualStyleBackColor = true;
            // 
            // lblSlow
            // 
            this.lblSlow.Location = new System.Drawing.Point(186, 97);
            this.lblSlow.Name = "lblSlow";
            this.lblSlow.Size = new System.Drawing.Size(55, 19);
            this.lblSlow.TabIndex = 2;
            this.lblSlow.Text = "slow";
            this.lblSlow.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbMouseWheel
            // 
            this.tbMouseWheel.Location = new System.Drawing.Point(247, 97);
            this.tbMouseWheel.Maximum = 50;
            this.tbMouseWheel.Minimum = 5;
            this.tbMouseWheel.Name = "tbMouseWheel";
            this.tbMouseWheel.Size = new System.Drawing.Size(173, 16);
            this.tbMouseWheel.TabIndex = 3;
            this.tbMouseWheel.ThumbSize = new System.Drawing.Size(8, 16);
            this.tbMouseWheel.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbMouseWheel.Value = 5;
            // 
            // groupOverlays
            // 
            this.groupOverlays.Controls.Add(this.panelReaderOverlays);
            this.groupOverlays.Controls.Add(this.cbNavigationOverlayPosition);
            this.groupOverlays.Controls.Add(this.labelNavigationOverlayPosition);
            this.groupOverlays.Controls.Add(this.chkShowPageNames);
            this.groupOverlays.Controls.Add(this.tbOverlayScaling);
            this.groupOverlays.Controls.Add(this.chkShowCurrentPageOverlay);
            this.groupOverlays.Controls.Add(this.chkShowStatusOverlay);
            this.groupOverlays.Controls.Add(this.chkShowVisiblePartOverlay);
            this.groupOverlays.Controls.Add(this.chkShowNavigationOverlay);
            this.groupOverlays.Controls.Add(this.labelOverlaySize);
            this.groupOverlays.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupOverlays.Location = new System.Drawing.Point(0, 690);
            this.groupOverlays.Name = "groupOverlays";
            this.groupOverlays.Size = new System.Drawing.Size(498, 356);
            this.groupOverlays.TabIndex = 2;
            this.groupOverlays.Text = "Overlays";
            // 
            // panelReaderOverlays
            // 
            this.panelReaderOverlays.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelReaderOverlays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelReaderOverlays.Controls.Add(this.labelVisiblePartOverlay);
            this.panelReaderOverlays.Controls.Add(this.labelNavigationOverlay);
            this.panelReaderOverlays.Controls.Add(this.labelStatusOverlay);
            this.panelReaderOverlays.Controls.Add(this.labelPageOverlay);
            this.panelReaderOverlays.Location = new System.Drawing.Point(118, 39);
            this.panelReaderOverlays.Name = "panelReaderOverlays";
            this.panelReaderOverlays.Size = new System.Drawing.Size(258, 134);
            this.panelReaderOverlays.TabIndex = 8;
            // 
            // labelVisiblePartOverlay
            // 
            this.labelVisiblePartOverlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVisiblePartOverlay.BackColor = System.Drawing.Color.Gainsboro;
            this.labelVisiblePartOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelVisiblePartOverlay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.labelVisiblePartOverlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVisiblePartOverlay.Location = new System.Drawing.Point(204, 75);
            this.labelVisiblePartOverlay.Name = "labelVisiblePartOverlay";
            this.labelVisiblePartOverlay.Size = new System.Drawing.Size(49, 51);
            this.labelVisiblePartOverlay.TabIndex = 3;
            this.labelVisiblePartOverlay.Text = "Visible Part";
            this.labelVisiblePartOverlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelVisiblePartOverlay.UseMnemonic = false;
            // 
            // labelNavigationOverlay
            // 
            this.labelNavigationOverlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelNavigationOverlay.BackColor = System.Drawing.Color.Gainsboro;
            this.labelNavigationOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelNavigationOverlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNavigationOverlay.Location = new System.Drawing.Point(55, 100);
            this.labelNavigationOverlay.Name = "labelNavigationOverlay";
            this.labelNavigationOverlay.Size = new System.Drawing.Size(143, 26);
            this.labelNavigationOverlay.TabIndex = 2;
            this.labelNavigationOverlay.Text = "Navigation";
            this.labelNavigationOverlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelNavigationOverlay.UseMnemonic = false;
            // 
            // labelStatusOverlay
            // 
            this.labelStatusOverlay.BackColor = System.Drawing.Color.Gainsboro;
            this.labelStatusOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelStatusOverlay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.labelStatusOverlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatusOverlay.Location = new System.Drawing.Point(60, 49);
            this.labelStatusOverlay.Name = "labelStatusOverlay";
            this.labelStatusOverlay.Size = new System.Drawing.Size(134, 26);
            this.labelStatusOverlay.TabIndex = 1;
            this.labelStatusOverlay.Text = "Messages and Status";
            this.labelStatusOverlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelStatusOverlay.UseMnemonic = false;
            // 
            // labelPageOverlay
            // 
            this.labelPageOverlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPageOverlay.BackColor = System.Drawing.Color.Gainsboro;
            this.labelPageOverlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPageOverlay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.labelPageOverlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPageOverlay.Location = new System.Drawing.Point(204, 3);
            this.labelPageOverlay.Name = "labelPageOverlay";
            this.labelPageOverlay.Size = new System.Drawing.Size(49, 36);
            this.labelPageOverlay.TabIndex = 0;
            this.labelPageOverlay.Text = "Page";
            this.labelPageOverlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelPageOverlay.UseMnemonic = false;
            // 
            // cbNavigationOverlayPosition
            // 
            this.cbNavigationOverlayPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNavigationOverlayPosition.FormattingEnabled = true;
            this.cbNavigationOverlayPosition.Items.AddRange(new object[] {
            "at Bottom",
            "on Top"});
            this.cbNavigationOverlayPosition.Location = new System.Drawing.Point(84, 313);
            this.cbNavigationOverlayPosition.Name = "cbNavigationOverlayPosition";
            this.cbNavigationOverlayPosition.Size = new System.Drawing.Size(121, 21);
            this.cbNavigationOverlayPosition.TabIndex = 6;
            // 
            // labelNavigationOverlayPosition
            // 
            this.labelNavigationOverlayPosition.AutoSize = true;
            this.labelNavigationOverlayPosition.Location = new System.Drawing.Point(18, 316);
            this.labelNavigationOverlayPosition.Name = "labelNavigationOverlayPosition";
            this.labelNavigationOverlayPosition.Size = new System.Drawing.Size(61, 13);
            this.labelNavigationOverlayPosition.TabIndex = 5;
            this.labelNavigationOverlayPosition.Text = "Navigation:";
            // 
            // chkShowPageNames
            // 
            this.chkShowPageNames.AutoSize = true;
            this.chkShowPageNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowPageNames.Location = new System.Drawing.Point(283, 193);
            this.chkShowPageNames.Name = "chkShowPageNames";
            this.chkShowPageNames.Size = new System.Drawing.Size(181, 17);
            this.chkShowPageNames.TabIndex = 4;
            this.chkShowPageNames.Text = "Current Page also displays Name";
            this.chkShowPageNames.UseVisualStyleBackColor = true;
            // 
            // tbOverlayScaling
            // 
            this.tbOverlayScaling.Location = new System.Drawing.Point(288, 316);
            this.tbOverlayScaling.Maximum = 150;
            this.tbOverlayScaling.Minimum = 40;
            this.tbOverlayScaling.Name = "tbOverlayScaling";
            this.tbOverlayScaling.Size = new System.Drawing.Size(184, 16);
            this.tbOverlayScaling.TabIndex = 8;
            this.tbOverlayScaling.ThumbSize = new System.Drawing.Size(8, 16);
            this.tbOverlayScaling.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbOverlayScaling.Value = 50;
            this.tbOverlayScaling.ValueChanged += new System.EventHandler(this.tbOverlayScalingChanged);
            // 
            // chkShowCurrentPageOverlay
            // 
            this.chkShowCurrentPageOverlay.AutoSize = true;
            this.chkShowCurrentPageOverlay.Location = new System.Drawing.Point(58, 193);
            this.chkShowCurrentPageOverlay.Name = "chkShowCurrentPageOverlay";
            this.chkShowCurrentPageOverlay.Size = new System.Drawing.Size(117, 17);
            this.chkShowCurrentPageOverlay.TabIndex = 0;
            this.chkShowCurrentPageOverlay.Text = "Show current Page";
            this.chkShowCurrentPageOverlay.UseVisualStyleBackColor = true;
            // 
            // chkShowStatusOverlay
            // 
            this.chkShowStatusOverlay.AutoSize = true;
            this.chkShowStatusOverlay.Location = new System.Drawing.Point(58, 217);
            this.chkShowStatusOverlay.Name = "chkShowStatusOverlay";
            this.chkShowStatusOverlay.Size = new System.Drawing.Size(158, 17);
            this.chkShowStatusOverlay.TabIndex = 1;
            this.chkShowStatusOverlay.Text = "Show Messages and Status";
            this.chkShowStatusOverlay.UseVisualStyleBackColor = true;
            // 
            // chkShowVisiblePartOverlay
            // 
            this.chkShowVisiblePartOverlay.AutoSize = true;
            this.chkShowVisiblePartOverlay.Location = new System.Drawing.Point(58, 241);
            this.chkShowVisiblePartOverlay.Name = "chkShowVisiblePartOverlay";
            this.chkShowVisiblePartOverlay.Size = new System.Drawing.Size(135, 17);
            this.chkShowVisiblePartOverlay.TabIndex = 2;
            this.chkShowVisiblePartOverlay.Text = "Show visible Page Part";
            this.chkShowVisiblePartOverlay.UseVisualStyleBackColor = true;
            // 
            // chkShowNavigationOverlay
            // 
            this.chkShowNavigationOverlay.AutoSize = true;
            this.chkShowNavigationOverlay.Location = new System.Drawing.Point(58, 264);
            this.chkShowNavigationOverlay.Name = "chkShowNavigationOverlay";
            this.chkShowNavigationOverlay.Size = new System.Drawing.Size(171, 17);
            this.chkShowNavigationOverlay.TabIndex = 3;
            this.chkShowNavigationOverlay.Text = "Show Navigation automatically";
            this.chkShowNavigationOverlay.UseVisualStyleBackColor = true;
            // 
            // labelOverlaySize
            // 
            this.labelOverlaySize.AutoSize = true;
            this.labelOverlaySize.Location = new System.Drawing.Point(244, 316);
            this.labelOverlaySize.Name = "labelOverlaySize";
            this.labelOverlaySize.Size = new System.Drawing.Size(38, 13);
            this.labelOverlaySize.TabIndex = 7;
            this.labelOverlaySize.Text = "Sizing:";
            // 
            // grpKeyboard
            // 
            this.grpKeyboard.Controls.Add(this.btExportKeyboard);
            this.grpKeyboard.Controls.Add(this.btImportKeyboard);
            this.grpKeyboard.Controls.Add(this.keyboardShortcutEditor);
            this.grpKeyboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpKeyboard.Location = new System.Drawing.Point(0, 300);
            this.grpKeyboard.Name = "grpKeyboard";
            this.grpKeyboard.Size = new System.Drawing.Size(498, 390);
            this.grpKeyboard.TabIndex = 4;
            this.grpKeyboard.Text = "Keyboard";
            // 
            // btExportKeyboard
            // 
            this.btExportKeyboard.Location = new System.Drawing.Point(274, 357);
            this.btExportKeyboard.Name = "btExportKeyboard";
            this.btExportKeyboard.Size = new System.Drawing.Size(102, 23);
            this.btExportKeyboard.TabIndex = 1;
            this.btExportKeyboard.Text = "Export...";
            this.btExportKeyboard.UseVisualStyleBackColor = true;
            this.btExportKeyboard.Click += new System.EventHandler(this.btExportKeyboard_Click);
            // 
            // btImportKeyboard
            // 
            this.btImportKeyboard.ContextMenuStrip = this.cmKeyboardLayout;
            this.btImportKeyboard.Location = new System.Drawing.Point(382, 357);
            this.btImportKeyboard.Name = "btImportKeyboard";
            this.btImportKeyboard.Size = new System.Drawing.Size(102, 23);
            this.btImportKeyboard.TabIndex = 2;
            this.btImportKeyboard.Text = "Import...";
            this.btImportKeyboard.UseVisualStyleBackColor = true;
            this.btImportKeyboard.ShowContextMenu += new System.EventHandler(this.btImportKeyboard_ShowContextMenu);
            this.btImportKeyboard.Click += new System.EventHandler(this.btLoadKeyboard_Click);
            // 
            // cmKeyboardLayout
            // 
            this.cmKeyboardLayout.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miDefaultKeyboardLayout,
            this.toolStripMenuItem1});
            this.cmKeyboardLayout.Name = "cmKeyboardLayout";
            this.cmKeyboardLayout.Size = new System.Drawing.Size(113, 32);
            // 
            // miDefaultKeyboardLayout
            // 
            this.miDefaultKeyboardLayout.Name = "miDefaultKeyboardLayout";
            this.miDefaultKeyboardLayout.Size = new System.Drawing.Size(112, 22);
            this.miDefaultKeyboardLayout.Text = "&Default";
            this.miDefaultKeyboardLayout.Click += new System.EventHandler(this.miDefaultKeyboardLayout_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(109, 6);
            // 
            // keyboardShortcutEditor
            // 
            this.keyboardShortcutEditor.AllowDrop = true;
            this.keyboardShortcutEditor.Location = new System.Drawing.Point(12, 37);
            this.keyboardShortcutEditor.Name = "keyboardShortcutEditor";
            this.keyboardShortcutEditor.Shortcuts = null;
            this.keyboardShortcutEditor.Size = new System.Drawing.Size(472, 314);
            this.keyboardShortcutEditor.TabIndex = 0;
            this.keyboardShortcutEditor.DragDrop += new System.Windows.Forms.DragEventHandler(this.keyboardShortcutEditor_DragDrop);
            this.keyboardShortcutEditor.DragOver += new System.Windows.Forms.DragEventHandler(this.keyboardShortcutEditor_DragOver);
            // 
            // grpDisplay
            // 
            this.grpDisplay.Controls.Add(this.tbGamma);
            this.grpDisplay.Controls.Add(this.labelGamma);
            this.grpDisplay.Controls.Add(this.chkAnamorphicScaling);
            this.grpDisplay.Controls.Add(this.chkHighQualityDisplay);
            this.grpDisplay.Controls.Add(this.labelSharpening);
            this.grpDisplay.Controls.Add(this.tbSharpening);
            this.grpDisplay.Controls.Add(this.btResetColor);
            this.grpDisplay.Controls.Add(this.chkAutoContrast);
            this.grpDisplay.Controls.Add(this.labelSaturation);
            this.grpDisplay.Controls.Add(this.tbSaturation);
            this.grpDisplay.Controls.Add(this.labelBrightness);
            this.grpDisplay.Controls.Add(this.tbBrightness);
            this.grpDisplay.Controls.Add(this.tbContrast);
            this.grpDisplay.Controls.Add(this.labelContrast);
            this.grpDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDisplay.Location = new System.Drawing.Point(0, 0);
            this.grpDisplay.Name = "grpDisplay";
            this.grpDisplay.Size = new System.Drawing.Size(498, 300);
            this.grpDisplay.TabIndex = 1;
            this.grpDisplay.Text = "Display";
            // 
            // tbGamma
            // 
            this.tbGamma.Location = new System.Drawing.Point(150, 193);
            this.tbGamma.Minimum = -100;
            this.tbGamma.Name = "tbGamma";
            this.tbGamma.Size = new System.Drawing.Size(332, 16);
            this.tbGamma.TabIndex = 12;
            this.tbGamma.Text = "tbSaturation";
            this.tbGamma.ThumbSize = new System.Drawing.Size(8, 16);
            this.tbGamma.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbGamma.ValueChanged += new System.EventHandler(this.tbColorAdjustmentChanged);
            this.tbGamma.DoubleClick += new System.EventHandler(this.tbGamma_DoubleClick);
            // 
            // labelGamma
            // 
            this.labelGamma.Location = new System.Drawing.Point(14, 193);
            this.labelGamma.Name = "labelGamma";
            this.labelGamma.Size = new System.Drawing.Size(133, 13);
            this.labelGamma.TabIndex = 11;
            this.labelGamma.Text = "Gamma Adjustment:";
            this.labelGamma.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chkAnamorphicScaling
            // 
            this.chkAnamorphicScaling.AutoSize = true;
            this.chkAnamorphicScaling.Location = new System.Drawing.Point(12, 60);
            this.chkAnamorphicScaling.Name = "chkAnamorphicScaling";
            this.chkAnamorphicScaling.Size = new System.Drawing.Size(120, 17);
            this.chkAnamorphicScaling.TabIndex = 0;
            this.chkAnamorphicScaling.Text = "&Anamorphic Scaling";
            this.chkAnamorphicScaling.UseVisualStyleBackColor = true;
            // 
            // chkHighQualityDisplay
            // 
            this.chkHighQualityDisplay.AutoSize = true;
            this.chkHighQualityDisplay.Location = new System.Drawing.Point(12, 37);
            this.chkHighQualityDisplay.Name = "chkHighQualityDisplay";
            this.chkHighQualityDisplay.Size = new System.Drawing.Size(83, 17);
            this.chkHighQualityDisplay.TabIndex = 0;
            this.chkHighQualityDisplay.Text = "&High Quality";
            this.chkHighQualityDisplay.UseVisualStyleBackColor = true;
            // 
            // labelSharpening
            // 
            this.labelSharpening.Location = new System.Drawing.Point(17, 225);
            this.labelSharpening.Name = "labelSharpening";
            this.labelSharpening.Size = new System.Drawing.Size(132, 13);
            this.labelSharpening.TabIndex = 8;
            this.labelSharpening.Text = "Sharpening:";
            this.labelSharpening.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbSharpening
            // 
            this.tbSharpening.LargeChange = 1;
            this.tbSharpening.Location = new System.Drawing.Point(149, 225);
            this.tbSharpening.Maximum = 3;
            this.tbSharpening.Name = "tbSharpening";
            this.tbSharpening.Size = new System.Drawing.Size(333, 18);
            this.tbSharpening.TabIndex = 9;
            this.tbSharpening.Text = "tbSaturation";
            this.tbSharpening.ThumbSize = new System.Drawing.Size(8, 16);
            this.tbSharpening.TickFrequency = 1;
            this.tbSharpening.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbSharpening.DoubleClick += new System.EventHandler(this.tbSharpening_DoubleClick);
            // 
            // btResetColor
            // 
            this.btResetColor.Location = new System.Drawing.Point(394, 265);
            this.btResetColor.Name = "btResetColor";
            this.btResetColor.Size = new System.Drawing.Size(91, 23);
            this.btResetColor.TabIndex = 10;
            this.btResetColor.Text = "&Reset";
            this.btResetColor.UseVisualStyleBackColor = true;
            this.btResetColor.Click += new System.EventHandler(this.btReset_Click);
            // 
            // chkAutoContrast
            // 
            this.chkAutoContrast.AutoSize = true;
            this.chkAutoContrast.Location = new System.Drawing.Point(12, 95);
            this.chkAutoContrast.Name = "chkAutoContrast";
            this.chkAutoContrast.Size = new System.Drawing.Size(184, 17);
            this.chkAutoContrast.TabIndex = 1;
            this.chkAutoContrast.Text = "Automatic &Contrast Enhancement";
            this.chkAutoContrast.UseVisualStyleBackColor = true;
            // 
            // labelSaturation
            // 
            this.labelSaturation.Location = new System.Drawing.Point(11, 122);
            this.labelSaturation.Name = "labelSaturation";
            this.labelSaturation.Size = new System.Drawing.Size(136, 13);
            this.labelSaturation.TabIndex = 2;
            this.labelSaturation.Text = "Saturation Adjustment:";
            this.labelSaturation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbSaturation
            // 
            this.tbSaturation.Location = new System.Drawing.Point(148, 122);
            this.tbSaturation.Minimum = -100;
            this.tbSaturation.Name = "tbSaturation";
            this.tbSaturation.Size = new System.Drawing.Size(334, 16);
            this.tbSaturation.TabIndex = 3;
            this.tbSaturation.ThumbSize = new System.Drawing.Size(8, 16);
            this.tbSaturation.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbSaturation.ValueChanged += new System.EventHandler(this.tbColorAdjustmentChanged);
            this.tbSaturation.DoubleClick += new System.EventHandler(this.tbSaturation_DoubleClick);
            // 
            // labelBrightness
            // 
            this.labelBrightness.Location = new System.Drawing.Point(14, 144);
            this.labelBrightness.Name = "labelBrightness";
            this.labelBrightness.Size = new System.Drawing.Size(133, 13);
            this.labelBrightness.TabIndex = 4;
            this.labelBrightness.Text = "Brightness Adjustment:";
            this.labelBrightness.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbBrightness
            // 
            this.tbBrightness.Location = new System.Drawing.Point(148, 144);
            this.tbBrightness.Minimum = -100;
            this.tbBrightness.Name = "tbBrightness";
            this.tbBrightness.Size = new System.Drawing.Size(334, 16);
            this.tbBrightness.TabIndex = 5;
            this.tbBrightness.Text = "tbBrightness";
            this.tbBrightness.ThumbSize = new System.Drawing.Size(8, 16);
            this.tbBrightness.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbBrightness.ValueChanged += new System.EventHandler(this.tbColorAdjustmentChanged);
            this.tbBrightness.DoubleClick += new System.EventHandler(this.tbBrightness_DoubleClick);
            // 
            // tbContrast
            // 
            this.tbContrast.Location = new System.Drawing.Point(148, 168);
            this.tbContrast.Minimum = -100;
            this.tbContrast.Name = "tbContrast";
            this.tbContrast.Size = new System.Drawing.Size(334, 16);
            this.tbContrast.TabIndex = 7;
            this.tbContrast.Text = "tbSaturation";
            this.tbContrast.ThumbSize = new System.Drawing.Size(8, 16);
            this.tbContrast.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbContrast.ValueChanged += new System.EventHandler(this.tbColorAdjustmentChanged);
            this.tbContrast.DoubleClick += new System.EventHandler(this.tbContrast_DoubleClick);
            // 
            // labelContrast
            // 
            this.labelContrast.Location = new System.Drawing.Point(14, 168);
            this.labelContrast.Name = "labelContrast";
            this.labelContrast.Size = new System.Drawing.Size(133, 13);
            this.labelContrast.TabIndex = 6;
            this.labelContrast.Text = "Contrast Adjustment:";
            this.labelContrast.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pageAdvanced
            // 
            this.pageAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pageAdvanced.AutoScroll = true;
            this.pageAdvanced.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageAdvanced.Controls.Add(this.grpWirelessSetup);
            this.pageAdvanced.Controls.Add(this.grpIntegration);
            this.pageAdvanced.Controls.Add(this.groupMessagesAndSocial);
            this.pageAdvanced.Controls.Add(this.groupMemory);
            this.pageAdvanced.Controls.Add(this.grpDatabaseBackup);
            this.pageAdvanced.Controls.Add(this.groupOtherComics);
            this.pageAdvanced.Controls.Add(this.grpLanguages);
            this.pageAdvanced.Location = new System.Drawing.Point(84, 8);
            this.pageAdvanced.Name = "pageAdvanced";
            this.pageAdvanced.Size = new System.Drawing.Size(517, 408);
            this.pageAdvanced.TabIndex = 9;
            // 
            // grpWirelessSetup
            // 
            this.grpWirelessSetup.Controls.Add(this.btTestWifi);
            this.grpWirelessSetup.Controls.Add(this.lblWifiStatus);
            this.grpWirelessSetup.Controls.Add(this.lblWifiAddresses);
            this.grpWirelessSetup.Controls.Add(this.txWifiAddresses);
            this.grpWirelessSetup.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpWirelessSetup.Location = new System.Drawing.Point(0, 1411);
            this.grpWirelessSetup.Name = "grpWirelessSetup";
            this.grpWirelessSetup.Size = new System.Drawing.Size(498, 136);
            this.grpWirelessSetup.TabIndex = 8;
            this.grpWirelessSetup.Text = "Wireless Setup";
            // 
            // btTestWifi
            // 
            this.btTestWifi.Location = new System.Drawing.Point(382, 63);
            this.btTestWifi.Name = "btTestWifi";
            this.btTestWifi.Size = new System.Drawing.Size(104, 23);
            this.btTestWifi.TabIndex = 3;
            this.btTestWifi.Text = "Test";
            this.btTestWifi.UseVisualStyleBackColor = true;
            this.btTestWifi.Click += new System.EventHandler(this.btTestWifi_Click);
            // 
            // lblWifiStatus
            // 
            this.lblWifiStatus.Location = new System.Drawing.Point(6, 93);
            this.lblWifiStatus.Name = "lblWifiStatus";
            this.lblWifiStatus.Size = new System.Drawing.Size(370, 21);
            this.lblWifiStatus.TabIndex = 2;
            this.lblWifiStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWifiAddresses
            // 
            this.lblWifiAddresses.AutoSize = true;
            this.lblWifiAddresses.Location = new System.Drawing.Point(4, 41);
            this.lblWifiAddresses.Name = "lblWifiAddresses";
            this.lblWifiAddresses.Size = new System.Drawing.Size(490, 13);
            this.lblWifiAddresses.TabIndex = 1;
            this.lblWifiAddresses.Text = "Semicolon separated list of IP addresses for Wireless Devices which where not det" +
    "ected automatically:";
            // 
            // txWifiAddresses
            // 
            this.txWifiAddresses.Location = new System.Drawing.Point(6, 65);
            this.txWifiAddresses.Name = "txWifiAddresses";
            this.txWifiAddresses.Size = new System.Drawing.Size(370, 20);
            this.txWifiAddresses.TabIndex = 0;
            // 
            // grpIntegration
            // 
            this.grpIntegration.Controls.Add(this.btAssociateExtensions);
            this.grpIntegration.Controls.Add(this.labelCheckedFormats);
            this.grpIntegration.Controls.Add(this.chkOverwriteAssociations);
            this.grpIntegration.Controls.Add(this.lbFormats);
            this.grpIntegration.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpIntegration.Location = new System.Drawing.Point(0, 1071);
            this.grpIntegration.Name = "grpIntegration";
            this.grpIntegration.Size = new System.Drawing.Size(498, 340);
            this.grpIntegration.TabIndex = 0;
            this.grpIntegration.Text = "Explorer Integration";
            // 
            // btAssociateExtensions
            // 
            this.btAssociateExtensions.Location = new System.Drawing.Point(382, 57);
            this.btAssociateExtensions.Name = "btAssociateExtensions";
            this.btAssociateExtensions.Size = new System.Drawing.Size(104, 23);
            this.btAssociateExtensions.TabIndex = 4;
            this.btAssociateExtensions.Text = "Change...";
            this.btAssociateExtensions.UseVisualStyleBackColor = true;
            this.btAssociateExtensions.Click += new System.EventHandler(this.btAssociateExtensions_Click);
            // 
            // labelCheckedFormats
            // 
            this.labelCheckedFormats.AutoSize = true;
            this.labelCheckedFormats.Location = new System.Drawing.Point(3, 35);
            this.labelCheckedFormats.Name = "labelCheckedFormats";
            this.labelCheckedFormats.Size = new System.Drawing.Size(253, 13);
            this.labelCheckedFormats.TabIndex = 0;
            this.labelCheckedFormats.Text = "Checked formats will be associated with ComicRack";
            // 
            // chkOverwriteAssociations
            // 
            this.chkOverwriteAssociations.AutoSize = true;
            this.chkOverwriteAssociations.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkOverwriteAssociations.Location = new System.Drawing.Point(6, 307);
            this.chkOverwriteAssociations.Name = "chkOverwriteAssociations";
            this.chkOverwriteAssociations.Size = new System.Drawing.Size(289, 17);
            this.chkOverwriteAssociations.TabIndex = 2;
            this.chkOverwriteAssociations.Text = "Overwrite existing associations instead of \'Open With ...\'";
            this.chkOverwriteAssociations.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkOverwriteAssociations.UseVisualStyleBackColor = true;
            // 
            // lbFormats
            // 
            this.lbFormats.CheckOnClick = true;
            this.lbFormats.FormattingEnabled = true;
            this.lbFormats.Location = new System.Drawing.Point(6, 57);
            this.lbFormats.Name = "lbFormats";
            this.lbFormats.Size = new System.Drawing.Size(371, 244);
            this.lbFormats.TabIndex = 1;
            // 
            // groupMessagesAndSocial
            // 
            this.groupMessagesAndSocial.Controls.Add(this.btResetMessages);
            this.groupMessagesAndSocial.Controls.Add(this.labelReshowHidden);
            this.groupMessagesAndSocial.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupMessagesAndSocial.Location = new System.Drawing.Point(0, 996);
            this.groupMessagesAndSocial.Name = "groupMessagesAndSocial";
            this.groupMessagesAndSocial.Size = new System.Drawing.Size(498, 75);
            this.groupMessagesAndSocial.TabIndex = 6;
            this.groupMessagesAndSocial.Text = "Messages and Social";
            // 
            // btResetMessages
            // 
            this.btResetMessages.Location = new System.Drawing.Point(382, 41);
            this.btResetMessages.Name = "btResetMessages";
            this.btResetMessages.Size = new System.Drawing.Size(104, 23);
            this.btResetMessages.TabIndex = 1;
            this.btResetMessages.Text = "Reset";
            this.btResetMessages.UseVisualStyleBackColor = true;
            this.btResetMessages.Click += new System.EventHandler(this.btResetMessages_Click);
            // 
            // labelReshowHidden
            // 
            this.labelReshowHidden.Location = new System.Drawing.Point(6, 46);
            this.labelReshowHidden.Name = "labelReshowHidden";
            this.labelReshowHidden.Size = new System.Drawing.Size(370, 17);
            this.labelReshowHidden.TabIndex = 0;
            this.labelReshowHidden.Text = "To reshow hidden messages press";
            // 
            // groupMemory
            // 
            this.groupMemory.Controls.Add(this.grpMaximumMemoryUsage);
            this.groupMemory.Controls.Add(this.grpMemoryCache);
            this.groupMemory.Controls.Add(this.grpDiskCache);
            this.groupMemory.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupMemory.Location = new System.Drawing.Point(0, 641);
            this.groupMemory.Name = "groupMemory";
            this.groupMemory.Size = new System.Drawing.Size(498, 355);
            this.groupMemory.TabIndex = 1;
            this.groupMemory.Text = "Caches & Memory Usage";
            // 
            // grpMaximumMemoryUsage
            // 
            this.grpMaximumMemoryUsage.Controls.Add(this.lblMaximumMemoryUsageValue);
            this.grpMaximumMemoryUsage.Controls.Add(this.tbMaximumMemoryUsage);
            this.grpMaximumMemoryUsage.Controls.Add(this.lblMaximumMemoryUsage);
            this.grpMaximumMemoryUsage.Location = new System.Drawing.Point(7, 255);
            this.grpMaximumMemoryUsage.Name = "grpMaximumMemoryUsage";
            this.grpMaximumMemoryUsage.Size = new System.Drawing.Size(476, 86);
            this.grpMaximumMemoryUsage.TabIndex = 14;
            this.grpMaximumMemoryUsage.TabStop = false;
            this.grpMaximumMemoryUsage.Text = "Maximum Memory Usage";
            // 
            // lblMaximumMemoryUsageValue
            // 
            this.lblMaximumMemoryUsageValue.AutoSize = true;
            this.lblMaximumMemoryUsageValue.Location = new System.Drawing.Point(397, 31);
            this.lblMaximumMemoryUsageValue.Name = "lblMaximumMemoryUsageValue";
            this.lblMaximumMemoryUsageValue.Size = new System.Drawing.Size(63, 13);
            this.lblMaximumMemoryUsageValue.TabIndex = 2;
            this.lblMaximumMemoryUsageValue.Text = "Slider Value";
            // 
            // tbMaximumMemoryUsage
            // 
            this.tbMaximumMemoryUsage.LargeChange = 4;
            this.tbMaximumMemoryUsage.Location = new System.Drawing.Point(7, 24);
            this.tbMaximumMemoryUsage.Maximum = 64;
            this.tbMaximumMemoryUsage.Name = "tbMaximumMemoryUsage";
            this.tbMaximumMemoryUsage.Size = new System.Drawing.Size(379, 29);
            this.tbMaximumMemoryUsage.TabIndex = 1;
            this.tbMaximumMemoryUsage.ThumbSize = new System.Drawing.Size(10, 20);
            this.tbMaximumMemoryUsage.TickFrequency = 8;
            this.tbMaximumMemoryUsage.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.tbMaximumMemoryUsage.TickThickness = 2;
            this.tbMaximumMemoryUsage.ValueChanged += new System.EventHandler(this.tbSystemMemory_ValueChanged);
            // 
            // lblMaximumMemoryUsage
            // 
            this.lblMaximumMemoryUsage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMaximumMemoryUsage.Location = new System.Drawing.Point(3, 58);
            this.lblMaximumMemoryUsage.Name = "lblMaximumMemoryUsage";
            this.lblMaximumMemoryUsage.Size = new System.Drawing.Size(470, 25);
            this.lblMaximumMemoryUsage.TabIndex = 0;
            this.lblMaximumMemoryUsage.Text = "Limiting the memory can adversely affect the performance.";
            this.lblMaximumMemoryUsage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpMemoryCache
            // 
            this.grpMemoryCache.Controls.Add(this.lblPageMemCacheUsage);
            this.grpMemoryCache.Controls.Add(this.labelMemThumbSize);
            this.grpMemoryCache.Controls.Add(this.lblThumbMemCacheUsage);
            this.grpMemoryCache.Controls.Add(this.numMemPageCount);
            this.grpMemoryCache.Controls.Add(this.labelMemPageCount);
            this.grpMemoryCache.Controls.Add(this.chkMemPageOptimized);
            this.grpMemoryCache.Controls.Add(this.chkMemThumbOptimized);
            this.grpMemoryCache.Controls.Add(this.numMemThumbSize);
            this.grpMemoryCache.Location = new System.Drawing.Point(6, 162);
            this.grpMemoryCache.Name = "grpMemoryCache";
            this.grpMemoryCache.Size = new System.Drawing.Size(476, 85);
            this.grpMemoryCache.TabIndex = 13;
            this.grpMemoryCache.TabStop = false;
            this.grpMemoryCache.Text = "Memory Cache";
            // 
            // lblPageMemCacheUsage
            // 
            this.lblPageMemCacheUsage.AutoSize = true;
            this.lblPageMemCacheUsage.Location = new System.Drawing.Point(299, 52);
            this.lblPageMemCacheUsage.Name = "lblPageMemCacheUsage";
            this.lblPageMemCacheUsage.Size = new System.Drawing.Size(124, 13);
            this.lblPageMemCacheUsage.TabIndex = 8;
            this.lblPageMemCacheUsage.Text = "usage Page Mem Cache";
            // 
            // labelMemThumbSize
            // 
            this.labelMemThumbSize.AutoSize = true;
            this.labelMemThumbSize.Location = new System.Drawing.Point(19, 29);
            this.labelMemThumbSize.Name = "labelMemThumbSize";
            this.labelMemThumbSize.Size = new System.Drawing.Size(86, 13);
            this.labelMemThumbSize.TabIndex = 0;
            this.labelMemThumbSize.Text = "Thumbnails [MB]";
            // 
            // lblThumbMemCacheUsage
            // 
            this.lblThumbMemCacheUsage.AutoSize = true;
            this.lblThumbMemCacheUsage.Location = new System.Drawing.Point(299, 26);
            this.lblThumbMemCacheUsage.Name = "lblThumbMemCacheUsage";
            this.lblThumbMemCacheUsage.Size = new System.Drawing.Size(132, 13);
            this.lblThumbMemCacheUsage.TabIndex = 7;
            this.lblThumbMemCacheUsage.Text = "usage Thumb Mem Cache";
            // 
            // numMemPageCount
            // 
            this.numMemPageCount.Location = new System.Drawing.Point(145, 51);
            this.numMemPageCount.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numMemPageCount.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMemPageCount.Name = "numMemPageCount";
            this.numMemPageCount.Size = new System.Drawing.Size(67, 20);
            this.numMemPageCount.TabIndex = 4;
            this.numMemPageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMemPageCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelMemPageCount
            // 
            this.labelMemPageCount.AutoSize = true;
            this.labelMemPageCount.Location = new System.Drawing.Point(19, 52);
            this.labelMemPageCount.Name = "labelMemPageCount";
            this.labelMemPageCount.Size = new System.Drawing.Size(73, 13);
            this.labelMemPageCount.TabIndex = 3;
            this.labelMemPageCount.Text = "Pages [count]";
            // 
            // chkMemPageOptimized
            // 
            this.chkMemPageOptimized.AutoSize = true;
            this.chkMemPageOptimized.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMemPageOptimized.Location = new System.Drawing.Point(218, 51);
            this.chkMemPageOptimized.Name = "chkMemPageOptimized";
            this.chkMemPageOptimized.Size = new System.Drawing.Size(70, 17);
            this.chkMemPageOptimized.TabIndex = 5;
            this.chkMemPageOptimized.Text = "optimized";
            this.chkMemPageOptimized.UseVisualStyleBackColor = true;
            // 
            // chkMemThumbOptimized
            // 
            this.chkMemThumbOptimized.AutoSize = true;
            this.chkMemThumbOptimized.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkMemThumbOptimized.Location = new System.Drawing.Point(218, 25);
            this.chkMemThumbOptimized.Name = "chkMemThumbOptimized";
            this.chkMemThumbOptimized.Size = new System.Drawing.Size(70, 17);
            this.chkMemThumbOptimized.TabIndex = 2;
            this.chkMemThumbOptimized.Text = "optimized";
            this.chkMemThumbOptimized.UseVisualStyleBackColor = true;
            // 
            // numMemThumbSize
            // 
            this.numMemThumbSize.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMemThumbSize.Location = new System.Drawing.Point(145, 24);
            this.numMemThumbSize.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numMemThumbSize.Name = "numMemThumbSize";
            this.numMemThumbSize.Size = new System.Drawing.Size(67, 20);
            this.numMemThumbSize.TabIndex = 1;
            this.numMemThumbSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMemThumbSize.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // grpDiskCache
            // 
            this.grpDiskCache.Controls.Add(this.chkEnableInternetCache);
            this.grpDiskCache.Controls.Add(this.lblInternetCacheUsage);
            this.grpDiskCache.Controls.Add(this.btClearPageCache);
            this.grpDiskCache.Controls.Add(this.numPageCacheSize);
            this.grpDiskCache.Controls.Add(this.numInternetCacheSize);
            this.grpDiskCache.Controls.Add(this.btClearThumbnailCache);
            this.grpDiskCache.Controls.Add(this.btClearInternetCache);
            this.grpDiskCache.Controls.Add(this.chkEnablePageCache);
            this.grpDiskCache.Controls.Add(this.lblPageCacheUsage);
            this.grpDiskCache.Controls.Add(this.numThumbnailCacheSize);
            this.grpDiskCache.Controls.Add(this.chkEnableThumbnailCache);
            this.grpDiskCache.Controls.Add(this.lblThumbCacheUsage);
            this.grpDiskCache.Location = new System.Drawing.Point(6, 35);
            this.grpDiskCache.Name = "grpDiskCache";
            this.grpDiskCache.Size = new System.Drawing.Size(476, 120);
            this.grpDiskCache.TabIndex = 12;
            this.grpDiskCache.TabStop = false;
            this.grpDiskCache.Text = "Disk Cache";
            // 
            // chkEnableInternetCache
            // 
            this.chkEnableInternetCache.AutoSize = true;
            this.chkEnableInternetCache.Location = new System.Drawing.Point(22, 31);
            this.chkEnableInternetCache.Name = "chkEnableInternetCache";
            this.chkEnableInternetCache.Size = new System.Drawing.Size(87, 17);
            this.chkEnableInternetCache.TabIndex = 0;
            this.chkEnableInternetCache.Text = "Internet [MB]";
            this.chkEnableInternetCache.UseVisualStyleBackColor = true;
            // 
            // lblInternetCacheUsage
            // 
            this.lblInternetCacheUsage.AutoSize = true;
            this.lblInternetCacheUsage.Location = new System.Drawing.Point(298, 31);
            this.lblInternetCacheUsage.Name = "lblInternetCacheUsage";
            this.lblInternetCacheUsage.Size = new System.Drawing.Size(109, 13);
            this.lblInternetCacheUsage.TabIndex = 3;
            this.lblInternetCacheUsage.Text = "usage Internet Cache";
            // 
            // btClearPageCache
            // 
            this.btClearPageCache.Location = new System.Drawing.Point(218, 80);
            this.btClearPageCache.Name = "btClearPageCache";
            this.btClearPageCache.Size = new System.Drawing.Size(74, 21);
            this.btClearPageCache.TabIndex = 10;
            this.btClearPageCache.Text = "Clear";
            this.btClearPageCache.UseVisualStyleBackColor = true;
            this.btClearPageCache.Click += new System.EventHandler(this.btClearPageCache_Click);
            // 
            // numPageCacheSize
            // 
            this.numPageCacheSize.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPageCacheSize.Location = new System.Drawing.Point(145, 82);
            this.numPageCacheSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numPageCacheSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPageCacheSize.Name = "numPageCacheSize";
            this.numPageCacheSize.Size = new System.Drawing.Size(67, 20);
            this.numPageCacheSize.TabIndex = 9;
            this.numPageCacheSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPageCacheSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numInternetCacheSize
            // 
            this.numInternetCacheSize.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numInternetCacheSize.Location = new System.Drawing.Point(145, 29);
            this.numInternetCacheSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numInternetCacheSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numInternetCacheSize.Name = "numInternetCacheSize";
            this.numInternetCacheSize.Size = new System.Drawing.Size(67, 20);
            this.numInternetCacheSize.TabIndex = 1;
            this.numInternetCacheSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numInternetCacheSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btClearThumbnailCache
            // 
            this.btClearThumbnailCache.Location = new System.Drawing.Point(218, 54);
            this.btClearThumbnailCache.Name = "btClearThumbnailCache";
            this.btClearThumbnailCache.Size = new System.Drawing.Size(74, 21);
            this.btClearThumbnailCache.TabIndex = 6;
            this.btClearThumbnailCache.Text = "Clear";
            this.btClearThumbnailCache.UseVisualStyleBackColor = true;
            this.btClearThumbnailCache.Click += new System.EventHandler(this.btClearThumbnailCache_Click);
            // 
            // btClearInternetCache
            // 
            this.btClearInternetCache.Location = new System.Drawing.Point(218, 27);
            this.btClearInternetCache.Name = "btClearInternetCache";
            this.btClearInternetCache.Size = new System.Drawing.Size(74, 21);
            this.btClearInternetCache.TabIndex = 2;
            this.btClearInternetCache.Text = "Clear";
            this.btClearInternetCache.UseVisualStyleBackColor = true;
            this.btClearInternetCache.Click += new System.EventHandler(this.btClearInternetCache_Click);
            // 
            // chkEnablePageCache
            // 
            this.chkEnablePageCache.AutoSize = true;
            this.chkEnablePageCache.Location = new System.Drawing.Point(22, 84);
            this.chkEnablePageCache.Name = "chkEnablePageCache";
            this.chkEnablePageCache.Size = new System.Drawing.Size(81, 17);
            this.chkEnablePageCache.TabIndex = 8;
            this.chkEnablePageCache.Text = "&Pages [MB]";
            this.chkEnablePageCache.UseVisualStyleBackColor = true;
            // 
            // lblPageCacheUsage
            // 
            this.lblPageCacheUsage.AutoSize = true;
            this.lblPageCacheUsage.Location = new System.Drawing.Point(298, 86);
            this.lblPageCacheUsage.Name = "lblPageCacheUsage";
            this.lblPageCacheUsage.Size = new System.Drawing.Size(98, 13);
            this.lblPageCacheUsage.TabIndex = 11;
            this.lblPageCacheUsage.Text = "usage Page Cache";
            // 
            // numThumbnailCacheSize
            // 
            this.numThumbnailCacheSize.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numThumbnailCacheSize.Location = new System.Drawing.Point(145, 55);
            this.numThumbnailCacheSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numThumbnailCacheSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numThumbnailCacheSize.Name = "numThumbnailCacheSize";
            this.numThumbnailCacheSize.Size = new System.Drawing.Size(67, 20);
            this.numThumbnailCacheSize.TabIndex = 5;
            this.numThumbnailCacheSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numThumbnailCacheSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // chkEnableThumbnailCache
            // 
            this.chkEnableThumbnailCache.AutoSize = true;
            this.chkEnableThumbnailCache.Location = new System.Drawing.Point(22, 57);
            this.chkEnableThumbnailCache.Name = "chkEnableThumbnailCache";
            this.chkEnableThumbnailCache.Size = new System.Drawing.Size(105, 17);
            this.chkEnableThumbnailCache.TabIndex = 4;
            this.chkEnableThumbnailCache.Text = "&Thumbnails [MB]";
            this.chkEnableThumbnailCache.UseVisualStyleBackColor = true;
            // 
            // lblThumbCacheUsage
            // 
            this.lblThumbCacheUsage.AutoSize = true;
            this.lblThumbCacheUsage.Location = new System.Drawing.Point(298, 58);
            this.lblThumbCacheUsage.Name = "lblThumbCacheUsage";
            this.lblThumbCacheUsage.Size = new System.Drawing.Size(106, 13);
            this.lblThumbCacheUsage.TabIndex = 7;
            this.lblThumbCacheUsage.Text = "usage Thumb Cache";
            // 
            // grpDatabaseBackup
            // 
            this.grpDatabaseBackup.Controls.Add(this.btRestoreDatabase);
            this.grpDatabaseBackup.Controls.Add(this.btBackupDatabase);
            this.grpDatabaseBackup.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDatabaseBackup.Location = new System.Drawing.Point(0, 548);
            this.grpDatabaseBackup.Name = "grpDatabaseBackup";
            this.grpDatabaseBackup.Size = new System.Drawing.Size(498, 93);
            this.grpDatabaseBackup.TabIndex = 4;
            this.grpDatabaseBackup.Text = "Database Backup";
            // 
            // btRestoreDatabase
            // 
            this.btRestoreDatabase.Location = new System.Drawing.Point(259, 41);
            this.btRestoreDatabase.Name = "btRestoreDatabase";
            this.btRestoreDatabase.Size = new System.Drawing.Size(227, 23);
            this.btRestoreDatabase.TabIndex = 1;
            this.btRestoreDatabase.Text = "Restore Database...";
            this.btRestoreDatabase.UseVisualStyleBackColor = true;
            this.btRestoreDatabase.Click += new System.EventHandler(this.btRestoreDatabase_Click);
            // 
            // btBackupDatabase
            // 
            this.btBackupDatabase.Location = new System.Drawing.Point(9, 41);
            this.btBackupDatabase.Name = "btBackupDatabase";
            this.btBackupDatabase.Size = new System.Drawing.Size(247, 23);
            this.btBackupDatabase.TabIndex = 0;
            this.btBackupDatabase.Text = "Backup Database...";
            this.btBackupDatabase.UseVisualStyleBackColor = true;
            this.btBackupDatabase.Click += new System.EventHandler(this.btBackupDatabase_Click);
            // 
            // groupOtherComics
            // 
            this.groupOtherComics.Controls.Add(this.chkUpdateComicFiles);
            this.groupOtherComics.Controls.Add(this.labelExcludeCover);
            this.groupOtherComics.Controls.Add(this.chkAutoUpdateComicFiles);
            this.groupOtherComics.Controls.Add(this.txCoverFilter);
            this.groupOtherComics.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupOtherComics.Location = new System.Drawing.Point(0, 372);
            this.groupOtherComics.Name = "groupOtherComics";
            this.groupOtherComics.Size = new System.Drawing.Size(498, 176);
            this.groupOtherComics.TabIndex = 5;
            this.groupOtherComics.Text = "Books";
            // 
            // chkUpdateComicFiles
            // 
            this.chkUpdateComicFiles.AutoSize = true;
            this.chkUpdateComicFiles.Location = new System.Drawing.Point(9, 42);
            this.chkUpdateComicFiles.Name = "chkUpdateComicFiles";
            this.chkUpdateComicFiles.Size = new System.Drawing.Size(185, 17);
            this.chkUpdateComicFiles.TabIndex = 0;
            this.chkUpdateComicFiles.Text = "Allow writing of Book info into files";
            this.chkUpdateComicFiles.UseVisualStyleBackColor = true;
            // 
            // labelExcludeCover
            // 
            this.labelExcludeCover.AutoSize = true;
            this.labelExcludeCover.Location = new System.Drawing.Point(6, 93);
            this.labelExcludeCover.Name = "labelExcludeCover";
            this.labelExcludeCover.Size = new System.Drawing.Size(381, 13);
            this.labelExcludeCover.TabIndex = 2;
            this.labelExcludeCover.Text = "Semicolon separated list of image names never to be used as cover thumbnails:";
            // 
            // chkAutoUpdateComicFiles
            // 
            this.chkAutoUpdateComicFiles.AutoSize = true;
            this.chkAutoUpdateComicFiles.Location = new System.Drawing.Point(9, 65);
            this.chkAutoUpdateComicFiles.Name = "chkAutoUpdateComicFiles";
            this.chkAutoUpdateComicFiles.Size = new System.Drawing.Size(196, 17);
            this.chkAutoUpdateComicFiles.TabIndex = 1;
            this.chkAutoUpdateComicFiles.Text = "Book files are updated automatically";
            this.chkAutoUpdateComicFiles.UseVisualStyleBackColor = true;
            // 
            // txCoverFilter
            // 
            this.txCoverFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txCoverFilter.Location = new System.Drawing.Point(9, 112);
            this.txCoverFilter.Multiline = true;
            this.txCoverFilter.Name = "txCoverFilter";
            this.txCoverFilter.Size = new System.Drawing.Size(482, 54);
            this.txCoverFilter.TabIndex = 3;
            // 
            // grpLanguages
            // 
            this.grpLanguages.Controls.Add(this.btTranslate);
            this.grpLanguages.Controls.Add(this.labelLanguage);
            this.grpLanguages.Controls.Add(this.lbLanguages);
            this.grpLanguages.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpLanguages.Location = new System.Drawing.Point(0, 0);
            this.grpLanguages.Name = "grpLanguages";
            this.grpLanguages.Size = new System.Drawing.Size(498, 372);
            this.grpLanguages.TabIndex = 7;
            this.grpLanguages.Text = "Languages";
            // 
            // btTranslate
            // 
            this.btTranslate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btTranslate.Location = new System.Drawing.Point(207, 339);
            this.btTranslate.Name = "btTranslate";
            this.btTranslate.Size = new System.Drawing.Size(284, 23);
            this.btTranslate.TabIndex = 12;
            this.btTranslate.Text = "Help localizing ComicRack...";
            this.btTranslate.UseVisualStyleBackColor = true;
            this.btTranslate.Click += new System.EventHandler(this.btTranslate_Click);
            // 
            // labelLanguage
            // 
            this.labelLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLanguage.Location = new System.Drawing.Point(6, 33);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(485, 35);
            this.labelLanguage.TabIndex = 11;
            this.labelLanguage.Text = "Select the User Interface language for ComicRack (ComicRack must be restarted for" +
    " any change to take effect):";
            // 
            // lbLanguages
            // 
            this.lbLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLanguages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbLanguages.FormattingEnabled = true;
            this.lbLanguages.ItemHeight = 15;
            this.lbLanguages.Location = new System.Drawing.Point(6, 75);
            this.lbLanguages.Name = "lbLanguages";
            this.lbLanguages.Size = new System.Drawing.Size(485, 259);
            this.lbLanguages.TabIndex = 0;
            this.lbLanguages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbLanguages_DrawItem);
            // 
            // pageLibrary
            // 
            this.pageLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pageLibrary.AutoScroll = true;
            this.pageLibrary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageLibrary.Controls.Add(this.grpVirtualTags);
            this.pageLibrary.Controls.Add(this.grpServerSettings);
            this.pageLibrary.Controls.Add(this.grpSharing);
            this.pageLibrary.Controls.Add(this.groupLibraryDisplay);
            this.pageLibrary.Controls.Add(this.grpScanning);
            this.pageLibrary.Controls.Add(this.groupComicFolders);
            this.pageLibrary.Location = new System.Drawing.Point(84, 8);
            this.pageLibrary.Name = "pageLibrary";
            this.pageLibrary.Size = new System.Drawing.Size(517, 408);
            this.pageLibrary.TabIndex = 10;
            // 
            // grpVirtualTags
            // 
            this.grpVirtualTags.Controls.Add(this.grpVtagConfig);
            this.grpVirtualTags.Controls.Add(this.lblVirtualTags);
            this.grpVirtualTags.Controls.Add(this.cbVirtualTags);
            this.grpVirtualTags.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpVirtualTags.Location = new System.Drawing.Point(0, 1058);
            this.grpVirtualTags.Name = "grpVirtualTags";
            this.grpVirtualTags.Size = new System.Drawing.Size(498, 279);
            this.grpVirtualTags.TabIndex = 0;
            this.grpVirtualTags.Text = "Virtual Tags";
            // 
            // grpVtagConfig
            // 
            this.grpVtagConfig.Controls.Add(this.lblCaptionFormat);
            this.grpVtagConfig.Controls.Add(this.txtCaptionFormat);
            this.grpVtagConfig.Controls.Add(this.btInsertValue);
            this.grpVtagConfig.Controls.Add(this.lblVirtualTagDescription);
            this.grpVtagConfig.Controls.Add(this.lblVirtualTagName);
            this.grpVtagConfig.Controls.Add(this.txtVirtualTagDescription);
            this.grpVtagConfig.Controls.Add(this.txtVirtualTagName);
            this.grpVtagConfig.Controls.Add(this.chkVirtualTagEnable);
            this.grpVtagConfig.Controls.Add(this.lblCaptionText);
            this.grpVtagConfig.Controls.Add(this.lblCaptionSuffix);
            this.grpVtagConfig.Controls.Add(this.rtfVirtualTagCaption);
            this.grpVtagConfig.Controls.Add(this.lblFieldConfig);
            this.grpVtagConfig.Controls.Add(this.txtCaptionPrefix);
            this.grpVtagConfig.Controls.Add(this.lblCaptionPrefix);
            this.grpVtagConfig.Controls.Add(this.btnCaptionInsert);
            this.grpVtagConfig.Controls.Add(this.txtCaptionSuffix);
            this.grpVtagConfig.Location = new System.Drawing.Point(14, 63);
            this.grpVtagConfig.Name = "grpVtagConfig";
            this.grpVtagConfig.Size = new System.Drawing.Size(472, 199);
            this.grpVtagConfig.TabIndex = 5;
            this.grpVtagConfig.TabStop = false;
            this.grpVtagConfig.Text = "Config";
            // 
            // lblCaptionFormat
            // 
            this.lblCaptionFormat.AutoSize = true;
            this.lblCaptionFormat.Location = new System.Drawing.Point(275, 151);
            this.lblCaptionFormat.Name = "lblCaptionFormat";
            this.lblCaptionFormat.Size = new System.Drawing.Size(39, 13);
            this.lblCaptionFormat.TabIndex = 28;
            this.lblCaptionFormat.Text = "Format";
            this.lblCaptionFormat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtCaptionFormat
            // 
            this.txtCaptionFormat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCaptionFormat.Location = new System.Drawing.Point(269, 167);
            this.txtCaptionFormat.Name = "txtCaptionFormat";
            this.txtCaptionFormat.Size = new System.Drawing.Size(50, 20);
            this.txtCaptionFormat.TabIndex = 27;
            this.toolTip.SetToolTip(this.txtCaptionFormat, resources.GetString("txtCaptionFormat.ToolTip"));
            this.txtCaptionFormat.WordWrap = false;
            // 
            // btInsertValue
            // 
            this.btInsertValue.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallArrowDown;
            this.btInsertValue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btInsertValue.Location = new System.Drawing.Point(96, 164);
            this.btInsertValue.Name = "btInsertValue";
            this.btInsertValue.Size = new System.Drawing.Size(167, 23);
            this.btInsertValue.TabIndex = 26;
            this.btInsertValue.Text = "Choose Value";
            this.btInsertValue.UseVisualStyleBackColor = true;
            // 
            // lblVirtualTagDescription
            // 
            this.lblVirtualTagDescription.AutoSize = true;
            this.lblVirtualTagDescription.Location = new System.Drawing.Point(72, 49);
            this.lblVirtualTagDescription.Name = "lblVirtualTagDescription";
            this.lblVirtualTagDescription.Size = new System.Drawing.Size(69, 13);
            this.lblVirtualTagDescription.TabIndex = 25;
            this.lblVirtualTagDescription.Text = "Description : ";
            // 
            // lblVirtualTagName
            // 
            this.lblVirtualTagName.AutoSize = true;
            this.lblVirtualTagName.Location = new System.Drawing.Point(72, 23);
            this.lblVirtualTagName.Name = "lblVirtualTagName";
            this.lblVirtualTagName.Size = new System.Drawing.Size(44, 13);
            this.lblVirtualTagName.TabIndex = 24;
            this.lblVirtualTagName.Text = "Name : ";
            // 
            // txtVirtualTagDescription
            // 
            this.txtVirtualTagDescription.Location = new System.Drawing.Point(147, 46);
            this.txtVirtualTagDescription.Name = "txtVirtualTagDescription";
            this.txtVirtualTagDescription.Size = new System.Drawing.Size(308, 20);
            this.txtVirtualTagDescription.TabIndex = 2;
            this.txtVirtualTagDescription.Validated += new System.EventHandler(this.VirtualTag_Validated);
            // 
            // txtVirtualTagName
            // 
            this.txtVirtualTagName.Location = new System.Drawing.Point(122, 20);
            this.txtVirtualTagName.Name = "txtVirtualTagName";
            this.txtVirtualTagName.Size = new System.Drawing.Size(333, 20);
            this.txtVirtualTagName.TabIndex = 1;
            this.txtVirtualTagName.Validated += new System.EventHandler(this.UpdateVirtualTagsComboBox);
            // 
            // chkVirtualTagEnable
            // 
            this.chkVirtualTagEnable.AutoSize = true;
            this.chkVirtualTagEnable.Location = new System.Drawing.Point(10, 34);
            this.chkVirtualTagEnable.Name = "chkVirtualTagEnable";
            this.chkVirtualTagEnable.Size = new System.Drawing.Size(59, 17);
            this.chkVirtualTagEnable.TabIndex = 3;
            this.chkVirtualTagEnable.Text = "Enable";
            this.chkVirtualTagEnable.UseVisualStyleBackColor = true;
            this.chkVirtualTagEnable.Validated += new System.EventHandler(this.UpdateVirtualTagsComboBox);
            // 
            // lblCaptionText
            // 
            this.lblCaptionText.AutoSize = true;
            this.lblCaptionText.Location = new System.Drawing.Point(7, 82);
            this.lblCaptionText.Name = "lblCaptionText";
            this.lblCaptionText.Size = new System.Drawing.Size(52, 13);
            this.lblCaptionText.TabIndex = 3;
            this.lblCaptionText.Text = "Caption : ";
            // 
            // lblCaptionSuffix
            // 
            this.lblCaptionSuffix.AutoSize = true;
            this.lblCaptionSuffix.Location = new System.Drawing.Point(349, 151);
            this.lblCaptionSuffix.Name = "lblCaptionSuffix";
            this.lblCaptionSuffix.Size = new System.Drawing.Size(33, 13);
            this.lblCaptionSuffix.TabIndex = 20;
            this.lblCaptionSuffix.Text = "Suffix";
            // 
            // rtfVirtualTagCaption
            // 
            this.rtfVirtualTagCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtfVirtualTagCaption.DetectUrls = false;
            this.rtfVirtualTagCaption.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfVirtualTagCaption.Location = new System.Drawing.Point(64, 79);
            this.rtfVirtualTagCaption.Name = "rtfVirtualTagCaption";
            this.rtfVirtualTagCaption.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtfVirtualTagCaption.Size = new System.Drawing.Size(391, 62);
            this.rtfVirtualTagCaption.TabIndex = 4;
            this.rtfVirtualTagCaption.Text = "";
            this.rtfVirtualTagCaption.Validated += new System.EventHandler(this.VirtualTag_Validated);
            // 
            // lblFieldConfig
            // 
            this.lblFieldConfig.AutoSize = true;
            this.lblFieldConfig.Location = new System.Drawing.Point(165, 151);
            this.lblFieldConfig.Name = "lblFieldConfig";
            this.lblFieldConfig.Size = new System.Drawing.Size(29, 13);
            this.lblFieldConfig.TabIndex = 19;
            this.lblFieldConfig.Text = "Field";
            this.lblFieldConfig.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtCaptionPrefix
            // 
            this.txtCaptionPrefix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCaptionPrefix.Location = new System.Drawing.Point(10, 167);
            this.txtCaptionPrefix.Name = "txtCaptionPrefix";
            this.txtCaptionPrefix.Size = new System.Drawing.Size(80, 20);
            this.txtCaptionPrefix.TabIndex = 5;
            // 
            // lblCaptionPrefix
            // 
            this.lblCaptionPrefix.AutoSize = true;
            this.lblCaptionPrefix.Location = new System.Drawing.Point(34, 151);
            this.lblCaptionPrefix.Name = "lblCaptionPrefix";
            this.lblCaptionPrefix.Size = new System.Drawing.Size(33, 13);
            this.lblCaptionPrefix.TabIndex = 18;
            this.lblCaptionPrefix.Text = "Prefix";
            // 
            // btnCaptionInsert
            // 
            this.btnCaptionInsert.Location = new System.Drawing.Point(410, 164);
            this.btnCaptionInsert.Name = "btnCaptionInsert";
            this.btnCaptionInsert.Size = new System.Drawing.Size(51, 23);
            this.btnCaptionInsert.TabIndex = 8;
            this.btnCaptionInsert.Text = "Insert";
            this.btnCaptionInsert.UseVisualStyleBackColor = true;
            this.btnCaptionInsert.Click += new System.EventHandler(this.btnCaptionInsert_Click);
            // 
            // txtCaptionSuffix
            // 
            this.txtCaptionSuffix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCaptionSuffix.Location = new System.Drawing.Point(325, 167);
            this.txtCaptionSuffix.Name = "txtCaptionSuffix";
            this.txtCaptionSuffix.Size = new System.Drawing.Size(80, 20);
            this.txtCaptionSuffix.TabIndex = 7;
            // 
            // lblVirtualTags
            // 
            this.lblVirtualTags.AutoSize = true;
            this.lblVirtualTags.Location = new System.Drawing.Point(10, 39);
            this.lblVirtualTags.Name = "lblVirtualTags";
            this.lblVirtualTags.Size = new System.Drawing.Size(45, 13);
            this.lblVirtualTags.TabIndex = 1;
            this.lblVirtualTags.Text = "Tag # : ";
            // 
            // cbVirtualTags
            // 
            this.cbVirtualTags.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVirtualTags.FormattingEnabled = true;
            this.cbVirtualTags.Location = new System.Drawing.Point(61, 36);
            this.cbVirtualTags.Name = "cbVirtualTags";
            this.cbVirtualTags.Size = new System.Drawing.Size(424, 21);
            this.cbVirtualTags.TabIndex = 0;
            this.cbVirtualTags.SelectedIndexChanged += new System.EventHandler(this.cbVirtualTags_SelectedIndexChanged);
            // 
            // grpServerSettings
            // 
            this.grpServerSettings.Controls.Add(this.txPrivateListingPassword);
            this.grpServerSettings.Controls.Add(this.labelPrivateListPassword);
            this.grpServerSettings.Controls.Add(this.labelPublicServerAddress);
            this.grpServerSettings.Controls.Add(this.txPublicServerAddress);
            this.grpServerSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpServerSettings.Location = new System.Drawing.Point(0, 910);
            this.grpServerSettings.Name = "grpServerSettings";
            this.grpServerSettings.Size = new System.Drawing.Size(498, 148);
            this.grpServerSettings.TabIndex = 3;
            this.grpServerSettings.Text = "Server Settings";
            // 
            // txPrivateListingPassword
            // 
            this.txPrivateListingPassword.Location = new System.Drawing.Point(12, 114);
            this.txPrivateListingPassword.Name = "txPrivateListingPassword";
            this.txPrivateListingPassword.Password = null;
            this.txPrivateListingPassword.Size = new System.Drawing.Size(379, 20);
            this.txPrivateListingPassword.TabIndex = 3;
            this.txPrivateListingPassword.UseSystemPasswordChar = true;
            // 
            // labelPrivateListPassword
            // 
            this.labelPrivateListPassword.AutoSize = true;
            this.labelPrivateListPassword.Location = new System.Drawing.Point(13, 96);
            this.labelPrivateListPassword.Name = "labelPrivateListPassword";
            this.labelPrivateListPassword.Size = new System.Drawing.Size(307, 13);
            this.labelPrivateListPassword.TabIndex = 2;
            this.labelPrivateListPassword.Text = "Password used to protect your private Internet Share list entries:";
            // 
            // labelPublicServerAddress
            // 
            this.labelPublicServerAddress.AutoSize = true;
            this.labelPublicServerAddress.Location = new System.Drawing.Point(14, 41);
            this.labelPublicServerAddress.Name = "labelPublicServerAddress";
            this.labelPublicServerAddress.Size = new System.Drawing.Size(368, 13);
            this.labelPublicServerAddress.TabIndex = 0;
            this.labelPublicServerAddress.Text = "External IP address of your server if ComicRack should not guess it correctly:";
            // 
            // txPublicServerAddress
            // 
            this.txPublicServerAddress.Location = new System.Drawing.Point(12, 60);
            this.txPublicServerAddress.Name = "txPublicServerAddress";
            this.txPublicServerAddress.Size = new System.Drawing.Size(379, 20);
            this.txPublicServerAddress.TabIndex = 1;
            // 
            // grpSharing
            // 
            this.grpSharing.Controls.Add(this.chkAutoConnectShares);
            this.grpSharing.Controls.Add(this.btRemoveShare);
            this.grpSharing.Controls.Add(this.btAddShare);
            this.grpSharing.Controls.Add(this.tabShares);
            this.grpSharing.Controls.Add(this.chkLookForShared);
            this.grpSharing.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSharing.Location = new System.Drawing.Point(0, 509);
            this.grpSharing.Name = "grpSharing";
            this.grpSharing.Size = new System.Drawing.Size(498, 401);
            this.grpSharing.TabIndex = 1;
            this.grpSharing.Text = "Sharing";
            // 
            // chkAutoConnectShares
            // 
            this.chkAutoConnectShares.AutoSize = true;
            this.chkAutoConnectShares.Location = new System.Drawing.Point(261, 36);
            this.chkAutoConnectShares.Name = "chkAutoConnectShares";
            this.chkAutoConnectShares.Size = new System.Drawing.Size(130, 17);
            this.chkAutoConnectShares.TabIndex = 1;
            this.chkAutoConnectShares.Text = "Connect automatically";
            this.chkAutoConnectShares.UseVisualStyleBackColor = true;
            // 
            // btRemoveShare
            // 
            this.btRemoveShare.Location = new System.Drawing.Point(398, 89);
            this.btRemoveShare.Name = "btRemoveShare";
            this.btRemoveShare.Size = new System.Drawing.Size(92, 23);
            this.btRemoveShare.TabIndex = 4;
            this.btRemoveShare.Text = "Remove";
            this.btRemoveShare.UseVisualStyleBackColor = true;
            this.btRemoveShare.Click += new System.EventHandler(this.btRmoveShare_Click);
            // 
            // btAddShare
            // 
            this.btAddShare.Location = new System.Drawing.Point(398, 61);
            this.btAddShare.Name = "btAddShare";
            this.btAddShare.Size = new System.Drawing.Size(92, 23);
            this.btAddShare.TabIndex = 3;
            this.btAddShare.Text = "Add Share";
            this.btAddShare.UseVisualStyleBackColor = true;
            this.btAddShare.Click += new System.EventHandler(this.btAddShare_Click);
            // 
            // tabShares
            // 
            this.tabShares.Location = new System.Drawing.Point(12, 59);
            this.tabShares.Name = "tabShares";
            this.tabShares.SelectedIndex = 0;
            this.tabShares.Size = new System.Drawing.Size(381, 336);
            this.tabShares.TabIndex = 2;
            // 
            // chkLookForShared
            // 
            this.chkLookForShared.AutoSize = true;
            this.chkLookForShared.Location = new System.Drawing.Point(12, 36);
            this.chkLookForShared.Name = "chkLookForShared";
            this.chkLookForShared.Size = new System.Drawing.Size(154, 17);
            this.chkLookForShared.TabIndex = 0;
            this.chkLookForShared.Text = "Look for local Book Shares";
            this.chkLookForShared.UseVisualStyleBackColor = true;
            // 
            // groupLibraryDisplay
            // 
            this.groupLibraryDisplay.Controls.Add(this.chkLibraryGaugesTotal);
            this.groupLibraryDisplay.Controls.Add(this.chkLibraryGaugesUnread);
            this.groupLibraryDisplay.Controls.Add(this.chkLibraryGaugesNumeric);
            this.groupLibraryDisplay.Controls.Add(this.chkLibraryGaugesNew);
            this.groupLibraryDisplay.Controls.Add(this.chkLibraryGauges);
            this.groupLibraryDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupLibraryDisplay.Location = new System.Drawing.Point(0, 339);
            this.groupLibraryDisplay.Name = "groupLibraryDisplay";
            this.groupLibraryDisplay.Size = new System.Drawing.Size(498, 170);
            this.groupLibraryDisplay.TabIndex = 4;
            this.groupLibraryDisplay.Text = "Display";
            // 
            // chkLibraryGaugesTotal
            // 
            this.chkLibraryGaugesTotal.AutoSize = true;
            this.chkLibraryGaugesTotal.Location = new System.Drawing.Point(33, 111);
            this.chkLibraryGaugesTotal.Name = "chkLibraryGaugesTotal";
            this.chkLibraryGaugesTotal.Size = new System.Drawing.Size(113, 17);
            this.chkLibraryGaugesTotal.TabIndex = 1;
            this.chkLibraryGaugesTotal.Text = "For Total of Books";
            this.chkLibraryGaugesTotal.UseVisualStyleBackColor = true;
            // 
            // chkLibraryGaugesUnread
            // 
            this.chkLibraryGaugesUnread.AutoSize = true;
            this.chkLibraryGaugesUnread.Location = new System.Drawing.Point(33, 92);
            this.chkLibraryGaugesUnread.Name = "chkLibraryGaugesUnread";
            this.chkLibraryGaugesUnread.Size = new System.Drawing.Size(112, 17);
            this.chkLibraryGaugesUnread.TabIndex = 1;
            this.chkLibraryGaugesUnread.Text = "For Unread Books";
            this.chkLibraryGaugesUnread.UseVisualStyleBackColor = true;
            // 
            // chkLibraryGaugesNumeric
            // 
            this.chkLibraryGaugesNumeric.AutoSize = true;
            this.chkLibraryGaugesNumeric.Location = new System.Drawing.Point(33, 131);
            this.chkLibraryGaugesNumeric.Name = "chkLibraryGaugesNumeric";
            this.chkLibraryGaugesNumeric.Size = new System.Drawing.Size(201, 17);
            this.chkLibraryGaugesNumeric.TabIndex = 1;
            this.chkLibraryGaugesNumeric.Text = "Also show numbers and not only bars";
            this.chkLibraryGaugesNumeric.UseVisualStyleBackColor = true;
            // 
            // chkLibraryGaugesNew
            // 
            this.chkLibraryGaugesNew.AutoSize = true;
            this.chkLibraryGaugesNew.Location = new System.Drawing.Point(33, 72);
            this.chkLibraryGaugesNew.Name = "chkLibraryGaugesNew";
            this.chkLibraryGaugesNew.Size = new System.Drawing.Size(99, 17);
            this.chkLibraryGaugesNew.TabIndex = 1;
            this.chkLibraryGaugesNew.Text = "For New Books";
            this.chkLibraryGaugesNew.UseVisualStyleBackColor = true;
            // 
            // chkLibraryGauges
            // 
            this.chkLibraryGauges.AutoSize = true;
            this.chkLibraryGauges.Location = new System.Drawing.Point(12, 42);
            this.chkLibraryGauges.Name = "chkLibraryGauges";
            this.chkLibraryGauges.Size = new System.Drawing.Size(127, 17);
            this.chkLibraryGauges.TabIndex = 0;
            this.chkLibraryGauges.Text = "Enable Live Counters";
            this.chkLibraryGauges.UseVisualStyleBackColor = true;
            this.chkLibraryGauges.CheckedChanged += new System.EventHandler(this.chkLibraryGauges_CheckedChanged);
            // 
            // grpScanning
            // 
            this.grpScanning.Controls.Add(this.chkDontAddRemovedFiles);
            this.grpScanning.Controls.Add(this.chkAutoRemoveMissing);
            this.grpScanning.Controls.Add(this.lblScan);
            this.grpScanning.Controls.Add(this.btScan);
            this.grpScanning.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpScanning.Location = new System.Drawing.Point(0, 203);
            this.grpScanning.Name = "grpScanning";
            this.grpScanning.Size = new System.Drawing.Size(498, 136);
            this.grpScanning.TabIndex = 0;
            this.grpScanning.Text = "Scanning";
            // 
            // chkDontAddRemovedFiles
            // 
            this.chkDontAddRemovedFiles.AutoSize = true;
            this.chkDontAddRemovedFiles.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkDontAddRemovedFiles.Location = new System.Drawing.Point(12, 58);
            this.chkDontAddRemovedFiles.Name = "chkDontAddRemovedFiles";
            this.chkDontAddRemovedFiles.Size = new System.Drawing.Size(322, 17);
            this.chkDontAddRemovedFiles.TabIndex = 1;
            this.chkDontAddRemovedFiles.Text = "Files manually removed from the Library will not be added again";
            this.chkDontAddRemovedFiles.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkDontAddRemovedFiles.UseVisualStyleBackColor = true;
            // 
            // chkAutoRemoveMissing
            // 
            this.chkAutoRemoveMissing.AutoSize = true;
            this.chkAutoRemoveMissing.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkAutoRemoveMissing.Location = new System.Drawing.Point(12, 35);
            this.chkAutoRemoveMissing.Name = "chkAutoRemoveMissing";
            this.chkAutoRemoveMissing.Size = new System.Drawing.Size(301, 17);
            this.chkAutoRemoveMissing.TabIndex = 0;
            this.chkAutoRemoveMissing.Text = "Automatically remove missing files from Library during Scan";
            this.chkAutoRemoveMissing.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkAutoRemoveMissing.UseVisualStyleBackColor = true;
            // 
            // lblScan
            // 
            this.lblScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScan.Location = new System.Drawing.Point(9, 83);
            this.lblScan.Name = "lblScan";
            this.lblScan.Size = new System.Drawing.Size(480, 43);
            this.lblScan.TabIndex = 8;
            this.lblScan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btScan
            // 
            this.btScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btScan.Location = new System.Drawing.Point(403, 33);
            this.btScan.Name = "btScan";
            this.btScan.Size = new System.Drawing.Size(88, 23);
            this.btScan.TabIndex = 2;
            this.btScan.Text = "Scan";
            this.btScan.UseVisualStyleBackColor = true;
            this.btScan.Click += new System.EventHandler(this.btScan_Click);
            // 
            // groupComicFolders
            // 
            this.groupComicFolders.Controls.Add(this.btOpenFolder);
            this.groupComicFolders.Controls.Add(this.btChangeFolder);
            this.groupComicFolders.Controls.Add(this.lbPaths);
            this.groupComicFolders.Controls.Add(this.labelWatchedFolders);
            this.groupComicFolders.Controls.Add(this.btRemoveFolder);
            this.groupComicFolders.Controls.Add(this.btAddFolder);
            this.groupComicFolders.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupComicFolders.Location = new System.Drawing.Point(0, 0);
            this.groupComicFolders.Name = "groupComicFolders";
            this.groupComicFolders.Size = new System.Drawing.Size(498, 203);
            this.groupComicFolders.TabIndex = 0;
            this.groupComicFolders.Text = "Book Folders";
            // 
            // btOpenFolder
            // 
            this.btOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOpenFolder.Location = new System.Drawing.Point(400, 134);
            this.btOpenFolder.Name = "btOpenFolder";
            this.btOpenFolder.Size = new System.Drawing.Size(89, 23);
            this.btOpenFolder.TabIndex = 4;
            this.btOpenFolder.Text = "Open";
            this.btOpenFolder.UseVisualStyleBackColor = true;
            this.btOpenFolder.Click += new System.EventHandler(this.btOpenFolder_Click);
            // 
            // btChangeFolder
            // 
            this.btChangeFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btChangeFolder.Location = new System.Drawing.Point(400, 66);
            this.btChangeFolder.Name = "btChangeFolder";
            this.btChangeFolder.Size = new System.Drawing.Size(89, 23);
            this.btChangeFolder.TabIndex = 2;
            this.btChangeFolder.Text = "&Change...";
            this.btChangeFolder.UseVisualStyleBackColor = true;
            this.btChangeFolder.Click += new System.EventHandler(this.btChangeFolder_Click);
            // 
            // lbPaths
            // 
            this.lbPaths.AllowDrop = true;
            this.lbPaths.FormattingEnabled = true;
            this.lbPaths.IntegralHeight = false;
            this.lbPaths.Location = new System.Drawing.Point(12, 37);
            this.lbPaths.Name = "lbPaths";
            this.lbPaths.Size = new System.Drawing.Size(377, 120);
            this.lbPaths.TabIndex = 0;
            this.lbPaths.DrawItemText += new System.Windows.Forms.DrawItemEventHandler(this.lbPaths_DrawItemText);
            this.lbPaths.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbPaths_DrawItem);
            this.lbPaths.SelectedIndexChanged += new System.EventHandler(this.lbPaths_SelectedIndexChanged);
            this.lbPaths.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbPaths_DragDrop);
            this.lbPaths.DragOver += new System.Windows.Forms.DragEventHandler(this.lbPaths_DragOver);
            // 
            // labelWatchedFolders
            // 
            this.labelWatchedFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWatchedFolders.Location = new System.Drawing.Point(9, 163);
            this.labelWatchedFolders.Name = "labelWatchedFolders";
            this.labelWatchedFolders.Size = new System.Drawing.Size(480, 26);
            this.labelWatchedFolders.TabIndex = 0;
            this.labelWatchedFolders.Text = "Checked folders will be watched for changes (rename, move) while the program is r" +
    "unning.";
            this.labelWatchedFolders.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btRemoveFolder
            // 
            this.btRemoveFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemoveFolder.Location = new System.Drawing.Point(400, 95);
            this.btRemoveFolder.Name = "btRemoveFolder";
            this.btRemoveFolder.Size = new System.Drawing.Size(89, 23);
            this.btRemoveFolder.TabIndex = 3;
            this.btRemoveFolder.Text = "&Remove";
            this.btRemoveFolder.UseVisualStyleBackColor = true;
            this.btRemoveFolder.Click += new System.EventHandler(this.btRemoveFolder_Click);
            // 
            // btAddFolder
            // 
            this.btAddFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddFolder.Location = new System.Drawing.Point(400, 37);
            this.btAddFolder.Name = "btAddFolder";
            this.btAddFolder.Size = new System.Drawing.Size(89, 23);
            this.btAddFolder.TabIndex = 1;
            this.btAddFolder.Text = "&Add...";
            this.btAddFolder.UseVisualStyleBackColor = true;
            this.btAddFolder.Click += new System.EventHandler(this.btAddFolder_Click);
            // 
            // memCacheUpate
            // 
            this.memCacheUpate.Enabled = true;
            this.memCacheUpate.Interval = 1000;
            this.memCacheUpate.Tick += new System.EventHandler(this.memCacheUpate_Tick);
            // 
            // pageScripts
            // 
            this.pageScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pageScripts.AutoScroll = true;
            this.pageScripts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pageScripts.Controls.Add(this.grpScriptSettings);
            this.pageScripts.Controls.Add(this.grpScripts);
            this.pageScripts.Controls.Add(this.grpPackages);
            this.pageScripts.Location = new System.Drawing.Point(84, 8);
            this.pageScripts.Name = "pageScripts";
            this.pageScripts.Size = new System.Drawing.Size(517, 408);
            this.pageScripts.TabIndex = 11;
            // 
            // grpScriptSettings
            // 
            this.grpScriptSettings.Controls.Add(this.btAddLibraryFolder);
            this.grpScriptSettings.Controls.Add(this.chkDisableScripting);
            this.grpScriptSettings.Controls.Add(this.labelScriptPaths);
            this.grpScriptSettings.Controls.Add(this.txLibraries);
            this.grpScriptSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpScriptSettings.Location = new System.Drawing.Point(0, 752);
            this.grpScriptSettings.Name = "grpScriptSettings";
            this.grpScriptSettings.Size = new System.Drawing.Size(498, 192);
            this.grpScriptSettings.TabIndex = 5;
            this.grpScriptSettings.Text = "Script Settings";
            // 
            // btAddLibraryFolder
            // 
            this.btAddLibraryFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAddLibraryFolder.Location = new System.Drawing.Point(369, 163);
            this.btAddLibraryFolder.Name = "btAddLibraryFolder";
            this.btAddLibraryFolder.Size = new System.Drawing.Size(121, 23);
            this.btAddLibraryFolder.TabIndex = 3;
            this.btAddLibraryFolder.Text = "Add Folder...";
            this.btAddLibraryFolder.UseVisualStyleBackColor = true;
            this.btAddLibraryFolder.Click += new System.EventHandler(this.btAddLibraryFolder_Click);
            // 
            // chkDisableScripting
            // 
            this.chkDisableScripting.AutoSize = true;
            this.chkDisableScripting.Location = new System.Drawing.Point(9, 39);
            this.chkDisableScripting.Name = "chkDisableScripting";
            this.chkDisableScripting.Size = new System.Drawing.Size(109, 17);
            this.chkDisableScripting.TabIndex = 0;
            this.chkDisableScripting.Text = "Disable all Scripts";
            this.chkDisableScripting.UseVisualStyleBackColor = true;
            // 
            // labelScriptPaths
            // 
            this.labelScriptPaths.Location = new System.Drawing.Point(6, 60);
            this.labelScriptPaths.Name = "labelScriptPaths";
            this.labelScriptPaths.Size = new System.Drawing.Size(478, 29);
            this.labelScriptPaths.TabIndex = 1;
            this.labelScriptPaths.Text = "Semicolon separated list of library paths for scripts (e.g. python libraries):";
            this.labelScriptPaths.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txLibraries
            // 
            this.txLibraries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txLibraries.Location = new System.Drawing.Point(7, 92);
            this.txLibraries.Multiline = true;
            this.txLibraries.Name = "txLibraries";
            this.txLibraries.Size = new System.Drawing.Size(482, 63);
            this.txLibraries.TabIndex = 2;
            // 
            // grpScripts
            // 
            this.grpScripts.Controls.Add(this.chkHideSampleScripts);
            this.grpScripts.Controls.Add(this.btConfigScript);
            this.grpScripts.Controls.Add(this.lvScripts);
            this.grpScripts.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpScripts.Location = new System.Drawing.Point(0, 378);
            this.grpScripts.Name = "grpScripts";
            this.grpScripts.Size = new System.Drawing.Size(498, 374);
            this.grpScripts.TabIndex = 4;
            this.grpScripts.Text = "Available Scripts";
            // 
            // chkHideSampleScripts
            // 
            this.chkHideSampleScripts.AutoSize = true;
            this.chkHideSampleScripts.Location = new System.Drawing.Point(9, 345);
            this.chkHideSampleScripts.Name = "chkHideSampleScripts";
            this.chkHideSampleScripts.Size = new System.Drawing.Size(119, 17);
            this.chkHideSampleScripts.TabIndex = 8;
            this.chkHideSampleScripts.Text = "Hide sample Scripts";
            this.chkHideSampleScripts.UseVisualStyleBackColor = true;
            this.chkHideSampleScripts.CheckedChanged += new System.EventHandler(this.chkHideSampleScripts_CheckedChanged);
            // 
            // btConfigScript
            // 
            this.btConfigScript.Enabled = false;
            this.btConfigScript.Location = new System.Drawing.Point(398, 339);
            this.btConfigScript.Name = "btConfigScript";
            this.btConfigScript.Size = new System.Drawing.Size(87, 23);
            this.btConfigScript.TabIndex = 7;
            this.btConfigScript.Text = "Configure...";
            this.btConfigScript.UseVisualStyleBackColor = true;
            this.btConfigScript.Click += new System.EventHandler(this.btConfigScript_Click);
            // 
            // lvScripts
            // 
            this.lvScripts.CheckBoxes = true;
            this.lvScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chScriptName,
            this.chScriptPackage});
            this.lvScripts.FullRowSelect = true;
            this.lvScripts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvScripts.HideSelection = false;
            this.lvScripts.Location = new System.Drawing.Point(9, 42);
            this.lvScripts.MultiSelect = false;
            this.lvScripts.Name = "lvScripts";
            this.lvScripts.ShowItemToolTips = true;
            this.lvScripts.Size = new System.Drawing.Size(476, 291);
            this.lvScripts.SmallImageList = this.imageList;
            this.lvScripts.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvScripts.TabIndex = 6;
            this.lvScripts.UseCompatibleStateImageBehavior = false;
            this.lvScripts.View = System.Windows.Forms.View.Details;
            this.lvScripts.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvScripts_ItemChecked);
            this.lvScripts.SelectedIndexChanged += new System.EventHandler(this.lvScripts_SelectedIndexChanged);
            // 
            // chScriptName
            // 
            this.chScriptName.Text = "Name";
            this.chScriptName.Width = 250;
            // 
            // chScriptPackage
            // 
            this.chScriptPackage.Text = "Package";
            this.chScriptPackage.Width = 190;
            // 
            // grpPackages
            // 
            this.grpPackages.Controls.Add(this.btRemovePackage);
            this.grpPackages.Controls.Add(this.btInstallPackage);
            this.grpPackages.Controls.Add(this.lvPackages);
            this.grpPackages.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPackages.Location = new System.Drawing.Point(0, 0);
            this.grpPackages.Name = "grpPackages";
            this.grpPackages.Size = new System.Drawing.Size(498, 378);
            this.grpPackages.TabIndex = 13;
            this.grpPackages.Text = "Script Packages";
            // 
            // btRemovePackage
            // 
            this.btRemovePackage.Location = new System.Drawing.Point(398, 344);
            this.btRemovePackage.Name = "btRemovePackage";
            this.btRemovePackage.Size = new System.Drawing.Size(86, 23);
            this.btRemovePackage.TabIndex = 2;
            this.btRemovePackage.Text = "Remove";
            this.btRemovePackage.UseVisualStyleBackColor = true;
            this.btRemovePackage.Click += new System.EventHandler(this.btRemovePackage_Click);
            // 
            // btInstallPackage
            // 
            this.btInstallPackage.Location = new System.Drawing.Point(306, 344);
            this.btInstallPackage.Name = "btInstallPackage";
            this.btInstallPackage.Size = new System.Drawing.Size(86, 23);
            this.btInstallPackage.TabIndex = 1;
            this.btInstallPackage.Text = "Install...";
            this.btInstallPackage.UseVisualStyleBackColor = true;
            this.btInstallPackage.Click += new System.EventHandler(this.btInstallPackage_Click);
            // 
            // lvPackages
            // 
            this.lvPackages.AllowDrop = true;
            this.lvPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPackageName,
            this.chPackageAuthor,
            this.chPackageDescription});
            listViewGroup1.Header = "Installed";
            listViewGroup1.Name = "packageGroupInstalled";
            listViewGroup2.Header = "To be removed (requires restart)";
            listViewGroup2.Name = "packageGroupRemove";
            listViewGroup3.Header = "To be installed (requires restart)";
            listViewGroup3.Name = "packageGroupInstall";
            this.lvPackages.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.lvPackages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvPackages.HideSelection = false;
            this.lvPackages.LargeImageList = this.packageImageList;
            this.lvPackages.Location = new System.Drawing.Point(16, 37);
            this.lvPackages.Name = "lvPackages";
            this.lvPackages.ShowItemToolTips = true;
            this.lvPackages.Size = new System.Drawing.Size(468, 301);
            this.lvPackages.SmallImageList = this.packageImageList;
            this.lvPackages.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvPackages.TabIndex = 0;
            this.lvPackages.UseCompatibleStateImageBehavior = false;
            this.lvPackages.View = System.Windows.Forms.View.Details;
            this.lvPackages.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvPackages_DragDrop);
            this.lvPackages.DragOver += new System.Windows.Forms.DragEventHandler(this.lvPackages_DragOver);
            this.lvPackages.DoubleClick += new System.EventHandler(this.lvPackages_DoubleClick);
            // 
            // chPackageName
            // 
            this.chPackageName.Text = "Package";
            this.chPackageName.Width = 130;
            // 
            // chPackageAuthor
            // 
            this.chPackageAuthor.Text = "Author";
            this.chPackageAuthor.Width = 89;
            // 
            // chPackageDescription
            // 
            this.chPackageDescription.Text = "Description";
            this.chPackageDescription.Width = 217;
            // 
            // packageImageList
            // 
            this.packageImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.packageImageList.ImageSize = new System.Drawing.Size(32, 32);
            this.packageImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabReader
            // 
            this.tabReader.Appearance = System.Windows.Forms.Appearance.Button;
            this.tabReader.AutoEllipsis = true;
            this.tabReader.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ReaderPref;
            this.tabReader.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.tabReader.Location = new System.Drawing.Point(3, 7);
            this.tabReader.Name = "tabReader";
            this.tabReader.Size = new System.Drawing.Size(75, 56);
            this.tabReader.TabIndex = 13;
            this.tabReader.Text = "Reader";
            this.tabReader.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tabReader.UseVisualStyleBackColor = true;
            this.tabReader.CheckedChanged += new System.EventHandler(this.chkAdvanced_CheckedChanged);
            // 
            // tabLibraries
            // 
            this.tabLibraries.Appearance = System.Windows.Forms.Appearance.Button;
            this.tabLibraries.AutoEllipsis = true;
            this.tabLibraries.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.LibraryPref;
            this.tabLibraries.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.tabLibraries.Location = new System.Drawing.Point(3, 66);
            this.tabLibraries.Name = "tabLibraries";
            this.tabLibraries.Size = new System.Drawing.Size(75, 56);
            this.tabLibraries.TabIndex = 14;
            this.tabLibraries.Text = "Libraries";
            this.tabLibraries.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tabLibraries.UseVisualStyleBackColor = true;
            this.tabLibraries.CheckedChanged += new System.EventHandler(this.chkAdvanced_CheckedChanged);
            // 
            // tabBehavior
            // 
            this.tabBehavior.Appearance = System.Windows.Forms.Appearance.Button;
            this.tabBehavior.AutoEllipsis = true;
            this.tabBehavior.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.BehaviorPref;
            this.tabBehavior.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.tabBehavior.Location = new System.Drawing.Point(3, 126);
            this.tabBehavior.Name = "tabBehavior";
            this.tabBehavior.Size = new System.Drawing.Size(75, 56);
            this.tabBehavior.TabIndex = 15;
            this.tabBehavior.Text = "Behavior";
            this.tabBehavior.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tabBehavior.UseVisualStyleBackColor = true;
            this.tabBehavior.CheckedChanged += new System.EventHandler(this.chkAdvanced_CheckedChanged);
            // 
            // tabScripts
            // 
            this.tabScripts.Appearance = System.Windows.Forms.Appearance.Button;
            this.tabScripts.AutoEllipsis = true;
            this.tabScripts.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ScriptingPref;
            this.tabScripts.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.tabScripts.Location = new System.Drawing.Point(3, 187);
            this.tabScripts.Name = "tabScripts";
            this.tabScripts.Size = new System.Drawing.Size(75, 56);
            this.tabScripts.TabIndex = 16;
            this.tabScripts.Text = "Scripts";
            this.tabScripts.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tabScripts.UseVisualStyleBackColor = true;
            this.tabScripts.CheckedChanged += new System.EventHandler(this.chkAdvanced_CheckedChanged);
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Appearance = System.Windows.Forms.Appearance.Button;
            this.tabAdvanced.AutoEllipsis = true;
            this.tabAdvanced.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AdvancedPref;
            this.tabAdvanced.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.tabAdvanced.Location = new System.Drawing.Point(3, 248);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Size = new System.Drawing.Size(75, 56);
            this.tabAdvanced.TabIndex = 17;
            this.tabAdvanced.Text = "Advanced";
            this.tabAdvanced.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tabAdvanced.UseVisualStyleBackColor = true;
            this.tabAdvanced.CheckedChanged += new System.EventHandler(this.chkAdvanced_CheckedChanged);
            // 
            // PreferencesDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(610, 453);
            this.Controls.Add(this.pageAdvanced);
            this.Controls.Add(this.tabAdvanced);
            this.Controls.Add(this.tabScripts);
            this.Controls.Add(this.tabBehavior);
            this.Controls.Add(this.tabLibraries);
            this.Controls.Add(this.tabReader);
            this.Controls.Add(this.pageReader);
            this.Controls.Add(this.pageLibrary);
            this.Controls.Add(this.pageScripts);
            this.Controls.Add(this.pageBehavior);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.pageReader.ResumeLayout(false);
            this.groupHardwareAcceleration.ResumeLayout(false);
            this.groupHardwareAcceleration.PerformLayout();
            this.grpMouse.ResumeLayout(false);
            this.grpMouse.PerformLayout();
            this.groupOverlays.ResumeLayout(false);
            this.groupOverlays.PerformLayout();
            this.panelReaderOverlays.ResumeLayout(false);
            this.grpKeyboard.ResumeLayout(false);
            this.cmKeyboardLayout.ResumeLayout(false);
            this.grpDisplay.ResumeLayout(false);
            this.grpDisplay.PerformLayout();
            this.pageAdvanced.ResumeLayout(false);
            this.grpWirelessSetup.ResumeLayout(false);
            this.grpWirelessSetup.PerformLayout();
            this.grpIntegration.ResumeLayout(false);
            this.grpIntegration.PerformLayout();
            this.groupMessagesAndSocial.ResumeLayout(false);
            this.groupMemory.ResumeLayout(false);
            this.grpMaximumMemoryUsage.ResumeLayout(false);
            this.grpMaximumMemoryUsage.PerformLayout();
            this.grpMemoryCache.ResumeLayout(false);
            this.grpMemoryCache.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMemPageCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMemThumbSize)).EndInit();
            this.grpDiskCache.ResumeLayout(false);
            this.grpDiskCache.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageCacheSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInternetCacheSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThumbnailCacheSize)).EndInit();
            this.grpDatabaseBackup.ResumeLayout(false);
            this.groupOtherComics.ResumeLayout(false);
            this.groupOtherComics.PerformLayout();
            this.grpLanguages.ResumeLayout(false);
            this.pageLibrary.ResumeLayout(false);
            this.grpVirtualTags.ResumeLayout(false);
            this.grpVirtualTags.PerformLayout();
            this.grpVtagConfig.ResumeLayout(false);
            this.grpVtagConfig.PerformLayout();
            this.grpServerSettings.ResumeLayout(false);
            this.grpServerSettings.PerformLayout();
            this.grpSharing.ResumeLayout(false);
            this.grpSharing.PerformLayout();
            this.groupLibraryDisplay.ResumeLayout(false);
            this.groupLibraryDisplay.PerformLayout();
            this.grpScanning.ResumeLayout(false);
            this.grpScanning.PerformLayout();
            this.groupComicFolders.ResumeLayout(false);
            this.pageScripts.ResumeLayout(false);
            this.grpScriptSettings.ResumeLayout(false);
            this.grpScriptSettings.PerformLayout();
            this.grpScripts.ResumeLayout(false);
            this.grpScripts.PerformLayout();
            this.grpPackages.ResumeLayout(false);
            this.ResumeLayout(false);

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
		private static int activeTab = -1;
		private readonly List<CheckBox> tabButtons = new List<CheckBox>();
        private CollapsibleGroupBox grpVirtualTags;
        private Label lblVirtualTags;
        private ComboBox cbVirtualTags;
        private GroupBox grpVtagConfig;
        private Label lblCaptionText;
        private Label lblCaptionSuffix;
        private RichTextBox rtfVirtualTagCaption;
        private Label lblFieldConfig;
        private TextBox txtCaptionPrefix;
        private Label lblCaptionPrefix;
        private Button btnCaptionInsert;
        private TextBox txtCaptionSuffix;
        private CheckBox chkVirtualTagEnable;
        private Label lblVirtualTagDescription;
        private Label lblVirtualTagName;
        private TextBox txtVirtualTagDescription;
        private TextBox txtVirtualTagName;
        private Button btInsertValue;
        private TextBox txtCaptionFormat;
        private Label lblCaptionFormat;
    }
}
