using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Drawing.ExtendedColors;

/// <summary>
/// source: <a href="https://github.com/dotnet/winforms">dotnet/winforms</a>. (.NET Foundation, MIT license)<br/>
/// <c>src/System.Windows.Forms/System/Windows/Forms/Controls/ToolStrips/ProfessionalColorTable.KnownColors.cs</c><br/>
/// <c>src/System.Windows.Forms/System/Windows/Forms/Controls/ToolStrips/ProfessionalColorTable.cs</c><br/>
/// </summary>
/// <remarks>
/// <para>Uses <see cref="SystemColorsEx"/> as a base so that derived colors respect <see cref="ThemeExtensions.IsDarkModeEnabled"/>.</para>
/// <para>Heavily trimmed.</para>
/// </remarks>
public partial class ProfessionalColorTableEx : ProfessionalColorTable
{

    private Dictionary<KnownColors, Color>? _professionalRGB;
    private bool _usingSystemColors;
    //private bool _useSystemColors;
    private string? _lastKnownColorScheme = string.Empty;

    private const string OliveColorScheme = "HomeStead";
    private const string NormalColorScheme = "NormalColor";
    private const string SilverColorScheme = "Metallic";
    private const string RoyaleColorScheme = "Royale";  // sometimes returns NormalColor, sometimes returns Royale.

    private const string LunaFileName = "luna.msstyles";
    private const string RoyaleFileName = "royale.msstyles";
    private const string AeroFileName = "aero.msstyles";

    private object? _colorFreshnessKey;

    public ProfessionalColorTableEx()
    {
        base.UseSystemColors = true;
    }

    private Dictionary<KnownColors, Color> ColorTable
    {
        get
        {
            if (UseSystemColors)
            {
                // someone has turned off theme support for the color table.
                if (!_usingSystemColors || _professionalRGB is null)
                {
                    _professionalRGB ??= new Dictionary<KnownColors, Color>((int)KnownColors.lastKnownColor);
                    InitSystemColors(ref _professionalRGB);
                }
            }
            //else if (ToolStripManager.VisualStylesEnabled)
            //{
            //    // themes are on and enabled in the manager
            //    if (_usingSystemColors || _professionalRGB is null)
            //    {
            //        _professionalRGB ??= new Dictionary<KnownColors, Color>((int)KnownColors.lastKnownColor);
            //        InitThemedColors(ref _professionalRGB);
            //    }
            //}
            else
            {
                // themes are off.
                if (!_usingSystemColors || _professionalRGB is null)
                {
                    _professionalRGB ??= new Dictionary<KnownColors, Color>((int)KnownColors.lastKnownColor);
                    InitSystemColors(ref _professionalRGB);
                }
            }

            return _professionalRGB;
        }
    }

    /// <summary>
    ///  When this is specified, professional colors picks from SystemColors
    ///  rather than colors that match the current theme. If theming is not
    ///  turned on, we'll fall back to SystemColorsEx.
    /// </summary>
    //public bool UseSystemColors
    //{
    //    get => _useSystemColors;
    //    set
    //    {
    //        if (_useSystemColors == value)
    //        {
    //            return;
    //        }

    //        _useSystemColors = value;
    //        ResetRGBTable();
    //    }
    //}

    private Color FromKnownColor(KnownColors color)
    {
        //if (ProfessionalColors.ColorFreshnessKey != _colorFreshnessKey || ProfessionalColors.ColorScheme != _lastKnownColorScheme)
        //{
        //    ResetRGBTable();
        //}

        //_colorFreshnessKey = ProfessionalColors.ColorFreshnessKey;
        //_lastKnownColorScheme = ProfessionalColors.ColorScheme;

        return ColorTable[color];
    }

    private void ResetRGBTable()
    {
        _professionalRGB?.Clear();
        _professionalRGB = null;
    }

    public override Color ButtonSelectedHighlight => FromKnownColor(KnownColors.ButtonSelectedHighlight);

    public override Color ButtonSelectedHighlightBorder => ButtonPressedBorder;

    public override Color ButtonPressedHighlight => FromKnownColor(KnownColors.ButtonPressedHighlight);

    public override Color ButtonPressedHighlightBorder => SystemColorsEx.Highlight;

    public override Color ButtonCheckedHighlight => FromKnownColor(KnownColors.ButtonCheckedHighlight);

    public override Color ButtonCheckedHighlightBorder => SystemColorsEx.Highlight;

    public override Color ButtonPressedBorder => FromKnownColor(KnownColors.msocbvcrCBCtlBdrMouseOver);

    public override Color ButtonSelectedBorder => FromKnownColor(KnownColors.msocbvcrCBCtlBdrMouseOver);

