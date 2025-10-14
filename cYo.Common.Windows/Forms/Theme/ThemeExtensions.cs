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

namespace cYo.Common.Windows.Forms
{
	/// <summary>
	/// Class to centralisation application Theming. When theming is enabled, calls <see cref="KnownColorTableEx"/> on initialization to override built-in <see cref="System.Drawing.SystemColors"/> with theme-defined colors.<br/>
	/// Exposes <see cref="Theme(Control)"/> for recursive <see cref="Control"/> theming by setting class fields and leveraging <see cref="UXTheme"/> for native Windows OS theming.
	/// </summary>
	public static class ThemeExtensions
    {
        /// <summary>
        /// <para>Indicates whether Dark Mode has been <b>enabled</b>. Set on initialization and referenced in all public non-initialization <see cref="ThemeExtensions"/> calls.</para>
        /// <para>Also serves as a <b>global reference</b> for other classes.</para>
        /// </summary>
        /// <remarks>
        /// This is intended to mirror the <c>Application.IsDarkModeEnabled</c> which is available in .NET 9+.
        /// </remarks>
        public static bool IsDarkModeEnabled = false;

        private static bool IsThemed { get; set; } = false;

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

		/// <summary>
		/// Sets <see cref="IsDarkModeEnabled"/>. If <paramref name="useDarkMode"/> is <c>true</c>, initializes <see cref="KnownColorTableEx"/> which replaces built-in <see cref="System.Drawing.SystemColors"/> with Dark Mode colors.
		/// </summary>
		/// <param name="useDarkMode">Determines if the Dark Mode theme should be used (and <see cref="System.Drawing.SystemColors"/> replaced).</param>
		/// <remarks>
		/// If <see cref="IsDarkModeEnabled"/> is set to false, all calls to other <see cref="ThemeExtensions"/> functions will immediately return.
		/// </remarks>
		public static void Initialize(Theme.Themes theme)
        {
            IsDarkModeEnabled = theme == Forms.Theme.Themes.Dark;
            IsThemed = ThemeFactory(theme);

			if (IsDarkModeEnabled)
            {
                KnownColorTableEx darkColorTable = new KnownColorTableEx();
                darkColorTable.Initialize(IsDarkModeEnabled);
                darkColorTable.SetColor(KnownColor.WhiteSmoke, ThemeColors.BlackSmoke.ToArgb());
                UXTheme.Initialize();
            }
        }

        // Returns if a theme other than default is being applied
        private static bool ThemeFactory(Theme.Themes theme)
        {
            switch (theme)
            {
                case Forms.Theme.Themes.Dark:
                    ThemeColors.Register<DarkThemeColorTable>();
                    return true;
                case Forms.Theme.Themes.Default:
                default:
                    ThemeColors.Register<ThemeColorTable>();
                    return false;
            };
        }

        /// <summary>
		/// Runs the provided <see cref="Action"/> only if the Theme isn't <see cref="Theme.Themes.Default"/> or if <paramref name="onlyDrawIfDefault"/> is false and we are in the <see cref="Theme.Themes.Default"/> Theme, then it will always draw.
		/// </summary>
		/// <param name="action">The <see cref="Action"/> to run</param>
		/// <param name="onlyDrawIfDefault"></param>
		/// <returns>a <see cref="Boolean"/> if it was successful in drawing</returns>
		public static bool TryDrawTheme(Action action, bool onlyDrawIfDefault = false)
		{
            if ((!ThemeColors.IsDefault && !onlyDrawIfDefault) || (ThemeColors.IsDefault && onlyDrawIfDefault)) // We are themed or we have requested to only draw when in the default 
			{
				action();
                return true;
			}
            return false;
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
            {
                form.BackColor = ThemeColors.Material.Window;
                //form.ForeColor = SystemColors.ControlText;
                SetWindowUXTheme(form);
            }

            if (control.BackColor.IsSystemColor)
            {
                try
                {
                    control.BackColor = KnownColorTableEx.GetSystemColor(control.BackColor.ToKnownColor());
                }
                catch
                {}
            }

            if (control.ForeColor.IsSystemColor)
            {
                try
                {
                    control.ForeColor = KnownColorTableEx.GetSystemColor(control.ForeColor.ToKnownColor());
                }
                catch
                {}
            }

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
            //panel.BackColor = Color.Red;
            //if (panel.BackColor == Color.Transparent && panel.Parent.BackColor != Color.Transparent)
                //panel.BackColor = panel.Parent.BackColor; // SystemColors.Control; // changing this breaks checkboxes
        }

