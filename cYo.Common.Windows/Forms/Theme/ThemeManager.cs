using cYo.Common.Drawing.ExtendedColors;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms.Theme.Internal;
using cYo.Common.Windows.Forms.Theme.Resources;
using System.Drawing;

namespace cYo.Common.Windows.Forms.Theme;

public class ThemeManager
{
    /// <summary>
    /// <para>Indicates whether Dark Mode has been <b>enabled</b>. Set on initialization and referenced in all public non-initialization <see cref="ThemeExtensions"/> calls.</para>
    /// <para>Also serves as a <b>global reference</b> for other classes.</para>
    /// </summary>
    /// <remarks>
    /// This is intended to mirror the <c>Application.IsDarkModeEnabled</c> which is available in .NET 9+.
    /// </remarks>
    public static bool IsDarkModeEnabled { get; private set; } = false;

    private static bool IsThemed { get; set; } = false;

    /// <summary>
    /// Sets <see cref="IsDarkModeEnabled"/>. If <paramref name="useDarkMode"/> is <c>true</c>, initializes <see cref="KnownColorTableEx"/> which replaces built-in <see cref="System.Drawing.SystemColors"/> with Dark Mode colors.
    /// </summary>
    /// <param name="useDarkMode">Determines if the Dark Mode theme should be used (and <see cref="System.Drawing.SystemColors"/> replaced).</param>
    /// <remarks>
    /// If <see cref="IsDarkModeEnabled"/> is set to false, all calls to other <see cref="ThemeExtensions"/> functions will immediately return.
    /// </remarks>
    public static void Initialize(Themes theme)
    {
        // TODO: add a way for a theme to indicate whether it is a Light or Dark Color Mode theme
        IsDarkModeEnabled = theme == Themes.Dark;
        IsThemed = ThemeFactory(theme);

        if (IsDarkModeEnabled)
        {
            ThemeHandler.Register<DarkMode.DarkThemeHandler>();
            KnownColorTableEx darkColorTable = new KnownColorTableEx();
            darkColorTable.Initialize(IsDarkModeEnabled);
            darkColorTable.SetColor(KnownColor.WhiteSmoke, ThemeColors.BlackSmoke.ToArgb());
            UXTheme.Initialize();
        }
    }

    // Returns if a theme other than default is being applied
    private static bool ThemeFactory(Themes theme)
    {
        switch (theme)
        {
            case Themes.Dark:
                ThemeColors.Register<DarkMode.DarkThemeColorTable>();
                return true;
            case Themes.Default:
            default:
                ThemeColors.Register<ThemeColorTable>();
                return false;
        };
    }
}
