using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme;

public interface ITheme
{
    UIComponent UIComponent { get; }

    void ApplyTheme(Control control);
}
