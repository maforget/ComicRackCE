using System;
using System.ComponentModel;

namespace cYo.Common.Mathematics
{
	[TypeConverter(typeof(Vector3Converter))]
	public struct Vector3
	{
		private float x;

		private float y;

		private float z;

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

		public static Vector3 Zero => default(Vector3);

		public static Vector3 LookAt => new Vector3(0f, 0f, 1f);

		public static Vector3 Up => new Vector3(0f, 1f, 0f);

		public static Vector3 Right => new Vector3(1f, 0f, 0f);

		public Vector2 Vector2 => new Vector2(x, y);

		public Vector4 Vector4 => new Vector4(x, y, z, 0f);

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
				default:
					throw new IndexOutOfRangeException("Invalid vector index!");
				}
			}
		}

		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3(Vector2 vec2)
		{
			x = vec2.X;
			y = vec2.Y;
			z = 0f;
		}

		public Vector3(Vector2 vec2, float z)
		{
			x = vec2.X;
			y = vec2.Y;
			this.z = z;
		}

		public static Vector3 From(byte[] bytes)
		{
			return From(bytes, 0);
		}

		public unsafe static Vector3 From(byte[] bytes, int offset)
		{
			fixed (byte* ptr = &bytes[offset])
			{
				float* ptr2 = (float*)ptr;
				return new Vector3(*ptr2, ptr2[1], ptr2[2]);
			}
		}

		public void AddSkinned(Vector3 sourceVec, ref Matrix4 joint, float weight)
		{
			x += (sourceVec.x * joint.A1 + sourceVec.y * joint.B1 + sourceVec.z * joint.C1 + joint.D1) * weight;
			y += (sourceVec.x * joint.A2 + sourceVec.y * joint.B2 + sourceVec.z * joint.C2 + joint.D2) * weight;
			z += (sourceVec.x * joint.A3 + sourceVec.y * joint.B3 + sourceVec.z * joint.C3 + joint.D3) * weight;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Vector3 vector = (Vector3)obj;
			if (vector.x == x && vector.y == y)
			{
				return vector.z == z;
			}
			return false;
		}

		public override string ToString()
		{
			return "X: " + x + " Y: " + y + " Z: " + z;
		}

		public static Vector3 Unit(Vector3 a)
		{
			float scalar = 1f / a.Length();
			return a * scalar;
		}

		public void Normalize()
		{
			float num = 1f / Length();
			x *= num;
			y *= num;
			z *= num;
		}

		public float Length()
		{
			return Numeric.Sqrt(this * this);
		}

		public float LengthSquared()
		{
			return this * this;
		}

		public static Vector3 Cross(Vector3 a, Vector3 b)
		{
			return new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
		}

		public static Vector3 CrossUnit(Vector3 a, Vector3 b)
		{
			Vector3 result = new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
			float num = Numeric.InvSqrt(result.x * result.x + result.y * result.y * result.z * result.z);
			result.x *= num;
			result.y *= num;
			result.z *= num;
			return result;
		}

		public static Vector3 FromArray(float[] vec)
		{
			return new Vector3(vec[0], vec[1], vec[2]);
		}

		public static Vector3 Lerp(Vector3 a, Vector3 b, float time)
		{
			return new Vector3(a.x + (b.x - a.x) * time, a.y + (b.y - a.y) * time, a.z + (b.z - a.z) * time);
		}

		public void SetZero()
		{
			x = 0f;
			y = 0f;
			z = 0f;
		}

		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public void Add(Vector3 a)
		{
			x += a.x;
			y += a.y;
			z += a.z;
		}

		public void AddWeighted(Vector3 a, float scalar)
		{
			x += a.x * scalar;
			y += a.y * scalar;
			z += a.z * scalar;
		}

		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static Vector3 operator -(Vector3 a)
		{
			return new Vector3(0f - a.x, 0f - a.y, 0f - a.z);
		}

		public static Vector3 operator /(Vector3 vec, float divisor)
		{
			return new Vector3(vec.x / divisor, vec.y / divisor, vec.z / divisor);
		}

		public static float operator *(Vector3 a, Vector3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		public static float Dot(Vector3 a, Vector3 b)
		{
			return a * b;
		}

		public static Vector3 operator *(Vector3 v, Matrix4 m)
		{
			return new Vector3(v.x * m.A1 + v.y * m.B1 + v.z * m.C1 + m.D1, v.x * m.A2 + v.y * m.B2 + v.z * m.C2 + m.D2, v.x * m.A3 + v.y * m.B3 + v.z * m.C3 + m.D3);
		}

		public void Mul(ref Matrix4 m)
		{
			float num = x;
			float num2 = y;
			float num3 = z;
			x = num * m.A1 + num2 * m.B1 + num3 * m.C1 + m.D1;
			y = num * m.A2 + num2 * m.B2 + num3 * m.C2 + m.D2;
			z = num * m.A3 + num2 * m.B3 + num3 * m.C3 + m.D3;
		}

		public static Vector3 operator *(Vector3 v, Matrix3 m)
		{
			return new Vector3(v.x * m.A1 + v.y * m.B1 + v.z * m.C1, v.x * m.A2 + v.y * m.B2 + v.z * m.C2, v.x * m.A3 + v.y * m.B3 + v.z * m.C3);
		}

		public static Vector3 Scale(Vector3 a, Vector3 b)
		{
			return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public static bool AllLess(Vector3 a, Vector3 b)
		{
			if (a.x < b.x && a.y < b.y)
			{
				return a.z < b.z;
			}
			return false;
		}

		public static bool AllLessOrEqual(Vector3 a, Vector3 b)
		{
			if (a.x <= b.x && a.y <= b.y)
			{
				return a.z <= b.z;
			}
			return false;
		}

		public static bool OneLess(Vector3 a, Vector3 b)
		{
			if (!(a.x < b.x) && !(a.y < b.y))
			{
				return a.z < b.z;
			}
			return true;
		}

		public static bool OneLessOrEqual(Vector3 a, Vector3 b)
		{
			if (!(a.x <= b.x) && !(a.y <= b.y))
			{
				return a.z <= b.z;
			}
			return true;
		}

		public static Vector3 operator *(Vector3 vec, float scalar)
		{
			return new Vector3(vec.x * scalar, vec.y * scalar, vec.z * scalar);
		}

		public void Mul(float scalar)
		{
			x *= scalar;
			y *= scalar;
			z *= scalar;
		}

		public static Vector3 operator *(float scalar, Vector3 vec)
		{
			return new Vector3(vec.x * scalar, vec.y * scalar, vec.z * scalar);
		}

		public static bool operator ==(Vector3 vec, Vector3 vec2)
		{
			if (vec.x == vec2.x && vec.y == vec2.y)
			{
				return vec.z == vec2.z;
			}
			return false;
		}

		public static bool operator !=(Vector3 vec, Vector3 vec2)
		{
			if (vec.x == vec2.x && vec.y == vec2.y)
			{
				return vec.z != vec2.z;
			}
			return true;
		}
	}
}
