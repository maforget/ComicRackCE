using cYo.Common.Windows.Forms.Theme.Resources;

namespace cYo.Common.Windows.Forms.Theme;

public interface IThemeCustom
{
    UIComponent UIComponent { get; }
    ThemeControlDefinition ControlDefinition { get; }
}
