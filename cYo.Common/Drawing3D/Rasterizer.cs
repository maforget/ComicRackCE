using System;
using System.Drawing;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;

namespace cYo.Common.Drawing3D
{
	public static class Rasterizer
	{
		private struct Span
		{
			public int X1;

			public int X2;

			public float Z1;

			public float Z2;

			public float U1;

			public float U2;

			public float V1;

			public float V2;

			public float R1;

			public float G1;

			public float B1;

			public float A1;

			public float R2;

			public float G2;

			public float B2;

			public float A2;

			public static readonly Span Empty = new Span(0, 0);

			public Span(int x1, int x2, float z1, float z2, float u1, float u2, float v1, float v2, float r1, float r2, float g1, float g2, float b1, float b2, float a1, float a2)
			{
				X1 = x1;
				X2 = x2;
				U1 = u1;
				U2 = u2;
				V1 = v1;
				V2 = v2;
				Z1 = z1;
				Z2 = z2;
				R1 = r1;
				G1 = g1;
				B1 = b1;
				A1 = a1;
				R2 = r2;
				G2 = g2;
				B2 = b2;
				A2 = a2;
			}

			public Span(int start, int end)
				: this(start, end, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f)
			{
			}
		}

		public static void RasterizeLine(IFrameBuffer fb, Vertex v1, Vertex v2)
		{
			float a = v1.X;
			float a2 = v1.Y;
			float b = v2.X;
			float b2 = v2.Y;
			float a3 = v1.A;
			float b3 = v2.A;
			float a4 = v1.R;
			float b4 = v2.R;
			float a5 = v1.G;
			float b5 = v2.G;
			float a6 = v1.B;
			float b6 = v2.B;
			float a7 = v1.Z;
			float b7 = v2.Z;
			float num = b - a;
			float num2 = (b2 - a2) / num;
			bool flag = Math.Abs(num2) > 1f;
			if (flag)
			{
				CloneUtility.Swap(ref a, ref a2);
				CloneUtility.Swap(ref b, ref b2);
				num = b - a;
				num2 = (b2 - a2) / num;
			}
			if (num < 0f)
			{
				CloneUtility.Swap(ref a, ref b);
				CloneUtility.Swap(ref a2, ref b2);
				CloneUtility.Swap(ref a3, ref b3);
				CloneUtility.Swap(ref a4, ref b4);
				CloneUtility.Swap(ref a5, ref b5);
				CloneUtility.Swap(ref a6, ref b6);
				CloneUtility.Swap(ref a7, ref b7);
			}
			float num3 = (b3 - a3) / num;
			float num4 = (b4 - a4) / num;
			float num5 = (b5 - a5) / num;
			float num6 = (b6 - a6) / num;
			float num7 = (b7 - a7) / num;
			for (float num8 = a; num8 <= b; num8 += 1f)
			{
				Point pt = (flag ? new Point((int)Math.Round(a2), (int)Math.Round(num8)) : new Point((int)Math.Round(num8), (int)Math.Round(a2)));
				fb.SetColor(pt, new ColorF(a3, a4, a5, a6));
				a2 += num2;
				a3 += num3;
				a4 += num4;
				a5 += num5;
				a6 += num6;
				a7 += num7;
			}
		}

		public static void RasterizeTriangle(IFrameBuffer fb, Vertex v1, Vertex v2, Vertex v3, ITexture texture)
		{
			Size size = fb.Size;
			int num = (int)Numeric.Min(v1.Y, v2.Y, v3.Y).Clamp(0f, size.Height - 1);
			int num2 = (int)Numeric.Max(v1.Y, v2.Y, v3.Y).Clamp(0f, size.Height - 1);
			int num3 = num2 - num;
			Span[] array = new Span[num3 + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i].X1 = int.MaxValue;
				array[i].X2 = int.MinValue;
			}
			ScanEdge(v1, v2, array, num, size.Height);
			ScanEdge(v2, v3, array, num, size.Height);
			ScanEdge(v3, v1, array, num, size.Height);
			for (int j = 0; j <= num3; j++)
			{
				int num4 = array[j].X1;
				int num5 = array[j].X2;
				if (num4 >= size.Width || num5 < 0)
				{
					continue;
				}
				float num6 = array[j].Z1;
				float z = array[j].Z2;
				float num7 = array[j].U1;
				float u = array[j].U2;
				float num8 = array[j].V1;
				float v4 = array[j].V2;
				float num9 = array[j].R1;
				float r = array[j].R2;
				float num10 = array[j].G1;
				float g = array[j].G2;
				float num11 = array[j].B1;
				float b = array[j].B2;
				float num12 = array[j].A1;
				float a = array[j].A2;
				if (num4 < 0)
				{
					float num13 = (float)(-num4) / (float)(num5 - num4);
					num6 += (z - num6) * num13;
					num7 += (u - num7) * num13;
					num8 += (v4 - num8) * num13;
					num9 += (r - num9) * num13;
					num10 += (g - num10) * num13;
					num11 += (b - num11) * num13;
					num12 += (a - num12) * num13;
					num4 = 0;
				}
				float num14 = 1f / (float)(num5 - num4);
				float num15 = (u - num7) * num14;
				float num16 = (v4 - num8) * num14;
				float num17 = (z - num6) * num14;
				float num18 = (r - num9) * num14;
				float num19 = (g - num10) * num14;
				float num20 = (b - num11) * num14;
				float num21 = (a - num12) * num14;
				if (num5 >= size.Width)
				{
					num5 = size.Width - 1;
				}
				for (int k = num4; k <= num5; k++)
				{
					ColorF color = new ColorF(num12, num9, num10, num11);
					if (texture != null)
					{
						color *= (ColorF)texture.GetColor((int)(num7 / num6), (int)(num8 / num6));
					}
					fb.SetColor(k, j + num, color);
					num7 += num15;
					num8 += num16;
					num6 += num17;
					num9 += num18;
					num10 += num19;
					num11 += num20;
					num12 += num21;
				}
			}
		}

