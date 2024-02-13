using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class ComboBoxEx : ComboBox, IPromptText
	{
		private static class NativeMethods
		{
			public struct RECT
			{
				public int left;

				public int top;

				public int right;

				public int bottom;
			}

			public enum ComboBoxButtonState
			{
				STATE_SYSTEM_NONE = 0,
				STATE_SYSTEM_INVISIBLE = 0x8000,
				STATE_SYSTEM_PRESSED = 8
			}

			public struct COMBOBOXINFO
			{
				public int cbSize;

				public RECT rcItem;

				public RECT rcButton;

				public ComboBoxButtonState buttonState;

				public IntPtr hwndCombo;

				public IntPtr hwndEdit;

				public IntPtr hwndList;
			}

			public const int ECM_FIRST = 5376;

			public const int EM_SETCUEBANNER = 5377;

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

			[DllImport("user32.dll")]
			public static extern bool GetComboBoxInfo(IntPtr hwnd, ref COMBOBOXINFO pcbi);
		}

		private bool quickSelectAll;

		private string promptText;

		private bool focusSelect = true;

		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Category("Appearance")]
		[Description("The prompt text to display when there is nothing in the Text property.")]
		public string PromptText
		{
			get
			{
				return promptText;
			}
			set
			{
				promptText = value;
				if (base.IsHandleCreated)
				{
					SetPromptText();
				}
			}
		}

		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Category("Behavior")]
		[Description("Automatically select the text when control receives the focus.")]
		[DefaultValue(true)]
		public bool FocusSelect
		{
			get
			{
				return focusSelect;
			}
			set
			{
				focusSelect = value;
			}
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			SetPromptText();
		}

		private void SetPromptText()
		{
			IntPtr textHandle = GetTextHandle();
			if (textHandle != IntPtr.Zero)
			{
				NativeMethods.SendMessage(textHandle, NativeMethods.EM_SETCUEBANNER, IntPtr.Zero, promptText);
			}
		}

		private IntPtr GetTextHandle()
		{
			NativeMethods.COMBOBOXINFO pcbi = default(NativeMethods.COMBOBOXINFO);
			pcbi.cbSize = Marshal.SizeOf((object)pcbi);
			NativeMethods.GetComboBoxInfo(base.Handle, ref pcbi);
			return pcbi.hwndEdit;
		}

		protected override void OnEnter(EventArgs e)
		{
			if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(PromptText))
			{
				Text = PromptText;
			}
			if (Text.Length > 0 && focusSelect)
			{
				SelectAll();
				quickSelectAll = true;
			}
			base.OnEnter(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			if (!string.IsNullOrEmpty(PromptText) && base.SelectedText == PromptText)
			{
				Text = string.Empty;
			}
			quickSelectAll = false;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (quickSelectAll)
			{
				SelectAll();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			quickSelectAll = false;
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			quickSelectAll = false;
		}
	}
}
