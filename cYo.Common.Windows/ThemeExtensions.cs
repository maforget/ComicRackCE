using cYo.Common.Drawing;
using cYo.Common.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static cYo.Common.Windows.Forms.ComboBoxSkinner;

namespace cYo.Common.Windows.Forms
{
    /// <summary>
    /// Class to centralisation application Theming. When theming is enabled, calls <see cref="KnownColorTableEx"/> on initialization to override built-in <see cref="SystemColors"/> with theme-defined colors.<br/>
    /// Exposes <see cref="Theme(Control)"/> for recursive <see cref="Control"/> theming by setting class fields and leveraging <see cref="UXTheme"/> for native Windows OS theming.
    /// </summary>
    public static class ThemeExtensions
    {
        /// <summary>
        /// <para>Indicates whether Dark Mode has been <b>enabled</b>. Set on initialization and referenced in all public non-initialization <see cref="ThemeExtensions"/> calls.</para>
        /// <para>Also serves as a <b>global reference</b> for other classes.</para>
        /// </summary>
        /// <remarks>
        /// This is intended to mirror the <c>Application.IsDarkModeEnabled</c> which is available . .NET 9+.
        /// </remarks>
        public static bool IsDarkModeEnabled = false;

        /// <summary>
        /// <see cref="Control"/> to Theme Handler method mappings. For controls which don't respond to <see cref="SystemColors"/> change, or
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
            { typeof(ListBox), c => ThemeListBox((ListBox)c) },
            { typeof(ListView), c => ThemeListView((ListView)c) },
            { typeof(Panel), c => ThemePanel((Panel)c) },
            { typeof(ProgressBar), c => ThemeProgressBar((ProgressBar)c) },
            { typeof(RichTextBox), c => ThemeRichTextBox((RichTextBox)c) },
            { typeof(StatusStrip), c => ThemeStatusStrip((StatusStrip)c) },
            { typeof(TextBox), c => ThemeTextBox((TextBox)c) },
            { typeof(TreeView), c => ThemeTreeView((TreeView)c) }
        };

        /// <summary>
        /// <see cref="Control"/> to <see cref="UXTheme"/>Handler method mappings. For controls that have a theme class other than <c>"DarkMode_Explorer"</c>.<br/>
        /// Also for controls which a Win32 subclass, requiring that the parent <see cref="Control"/> and subclass are both themed in separate calls.
        /// </summary>
        private static readonly Dictionary<Type, Action<Control>> complexUXThemeHandlers = new()
        {
            { typeof(ComboBox), c => SetComboBoxUXTheme((ComboBox)c) },
            { typeof(ListView), c => SetListViewUXTheme((ListView)c) },
            { typeof(TabControl), c => SetTabControlUXTheme((TabControl)c) }
        };

        /// <summary>
        /// Theme <see cref="Color"/> definitions. For when <see cref="SystemColors"/>, <see cref="KnownColor"/> and <see cref="ProfessionalColors"/> simply isn't enough (or <b>consistent</b> enough)
        /// </summary>
        public static class Colors
        {
            // WhiteSmoke is (245,245,245), but (10,10,10) would be too dark
            public static readonly Color BlackSmoke = Color.FromArgb(48, 48, 48);

            public static readonly Color DefaultBorder = Color.FromArgb(155, 155, 155);

            public static readonly Color GroupHeader = Color.FromArgb(155, 155, 155);

            public static class Button
            {
                public static readonly Color Back = Color.FromArgb(51, 51, 51);
                public static readonly Color CheckedBack = Color.FromArgb(102, 102, 102);
                public static readonly Color Fore = Color.White;
                public static readonly Color Border = DefaultBorder;
                public static readonly Color MouseOverBack = Color.FromArgb(71, 71, 71);
            }

            public static class Header
            {
                public static readonly Color Back = Color.FromArgb(32, 32, 32);
                public static readonly Color Separator = Color.FromArgb(99, 99, 99);
                public static readonly Color Text = SystemColors.WindowText;
                public static readonly Color GroupText = Color.Orange;
                public static readonly Color GroupSeparator = Color.White;
            }