		public static void RasterizeQuad(IFrameBuffer fb, Vertex v1, Vertex v2, Vertex v3, Vertex v4, ITexture texture)
		{
			RasterizeTriangle(fb, v1, v2, v3, texture);
			RasterizeTriangle(fb, v3, v4, v1, texture);
		}

		private static void ScanEdge(Vertex v1, Vertex v2, Span[] spans, int yBase, int fbHeight)
		{
			if (v1.Y >= v2.Y && v1.Y <= v2.Y)
			{
				return;
			}
			Vertex a = new Vertex(v1);
			Vertex b = new Vertex(v2);
			if (v1.Y > v2.Y)
			{
				CloneUtility.Swap(ref a, ref b);
			}
			int num = (int)a.Y;
			int num2 = (int)b.Y;
			if (num >= fbHeight || num2 < 0)
			{
				return;
			}
			if (num2 >= fbHeight)
			{
				num2 = fbHeight - 1;
			}
			a.U /= a.Z;
			a.V /= a.Z;
			a.Z = 1f / a.Z;
			b.U /= b.Z;
			b.V /= b.Z;
			b.Z = 1f / b.Z;
			float num3 = b.Y - a.Y;
			float num4 = a.X;
			float num5 = a.U;
			float num6 = a.V;
			float num7 = a.Z;
			float num8 = a.R;
			float num9 = a.G;
			float num10 = a.B;
			float num11 = a.A;
			float num12 = (b.X - a.X) / num3;
			float num13 = (b.Z - a.Z) / num3;
			float num14 = (b.V - a.V) / num3;
			float num15 = (b.U - a.U) / num3;
			float num16 = (b.R - num8) / num3;
			float num17 = (b.G - num9) / num3;
			float num18 = (b.B - num10) / num3;
			float num19 = (b.A - num11) / num3;
			if (num < 0)
			{
				num4 += num12 * (float)(-num);
				num7 += num13 * (float)(-num);
				num5 += num15 * (float)(-num);
				num6 += num14 * (float)(-num);
				num8 += num16 * (float)(-num);
				num9 += num17 * (float)(-num);
				num10 += num18 * (float)(-num);
				num11 += num19 * (float)(-num);
				num = 0;
			}
			num -= yBase;
			num2 -= yBase;
			for (int i = num; i < num2; i++)
			{
				int num20 = (int)num4;
				if (num20 < spans[i].X1)
				{
					spans[i].X1 = num20;
					spans[i].Z1 = num7;
					spans[i].U1 = num5;
					spans[i].V1 = num6;
					spans[i].R1 = num8;
					spans[i].G1 = num9;
					spans[i].B1 = num10;
					spans[i].A1 = num11;
				}
				if (num20 > spans[i].X2)
				{
					spans[i].X2 = num20;
					spans[i].Z2 = num7;
					spans[i].U2 = num5;
					spans[i].V2 = num6;
					spans[i].R2 = num8;
					spans[i].G2 = num9;
					spans[i].B2 = num10;
					spans[i].A2 = num11;
				}
				num4 += num12;
				num7 += num13;
				num5 += num15;
				num6 += num14;
				num8 += num16;
				num9 += num17;
				num10 += num18;
				num11 += num19;
			}
		}
	}
}
