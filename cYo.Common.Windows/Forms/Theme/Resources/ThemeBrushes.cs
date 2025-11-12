using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Windows.Forms.Theme.Resources;

/// <summary>
/// Brushes for <see cref="ThemeColors"/>.
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

    public static class DetailView
    {
        public static Brush RowHighlight => FromThemeColor(ThemeColors.DetailView.RowHighlight);
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

    /// <summary>
    /// Returns a <see cref="Brush"/> with the specified <paramref name="color"/>.
    /// </summary>
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
