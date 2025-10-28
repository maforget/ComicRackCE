using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Rendering;

internal class DrawDarkComboBox
{
    // Use custom SelectedText Highlight color.
    // related: cYo.Common.Windows.Forms.ComboBoxSkinner.comboBox_DrawItem (private, requires instantiation, comes with ComboBoxSkinner baggage)
    internal static void ItemHighlight(object sender, DrawItemEventArgs e)
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
                e.Graphics.FillRectangle(DarkBrushes.SelectedText.Highlight, e.Bounds);
                ControlPaint.DrawBorder(e.Graphics, e.Bounds, DarkColors.SelectedText.Focus, ButtonBorderStyle.Solid);
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
}
