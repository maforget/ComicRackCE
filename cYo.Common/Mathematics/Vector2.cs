using System;
using System.ComponentModel;
using System.Drawing;

namespace cYo.Common.Mathematics
{
	[TypeConverter(typeof(Vector2Converter))]
	public struct Vector2
	{
		private float x;

		private float y;

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

		public static Vector2 Zero => default(Vector2);

		public static Vector2 One => new Vector2(1f, 1f);

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
				default:
					throw new IndexOutOfRangeException("Invalid vector index!");
				}
			}
		}

		public Vector2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public static Vector2 From(byte[] bytes)
		{
			return From(bytes, 0);
		}

		public unsafe static Vector2 From(byte[] bytes, int offset)
		{
			fixed (byte* ptr = &bytes[offset])
			{
				float* ptr2 = (float*)ptr;
				return new Vector2(*ptr2, ptr2[1]);
			}
		}

		public override string ToString()
		{
			return "X: " + x + " Y: " + y;
		}

		public static Vector2 Unit(Vector2 a)
		{
			return a / a.Length();
		}

		public static Vector2 Ortho(Vector2 a)
		{
			return new Vector2(a.Y, 0f - a.X);
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

		public static Vector2 FromArray(float[] vec)
		{
			return new Vector2(vec[0], vec[1]);
		}

		public static Vector2 Lerp(Vector2 a, Vector2 b, float time)
		{
			return new Vector2(a.x + (b.x - a.x) * time, a.y + (b.y - a.y) * time);
		}

		public static Vector2 operator +(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x + b.x, a.y + b.y);
		}

		public static Vector2 operator -(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x - b.x, a.y - b.y);
		}

		public static Vector2 operator -(Vector2 a)
		{
			return new Vector2(0f - a.x, 0f - a.y);
		}

		public static Vector2 operator /(Vector2 vec, float divisor)
		{
			return new Vector2(vec.x / divisor, vec.y / divisor);
		}

		public static float operator *(Vector2 a, Vector2 b)
		{
			return a.x * b.x + a.y * b.y;
		}

		public static float Dot(Vector2 a, Vector2 b)
		{
			return a * b;
		}

		public static Vector2 operator *(Vector2 vec, float scalar)
		{
			return new Vector2(vec.x * scalar, vec.y * scalar);
		}

		public Vector3 ToVector3()
		{
			return new Vector3(this);
		}

		public static Vector2 Rotate(Vector2 vec, float angle)
		{
			float num = Numeric.Cos(angle);
			float num2 = Numeric.Sin(angle);
			return new Vector2(vec.x * num + vec.y * num2, (0f - vec.x) * num2 + vec.y * num);
		}

		public static Vector2 MultiplyElements(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x * b.x, a.y * b.y);
		}

		public static Vector2 DivideElements(Vector2 a, Vector2 b)
		{
			return new Vector2(a.x / b.x, a.y / b.y);
		}

		public static bool operator ==(Vector2 a, Vector2 b)
		{
			if (a.x == b.x)
			{
				return a.y == b.y;
			}
			return false;
		}

		public static bool operator !=(Vector2 a, Vector2 b)
		{
			if (a.x == b.x)
			{
				return a.y != b.y;
			}
			return true;
		}

		public static Vector2 Max(Vector2 a, Vector2 b)
		{
			return new Vector2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
		}

		public static explicit operator Vector2(Size size)
		{
			return new Vector2(size.Width, size.Height);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Vector2 vector = (Vector2)obj;
			if (vector.x == x)
			{
				return vector.y == y;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode();
		}
	}
}
