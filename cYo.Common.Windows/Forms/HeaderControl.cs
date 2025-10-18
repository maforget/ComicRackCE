using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Common.Windows.Properties;

namespace cYo.Common.Windows.Forms
{
    public class HeaderControl : Control
    {
        private bool pressed;

        private HeaderAdornments headerAdornments;

        private StringAlignment textAlignment;

        private static readonly Image sortUpImage = Resources.SortUp;

        private static readonly Image sortDownImage = Resources.SortDown;

        private static readonly Image dropDownImage = Resources.SmallArrowDown;

        [DefaultValue(false)]
        public bool Pressed
        {
            get
            {
                return pressed;
            }
            set
            {
                if (pressed != value)
                {
                    pressed = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(HeaderAdornments.None)]
        public HeaderAdornments HeaderAdornments
        {
            get
            {
                return headerAdornments;
            }
            set
            {
                if (headerAdornments != value)
                {
                    headerAdornments = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(StringAlignment.Near)]
        public StringAlignment TextAlignment
        {
            get
            {
                return textAlignment;
            }
            set
            {
                if (textAlignment != value)
                {
                    textAlignment = value;
                    Invalidate();
                }
            }
        }

        public HeaderControl()
        {
            SetStyle(ControlStyles.ResizeRedraw, value: true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
            SetStyle(ControlStyles.UserPaint, value: true);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Draw(e.Graphics, base.ClientRectangle, Font, TextAlignment, Text, HeaderState.Normal, HeaderAdornments);
        }

        public static void Draw(Graphics graphics, Rectangle bounds, Font font, StringAlignment alignment, string text, HeaderState state, HeaderAdornments adornments)
        {
            using (StringFormat format = new StringFormat(StringFormatFlags.NoWrap)
            {
                LineAlignment = StringAlignment.Center,
                Alignment = alignment,
                Trimming = StringTrimming.EllipsisCharacter
            })
            {
                Draw(graphics, bounds, font, text, SystemColors.ControlText, format, state, adornments);
            }
        }

        // ComicBrowser/Pages Views > Details 
        public static void Draw(Graphics graphics, Rectangle bounds, Font font, string text, Color textColor, StringFormat format, HeaderState state, HeaderAdornments adornments)
        {
            graphics.FillRectangle(ThemeBrushes.Header.Back, bounds);
            StyledRenderer.AlphaStyle state2;
            switch (state)
            {
                case HeaderState.Pressed:
                    state2 = StyledRenderer.AlphaStyle.SelectedHot;
                    break;
                case HeaderState.Hot:
                    state2 = StyledRenderer.AlphaStyle.Selected;
                    break;
                case HeaderState.Active:
                    state2 = StyledRenderer.AlphaStyle.Hot;
                    break;
                default:
                    state2 = StyledRenderer.AlphaStyle.Hot;
                    break;
            }
            bounds.Width--;
            bounds.Height--;
            graphics.DrawStyledRectangle(bounds, state2, ThemeColors.Header.Separator, StyledRenderer.Default.Frame(0, 1));
            bounds.Inflate(-2, 0);

            using (graphics.SaveState())
            {
                if (adornments.IsSet(HeaderAdornments.DropDown))
                {
                    Rectangle dropDownBounds = GetDropDownBounds(bounds);
                    graphics.DrawLine(SystemPens.ControlDark, dropDownBounds.TopLeft().Add(0, 4), dropDownBounds.BottomLeft().Add(0, -4));
                    graphics.DrawLine(SystemPens.ControlLight, dropDownBounds.TopLeft().Add(1, 4), dropDownBounds.BottomLeft().Add(1, -4));
                    graphics.DrawImage(dropDownImage, dropDownImage.Size.Align(dropDownBounds, ContentAlignment.MiddleCenter));
                    dropDownBounds.Inflate(4, 4);
                    graphics.SetClip(dropDownBounds, CombineMode.Exclude);
                }
                if (adornments.IsSet(HeaderAdornments.SortDown))
                {
                    graphics.DrawImage(sortDownImage, sortDownImage.Size.Align(bounds.Pad(0, 1), ContentAlignment.TopCenter), 0.7f);
                }
                else if (adornments.IsSet(HeaderAdornments.SortUp))
                {
                    graphics.DrawImage(sortUpImage, sortDownImage.Size.Align(bounds.Pad(0, 1), ContentAlignment.TopCenter), 0.7f);
                }
                using (Brush brush = new SolidBrush(textColor))
                {
                    graphics.DrawString(text, font, brush, bounds.Pad(0, 3), format);
                }
            }
        }

        public static Rectangle GetDropDownBounds(Rectangle bounds)
        {
            int num = Math.Min(dropDownImage.Width + 4, bounds.Width);
            return new Rectangle(bounds.Right - num, bounds.Top, num, bounds.Height);
        }
    }
}
