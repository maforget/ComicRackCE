using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Display;

namespace cYo.Projects.ComicRack.Engine.Controls
{
	public partial class MagnifySetupControl : UserControlEx
	{
		private TrackBarLite tbWidth;
		private TrackBarLite tbHeight;
		private TrackBarLite tbOpaque;
		private TrackBarLite tbZoom;

		public int MagnifyWidth
		{
			get
			{
				return tbWidth.Value;
			}
			set
			{
				tbWidth.Value = value;
			}
		}

		public int MagnifyHeight
		{
			get
			{
				return tbHeight.Value;
			}
			set
			{
				tbHeight.Value = value;
			}
		}

		public float MagnifyOpaque
		{
			get
			{
				return (float)tbOpaque.Value / 100f;
			}
			set
			{
				tbOpaque.Value = (int)(value * 100f);
			}
		}

		public float MagnifyZoom
		{
			get
			{
				return (float)tbZoom.Value / 100f;
			}
			set
			{
				tbZoom.Value = (int)(value * 100f);
			}
		}

		public Size MagnifySize
		{
			get
			{
				return new Size(MagnifyWidth, MagnifyHeight);
			}
			set
			{
				MagnifyWidth = value.Width;
				MagnifyHeight = value.Height;
			}
		}

		public MagnifierStyle MagnifyStyle
		{
			get
			{
				if (!chkSimpleStyle.Checked)
				{
					return MagnifierStyle.Glass;
				}
				return MagnifierStyle.Simple;
			}
			set
			{
				chkSimpleStyle.Checked = value == MagnifierStyle.Simple;
			}
		}

		public bool AutoHideMagnifier
		{
			get
			{
				return chkAutoHideMagnifier.Checked;
			}
			set
			{
				chkAutoHideMagnifier.Checked = value;
			}
		}

		public bool AutoMagnifier
		{
			get
			{
				return chkAutoMagnifier.Checked;
			}
			set
			{
				chkAutoMagnifier.Checked = value;
			}
		}

		public event EventHandler ValuesChanged;

		public MagnifySetupControl()
		{
			InitializeComponent();
			LocalizeUtility.Localize(this, components);
		}

		private void ControlValuesChanged(object sender, EventArgs e)
		{
			OnValuesChanged();
		}

		private void OnValuesChanged()
		{
			if (this.ValuesChanged != null)
			{
				this.ValuesChanged(this, EventArgs.Empty);
			}
		}
	}
}
