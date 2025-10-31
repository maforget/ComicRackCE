using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace cYo.Common.Win32;

/// <summary>
/// Wrapper around <c>uxtheme.dll</c> which is used for native Windows OS theming.
/// Required as Windows will always run older .NET applications in Light mode, regardless of app preference.
/// </summary>
/// <remarks>
/// UXTheme allows informing Windows of a Dark Mode theme preference, and applying that theme to a <c><see cref="System.Windows.Forms.Form"/></c>.<br/>
/// Implementation is based on Win10 LTSC build 19044: higher Windows versions will have better theming support that
/// could be leveraged, and older Windows versions may have missing Dark Mode theme definitions.
/// </remarks>
public static class UXTheme
{

    /// <summary>
		/// Win32 API native things
		/// </summary>
    private static class Native
    {

        public static readonly int DWMWA_USE_IMMERSIVE_DARK_MODE = GetDwmDarkModeAttribute();

        #region Constants
        public const uint LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800;

        public const int WM_THEMECHANGED = 0x031A;

        public static int YES = 1;

        // Win32 API ListView constants
        public const int LVM_FIRST = 0x1000;
        public const int LVM_GETHEADER = LVM_FIRST + 31;
        #endregion

        #region Enum/Struct
        public enum PreferredAppMode
        {
            Default,
            AllowDark,
            ForceDark,
            ForceLight,
            Max
        }

        public struct RECT
        {
            public int left;

            public int top;

            public int right;

            public int bottom;

            // Add if required. Disabled due to not currently used + System.Drawing dependency.
            //public RECT(Rectangle r)
            //{
            //    left = r.Left; top = r.Top; right = r.Right; bottom = r.Bottom;
            //}

            public RECT(int l, int t, int r_, int b)
            {
                left = l; top = t; right = r_; bottom = b;
            }
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
        #endregion

        #region Functions
        [DllImport("user32.dll")]
        public static extern bool GetComboBoxInfo(IntPtr hwnd, ref COMBOBOXINFO pcbi);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

        [DllImport("uxtheme.dll", EntryPoint = "#133", SetLastError = true)]
        internal static extern bool AllowDarkModeForWindow(IntPtr hWnd, bool allow);

        [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true)]
        internal static extern bool AllowDarkModeForApp(bool allow);

        [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true)]
        internal static extern PreferredAppMode SetPreferredAppMode(PreferredAppMode mode);

        [DllImport("uxtheme.dll", EntryPoint = "#136", ExactSpelling = true)]
        internal static extern void FlushMenuThemes();

        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        private const uint GW_CHILD = 5;
        private const uint GW_HWNDNEXT = 2;

        internal static IEnumerable<IntPtr> EnumChildWindows(IntPtr parent)
        {
            IntPtr child = GetWindow(parent, GW_CHILD);
            while (child != IntPtr.Zero)
            {
                yield return child;
                child = GetWindow(child, GW_HWNDNEXT);
            }
        }

