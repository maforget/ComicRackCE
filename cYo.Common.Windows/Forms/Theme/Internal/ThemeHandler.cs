using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.Internal;

public static class ThemeHandler
{
    private static IThemeHandler Handler;

    /// <summary>
    /// Registers a theme handler type that implements IThemeHandler.
    /// </summary>
    public static void Register<T>() where T : IThemeHandler, new()
    {
        Handler = new T();
    }

    public static void SetTheme(this Control control) => SetTheme(control, false);

    /// <summary>
    /// Applies the registered theme handler to the specified control.
    /// </summary>
    public static void SetTheme(this Control control, bool recursive)
    {
        if (Handler != null)
            Handler.Handle(control);

        if (Handler != null && recursive)
            foreach (Control childControl in control.Controls)
                childControl.SetTheme(true);
    }
}

//public abstract class BaseThemeHandler : IThemeHandler
//{
//    protected readonly Dictionary<Type, Action<Control>> ThemeHandlers = new();

//    public abstract void RegisterHandlers();

//    public virtual void Handle(Control control)
//    {
//        if (ThemeHandlers.TryGetValue(control.GetType(), out var themeAction))
//            themeAction(control);

//        foreach (Control child in control.Controls)
//            Handle(child);
//    }
//}