using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Sync;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class DeviceSelectDialog : FormEx
	{
		private bool repairMode;

		private string currentKey;

		private HashSet<string> usedKeys = new HashSet<string>();

		public DeviceSelectDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			lvDevices.Columns.ScaleDpi();
			LocalizeUtility.Localize(this, components);
		}

		private void lvDevices_DoubleClick(object sender, EventArgs e)
		{
			if (!btOK.Enabled)
			{
				return;
			}
			base.DialogResult = DialogResult.OK;
			Hide();
		}

		private void lvDevices_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (repairMode && lvDevices.SelectedItems.Count > 0)
			{
				txSyncId.Text = ((ISyncProvider)lvDevices.SelectedItems[0].Tag).Device.Key;
			}
		}

		private void txSyncId_TextChanged(object sender, EventArgs e)
		{
			if (repairMode)
			{
				UpdateRepairOkState();
			}
		}

		private void UpdateRepairOkState()
		{
			string key = txSyncId.Text.Trim();
			btOK.Enabled = key.Length > 0 && key != currentKey && !usedKeys.Contains(key);
		}

		// The Sync ID column and the manual entry are only used to re-pair a device.
		private void ConfigureForPairing()
		{
			lvDevices.Columns.Remove(colSyncId);
			int height = pnlSyncId.Height;
			pnlSyncId.Visible = false;
			ClientSize = new Size(ClientSize.Width, ClientSize.Height - height);
			lvDevices.Height += height;
		}

		private void ConfigureForRepair(string deviceName, string currentKey, IEnumerable<string> usedKeys)
		{
			repairMode = true;
			this.currentKey = currentKey;
			this.usedKeys = new HashSet<string>(usedKeys);
			Text = TR.Load(base.Name)["RepairDevice", "Re-pair Device"];
			lblSyncId.Text = string.Format(lblSyncId.Text, deviceName, currentKey);
			// make room for the extra column, and for the whole serial (it is a GUID like the Sync ID)
			colSerial.Width = colSyncId.Width;
			int columnsWidth = lvDevices.Columns.Cast<ColumnHeader>().Sum((ColumnHeader c) => c.Width);
			int missing = columnsWidth + SystemInformation.VerticalScrollBarWidth + 4 - lvDevices.ClientSize.Width;
			if (missing > 0)
			{
				ClientSize = new Size(ClientSize.Width + missing, ClientSize.Height);
			}
			UpdateRepairOkState();
		}

		private void FillList(IEnumerable<ISyncProvider> syncProviders)
		{
			foreach (ISyncProvider syncProvider in syncProviders)
			{
				ListViewItem listViewItem = new ListViewItem(syncProvider.Device.Name)
				{
					Tag = syncProvider
				};
				listViewItem.SubItems.Add(syncProvider.Device.Model);
				listViewItem.SubItems.Add(syncProvider.Device.SerialNumber);
				listViewItem.SubItems.Add(syncProvider.Device.Key);
				lvDevices.Items.Add(listViewItem);
			}
			if (lvDevices.Items.Count > 0)
			{
				lvDevices.Items[0].Selected = true;
			}
			ListView listView = lvDevices;
			bool enabled = (btOK.Enabled = lvDevices.Items.Count > 0);
			listView.Enabled = enabled;
			ApplyTheme();
		}

		private static List<ISyncProvider> DiscoverProviders(IWin32Window parent)
		{
			List<ISyncProvider> syncProviders = new List<ISyncProvider>();
			AutomaticProgressDialog.Process(parent, TR.Messages["DiscoveringDevicesCaption", "Discovering connected devices"], TR.Messages["DiscoveringDevicesDescription", "Searching all connected Devices for installed ComicRack clients"], 1000, delegate
			{
				syncProviders.AddRange(DeviceSyncFactory.Discover());
			}, AutomaticProgressDialogOptions.None);
			return syncProviders;
		}

		/// <summary>
		/// Lets the user pick the new Sync ID of an already paired device (e.g. after the device app
		/// was reinstalled and generated a new ID), either from the devices currently found or by
		/// typing it in. Returns null if the dialog was cancelled.
		/// </summary>
		/// <param name="devices">List of <seealso cref="DeviceSyncSettings"/></param>
		/// <param name="currentDevice">The device to repair</param>
		public static (string Name, string DeviceKey) SelectSyncId(IWin32Window parent, IEnumerable<DeviceSyncSettings> devices, DeviceSyncSettings currentDevice)
		{
			using (DeviceSelectDialog deviceSelectDialog = new DeviceSelectDialog())
			{
				string currentKey = currentDevice.DeviceKey;
				string deviceName = currentDevice.DeviceName;
				var otherKeys = devices.Where(d => d.DeviceKey != currentDevice.DeviceKey).Select(d => d.DeviceKey);

				deviceSelectDialog.ConfigureForRepair(deviceName, currentKey, otherKeys);
				// devices already paired (including this one) are not valid targets, the one with the same name goes first
				deviceSelectDialog.FillList(DiscoverProviders(parent)
					.Where((ISyncProvider sd) => sd.Device.Key != currentKey && !deviceSelectDialog.usedKeys.Contains(sd.Device.Key))
					.OrderByDescending((ISyncProvider sd) => string.Equals(sd.Device.Name, deviceName, StringComparison.OrdinalIgnoreCase)));
				deviceSelectDialog.UpdateRepairOkState();

				if (deviceSelectDialog.ShowDialog(parent) == DialogResult.OK)
				{
					return deviceSelectDialog.GetRepairResult(currentDevice);
				}
				return (null, null);
			}
		}

		// The Sync ID can be typed without selecting a device (or after selecting a different one), so
		// the list selection can be empty. The name only follows the selected device when the ID is its own.
		private (string Name, string DeviceKey) GetRepairResult(DeviceSyncSettings currentDevice)
		{
			string newKey = txSyncId.Text.Trim();
			ISyncProvider selectedProvider = lvDevices.SelectedItems.Count > 0 ? lvDevices.SelectedItems[0].Tag as ISyncProvider : null;
			string newName = (selectedProvider != null && selectedProvider.Device.Key == newKey) ? selectedProvider.Device.Name : currentDevice.DeviceName;
			return (newName, newKey);
		}

		public static ISyncProvider SelectProvider(IWin32Window parent, IEnumerable<DeviceSyncSettings> devices)
		{
			using (DeviceSelectDialog deviceSelectDialog = new DeviceSelectDialog())
			{
				deviceSelectDialog.ConfigureForPairing();
				List<ISyncProvider> syncProviders = DiscoverProviders(parent);
				deviceSelectDialog.FillList(syncProviders.Where((ISyncProvider sd) => devices.All((DeviceSyncSettings d) => d.DeviceKey != sd.Device.Key)));
				if (deviceSelectDialog.ShowDialog(parent) == DialogResult.Cancel)
				{
					return null;
				}
				return (deviceSelectDialog.lvDevices.SelectedItems.Count == 0) ? null : (deviceSelectDialog.lvDevices.SelectedItems[0].Tag as ISyncProvider);
			}
		}

	}
}
