using System;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;

namespace cYo.Common.Windows.Forms
{
	public class BackgroundImageDisplayer : DisposableObject, IBitmapDisplayControl, IDisposable
	{
		private readonly Control control;

		private Bitmap image;

		public ContentAlignment Alignment
		{
			get;
			set;
		}

		public bool AutoDispose
		{
			get;
			set;
		}

		public Control Control => control;

		public float Opacity
		{
			get;
			set;
		}

		public Bitmap Bitmap
		{
			get
			{
				return image;
			}
			set
			{
				SetBitmap(value);
				if (image != null && AutoDispose)
				{
					image.Dispose();
				}
				image = value;
			}
		}

		public object Tag
		{
			get
			{
				return control.Tag;
			}
			set
			{
				control.Tag = value;
			}
		}

		public BackgroundImageDisplayer(Control c)
		{
			Opacity = 1f;
			AutoDispose = true;
			Alignment = ContentAlignment.MiddleCenter;
			control = c;
		}

		protected virtual void OnSetImage(Bitmap image)
		{
			if (control.InvokeIfRequired(delegate
			{
				OnSetImage(image);
			}))
			{
				return;
			}
			Bitmap bitmap = ((image == null) ? null : new Bitmap(control.ClientRectangle.Width, control.ClientRectangle.Height));
			if (bitmap != null)
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.Clear(control.BackColor);
					Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
					Rectangle bounds = rectangle.Align(control.ClientRectangle, Alignment);
					graphics.DrawImage(image, bounds, Opacity);
				}
				if (control.BackgroundImage != null)
				{
					control.BackgroundImage.Dispose();
				}
			}
			control.BackgroundImage = bitmap;
		}

		public void SetBitmap(Bitmap image)
		{
			OnSetImage(image);
		}

		protected override void Dispose(bool disposing)
		{
			if (control.BackgroundImage != null)
			{
				control.BackgroundImage.Dispose();
			}
			control.Dispose();
		}
	}
}
