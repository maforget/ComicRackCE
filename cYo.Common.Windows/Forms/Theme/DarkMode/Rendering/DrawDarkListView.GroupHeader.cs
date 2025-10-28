using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using cYo.Common.Windows.Forms.Theme.Resources;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Rendering;

internal static partial class DrawDarkListView
{

    #region Native interop & constants
    private class Native
    {
        internal const int LVM_FIRST = 0x1000;
        internal const int LVM_GETGROUPRECT = LVM_FIRST + 98;
        internal const int LVM_GETGROUPINFO = LVM_FIRST + 149;
        internal const int LVM_SETTEXTCOLOR = LVM_FIRST + 36;
        internal const int LVGGR_HEADER = 1;

        internal const uint LVGF_HEADER = 0x00000001;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct LVGROUP
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
        internal struct RECT
        {
            public int left, top, right, bottom;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        // wrappers for the specific types to aid marshalling
        internal static int SendMessageGetGroupRect(IntPtr hwnd, int msg, IntPtr groupId, ref RECT rect)
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

        internal static IntPtr SendMessageGetGroupInfo(IntPtr hwnd, int msg, IntPtr groupId, IntPtr pGroup)
        {
            return SendMessage(hwnd, msg, groupId, pGroup);
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = false)]
        internal static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport("gdi32.dll")]
        internal static extern int ExcludeClipRect(IntPtr hdc, int left, int top, int right, int bottom);

