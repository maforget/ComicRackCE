using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Mathematics;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class ZoomDialog : FormEx
	{
		public float Zoom
		{
			get
			{
				return (float)(numPercentage.Value / 100m);
			}
			set
			{
				numPercentage.Value = (int)(value * 100f).Clamp((float)numPercentage.Minimum, (float)numPercentage.Maximum);
				numPercentage.Select(0, 100);
			}
		}

		public ZoomDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
		}

		public static float Show(IWin32Window parent, float zoom)
		{
			using (ZoomDialog zoomDialog = new ZoomDialog())
			{
				zoomDialog.Zoom = zoom;
				if (zoomDialog.ShowDialog(parent) == DialogResult.OK)
				{
					return zoomDialog.Zoom;
				}
				return zoom;
			}
		}

	}
}
