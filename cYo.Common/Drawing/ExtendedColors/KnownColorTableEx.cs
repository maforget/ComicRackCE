using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace cYo.Common.Drawing.ExtendedColors;

public class KnownColorTableEx
{
    #region NET_10 KnownColorTable
    // These values were based on manual investigation of dark mode themes in the
    // Win32 Common Controls and WinUI. There aren't direct mappings published by
    // Windows, these may change slightly when this feature is finalized to make
    // sure we have the best experience in hybrid dark mode scenarios (mixing
    // WPF, WinForms, and WinUI).

    /// <summary>
    /// source: <a href="https://github.com/dotnet/runtime">dotnet/runtime</a>. (.NET Foundation, MIT license)<br/>
    /// <c>src/libraries/System.Drawing.Primitives/src/System/Drawing/KnownColorTable.cs</c><br/>
    /// source refers to Dark Theme colors as <c>AlternateSystemColors</c>; this has been kept the same.
    /// </summary>
    /// <remarks>
    /// <para>These are defaults and could do with checking/tweaking.</para>
    /// <para>
    /// <see cref="SystemColors.HighlightText"/> is set to <see cref="SystemColors.ControlText"/> elsewhere:<br/>
    /// <a href="https://github.com/dotnet/winforms/blob/0a52f06318e5176b427ae353a416e19635531fc6/src/System.Windows.Forms/System/Windows/Forms/Controls/PropertyGrid/PropertyGrid.cs#L78">Application.IsDarkModeEnabled ? SystemColors.ControlText : SystemColors.HighlightText;</a>.<br/>
    /// Setting here until we have a reason not to, instead of creating a <c>PropertyGrid</c> class and having to re-write <see cref="System.Windows.Forms.Control"/>.
    /// </para>
    /// </remarks>
    private static ReadOnlySpan<uint> AlternateSystemColors =>
    [
        0,          // To align with KnownColor.ActiveBorder = 1

                    // Existing   New
        0xFF464646, // FFB4B4B4 - FF464646: ActiveBorder - Dark gray
        0xFF3C5F78, // FF99B4D1 - FF3C5F78: ActiveCaption - Highlighted Text Background
        0xFFFFFFFF, // FF000000 - FFBEBEBE: ActiveCaptionText - White
        0xFF3C3C3C, // FFABABAB - FF3C3C3C: AppWorkspace - Panel Background
        0xFF202020, // FFF0F0F0 - FF373737: Control - Normal Panel/Windows Background
        0xFF4A4A4A, // FFA0A0A0 - FF464646: ControlDark - A lighter gray for dark mode
        0xFF5A5A5A, // FF696969 - FF5A5A5A: ControlDarkDark - An even lighter gray for dark mode
        0xFF2E2E2E, // FFE3E3E3 - FF2E2E2E: ControlLight - Unfocused Textbox Background
        0xFF1F1F1F, // FFFFFFFF - FF1F1F1F: ControlLightLight - Focused Textbox Background
        0xFFFFFFFF, // FF000000 - FFFFFFFF: ControlText - Control Forecolor and Text Color
        0xFF101010, // FF000000 - FF101010: Desktop - Black
        0xFF969696, // FF6D6D6D - FF969696: GrayText - Prompt Text Focused TextBox
        0xFF2864B4, // FF0078D7 - FF2864B4: Highlight - Highlighted Panel in DarkMode
        //0xFF000000, // FFFFFFFF - FF000000: HighlightText - White
        0xFFFFFFFF, // see remarks above.
        0xFF2D5FAF, // FF0066CC - FF2D5FAF: HotTrack - Background of the ToggleSwitch
        0xFF3C3F41, // FFF4F7FC - FF3C3F41: InactiveBorder - Dark gray
        0xFF374B5A, // FFBFCBDD - FF374B5A: InactiveCaption - Highlighted Panel in DarkMode
        0xFFBEBEBE, // FF000000 - FFBEBEBE: InactiveCaptionText - Middle Dark Panel
        0xFF50503C, // FFFFFFE1 - FF50503C: Info - Link Label
        0xFFBEBEBE, // FF000000 - FFBEBEBE: InfoText - Prompt Text Color
        0xFF373737, // FFF0F0F0 - FF373737: Menu - Normal Menu Background
        0xFFF0F0F0, // FF000000 - FFF0F0F0: MenuText - White
        0xFF505050, // FFC8C8C8 - FF505050: ScrollBar - Scrollbars and Scrollbar Arrows
        0xFF323232, // FFFFFFFF - FF323232: Window - Window Background
        0xFF282828, // FF646464 - FF282828: WindowFrame - White
        0xFFF0F0F0, // FF000000 - FFF0F0F0: WindowText - White
        0xFF202020, // FFF0F0F0 - FF373737: ButtonFace - Same as Window Background
        0xFF101010, // FFFFFFFF - FF101010: ButtonHighlight - White
        0xFF464646, // FFA0A0A0 - FF464646: ButtonShadow - Same as Scrollbar Elements
        0XFF416482, // FFB9D1EA - FF416482: GradientActiveCaption - Same as Highlighted Text Background
        0xFF557396, // FFD7E4F2 - FF557396: GradientInactiveCaption - Same as Highlighted Panel in DarkMode
        0xFF373737, // FFF0F0F0 - FF373737: MenuBar - Same as Normal Menu Background
        0xFF2A80D2  // FF3399FF - FF2A80D2: MenuHighlight - Same as Highlighted Menu Background
    ];

