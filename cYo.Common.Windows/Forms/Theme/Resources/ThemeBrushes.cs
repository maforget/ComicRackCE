using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Windows.Forms.Theme.Resources;

/// <summary>
/// TODO : ADD SUMMARY
/// </summary>
public static class ThemeBrushes
{
    private static readonly Dictionary<Color, SolidBrush> cache = new();

    public static class Caption
    {
        public static Brush Back => FromThemeColor(ThemeColors.Caption.Back);
    }

    public static class CollapsibleGroupBox
    {
        public static Brush HeaderText => FromThemeColor(ThemeColors.CollapsibleGroupBox.HeaderText);
    }

    public static class Header
    {
        public static Brush Back => FromThemeColor(ThemeColors.Header.Back);
    }

    public static class NiceTreeSkin
    {
        public static Brush Separator => FromThemeColor(ThemeColors.NiceTreeSkin.Separator);
    }

    public static class Stack
    {
        public static Brush Fill => FromThemeColor(ThemeColors.Stack.Fill);
    }


    //public static class DarkMode
    //{
    //    public static class SelectedText
    //    {
    //        public static Brush Highlight => FromThemeColor(ThemeColors.DarkMode.SelectedText.Highlight);
    //    }
    //}

    public static Brush FromThemeColor(Color color)
    {
        if (!cache.TryGetValue(color, out var brush))
        {
            brush = new SolidBrush(color);
            cache[color] = brush;
        }
        return brush;
    }

    public static void Reset()
    {
        foreach (var brush in cache.Values)
            brush.Dispose();
        cache.Clear();
    }
}