            public static class SelectedText
            {
                public static readonly Color HighLight = Color.FromArgb(52, 67, 86);
                public static readonly Color Focus = Color.FromArgb(40, 100, 180);
            }

            public static class SmartQuery
            {
                public static readonly Color Back = TextBox.Back;
                public static readonly Color Exception = Color.FromArgb(125, 31, 31);
                public static readonly Color Fore = SystemColors.ControlText;
                public static readonly Color Keyword = Color.FromArgb(250, 198, 0);
                public static readonly Color Qualifier = Color.FromArgb(76, 163, 255);
                public static readonly Color Negation = Color.Red;
                public static readonly Color String = Color.FromArgb(255, 125, 125);

            }

            public static class TextBox
            {
                public static readonly Color Back = Color.FromArgb(56, 56, 56);
                public static readonly Color MouseOverBack = Color.FromArgb(86, 86, 86);
                public static readonly Color EnterBack = Color.FromArgb(71, 71, 71);
            }

            public static class ToolStrip
            {
                // currently this is purely for statusstrip border.
                // TODO: move to renderer (will need to account for which borders need to be drawn)
                public static readonly Color BorderColor = Color.FromArgb(100, 100, 100);
            }

            public static class TreeView
            {
                public static readonly Color Back = SystemColors.Window;
                public static readonly Color Fore = SystemColors.ControlText;
            }
        }

        /// <summary>
		/// Sets <see cref="IsDarkModeEnabled"/>. If <paramref name="useDarkMode"/> is <c>true</c>, initializes <see cref="KnownColorTableEx"/> which replaces built-in <see cref="SystemColors"/> with Dark Mode colors.
        /// </summary>
        /// <param name="useDarkMode">Determines if the Dark Mode theme should be used (and <see cref="SystemColors"/> replaced).</param>
        /// <remarks>
        /// If <see cref="IsDarkModeEnabled"/> is set to false, all calls to other <see cref="ThemeExtensions"/> functions will immediately return.
        /// </remarks>
        public static void Initialize(bool useDarkMode = false)
        {
            IsDarkModeEnabled = useDarkMode;

            if (!useDarkMode) return;

            KnownColorTableEx darkColorTable = new KnownColorTableEx();
            darkColorTable.Initialize(useDarkMode);

            darkColorTable.SetColor(KnownColor.WhiteSmoke, Colors.BlackSmoke.ToArgb());

            UXTheme.Initialize();
        }

        /// <summary>
		/// Themes a <see cref="Control"/>, recursively. The specifics are determined by the control <see cref="Type"/> mapping in <see cref="themeHandlers"/> and <see cref="complexUXThemeHandlers"/>.<br/>
        /// Also sets the window theme for a <see cref="Form"/>.
        /// </summary>
        /// <param name="control"><see cref="Control"/> to be themed.</param>
        /// <remarks>
        /// Theming can essentially be broken down into these (optional) parts:<br/>
        /// - <see cref="themeHandlers"/> - Set <typeparamref name="ForeColor"/> and <typeparamref name="BackColor"/><br/>
        /// - <see cref="themeHandlers"/> - Attach custom <see cref="EventHandler"/> (<c>Draw</c>, <c>Paint</c>, <c>Mouse</c>)<br/>
        /// - <see cref="themeHandlers"/> - Set <see cref="BorderStyle"/> and <see cref="FlatStyle"/> (either for looks, or because it's required for <see cref="UXTheme.SetControlTheme(IntPtr, string, string)"/> to identify a part)<br/>
        /// - <see cref="complexUXThemeHandlers"/> - Apply native Windows OS theming via <see cref="UXTheme.SetControlTheme(IntPtr, string, string)"/><br/>
        /// </remarks>
        public static void Theme(this Control control)
        {
            if (!IsDarkModeEnabled) return;

            if (control is Form form)
                SetWindowUXTheme(form);

            if (themeHandlers.TryGetValue(control.GetType(), out var theme))
            {
                theme(control);
            }
            else if (themeHandlers.TryGetValue(control.GetType().BaseType, out var themeBase))
            {
                themeBase(control);
            }

            SetControlUXTheme(control);

            foreach (Control childControl in control.Controls)
                Theme(childControl);
        }

