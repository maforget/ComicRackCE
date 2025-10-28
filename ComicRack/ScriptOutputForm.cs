using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer
{
	public partial class ScriptOutputForm : FormEx
	{
		public ScriptOutputForm()
		{
			InitializeComponent();
		}

        private Rectangle safeBounds;

        public Rectangle SafeBounds
        {
            get
            {
                return safeBounds;
            }
            set
            {
                base.StartPosition = FormStartPosition.Manual;
                base.Bounds = value;
                safeBounds = value;
            }
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            UpdateSafeBounds();
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            UpdateSafeBounds();
        }

        private void UpdateSafeBounds()
        {
            if (base.IsHandleCreated && base.WindowState == FormWindowState.Normal && base.FormBorderStyle != 0)
            {
                safeBounds = base.Bounds;
            }
        }
    }
}
