using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class AutoRepeatButton : Button
	{
		private bool supressFinalClick;

		private bool finalClick;

		private int currentTime;

		private Timer timer;

		private int repeatTime = 250;

		private int speedUp = 10;

		private bool repeatEnabled = true;

		[DefaultValue(250)]
		public int RepeatTime
		{
			get
			{
				return repeatTime;
			}
			set
			{
				repeatTime = value;
			}
		}

		[DefaultValue(10)]
		public int SpeedUp
		{
			get
			{
				return speedUp;
			}
			set
			{
				speedUp = value;
			}
		}

		[DefaultValue(true)]
		public bool RepeatEnabled
		{
			get
			{
				return repeatEnabled;
			}
			set
			{
				repeatEnabled = value;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && timer != null)
			{
				timer.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			base.OnMouseDown(mevent);
			if ((mevent.Button & MouseButtons.Left) != 0)
			{
				currentTime = repeatTime;
				InitTimer(currentTime);
			}
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			base.OnMouseUp(mevent);
			InitTimer(0);
			if (supressFinalClick)
			{
				finalClick = false;
			}
		}

		protected override void OnClick(EventArgs e)
		{
			if (finalClick)
			{
				base.OnClick(e);
			}
			finalClick = true;
		}

		private void InitTimer(int repeatTime)
		{
			if (timer == null)
			{
				timer = new Timer();
				timer.Tick += timer_Tick;
			}
			finalClick = true;
			supressFinalClick = false;
			timer.Stop();
			if (repeatEnabled && repeatTime != 0)
			{
				timer.Interval = repeatTime;
				timer.Start();
			}
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			OnClick(e);
			currentTime -= speedUp;
			if (currentTime < 50)
			{
				currentTime = 50;
			}
			InitTimer(currentTime);
			supressFinalClick = true;
		}
	}
}