        /// <summary>
		/// Sets <see cref="Form"/> window <see cref="UXTheme"/>. A handle is required: if not yet created, subscribes to <see cref="Form.OnHandleCreated(EventArgs)"/>.
        /// </summary>
        /// <param name="form"><see cref="Form"/> to be (window) themed.</param>
        private static void SetWindowUXTheme(Form form)
        {
            if (!form.IsHandleCreated)
            {
                form.HandleCreated += (s, e) => SetWindowUXTheme((s as Form));
                return;
            }
            UXTheme.SetWindowTheme(form.Handle);
        }

        /// <summary>
		/// Sets <see cref="Control"/> <see cref="UXTheme"/>. A handle is required: if not yet created, subscribes to <see cref="Control.OnHandleCreated(EventArgs)"/>.
        /// </summary>
        /// <param name="control"><see cref="Control"/> to be themed. <see cref="complexUXThemeHandlers"/> is checked to see if control <see cref="Type"/> requires complex handling.</param>
        private static void SetControlUXTheme(Control control)
        {
            if (!control.IsHandleCreated)
            {
                control.HandleCreated += (s, e) => SetControlUXTheme((s as Control));
                return;
            }

            if (complexUXThemeHandlers.TryGetValue(control.GetType(), out var uxTheme))
            {
                uxTheme(control);
            }
            else if (complexUXThemeHandlers.TryGetValue(control.GetType().BaseType, out var uxThemeBase))
            {
                uxThemeBase(control);
            }
            else
            {
                UXTheme.SetControlTheme(control.Handle);
            }
        }

        #region Theme Handlers

        private static void ThemePanel(Panel panel)
        {
            panel.BackColor = SystemColors.Control;
        }
        private static void ThemeGroupBox(GroupBox groupBox)
        {
            groupBox.ForeColor = SystemColors.WindowText;
        }

        private static void ThemeButton(Button button)
        {
            if (button.Image == null && button.BackgroundImage == null)
            {
                button.FlatStyle = FlatStyle.System;
            }
            else
            {
                button.FlatStyle = FlatStyle.Flat;
                if (button.FlatAppearance.BorderSize != 0)
                {
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.BorderColor = Colors.Button.Border;
                }
            }
        }

        private static void ThemeCheckBox(CheckBox checkBox)
        {
            if (checkBox.Appearance == Appearance.Button)
            {
                // although it has the appearance of a button, the theme engine doesn't style it as such, so we have to do it manually
                // this might be handled correctly in Win11 builds
                checkBox.FlatStyle = FlatStyle.Flat;
                checkBox.BackColor = Colors.Button.Back;
                checkBox.ForeColor = Colors.Button.Fore;
                checkBox.FlatAppearance.BorderSize = 1;
                checkBox.FlatAppearance.BorderColor = Colors.Button.Border;
                checkBox.FlatAppearance.CheckedBackColor = Colors.Button.CheckedBack;
                checkBox.FlatAppearance.MouseOverBackColor = Colors.Button.MouseOverBack;
            }
            else if (OsVersionEx.IsWindows11_OrGreater() && checkBox.FlatStyle == FlatStyle.System)
            {
                checkBox.FlatStyle = FlatStyle.Standard; // Win11 can theme Flat/Standard, so only change if System
            }
            else if (!OsVersionEx.IsWindows11_OrGreater())
            {
                checkBox.FlatStyle = FlatStyle.Flat; // Win10 19044 draws standard checkboxes w/ white background, so force flat
            }
        }
        private static void ThemeComboBox(ComboBox comboBox)
        {
            comboBox.BackColor = Colors.TextBox.Back;
            comboBox.ForeColor = SystemColors.WindowText;

            // Blue -> Gray highlight
            // results in DropDown instead of DropDownList theme formatting (highlighted text when not dropped down)
            // we can get (mostly) work around it by not drawing the background in ComboBox_DrawItem but 2 issues:
            // - we lose visual feedback that this is the the selected box (could draw out focus rectangle)
            // - dropdown button still has editable theming (separator line)
            if (comboBox.DrawMode == DrawMode.Normal)
            {
                // OwnerDrawFixed is an unverified assumption (OwnerDrawVariable might be required in some cases)
                comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                comboBox.DrawItem -= ComboBox_DrawItem;
                comboBox.DrawItem += ComboBox_DrawItem;
            }
        }

