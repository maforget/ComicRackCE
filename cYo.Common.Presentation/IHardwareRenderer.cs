using System.Drawing;

namespace cYo.Common.Presentation
{
	public interface IHardwareRenderer : IBitmapRenderer
	{
		bool IsSoftwareRenderer
		{
			get;
		}

		StencilMode StencilMode
		{
			get;
			set;
		}

		bool OptimizedTextures
		{
			get;
			set;
		}

		bool EnableFilter
		{
			get;
			set;
		}

		BlendingOperation BlendingOperation
		{
			get;
			set;
		}

		Bitmap GetFramebuffer(Rectangle rc, bool flip);

		void ClearStencil();
	}
}
