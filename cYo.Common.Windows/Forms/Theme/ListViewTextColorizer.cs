using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;

namespace cYo.Common.Windows.Forms.Theme;

public class ListViewTextColorizer : NativeWindow, IDisposable
{
    private readonly ListView _lv;
    private readonly Color _textColor;
    private readonly Color _headerBackColor;
    private readonly bool _disableTheming;
    private bool _attached = false;

    public ListViewTextColorizer(ListView listView, Color textColor, Color headerBackColor, bool disableTheming = false)
    {
        _lv = listView ?? throw new ArgumentNullException(nameof(listView));
        _textColor = textColor;
        _headerBackColor = headerBackColor;
        _disableTheming = disableTheming;

        // Attach to handle lifecycle
        if (listView.IsHandleCreated)
            Attach();

        listView.HandleCreated += (s, e) => Attach();
        listView.HandleDestroyed += (s, e) => Detach();
    }

    private void Attach()
    {
        if (_attached) return;
        AssignHandle(_lv.Handle);
        _attached = true;

        // Optionally disable theming for the whole ListView (makes GDI draw headers)
        if (_disableTheming)
        {
            SetWindowTheme(_lv.Handle, "", "");
        }

        // Set item/subitem text/background color (affects items & subitems)
        //SendMessage(_lv.Handle, LVM_SETTEXTCOLOR, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(_textColor));
        //SendMessage(_lv.Handle, LVM_SETBKCOLOR, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(backColor));

        _lv.Invalidate();
    }

    private void Detach()
    {
        if (!_attached) return;
        ReleaseHandle();
        _attached = false;
    }

    protected override void WndProc(ref Message m)
    {
        const int WM_PAINT = 0x000F;

        if (m.Msg == WM_PAINT)
        {
            // Let the control paint itself first
            base.WndProc(ref m);

            // Now overlay our group header text (and optionally background)
            try
            {
                DrawGroupHeaderOverlays();
            }
            catch
            {
                // best-effort; swallow exceptions so we don't break the control painting
            }

            return;
        }

        base.WndProc(ref m);
    }

    private void DrawGroupHeaderOverlays()
    {
        if (!_lv.IsHandleCreated) return;
        if (_lv.Groups == null || _lv.Groups.Count == 0) return;

        using (Graphics g = Graphics.FromHwnd(_lv.Handle))
        {
            // Prepare text drawing flags
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;

            foreach (ListViewGroup group in _lv.Groups)
            {
                int nativeGroupId = GetNativeGroupId(group);
                if (nativeGroupId < 0)
                    continue;

                // Get rectangle for the group header (LVGGR_HEADER = 1)
                RECT rc = new RECT { left = LVGGR_HEADER };
                int res = SendMessageGetGroupRect(_lv.Handle, LVM_GETGROUPRECT, (IntPtr)nativeGroupId, ref rc);
                if (res == 0)
                    continue; // failed

                // We actually get the whole group, so rc.Top + HeaderHeight instead of rc.Bottom
                Rectangle bounds = Rectangle.FromLTRB(rc.left, rc.top, rc.right, rc.top + FormUtility.ScaleDpiY(20));

                // Fill background
                using (var bg = new SolidBrush(_headerBackColor))
                    g.FillRectangle(bg, bounds);

                // Draw header text
                string headerText = GetGroupHeaderText(nativeGroupId);
                if (string.IsNullOrEmpty(headerText))
                    headerText = group.Header ?? string.Empty;

                bounds.Inflate(-10,0);
                TextRenderer.DrawText(g, headerText, _lv.Font, bounds, _textColor, flags);

                Size headerTextSize = TextRenderer.MeasureText(headerText, _lv.Font);

                using (Pen pen = new Pen(ThemeColors.ItemView.GroupSeparator, 1))
                    g.DrawLine(pen, rc.left + headerTextSize.Width + 10, rc.top + FormUtility.ScaleDpiY(10), rc.right - FormUtility.ScaleDpiY(12), rc.top + FormUtility.ScaleDpiY(10));
            }
        }
    }

