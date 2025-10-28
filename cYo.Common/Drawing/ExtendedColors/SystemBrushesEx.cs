using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Drawing.ExtendedColors;

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
