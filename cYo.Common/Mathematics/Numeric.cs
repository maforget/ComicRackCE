using System;
using System.Drawing;
using System.Linq;

namespace cYo.Common.Mathematics
{
	public static class Numeric
	{
		public static float PI = (float)Math.PI;

		public static float FloatEpsilon = float.MinValue;

		public static double DoubleEpsilon = double.MinValue;

		public static float Sqrt(float value)
		{
			return (float)Math.Sqrt(value);
		}

		public static float InvSqrt(float x)
		{
			return 1f / Sqrt(x);
		}

		public static float Abs(float value)
		{
			return Math.Abs(value);
		}

		public static float Log(double a)
		{
			return (float)Math.Log(a);
		}

		public static float Log(double a, double newBase)
		{
			return (float)Math.Log(a, newBase);
		}

		public static float Log10(double a)
		{
			return (float)Math.Log10(a);
		}

		public static float Exp(double power)
		{
			return (float)Math.Exp(power);
		}

		public static float Pow(double a, double power)
		{
			return (float)Math.Pow(a, power);
		}

		public static float Cos(float angle)
		{
			return (float)Math.Cos(angle);
		}

		public static float Acos(float number)
		{
			return (float)Math.Acos(number);
		}

		public static float Sin(float angle)
		{
			return (float)Math.Sin(angle);
		}

		public static float Cot(float angle)
		{
			return Cos(angle) / Sin(angle);
		}

		public static float DegToRad(float angle)
		{
			return angle * (PI / 180f);
		}

		public static float RadToDeg(float angle)
		{
			return angle * (180f / PI);
		}

		public static double DegToRad(double angle)
		{
			return angle * (Math.PI / 180.0);
		}

		public static double RadToDeg(double angle)
		{
			return angle * (180.0 / Math.PI);
		}

		public static int Clamp(this int value, int min, int max, int minValue, int maxValue = int.MaxValue)
		{
			if (maxValue == int.MaxValue)
			{
				maxValue = max;
			}
			if (value < min)
			{
				return minValue;
			}
			if (value > max)
			{
				return maxValue;
			}
			return value;
		}

		public static int Clamp(this int value, int min, int max)
		{
			return Math.Max(Math.Min(value, max), min);
		}

		public static float Clamp(this float value, float min, float max)
		{
			return Math.Max(Math.Min(value, max), min);
		}

		public static double Clamp(this double value, double min, double max)
		{
			return Math.Max(Math.Min(value, max), min);
		}

		public static Size Clamp(this Size value, Size min, Size max)
		{
			return new Size(value.Width.Clamp(min.Width, max.Width), value.Height.Clamp(max.Height, max.Width));
		}

		public static bool CompareTo(this float f, float t, float limit)
		{
			return Math.Abs(f - t) < limit;
		}

		public static bool CompareTo(this int f, int t, int limit)
		{
			return Math.Abs(f - t) < limit;
		}

		public static bool CompareTo(this Size f, Size t, int limit)
		{
			if (f.Width.CompareTo(t.Width, limit))
			{
				return f.Height.CompareTo(t.Height, limit);
			}
			return false;
		}

		public static bool Equals(float a, float b)
		{
			if (a > b - FloatEpsilon)
			{
				return a < b + FloatEpsilon;
			}
			return false;
		}

		public static bool Equals(double a, double b)
		{
			if (a > b - DoubleEpsilon)
			{
				return a < b + DoubleEpsilon;
			}
			return false;
		}

		public static int Rollover(int n, int count, int add)
		{
			add = add.Clamp(-count + 1, count - 1);
			n += add;
			if (n < 0)
			{
				n = count + n;
			}
			n %= count;
			return n;
		}

		public static int Select(int n, int[] values, bool wrap)
		{
			int num = values.Length;
			if (n > values[num - 1])
			{
				if (wrap)
				{
					return values[0];
				}
				return values[num - 1];
			}
			if (n < values[0])
			{
				if (wrap)
				{
					return values[num - 1];
				}
				return values[0];
			}
			foreach (int num2 in values)
			{
				if (n < num2)
				{
					return num2;
				}
			}
			return values[0];
		}

		public static float Select(float n, float[] values, bool wrap)
		{
			int num = values.Length;
			if (n > values[num - 1])
			{
				if (wrap)
				{
					return values[0];
				}
				return values[num - 1];
			}
			if (n < values[0])
			{
				if (wrap)
				{
					return values[num - 1];
				}
				return values[0];
			}
			foreach (float num2 in values)
			{
				if (n < num2)
				{
					return num2;
				}
			}
			return values[0];
		}

		public static bool InRange(int n, int min, int count)
		{
			if (n >= min)
			{
				return n <= min + count;
			}
			return false;
		}

		public static float Min(params float[] n)
		{
			return n.Min();
		}

		public static float Max(params float[] n)
		{
			return n.Max();
		}

		public static int BinaryHash(params bool[] flags)
		{
			int num = 0;
			foreach (bool item in flags.Reverse())
			{
				num <<= 1;
				num += (item ? 1 : 0);
			}
			return num;
		}

		public static int HighestBit(int n)
		{
			int num = -1;
			while (n != 0)
			{
				num++;
				n >>= 1;
			}
			return num;
		}
	}
}