        private static void ThemeDataGridView(DataGridView gridView)
        {
            gridView.EnableHeadersVisualStyles = false;
            gridView.DefaultCellStyle.SelectionBackColor = Colors.SelectedText.HighLight;
            gridView.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
            gridView.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void ThemeListBox(ListBox listBox)
        {
            listBox.BackColor = Colors.TextBox.Back;
            listBox.ForeColor = SystemColors.WindowText;
            listBox.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Manually set <see cref="ProgressBar"/> background due to bug in Dark Mode.
        /// </summary>
        /// <param name="progressBar"><see cref="ProgressBar"/> to be themed.</param>
        /// <remarks>
        /// Disabling Visual Styles does not seem to be required despite base .NET 4.8 <see cref="ProgressBar"/> not disabling it. 
        /// <a href="https://github.com/dotnet/winforms/blob/be8f5b01a967606a1e6cf57af373ef7785aa1fe0/src/System.Windows.Forms/System/Windows/Forms/Controls/ProgressBar/ProgressBar.cs#L82">(.NET 10)</a><br/>
        /// Invisible background/border bug is tracked in <a href="https://github.com/dotnet/winforms/issues/11914">[Dark Mode] Improve visual contrast of the ProgressBar control in dark mode. #11914</a><br/>
        /// Related issue to watch: <a href="https://github.com/dotnet/winforms/issues/11938">[Dark Mode] The BackColor of the ProgressBar control should not be changed #11938</a>
        /// </remarks>
        private static void ThemeProgressBar(ProgressBar progressBar)
        {
            progressBar.BackColor = Colors.TextBox.Back;
        }

        private static void ThemeRichTextBox(RichTextBox richTextBox)
        {
            richTextBox.BackColor = Colors.TextBox.Back;
            richTextBox.BorderStyle = BorderStyle.None;
        }

        private static void ThemeListView(ListView listView)
        {
            listView.BackColor = Colors.TextBox.Back;
            listView.ForeColor = SystemColors.WindowText;

            //if (!(listView is ListViewEx) && listView.View == View.Details && listView.HeaderStyle != ColumnHeaderStyle.None)
            if (!listView.OwnerDraw && listView.View == View.Details && listView.HeaderStyle != ColumnHeaderStyle.None)
            {
                listView.OwnerDraw = true;
                listView.DrawItem -= ListView_DrawItem;
                listView.DrawItem += ListView_DrawItem;
                listView.DrawColumnHeader -= ListView_DrawColumnHeader;
                listView.DrawColumnHeader += ListView_DrawColumnHeader;
                listView.DrawSubItem -= ListView_DrawSubItem;
                listView.DrawSubItem += ListView_DrawSubItem;
            }
        }

        private static void ThemeStatusStrip(StatusStrip statusStrip)
        {
            statusStrip.Renderer = Renderer();
            foreach (ToolStripStatusLabel tsLabel in statusStrip.Items.OfType<ToolStripStatusLabel>())
            {
                if (tsLabel.BorderStyle.Equals(Border3DStyle.SunkenOuter))
                {
                    tsLabel.BorderSides = ToolStripStatusLabelBorderSides.None;
                    tsLabel.Paint -= ToolStripStatusLabel_Paint;
                    tsLabel.Paint += ToolStripStatusLabel_Paint;
                }

            }
        }

        private static void ThemeTextBox(TextBox textBox)
        {
            // TextBoxEx did not like BorderStyle being set 
            if (!(textBox is TextBoxEx))
                textBox.BorderStyle = BorderStyle.FixedSingle;

            textBox.BackColor = Colors.TextBox.Back;
            textBox.ForeColor = SystemColors.ControlText;
            textBox.MouseLeave -= TextBox_MouseLeave;
            textBox.MouseLeave += TextBox_MouseLeave;
            textBox.MouseHover -= TextBox_MouseHover;
            textBox.MouseHover += TextBox_MouseHover;
            textBox.Enter -= TextBox_Enter;
            textBox.Enter += TextBox_Enter;
            textBox.Leave -= TextBox_Leave;
            textBox.Leave += TextBox_Leave;
        }

        private static void ThemeTreeView(TreeView treeView)
        {
            if (!treeView.IsHandleCreated)
            {
                treeView.HandleCreated += (s, e) => ThemeTreeView((s as TreeView));
                return;
            }
            // use the TreeViewEx.SetColor method to avoid having to DllImport & declare native constants
            TreeViewEx.SetColor(treeView);
        }

        #endregion


        #region Complex UX Theming
        private static void SetComboBoxUXTheme(ComboBox comboBox)
        {
            UXTheme.SetComboBoxTheme(comboBox.Handle);
        }

        /// <summary>
        /// Custom <see cref="ListView"/> <see cref="UXTheme"/> handling as theme class depends on <c>ListView</c> settings.
        /// </summary>
        /// <param name="listView"><see cref="ListView"/> to be themed.</param>
        /// <remarks>
        /// <see cref="ListView.Groups"/> dark headers bug tracked in <a href="https://github.com/dotnet/winforms/issues/3320">ListView Group Header Color #3320</a>
        /// </remarks>
        private static void SetListViewUXTheme(ListView listView)
        {
            // ShowGroups defaults to true, so let's check the count before giving up on scrollbars
            //if (listView.ShowGroups && listView.HeaderStyle != ColumnHeaderStyle.None) // why do we care about ColumnHeaderStyle when this affects Group headers?
            if (listView.ShowGroups && listView.Groups.Count > 1)
            {
                // sacrifice dark scrollbars to show readable group headers
                UXTheme.SetControlTheme(listView.Handle, "DarkMode_ItemsView");
                //Native.SetWindowTheme(hwnd,null, "DarkMode_ItemsView::ListView"); //messed up scrollbar
            }
            else
            {
                // we don't have groups - let's get dark scrollbars
                UXTheme.SetControlTheme(listView.Handle);
            }

            // header has to be themed separately
            UXTheme.SetListViewHeaderTheme(listView.Handle);
        }

        private static void SetTabControlUXTheme(TabControl tabControl)
        {
            UXTheme.SetControlTheme(tabControl.Handle, null, "DarkMode::FileExplorerBannerContainer");
        }

        #endregion


        #region Custom Draw Event Handlers

        // trimmed version of cYo.Common.Windows.Forms.ComboBoxSkinner.comboBox_DrawItem
        // TODO: de-duplicate
        private static void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            ComboBox comboBox = (ComboBox)sender;
            var obj = comboBox.Items[e.Index];
            IComboBoxItem comboBoxItem = obj as IComboBoxItem;
            bool flag = (e.State & DrawItemState.ComboBoxEdit) != 0;
            bool flag2 = comboBoxItem != null && comboBoxItem.IsSeparator && !flag && e.Index > 0;
            Pen separatorPen = ThemeExtensions.IsDarkModeEnabled ? SystemPens.ControlText : SystemPens.ControlLight;


            Color foreColor = SystemColors.ControlText;

            // only draw background and use actual ForeColor when DroppedDown
            if (comboBox.DroppedDown)
            {
                e.DrawBackground();
                // override SelectedText highlighting
                if (e.State.HasFlag(DrawItemState.Selected))
                {
                    using (Brush highlightBrush = new SolidBrush(Colors.SelectedText.HighLight))
                    {
                        e.Graphics.FillRectangle(highlightBrush, e.Bounds);
                    }
                    ControlPaint.DrawBorder(e.Graphics, e.Bounds, Colors.SelectedText.Focus, ButtonBorderStyle.Solid);
                }
            }
            // focus rectangle doesn't match theme (it's dotted black/grey instead of solid blue)
            //e.DrawFocusRectangle();
            // this doesn't look great as it's an inner border and omits the button
            // not sure how to draw out of combobox items bounds
            // ControlPaint.DrawBorder(e.Graphics, e.Bounds, Color.Red, ButtonBorderStyle.Solid);

            // e.ForeColor -> SystemColors.ControlText override. so we get white instead of black text
            using (Brush brush = new SolidBrush(SystemColors.ControlText))
            {
                Rectangle rectangle = e.Bounds;
                if (comboBoxItem != null && comboBoxItem.IsOwnerDrawn)
                {
                    comboBoxItem.Draw(e.Graphics, rectangle, e.ForeColor, comboBox.Font);
                }
                else
                {
                    using (StringFormat format = new StringFormat(StringFormatFlags.NoWrap)
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Near
                    })
                    {
                        e.Graphics.DrawString(comboBox.GetItemText(obj), comboBox.Font, brush, rectangle, format);
                    }
                }
            }
        }