        [DllImport("gdi32.dll")]
        internal static extern int SelectClipRgn(IntPtr hdc, IntPtr hrgn);

    }
    #endregion

    public static void ThemeGroupHeader(this ListView listView, bool reduceFlicker = true) => new GroupHeaderEx(listView, reduceFlicker);

    public class GroupHeaderEx : NativeWindow, IDisposable
    {
        private readonly ListView listView;
        private readonly Color textColor;
        private readonly Color headerBackColor;
        //private readonly bool disableTheming;
        private bool attached = false;
        //private bool reduceFlicker;
        //private bool suppressNextErase;

        public GroupHeaderEx(ListView listView, Color? textColor, Color? headerBackColor, bool reduceFlicker = true)
        {
            this.listView = listView ?? throw new ArgumentNullException(nameof(listView));
            this.textColor = textColor ?? ThemeColors.ItemView.GroupText;
            this.headerBackColor = headerBackColor ?? listView.BackColor;
            //this.reduceFlicker = reduceFlicker;

            // Attach to handle lifecycle
            if (listView.IsHandleCreated)
                Attach();

            listView.HandleCreated += (s, e) => Attach();
            listView.HandleDestroyed += (s, e) => Detach();
        }

        #region Constructors
        public GroupHeaderEx(ListView listView, Color textColor, Color lineColor)
            : this(listView, textColor, lineColor, true)
        {
        }

        public GroupHeaderEx(ListView listView, Color textColor)
            : this(listView, textColor, null, true)
        {
        }

        public GroupHeaderEx(ListView listView, bool reduceFlicker)
            : this(listView, null, null, reduceFlicker)
        {
        }

        public GroupHeaderEx(ListView listView)
            : this(listView, null, null, true)
        {
        }
        #endregion

        #region Attach/Detach/Dispose
        private void Attach()
        {
            if (attached) return;
            AssignHandle(listView.Handle);
            attached = true;

            // Set item/subitem text/background color (affects items & subitems)
            //SendMessage(listView.Handle, LVM_SETTEXTCOLOR, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(textColor));
            //SendMessage(listView.Handle, LVM_SETBKCOLOR, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(backColor));

            listView.Invalidate();
        }

        private void Detach()
        {
            if (!attached) return;
            ReleaseHandle();
            attached = false;
        }

        public void Dispose()
        {
            Detach();
        }
        #endregion

        protected override void WndProc(ref Message m)
        {
            const int WM_PAINT = 0x000F;
            const int WM_ERASEBKGND = 0x0014;
            const int WM_MOUSEMOVE = 0x0200;
            //const int WM_LBUTTONDOWN = 0x0201;

            switch (m.Msg)
            {
                case WM_ERASEBKGND:
                    IntPtr hdc = m.WParam;
                    foreach (ListViewGroup group in listView.Groups)
                    {
                        if (group.Items.Count > 0)
                        {
                            Rectangle rc = GetGroupHeaderRect(group);
                            Native.ExcludeClipRect(hdc, rc.Left, rc.Top, rc.Right, rc.Bottom);
                        }
                    }
                    break;

                case WM_MOUSEMOVE:
                    if (IsMouseOverGroupHeader())
                    {
                        // skip repaint triggers over header
                        return;
                    }
                    break;
            }

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
            if (!listView.IsHandleCreated) return;
            if (listView.Groups == null || listView.Groups.Count == 0) return; // may also need to return if listView.ShowGroups is false

            using (Graphics g = Graphics.FromHwnd(listView.Handle))
            {
                // Prepare text drawing flags
                TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;

                foreach (ListViewGroup group in listView.Groups)
                {
                    int nativeGroupId = GetNativeGroupId(group);
                    if (nativeGroupId < 0 || group.Items.Count == 0)
                        continue;

                    // Get rectangle for the group header (LVGGR_HEADER = 1)
                    Native.RECT rc = new Native.RECT { top = Native.LVGGR_HEADER }; // top, not left
                    int res = Native.SendMessageGetGroupRect(listView.Handle, Native.LVM_GETGROUPRECT, (IntPtr)nativeGroupId, ref rc);
                    if (res == 0)
                        continue; // failed

                    Rectangle bounds = Rectangle.FromLTRB(rc.left, rc.top, rc.right, rc.bottom);

                    // Fill background
                    using (var bg = new SolidBrush(headerBackColor))
                        g.FillRectangle(bg, bounds);

                    // Draw header text
                    string headerText = GetGroupHeaderText(nativeGroupId);
                    if (string.IsNullOrEmpty(headerText))
                        headerText = group.Header ?? string.Empty;

                    bounds.Inflate(-10, 0);
                    TextRenderer.DrawText(g, headerText, listView.Font, bounds, textColor, flags);

                    Size headerTextSize = TextRenderer.MeasureText(headerText, listView.Font);

                    using (Pen pen = new Pen(ThemeColors.ItemView.GroupSeparator, 1))
                        g.DrawLine(pen, rc.left + headerTextSize.Width + 10, rc.top + FormUtility.ScaleDpiY(10), rc.right - FormUtility.ScaleDpiY(12), rc.top + FormUtility.ScaleDpiY(10));
                }
            }
        }

        private bool IsMouseOverGroupHeader()
        {
            if (listView.Groups.Count == 0)
                return false;

            Point cursor = listView.PointToClient(Cursor.Position);
            foreach (ListViewGroup group in listView.Groups)
            {
                Rectangle rc = GetGroupHeaderRect(group);
                if (rc.Contains(cursor))
                    return true;
            }
            return false;
        }

        private Rectangle GetGroupHeaderRect(ListViewGroup group)
        {
            int id = GetNativeGroupId(group);
            if (id < 0)
                return Rectangle.Empty;

            Native.RECT rc = new Native.RECT { top = Native.LVGGR_HEADER }; // LVGGR_HEADER = 1
            if (Native.SendMessageGetGroupRect(listView.Handle, Native.LVM_GETGROUPRECT, (IntPtr)id, ref rc) != 0)
                return Rectangle.FromLTRB(rc.left, rc.top, rc.right, rc.bottom);
                //return Rectangle.FromLTRB(rc.left, rc.top, rc.right, rc.top + FormUtility.ScaleDpiY(24));

            return Rectangle.Empty;
        }

        private string GetGroupHeaderText(int nativeGroupId)
        {
            // Build LVGROUP with buffer for header (unicode)
            Native.LVGROUP group = new Native.LVGROUP();
            group.cbSize = (uint)Marshal.SizeOf(typeof(Native.LVGROUP));
            group.mask = Native.LVGF_HEADER;
            // allocate buffer (260 chars)
            group.pszHeader = Marshal.AllocHGlobal(260 * 2);
            group.cchHeader = 260;

            try
            {
                IntPtr pGroup = Marshal.AllocHGlobal(Marshal.SizeOf(group));
                try
                {
                    Marshal.StructureToPtr(group, pGroup, false);
                    IntPtr res = Native.SendMessageGetGroupInfo(listView.Handle, Native.LVM_GETGROUPINFO, (IntPtr)nativeGroupId, pGroup);
                    if (res == IntPtr.Zero)
                        return null;

                    Native.LVGROUP resultGroup = (Native.LVGROUP)Marshal.PtrToStructure(pGroup, typeof(Native.LVGROUP));
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
                return listView.Groups.IndexOf(group);
            }
            catch
            {
                return -1;
            }
        }
    }
}