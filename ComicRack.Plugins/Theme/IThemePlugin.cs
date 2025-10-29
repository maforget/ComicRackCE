using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;

namespace cYo.Projects.ComicRack.Plugins.Theme
{
	public interface IThemePlugin: ITheme
	{
		Themes CurrentTheme { get; }
		bool IsDarkModeEnabled { get; }
		ToolStripRenderer ToolStripRenderer { get; }
	}
}
