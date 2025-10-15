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
        //if (listView.ShowGroups && listView.Groups.Count > 1)
        //{
        //    // sacrifice dark scrollbars to show readable group headers
        //    UXTheme.SetControlTheme(listView.Handle, "DarkMode_ItemsView");
        //    //Native.SetWindowTheme(hwnd,null, "DarkMode_ItemsView::ListView"); //messed up scrollbar
        //}
        //else
        //{
        //    // we don't have groups - let's get dark scrollbars
        //    UXTheme.SetControlTheme(listView.Handle);
        //}


        UXTheme.SetControlTheme(listView.Handle);

        // header has to be themed separately
        UXTheme.SetListViewHeaderTheme(listView.Handle);
        if (listView.ShowGroups && listView.Groups.Count > 1)
        {
            // Color GroupHeaders
            var colorizer = new Theme.ListViewTextColorizer(listView, ThemeColors.ItemView.GroupText, listView.BackColor, disableTheming: false);
            UXTheme.SetWindowTheme(colorizer.Handle);
        }
    }

    private static void SetTabControlUXTheme(TabControl tabControl)
    {
        UXTheme.SetTabControlTheme(tabControl.Handle);
    }

    #endregion
}