    private string GetGroupHeaderText(int nativeGroupId)
    {
        // Build LVGROUP with buffer for header (unicode)
        LVGROUP group = new LVGROUP();
        group.cbSize = (uint)Marshal.SizeOf(typeof(LVGROUP));
        group.mask = LVGF_HEADER;
        // allocate buffer (260 chars)
        group.pszHeader = Marshal.AllocHGlobal(260 * 2);
        group.cchHeader = 260;

        try
        {
            IntPtr pGroup = Marshal.AllocHGlobal(Marshal.SizeOf(group));
            try
            {
                Marshal.StructureToPtr(group, pGroup, false);
                IntPtr res = SendMessageGetGroupInfo(_lv.Handle, LVM_GETGROUPINFO, (IntPtr)nativeGroupId, pGroup);
                if (res == IntPtr.Zero)
                    return null;

                LVGROUP resultGroup = (LVGROUP)Marshal.PtrToStructure(pGroup, typeof(LVGROUP));
                string header = Marshal.PtrToStringUni(resultGroup.pszHeader);
                return header ?? string.Empty;
            }
            finally
            {
                Marshal.FreeHGlobal(pGroup);
            }
        }
        finally
        {
            if (group.pszHeader != IntPtr.Zero)
                Marshal.FreeHGlobal(group.pszHeader);
        }
    }

    /// <summary>
    /// The managed ListViewGroup contains an internal native ID field. We try common internal field names via reflection.
    /// If that fails, we fall back on the group's index (best-effort).
    /// </summary>
    private int GetNativeGroupId(ListViewGroup group)
    {
        if (group == null) return -1;

        // Try common internal field names that WinForms uses across versions:
        string[] possibleFieldNames = new[] { "ID", "id", "nativeId", "_id" };

        foreach (var name in possibleFieldNames)
        {
            FieldInfo fi = typeof(ListViewGroup).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (fi != null)
            {
                object val = fi.GetValue(group);
                if (val is int intVal)
                    return intVal;
                if (val is short s)
                    return s;
                if (val is long l)
                    return (int)l;
            }
        }

        // Fallback: try to infer from the group's position. This might fail if native IDs are different.
        try
        {
            return _lv.Groups.IndexOf(group);
        }
        catch
        {
            return -1;
        }
    }

    public void Dispose()
    {
        Detach();
    }

    #region Native interop & constants

    private const int LVM_FIRST = 0x1000;
    private const int LVM_GETGROUPRECT = LVM_FIRST + 98;
    private const int LVM_GETGROUPINFO = LVM_FIRST + 149;
    private const int LVM_SETTEXTCOLOR = LVM_FIRST + 36;
    private const int LVGGR_HEADER = 1;

    private const uint LVGF_HEADER = 0x00000001;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct LVGROUP
    {
        public uint cbSize;
        public uint mask;
        public IntPtr pszHeader; // LPWSTR
        public int cchHeader;
        public IntPtr pszFooter;
        public int cchFooter;
        public int iGroupId;
        public uint stateMask;
        public uint state;
        public uint uAlign;
        // rest omitted
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int left, top, right, bottom;
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    // wrappers for the specific types to aid marshalling
    private static int SendMessageGetGroupRect(IntPtr hwnd, int msg, IntPtr groupId, ref RECT rect)
    {
        // For LVM_GETGROUPRECT, lParam is a pointer to RECT where left must be set to LVGGR_*
        IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(rect));
        try
        {
            Marshal.StructureToPtr(rect, p, false);
            IntPtr res = SendMessage(hwnd, msg, groupId, p);
            if (res == IntPtr.Zero)
            {
                return 0;
            }
            RECT outRect = (RECT)Marshal.PtrToStructure(p, typeof(RECT));
            rect = outRect;
            return 1;
        }
        finally
        {
            Marshal.FreeHGlobal(p);
        }
    }

    private static IntPtr SendMessageGetGroupInfo(IntPtr hwnd, int msg, IntPtr groupId, IntPtr pGroup)
    {
        return SendMessage(hwnd, msg, groupId, pGroup);
    }

    [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = false)]
    private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

    #endregion
}
