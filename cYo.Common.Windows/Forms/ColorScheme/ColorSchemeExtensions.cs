using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.ColorScheme
{
    public static class ColorSchemeExtensions
    {
        public static void SetColorScheme(this Control control, bool darkMode = false)
        {
            //Need to set the color even if the SystemColors as been changed so all the Control is drawn correctly.
            if (darkMode)
            {
                control.BackColor = Color.FromArgb(32, 32, 32);
                control.ForeColor = Color.White;
            }
        }

        public static void SetDarkMode(bool darkMode = false)
        {
            Color backColor = Color.FromArgb(32, 32, 32);
            Color foreColor = Color.White;

            ColorSchemeController colorSchemeController = new ColorSchemeController();

            colorSchemeController.SetColor(KnownColor.Control, backColor.ToArgb());
            colorSchemeController.SetColor(KnownColor.Window, backColor.ToArgb());
            //colorSchemeController.SetColor(KnownColor.ControlLight, backColor.ToArgb());
            colorSchemeController.SetColor(KnownColor.ControlLightLight, backColor.ToArgb()); //TabBar Selected Color
            colorSchemeController.SetColor(KnownColor.ButtonFace, backColor.ToArgb()); //Context Menu BackColor
            //colorSchemeController.SetColor(KnownColor.InactiveBorder, backColor.ToArgb());
            colorSchemeController.SetColor(KnownColor.WhiteSmoke, backColor.ToArgb()); //Default Reader Background color

            colorSchemeController.SetColor(KnownColor.ControlText, foreColor.ToArgb());
            colorSchemeController.SetColor(KnownColor.WindowText, foreColor.ToArgb());
            //colorSchemeController.SetColor(KnownColor.ControlDark, foreColor.ToArgb());
            colorSchemeController.SetColor(KnownColor.ControlDarkDark, foreColor.ToArgb());
            //colorSchemeController.SetColor(KnownColor.InfoText, foreColor.ToArgb());
            //colorSchemeController.SetColor(KnownColor.ActiveCaptionText, foreColor.ToArgb());
            //colorSchemeController.SetColor(KnownColor.ButtonShadow, foreColor.ToArgb()); //Context Menu Separator
            colorSchemeController.SetColor(KnownColor.MenuText, foreColor.ToArgb()); //Context Menu Text
        }
    }
}