        private static void ThemeGroupBox(GroupBox groupBox)
        {
            groupBox.ForeColor = SystemColors.WindowText;
        }

        private static void ThemeButton(Button button)
        {
            if (button.Image == null && button.BackgroundImage == null && button.GetType() != typeof(SplitButton))
            {
                button.FlatStyle = FlatStyle.System;
            }
            else
            {
                button.FlatStyle = FlatStyle.Flat;
                if (button.FlatAppearance.BorderSize != 0)
                {
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatAppearance.BorderColor = ThemeColors.Button.Border;
                }
            }
        }

        private static void ThemeCheckBox(CheckBox checkBox)
        {
            checkBox.Paint += CheckBox_Paint;
            if (checkBox.Appearance == Appearance.Button)
            {
                // although it has the appearance of a button, the theme engine doesn't style it as such, so we have to do it manually
                // this might be handled correctly in Win11 builds
                checkBox.FlatStyle = FlatStyle.Flat;
                checkBox.BackColor = ThemeColors.Button.Back;
                checkBox.ForeColor = ThemeColors.Button.Text;
                checkBox.FlatAppearance.BorderSize = 1;
                checkBox.FlatAppearance.BorderColor = ThemeColors.Button.Border;
                checkBox.FlatAppearance.CheckedBackColor = ThemeColors.Button.CheckedBack;
                checkBox.FlatAppearance.MouseOverBackColor = ThemeColors.Button.MouseOverBack;
            }
            else if (checkBox.FlatStyle == FlatStyle.System)
            {
                checkBox.FlatStyle = FlatStyle.Standard;
            }
        }

        private static void ThemeComboBox(ComboBox comboBox)
        {
            comboBox.BackColor = ThemeColors.ComboBox.Back;
            comboBox.ForeColor = SystemColors.WindowText;

            // Blue -> Gray highlight
            // results in DropDown instead of DropDownList theme formatting (highlighted text when not dropped down)
            // we can get (mostly) work around it by not drawing the background in ComboBox_DrawItem but 2 issues:
            // - we lose visual feedback that this is the the selected box (could draw out focus rectangle)
            // - dropdown button still has editable theming (separator line)
            //if (comboBox.DrawMode == DrawMode.Normal)
            //{
            //    // OwnerDrawFixed is an unverified assumption (OwnerDrawVariable might be required in some cases)
            //    comboBox.DrawMode = DrawMode.OwnerDrawFixed;
            //    comboBox.DrawItem -= ComboBox_DrawItemHighlight;
            //    comboBox.DrawItem += ComboBox_DrawItemHighlight;
            //}
        }

        private static void ThemeDataGridView(DataGridView gridView)
        {
            gridView.EnableHeadersVisualStyles = false;
            gridView.BorderStyle = BorderStyle.None;
            gridView.BackgroundColor = ThemeColors.ListBox.Back;

            gridView.DefaultCellStyle.BackColor = ThemeColors.TextBox.Back;
            gridView.DefaultCellStyle.SelectionBackColor = ThemeColors.SelectedText.Highlight;
            gridView.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
            
            gridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            gridView.ColumnHeadersDefaultCellStyle.BackColor = ThemeColors.Header.Back;
            gridView.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;

            gridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            gridView.RowHeadersDefaultCellStyle.BackColor = ThemeColors.Header.Back;
            gridView.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
        }

        private static void ThemeLabel(Label label)
        {
            label.Paint += Label_Paint;
        }

        private static void ThemeListBox(ListBox listBox)
        {
            listBox.BackColor = ThemeColors.ListBox.Back;
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
            progressBar.BackColor = ThemeColors.TextBox.Back;
        }

