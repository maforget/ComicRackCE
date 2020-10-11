using System.Drawing;
using cYo.Common.Mathematics;

namespace cYo.Common.Drawing
{
	public struct ColorF
	{
		public float A;

		public float R;

		public float G;

		public float B;

		public ColorF(float a, float r, float g, float b)
		{
			A = a;
			R = r;
			G = g;
			B = b;
		}

		public ColorF(float g)
			: this(1f, g, g, g)
		{
		}

		public ColorF(Color color)
		{
			A = (float)(int)color.A / 255f;
			R = (float)(int)color.R / 255f;
			G = (float)(int)color.G / 255f;
			B = (float)(int)color.B / 255f;
		}

		public Color ToColor()
		{
			return Color.FromArgb((byte)(A * 255f), (byte)(R * 255f), (byte)(G * 255f), (byte)(B * 255f));
		}

		public void Clamp()
		{
			A = ClampOne(A);
			R = ClampOne(R);
			G = ClampOne(G);
			B = ClampOne(B);
		}

		public static float ClampOne(float f)
		{
			return f.Clamp(0f, 1f);
		}

		public static ColorF Multiply(ColorF a, ColorF b)
		{
			return new ColorF(a.A * b.A, a.R * b.R, a.G * b.G, a.B * b.B);
		}

		public static ColorF Multiply(ColorF a, float b)
		{
			return new ColorF(a.A, a.R * b, a.G * b, a.B * b);
		}

		public static ColorF Add(ColorF a, ColorF b)
		{
			return new ColorF(a.A + b.A, a.R + b.R, a.G + b.G, a.B + b.B);
		}

		public static ColorF operator *(ColorF a, ColorF b)
		{
			return Multiply(a, b);
		}

		public static ColorF operator *(ColorF a, float b)
		{
			return Multiply(a, b);
		}

		public static ColorF operator +(ColorF a, ColorF b)
		{
			return Add(a, b);
		}

		public static implicit operator Color(ColorF color)
		{
			return color.ToColor();
		}

		public static implicit operator ColorF(Color color)
		{
			return new ColorF(color);
		}
	}
}
