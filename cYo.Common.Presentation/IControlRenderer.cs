using System;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Presentation
{
	public interface IControlRenderer : IBitmapRenderer, IDisposable
	{
		Control Control
		{
			get;
		}

		Size Size
		{
			get;
		}

		event EventHandler Paint;

		void Draw();
	}
}