    private static uint GetAlternateSystemColorArgb(KnownColor color)
    {
        // Shift the original (split) index to fit the alternate color map.
        int index = color <= KnownColor.WindowText
            ? (int)color
            : (int)color - (int)KnownColor.ButtonFace + (int)KnownColor.WindowText + 1;

        try
        {
            return AlternateSystemColors[index];
        }
        catch
        {
            return 0xFFFF0000;
        }
    }
    #endregion


    public static event Action SystemColorsChanging;
    public static event Action SystemColorsChanged;

    #region system data
    private readonly FieldInfo _colorTableField;

    private readonly PropertyInfo _threadDataProperty;

    private IDictionary ThreadData => (IDictionary)_threadDataProperty.GetValue(null, null);

    private object SystemBrushesKey { get; }

    private object SystemPensKey { get; }

    private int[] OriginalColors { get; }
    private IReadOnlyDictionary<int, int> KnownOriginalColors { get; }

    private int[] _colorTable;

    #endregion


    #region public

    public KnownColorTableEx()
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

    public void Initialize(bool useAlternateColors = false)
    {
        SystemColorsEx.UseAlternativeColorSet = useAlternateColors;
        if (!useAlternateColors) return;

        //KnownColors.ForEach(color => setColor(color, Color.Red.ToArgb())); // For testing
        //KnownColors.ForEach(color => setColor(color, GetAlternateSystemColorArgb(color))); // Old behavior of replacing SystemColors (disable global using and enable this to revert)

        //foreach (KnownColor color in Enum.GetValues(typeof(KnownColor)))
        //{
        //    setColor(color, unchecked((int)GetAlternateSystemColorArgb(color)));
        //}
        refreshThreadData();
    }

    public void SetColor(KnownColor knownColor, int argb)
    {
        setColor(knownColor, argb);
        refreshThreadData();
    }

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

        refreshThreadData();
    }

    public void Reset(KnownColor color) => SetColor(color, OriginalColors[(int)color]);

    public void ResetAll() => Load(KnownOriginalColors);

    public readonly HashSet<KnownColor> KnownColors = new(
        new[]
        {
            SystemColors.Window,             // most backgrounds and gradient start (Toolstrip, menu)
            SystemColors.WindowText,         // most text boxes - list, combo etc
            SystemColors.GrayText,           // disabled menu text (expect should be other controls too)
            SystemColors.Highlight,          // all the highlights (except combobox): toolstrip, selected item, active menu item

            SystemColors.ButtonFace,         // main menu bar, toolstrip gradient
            SystemColors.ButtonShadow,       // menu border. Lines - menu dividers, toolstrip dividers
            SystemColors.ButtonHighlight,    // MainForm bottom line, toolstrip dividers, Re-size grip (bottom right)

            SystemColors.ControlLightLight,  // active tab label background ... but also button highlight
            SystemColors.ControlLight,       // grip highlight, remaining tasks text
            SystemColors.Control,            // Form background (not tabs or menu). gradient end. inactive tab label
            SystemColors.ControlText,        // most read-only text. active menu item. menu arrows
            SystemColors.ControlDark,        // inner active button shadow. selected pref menu buttom (checkbox?) outline
            SystemColors.ControlDarkDark,    // Outer button shadow

            SystemColors.MenuText,           // inactive (not-selected) menu text

            SystemColors.Info,               // hover-over tooltip background
            SystemColors.InfoText,           // hover-over tooltip text

            SystemColors.WindowFrame,        // hover-over tooltip border (probably others but not observed)

            SystemColors.AppWorkspace,       // 1px border around tab|panel|whatever-the-element-is. doubled for open comic as it's a seperate element.

            SystemColors.HighlightText,     // selected ComboBox text color in some cases (probably related to DropDownList etc setting)

            // these colors were not observed
            // might be due to OS settings, not interacting with a UI element that uses them, or they are genuinely not used
            SystemColors.Desktop,
            SystemColors.ScrollBar,         // I think this is intended as a joke

            SystemColors.HotTrack,

            SystemColors.ActiveBorder,
            SystemColors.ActiveCaption,
            SystemColors.ActiveCaptionText,
            SystemColors.GradientActiveCaption,

            SystemColors.InactiveBorder,
            SystemColors.InactiveCaption,
            SystemColors.InactiveCaptionText,
            SystemColors.GradientInactiveCaption,

            SystemColors.Menu,
            SystemColors.MenuBar,
            SystemColors.MenuHighlight
        }.Select(_ => _.ToKnownColor())
    );

    public static uint GetSystemColorArgb(KnownColor color)
    {
        //Debug.Assert(Color.IsKnownColorSystem(color));
        return (!SystemColorsEx.s_useAlternativeColorSet)
            ? (uint)Color.FromKnownColor(color).ToArgb() //ColorValueTable[(int)color]
            : GetAlternateSystemColorArgb(color);
    }
    #endregion

    public static Color GetSystemColor(KnownColor color)
    {
        return Color.FromArgb(unchecked((int)GetSystemColorArgb(color)));
    }


    #region private
    private void setColor(KnownColor knownColor, uint argb) => _colorTable[(int)knownColor] = unchecked((int)argb);
    private void setColor(KnownColor knownColor, int argb) => _colorTable[(int)knownColor] = argb;

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

    private void refreshThreadData()
    {
        if (SystemBrushesKey != null)
            ThreadData[SystemBrushesKey] = null;

        if (SystemPensKey != null)
            ThreadData[SystemPensKey] = null;

        fireColorsChangedEvents();
    }
    #endregion

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