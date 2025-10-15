using cYo.Common.Drawing;
using cYo.Common.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace cYo.Common.Windows.Forms;

/// <summary>
/// Class to centralisation application Theming. When theming is enabled, calls <see cref="KnownColorTableEx"/> on initialization to override built-in <see cref="System.Drawing.SystemColors"/> with theme-defined colors.<br/>
/// Exposes <see cref="Theme(Control)"/> for recursive <see cref="Control"/> theming by setting class fields and leveraging <see cref="UXTheme"/> for native Windows OS theming.
/// </summary>
public static partial class ThemeExtensions
{
    /// <summary>
		/// <see cref="Control"/> to Theme Handler method mappings. For controls which don't respond to <see cref="System.Drawing.SystemColors"/> change, or
		/// require additional tweaking such as setting <see cref="BorderStyle"/> or <see cref="FlatStyle"/>.
		/// </summary>
		/// <remarks>
		/// <see cref="BorderStyle"/> or <see cref="FlatStyle"/> can affect whether <see cref="UXTheme.SetControlTheme(IntPtr, string, string)"/> is able to theme a <see cref="Control"/> or not.<br/>
		/// </remarks>
		private static readonly Dictionary<Type, Action<Control>> themeHandlers = new()
    {
        { typeof(Button), c => ThemeButton((Button)c) },
        { typeof(CheckBox), c => ThemeCheckBox((CheckBox)c) },
        { typeof(ComboBox), c => ThemeComboBox((ComboBox)c) },
        { typeof(DataGridView), c => ThemeDataGridView((DataGridView)c) },
        { typeof(GroupBox), c => ThemeGroupBox((GroupBox)c) },
        { typeof(Label), c => ThemeLabel((Label)c) },
        { typeof(ListBox), c => ThemeListBox((ListBox)c) },
        { typeof(ListView), c => ThemeListView((ListView)c) },
        { typeof(Panel), c => ThemePanel((Panel)c) },
        { typeof(ProgressBar), c => ThemeProgressBar((ProgressBar)c) },
        { typeof(RichTextBox), c => ThemeRichTextBox((RichTextBox)c) },
        { typeof(StatusStrip), c => ThemeStatusStrip((StatusStrip)c) },
        { typeof(TabPage), c => ThemeTabPage((TabPage)c) },
        { typeof(TextBox), c => ThemeTextBox((TextBox)c) },
        { typeof(TreeView), c => ThemeTreeView((TreeView)c) }
    };

    /// <summary>
    /// <see cref="Control"/> to <see cref="UXTheme"/>Handler method mappings. For controls that have a theme class other than <c>"DarkMode_Explorer"</c>.<br/>
    /// Also for controls which a Win32 subclass, requiring that the parent <see cref="Control"/> and subclass are both themed in separate calls.
    /// </summary>
    private static readonly Dictionary<Type, Action<Control>> complexUXThemeHandlers = new()
    {
        { typeof(CheckBox), c => SetCheckBoxUXTheme((CheckBox)c) },
        { typeof(ComboBox), c => SetComboBoxUXTheme((ComboBox)c) },
        { typeof(ListView), c => SetListViewUXTheme((ListView)c) },
        { typeof(TabControl), c => SetTabControlUXTheme((TabControl)c) }
    };
}
