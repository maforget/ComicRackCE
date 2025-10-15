using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Windows.Forms
{
    public static class ThemeBrushes
    {
        private static readonly Dictionary<Color, SolidBrush> cache = new();

        public static class Caption
        {
            public static Brush Back => FromThemeColor(ThemeColors.Caption.Back);
        }

        public static class CheckBox
        {
            public static Brush Back => FromThemeColor(ThemeColors.CheckBox.Back);
            public static Brush BackCorner => FromThemeColor(ThemeColors.CheckBox.BackCorner);
            public static Brush BackVertex => FromThemeColor(ThemeColors.CheckBox.BackVertex);
            //public static Brush Border => FromThemeColor(ThemeColors.CheckBox.Border);
            public static Brush BorderEdge => FromThemeColor(ThemeColors.CheckBox.BorderEdge);
            public static Brush BorderCorner => FromThemeColor(ThemeColors.CheckBox.BorderCorner);
            //public static Brush UncheckedBorder => FromThemeColor(ThemeColors.CheckBox.UncheckedBorder);
            public static Brush UncheckedBorderEdge => FromThemeColor(ThemeColors.CheckBox.UncheckedBorderEdge);
            public static Brush UncheckedBack => FromThemeColor(ThemeColors.CheckBox.UncheckedBack);
            public static Brush UncheckedBackCorner => FromThemeColor(ThemeColors.CheckBox.UncheckedBackCorner);
            public static Brush UncheckedBackVertex => FromThemeColor(ThemeColors.CheckBox.UncheckedBackVertex);
            //public static Brush UncheckedDisabledBorder => FromThemeColor(ThemeColors.CheckBox.UncheckedDisabledBorder);
            public static Brush UncheckedDisabledBorderEdge => FromThemeColor(ThemeColors.CheckBox.UncheckedDisabledBorderEdge);
            public static Brush UncheckedDisabledBackCorner => FromThemeColor(ThemeColors.CheckBox.UncheckedDisabledBackCorner);
            public static Brush UncheckedDisabledBackVertex => FromThemeColor(ThemeColors.CheckBox.UncheckedDisabledBackVertex);
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

        public static class SelectedText
        {
            public static Brush Highlight => FromThemeColor(ThemeColors.SelectedText.Highlight);
        }

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
}
