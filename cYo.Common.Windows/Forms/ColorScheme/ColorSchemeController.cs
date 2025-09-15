﻿using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Windows.Forms.ColorScheme
{
    public class ColorSchemeController
    {
        public static event Action SystemColorsChanging;
        public static event Action SystemColorsChanged;

        public ColorSchemeController()
        {
            // force init color table
            byte unused = SystemColors.Window.R;

            var systemDrawingAssembly = typeof(Color).Assembly;

            //string colorTableField = Runtime.IsMono ? "s_colorTable" : "colorTable";
            string colorTableField = "colorTable";
            _colorTableField = systemDrawingAssembly.GetType("System.Drawing.KnownColorTable")
                .GetField(colorTableField, BindingFlags.Static | BindingFlags.NonPublic);

            _colorTable = readColorTable();
            SystemEvents.UserPreferenceChanging += userPreferenceChanging;

            OriginalColors = _colorTable.ToArray();
            KnownOriginalColors = KnownColors.Cast<int>()
                .ToDictionary(i => i, i => OriginalColors[i]);

            _threadDataProperty = systemDrawingAssembly.GetType("System.Drawing.SafeNativeMethods")
                .GetNestedType("Gdip", BindingFlags.NonPublic)
                .GetProperty("ThreadData", BindingFlags.Static | BindingFlags.NonPublic);

            //string systemBrushesKeyField = Runtime.IsMono ? "s_systemBrushesKey" : "SystemBrushesKey";
            string systemBrushesKeyField = "SystemBrushesKey";

            SystemBrushesKey = typeof(SystemBrushes)
                .GetField(systemBrushesKeyField, BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null);

            SystemPensKey = typeof(SystemPens)
                .GetField("SystemPensKey", BindingFlags.Static | BindingFlags.NonPublic)
                ?.GetValue(null);
        }

        private void userPreferenceChanging(object sender, UserPreferenceChangingEventArgs e)
        {
            if (e.Category != UserPreferenceCategory.Color)
                return;

            _colorTable = readColorTable();
            fireColorsChangedEvents();
        }

        private static void fireColorsChangedEvents()
        {
            SystemColorsChanging?.Invoke();
            SystemColorsChanged?.Invoke();
        }

        private int[] readColorTable() => (int[])_colorTableField.GetValue(null);

        public void SetColor(KnownColor knownColor, int argb)
        {
            setColor(knownColor, argb);

            if (SystemBrushesKey != null)
                ThreadData[SystemBrushesKey] = null;

            if (SystemPensKey != null)
                ThreadData[SystemPensKey] = null;

            fireColorsChangedEvents();
        }

        private void setColor(KnownColor knownColor, int argb) => _colorTable[(int)knownColor] = argb;

        public int GetOriginalColor(KnownColor knownColor) => OriginalColors[(int)knownColor];

        public int GetColor(KnownColor knownColor)
        {
            if (!KnownColors.Contains(knownColor))
                throw new ArgumentException();

            return _colorTable[(int)knownColor];
        }

        public IReadOnlyDictionary<int, int> Save() => KnownColors.Cast<int>().ToDictionary(i => i, i => _colorTable[i]);

        public void Load(IReadOnlyDictionary<int, int> saved)
        {
            foreach (var color in KnownColors)
            {
                var value = saved.TryGet((int)color, KnownOriginalColors[(int)color]);
                setColor(color, value);
            }

            if (SystemBrushesKey != null)
                ThreadData[SystemBrushesKey] = null;

            if (SystemPensKey != null)
                ThreadData[SystemPensKey] = null;

            fireColorsChangedEvents();
        }

        public void Reset(KnownColor color) => SetColor(color, OriginalColors[(int)color]);

        public void ResetAll() => Load(KnownOriginalColors);

        private IDictionary ThreadData => (IDictionary)_threadDataProperty.GetValue(null, null);

        private object SystemBrushesKey { get; }
        private object SystemPensKey { get; }

        public readonly HashSet<KnownColor> KnownColors = new HashSet<KnownColor>(
            new[]
            {
                SystemColors.Control,
                SystemColors.ControlText,

                SystemColors.ButtonFace, // menu gradient
				SystemColors.ButtonShadow, // menu border

				SystemColors.Window,
                SystemColors.WindowText,
                SystemColors.GrayText,

                SystemColors.HotTrack,
                SystemColors.Highlight,
                SystemColors.HighlightText,

                SystemColors.ActiveCaption,
                SystemColors.GradientActiveCaption,

                SystemColors.InactiveCaption,
                SystemColors.GradientInactiveCaption,

                SystemColors.ActiveBorder
            }.Select(_ => _.ToKnownColor())
        );

        private int[] OriginalColors { get; }
        private IReadOnlyDictionary<int, int> KnownOriginalColors { get; }

        private int[] _colorTable;
        private readonly FieldInfo _colorTableField;
        private readonly PropertyInfo _threadDataProperty;
    }

    public static class CollectionExtensions
    {
        public static TVal TryGet<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key)
        {
            if (key == null)
                return default;

            dict.TryGetValue(key, out var val);

            return val;
        }

        public static TVal TryGet<TKey, TVal>(this IReadOnlyDictionary<TKey, TVal> dict, TKey key, TVal defaultValue)
        {
            if (key == null || !dict.TryGetValue(key, out var val))
                return defaultValue;

            return val;
        }
    }

}
