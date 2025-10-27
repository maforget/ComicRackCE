using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine.Drawing;

namespace cYo.Projects.ComicRack.Engine.Controls
{
    public class RatingControl : Control
    {
        private Image ratingImage;

        private int maximumRating = 5;

        private int ratingDigits = 1;

        private bool drawText;

        private bool drawBorder = true;

        private bool centerRating = true;

        private RatingRenderer Renderer;

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (!(base.Text == value))
                {
                    base.Text = value;
                    Invalidate();
                }
            }
        }

        [Category("Display")]
        [Description("The image to display a rating point")]
        [DefaultValue(null)]
        public Image RatingImage
        {
            get
            {
                return ratingImage;
            }
            set
            {
                if (ratingImage != value)
                {
                    ratingImage = value;
                    Invalidate();
                }
            }
        }

        [Category("Behavior")]
        [Description("Number of stars to use")]
        [DefaultValue(5)]
        public int MaximumRating
        {
            get
            {
                return maximumRating;
            }
            set
            {
                value = value.Clamp(0, 100);
                if (maximumRating != value)
                {
                    maximumRating = value;
                    Invalidate();
                }
            }
        }

        [Category("Behavior")]
        [Description("The rating value")]
        [DefaultValue(0)]
        public float Rating
        {
            get
            {
                float.TryParse(Text, out var result);
                return ((float)Math.Round(result, ratingDigits)).Clamp(0f, MaximumRating);
            }
            set
            {
                value = value.Clamp(0f, MaximumRating);
                if (Rating != value)
                {
                    Text = value.ToString();
                }
            }
        }

        [Category("Behavior")]
        [Description("Digits for the rating")]
        [DefaultValue(1)]
        public int RatingDigits
        {
            get
            {
                return ratingDigits;
            }
            set
            {
                ratingDigits = value;
            }
        }

        [Category("Display")]
        [Description("Also draw the numeric value")]
        [DefaultValue(false)]
        public bool DrawText
        {
            get
            {
                return drawText;
            }
            set
            {
                if (drawText != value)
                {
                    drawText = value;
                    Invalidate();
                }
            }
        }

        [Category("Display")]
        [Description("Draw Border")]
        [DefaultValue(true)]
        public bool DrawBorder
        {
            get
            {
                return drawBorder;
            }
            set
            {
                if (drawBorder != value)
                {
                    drawBorder = value;
                    Invalidate();
                }
            }
        }

        [Category("Display")]
        [Description("Center Rating Stars")]
        [DefaultValue(true)]
        public bool CenterRating
        {
            get
            {
                return centerRating;
            }
            set
            {
                if (centerRating != value)
                {
                    centerRating = value;
                    Invalidate();
                }
            }
        }

        public RatingControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
            SetStyle(ControlStyles.UserPaint, value: true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
            SetStyle(ControlStyles.ResizeRedraw, value: true);
            SetStyle(ControlStyles.Selectable, value: true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
            ThemeExtensions.InvokeAction(() => BackColor = ThemeColors.DarkMode.RatingControl.Back);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics graphics = e.Graphics;
            if (Application.RenderWithVisualStyles)
            {
                if (drawBorder)
                {
                    VisualStyleElement element = (Focused ? VisualStyleElement.TextBox.TextEdit.Focused : VisualStyleElement.TextBox.TextEdit.Normal);
                    VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(element);
                    //visualStyleRenderer.DrawBackground(graphics, base.ClientRectangle);
                    visualStyleRenderer.DrawThemeBackground(e, base.ClientRectangle, BackColor);
                }
                DrawContent(graphics);
                return;
            }
            if (drawBorder)
            {
                graphics.Clear(SystemColors.Window);
            }
            DrawContent(graphics);
            if (drawBorder)
            {
                if (Focused)
                {
                    Rectangle clientRectangle = base.ClientRectangle;
                    clientRectangle.Inflate(-2, -2);
                    //ControlPaint.DrawFocusRectangle(graphics, clientRectangle);
                    ControlPaintEx.DrawFocusRectangle(graphics, clientRectangle);
                }
                //ControlPaint.DrawBorder3D(graphics, base.ClientRectangle, Border3DStyle.Sunken);
                ControlPaintEx.DrawBorder3D(graphics, base.ClientRectangle, Border3DStyle.Sunken);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            if (Renderer != null)
            {
                Text = Math.Round(Renderer.GetRatingFromStrip(e.Location), ratingDigits).ToString();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (base.Capture && Renderer != null)
            {
                Text = Math.Round(Renderer.GetRatingFromStrip(e.Location), ratingDigits).ToString();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            AdjustRating(e.Delta / SystemInformation.MouseWheelScrollDelta);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
                Rating = int.Parse(e.KeyChar.ToString());
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Right:
                    AdjustRating(1);
                    break;
                case Keys.Prior:
                    Rating++;
                    break;
                case Keys.Left:
                case Keys.Down:
                    AdjustRating(-1);
                    break;
                case Keys.Next:
                    Rating--;
                    break;
                case Keys.Home:
                    Rating = 0f;
                    break;
                case Keys.End:
                    Rating = MaximumRating;
                    break;
            }
            base.OnKeyDown(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Prior:
                case Keys.Next:
                case Keys.End:
                case Keys.Home:
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        private void AdjustRating(int direction)
        {
            Rating += (float)direction * (float)Math.Pow(10.0, -RatingDigits);
        }

        private void DrawContent(Graphics gr)
        {
            Rectangle rectangle = base.ClientRectangle;
            rectangle.Inflate(-2, -2);
            if (drawText)
            {
                Color color = ThemeColors.RatingControl.Unrated;
                if (Rating != 0f)
                    ThemeExtensions.InvokeAction(
                        () => color = ForeColor,
                        () => color = ThemeColors.DarkMode.RatingControl.Rated
                    );
                string ratingText = GetRatingText(Rating);
                Size size = gr.MeasureString(ratingText, Font).ToSize();
                size.Width += 4;
                Rectangle rectangle2 = new Rectangle(rectangle.Right - size.Width - 2, rectangle.Top + (rectangle.Height - size.Height) / 2, size.Width, size.Height + 1);
                using (gr.AntiAlias())
                {
                    using (Pen pen = new Pen(color, 1.5f))
                    {
                        using (Brush brush = new SolidBrush(color))
                        {
                            using (StringFormat format = new StringFormat
                            {
                                Alignment = StringAlignment.Center,
                                LineAlignment = StringAlignment.Center
                            })
                            {
                                using (GraphicsPath path = rectangle2.ConvertToPath(2, 2))
                                {
                                    gr.DrawPath(pen, path);
                                }
                                gr.DrawString(ratingText, Font, brush, rectangle2, format);
                            }
                        }
                    }
                }
                rectangle = rectangle.Pad(0, 0, size.Width + 2);
            }
            Renderer = new RatingRenderer(RatingImage, rectangle, MaximumRating);
            Renderer.RatingScaleMode = (CenterRating ? RectangleScaleMode.Center : RectangleScaleMode.None);
            Renderer.DrawRatingStrip(gr, Rating);
        }

        private int GetRatingTextWidth(Graphics gr)
        {
            if (!DrawText)
            {
                return 0;
            }
            return (int)Math.Ceiling(gr.MeasureString(GetRatingText(MaximumRating), Font).Width);
        }

        private string GetRatingText(float rating)
        {
            return rating.ToString("N" + ratingDigits);
        }

        public static ToolStripControlHost InsertRatingControl(ContextMenuStrip strip, int index, Image star, Func<IEditRating> rating)
        {
            RatingControl r = new RatingControl();
            r.Height = FormUtility.ScaleDpiY(18);
            r.Width = FormUtility.ScaleDpiX(200);
            r.RatingImage = star;
            r.DrawText = true;
            r.DrawBorder = false;
            r.BackColor = Color.Transparent;
            r.CenterRating = false;
            strip.Opening += delegate
            {
                IEditRating editRating = rating();
                r.Tag = editRating;
                r.Rating = editRating.GetRating();
            };
            strip.Closed += delegate
            {
                (r.Tag as IEditRating).SetRating(r.Rating);
            };
            ToolStripControlHost toolStripControlHost = new ToolStripControlHost(r);
            strip.Items.Insert(index, toolStripControlHost);
            return toolStripControlHost;
        }

        public static ToolStripControlHost AddRatingControl(ContextMenuStrip strip, Image star, Func<IEditRating> rating)
        {
            return InsertRatingControl(strip, strip.Items.Count, star, rating);
        }
    }
}
