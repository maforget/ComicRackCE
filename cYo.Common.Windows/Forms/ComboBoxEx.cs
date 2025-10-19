using cYo.Common.Windows.Forms.Theme;
using cYo.Common.Windows.Forms.Theme.Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static cYo.Common.Win32.ExecuteProcess;

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

			public const int WM_CTLCOLORSTATIC = 0x0138; //312U;

			public const int WM_PAINT = 15;

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);

			[DllImport("user32.dll")]
			public static extern bool GetComboBoxInfo(IntPtr hwnd, ref COMBOBOXINFO pcbi);

			[DllImport("gdi32.dll")]
			public static extern int SetTextColor(IntPtr hdc, int crColor);

			[DllImport("gdi32.dll")]
			public static extern int SetBkColor(IntPtr hdc, int crColor);

			[StructLayout(LayoutKind.Sequential)]
			public struct PAINTSTRUCT
			{
				public IntPtr hdc;
				public bool fErase;
				public Rectangle rcPaint;
				public bool fRestore;
				public bool fIncUpdate;
				[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
				public byte[] rgbReserved;
			}

            [DllImport("gdi32.dll")]
            private static extern IntPtr CreateSolidBrush(int crColor);

			// cache it
            public static readonly IntPtr darkEditBrush = CreateSolidBrush(ColorTranslator.ToWin32(Color.FromArgb(64, 64, 64)));

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

		private NativeMethods.COMBOBOXINFO GetChildHandle()
		{
			NativeMethods.COMBOBOXINFO pcbi = default(NativeMethods.COMBOBOXINFO);
			pcbi.cbSize = Marshal.SizeOf((object)pcbi);
			NativeMethods.GetComboBoxInfo(base.Handle, ref pcbi);
			return pcbi;
		}

		private IntPtr GetTextHandle()
		{
			NativeMethods.COMBOBOXINFO pcbi = GetChildHandle();
			return pcbi.hwndEdit;
		}

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            IntPtr hdc = m.WParam;   // handle to device context (DC)
            IntPtr hwndChild = m.LParam;  // handle to the static control
            NativeMethods.COMBOBOXINFO pcbi = GetChildHandle();


			if (!ThemeManager.IsDarkModeEnabled || pcbi.hwndEdit == IntPtr.Zero)
				return;

			switch (m.Msg)
			{
				case NativeMethods.WM_CTLCOLORSTATIC:
					if (hwndChild == pcbi.hwndEdit)
					{
						NativeMethods.SetBkColor(hdc, ColorTranslator.ToWin32(ThemeColors.DarkMode.ComboBox.Disabled));
						NativeMethods.SetTextColor(hdc, ColorTranslator.ToWin32(SystemColors.GrayText));

						m.Result = NativeMethods.darkEditBrush;
						return;
					}

					// Additional handling for Simple style listbox when disabled
					if (DropDownStyle == ComboBoxStyle.Simple && !Enabled && hwndChild == pcbi.hwndList)
					{
						NativeMethods.SetBkColor(hdc, ColorTranslator.ToWin32(ThemeColors.DarkMode.ComboBox.Disabled));
						NativeMethods.SetTextColor(hdc, ColorTranslator.ToWin32(SystemColors.GrayText));
						m.Result = NativeMethods.darkEditBrush;
						return;
					}

					break;
				case NativeMethods.WM_PAINT:
					if (!GetStyle(ControlStyles.UserPaint) && (FlatStyle == FlatStyle.Flat || FlatStyle == FlatStyle.Popup) && !(SystemInformation.HighContrast && BackColor == SystemColors.Window))
					{
                        using Graphics g = Graphics.FromHdc(hdc);

                        // Special handling for disabled DropDownList in dark mode
                        if (!Enabled && DropDownStyle == ComboBoxStyle.DropDownList)
						{
							// The text area for DropDownList (excluding the dropdown button)
							Rectangle textBounds = ClientRectangle;
							textBounds.Width -= SystemInformation.VerticalScrollBarWidth;

							// Fill the background
							using var bgBrush = new SolidBrush(ThemeColors.DarkMode.ComboBox.Disabled);
							g.FillRectangle(bgBrush, textBounds);

							// Draw the text
							TextRenderer.DrawText(
								g,
								Text,
								Font,
								textBounds,
                                SystemColors.GrayText,
								TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
						}

						return;
					}
					break;
			}

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
