using System;
using System.ComponentModel;
using System.Drawing;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	[ToolboxBitmap(typeof(ComboBox))]
	[ToolboxItem(true)]
	[ToolboxItemFilter("System.Windows.Forms")]
	[Description("Displays an editable text box with a drop-down list of permitted values.")]
	public class PopupComboBox : PopupComboBoxBase
	{
		[SuppressUnmanagedCodeSecurity]
		private static class NativeMethods
		{
			public const int WM_USER = 1024;

			public const int WM_COMMAND = 273;

			public const int WM_REFLECT = 8192;

			public const int CBN_DROPDOWN = 7;

			public static int HIWORD(int n)
			{
				return (n >> 16) & 0xFFFF;
			}

			public static int HIWORD(IntPtr n)
			{
				return HIWORD(n.ToInt32());
			}
		}

		private Popup dropDown;

		private Control dropDownControl;

		private DateTime dropDownHideTime;

		public Control DropDownControl
		{
			get
			{
				return dropDownControl;
			}
			set
			{
				if (dropDownControl != value)
				{
					dropDownControl = value;
					if (dropDown != null)
					{
						dropDown.Closed -= dropDown_Closed;
						dropDown.Dispose();
					}
					dropDown = new Popup(value);
					dropDown.Closed += dropDown_Closed;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int DropDownWidth
		{
			get
			{
				return base.DropDownWidth;
			}
			set
			{
				base.DropDownWidth = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int DropDownHeight
		{
			get
			{
				return base.DropDownHeight;
			}
			set
			{
				base.DropDownHeight = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool IntegralHeight
		{
			get
			{
				return base.IntegralHeight;
			}
			set
			{
				base.IntegralHeight = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ObjectCollection Items => base.Items;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int ItemHeight
		{
			get
			{
				return base.ItemHeight;
			}
			set
			{
				base.ItemHeight = value;
			}
		}

		public PopupComboBox()
		{
			dropDownHideTime = DateTime.Now;
			base.DropDownHeight = (base.DropDownWidth = 1);
			base.IntegralHeight = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && dropDown != null)
			{
				dropDown.Dispose();
			}
			base.Dispose(disposing);
		}

		public void ShowDropDown()
		{
			if (dropDown != null)
			{
				if ((DateTime.Now - dropDownHideTime).TotalSeconds > 0.5)
				{
					dropDown.Show(this);
					return;
				}
				dropDownHideTime = DateTime.Now.Subtract(new TimeSpan(0, 0, 1));
				Focus();
			}
		}

		public void HideDropDown()
		{
			if (dropDown != null)
			{
				dropDown.Hide();
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 8465 && NativeMethods.HIWORD(m.WParam) == NativeMethods.CBN_DROPDOWN)
			{
				ShowDropDown();
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		private void dropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			dropDownHideTime = DateTime.Now;
		}
	}
}