        /// <summary>Causes a window to use a different set of visual style information than its class normally uses. (<a href="https://source.dot.net/#System.Windows.Forms.Primitives/Windows.Win32.PInvoke.UXTHEME.dll.g.cs,1258">documentation source</a>)</summary>
        /// <param name="hwnd">
        /// <para>Type: <b><a href="https://docs.microsoft.com/windows/desktop/WinProg/windows-data-types">HWND</a></b> Handle to the window whose visual style information is to be changed.</para>
        /// </param>
        /// <param name="pszSubAppName">
        /// <para>Type: <b><a href="https://docs.microsoft.com/windows/desktop/WinProg/windows-data-types">LPCWSTR</a></b> Pointer to a string that contains the application name to use in place of the calling application's name. If this parameter is <b>NULL</b>, the calling application's name is used.</para>
        /// </param>
        /// <param name="pszSubIdList">
        /// <para>Type: <b><a href="https://docs.microsoft.com/windows/desktop/WinProg/windows-data-types">LPCWSTR</a></b> Pointer to a string that contains a semicolon-separated list of CLSID names to use in place of the actual list passed by the window's class. If this parameter is <b>NULL</b>, the ID list from the calling class is used.</para>
        /// </param>
        /// <returns>
        /// <para>Type: <b><a href="https://docs.microsoft.com/windows/desktop/WinProg/windows-data-types">HRESULT</a></b> If this function succeeds, it returns <b>S_OK</b>. Otherwise, it returns an <b>HRESULT</b> error code.</para>
        /// </returns>
        /// <remarks>
        /// <para>The theme manager retains the <paramref name="pszSubAppName"/> and the <paramref name="pszSubIdList"/> associations through the lifetime of the window, even if visual styles subsequently change. The window is sent a <a href="https://docs.microsoft.com/windows/desktop/winmsg/wm-themechanged">WM_THEMECHANGED</a> message at the end of a <b>SetWindowTheme</b> call, so that the new visual style can be found and applied.</para>
        /// <para>When <paramref name="pszSubAppName"/> and <paramref name="pszSubIdList"/> are <c>NULL</c>, the theme manager removes the previously applied associations. You can prevent visual styles from being applied to a specified window by specifying an empty string, (L" "), which does not match any section entries.</para>
        /// <para><see href="https://learn.microsoft.com/windows/win32/api/uxtheme/nf-uxtheme-setwindowtheme">Read more on docs.microsoft.com</see>.</para>
        /// </remarks>
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