    public override Color ButtonCheckedGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradSelectedBegin);

    public override Color ButtonCheckedGradientMiddle => FromKnownColor(KnownColors.msocbvcrCBGradSelectedMiddle);

    public override Color ButtonCheckedGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradSelectedEnd);

    public override Color ButtonSelectedGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradMouseOverBegin);

    public override Color ButtonSelectedGradientMiddle => FromKnownColor(KnownColors.msocbvcrCBGradMouseOverMiddle);

    public override Color ButtonSelectedGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradMouseOverEnd);

    public override Color ButtonPressedGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradMouseDownBegin);

    public override Color ButtonPressedGradientMiddle => FromKnownColor(KnownColors.msocbvcrCBGradMouseDownMiddle);

    public override Color ButtonPressedGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradMouseDownEnd);
    public override Color CheckBackground => FromKnownColor(KnownColors.msocbvcrCBCtlBkgdSelected);

    public override Color CheckSelectedBackground => FromKnownColor(KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver);

    public override Color CheckPressedBackground => FromKnownColor(KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver);

    public override Color GripDark => FromKnownColor(KnownColors.msocbvcrCBDragHandle);

    public override Color GripLight => FromKnownColor(KnownColors.msocbvcrCBDragHandleShadow);

    public override Color ImageMarginGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradVertBegin);

    public override Color ImageMarginGradientMiddle => FromKnownColor(KnownColors.msocbvcrCBGradVertMiddle);

    public override Color ImageMarginGradientEnd => (_usingSystemColors) ? SystemColorsEx.Control : FromKnownColor(KnownColors.msocbvcrCBGradVertEnd);

    public override Color ImageMarginRevealedGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin);

    public override Color ImageMarginRevealedGradientMiddle => FromKnownColor(KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle);

    public override Color ImageMarginRevealedGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd);

    public override Color MenuStripGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradMainMenuHorzBegin);

    public override Color MenuStripGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradMainMenuHorzEnd);

    public override Color MenuItemSelected => FromKnownColor(KnownColors.msocbvcrCBCtlBkgdMouseOver);

    public override Color MenuItemBorder => FromKnownColor(KnownColors.msocbvcrCBCtlBdrSelected);

    public override Color MenuBorder => FromKnownColor(KnownColors.msocbvcrCBMenuBdrOuter);

    public override Color MenuItemSelectedGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradMouseOverBegin);

    public override Color MenuItemSelectedGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradMouseOverEnd);

    public override Color MenuItemPressedGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradMenuTitleBkgdBegin);

    public override Color MenuItemPressedGradientMiddle => FromKnownColor(KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle);

    public override Color MenuItemPressedGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradMenuTitleBkgdEnd);

    public override Color RaftingContainerGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradMainMenuHorzBegin);

    public override Color RaftingContainerGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradMainMenuHorzEnd);

    public override Color SeparatorDark => FromKnownColor(KnownColors.msocbvcrCBSplitterLine);

    public override Color SeparatorLight => FromKnownColor(KnownColors.msocbvcrCBSplitterLineLight);

    // Note: the color is retained for backwards compatibility
    public virtual Color StatusStripBorder => SystemColorsEx.ButtonHighlight;

    public override Color StatusStripGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradMainMenuHorzBegin);

    public override Color StatusStripGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradMainMenuHorzEnd);

    public override Color ToolStripBorder => FromKnownColor(KnownColors.msocbvcrCBShadow);

    public override Color ToolStripDropDownBackground => FromKnownColor(KnownColors.msocbvcrCBMenuBkgd);

    public override Color ToolStripGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradVertBegin);

    public override Color ToolStripGradientMiddle => FromKnownColor(KnownColors.msocbvcrCBGradVertMiddle);

    public override Color ToolStripGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradVertEnd);

    public override Color ToolStripContentPanelGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradMainMenuHorzBegin);

    public override Color ToolStripContentPanelGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradMainMenuHorzEnd);

    public override Color ToolStripPanelGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradMainMenuHorzBegin);

    public override Color ToolStripPanelGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradMainMenuHorzEnd);

    public override Color OverflowButtonGradientBegin => FromKnownColor(KnownColors.msocbvcrCBGradOptionsBegin);

    public override Color OverflowButtonGradientMiddle => FromKnownColor(KnownColors.msocbvcrCBGradOptionsMiddle);

    public override Color OverflowButtonGradientEnd => FromKnownColor(KnownColors.msocbvcrCBGradOptionsEnd);

    internal Color ComboBoxButtonGradientBegin => MenuItemPressedGradientBegin;
    internal Color ComboBoxButtonGradientEnd => MenuItemPressedGradientEnd;
    internal Color ComboBoxButtonSelectedGradientBegin => MenuItemSelectedGradientBegin;
    internal Color ComboBoxButtonSelectedGradientEnd => MenuItemSelectedGradientEnd;
    internal Color ComboBoxButtonPressedGradientBegin => ButtonPressedGradientBegin;
    internal Color ComboBoxButtonPressedGradientEnd => ButtonPressedGradientEnd;
    internal Color ComboBoxButtonOnOverflow => ToolStripDropDownBackground;
    internal Color ComboBoxBorder => ButtonSelectedHighlightBorder;
    internal Color TextBoxBorder => ButtonSelectedHighlightBorder;

    private static Color GetAlphaBlendedColor(Graphics g, Color src, Color dest, int alpha)
    {
        int red = (src.R * alpha + (255 - alpha) * dest.R) / 255;
        int green = (src.G * alpha + (255 - alpha) * dest.G) / 255;
        int blue = (src.B * alpha + (255 - alpha) * dest.B) / 255;
        int newAlpha = (src.A * alpha + (255 - alpha) * dest.A) / 255;

        return Color.FromArgb(newAlpha, red, green, blue);
        //if (g is null)
        //{
        //    return Color.FromArgb(newAlpha, red, green, blue);
        //}
        //else
        //{
        //    return g.FindNearestColor(Color.FromArgb(newAlpha, red, green, blue));
        //}
    }

    // this particular method gets us closer to office by increasing the resolution.

    private static Color GetAlphaBlendedColorHighRes(Graphics? graphics, Color src, Color dest, int alpha)
    {
        int sum;
        int nPart2;

        int nPart1 = alpha;
        if (nPart1 < 100)
        {
            nPart2 = 100 - nPart1;
            sum = 100;
        }
        else
        {
            nPart2 = 1000 - nPart1;
            sum = 1000;
        }

        // By adding on sum/2 before dividing by sum, we properly round the value,
        // rather than truncating it, while doing integer math.
        int r = (nPart1 * src.R + nPart2 * dest.R + sum / 2) / sum;
        int g = (nPart1 * src.G + nPart2 * dest.G + sum / 2) / sum;
        int b = (nPart1 * src.B + nPart2 * dest.B + sum / 2) / sum;

        return Color.FromArgb(r, g, b);

        //if (graphics is null)
        //{
        //    return Color.FromArgb(r, g, b);
        //}

        //return graphics.FindNearestColor(Color.FromArgb(r, g, b));
    }

    private static void InitCommonColors(ref Dictionary<KnownColors, Color> rgbTable)
    {
        // We need to calculate our own alpha blended color based on the Highlight and Window
        // colors on the system. Since terminalserver + alphablending doesn't work we can't just do a
        // FromARGB here. So we have a simple function which calculates the blending for us.

        //using var screen = GdiCache.GetScreenDCGraphics();
        rgbTable[KnownColors.ButtonPressedHighlight] = GetAlphaBlendedColor(
            null,
            SystemColorsEx.Window,
            GetAlphaBlendedColor(null, SystemColorsEx.Highlight, SystemColorsEx.Window, 160),
            50);
        rgbTable[KnownColors.ButtonCheckedHighlight] = GetAlphaBlendedColor(
            null,
            SystemColorsEx.Window,
            GetAlphaBlendedColor(null, SystemColorsEx.Highlight, SystemColorsEx.Window, 80),
            20);
        rgbTable[KnownColors.ButtonSelectedHighlight] = rgbTable[KnownColors.ButtonCheckedHighlight];

        //if (!DisplayInformation.LowResolution)
        //{
        //    using var screen = GdiCache.GetScreenDCGraphics();
        //    rgbTable[KnownColors.ButtonPressedHighlight] = GetAlphaBlendedColor(
        //        screen,
        //        SystemColorsEx.Window,
        //        GetAlphaBlendedColor(screen, SystemColorsEx.Highlight, SystemColorsEx.Window, 160),
        //        50);
        //    rgbTable[KnownColors.ButtonCheckedHighlight] = GetAlphaBlendedColor(
        //        screen,
        //        SystemColorsEx.Window,
        //        GetAlphaBlendedColor(screen, SystemColorsEx.Highlight, SystemColorsEx.Window, 80),
        //        20);
        //    rgbTable[KnownColors.ButtonSelectedHighlight] = rgbTable[KnownColors.ButtonCheckedHighlight];
        //}
        //else
        //{
        //    rgbTable[KnownColors.ButtonPressedHighlight] = SystemColorsEx.Highlight;
        //    rgbTable[KnownColors.ButtonCheckedHighlight] = SystemColorsEx.ControlLight;
        //    rgbTable[KnownColors.ButtonSelectedHighlight] = SystemColorsEx.ControlLight;
        //}
    }

    private void InitSystemColors(ref Dictionary<KnownColors, Color> rgbTable)
    {
        _usingSystemColors = true;

        InitCommonColors(ref rgbTable);

        // use locals so we aren't fetching again and again.
        Color buttonFace = SystemColorsEx.ButtonFace;
        Color buttonShadow = SystemColorsEx.ButtonShadow;
        Color highlight = SystemColorsEx.Highlight;
        Color window = SystemColorsEx.Window;
        Color empty = Color.Empty;
        Color controlText = SystemColorsEx.ControlText;
        Color buttonHighlight = SystemColorsEx.ButtonHighlight;
        Color grayText = SystemColorsEx.GrayText;
        Color highlightText = SystemColorsEx.HighlightText;
        Color windowText = SystemColorsEx.WindowText;

        // initialize to high contrast
        Color gradientBegin = buttonFace;
        Color gradientMiddle = buttonFace;
        Color gradientEnd = buttonFace;
        Color msocbvcrCBCtlBkgdMouseOver = highlight;
        Color msocbvcrCBCtlBkgdMouseDown = highlight;

        bool lowResolution = false; //bool lowResolution = DisplayInformation.LowResolution;
        bool highContrast = false; //bool highContrast = DisplayInformation.HighContrast;

        if (lowResolution)
        {
            msocbvcrCBCtlBkgdMouseOver = window;
        }
        else if (!highContrast)
        {
            gradientBegin = GetAlphaBlendedColorHighRes(null, buttonFace, window, 23);
            gradientMiddle = GetAlphaBlendedColorHighRes(null, buttonFace, window, 50);
            gradientEnd = SystemColorsEx.ButtonFace;

            msocbvcrCBCtlBkgdMouseOver = GetAlphaBlendedColorHighRes(null, highlight, window, 30);
            msocbvcrCBCtlBkgdMouseDown = GetAlphaBlendedColorHighRes(null, highlight, window, 50);
        }

        if (lowResolution || highContrast)
        {
            rgbTable[KnownColors.msocbvcrCBBkgd] = buttonFace;
            rgbTable[KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = SystemColorsEx.ControlLight;
            rgbTable[KnownColors.msocbvcrCBDragHandle] = controlText;
            rgbTable[KnownColors.msocbvcrCBGradMainMenuHorzEnd] = buttonFace;
            rgbTable[KnownColors.msocbvcrCBGradOptionsBegin] = buttonShadow;
            rgbTable[KnownColors.msocbvcrCBGradOptionsMiddle] = buttonShadow;
            rgbTable[KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = buttonShadow;
            rgbTable[KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = buttonShadow;
            rgbTable[KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = buttonShadow;
            rgbTable[KnownColors.msocbvcrCBMenuBdrOuter] = controlText;
            rgbTable[KnownColors.msocbvcrCBMenuBkgd] = window;
            rgbTable[KnownColors.msocbvcrCBSplitterLine] = buttonShadow;
        }
        else
        {
            rgbTable[KnownColors.msocbvcrCBBkgd] = GetAlphaBlendedColorHighRes(null, window, buttonFace, 165);
            rgbTable[KnownColors.msocbvcrCBCtlBkgdSelectedMouseOver] = GetAlphaBlendedColorHighRes(null, highlight, window, 50);
            rgbTable[KnownColors.msocbvcrCBDragHandle] = GetAlphaBlendedColorHighRes(null, buttonShadow, window, 75);
            rgbTable[KnownColors.msocbvcrCBGradMainMenuHorzEnd] = GetAlphaBlendedColorHighRes(null, buttonFace, window, 205);
            rgbTable[KnownColors.msocbvcrCBGradOptionsBegin] = GetAlphaBlendedColorHighRes(null, buttonFace, window, 70);
            rgbTable[KnownColors.msocbvcrCBGradOptionsMiddle] = GetAlphaBlendedColorHighRes(null, buttonFace, window, 90);
            rgbTable[KnownColors.msocbvcrCBGradMenuIconBkgdDroppedBegin] = GetAlphaBlendedColorHighRes(null, buttonFace, window, 40);
            rgbTable[KnownColors.msocbvcrCBGradMenuIconBkgdDroppedMiddle] = GetAlphaBlendedColorHighRes(null, buttonFace, window, 70);
            rgbTable[KnownColors.msocbvcrCBGradMenuIconBkgdDroppedEnd] = GetAlphaBlendedColorHighRes(null, buttonFace, window, 90);
            rgbTable[KnownColors.msocbvcrCBMenuBdrOuter] = GetAlphaBlendedColorHighRes(null, controlText, buttonShadow, 20);
            rgbTable[KnownColors.msocbvcrCBMenuBkgd] = GetAlphaBlendedColorHighRes(null, buttonFace, window, 143);
            rgbTable[KnownColors.msocbvcrCBSplitterLine] = GetAlphaBlendedColorHighRes(null, buttonShadow, window, 70);
        }

        rgbTable[KnownColors.msocbvcrCBCtlBkgdSelected] = (lowResolution) ? SystemColorsEx.ControlLight : highlight;

        rgbTable[KnownColors.msocbvcrCBBdrOuterDocked] = buttonFace;
        rgbTable[KnownColors.msocbvcrCBBdrOuterDocked] = buttonShadow;
        rgbTable[KnownColors.msocbvcrCBBdrOuterFloating] = buttonShadow;
        rgbTable[KnownColors.msocbvcrCBCtlBdrMouseDown] = highlight;

        rgbTable[KnownColors.msocbvcrCBCtlBdrMouseOver] = highlight;
        rgbTable[KnownColors.msocbvcrCBCtlBdrSelected] = highlight;
        rgbTable[KnownColors.msocbvcrCBCtlBdrSelectedMouseOver] = highlight;
        rgbTable[KnownColors.msocbvcrCBCtlBkgd] = empty;
        rgbTable[KnownColors.msocbvcrCBCtlBkgdLight] = window;
        rgbTable[KnownColors.msocbvcrCBCtlBkgdMouseDown] = highlight;
        rgbTable[KnownColors.msocbvcrCBCtlBkgdMouseOver] = window;
        rgbTable[KnownColors.msocbvcrCBCtlText] = controlText;
        rgbTable[KnownColors.msocbvcrCBCtlTextDisabled] = buttonShadow;
        rgbTable[KnownColors.msocbvcrCBCtlTextLight] = grayText;
        rgbTable[KnownColors.msocbvcrCBCtlTextMouseDown] = highlightText;
        rgbTable[KnownColors.msocbvcrCBCtlTextMouseOver] = windowText;
        rgbTable[KnownColors.msocbvcrCBDockSeparatorLine] = empty;

        rgbTable[KnownColors.msocbvcrCBDragHandleShadow] = window;
        rgbTable[KnownColors.msocbvcrCBDropDownArrow] = empty;

        rgbTable[KnownColors.msocbvcrCBGradMainMenuHorzBegin] = buttonFace;

        rgbTable[KnownColors.msocbvcrCBGradMouseOverEnd] = msocbvcrCBCtlBkgdMouseOver;
        rgbTable[KnownColors.msocbvcrCBGradMouseOverBegin] = msocbvcrCBCtlBkgdMouseOver;
        rgbTable[KnownColors.msocbvcrCBGradMouseOverMiddle] = msocbvcrCBCtlBkgdMouseOver;

        rgbTable[KnownColors.msocbvcrCBGradOptionsEnd] = buttonShadow;
        rgbTable[KnownColors.msocbvcrCBGradOptionsMouseOverBegin] = empty;
        rgbTable[KnownColors.msocbvcrCBGradOptionsMouseOverEnd] = empty;
        rgbTable[KnownColors.msocbvcrCBGradOptionsMouseOverMiddle] = empty;
        rgbTable[KnownColors.msocbvcrCBGradOptionsSelectedBegin] = empty;
        rgbTable[KnownColors.msocbvcrCBGradOptionsSelectedEnd] = empty;
        rgbTable[KnownColors.msocbvcrCBGradOptionsSelectedMiddle] = empty;
        rgbTable[KnownColors.msocbvcrCBGradSelectedBegin] = empty;
        rgbTable[KnownColors.msocbvcrCBGradSelectedEnd] = empty;
        rgbTable[KnownColors.msocbvcrCBGradSelectedMiddle] = empty;

        rgbTable[KnownColors.msocbvcrCBGradVertBegin] = gradientBegin;
        rgbTable[KnownColors.msocbvcrCBGradVertMiddle] = gradientMiddle;
        rgbTable[KnownColors.msocbvcrCBGradVertEnd] = gradientEnd;

        rgbTable[KnownColors.msocbvcrCBGradMouseDownBegin] = msocbvcrCBCtlBkgdMouseDown;
        rgbTable[KnownColors.msocbvcrCBGradMouseDownMiddle] = msocbvcrCBCtlBkgdMouseDown;
        rgbTable[KnownColors.msocbvcrCBGradMouseDownEnd] = msocbvcrCBCtlBkgdMouseDown;

        rgbTable[KnownColors.msocbvcrCBGradMenuTitleBkgdBegin] = gradientBegin;
        rgbTable[KnownColors.msocbvcrCBGradMenuTitleBkgdEnd] = gradientMiddle;

        rgbTable[KnownColors.msocbvcrCBIconDisabledDark] = buttonShadow;
        rgbTable[KnownColors.msocbvcrCBIconDisabledLight] = buttonFace;
        rgbTable[KnownColors.msocbvcrCBLabelBkgnd] = buttonShadow;
        rgbTable[KnownColors.msocbvcrCBLowColorIconDisabled] = empty;
        rgbTable[KnownColors.msocbvcrCBMainMenuBkgd] = buttonFace;

        rgbTable[KnownColors.msocbvcrCBMenuCtlText] = windowText;
        rgbTable[KnownColors.msocbvcrCBMenuCtlTextDisabled] = grayText;
        rgbTable[KnownColors.msocbvcrCBMenuIconBkgd] = empty;
        rgbTable[KnownColors.msocbvcrCBMenuIconBkgdDropped] = buttonShadow;
        rgbTable[KnownColors.msocbvcrCBMenuShadow] = empty;
        rgbTable[KnownColors.msocbvcrCBMenuSplitArrow] = buttonShadow;
        rgbTable[KnownColors.msocbvcrCBOptionsButtonShadow] = empty;

        rgbTable[KnownColors.msocbvcrCBShadow] = rgbTable[KnownColors.msocbvcrCBBkgd];

        rgbTable[KnownColors.msocbvcrCBSplitterLineLight] = buttonHighlight;
        rgbTable[KnownColors.msocbvcrCBTearOffHandle] = empty;
        rgbTable[KnownColors.msocbvcrCBTearOffHandleMouseOver] = empty;
        rgbTable[KnownColors.msocbvcrCBTitleBkgd] = buttonShadow;
        rgbTable[KnownColors.msocbvcrCBTitleText] = buttonHighlight;
        rgbTable[KnownColors.msocbvcrDisabledFocuslessHighlightedText] = grayText;
        rgbTable[KnownColors.msocbvcrDisabledHighlightedText] = grayText;
        rgbTable[KnownColors.msocbvcrDlgGroupBoxText] = controlText;
        rgbTable[KnownColors.msocbvcrDocTabBdr] = buttonShadow;
        rgbTable[KnownColors.msocbvcrDocTabBdrDark] = buttonFace;
        rgbTable[KnownColors.msocbvcrDocTabBdrDarkMouseDown] = highlight;
        rgbTable[KnownColors.msocbvcrDocTabBdrDarkMouseOver] = SystemColorsEx.MenuText;
        rgbTable[KnownColors.msocbvcrDocTabBdrLight] = buttonFace;
        rgbTable[KnownColors.msocbvcrDocTabBdrLightMouseDown] = highlight;
        rgbTable[KnownColors.msocbvcrDocTabBdrLightMouseOver] = SystemColorsEx.MenuText;
        rgbTable[KnownColors.msocbvcrDocTabBdrMouseDown] = highlight;
        rgbTable[KnownColors.msocbvcrDocTabBdrMouseOver] = SystemColorsEx.MenuText;
        rgbTable[KnownColors.msocbvcrDocTabBdrSelected] = buttonShadow;
        rgbTable[KnownColors.msocbvcrDocTabBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrDocTabBkgdMouseDown] = highlight;
        rgbTable[KnownColors.msocbvcrDocTabBkgdMouseOver] = highlight;
        rgbTable[KnownColors.msocbvcrDocTabBkgdSelected] = window;
        rgbTable[KnownColors.msocbvcrDocTabText] = controlText;
        rgbTable[KnownColors.msocbvcrDocTabTextMouseDown] = highlightText;
        rgbTable[KnownColors.msocbvcrDocTabTextMouseOver] = highlight;
        rgbTable[KnownColors.msocbvcrDocTabTextSelected] = windowText;
        rgbTable[KnownColors.msocbvcrDWActiveTabBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrDWActiveTabBkgd] = buttonShadow;
        rgbTable[KnownColors.msocbvcrDWActiveTabText] = buttonFace;
        rgbTable[KnownColors.msocbvcrDWActiveTabText] = controlText;
        rgbTable[KnownColors.msocbvcrDWActiveTabTextDisabled] = buttonShadow;
        rgbTable[KnownColors.msocbvcrDWActiveTabTextDisabled] = controlText;
        rgbTable[KnownColors.msocbvcrDWInactiveTabBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrDWInactiveTabBkgd] = buttonShadow;
        rgbTable[KnownColors.msocbvcrDWInactiveTabText] = buttonHighlight;
        rgbTable[KnownColors.msocbvcrDWInactiveTabText] = controlText;
        rgbTable[KnownColors.msocbvcrDWTabBkgdMouseDown] = buttonFace;
        rgbTable[KnownColors.msocbvcrDWTabBkgdMouseOver] = buttonFace;
        rgbTable[KnownColors.msocbvcrDWTabTextMouseDown] = controlText;
        rgbTable[KnownColors.msocbvcrDWTabTextMouseOver] = controlText;
        rgbTable[KnownColors.msocbvcrFocuslessHighlightedBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrFocuslessHighlightedBkgd] = SystemColorsEx.InactiveCaption;
        rgbTable[KnownColors.msocbvcrFocuslessHighlightedText] = controlText;
        rgbTable[KnownColors.msocbvcrFocuslessHighlightedText] = SystemColorsEx.InactiveCaptionText;
        rgbTable[KnownColors.msocbvcrGDHeaderBdr] = highlight;
        rgbTable[KnownColors.msocbvcrGDHeaderBkgd] = window;
        rgbTable[KnownColors.msocbvcrGDHeaderCellBdr] = buttonShadow;
        rgbTable[KnownColors.msocbvcrGDHeaderCellBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrGDHeaderCellBkgdSelected] = empty;
        rgbTable[KnownColors.msocbvcrGDHeaderSeeThroughSelection] = highlight;
        rgbTable[KnownColors.msocbvcrGSPDarkBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrGSPDarkBkgd] = window;
        rgbTable[KnownColors.msocbvcrGSPGroupContentDarkBkgd] = window;
        rgbTable[KnownColors.msocbvcrGSPGroupContentLightBkgd] = window;
        rgbTable[KnownColors.msocbvcrGSPGroupContentText] = windowText;
        rgbTable[KnownColors.msocbvcrGSPGroupContentTextDisabled] = grayText;
        rgbTable[KnownColors.msocbvcrGSPGroupHeaderDarkBkgd] = window;
        rgbTable[KnownColors.msocbvcrGSPGroupHeaderLightBkgd] = window;
        rgbTable[KnownColors.msocbvcrGSPGroupHeaderText] = controlText;
        rgbTable[KnownColors.msocbvcrGSPGroupHeaderText] = windowText;
        rgbTable[KnownColors.msocbvcrGSPGroupline] = buttonShadow;
        rgbTable[KnownColors.msocbvcrGSPGroupline] = window;
        rgbTable[KnownColors.msocbvcrGSPHyperlink] = empty;
        rgbTable[KnownColors.msocbvcrGSPLightBkgd] = window;
        rgbTable[KnownColors.msocbvcrHyperlink] = empty;
        rgbTable[KnownColors.msocbvcrHyperlinkFollowed] = empty;
        rgbTable[KnownColors.msocbvcrJotNavUIBdr] = buttonShadow;
        rgbTable[KnownColors.msocbvcrJotNavUIBdr] = windowText;
        rgbTable[KnownColors.msocbvcrJotNavUIGradBegin] = buttonFace;
        rgbTable[KnownColors.msocbvcrJotNavUIGradBegin] = window;
        rgbTable[KnownColors.msocbvcrJotNavUIGradEnd] = window;
        rgbTable[KnownColors.msocbvcrJotNavUIGradMiddle] = buttonFace;
        rgbTable[KnownColors.msocbvcrJotNavUIGradMiddle] = window;
        rgbTable[KnownColors.msocbvcrJotNavUIText] = windowText;
        rgbTable[KnownColors.msocbvcrListHeaderArrow] = controlText;
        rgbTable[KnownColors.msocbvcrNetLookBkgnd] = empty;
        rgbTable[KnownColors.msocbvcrOABBkgd] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOBBkgdBdr] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOBBkgdBdrContrast] = window;
        rgbTable[KnownColors.msocbvcrOGMDIParentWorkspaceBkgd] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOGRulerActiveBkgd] = window;
        rgbTable[KnownColors.msocbvcrOGRulerBdr] = controlText;
        rgbTable[KnownColors.msocbvcrOGRulerBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrOGRulerInactiveBkgd] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOGRulerTabBoxBdr] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOGRulerTabBoxBdrHighlight] = buttonHighlight;
        rgbTable[KnownColors.msocbvcrOGRulerTabStopTicks] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOGRulerText] = windowText;
        rgbTable[KnownColors.msocbvcrOGTaskPaneGroupBoxHeaderBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrOGWorkspaceBkgd] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOLKFlagNone] = buttonHighlight;
        rgbTable[KnownColors.msocbvcrOLKFolderbarDark] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOLKFolderbarLight] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOLKFolderbarText] = window;
        rgbTable[KnownColors.msocbvcrOLKGridlines] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOLKGroupLine] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOLKGroupNested] = buttonFace;
        rgbTable[KnownColors.msocbvcrOLKGroupShaded] = buttonFace;
        rgbTable[KnownColors.msocbvcrOLKGroupText] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOLKIconBar] = buttonFace;
        rgbTable[KnownColors.msocbvcrOLKInfoBarBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrOLKInfoBarText] = controlText;
        rgbTable[KnownColors.msocbvcrOLKPreviewPaneLabelText] = windowText;
        rgbTable[KnownColors.msocbvcrOLKTodayIndicatorDark] = highlight;
        rgbTable[KnownColors.msocbvcrOLKTodayIndicatorLight] = buttonFace;
        rgbTable[KnownColors.msocbvcrOLKWBActionDividerLine] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOLKWBButtonDark] = buttonFace;
        rgbTable[KnownColors.msocbvcrOLKWBButtonLight] = buttonFace;
        rgbTable[KnownColors.msocbvcrOLKWBButtonLight] = buttonHighlight;
        rgbTable[KnownColors.msocbvcrOLKWBDarkOutline] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOLKWBFoldersBackground] = window;
        rgbTable[KnownColors.msocbvcrOLKWBHoverButtonDark] = empty;
        rgbTable[KnownColors.msocbvcrOLKWBHoverButtonLight] = empty;
        rgbTable[KnownColors.msocbvcrOLKWBLabelText] = windowText;
        rgbTable[KnownColors.msocbvcrOLKWBPressedButtonDark] = empty;
        rgbTable[KnownColors.msocbvcrOLKWBPressedButtonLight] = empty;
        rgbTable[KnownColors.msocbvcrOLKWBSelectedButtonDark] = empty;
        rgbTable[KnownColors.msocbvcrOLKWBSelectedButtonLight] = empty;
        rgbTable[KnownColors.msocbvcrOLKWBSplitterDark] = buttonShadow;
        rgbTable[KnownColors.msocbvcrOLKWBSplitterLight] = buttonFace;
        rgbTable[KnownColors.msocbvcrOLKWBSplitterLight] = buttonShadow;
        rgbTable[KnownColors.msocbvcrPlacesBarBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrPPOutlineThumbnailsPaneTabAreaBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrPPOutlineThumbnailsPaneTabBdr] = buttonShadow;
        rgbTable[KnownColors.msocbvcrPPOutlineThumbnailsPaneTabInactiveBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrPPOutlineThumbnailsPaneTabText] = windowText;
        rgbTable[KnownColors.msocbvcrPPSlideBdrActiveSelected] = highlight;
        rgbTable[KnownColors.msocbvcrPPSlideBdrActiveSelectedMouseOver] = highlight;
        rgbTable[KnownColors.msocbvcrPPSlideBdrInactiveSelected] = grayText;
        rgbTable[KnownColors.msocbvcrPPSlideBdrMouseOver] = highlight;
        rgbTable[KnownColors.msocbvcrPubPrintDocScratchPageBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrPubWebDocScratchPageBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrSBBdr] = buttonShadow;
        rgbTable[KnownColors.msocbvcrScrollbarBkgd] = buttonShadow;
        rgbTable[KnownColors.msocbvcrToastGradBegin] = buttonFace;
        rgbTable[KnownColors.msocbvcrToastGradEnd] = buttonFace;
        rgbTable[KnownColors.msocbvcrWPBdrInnerDocked] = empty;
        rgbTable[KnownColors.msocbvcrWPBdrOuterDocked] = buttonFace;
        rgbTable[KnownColors.msocbvcrWPBdrOuterFloating] = buttonShadow;
        rgbTable[KnownColors.msocbvcrWPBkgd] = window;
        rgbTable[KnownColors.msocbvcrWPCtlBdr] = buttonShadow;
        rgbTable[KnownColors.msocbvcrWPCtlBdrDefault] = buttonShadow;
        rgbTable[KnownColors.msocbvcrWPCtlBdrDefault] = controlText;
        rgbTable[KnownColors.msocbvcrWPCtlBdrDisabled] = buttonShadow;
        rgbTable[KnownColors.msocbvcrWPCtlBkgd] = buttonFace;
        rgbTable[KnownColors.msocbvcrWPCtlBkgdDisabled] = buttonFace;
        rgbTable[KnownColors.msocbvcrWPCtlText] = controlText;
        rgbTable[KnownColors.msocbvcrWPCtlTextDisabled] = buttonShadow;
        rgbTable[KnownColors.msocbvcrWPCtlTextMouseDown] = highlightText;
        rgbTable[KnownColors.msocbvcrWPGroupline] = buttonShadow;
        rgbTable[KnownColors.msocbvcrWPInfoTipBkgd] = SystemColorsEx.Info;
        rgbTable[KnownColors.msocbvcrWPInfoTipText] = SystemColorsEx.InfoText;
        rgbTable[KnownColors.msocbvcrWPNavBarBkgnd] = buttonFace;
        rgbTable[KnownColors.msocbvcrWPText] = controlText;
        rgbTable[KnownColors.msocbvcrWPText] = windowText;
        rgbTable[KnownColors.msocbvcrWPTextDisabled] = grayText;
        rgbTable[KnownColors.msocbvcrWPTitleBkgdActive] = highlight;
        rgbTable[KnownColors.msocbvcrWPTitleBkgdInactive] = buttonFace;
        rgbTable[KnownColors.msocbvcrWPTitleTextActive] = highlightText;
        rgbTable[KnownColors.msocbvcrWPTitleTextInactive] = controlText;
        rgbTable[KnownColors.msocbvcrXLFormulaBarBkgd] = buttonFace;
    }
}
