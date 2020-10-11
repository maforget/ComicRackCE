using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using cYo.Common.Drawing;

namespace cYo.Common.Presentation
{
	public interface IBitmapRenderer
	{
		Matrix Transform
		{
			get;
			set;
		}

		bool HighQuality
		{
			get;
			set;
		}

		float Opacity
		{
			get;
			set;
		}

		CompositingMode CompositingMode
		{
			get;
			set;
		}

		RectangleF Clip
		{
			get;
			set;
		}

		bool IsHardware
		{
			get;
		}

		bool IsLocked
		{
			get;
		}

		void Clear(Color color);

		bool BeginScene(Graphics gr);

		void EndScene();

		void DrawImage(RendererImage image, RectangleF dest, RectangleF src, BitmapAdjustment transform, float opacity);

		void DrawBlurredImage(RendererImage image, RectangleF dest, RectangleF src, float blur);

		void FillRectangle(RectangleF bounds, Color color);

		void DrawLine(IEnumerable<PointF> points, Color color, float width);

		bool IsVisible(RectangleF bounds);

		IDisposable SaveState();

		void TranslateTransform(float dx, float dy);

		void ScaleTransform(float dx, float dy);

		void RotateTransform(float angel);
	}
}