        /// <summary>Sets the value of Desktop Window Manager (DWM) non-client rendering attributes for a window. (<a href="https://source.dot.net/#System.Windows.Forms.Primitives/Windows.Win32.PInvoke.dwmapi.dll.g.cs,47">documentation source</a>)</summary>
        /// <param name="hwnd">The handle to the window for which the attribute value is to be set.</param>
        /// <param name="dwAttribute">A flag describing which value to set, specified as a value of the [DWMWINDOWATTRIBUTE](/windows/desktop/api/dwmapi/ne-dwmapi-dwmwindowattribute) enumeration. This parameter specifies which attribute to set, and the *pvAttribute* parameter points to an object containing the attribute value.</param>
        /// <param name="pvAttribute">A pointer to an object containing the attribute value to set. The type of the value set depends on the value of the *dwAttribute* parameter. The [**DWMWINDOWATTRIBUTE**](/windows/desktop/api/Dwmapi/ne-dwmapi-dwmwindowattribute) enumeration topic indicates, in the row for each flag, what type of value you should pass a pointer to in the *pvAttribute* parameter.</param>
        /// <param name="cbAttribute">The size, in bytes, of the attribute value being set via the *pvAttribute* parameter. The type of the value set, and therefore its size in bytes, depends on the value of the *dwAttribute* parameter.</param>
        /// <returns>
        /// <para>Type: **[HRESULT](/windows/desktop/com/structure-of-com-error-codes)** If the function succeeds, it returns **S_OK**. Otherwise, it returns an [**HRESULT**](/windows/desktop/com/structure-of-com-error-codes) [error code](/windows/desktop/com/com-error-codes-10). If Desktop Composition has been disabled (Windows 7 and earlier), then this function returns **DWM_E_COMPOSITIONDISABLED**.</para>
        /// </returns>
        /// <remarks>It's not valid to call this function with the *dwAttribute* parameter set to **DWMWA_NCRENDERING_ENABLED**. To enable or disable non-client rendering, you should use the **DWMWA_NCRENDERING_POLICY** attribute, and set the desired value. For more info, and a code example, see [Controlling non-client region rendering](/windows/desktop/dwm/composition-ovw#controlling-non-client-region-rendering).</remarks>
        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, ref int pvAttribute, int cbAttribute);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        #endregion

    }

    /// <summary>Does the current Windows OS version <b>support</b> Dark Mode theming? Set on initialization and referenced in all non-initialization calls.</summary>
    public static bool IsDarkModeSupported = false;

    /// <summary>
		/// Checks if current Windows OS version has Dark Mode theming support and set <c><see cref="IsDarkModeSupported"/></c> accordingly.
    /// </summary>
    /// <remarks>
    /// If <c><see cref="IsDarkModeSupported"/></c> is set to false, all calls to others <c><see cref="UXTheme"/></c> functions will immediately return.
    /// </remarks>
    public static void Initialize()
    {
        // loading the library would allow us to use OpenThemeData and get VisualStyle element info
        // for now outside of scope as should be focusing on .NET 9+
        //IntPtr hUxTheme = Native.LoadLibraryEx("uxtheme.dll", IntPtr.Zero, Native.LOAD_LIBRARY_SEARCH_SYSTEM32);
        //if (hUxTheme == IntPtr.Zero) return;
        IsDarkModeSupported = Native.DWMWA_USE_IMMERSIVE_DARK_MODE != 0;

        Native.AllowDarkModeForApp(true);
        //Native.AllowDarkModeForWindow(hUxTheme, true); // we don't have a window handle yet
        Native.SetPreferredAppMode(Native.PreferredAppMode.ForceDark);

        Native.FlushMenuThemes();
    }

    /// <summary>Sets Dark Mode for App Window. (Effectively <c><see cref="System.Windows.Forms.Form"/></c>)</summary>
    /// <param name="hwnd">The handle to the window for which Dark Mode will be set.</param>
    /// <remarks>This could be expanded beyond Light/Dark to other Windows themes</remarks>
    public static void SetWindowTheme(IntPtr hwnd)
    {
        if (!IsDarkModeSupported || hwnd == null || hwnd == IntPtr.Zero) return;

        Native.AllowDarkModeForWindow(hwnd, true);
        Native.SetWindowTheme(hwnd, "DarkMode_Explorer", null);

        Native.DwmSetWindowAttribute(hwnd, Native.DWMWA_USE_IMMERSIVE_DARK_MODE, ref Native.YES, sizeof(int));

        Native.FlushMenuThemes();
    }

    /// <summary>
		/// Sets Dark Mode for a single <c><see cref="System.Windows.Forms.Control"/></c>.<br/>
    /// Uses an undocumented API. Support varies between Windows OS versions.
		/// </summary>
		/// <param name="hwnd">The handle to the <c><see cref="System.Windows.Forms.Control"/></c> for which Dark Mode will be set.</param>
    /// <param name="themeClass">Maps to <paramref name="pszSubAppName"/> parameter of <c><see cref="Native.SetWindowTheme(IntPtr, string, string)"/></c></param>
    /// <param name="subClass">Maps to <paramref name="pszSubIdList"/> parameter of <c><see cref="Native.SetWindowTheme(IntPtr, string, string)"/></c></param>
    /// <remarks>
    /// We are essentially asking the Theme Engine to theme a Control for us, hence the high dependence on Windows OS version.<br/>
    /// Think how early Windows Dark Mode still had Light File/Folder properties, Notepad and Task Manager.
		/// </remarks>
    public static void SetControlTheme(IntPtr hwnd, string themeClass = "DarkMode_Explorer", string subClass = null)
    {
        if (!IsDarkModeSupported || hwnd == null || hwnd == IntPtr.Zero) return;

        Native.SetWindowTheme(hwnd, themeClass, subClass);
        Native.AllowDarkModeForWindow(hwnd, true);
    }

    /// <summary>
    /// Sets Dark Mode for a <c><see cref="System.Windows.Forms.ComboBox"/></c>. Requires special treatment as it is made up of different Win32 Classes.
    /// </summary>
    /// <param name="hwnd">The handle to the <c><see cref="System.Windows.Forms.ComboBox"/></c> for which Dark Mode will be set.</param>
    /// <remarks>
    /// We need to theme the parent to get a Dark Mode box, and the list to get a Dark Mode scrollbar.<br/>
    /// The <c>Edit</c> is left alone as it gets themed badly (White on White). The <c>DropDownList</c> could be themed but it's a lot of work for little gain.<br/>
    /// (Instead of being a Child Class it exists on its own, which makes finding the handle for it challenging)
    /// </remarks>
    public static void SetComboBoxTheme(IntPtr hwnd)
    {
        if (!IsDarkModeSupported || hwnd == null || hwnd == IntPtr.Zero) return;

        Native.SetWindowTheme(hwnd, null, "DarkMode_CFD::Combobox");
        Native.COMBOBOXINFO pcbi = default(Native.COMBOBOXINFO);
        pcbi.cbSize = Marshal.SizeOf((object)pcbi);
        Native.GetComboBoxInfo(hwnd, ref pcbi);

        //IntPtr comboHandle = pcbi.hwndCombo;
        //if (comboHandle != IntPtr.Zero)
        //{
            //Native.SetWindowTheme(comboHandle, "DarkMode_CFD", null);
            //Native.SetWindowTheme(comboHandle, null, "DarkMode_CFD::Combobox");
            //Native.DwmSetWindowAttribute(comboHandle, Native.DWMWA_USE_IMMERSIVE_DARK_MODE, ref Native.YES, sizeof(int));
        //}

        IntPtr listHandle = pcbi.hwndList;
        if (listHandle != IntPtr.Zero)
        {
            Native.SetWindowTheme(listHandle, "DarkMode_Explorer", null);
            //Native.DwmSetWindowAttribute(listHandle, Native.DWMWA_USE_IMMERSIVE_DARK_MODE, ref Native.YES, sizeof(int));
        }

        //IntPtr textHandle = pcbi.hwndEdit;
        //if (textHandle != IntPtr.Zero)
        //{
            //Native.SetWindowTheme(textHandle, "DarkMode_CEdit", null);
            //Native.SetWindowTheme(textHandle, null, "DarkMode_CFD::Edit");
            //Native.DwmSetWindowAttribute(textHandle, Native.DWMWA_USE_IMMERSIVE_DARK_MODE, ref Native.YES, sizeof(int));
        //}
    }

    /// <summary>
    /// Sets Dark Mode for a <c><see cref="System.Windows.Forms.ListView"/> Header</c>. Requires special treatment as it is made up of different Win32 Classes.
    /// </summary>
    /// <param name="hwnd">The handle to the <c><see cref="System.Windows.Forms.ListView"/></c> for which Dark Mode will be set.</param>
    /// <remarks>
    /// The <c><see cref="System.Windows.Forms.ListView"/></c><br/> itself is themed separately depending on its configuration. <br/>
    /// This is only way to get a dark empty-column header, but column header text is also set to dark and must be handled manually.
    /// </remarks>
    public static void SetListViewTheme(IntPtr hwnd)
    {
        if (!IsDarkModeSupported || hwnd == null || hwnd == IntPtr.Zero) return;

        SetControlTheme((hwnd));

        // header has to be themed separately
        IntPtr columnHeaderHandle = Native.SendMessage(hwnd, Native.LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
        if (columnHeaderHandle != IntPtr.Zero)
        {
            Native.SetWindowTheme(columnHeaderHandle, "DarkMode_ItemsView", null); //working but dark header text
            //NativeMethods.SetWindowTheme(columnHeaderHandle, null, "DarkMode_ItemsView::Header");
        }
    }

    public static void SetTabControlTheme(IntPtr hwnd)
    {
        if (!IsDarkModeSupported || hwnd == null || hwnd == IntPtr.Zero) return;

        Native.SetWindowTheme(hwnd, null, "DarkMode::FileExplorerBannerContainer");
        //foreach (var childHandle in Native.EnumChildWindows(hwnd))
        //{
        //    Native.SetWindowTheme(childHandle, null, "DarkMode::FileExplorerBannerContainer");
        //}
    }

    /// <summary>
		/// Gets the Desktop Window Manager (DWM) "Dark Mode" value. Returns 0 if the Windows OS version (Theme Engine) is too old to support Dark Mode.
    /// </summary>
    private static int GetDwmDarkModeAttribute()
    {
        // DWMWA_USE_IMMERSIVE_DARK_MODE is 20 in recent builds
        var build = Environment.OSVersion.Version.Build;
        return (build < 17763) ? 0 : (build >= 18985) ? 20 : 19;
    }
}