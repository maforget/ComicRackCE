using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Drawing.ExtendedColors;

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
