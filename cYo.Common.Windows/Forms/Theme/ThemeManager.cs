using cYo.Common.Drawing.ExtendedColors;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms.Theme.Internal;
using cYo.Common.Windows.Forms.Theme.Resources;
using System.Drawing;

namespace cYo.Common.Windows.Forms.Theme;

public class ThemeManager
{
    /// <summary>
    /// Indicates whether Dark Mode has been enabled based on the current <see cref="Themes"/>.<br/>
    /// This is used to indicate to Plugins
    /// </summary>
    /// <remarks>
    /// This is intended to mirror the <c>Application.IsDarkModeEnabled</c> which is available in .NET 9+.
    /// </remarks>
    public static bool IsDarkModeEnabled { get; private set; } = false;

    private static bool IsThemed { get; set; } = false;

    /// <summary>
    /// Initializes the required <see cref="Theme"/> components and sets <see cref="IsDarkModeEnabled"/> and <see cref="IsThemed"/>
    /// based on the <paramref name="theme"/>.<br/>
    /// </summary>
    /// <param name="theme">Current theme.</param>
    public static void Initialize(Themes theme)
    {
        // TODO: add a way for a theme to indicate whether it is a Light or Dark Color Mode theme
        IsDarkModeEnabled = InitializeDarkMode(theme);
        IsThemed = ThemeFactory(theme);
    }

    /// <summary>
    /// Calls <see cref="ThemeColors.Register"/> to register <see cref="ThemeColorTable"/> based on the value of <paramref name="theme"/>.
    /// </summary>
    /// <param name="theme">Current theme.</param>
    /// <returns><paramref name="true"/> if <paramref name="theme"/> is not <see cref="Themes.Default"/></returns>
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

    /// <summary>
    /// Initializes Dark Mode if <paramref name="theme"/> is a Dark Mode theme.
    /// </summary>
    /// <remarks>
    /// <b>Dark Mode Initialization</b>
    /// <list type="bullet">
    ///     <item>
    ///         <term><see cref="ThemeHandler.Register"/></term>
    ///         <description>Register Dark Mode Theme Handler that will handle <see cref="System.Windows.Forms.Control"/> theming.</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="KnownColorTableEx.Initialize"/></term>
    ///         <description>Replaces <see cref="System.Drawing.SystemColors"/> with <see cref="SystemColorsEx"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="UXTheme.Initialize"/></term>
    ///         <description>Determines if the host Windows OS Theme Engine supports Dark Mode and sets related attributes.</description>
    ///     </item>
    /// </list>
    /// </remarks>
    /// <param name="theme">Current theme.</param>
    /// <returns><paramref name="true"/> if <paramref name="theme"/> is a Dark Mode theme.</returns>
    private static bool InitializeDarkMode(Themes theme)
    {
        if (theme == Themes.Dark)
        {
            ThemeHandler.Register<DarkMode.DarkThemeHandler>();
            KnownColorTableEx darkColorTable = new KnownColorTableEx();
            darkColorTable.Initialize(true);
            darkColorTable.SetColor(KnownColor.WhiteSmoke, ThemeColors.DarkMode.BlackSmoke.ToArgb());
            UXTheme.Initialize();
            return true;
        }
        return false;
    }
}
