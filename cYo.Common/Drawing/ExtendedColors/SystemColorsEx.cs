using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Drawing.ExtendedColors;

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
