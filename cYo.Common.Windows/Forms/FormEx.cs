﻿using System;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
    public class FormEx : Form, ITheme
    {
        public void ApplyTheme(Control? control = null)
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