        /// <summary>
        /// <para><see cref="ListView.OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs)"/> method to handle dark <see cref="ColumnHeader"/> text on dark background.</para>
        /// <para><c>DrawDefault</c> is executed unless <see cref="IsDarkModeEnabled"/> is <paramref name="true"/> and <see cref="ListView.View"/> is set to <see cref="View.Details"/></para>
        /// </summary>
        /// <param name="form"><see cref="Form"/> to be (window) themed.</param>
        public static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // explicit IsDarkModeEnabled check as public (referenced in TaskDialog and ListStyles)
            if (!IsDarkModeEnabled || (sender as ListView).View != View.Details)
            {
                e.DrawDefault = true;
                return;
            }

            e.DrawDefault = false;
            using (Brush bgBrush = new SolidBrush(Colors.Header.Back))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            }

            using (Pen sepPen = new Pen(Colors.Header.Separator))
            {
                int x = e.Bounds.Right - 2;
                int y1 = e.Bounds.Top;
                int y2 = e.Bounds.Bottom;
                e.Graphics.DrawLine(sepPen, x, y1, x, y2);
            }

            using (Brush textBrush = new SolidBrush(Colors.Header.Text))
            {
                // Draw the header text with custom color and font
                e.Graphics.DrawString(
                    e.Header.Text,
                    e.Font,
                    textBrush,
                    e.Bounds,
                    new StringFormat
                    {
                        Alignment = StringAlignment.Near, // left align text
                        LineAlignment = StringAlignment.Center, // vertically center text
                        Trimming = StringTrimming.EllipsisCharacter
                    });
            }
        }

        private static void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private static void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        #endregion


        #region Custom Paint Event Handlers
        private static void ToolStripStatusLabel_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Colors.ToolStrip.BorderColor, 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
            }
        }

        #endregion


        #region TextBox overrides
        private static void TextBox_MouseLeave(object sender, EventArgs e)
        {
            if (!(sender as TextBox).Focused)
                (sender as TextBox).BackColor = Colors.TextBox.Back;
        }
        private static void TextBox_MouseHover(object sender, EventArgs e)
        {
            if ((sender as TextBox).Enabled)
                (sender as TextBox).BackColor = Colors.TextBox.MouseOverBack;
        }
        private static void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.BackColor = Colors.TextBox.EnterBack;
            if (!textBox.Multiline)
            {
                textBox.BorderStyle = BorderStyle.Fixed3D;
            }
        }
        private static void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.BackColor = Colors.TextBox.Back;
            if (!textBox.Multiline)
            {
                textBox.BorderStyle = BorderStyle.FixedSingle;
            }
        }
        #endregion

        public static Bitmap RenderDarkCheckbox(CheckBoxState state, Size size)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                CheckBoxRenderer.DrawCheckBox(g, new Point(0, 0), state);
            }
            ImageProcessing.Invert(bmp);
            return bmp;
        }

        public static ToolStripProfessionalRenderer Renderer()
        {
            return (new ToolStripProfessionalRenderer(new DarkProfessionalColorsEx()));
        }
    }
}
