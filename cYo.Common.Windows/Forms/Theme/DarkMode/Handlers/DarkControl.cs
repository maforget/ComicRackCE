using cYo.Common.Win32;
using cYo.Common.Windows.Forms.Theme.DarkMode.Rendering;
using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using cYo.Common.Windows.Forms.Theme.Resources;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Handlers;

internal partial class DarkControl
{
    private static void DarkButton(Button button)
    {
        if (button.Image == null && button.BackgroundImage == null && button.GetType() != typeof(SplitButton))
            button.FlatStyle = FlatStyle.System;
        else
            DarkButtonBase(button);
    }

    private static void DarkCheckBox(CheckBox checkBox)
    {
		checkBox.SafeSubscribe(nameof(CheckBox.Paint), PaintDark.CheckBox);
        if (checkBox.Appearance == Appearance.Button)
            DarkButtonBase(checkBox);
        else if (checkBox.FlatStyle == FlatStyle.System)
            checkBox.FlatStyle = FlatStyle.Standard;
    }

    private static void DarkDataGridView(DataGridView gridView)
    {
        gridView.EnableHeadersVisualStyles = false;
        gridView.BorderStyle = BorderStyle.None;
        gridView.BackgroundColor = DarkColors.ListBox.Back;

        gridView.DefaultCellStyle.BackColor = DarkColors.TextBox.Back;
        gridView.DefaultCellStyle.SelectionBackColor = DarkColors.SelectedText.Highlight;
        gridView.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;

        gridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        gridView.ColumnHeadersDefaultCellStyle.BackColor = DarkColors.Header.Back;
        gridView.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;

        gridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        gridView.RowHeadersDefaultCellStyle.BackColor = DarkColors.Header.Back;
        gridView.RowHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
    }

    private static void DarkLabel(Label label)
    {
        label.SafeSubscribe(nameof(Label.Paint), PaintDark.Label);
    }

    private static void DarkListView(ListView listView)
    {
        //if (!(listView is ListViewEx) && listView.View == View.Details && listView.HeaderStyle != ColumnHeaderStyle.None)
        if (!listView.OwnerDraw && listView.View == View.Details && listView.HeaderStyle != ColumnHeaderStyle.None)
        {
            listView.OwnerDraw = true;
			listView.SafeSubscribe(nameof(ListView.DrawItem), DrawDarkListView.Item);
            listView.SafeSubscribe(nameof(ListView.DrawColumnHeader), DrawDarkListView.ColumnHeader);
            listView.SafeSubscribe(nameof(ListView.DrawSubItem), DrawDarkListView.SubItem);
            if (listView.Items.Count > 0) listView.Items[0].UseItemStyleForSubItems = false;
        }
    }

    private static void DarkUXListView(ListView listView)
    {
        UXTheme.SetListViewTheme(listView.Handle);
        if (listView.ShowGroups && listView.Groups.Count > 1)
            listView.ThemeGroupHeader();
    }

    private static void DarkStatusStrip(StatusStrip statusStrip)
    {
        statusStrip.Renderer = new ThemeToolStripProRenderer();
        foreach (ToolStripStatusLabel tsLabel in statusStrip.Items.OfType<ToolStripStatusLabel>())
        {
            if (tsLabel.BorderStyle.Equals(Border3DStyle.SunkenOuter))
            {
                tsLabel.BorderSides = ToolStripStatusLabelBorderSides.None;
                tsLabel.SafeSubscribe(nameof(ToolStripStatusLabel.Paint), PaintDark.ToolStripStatusLabel);
            }
        }
    }

    private static void DarkTabPage(TabPage tabPage)
    {
        if (tabPage.BackColor == Color.Transparent && tabPage.Parent is TabControl)
            tabPage.BackColor = DarkColors.UIComponent.Content; // this is tabPage.Parent.BackColor, which is System.Control
    }

    private static void DarkTextBoxBase(TextBoxBase textBox)
    {
        // Don't add a border if there's none to begin with (e.g. NumericUpDown)
        if (textBox.BorderStyle != BorderStyle.None)
            textBox.BorderStyle = BorderStyle.FixedSingle;
        DarkEventHandlers.TextBox_Mouse(textBox);
    }

    // HACK : TreeView backColor is set in about 5 different places
    private static void DarkTreeView(TreeView treeView)
    {
        // Only change if default BackColor
        if (treeView.BackColor == SystemColors.Window)
        {
            // TreeViewEx.OnCreateControl sets BackColor to DarkColors.TreeView.Back, so TreeViewEx should never have default BackColor
            // But it does for RemoteConnectionView (may be due to TreeView -> TreeViewEx somewhere)
            if (treeView.GetType() == typeof(TreeViewEx))
                treeView.BackColor = DarkColors.UIComponent.SidePanel; // RemoteConnectionView - ComicListLibraryBrowser.tvQueries
            else
                treeView.BackColor = DarkColors.TreeView.Back; // DeviceEditControl
        }
        treeView.ForeColor = DarkColors.TreeView.Text;
        treeView.WhenHandleCreated(treeView => TreeViewEx.SetColor((TreeView)treeView));
    }

    #region Helpers
    private static void DarkButtonBase(ButtonBase button)
    {
        button.FlatStyle = FlatStyle.Flat;

        //if (button is CheckBox)
        //{
        button.BackColor = DarkColors.Button.Back;
        button.ForeColor = DarkColors.Button.Text;
        button.FlatAppearance.CheckedBackColor = DarkColors.Button.CheckedBack;
        button.FlatAppearance.MouseOverBackColor = DarkColors.Button.MouseOverBack;
        //}

        if (button is CheckBox || button.FlatAppearance.BorderSize != 0)
        {
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = DarkColors.Button.Border;
        }
    }
	#endregion
}
