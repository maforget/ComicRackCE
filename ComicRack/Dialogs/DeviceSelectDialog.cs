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

		public DeviceSelectDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			lvDevices.Columns.ScaleDpi();
			LocalizeUtility.Localize(this, components);
		}

		private void lvDevices_DoubleClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			Hide();
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
				lvDevices.Items.Add(listViewItem);
			}
			if (lvDevices.Items.Count > 0)
			{
				lvDevices.Items[0].Selected = true;
			}
			ListView listView = lvDevices;
			bool enabled = (btOK.Enabled = lvDevices.Items.Count > 0);
			listView.Enabled = enabled;
		}

		public static ISyncProvider SelectProvider(IWin32Window parent, IEnumerable<DeviceSyncSettings> devices)
		{
			using (DeviceSelectDialog deviceSelectDialog = new DeviceSelectDialog())
			{
				List<ISyncProvider> syncProviders = new List<ISyncProvider>();
				AutomaticProgressDialog.Process(parent, TR.Messages["DiscoveringDevicesCaption", "Discovering connected devices"], TR.Messages["DiscoveringDevicesDescription", "Searching all connected Devices for installed ComicRack clients"], 1000, delegate
				{
					syncProviders.AddRange(DeviceSyncFactory.Discover());
				}, AutomaticProgressDialogOptions.None);
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
