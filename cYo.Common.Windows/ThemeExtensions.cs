using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common.Drawing;
using cYo.Common.Win32;
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
            { typeof(GroupBox), c => ThemeGroupBox((GroupBox)c) },
            { typeof(ListBox), c => ThemeListBox((ListBox)c) },
            { typeof(ListView), c => ThemeListView((ListView)c) },
            { typeof(Panel), c => ThemePanel((Panel)c) },
            { typeof(StatusStrip), c => ThemeStatusStrip((StatusStrip)c) },
            { typeof(TextBox), c => ThemeTextBox((TextBox)c) },
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

            public static class TextBox
            {
                public static readonly Color Back = Color.FromArgb(56, 56, 56);
                public static readonly Color MouseOverBack = Color.FromArgb(86, 86, 86);
                public static readonly Color EnterBack = Color.FromArgb(71, 71, 71);
            }

            public static class SelectedText
            {
                public static readonly Color HighLight = Color.FromArgb(52, 67, 86);
                public static readonly Color Focus = Color.FromArgb(40, 100, 180);
            }

            public static class Header
            {
                public static readonly Color Back = Color.FromArgb(32, 32, 32);
                public static readonly Color Seperator = Color.FromArgb(99, 99, 99);
                public static readonly Color Text = SystemColors.WindowText;
                public static readonly Color GroupText = Color.Orange;
                public static readonly Color GroupSeperator = Color.White;
            }

            public static class Button
            {
                public static readonly Color Back = Color.FromArgb(51, 51, 51);
                public static readonly Color CheckedBack = Color.FromArgb(102, 102, 102);
                public static readonly Color Fore = Color.White;
                public static readonly Color Border = DefaultBorder;
                public static readonly Color MouseOverBack = Color.FromArgb(71, 71, 71);
            }
            public static class ToolStrip
            {
                // currently this is purely for statusstrip border.
                // TODO: move to renderer (will need to account for which borders need to be drawn)
                public static readonly Color BorderColor = Color.FromArgb(100, 100, 100);
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
            //panel.BackColor = SystemColors.Control;
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

            checkBox.FlatStyle = FlatStyle.Flat;
            if (checkBox.Appearance == Appearance.Button)
            {
                // although it has the appearance of a button, the theme engine doesn't style it as such, so we have to do it manually
                // this might be handled correctly in Win11 builds
                checkBox.BackColor = Colors.Button.Back;
                checkBox.ForeColor = Colors.Button.Fore;
                checkBox.FlatAppearance.BorderSize = 1;
                checkBox.FlatAppearance.BorderColor = Colors.Button.Border;
                checkBox.FlatAppearance.CheckedBackColor = Colors.Button.CheckedBack;
                checkBox.FlatAppearance.MouseOverBackColor = Colors.Button.MouseOverBack;
            }
            else
            {
                checkBox.Paint -= CheckBox_Paint;
                checkBox.Paint += CheckBox_Paint;
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
            // - dropdown button still has editable theming (seperator line)
            if (comboBox.DrawMode == DrawMode.Normal)
            {
                // OwnerDrawFixed is an unverified assumption (OwnerDrawVariable might be required in some cases)
                comboBox.DrawMode = DrawMode.OwnerDrawFixed;
                comboBox.DrawItem -= ComboBox_DrawItem;
                comboBox.DrawItem += ComboBox_DrawItem;
            }
        }
        private static void ThemeListBox(ListBox listBox)
        {
            listBox.BackColor = Colors.TextBox.Back;
            listBox.ForeColor = SystemColors.WindowText;
        }

        private static void ThemeListView(ListView listView)
        {
            listView.BackColor = Colors.TextBox.Back;
            listView.ForeColor = SystemColors.WindowText;

            if (!(listView is ListViewEx) && listView.View == View.Details && listView.HeaderStyle != ColumnHeaderStyle.None)
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
        #endregion


        #region Complex UX Theming
        private static void SetComboBoxUXTheme(ComboBox comboBox)
        {
            UXTheme.SetComboBoxTheme(comboBox.Handle);
        }

        private static void SetListViewUXTheme(ListView listView)
        {
            if (listView.ShowGroups && listView.HeaderStyle != ColumnHeaderStyle.None)
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

        private static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            if ((sender as ListView).View != View.Details)
            {
                e.DrawDefault = true;
                return;
            }

            e.DrawDefault = false;
            using (Brush bgBrush = new SolidBrush(Colors.Header.Back))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            }

            using (Pen sepPen = new Pen(Colors.Header.Seperator))
            {
                int x = e.Bounds.Right - 1;
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

        private static void CheckBox_Paint(object sender, PaintEventArgs e)
        {
            // based on CheckedListEx.OnDrawItem()
            // this is not a great solution and is currently not Dpi aware.
            var checkBox = sender as CheckBox;
            CheckState itemCheckState = checkBox.CheckState;
            CheckBoxState state;
            Size size;
            Brush backGroundBrush = new SolidBrush(checkBox.Parent.BackColor);

            if (!checkBox.Enabled)
            {
                e.Graphics.Clear(checkBox.Parent.BackColor);
            }
            if (Application.RenderWithVisualStyles)
            {
                if (checkBox.Enabled)
                {
                    state = itemCheckState == CheckState.Unchecked ? CheckBoxState.UncheckedNormal : itemCheckState == CheckState.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.MixedNormal;
                }
                else
                {
                    state = itemCheckState == CheckState.Unchecked ? CheckBoxState.UncheckedDisabled : itemCheckState == CheckState.Checked ? CheckBoxState.CheckedDisabled : CheckBoxState.MixedDisabled;
                }
                size = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);

                var x = 0;
                var y = 0;
                if (checkBox.CheckAlign == System.Drawing.ContentAlignment.MiddleRight)
                {
                    x = checkBox.Width - size.Width;
                    y = (checkBox.Height / 2) - (size.Height / 2);
                }
                // this causes checkboxes to be drawn in the wrong place.
                // could be due to logic bug, or needing to doublebuffer or some other image processing/oprimisation
                //if (checkBox.CheckAlign == System.Drawing.ContentAlignment.TopLeft)
                //{
                //    // default, expected, here so we can color unhandled cases in red
                //}
                //else if (checkBox.CheckAlign == System.Drawing.ContentAlignment.MiddleLeft)
                //{
                //    y = (e.ClipRectangle.Height / 2) - (size.Height / 2);
                //}
                //else if (checkBox.CheckAlign == System.Drawing.ContentAlignment.MiddleRight)
                //{
                //    x = e.ClipRectangle.Width - size.Width;
                //    y = (e.ClipRectangle.Height / 2) - (size.Height / 2);
                //}
                //else
                //{
                //    e.Graphics.Clear(Color.Red);
                //}

                using (backGroundBrush)
                {
                    e.Graphics.FillRectangle(backGroundBrush, x, y, size.Width, size.Height + 1);
                }
                e.Graphics.DrawImage(RenderDarkCheckbox(state, size), x, y);
            }
            else
            {
                size = new Size(14, 14);
            }
            if (!checkBox.Enabled)
            {
                // may need to check TextAlign similar to CheckAlign above
                TextRenderer.DrawText(e.Graphics, checkBox.Text, checkBox.Font, new Point(size.Width + 1, 2), SystemColors.GrayText);
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