        private static void ThemeRichTextBox(RichTextBox richTextBox)
        {
            richTextBox.BackColor = ThemeColors.TextBox.Back;
            richTextBox.BorderStyle = BorderStyle.None;
        }

        private static void ThemeListView(ListView listView)
        {
            listView.BackColor = ThemeColors.TextBox.Back;
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
                if (listView.Items.Count > 0) listView.Items[0].UseItemStyleForSubItems = false;
            }
        }

        private static void ThemeStatusStrip(StatusStrip statusStrip)
        {
            statusStrip.Renderer = new DarkToolStripRenderer();
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

        private static void ThemeTabPage(TabPage tabPage)
        {
            if (tabPage.BackColor == Color.Transparent && tabPage.Parent is TabControl)
                tabPage.BackColor = ThemeColors.Material.Content; // this is tabPage.Parent.BackColor, which is System.Control
        }

        private static void ThemeTextBox(TextBox textBox)
        {
            // TextBoxEx did not like BorderStyle being set 
            if (!(textBox is TextBoxEx))
                textBox.BorderStyle = BorderStyle.FixedSingle;

            textBox.BackColor = ThemeColors.TextBox.Back;
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
            if (treeView.GetType() == typeof(TreeView))
            {
                if (!treeView.IsHandleCreated)
                {
                    treeView.HandleCreated += (s, e) => ThemeTreeView((s as TreeView));
                    return;
                }
                // use the TreeViewEx.SetColor method to avoid having to DllImport & declare native constants
                TreeViewEx.SetColor(treeView);
            }

        }
        #endregion


        private static void SetCheckBoxUXTheme(CheckBox checkBox)
        {
            if (checkBox.Appearance == Appearance.Button)
            {
                UXTheme.SetControlTheme(checkBox.Handle);
            }
            else
            {
                UXTheme.SetControlTheme(checkBox.Handle, "DarkMode_Explorer", "Button");
            }
        }

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
            UXTheme.SetTabControlTheme(tabControl.Handle);
        }

        #endregion


        #region Custom Draw Event Handlers

        // TODO: handle disabled checkboxes with images/icons (currently they're wiped)
        public static void CheckBox_Paint(object sender, PaintEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            bool onlyDrawDisabledText = OsVersionEx.IsWindows11_OrGreater() || checkBox.Appearance == Appearance.Button;
            TextFormatFlags textFormatFlags = GetTextFormatFlags(checkBox);

            // default OS drawing for enabled Win11 CheckBox or Appearance.Button
            if (onlyDrawDisabledText && checkBox.Enabled) return;

            Rectangle boxRect = GetCheckRectangle(checkBox, e.Graphics);
            Rectangle textRect = checkBox.Appearance == Appearance.Button ? e.ClipRectangle : GetTextRectangle(checkBox, e.Graphics, boxRect);

            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw actual Check (Box + Mark)
            if (!onlyDrawDisabledText)
                DrawDarkCheckBox(checkBox, e.Graphics, boxRect);

            // Clear text area
            using (var backBrush = new SolidBrush(checkBox.BackColor))
                e.Graphics.FillRectangle(backBrush, textRect);

            // Draw text
            TextRenderer.DrawText(
                e.Graphics,
                checkBox.Text,
                checkBox.Font,
                textRect,
                checkBox.Enabled ? checkBox.ForeColor : SystemColors.GrayText,
                textFormatFlags
            );
        }

        private static void Label_Paint(object sender, PaintEventArgs e)
        {
            Label label = sender as Label;

            if (!label.Enabled)
            {
                TextFormatFlags textFormatFlags = GetTextFormatFlags(label);
                TextRenderer.DrawText(e.Graphics, label.Text, label.Font, label.ClientRectangle, SystemColors.GrayText, label.BackColor, textFormatFlags);
            }
        }

        private static Rectangle GetCheckRectangle(CheckBox checkBox, Graphics g)
        {
            Size glyphSize = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
            Point checkPosition = GetImageAlignmentPoint(checkBox.ClientRectangle, glyphSize, checkBox.CheckAlign);

            Rectangle boxRect = new Rectangle(checkPosition, glyphSize);

            if (checkBox.CheckAlign == System.Drawing.ContentAlignment.MiddleRight)
                boxRect.X -= 1;
            return boxRect;
        }

