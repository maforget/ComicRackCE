using cYo.Common.Win32;
using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Handlers;

internal partial class DarkControl
{
    internal static readonly Dictionary<Type, DarkControlDefinition> DefinitionTable = new()
    {
        [typeof(Button)] = new DarkControlDefinition
        {
            Theme = c => DarkButton((Button)c)
        },

        [typeof(CheckBox)] = new DarkControlDefinition
        {
            Theme = c => DarkCheckBox((CheckBox)c)
        },

        [typeof(CheckedListBox)] = new DarkControlDefinition
        {
            BackColor = DarkColors.ListBox.Back,
            BorderStyle = BorderStyle.FixedSingle
            //Theme = c => DarkCheckedListBox((CheckedListBox)c) // TODO: Dark Mode CheckBoxes
        },

        [typeof(ComboBox)] = new DarkControlDefinition
        {
            BackColor = DarkColors.ComboBox.Back,
            UXTheme = c => UXTheme.SetComboBoxTheme(c.Handle) //SetComboBoxUXTheme((ComboBox)c)
        },

        [typeof(DataGridView)] = new DarkControlDefinition
        {
            Theme = c => DarkDataGridView((DataGridView)c)
        },

        [typeof(Form)] = new DarkControlDefinition
        {
            UXTheme = c => UXTheme.SetWindowTheme(c.Handle) //SetWindowUXTheme((Form)c)
        },

        [typeof(GroupBox)] = new DarkControlDefinition
        {
            ForeColor = SystemColors.ControlText
        },

        [typeof(Label)] = new DarkControlDefinition
        {
            Theme = c => DarkLabel((Label)c)
        },

        [typeof(ListBox)] = new DarkControlDefinition
        {
            BackColor = DarkColors.ListBox.Back,
            BorderStyle = BorderStyle.FixedSingle
        },

        [typeof(ListView)] = new DarkControlDefinition
        {
            BackColor = DarkColors.ListBox.Back,
            Theme = c => DarkListView((ListView)c),
            UXTheme = c => DarkUXListView((ListView)c) //SetListViewUXTheme((ListView)c)
        },

        // RatingControl lives in ComicRack.Engine which is inaccessible
        //[typeof(RatingControl)] = new DarkControlDefinition
        //{
        //    BackColor = DarkColors.RatingControl.Back
        //},

        [typeof(RichTextBox)] = new DarkControlDefinition
        {
            BackColor = DarkColors.TextBox.Back,
            BorderStyle = BorderStyle.None,
            //Theme = c => DarkTextBoxBase((TextBoxBase)c) // Need to account for BackColor change in SmartQuery before adding this
        },

        [typeof(StatusStrip)] = new DarkControlDefinition
        {
            Theme = c => DarkStatusStrip((StatusStrip)c)
        },

        [typeof(TabControl)] = new DarkControlDefinition
        {
            UXTheme = c => UXTheme.SetTabControlTheme(c.Handle) //SetTabControlUXTheme((TabControl)c)
        },

        [typeof(TabPage)] = new DarkControlDefinition
        {
            Theme = c => DarkTabPage((TabPage)c)
        },

        [typeof(TextBox)] = new DarkControlDefinition
        {
            BackColor = DarkColors.TextBox.Back,
            Theme = c => DarkTextBoxBase((TextBox)c)
        },

        // DeviceEditControl TreeView
        [typeof(TreeView)] = new DarkControlDefinition
        {
            BorderStyle = BorderStyle.None,
            Theme = c => DarkTreeView((TreeView)c)
        },

        [typeof(ProgressBar)] = new DarkControlDefinition
        {
            BackColor = DarkColors.TextBox.Back,
        },
    };
}

