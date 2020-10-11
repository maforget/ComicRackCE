using cYo.Common.Drawing;
using cYo.Common.Mathematics;

namespace cYo.Common.Drawing3D
{
	public class Vertex
	{
		public float X;

		public float Y;

		public float Z;

		public float U;

		public float V;

		public float R;

		public float G;

		public float B;

		public float A;

		public Vertex(float x, float y, float z, float u, float v, float r, float g, float b, float a)
		{
			X = x;
			Y = y;
			Z = z;
			U = u;
			V = v;
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public Vertex(float x, float y, float z, ColorF color)
			: this(x, y, z, 1f, 1f, color.R, color.G, color.B, color.A)
		{
		}

		public Vertex(float x, float y, float z, float u, float v)
			: this(x, y, z, u, v, 1f, 1f, 1f, 1f)
		{
		}

		public Vertex(float x, float y, float z)
			: this(x, y, z, 0f, 0f)
		{
		}

		public Vertex(Vertex v)
			: this(v.X, v.Y, v.Z, v.U, v.V, v.R, v.G, v.B, v.A)
		{
		}

		public ColorF ToColor()
		{
			return new ColorF(A, R, G, B);
		}

		public void SetColor(ColorF color)
		{
			A = color.A;
			R = color.R;
			G = color.G;
			B = color.B;
		}

		public static implicit operator Vector3(Vertex v)
		{
			return new Vector3(v.X, v.Y, v.Z);
		}

		public static bool operator ==(Vertex a, Vertex b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Vertex a, Vertex b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Vertex vertex = (Vertex)obj;
			if (X == vertex.X && Y == vertex.Y && Z == vertex.Z && U == vertex.U && V == vertex.V && A == vertex.A && R == vertex.R && G == vertex.G)
			{
				return B == vertex.B;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ (Z.GetHashCode() & U.GetHashCode() & V.GetHashCode());
		}
	}
}
