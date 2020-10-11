using System;

namespace cYo.Common.Presentation.Panels
{
	public class PanelRenderEventArgs : EventArgs
	{
		private readonly IBitmapRenderer renderer;

		public IBitmapRenderer Renderer => renderer;

		public PanelRenderEventArgs(IBitmapRenderer renderer)
		{
			this.renderer = renderer;
		}
	}
}