        private static Rectangle GetTextRectangle(CheckBox checkBox, Graphics g, Rectangle boxRect)
        {
            // this is the kind of thing Microsoft love to change between OS versions
            int padTextY = 1, padBoxRtl = 2, padBoxLtr = 3;

            Rectangle textRect = checkBox.ClientRectangle;
            textRect.Y -= FormUtility.ScaleDpiX(padTextY);

            if (checkBox.CheckAlign == System.Drawing.ContentAlignment.MiddleRight)
            {
                textRect.X += 1;
                textRect.Width = checkBox.Bounds.Width - FormUtility.ScaleDpiX(boxRect.Width) - FormUtility.ScaleDpiX(padBoxRtl);
            }
            else
            {
                textRect.X = boxRect.Right + FormUtility.ScaleDpiX(padBoxLtr);
                textRect.Width = checkBox.Bounds.Right - FormUtility.ScaleDpiX(textRect.X);
            }
            return textRect;
        }

        internal static Point GetImageAlignmentPoint(Rectangle bounds, Size imageSize, System.Drawing.ContentAlignment alignment)
        {
            int x = 0;
            int y = 0;

            switch (alignment)
            {
                case System.Drawing.ContentAlignment.TopLeft:
                    x = bounds.Left;
                    y = bounds.Top;
                    break;
                case System.Drawing.ContentAlignment.TopCenter:
                    x = bounds.Left + (bounds.Width - imageSize.Width) / 2;
                    y = bounds.Top;
                    break;
                case System.Drawing.ContentAlignment.TopRight:
                    x = bounds.Right - imageSize.Width;
                    y = bounds.Top;
                    break;
                case System.Drawing.ContentAlignment.MiddleLeft:
                    x = bounds.Left;
                    y = bounds.Top + (bounds.Height - imageSize.Height) / 2;
                    break;
                case System.Drawing.ContentAlignment.MiddleCenter:
                    x = bounds.Left + (bounds.Width - imageSize.Width) / 2;
                    y = bounds.Top + (bounds.Height - imageSize.Height) / 2;
                    break;
                case System.Drawing.ContentAlignment.MiddleRight:
                    x = bounds.Right - imageSize.Width;
                    y = bounds.Top + (bounds.Height - imageSize.Height) / 2;
                    break;
                case System.Drawing.ContentAlignment.BottomLeft:
                    x = bounds.Left;
                    y = bounds.Bottom - imageSize.Height;
                    break;
                case System.Drawing.ContentAlignment.BottomCenter:
                    x = bounds.Left + (bounds.Width - imageSize.Width) / 2;
                    y = bounds.Bottom - imageSize.Height;
                    break;
                case System.Drawing.ContentAlignment.BottomRight:
                    x = bounds.Right - imageSize.Width;
                    y = bounds.Bottom - imageSize.Height;
                    break;
            }

            return new Point(x, y);
        }

        private static void SetUncheckedBrushes(CheckBox checkBox, out Pen borderPen, out Brush borderEdgeBrush, out Brush backCornerBrush, out Brush backVertexBrush)
        {
            borderPen = ThemePens.CheckBox.UncheckedBorder;

            borderEdgeBrush = ThemeBrushes.CheckBox.UncheckedBorderEdge;
            backCornerBrush = ThemeBrushes.CheckBox.UncheckedBackCorner;
            backVertexBrush = ThemeBrushes.CheckBox.UncheckedBackVertex;

            if (!checkBox.Enabled)
            {
                borderPen = ThemePens.CheckBox.UncheckedDisabledBorder;

                borderEdgeBrush = ThemeBrushes.CheckBox.UncheckedDisabledBorderEdge;
                backCornerBrush = ThemeBrushes.CheckBox.UncheckedDisabledBackCorner;
                backVertexBrush = ThemeBrushes.CheckBox.UncheckedDisabledBackVertex;
            }
        }

