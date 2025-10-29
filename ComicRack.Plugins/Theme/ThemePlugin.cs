using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;
using cYo.Common.Windows.Forms.Theme.Resources;

namespace cYo.Projects.ComicRack.Plugins.Theme
{
	public class ThemePlugin : IThemePlugin
	{
		private static ThemePlugin instance;
		public static ThemePlugin Default => instance ??= new ThemePlugin();

		public Themes CurrentTheme { get; } = Themes.Default;

		public bool IsDarkModeEnabled => CurrentTheme == Themes.Dark;

		private ToolStripRenderer toolStripRenderer;
		public ToolStripRenderer ToolStripRenderer => toolStripRenderer ??= IsDarkModeEnabled ? new ThemeToolStripProRenderer() : new ToolStripSystemRenderer();

		public ThemePlugin(Themes theme = Themes.Default)
		{
			CurrentTheme = theme;
		}

		public static void Register(Themes theme)
		{
			instance = new ThemePlugin(theme);
		}

		public void ApplyTheme(Control control = null)
		{
			control?.Theme();
		}
	}
}
