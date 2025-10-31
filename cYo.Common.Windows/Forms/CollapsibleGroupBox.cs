using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common.Drawing;
using cYo.Common.Windows.Forms.Theme;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Common.Windows.Properties;

namespace cYo.Common.Windows.Forms
{
    public class CollapsibleGroupBox : ContainerControl
    {
        private Size fullSize;

        private int headerHeight = 24;

        private FontStyle fontStyle = FontStyle.Bold;

        private bool collapsed;

        private bool useTheme = true;

        private Rectangle cachedHeaderRectangle;

        private Bitmap collapsedImage;

        private Bitmap expandedImage;

        private static readonly Image arrowDown = Resources.SimpleArrowDown.ScaleDpi();

        private static readonly Image arrowRight = Resources.SimpleArrowRight.ScaleDpi();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FullHeight => fullSize.Height;

        [Browsable(false)]
        [DefaultValue(24)]
        public int HeaderHeight
        {
            get
            {
                return headerHeight;
            }
            set
            {
                if (headerHeight != value)
                {
                    headerHeight = value;
                    if (collapsed)
                    {
                        base.Height = FormUtility.ScaleDpiY(headerHeight);
                    }
                    Invalidate();
                }
            }
        }

        [DefaultValue(FontStyle.Bold)]
        public FontStyle HeaderFontStyle
        {
            get
            {
                return fontStyle;
            }
            set
            {
                if (fontStyle != value)
                {
                    fontStyle = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(false)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Collapsed
        {
            get
            {
                return collapsed;
            }
            set
            {
                if (value != collapsed)
                {
                    collapsed = value;
                    if (!value)
                    {
                        base.Size = fullSize;
                    }
                    else
                    {
                        fullSize = base.Size;
                        base.Height = FormUtility.ScaleDpiY(headerHeight);
                    }
                    Invalidate();
                    OnCollapsedChanged();
                }
            }
        }

        [DefaultValue(true)]
        public bool UseTheme
        {
            get
            {
                return useTheme;
            }
            set
            {
                if (useTheme != value)
                {
                    useTheme = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(false)]
        public bool TransparentTouch
        {
            get;
            set;
        }

        protected Rectangle HeaderRectangle
        {
            get
            {
                Rectangle rectangle = new Rectangle(0, 0, base.ClientRectangle.Width, FormUtility.ScaleDpiY(headerHeight));
                if (rectangle != cachedHeaderRectangle)
                {
                    cachedHeaderRectangle = rectangle;
                    RebuildRegion();
                }
                return rectangle;
            }
        }

        protected Rectangle ToggleRectange
        {
            get
            {
                Rectangle headerRectangle = HeaderRectangle;
                headerRectangle.Width = headerRectangle.Height;
                return headerRectangle;
            }
        }

        [DefaultValue(null)]
        public Bitmap CollapsedImage
        {
            get
            {
                return collapsedImage;
            }
            set
            {
                if (collapsedImage != value)
                {
                    collapsedImage = value;
                    Invalidate(ToggleRectange);
                }
            }
        }

        [DefaultValue(null)]
        public Bitmap ExpandedImage
        {
            get
            {
                return expandedImage;
            }
            set
            {
                if (expandedImage != value)
                {
                    expandedImage = value;
                    Invalidate(ToggleRectange);
                }
            }
        }

        public bool UsesTheme
        {
            get
            {
                if (useTheme && VisualStyleRenderer.IsSupported)
                {
                    return VisualStyleRenderer.IsElementDefined(VisualStyleElement.Tab.Body.Normal);
                }
                return false;
            }
        }

        public override Color BackColor
        {
            get
            {
                if (!UsesTheme || TransparentTouch)
                {
                    return base.BackColor;
                }
                return ThemeColors.CollapsibleGroupBox.Back;
            }
            set
            {
                base.BackColor = value;
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle displayRectangle = base.DisplayRectangle;
                return new Rectangle(0, FormUtility.ScaleDpiY(headerHeight), displayRectangle.Width, displayRectangle.Height - FormUtility.ScaleDpiY(headerHeight));
            }
        }

        public event EventHandler CollapsedChanged;

        public event EventHandler CollapseClicked;

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (ToggleRectange.Contains(e.Location))
            {
                Collapsed = !Collapsed;
                OnCollapseClicked();
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if (!HeaderRectangle.Contains(PointToClient(Cursor.Position)))
            {
                base.OnDoubleClick(e);
                return;
            }
            Collapsed = !Collapsed;
            OnCollapseClicked();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (UsesTheme && !TransparentTouch)
            {
                VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal);
                //visualStyleRenderer.DrawBackground(e.Graphics, base.ClientRectangle);
                visualStyleRenderer.DrawThemeBackground(e, base.ClientRectangle,BackColor);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            DrawBox(e.Graphics);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate(HeaderRectangle);
        }


        private void DrawBox(Graphics gr)
        {
            Rectangle headerRectangle = HeaderRectangle;
            // MultipleComicBooksDialog/Preferences
            if (UsesTheme)
            {

                using (LinearGradientBrush brush = new LinearGradientBrush(headerRectangle, ThemeColors.CollapsibleGroupBox.HeaderGradientStart, ThemeColors.CollapsibleGroupBox.HeaderGradientEnd, 0f))
                {
                    gr.FillRectangle(brush, headerRectangle);
                }
            }
            else
            {
                gr.FillRectangle(SystemBrushes.ControlDark, headerRectangle);
            }
            Image toggleImage = GetToggleImage();
            gr.DrawImage(toggleImage, toggleImage.Size.Align(ToggleRectange, System.Drawing.ContentAlignment.MiddleCenter));
            using (StringFormat format = new StringFormat
            {
                LineAlignment = StringAlignment.Center
            })
            {
                gr.DrawString(Text, FC.Get(Font, HeaderFontStyle), ThemeBrushes.CollapsibleGroupBox.HeaderText, headerRectangle.Pad(ToggleRectange.Width, 0), format);
            }
        }

        private Image GetToggleImage()
        {
            object obj;
            if (!collapsed)
            {
                obj = expandedImage;
                if (obj == null)
                {
                    return arrowDown;
                }
            }
            else
            {
                obj = collapsedImage ?? arrowRight;
            }
            return (Image)obj;
        }

        protected virtual void OnCollapsedChanged()
        {
            if (this.CollapsedChanged != null)
            {
                this.CollapsedChanged(this, EventArgs.Empty);
            }
        }

        protected virtual void OnCollapseClicked()
        {
            if (this.CollapseClicked != null)
            {
                this.CollapseClicked(this, EventArgs.Empty);
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control.SizeChanged += Control_SizeChanged;
            e.Control.LocationChanged += Control_SizeChanged;
            RebuildRegion();
        }

        private void Control_SizeChanged(object sender, EventArgs e)
        {
            RebuildRegion();
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            e.Control.SizeChanged -= Control_SizeChanged;
            e.Control.LocationChanged -= Control_SizeChanged;
            RebuildRegion();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RebuildRegion();
        }

        private void RebuildRegion()
        {
            if (!TransparentTouch || base.DesignMode)
            {
                return;
            }
            Region region = new Region(HeaderRectangle);
            foreach (Control control in base.Controls)
            {
                if (!(control is Label))
                {
                    region.Union(control.Bounds);
                }
            }
            base.Region = region;
        }
    }
}
