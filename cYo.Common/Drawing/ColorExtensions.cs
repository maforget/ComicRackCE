using System;
using System.Collections.Generic;
using System.Drawing;
using cYo.Common.Mathematics;

namespace cYo.Common.Drawing
{
	public static class ColorExtensions
	{
		public static Color GetAverage(this IEnumerable<Color> colors)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (Color color in colors)
			{
				num2 += color.R;
				num3 += color.G;
				num4 += color.B;
				num++;
			}
			if (num == 0)
			{
				throw new ArgumentException("must be no empty list", "colors");
			}
			return Color.FromArgb(num2 / num, num3 / num, num4 / num);
		}

		public static string IsNamedColor(string color)
		{
			Color color2 = Color.FromName(color);
			if (string.IsNullOrEmpty(color2.Name))
			{
				throw new ArgumentException("Only named colors allowed");
			}
			return color2.Name;
		}

		public static int ToRgb(this Color color)
		{
			return color.ToArgb() & 0xFFFFFF;
		}

		public static bool IsBlackOrWhite(this Color color)
		{
			int num = color.ToRgb();
			if (num != Color.White.ToRgb())
			{
				return num == Color.Black.ToRgb();
			}
			return true;
		}

		public static Color Transparent(this Color color, int alpha)
		{
			return Color.FromArgb(alpha, color);
		}

		public static Color Brightness(this Color color, float f)
		{
			int red = ((int)((float)(int)color.R * f)).Clamp(0, 255);
			int green = ((int)((float)(int)color.G * f)).Clamp(0, 255);
			int blue = ((int)((float)(int)color.B * f)).Clamp(0, 255);
			return Color.FromArgb(red, green, blue);
		}
	}
}
