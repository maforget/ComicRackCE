using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Windows.Forms;

public static class ThemePens
{
    private static readonly Dictionary<Color, Pen> cache = new();

    public static class CheckBox
    {
        public static Pen Border => FromThemeColor(ThemeColors.CheckBox.Border);
        public static Pen UncheckedBorder => FromThemeColor(ThemeColors.CheckBox.UncheckedBorder);
        public static Pen UncheckedDisabledBorder => FromThemeColor(ThemeColors.CheckBox.UncheckedDisabledBorder);
    }

    public static class ComboBox
    {
        public static Pen Separator => FromThemeColor(ThemeColors.ComboBox.Separator);
    }

    public static class NiceTreeSkin
    {
        public static Pen Separator => FromThemeColor(ThemeColors.NiceTreeSkin.Separator);
    }

    public static class SplitButton
    {
        public static Pen SeparatorLeft => FromThemeColor(ThemeColors.SplitButton.SeparatorLeft);
        public static Pen SeparatorRight => FromThemeColor(ThemeColors.SplitButton.SeparatorRight);
    }

    public static Pen FromThemeColor(Color color)
    {
        if (!cache.TryGetValue(color, out var pen))
        {
            pen = new Pen(color);
            cache[color] = pen;
        }
        return pen;
    }

    public static void Reset()
    {
        foreach (var pen in cache.Values)
            pen.Dispose();
        cache.Clear();
    }
}
