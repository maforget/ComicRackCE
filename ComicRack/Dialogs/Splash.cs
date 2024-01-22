using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Net;
using cYo.Common.Runtime;
using cYo.Common.Threading;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class Splash : LayeredForm
    {
        private volatile int progress;

        private volatile string message;

        private int messageLines = 3;

        private Color progressColor = Color.White;

        private int crashSequence;

        [DefaultValue(false)]
        public bool Fade
        {
            get;
            set;
        }

        [DefaultValue(0)]
        public int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    Invalidate(ProgressBounds);
                    if (!base.InvokeRequired)
                    {
                        Update();
                    }
                }
            }
        }

        [DefaultValue(null)]
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                if (!(message == value))
                {
                    message = value;
                    Invalidate(MessageBounds);
                    if (!base.InvokeRequired)
                    {
                        Update();
                    }
                }
            }
        }

        [DefaultValue(3)]
        public int MessageLines
        {
            get
            {
                return messageLines;
            }
            set
            {
                if (messageLines != value)
                {
                    messageLines = value;
                    Invalidate(MessageBounds);
                    if (!base.InvokeRequired)
                    {
                        Update();
                    }
                }
            }
        }

        [DefaultValue(typeof(Color), "White")]
        public Color ProgressColor
        {
            get
            {
                return progressColor;
            }
            set
            {
                if (!(progressColor == value))
                {
                    progressColor = value;
                    Invalidate(ProgressBounds);
                }
            }
        }

        public EventWaitHandle Initialized => initialized;

        protected Rectangle ProgressBounds
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                return new Rectangle(clientRectangle.Left + FormUtility.ScaleDpiX(6), clientRectangle.Bottom - FormUtility.ScaleDpiY(52), clientRectangle.Width - FormUtility.ScaleDpiX(28), FormUtility.ScaleDpiY(2));
            }
        }

        protected Rectangle MessageBounds
        {
            get
            {
                Rectangle rectangle = base.ClientRectangle.Pad(0, 0, FormUtility.ScaleDpiX(16), FormUtility.ScaleDpiY(18));
                return new Rectangle(rectangle.Right - FormUtility.ScaleDpiX(204), rectangle.Bottom - FormUtility.ScaleDpiY(52) - (messageLines - 1) * Font.Height, FormUtility.ScaleDpiX(200), messageLines * Font.Height);
            }
        }

        public Splash()
        {
            InitializeComponent();
            base.Surface = Resources.Splash.ScaleDpi();
        }

        protected override void OnLoad(EventArgs e)
        {
            Font = new Font(Font.FontFamily, FormUtility.ScaleDpiY(11), GraphicsUnit.Pixel);
            base.Alpha = 0;
            Show();
            if (Fade)
            {
                ThreadUtility.Animate(0, 250, delegate (float f)
                {
                    base.Alpha = (int)(f * 255f);
                });
            }
            Initialized.Set();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Fade)
            {
                ThreadUtility.Animate(0, 250, delegate (float f)
                {
                    base.Alpha = 255 - (int)(f * 255f);
                });
            }
            base.OnClosing(e);
        }

        protected override void OnClick(EventArgs e)
        {
            Close();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (crashSequence)
            {
                case 0:
                    if (e.KeyCode == Keys.C)
                    {
                        crashSequence++;
                        return;
                    }
                    break;
                case 1:
                    if (e.KeyCode == Keys.R)
                    {
                        crashSequence++;
                        return;
                    }
                    break;
                case 2:
                    if (e.KeyCode == Keys.A)
                    {
                        crashSequence++;
                        return;
                    }
                    break;
                case 3:
                    if (e.KeyCode == Keys.S)
                    {
                        crashSequence++;
                        return;
                    }
                    break;
                case 4:
                    if (e.KeyCode == Keys.H)
                    {
                        throw new InvalidOperationException("CRASH!");
                    }
                    break;
            }
            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rectangle = base.ClientRectangle.Pad(0, 0, FormUtility.ScaleDpiX(13), FormUtility.ScaleDpiY(17));
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            AssemblyCopyrightAttribute assemblyCopyrightAttribute = Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
            string str = assemblyCopyrightAttribute.Copyright + "\n";
            str = $"{str}V {Application.ProductVersion}{GitVersion.GetCurrentVersionInfo()}";
            str += $" {Marshal.SizeOf(typeof(IntPtr)) * 8} bit";
            Size size = e.Graphics.MeasureString(str, Font).ToSize();
            using (StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Far
            })
            {
                e.Graphics.DrawString(str, Font, Brushes.White, rectangle.Width - FormUtility.ScaleDpiX(8), rectangle.Height - size.Height - FormUtility.ScaleDpiY(6), stringFormat);
                using (Brush brush = new SolidBrush(progressColor))
                {
                    Rectangle progressBounds = ProgressBounds;
                    progressBounds.Width = progress * (rectangle.Width - FormUtility.ScaleDpiX(4)) / 100;
                    e.Graphics.FillRectangle(brush, progressBounds);
                }
                if (!string.IsNullOrEmpty(message))
                {
                    int num = 128;
                    int num2 = num / messageLines;
                    Rectangle messageBounds = MessageBounds;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    string[] array = message.Split('\n').Reverse().Take(messageLines)
                        .ToArray();
                    foreach (string s in array)
                    {
                        using (Brush brush2 = new SolidBrush(Color.FromArgb(num, Color.Black)))
                        {
                            e.Graphics.DrawString(s, Font, brush2, messageBounds, stringFormat);
                        }
                        messageBounds.Height -= Font.Height;
                        num -= num2;
                    }
                }
            }
        }
    }
}
