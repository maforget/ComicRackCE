using cYo.Common.Windows.Forms.Theme;
using System;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
    public class FormEx : Form, ITheme
    {
        public virtual UIComponent UIComponent => UIComponent.Window;

        public virtual void ApplyTheme(Control control = null)
        {
            ThemeExtensions.Theme(control ?? this);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyTheme();
        }
    }
}
