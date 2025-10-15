using cYo.Common.Drawing;
using cYo.Common.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace cYo.Common.Windows.Forms
{
	/// <summary>
	/// Class to centralisation application Theming. When theming is enabled, calls <see cref="KnownColorTableEx"/> on initialization to override built-in <see cref="System.Drawing.SystemColors"/> with theme-defined colors.<br/>
	/// Exposes <see cref="Theme(Control)"/> for recursive <see cref="Control"/> theming by setting class fields and leveraging <see cref="UXTheme"/> for native Windows OS theming.
	/// </summary>
	public static partial class ThemeExtensions
    {
        /// <summary>
        /// <para>Indicates whether Dark Mode has been <b>enabled</b>. Set on initialization and referenced in all public non-initialization <see cref="ThemeExtensions"/> calls.</para>
        /// <para>Also serves as a <b>global reference</b> for other classes.</para>
        /// </summary>
        /// <remarks>
        /// This is intended to mirror the <c>Application.IsDarkModeEnabled</c> which is available in .NET 9+.
        /// </remarks>
        public static bool IsDarkModeEnabled = false;

        private static bool IsThemed { get; set; } = false;

		/// <summary>
		/// Sets <see cref="IsDarkModeEnabled"/>. If <paramref name="useDarkMode"/> is <c>true</c>, initializes <see cref="KnownColorTableEx"/> which replaces built-in <see cref="System.Drawing.SystemColors"/> with Dark Mode colors.
		/// </summary>
		/// <param name="useDarkMode">Determines if the Dark Mode theme should be used (and <see cref="System.Drawing.SystemColors"/> replaced).</param>
		/// <remarks>
		/// If <see cref="IsDarkModeEnabled"/> is set to false, all calls to other <see cref="ThemeExtensions"/> functions will immediately return.
		/// </remarks>
		public static void Initialize(Theme.Themes theme)
        {
            IsDarkModeEnabled = theme == Forms.Theme.Themes.Dark;
            IsThemed = ThemeFactory(theme);

			if (IsDarkModeEnabled)
            {
                KnownColorTableEx darkColorTable = new KnownColorTableEx();
                darkColorTable.Initialize(IsDarkModeEnabled);
                darkColorTable.SetColor(KnownColor.WhiteSmoke, ThemeColors.BlackSmoke.ToArgb());
                UXTheme.Initialize();
            }
        }

        // Returns if a theme other than default is being applied
        private static bool ThemeFactory(Theme.Themes theme)
        {
            switch (theme)
            {
                case Forms.Theme.Themes.Dark:
                    ThemeColors.Register<DarkThemeColorTable>();
                    return true;
                case Forms.Theme.Themes.Default:
                default:
                    ThemeColors.Register<ThemeColorTable>();
                    return false;
            };
        }

        /// <summary>
		/// Runs the provided <see cref="Action"/> only if the Theme isn't <see cref="Theme.Themes.Default"/> or if <paramref name="onlyDrawIfDefault"/> is false and we are in the <see cref="Theme.Themes.Default"/> Theme, then it will always draw.
		/// </summary>
		/// <param name="action">The <see cref="Action"/> to run</param>
		/// <param name="onlyDrawIfDefault"></param>
		/// <returns>a <see cref="Boolean"/> if it was successful in drawing</returns>
		public static bool TryDrawTheme(Action action, bool onlyDrawIfDefault = false)
		{
            if ((!ThemeColors.IsDefault && !onlyDrawIfDefault) || (ThemeColors.IsDefault && onlyDrawIfDefault)) // We are themed or we have requested to only draw when in the default 
			{
				action();
                return true;
			}
            return false;
		}

		/// <summary>
		/// Themes a <see cref="Control"/>, recursively. The specifics are determined by the control <see cref="Type"/> mapping in <see cref="themeHandlers"/> and <see cref="complexUXThemeHandlers"/>.<br/>
        /// Also sets the window theme for a <see cref="Form"/>.
        /// </summary>
        /// <param name="control"><see cref="Control"/> to be themed.</param>
        /// <remarks>
        /// Theming can essentially be broken down into these (optional) parts:<br/>
        /// - <see cref="themeHandlers"/> - Set <typeparamref name="ForeColor"/> and <typeparamref name="BackColor"/><br/>
        /// - <see cref="themeHandlers"/> - Attach custom <see cref="EventHandler"/> (<c>Draw</c>, <c>Paint</c>, <c>Mouse</c>)<br/>
        /// - <see cref="themeHandlers"/> - Set <see cref="BorderStyle"/> and <see cref="FlatStyle"/> (either for looks, or because it's required for <see cref="UXTheme.SetControlTheme(IntPtr, string, string)"/> to identify a part)<br/>
        /// - <see cref="complexUXThemeHandlers"/> - Apply native Windows OS theming via <see cref="UXTheme.SetControlTheme(IntPtr, string, string)"/><br/>
        /// </remarks>
        public static void Theme(this Control control)
        {
            if (!IsDarkModeEnabled) return;

            if (control is Form form)
            {
                form.BackColor = ThemeColors.Material.Window;
                //form.ForeColor = SystemColors.ControlText;
                SetWindowUXTheme(form);
            }

            if (control.BackColor.IsSystemColor)
            {
                try
                {
                    control.BackColor = KnownColorTableEx.GetSystemColor(control.BackColor.ToKnownColor());
                }
                catch
                {}
            }

            if (control.ForeColor.IsSystemColor)
            {
                try
                {
                    control.ForeColor = KnownColorTableEx.GetSystemColor(control.ForeColor.ToKnownColor());
                }
                catch
                {}
            }

            if (themeHandlers.TryGetValue(control.GetType(), out var theme))
            {
                theme(control);
            }
            else if (themeHandlers.TryGetValue(control.GetType().BaseType, out var themeBase))
            {
                themeBase(control);
            }

            SetControlUXTheme(control);

            foreach (Control childControl in control.Controls)
                Theme(childControl);
        }

    }
}
