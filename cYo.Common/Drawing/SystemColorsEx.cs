using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Drawing
{
    /// <summary>
    /// source: <a href="https://github.com/dotnet/runtime">dotnet/runtime</a>. (.NET Foundation, MIT license)<br/>
    /// <c>src/libraries/System.Drawing.Primitives/src/System/Drawing/SystemColorsEx.cs</c><br/>
    /// source refers to Dark Theme colors as <c>AlternateSystemColors<c>; this has been kept the same.
    /// </summary>
    /// <remarks>Adds AlternateSystemColors (Dark Mode) support to SystemColorsEx.</remarks>
    public static class SystemColorsEx
    {
        internal static bool s_useAlternativeColorSet = false;

        public static Color ActiveBorder => KnownColorTableEx.GetSystemColor(KnownColor.ActiveBorder);
        public static Color ActiveCaption => KnownColorTableEx.GetSystemColor(KnownColor.ActiveCaption);
        public static Color ActiveCaptionText => KnownColorTableEx.GetSystemColor(KnownColor.ActiveCaptionText);
        public static Color AppWorkspace => KnownColorTableEx.GetSystemColor(KnownColor.AppWorkspace);

        public static Color ButtonFace => KnownColorTableEx.GetSystemColor(KnownColor.ButtonFace);
        public static Color ButtonHighlight => KnownColorTableEx.GetSystemColor(KnownColor.ButtonHighlight);
        public static Color ButtonShadow => KnownColorTableEx.GetSystemColor(KnownColor.ButtonShadow);

        public static Color Control => KnownColorTableEx.GetSystemColor(KnownColor.Control);
        public static Color ControlDark => KnownColorTableEx.GetSystemColor(KnownColor.ControlDark);
        public static Color ControlDarkDark => KnownColorTableEx.GetSystemColor(KnownColor.ControlDarkDark);
        public static Color ControlLight => KnownColorTableEx.GetSystemColor(KnownColor.ControlLight);
        public static Color ControlLightLight => KnownColorTableEx.GetSystemColor(KnownColor.ControlLightLight);
        public static Color ControlText => KnownColorTableEx.GetSystemColor(KnownColor.ControlText);

        public static Color Desktop => KnownColorTableEx.GetSystemColor(KnownColor.Desktop);

        public static Color GradientActiveCaption => KnownColorTableEx.GetSystemColor(KnownColor.GradientActiveCaption);
        public static Color GradientInactiveCaption => KnownColorTableEx.GetSystemColor(KnownColor.GradientInactiveCaption);
        public static Color GrayText => KnownColorTableEx.GetSystemColor(KnownColor.GrayText);

        public static Color Highlight => KnownColorTableEx.GetSystemColor(KnownColor.Highlight);
        public static Color HighlightText => KnownColorTableEx.GetSystemColor(KnownColor.HighlightText);
        public static Color HotTrack => KnownColorTableEx.GetSystemColor(KnownColor.HotTrack);

        public static Color InactiveBorder => KnownColorTableEx.GetSystemColor(KnownColor.InactiveBorder);
        public static Color InactiveCaption => KnownColorTableEx.GetSystemColor(KnownColor.InactiveCaption);
        public static Color InactiveCaptionText => KnownColorTableEx.GetSystemColor(KnownColor.InactiveCaptionText);
        public static Color Info => KnownColorTableEx.GetSystemColor(KnownColor.Info);
        public static Color InfoText => KnownColorTableEx.GetSystemColor(KnownColor.InfoText);

        public static Color Menu => KnownColorTableEx.GetSystemColor(KnownColor.Menu);
        public static Color MenuBar => KnownColorTableEx.GetSystemColor(KnownColor.MenuBar);
        public static Color MenuHighlight => KnownColorTableEx.GetSystemColor(KnownColor.MenuHighlight);
        public static Color MenuText => KnownColorTableEx.GetSystemColor(KnownColor.MenuText);

        public static Color ScrollBar => KnownColorTableEx.GetSystemColor(KnownColor.ScrollBar);

        public static Color Window => KnownColorTableEx.GetSystemColor(KnownColor.Window);
        public static Color WindowFrame => KnownColorTableEx.GetSystemColor(KnownColor.WindowFrame);
        public static Color WindowText => KnownColorTableEx.GetSystemColor(KnownColor.WindowText);

        /// <summary>
        /// When <see langword="true"/>, system <see cref="KnownColor"/> values will return
        /// the alternative color set (as returned by <see cref="SystemColors"/> statics or
        /// <see cref="Color.FromKnownColor(KnownColor)"/>). This is currently "dark mode"
        /// variants of the system colors.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="KnownColor"/> <see cref="Color"/> values are always looked up every
        /// time you use them and do not retain any other context. As such, existing
        /// <see cref="Color"/> values will change when this property is set.
        /// </para>
        /// <para>
        /// On Windows, system <see cref="KnownColor"/> values will always return the current
        /// Windows color when the OS has a high contrast theme enabled.
        /// </para>
        /// </remarks>
        //[Experimental(Experimentals.SystemColorsDiagId, UrlFormat = Experimentals.SharedUrlFormat)]
        public static bool UseAlternativeColorSet
        {
            get => s_useAlternativeColorSet;
            set => s_useAlternativeColorSet = value;
        }
    }

    /// <summary>
    /// source: <a href="https://github.com/dotnet/winforms">dotnet/winforms</a>. (.NET Foundation, MIT license)<br/>
    /// <c>src/System/Drawing/SystemBrushesEx.cs</c><br/>
    /// </summary>
    public static class SystemBrushesEx
    {
        private static readonly object s_systemBrushesKey = new();

        private static readonly Dictionary<Color, Brush> cache = new();

        public static Brush ActiveBorder => FromSystemColor(SystemColorsEx.ActiveBorder);
        public static Brush ActiveCaption => FromSystemColor(SystemColorsEx.ActiveCaption);
        public static Brush ActiveCaptionText => FromSystemColor(SystemColorsEx.ActiveCaptionText);
        public static Brush AppWorkspace => FromSystemColor(SystemColorsEx.AppWorkspace);

        public static Brush ButtonFace => FromSystemColor(SystemColorsEx.ButtonFace);
        public static Brush ButtonHighlight => FromSystemColor(SystemColorsEx.ButtonHighlight);
        public static Brush ButtonShadow => FromSystemColor(SystemColorsEx.ButtonShadow);

        public static Brush Control => FromSystemColor(SystemColorsEx.Control);
        public static Brush ControlLightLight => FromSystemColor(SystemColorsEx.ControlLightLight);
        public static Brush ControlLight => FromSystemColor(SystemColorsEx.ControlLight);
        public static Brush ControlDark => FromSystemColor(SystemColorsEx.ControlDark);
        public static Brush ControlDarkDark => FromSystemColor(SystemColorsEx.ControlDarkDark);
        public static Brush ControlText => FromSystemColor(SystemColorsEx.ControlText);

        public static Brush Desktop => FromSystemColor(SystemColorsEx.Desktop);

        public static Brush GradientActiveCaption => FromSystemColor(SystemColorsEx.GradientActiveCaption);
        public static Brush GradientInactiveCaption => FromSystemColor(SystemColorsEx.GradientInactiveCaption);
        public static Brush GrayText => FromSystemColor(SystemColorsEx.GrayText);

        public static Brush Highlight => FromSystemColor(SystemColorsEx.Highlight);
        public static Brush HighlightText => FromSystemColor(SystemColorsEx.HighlightText);
        public static Brush HotTrack => FromSystemColor(SystemColorsEx.HotTrack);

        public static Brush InactiveCaption => FromSystemColor(SystemColorsEx.InactiveCaption);
        public static Brush InactiveBorder => FromSystemColor(SystemColorsEx.InactiveBorder);
        public static Brush InactiveCaptionText => FromSystemColor(SystemColorsEx.InactiveCaptionText);
        public static Brush Info => FromSystemColor(SystemColorsEx.Info);
        public static Brush InfoText => FromSystemColor(SystemColorsEx.InfoText);

        public static Brush Menu => FromSystemColor(SystemColorsEx.Menu);
        public static Brush MenuBar => FromSystemColor(SystemColorsEx.MenuBar);
        public static Brush MenuHighlight => FromSystemColor(SystemColorsEx.MenuHighlight);
        public static Brush MenuText => FromSystemColor(SystemColorsEx.MenuText);

        public static Brush ScrollBar => FromSystemColor(SystemColorsEx.ScrollBar);

        public static Brush Window => FromSystemColor(SystemColorsEx.Window);
        public static Brush WindowFrame => FromSystemColor(SystemColorsEx.WindowFrame);
        public static Brush WindowText => FromSystemColor(SystemColorsEx.WindowText);

        public static Brush FromSystemColor(Color c)
        {
            if (!cache.TryGetValue(c, out var brush))
            {
                brush = new SolidBrush(c);
                cache[c] = brush;
            }
            return brush;
            //return new SolidBrush(c);
            //if (!c.IsSystemColor)
            //{
            //    throw new ArgumentException(SR.Format(SR.ColorNotSystemColor, c.ToString()));
            //}

            //if (!Gdip.ThreadData.TryGetValue(s_systemBrushesKey, out object? tempSystemBrushes) || tempSystemBrushes is not Brush[] systemBrushes)
            //{
            //    systemBrushes = new Brush[(int)KnownColor.WindowText + (int)KnownColor.MenuHighlight - (int)KnownColor.YellowGreen];
            //    Gdip.ThreadData[s_systemBrushesKey] = systemBrushes;
            //}

            //int idx = (int)c.ToKnownColor();
            //if (idx > (int)KnownColor.YellowGreen)
            //{
            //    idx -= (int)KnownColor.YellowGreen - (int)KnownColor.WindowText;
            //}

            //idx--;

            //Debug.Assert(idx >= 0 && idx < SystemBrushesEx.Length, "System colors have been added but our system color array has not been expanded.");

            //return systemBrushes[idx] ??= new SolidBrush(c, true);
        }

        public static void Reset()
        {
            foreach (var brush in cache.Values)
                brush.Dispose();
            cache.Clear();
        }
    }

    /// <summary>
    /// source: <a href="https://github.com/dotnet/winforms">dotnet/winforms</a>. (.NET Foundation, MIT license)<br/>
    /// <c>src/System/Drawing/SystemPensEx.cs</c><br/>
    /// </summary>
    public static class SystemPensEx
    {
        private static readonly object s_systemPensKey = new();

        private static readonly Dictionary<Color, Pen> cache = new();

        public static Pen ActiveBorder => FromSystemColor(SystemColorsEx.ActiveBorder);
        public static Pen ActiveCaption => FromSystemColor(SystemColorsEx.ActiveCaption);
        public static Pen ActiveCaptionText => FromSystemColor(SystemColorsEx.ActiveCaptionText);
        public static Pen AppWorkspace => FromSystemColor(SystemColorsEx.AppWorkspace);

        public static Pen ButtonFace => FromSystemColor(SystemColorsEx.ButtonFace);
        public static Pen ButtonHighlight => FromSystemColor(SystemColorsEx.ButtonHighlight);

        public static Pen ButtonShadow => FromSystemColor(SystemColorsEx.ButtonShadow);

        public static Pen Control => FromSystemColor(SystemColorsEx.Control);
        public static Pen ControlText => FromSystemColor(SystemColorsEx.ControlText);
        public static Pen ControlDark => FromSystemColor(SystemColorsEx.ControlDark);
        public static Pen ControlDarkDark => FromSystemColor(SystemColorsEx.ControlDarkDark);
        public static Pen ControlLight => FromSystemColor(SystemColorsEx.ControlLight);
        public static Pen ControlLightLight => FromSystemColor(SystemColorsEx.ControlLightLight);

        public static Pen Desktop => FromSystemColor(SystemColorsEx.Desktop);

        public static Pen GradientActiveCaption => FromSystemColor(SystemColorsEx.GradientActiveCaption);
        public static Pen GradientInactiveCaption => FromSystemColor(SystemColorsEx.GradientInactiveCaption);
        public static Pen GrayText => FromSystemColor(SystemColorsEx.GrayText);

        public static Pen Highlight => FromSystemColor(SystemColorsEx.Highlight);
        public static Pen HighlightText => FromSystemColor(SystemColorsEx.HighlightText);
        public static Pen HotTrack => FromSystemColor(SystemColorsEx.HotTrack);

        public static Pen InactiveBorder => FromSystemColor(SystemColorsEx.InactiveBorder);
        public static Pen InactiveCaption => FromSystemColor(SystemColorsEx.InactiveCaption);
        public static Pen InactiveCaptionText => FromSystemColor(SystemColorsEx.InactiveCaptionText);
        public static Pen Info => FromSystemColor(SystemColorsEx.Info);
        public static Pen InfoText => FromSystemColor(SystemColorsEx.InfoText);

        public static Pen Menu => FromSystemColor(SystemColorsEx.Menu);
        public static Pen MenuBar => FromSystemColor(SystemColorsEx.MenuBar);
        public static Pen MenuHighlight => FromSystemColor(SystemColorsEx.MenuHighlight);
        public static Pen MenuText => FromSystemColor(SystemColorsEx.MenuText);

        public static Pen ScrollBar => FromSystemColor(SystemColorsEx.ScrollBar);

        public static Pen Window => FromSystemColor(SystemColorsEx.Window);
        public static Pen WindowFrame => FromSystemColor(SystemColorsEx.WindowFrame);
        public static Pen WindowText => FromSystemColor(SystemColorsEx.WindowText);

        public static Pen FromSystemColor(Color c)
        {
            if (!cache.TryGetValue(c, out var pen))
            {
                pen = new Pen(c);
                cache[c] = pen;
            }
            return pen;
            //return new Pen(c);
            //if (!c.IsSystemColor)
            //{
            //    throw new ArgumentException(SR.Format(SR.ColorNotSystemColor, c.ToString()));
            //}

            //if (!Gdip.ThreadData.TryGetValue(s_systemPensKey, out object? tempSystemPens) || tempSystemPens is not Pen[] systemPens)
            //{
            //    systemPens = new Pen[(int)KnownColor.WindowText + (int)KnownColor.MenuHighlight - (int)KnownColor.YellowGreen];
            //    Gdip.ThreadData[s_systemPensKey] = systemPens;
            //}

            //int idx = (int)c.ToKnownColor();
            //if (idx > (int)KnownColor.YellowGreen)
            //{
            //    idx -= (int)KnownColor.YellowGreen - (int)KnownColor.WindowText;
            //}

            //idx--;
            //Debug.Assert(idx >= 0 && idx < SystemPensEx.Length, "System colors have been added but our system color array has not been expanded.");

            //return systemPens[idx] ??= new Pen(c, true);
        }

        public static void Reset()
        {
            foreach (var pen in cache.Values)
                pen.Dispose();
            cache.Clear();
        }
    }
}