        private static TextFormatFlags GetTextFormatFlags(ButtonBase button)
        {
            TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix;

            switch (button.TextAlign)
            {
                case System.Drawing.ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case System.Drawing.ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case System.Drawing.ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
                case System.Drawing.ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case System.Drawing.ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
                case System.Drawing.ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case System.Drawing.ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case System.Drawing.ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case System.Drawing.ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
            }

            // Right-to-left text handling
            if (button.RightToLeft == RightToLeft.Yes)
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;

            // Mnemonics (Alt+char underlines)
            if (button.UseMnemonic)
                flags &= ~TextFormatFlags.NoPrefix;
            else
                flags |= TextFormatFlags.NoPrefix;

            // Ellipsis
            if (button.AutoEllipsis)
                flags |= TextFormatFlags.EndEllipsis;

            return flags;
        }

        private static TextFormatFlags GetTextFormatFlags(Label label)
        {
            TextFormatFlags flags =
                TextFormatFlags.WordBreak |
                TextFormatFlags.TextBoxControl |
                TextFormatFlags.PreserveGraphicsTranslateTransform |
                TextFormatFlags.PreserveGraphicsClipping;

            // Alignment
            switch (label.TextAlign)
            {
                case System.Drawing.ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case System.Drawing.ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case System.Drawing.ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
                case System.Drawing.ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case System.Drawing.ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
                case System.Drawing.ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case System.Drawing.ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case System.Drawing.ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case System.Drawing.ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
            }

            // Mnemonics
            if (!label.UseMnemonic)
                flags |= TextFormatFlags.NoPrefix;
            //else if (!label.ShowKeyboardCues)
            //    flags |= TextFormatFlags.HidePrefix;

            // Right-to-left layout
            if (label.RightToLeft == RightToLeft.Yes)
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;

            // AutoEllipsis (only when not multiline)
            if (!label.AutoSize && label.AutoEllipsis)
                flags |= TextFormatFlags.EndEllipsis;

            return flags;
        }


        private static void DrawDarkCheckMark(CheckBox checkBox, Graphics g, Rectangle boxRect)
        {
            Point[] checkMark = new Point[6]
                {
                    new Point(boxRect.Left + 2, boxRect.Top + (boxRect.Height/2)+2),
                    new Point(boxRect.Left + boxRect.Width / 2 -1, boxRect.Bottom - 1),
                    new Point(boxRect.Right - 2, boxRect.Top + 5),
                    new Point(boxRect.Right - 2, boxRect.Top + 3),
                    new Point(boxRect.Left + boxRect.Width / 2 - 1, boxRect.Bottom - 3),
                    new Point(boxRect.Left + 2, boxRect.Top + (boxRect.Height / 2)),
                 };

            g.DrawPolygon(checkBox.Enabled ? SystemPens.ControlText : SystemPens.GrayText, checkMark);
            g.FillPolygon(checkBox.Enabled ? SystemBrushes.ControlText : SystemBrushes.GrayText, checkMark);
        }

