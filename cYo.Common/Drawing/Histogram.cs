using System;
using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Drawing
{
	public class Histogram
	{
		private readonly int size;

		private readonly Color white = Color.Empty;

		private readonly int[] reds;

		private readonly int[] greens;

		private readonly int[] blues;

		private readonly int[] grays;

		private readonly float[] redsNormalized;

		private readonly float[] greensNormalized;

		private readonly float[] bluesNormalized;

		private readonly float[] graysNormalized;

		private const float defaultThreshold = 0.005f;

		private const float range = 0.25f;

		private static readonly Histogram empty = new Histogram(new int[1], new int[1], new int[1], new int[1], 1);

		public int Size => size;

		public Color White => white;

		public int[] Reds => reds;

		public int[] Greens => greens;

		public int[] Blues => blues;

		public int[] Grays => grays;

		public float[] RedsNormalized => redsNormalized;

		public float[] GreensNormalized => greensNormalized;

		public float[] BluesNormalized => bluesNormalized;

		public float[] GraysNormalized => graysNormalized;

		public static Histogram Empty => empty;

		public Histogram(int[] reds, int[] greens, int[] blues, int[] grays, int pixelCount)
		{
			size = reds.Length;
			if (greens.Length != size || blues.Length != size || grays.Length != size)
			{
				throw new ArgumentException("Arrays must have all the same length");
			}
			this.reds = reds;
			this.greens = greens;
			this.blues = blues;
			this.grays = grays;
			redsNormalized = Normalize(reds, pixelCount);
			greensNormalized = Normalize(greens, pixelCount);
			bluesNormalized = Normalize(blues, pixelCount);
			graysNormalized = Normalize(grays, pixelCount);
		}

		public float GetBlackPointNormalized(float threshold = defaultThreshold)
		{
			return Math.Min(FindLowThreshold(graysNormalized, threshold), range);
		}

		public float GetWhitePointNormalized(float threshold = defaultThreshold)
		{
			return Math.Max(FindTopThreshold(graysNormalized, threshold), 3 * range);
		}

		private static float FindLowThreshold(IList<float> array, float threshold)
		{
			float num = 0f;
			for (int i = 0; i < array.Count; i++)
			{
				num += array[i];
				if (num >= threshold)
				{
					return (float)i / (float)array.Count;
				}
			}
			return 0f;
		}

		private static float FindTopThreshold(IList<float> array, float threshold)
		{
			float num = 0f;
			for (int num2 = array.Count - 1; num2 >= 0; num2--)
			{
				num += array[num2];
				if (num >= threshold)
				{
					return (float)num2 / (float)array.Count;
				}
			}
			return 1f;
		}

		private static float[] Normalize(IList<int> array, float factor)
		{
			float[] array2 = new float[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				array2[i] = (float)array[i] / factor;
			}
			return array2;
		}

		private static float[] Normalize(IList<float> array, float factor)
		{
			float[] array2 = new float[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				array2[i] = array[i] / factor;
			}
			return array2;
		}
	}
}
