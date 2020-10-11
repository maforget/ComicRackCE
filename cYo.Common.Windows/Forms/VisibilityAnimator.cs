using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Win32;

namespace cYo.Common.Windows.Forms
{
	public class VisibilityAnimator : Component
	{
		private class AnimationInfo
		{
			public Control Control
			{
				get;
				set;
			}

			public int Height
			{
				get;
				set;
			}

			public bool AutoSize
			{
				get;
				set;
			}

			public Size MinimumSize
			{
				get;
				set;
			}
		}

		private readonly Timer timer = new Timer();

		private AnimationInfo[] animations;

		private long animationStart;

		private bool? pendingVisible;

		public bool Visible
		{
			get
			{
				if (pendingVisible.HasValue)
				{
					return pendingVisible.Value;
				}
				return GetVisibility();
			}
			set
			{
				if (value != Visible)
				{
					StartAnimation(value);
				}
			}
		}

		public bool Enabled
		{
			get;
			set;
		}

		public List<Control> Controls
		{
			get;
			private set;
		}

		private static long Ticks => DateTime.Now.Ticks / 10000;

		public static bool EnableAnimation
		{
			get;
			set;
		}

		public static int AnimationDuration
		{
			get;
			set;
		}

		static VisibilityAnimator()
		{
			EnableAnimation = true;
			AnimationDuration = 100;
		}

		public VisibilityAnimator()
		{
			Controls = new List<Control>();
			Enabled = true;
			timer.Tick += AnimationEvent;
		}

		public VisibilityAnimator(IContainer container, Control control = null, int duration = 0)
			: this()
		{
			container.Add(this);
			if (control != null)
			{
				Controls.Add(control);
			}
			if (duration != 0)
			{
				AnimationDuration = duration;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				timer.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool GetVisibility()
		{
			return Controls.Any((Control t) => t.IsVisibleSet());
		}

		private void StartAnimation(bool targetVisibility)
		{
			if (!Enabled || !EnableAnimation || !IsContainerVisible())
			{
				CleanUp();
				Controls.ForEach(delegate(Control c)
				{
					c.Visible = targetVisibility;
				});
				return;
			}
			animationStart = 0L;
			animations = Controls.Select((Control t) => new AnimationInfo
			{
				Control = t,
				Height = t.Height,
				AutoSize = t.AutoSize,
				MinimumSize = t.MinimumSize
			}).ToArray();
			pendingVisible = targetVisibility;
			timer.Start();
		}

		private bool IsContainerVisible()
		{
			Control control = base.Container as Control;
			if (control != null && control.TopLevelControl != null)
			{
				return control.TopLevelControl.Visible;
			}
			return true;
		}

		private void SetHeight(Control control, int h)
		{
			control.SuspendLayout();
			control.AutoSize = false;
			control.MinimumSize = Size.Empty;
			control.Height = h;
			control.Visible = true;
			control.ResumeLayout();
		}

		private void CleanUp()
		{
			if (!pendingVisible.HasValue)
			{
				return;
			}
			AnimationInfo[] array = animations;
			foreach (AnimationInfo animationInfo in array)
			{
				animationInfo.Control.SuspendLayout();
				if (!pendingVisible.Value)
				{
					animationInfo.Control.Visible = false;
				}
				animationInfo.Control.AutoSize = animationInfo.AutoSize;
				animationInfo.Control.Height = animationInfo.Height;
				animationInfo.Control.MinimumSize = animationInfo.MinimumSize;
				if (pendingVisible.Value)
				{
					animationInfo.Control.Visible = true;
				}
				animationInfo.Control.ResumeLayout();
			}
			pendingVisible = null;
		}

		private void AnimationEvent(object sender, EventArgs e)
		{
			if (!pendingVisible.HasValue)
			{
				timer.Stop();
				return;
			}
			if (animationStart == 0L)
			{
				animationStart = Ticks;
			}
			float num = (float)(Ticks - animationStart) / (float)AnimationDuration;
			float val = num;
			if (!pendingVisible.Value)
			{
				val = 1f - num;
			}
			val = Math.Max(Math.Min(1f, val), 0f);
			AnimationInfo[] array = animations;
			foreach (AnimationInfo animationInfo in array)
			{
				SetHeight(animationInfo.Control, (int)((float)animationInfo.Height * val));
			}
			if (num >= 1f)
			{
				CleanUp();
			}
		}
	}
}