        private static void DrawDarkCheckBox(CheckBox checkBox, Graphics g, Rectangle boxRect)
        {
            g.Clear(checkBox.BackColor);

            Brush backBrush = checkBox.Checked ? ThemeBrushes.CheckBox.Back : ThemeBrushes.CheckBox.UncheckedBack;
            g.FillRectangle(backBrush, new Rectangle(boxRect.X + 2, boxRect.Y, boxRect.Width - 3, boxRect.Height));
            g.FillRectangle(backBrush, new Rectangle(boxRect.X, boxRect.Y + 2, boxRect.Width, boxRect.Height - 3));

            //boxRect.Inflate(-2, -3);
            // Fill background
            // Checkmark if checked
            if (checkBox.Checked)
                DrawDarkCheckMark(checkBox, g, boxRect);

            int left = boxRect.X;
            int right = boxRect.X + boxRect.Width;
            int top = boxRect.Y;
            int bottom = boxRect.Y + boxRect.Height;

            Pen borderPen = ThemePens.CheckBox.Border;
            
            Brush borderEdgeBrush = ThemeBrushes.CheckBox.BorderEdge;
            Brush backCornerBrush = ThemeBrushes.CheckBox.BackCorner;
            Brush backVertexBrush = ThemeBrushes.CheckBox.BackVertex;

            if (!checkBox.Checked)
                SetUncheckedBrushes(checkBox, out borderPen, out borderEdgeBrush, out backCornerBrush, out backVertexBrush);

            // Main Border
            g.DrawLine(borderPen, new Point(left + 3, top), new Point(right - 3, top));
            g.DrawLine(borderPen, new Point(left + 3, bottom), new Point(right - 3, bottom));
            g.DrawLine(borderPen, new Point(left, top + 3), new Point(left, bottom - 3));
            g.DrawLine(borderPen, new Point(right, top + 3), new Point(right, bottom - 3));

            // Border Edges
            // Top - Left + Right
            g.FillRectangle(borderEdgeBrush, left + 2, top, 1, 1);
            g.FillRectangle(borderEdgeBrush, right - 2, top, 1, 1);
            // Bottom - Left + Right
            g.FillRectangle(borderEdgeBrush, left + 2, bottom, 1, 1);
            g.FillRectangle(borderEdgeBrush, right - 2, bottom, 1, 1);
            // Left - Top + Bottom
            g.FillRectangle(borderEdgeBrush, left, top + 2, 1, 1);
            g.FillRectangle(borderEdgeBrush, left, bottom - 2, 1, 1);
            // Right - Top + Bottom
            g.FillRectangle(borderEdgeBrush, right, top + 2, 1, 1);
            g.FillRectangle(borderEdgeBrush, right, bottom - 2, 1, 1);

            // Inner Corners
            g.FillRectangle(backCornerBrush, left + 1, top + 1, 1, 1);
            g.FillRectangle(backCornerBrush, right - 1, top + 1, 1, 1);
            g.FillRectangle(backCornerBrush, right - 1, bottom - 1, 1, 1);
            g.FillRectangle(backCornerBrush, left + 1, bottom - 1, 1, 1);

            // Inner Vertices
            // Top - Left + Right
            g.FillRectangle(backVertexBrush, left + 2, top + 1, 1, 1);
            g.FillRectangle(backVertexBrush, right - 2, top + 1, 1, 1);
            // Bottom - Left + Right
            g.FillRectangle(backVertexBrush, left + 2, bottom - 1, 1, 1);
            g.FillRectangle(backVertexBrush, right - 2, bottom - 1, 1, 1);
            // Left - Top + Bottom
            g.FillRectangle(backVertexBrush, left + 1, top + 2, 1, 1);
            g.FillRectangle(backVertexBrush, left + 1, bottom - 2, 1, 1);
            // Right - Top + Bottom
            g.FillRectangle(backVertexBrush, right - 1, top + 2, 1, 1);
            g.FillRectangle(backVertexBrush, right - 1, bottom - 2, 1, 1);

            if (checkBox.Checked)
            {
                Brush cornerBrush = ThemeBrushes.CheckBox.BorderCorner;
                g.FillRectangle(cornerBrush, left + 1, top, 1, 1);
                g.FillRectangle(cornerBrush, left + 1, bottom, 1, 1);
                g.FillRectangle(cornerBrush, left, top + 1, 1, 1);
                g.FillRectangle(cornerBrush, left, bottom - 1, 1, 1);
                g.FillRectangle(cornerBrush, right - 1, top, 1, 1);
                g.FillRectangle(cornerBrush, right - 1, bottom, 1, 1);
                g.FillRectangle(cornerBrush, right, top + 1, 1, 1);
                g.FillRectangle(cornerBrush, right, bottom - 1, 1, 1);
            }
            // ... and that's how you draw a ✓. simples, right?
            // ... and that's how you draw a ✓. simples, right?

        }

