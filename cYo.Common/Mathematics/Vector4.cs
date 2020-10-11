using System;
using System.ComponentModel;
using System.Diagnostics;

namespace cYo.Common.Mathematics
{
	[TypeConverter(typeof(Vector4Converter))]
	public struct Vector4
	{
		private float x;

		private float y;

		private float z;

		private float w;

		public float X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		public float Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}

		public float Z
		{
			get
			{
				return z;
			}
			set
			{
				z = value;
			}
		}

		public float W
		{
			get
			{
				return w;
			}
			set
			{
				w = value;
			}
		}

		[DebuggerHidden]
		public static Vector4 Zero => default(Vector4);

		public Vector3 Vector3 => new Vector3(x, y, z);

		public float this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return x;
				case 1:
					return y;
				case 2:
					return z;
				case 3:
					return w;
				default:
					throw new IndexOutOfRangeException("Invalid vector index!");
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					x = value;
					break;
				case 1:
					y = value;
					break;
				case 2:
					z = value;
					break;
				case 3:
					w = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid vector index!");
				}
			}
		}

		public Vector4(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public Vector4(Vector3 vec)
			: this(vec, 0f)
		{
		}

		public Vector4(Vector3 vec, float w)
		{
			x = vec.X;
			y = vec.Y;
			z = vec.Z;
			this.w = w;
		}

		public override string ToString()
		{
			return "X: " + x + " Y: " + y + " Z: " + z + " W: " + w;
		}

		public static Vector4 Unit(Vector4 a)
		{
			return a / a.Length();
		}

		public void Normalize()
		{
			this /= Length();
		}

		public float Length()
		{
			return Numeric.Sqrt(this * this);
		}

		public float LengthSquared()
		{
			return this * this;
		}

		public static Vector4 Lerp(Vector4 a, Vector4 b, float time)
		{
			return new Vector4(a.x + (b.x - a.x) * time, a.y + (b.y - a.y) * time, a.z + (b.z - a.z) * time, a.W + (b.W - a.W) * time);
		}

		public static Vector4 operator +(Vector4 a, Vector4 b)
		{
			return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
		}

		public static float operator *(Vector4 a, Vector4 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
		}

		public static Vector4 operator *(Vector4 vec, float scalar)
		{
			return new Vector4(vec.x * scalar, vec.y * scalar, vec.z * scalar, vec.w * scalar);
		}

		public static float Dot(Vector4 a, Vector4 b)
		{
			return a * b;
		}

		public static Vector4 operator *(Vector4 v, Matrix4 m)
		{
			return new Vector4(v.x * m.A1 + v.y * m.B1 + v.z * m.C1 + v.w * m.D1, v.x * m.A2 + v.y * m.B2 + v.z * m.C2 + v.w * m.D2, v.x * m.A3 + v.y * m.B3 + v.z * m.C3 + v.w * m.D3, v.x * m.A4 + v.y * m.B4 + v.z * m.C4 + v.w * m.D4);
		}

		public static Vector4 operator /(Vector4 vec, float divisor)
		{
			return new Vector4(vec.x / divisor, vec.y / divisor, vec.z / divisor, vec.w / divisor);
		}

		public static bool operator ==(Vector4 a, Vector4 b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Vector4 a, Vector4 b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Vector4 vector = (Vector4)obj;
			if (vector.x == x && vector.y == y && vector.z == z)
			{
				return vector.w == w;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
		}
	}
}
