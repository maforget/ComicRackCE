using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Win32
{
	public static class VariableHeightTreeNode
	{
		private struct TVITEMEX
		{
			public Mask mask;

			public IntPtr item;

			public uint state;

			public uint stateMask;

			public IntPtr pszText;

			public int cchTextMax;

			public int iImage;

			public int iSelectedImage;

			public int iChildren;

			public IntPtr lParam;

			public int iIntegral;
		}

		[Flags]
		private enum Mask : uint
		{
			Text = 0x1u,
			Image = 0x2u,
			Param = 0x4u,
			State = 0x8u,
			Handle = 0x10u,
			SelectedImage = 0x20u,
			Children = 0x40u,
			Integral = 0x80u
		}

		private const int TVM_GETITEM = 4414;

		private const int TVM_SETITEM = 4415;

		[DllImport("user32")]
		private static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wp, ref TVITEMEX item);

		public static int GetHeight(this TreeNode tn)
		{
			TVITEMEX tVITEMEX = default(TVITEMEX);
			tVITEMEX.mask = Mask.Handle | Mask.Integral;
			tVITEMEX.item = tn.Handle;
			tVITEMEX.iIntegral = 1;
			TVITEMEX item = tVITEMEX;
			SendMessage(tn.TreeView.Handle, TVM_GETITEM, IntPtr.Zero, ref item);
			return item.iIntegral;
		}

		public static void SetHeight(this TreeNode tn, int height)
		{
			TVITEMEX tVITEMEX = default(TVITEMEX);
			tVITEMEX.mask = Mask.Handle | Mask.Integral;
			tVITEMEX.item = tn.Handle;
			tVITEMEX.iIntegral = height;
			TVITEMEX item = tVITEMEX;
			SendMessage(tn.TreeView.Handle, TVM_SETITEM, IntPtr.Zero, ref item);
		}
	}
}