        // Use custom SelectedText Highlight color.
        // related: cYo.Common.Windows.Forms.ComboBoxSkinner.comboBox_DrawItem (private, requires instantiation, comes with ComboBoxSkinner baggage)
        private static void ComboBox_DrawItemHighlight(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ComboBox comboBox = (ComboBox)sender;
            var item = comboBox.Items[e.Index];

            // override SelectedText highlighting
            if (comboBox.DroppedDown)
            {
                e.DrawBackground();
                if (e.State.HasFlag(DrawItemState.Selected))
                {
                    using (Brush highlightBrush = new SolidBrush(ThemeColors.SelectedText.Highlight))
                    {
                        e.Graphics.FillRectangle(highlightBrush, e.Bounds);
                    }
                    ControlPaint.DrawBorder(e.Graphics, e.Bounds, ThemeColors.SelectedText.Focus, ButtonBorderStyle.Solid);
                }
            }
            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                using (StringFormat format = new StringFormat(StringFormatFlags.NoWrap)
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near
                })
                {
                    e.Graphics.DrawString(comboBox.GetItemText(item), comboBox.Font, brush, e.Bounds, format);
                }
            }
        }


        /// <summary>
        /// <para><see cref="ListView.OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs)"/> method to handle dark <see cref="ColumnHeader"/> text on dark background.</para>
        /// <para><c>DrawDefault</c> is executed unless <see cref="IsDarkModeEnabled"/> is <paramref name="true"/> and <see cref="ListView.View"/> is set to <see cref="View.Details"/></para>
        /// </summary>
        public static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // explicit IsDarkModeEnabled check as public (referenced in TaskDialog and ListStyles)
            if (!IsDarkModeEnabled || (sender as ListView).View != View.Details)
            {
                e.DrawDefault = true;
                return;
            }

            e.DrawDefault = false;
            using (Brush bgBrush = new SolidBrush(ThemeColors.Header.Back))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
            }

            using (Pen sepPen = new Pen(ThemeColors.Header.Separator))
            {
                int x = e.Bounds.Right - 2;
                int y1 = e.Bounds.Top;
                int y2 = e.Bounds.Bottom;
                e.Graphics.DrawLine(sepPen, x, y1, x, y2);
            }

            using (Brush textBrush = new SolidBrush(ThemeColors.Header.Text))
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
            if (e.Item.ListView.View != View.Details)
                e.DrawDefault = true;

        }

        private static void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.Item.ListView.View != View.Details || e.Item == null)
            {
                e.DrawDefault = true;
                return;
            }
            ListView listView = (ListView)sender;
            Rectangle bounds = e.Bounds;
            Rectangle textRect = bounds; // Updated based on whether we have images
            ImageList imageList = listView.SmallImageList;

            Color backColor = e.Item.BackColor;
            Color textColor = e.Item.ForeColor;

            if (e.Item.Selected)
            {
                backColor = ThemeColors.SelectedText.Highlight;
                textColor = SystemColors.HighlightText; // currently same as ForeColor
            }

            // Fill background
            using (var backBrush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(backBrush, bounds);
            if (e.Item.Selected)
                ControlPaint.DrawBorder(e.Graphics, e.Bounds, ThemeColors.SelectedText.Focus, ButtonBorderStyle.Solid);

            // Handle first column having images
            if (e.ColumnIndex == 0 && imageList != null)
            {
                int imageWidth = imageList.ImageSize.Width;
                int imageHeight = imageList.ImageSize.Height;
                int imageY = bounds.Top + (bounds.Height - imageHeight) / 2;
                int imageX = bounds.Left + 4; // small padding from left

                // Shift text based on imageList != null, whether this item has an image or not
                textRect.X += imageX + imageWidth;

                // Draw the image if there is one to draw
                if (e.Item.ImageIndex >= 0 && e.Item.ImageIndex < imageList.Images.Count)
                    imageList.Draw(e.Graphics, imageX, imageY, imageWidth, imageHeight, e.Item.ImageIndex);
            }

            textRect.X += 2;
            textRect.Width = bounds.Right - textRect.X;

            // Draw text
            TextRenderer.DrawText(
                e.Graphics,
                e.SubItem.Text,
                e.SubItem.Font,
                textRect,
                textColor,
                backColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            // Draw column divider
            if (e.ColumnIndex < listView.Columns.Count)
            {
                using (var pen = new Pen(Color.FromArgb(77, 77, 77)))
                    e.Graphics.DrawLine(pen, bounds.Right - 2, bounds.Top, bounds.Right - 2, bounds.Bottom);
            }
        }

        #endregion


        #region Custom Paint Event Handlers
        private static void ToolStripStatusLabel_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(ThemeColors.ToolStrip.BorderColor, 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
            }
        }

        #endregion


        #region TextBox overrides
        private static void TextBox_MouseLeave(object sender, EventArgs e)
        {
            if (!(sender as TextBox).Focused)
                (sender as TextBox).BackColor = ThemeColors.TextBox.Back;
        }
        private static void TextBox_MouseHover(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Enabled && !textBox.Focused)
                textBox.BackColor = ThemeColors.TextBox.MouseOverBack;
        }
        private static void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.BackColor = ThemeColors.TextBox.EnterBack;
        }
        private static void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.BackColor = ThemeColors.TextBox.Back;
        }
        #endregion

        #region External Helper Functions
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

        public static void SetToolStripItemColor (ToolStripArrowRenderEventArgs e)
        {
            if (!IsDarkModeEnabled) return;
            e.ArrowColor = Color.White;
        }

        public static void SetToolStripItemColor(ToolStripItemTextRenderEventArgs e)
        {
            if (!IsDarkModeEnabled) return;
            e.TextColor = Color.White;
        }

        public static void SetSidePanelColor(Control control)
        {
            if (!IsDarkModeEnabled) return;
            control.BackColor = ThemeColors.Material.SidePanel;
            if (control.GetType() == typeof(TreeView) || control.GetType() == typeof(TreeViewEx))
            {
                TreeViewEx.SetColor((TreeView)control, ThemeColors.Material.SidePanel);
            }
        }

        public static void SetBorderStyle(Control control, BorderStyle? borderStyle = null)
        {
            if (!IsDarkModeEnabled) return;
            control.GetType().GetProperty("BorderStyle")?.SetValue(control, borderStyle ?? BorderStyle.None);
        }

        public static void RenderItemCheck(Graphics graphics, Rectangle rect, Color background, Color? checkColor = null)
        {
            if (!IsDarkModeEnabled) return;

            using (Brush brush = new SolidBrush(background))
            {
                graphics.FillRectangle(brush, rect);
            }
            using (var pen = new Pen(checkColor ?? Color.White, 2))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.DrawLines(pen, new[]
                {
                    new Point(rect.Left + 4, rect.Top + rect.Height/2 - 1),
                    new Point(rect.Left + rect.Width/3 + rect.Width/6, rect.Bottom - 5),
                    new Point(rect.Right - 4, rect.Top + 3)
                });
            }
        }

        public static void RenderOverflowButton(ToolStripItemRenderEventArgs e)
        {
            if (!IsDarkModeEnabled) return;

            var g = e.Graphics;
            var item = e.Item as ToolStripOverflowButton;

            const int overflowButtonWidth = 12;
            Rectangle overflowArrowRect = new Rectangle(item.Width - overflowButtonWidth + 1, item.Height - 8, 9, 5);

            Point middle = new Point(overflowArrowRect.Left + overflowArrowRect.Width / 2, overflowArrowRect.Top + overflowArrowRect.Height / 2);
            Point[] arrow = new Point[] {
                    new Point(middle.X - 2, middle.Y - 1),
                    new Point(middle.X + 3, middle.Y - 1),
                    new Point(middle.X,     middle.Y + 2)
                };

            g.FillPolygon(SystemBrushes.ControlText, arrow);
            g.DrawLine(SystemPens.ControlText, overflowArrowRect.Right - 7, overflowArrowRect.Y - 2, overflowArrowRect.Right - 3, overflowArrowRect.Y - 2);
        }
        #endregion

        public class DarkToolStripRenderer : ToolStripProfessionalRenderer
        {
            public DarkToolStripRenderer()
                : base(new DarkProfessionalColorsEx())
            {
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                e.TextColor = Color.White;
                base.OnRenderItemText(e);
            }

            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                e.ArrowColor = Color.White;
                base.OnRenderArrow(e);
            }

            protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
            {
                base.OnRenderItemCheck(e);
                ThemeExtensions.RenderItemCheck(e.Graphics, e.ImageRectangle, ColorTable.CheckPressedBackground);
            }

            protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
            {
                base.OnRenderOverflowButtonBackground(e);
                RenderOverflowButton(e);
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                //base.OnRenderToolStripBorder(e);
            }
        }
    }
}
