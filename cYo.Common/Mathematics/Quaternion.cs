namespace cYo.Common.Mathematics
{
	public struct Quaternion
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

		public static Quaternion Zero => new Quaternion(0f, 0f, 0f, 0f);

		public Matrix4 Matrix4
		{
			get
			{
				Matrix4 zero = Matrix4.Zero;
				zero.A1 = 1f - 2f * (y * y + z * z);
				zero.A2 = 2f * (x * y + w * z);
				zero.A3 = 2f * (x * z - w * y);
				zero.A4 = 0f;
				zero.B1 = 2f * (x * y - w * z);
				zero.B2 = 1f - 2f * (x * x + z * z);
				zero.B3 = 2f * (y * z + w * x);
				zero.B4 = 0f;
				zero.C1 = 2f * (x * z + w * y);
				zero.C2 = 2f * (y * z - w * x);
				zero.C3 = 1f - 2f * (x * x + y * y);
				zero.C4 = 0f;
				zero.D1 = 0f;
				zero.D2 = 0f;
				zero.D3 = 0f;
				zero.D4 = 1f;
				return zero;
			}
		}

		public Matrix3 Matrix
		{
			get
			{
				Matrix3 result = default(Matrix3);
				result.A1 = 1f - 2f * (y * y + z * z);
				result.A2 = 2f * (x * y + w * z);
				result.A3 = 2f * (x * z - w * y);
				result.B1 = 2f * (x * y - w * z);
				result.B2 = 1f - 2f * (x * x + z * z);
				result.B3 = 2f * (y * z + w * x);
				result.C1 = 2f * (x * z + w * y);
				result.C2 = 2f * (y * z - w * x);
				result.C3 = 1f - 2f * (x * x + y * y);
				return result;
			}
		}

		public Quaternion(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public float Dot(Quaternion b)
		{
			return x * b.x + y * b.y + z * b.z + w * b.w;
		}

		public static float Length(Quaternion q)
		{
			return Numeric.Sqrt(q.Dot(q));
		}

		public static Quaternion operator *(Quaternion q, float s)
		{
			return new Quaternion(s * q.x, s * q.y, s * q.z, s * q.w);
		}

		public static Quaternion operator *(float s, Quaternion q)
		{
			return new Quaternion(s * q.x, s * q.y, s * q.z, s * q.w);
		}

		public static Quaternion operator +(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
		}

		public static Quaternion operator -(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
		}

		public static Quaternion operator -(Quaternion a)
		{
			return new Quaternion(0f - a.x, 0f - a.y, 0f - a.z, 0f - a.w);
		}

		public static Quaternion Conjugate(Quaternion q)
		{
			return new Quaternion(0f - q.x, 0f - q.y, 0f - q.z, q.w);
		}

		public static float Norm(Quaternion q)
		{
			return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
		}

		public static Quaternion operator /(Quaternion a, float scalar)
		{
			float num = 1f / scalar;
			return new Quaternion(a.x * num, a.y + num, a.z * num, a.w * num);
		}

		public static Quaternion Inverse(Quaternion q)
		{
			return Conjugate(q) / Norm(q);
		}

		public static Quaternion operator /(Quaternion a, Quaternion b)
		{
			return a * Inverse(b);
		}

		public static Quaternion operator *(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.x * b.w + a.y * b.z - a.z * b.y + a.w * b.x, (0f - a.x) * b.z + a.y * b.w + a.z * b.x + a.w * b.y, a.x * b.y - a.y * b.x + a.z * b.w + a.w * b.z, (0f - a.x) * b.x - a.y * b.y - a.z * b.z + a.w * b.w);
		}

		public static Quaternion Normalize(Quaternion q)
		{
			float value = q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
			float num = 1f / Numeric.Sqrt(value);
			return new Quaternion(q.x * num, q.y * num, q.z * num, q.w * num);
		}

		public static Quaternion Log(Quaternion q)
		{
			float num = Numeric.Acos(q.w);
			float num2 = Numeric.Sin(num);
			if (num2 > 0f)
			{
				return new Quaternion(num * q.X / num2, num * q.Y / num2, num * q.Z / num2, 0f);
			}
			return new Quaternion(q.X, q.Y, q.Z, 0f);
		}

		public static Quaternion Exp(Quaternion q)
		{
			float num = Numeric.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z);
			float num2 = Numeric.Sin(num);
			float num3 = Numeric.Cos(num);
			if (num > 0f)
			{
				return new Quaternion(num2 * q.x / num, num2 * q.y / num, num2 * q.z / num, num3);
			}
			return new Quaternion(q.x, q.y, q.z, num3);
		}

		public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
		{
			return Normalize(a + t * (a - b));
		}

		public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
		{
			float num = a.Dot(b);
			float num2 = 1f;
			if (num < 0f)
			{
				num = 0f - num;
				num2 = -1f;
			}
			float s;
			float num5;
			if (num < 0.999f)
			{
				float num3 = Numeric.Acos(num);
				float num4 = 1f / Numeric.Sqrt(1f - num * num);
				s = Numeric.Sin((1f - t) * num3) * num4;
				num5 = Numeric.Sin(t * num3) * num4;
			}
			else
			{
				s = 1f - t;
				num5 = t;
			}
			Quaternion q = s * a + num2 * num5 * b;
			return Normalize(q);
		}

		public static Quaternion Squad(Quaternion a, Quaternion b, Quaternion ta, Quaternion tb, float t)
		{
			float t2 = 2f * t * (1f - t);
			Quaternion a2 = Slerp(a, b, t);
			Quaternion b2 = Slerp(ta, tb, t);
			return Slerp(a2, b2, t2);
		}

		public static Quaternion SimpleSquad(Quaternion prev, Quaternion a, Quaternion b, Quaternion post, float t)
		{
			if (prev.Dot(a) < 0f)
			{
				a = -a;
			}
			if (a.Dot(b) < 0f)
			{
				b = -b;
			}
			if (b.Dot(post) < 0f)
			{
				post = -post;
			}
			Quaternion ta = Spline(prev, a, b);
			Quaternion tb = Spline(a, b, post);
			return Squad(a, b, ta, tb, t);
		}

		public static Quaternion Spline(Quaternion pre, Quaternion q, Quaternion post)
		{
			Quaternion a = Conjugate(q);
			return q * Exp((Log(a * pre) + Log(a * post)) * -0.25f);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Quaternion quaternion = (Quaternion)obj;
			if (x == quaternion.x && y == quaternion.y && z == quaternion.z)
			{
				return w == quaternion.w;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
		}

		public static bool operator ==(Quaternion a, Quaternion b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Quaternion a, Quaternion b)
		{
			return !(a == b);
		}
	}
}
