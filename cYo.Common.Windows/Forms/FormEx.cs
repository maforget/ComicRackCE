using System;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
    public class FormEx : Form, ITheme
    {
        public void ApplyTheme(Control? control = null)
        {
            if (control == null)
            {
                ThemeExtensions.Theme(this);
            }
            else
            {
                ThemeExtensions.Theme(control);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyTheme();
        }
    }
}
