using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace cYo.Common.Presentation
{
	public class ControlGdiRenderer : BitmapGdiRenderer, IControlRenderer, IBitmapRenderer, IDisposable
	{
		public Control Control
		{
			get;
			private set;
		}

		public Size Size => Control.Size;

		public event EventHandler Paint;

		public ControlGdiRenderer(Control control, bool doubleBuffer)
		{
			if (doubleBuffer)
			{
				typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(control, true, null);
			}
			Control = control;
			Control.Paint += window_Paint;
		}

		private void window_Paint(object sender, PaintEventArgs e)
		{
			BeginScene(e.Graphics);
			OnPaint();
			EndScene();
		}

		public void Dispose()
		{
			Control.Paint -= window_Paint;
			GC.SuppressFinalize(this);
		}

		public void Draw()
		{
			Control.Invalidate();
		}

		protected virtual void OnPaint()
		{
			if (this.Paint != null)
			{
				this.Paint(this, EventArgs.Empty);
			}
		}
	}
}
