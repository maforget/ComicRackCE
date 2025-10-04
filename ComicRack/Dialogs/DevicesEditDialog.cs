using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Sync;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class DevicesEditDialog : FormEx
	{
		private static int selectedTab;

		public IList<DeviceSyncSettings> Devices
		{
			get
			{
				return (from TabPage tp in tabDevices.TabPages
					select ((DeviceEditControl)tp.Tag).Settings).ToList();
			}
			set
			{
				tabDevices.TabPages.Clear();
				foreach (DeviceSyncSettings item in value)
				{
					AddTab(item);
				}
			}
		}

		public DeviceEditControl CurrentDevice
		{
			get
			{
				TabPage tabPage = tabDevices.SelectedTab;
				if (tabPage != null)
				{
					return tabPage.Tag as DeviceEditControl;
				}
				return null;
			}
		}

		public DevicesEditDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			btDevice.Image = ((Bitmap)btDevice.Image).ScaleDpi();
			this.RestorePosition();
			LocalizeUtility.Localize(this, components);
			SetVisibility();
		}

		private void btPair_Click(object sender, EventArgs e)
		{
			ISyncProvider sd = DeviceSelectDialog.SelectProvider(this, Devices);
			if (sd != null && Devices.All((DeviceSyncSettings d) => d.DeviceKey != sd.Device.Key))
			{
				tabDevices.SelectedTab = AddTab(new DeviceSyncSettings
				{
					DeviceName = sd.Device.Name,
					DeviceKey = sd.Device.Key
				});
			}
		}

		private void cmDevice_Opening(object sender, CancelEventArgs e)
		{
			if (CurrentDevice == null)
			{
				e.Cancel = true;
				return;
			}
			miDevicePaste.Enabled = CurrentDevice != null && CurrentDevice.CanPaste;
			miDeviceCopyToAll.Visible = tabDevices.TabCount > 1;
		}

		private void miDeviceCopy_Click(object sender, EventArgs e)
		{
			if (CurrentDevice != null)
			{
				CurrentDevice.CopyShareSettings();
			}
		}

		private void miDevicePaste_Click(object sender, EventArgs e)
		{
			if (CurrentDevice != null)
			{
				CurrentDevice.PasteSharedSettings();
			}
		}

		private void miDeviceCopyToAll_Click(object sender, EventArgs e)
		{
			if (CurrentDevice == null)
			{
				return;
			}
			foreach (DeviceEditControl item in from t in tabDevices.TabPages.OfType<TabPage>()
				select t.Tag as DeviceEditControl into dec
				where dec != null
				select dec)
			{
				item.PasteSharedSettings();
			}
		}

		private void btDevice_Click(object sender, EventArgs e)
		{
			cmDevice.Show(btDevice, new Point(btDevice.Width, btDevice.Height), ToolStripDropDownDirection.BelowLeft);
		}

		private void miDeviceRename_Click(object sender, EventArgs e)
		{
			if (CurrentDevice != null)
			{
				string name = SelectItemDialog.GetName(this, TR.Messages["RenameDevice", "Rename Device"], CurrentDevice.DeviceName);
				if (!string.IsNullOrEmpty(name))
				{
					CurrentDevice.DeviceName = name;
				}
			}
		}

		private void miDeviceUnpair_Click(object sender, EventArgs e)
		{
			if (tabDevices.SelectedTab != null)
			{
				RemoveTab(tabDevices.SelectedTab);
			}
		}

		private TabPage AddTab(DeviceSyncSettings pd)
		{
			TabPage tb = new TabPage(pd.DeviceName)
			{
				Padding = new Padding(10),
				UseVisualStyleBackColor = true
			};
			DeviceEditControl se = new DeviceEditControl
			{
				Settings = pd,
				Dock = DockStyle.Fill
			};
			se.DeviceNameChanged += delegate
			{
				tb.Text = se.DeviceName;
			};
			tb.Controls.Add(se);
			tb.Tag = se;
			tabDevices.TabPages.Add(tb);
			SetVisibility();
			return tb;
		}

		private void RemoveTab(TabPage page)
		{
			tabDevices.TabPages.Remove(page);
			SetVisibility();
		}

		private void SetVisibility()
		{
			tabDevices.Visible = tabDevices.TabCount > 0;
			labelHint.Visible = tabDevices.TabCount == 0;
			btDevice.Visible = tabDevices.TabCount > 0;
		}

		public static bool Show(IWin32Window parent, IList<DeviceSyncSettings> portableDevices, DeviceSyncSettings device = null, Guid? listId = null)
		{
			using (DevicesEditDialog devicesEditDialog = new DevicesEditDialog())
			{
				devicesEditDialog.Devices = portableDevices;
				if (device != null)
				{
					int num = portableDevices.IndexOf(device);
					if (num >= 0 && listId.HasValue)
					{
						devicesEditDialog.tabDevices.SelectedIndex = num;
						DeviceEditControl deviceEditControl = (DeviceEditControl)devicesEditDialog.tabDevices.TabPages[num].Tag;
						deviceEditControl.SelectList(listId.Value);
					}
				}
				else if (devicesEditDialog.tabDevices.TabCount > 0)
				{
					devicesEditDialog.tabDevices.SelectedIndex = selectedTab.Clamp(0, devicesEditDialog.tabDevices.TabCount - 1);
				}
				DialogResult dialogResult = devicesEditDialog.ShowDialog(parent);
				selectedTab = devicesEditDialog.tabDevices.SelectedIndex;
				if (dialogResult == DialogResult.Cancel)
				{
					return false;
				}
				portableDevices.Clear();
				portableDevices.AddRange(devicesEditDialog.Devices);
				return true;
			}
		}


	}
}
