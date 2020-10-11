using System;
using System.ComponentModel;

namespace cYo.Common.Mathematics
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public struct Matrix3
	{
		public float A1;

		public float A2;

		public float A3;

		public float B1;

		public float B2;

		public float B3;

		public float C1;

		public float C2;

		public float C3;

		public static Matrix3 Identity
		{
			get
			{
				Matrix3 result = default(Matrix3);
				result.A1 = (result.B2 = (result.C3 = 1f));
				return result;
			}
		}

		public static Matrix3 Zero => default(Matrix3);

		public Vector3 Column1
		{
			get
			{
				return new Vector3(A1, B1, C1);
			}
			set
			{
				A1 = value.X;
				B1 = value.Y;
				C1 = value.Z;
			}
		}

		public Vector3 Column2
		{
			get
			{
				return new Vector3(A2, B2, C2);
			}
			set
			{
				A2 = value.X;
				B2 = value.Y;
				C2 = value.Z;
			}
		}

		public Vector3 Column3
		{
			get
			{
				return new Vector3(A3, B3, C3);
			}
			set
			{
				A3 = value.X;
				B3 = value.Y;
				C3 = value.Z;
			}
		}

		public Vector3 LookAtVector
		{
			get
			{
				return new Vector3(A3, B3, C3);
			}
			set
			{
				A3 = value.X;
				B3 = value.Y;
				C3 = value.Z;
			}
		}

		public Vector3 UpVector
		{
			get
			{
				return new Vector3(A2, B2, C2);
			}
			set
			{
				A2 = value.X;
				B2 = value.Y;
				C2 = value.Z;
			}
		}

		public Vector3 RightVector
		{
			get
			{
				return new Vector3(A1, B1, C1);
			}
			set
			{
				A1 = value.X;
				B1 = value.Y;
				C1 = value.Z;
			}
		}

		[Browsable(false)]
		public Quaternion Quaternion
		{
			get
			{
				float num = A1 + B2 + C3;
				float x;
				float y;
				float z;
				float w;
				if ((double)num > 1E-08)
				{
					float num2 = Numeric.Sqrt(num) * 2f;
					x = (B3 - C2) / num2;
					y = (C1 - A3) / num2;
					z = (A2 - B1) / num2;
					w = 0.25f * num2;
				}
				else if (A1 > B2 && A1 > C3)
				{
					float num2 = Numeric.Sqrt(1f + A1 - B2 - C3) * 2f;
					x = 0.25f * num2;
					y = (A2 + B1) / num2;
					z = (C1 + A3) / num2;
					w = (B3 - C2) / num2;
				}
				else if (B2 > C3)
				{
					float num2 = Numeric.Sqrt(1f + B2 - A1 - C3) * 2f;
					x = (A2 + B1) / num2;
					y = 0.25f * num2;
					z = (B3 + C2) / num2;
					w = (C1 - A3) / num2;
				}
				else
				{
					float num2 = Numeric.Sqrt(1f + C3 - A1 - B2) * 2f;
					x = (C1 + A3) / num2;
					y = (B3 + C2) / num2;
					z = 0.25f * num2;
					w = (A2 - B1) / num2;
				}
				return new Quaternion(x, y, z, w);
			}
		}

		public float this[int column, int row]
		{
			get
			{
				return this[column + row * 3];
			}
			set
			{
				this[column + row * 3] = value;
			}
		}

		public float this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return A1;
				case 1:
					return A2;
				case 2:
					return A3;
				case 3:
					return B1;
				case 4:
					return B2;
				case 5:
					return B3;
				case 6:
					return C1;
				case 7:
					return C2;
				case 8:
					return C3;
				default:
					throw new IndexOutOfRangeException("Invalid matrix index!");
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					A1 = value;
					break;
				case 1:
					A2 = value;
					break;
				case 2:
					A3 = value;
					break;
				case 3:
					B1 = value;
					break;
				case 4:
					B2 = value;
					break;
				case 5:
					B3 = value;
					break;
				case 6:
					C1 = value;
					break;
				case 7:
					C2 = value;
					break;
				case 8:
					C3 = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid matrix index!");
				}
			}
		}

		public Matrix3(float a1, float a2, float a3, float b1, float b2, float b3, float c1, float c2, float c3)
		{
			A1 = a1;
			B1 = b1;
			C1 = c1;
			A2 = a2;
			B2 = b2;
			C2 = c2;
			A3 = a3;
			B3 = b3;
			C3 = c3;
		}

		public Matrix3(Vector3 A, Vector3 B, Vector3 C)
		{
			A1 = A.X;
			B1 = B.X;
			C1 = C.X;
			A2 = A.Y;
			B2 = B.Y;
			C2 = C.Y;
			A3 = A.Z;
			B3 = B.Z;
			C3 = C.Z;
		}

		public static Matrix3 RotationX(float alpha)
		{
			float num = Numeric.Cos(alpha);
			float num2 = Numeric.Sin(alpha);
			return new Matrix3(1f, 0f, 0f, 0f, num, num2, 0f, 0f - num2, num);
		}

		public static Matrix3 RotationY(float alpha)
		{
			float num = Numeric.Cos(alpha);
			float num2 = Numeric.Sin(alpha);
			return new Matrix3(num, 0f, 0f - num2, 0f, 1f, 0f, num2, 0f, num);
		}

		public static Matrix3 RotationZ(float alpha)
		{
			float num = Numeric.Cos(alpha);
			float num2 = Numeric.Sin(alpha);
			return new Matrix3(num, num2, 0f, 0f - num2, num, 0f, 0f, 0f, 1f);
		}

		public static Matrix3 Rotation(float alpha, float beta, float gamma)
		{
			float num = Numeric.Sin(alpha);
			float num2 = Numeric.Cos(alpha);
			float num3 = Numeric.Sin(beta);
			float num4 = Numeric.Cos(beta);
			float num5 = Numeric.Sin(gamma);
			float num6 = Numeric.Cos(gamma);
			float num7 = num3 * num;
			float num8 = num3 * num2;
			Matrix3 result = default(Matrix3);
			result.A1 = num6 * num4;
			result.A2 = num5 * num4;
			result.A3 = 0f - num3;
			result.B1 = num6 * num7 - num5 * num2;
			result.B2 = num5 * num7 + num6 * num2;
			result.B3 = num4 * num;
			result.C1 = num6 * num8 + num5 * num;
			result.C2 = num5 * num8 - num6 * num;
			result.C3 = num4 * num2;
			return result;
		}

		public static Matrix3 Scaling(float value)
		{
			return Scaling(new Vector3(value, value, value));
		}

		public static Matrix3 Scaling(Vector3 vec)
		{
			Matrix3 result = default(Matrix3);
			result.A1 = vec.X;
			result.B2 = vec.Y;
			result.C3 = vec.Z;
			return result;
		}

		public static Matrix3 LookAt(Vector3 eye, Vector3 at, Vector3 up)
		{
			Vector3 vector = Vector3.Unit(at - eye);
			Vector3 b = Vector3.Unit(Vector3.Cross(up, vector));
			Vector3 vector2 = Vector3.Cross(vector, b);
			return new Matrix3(b.X, b.Y, b.Z, vector2.X, vector2.Y, vector2.Z, vector.X, vector.Y, vector.Z);
		}

		public static Matrix3 Rotation(Vector3 vec, float angle)
		{
			float num = Numeric.Cos(angle);
			float num2 = Numeric.Sin(angle);
			float num3 = 1f - num;
			Matrix3 matrix = new Matrix3(num3 * vec.X * vec.X + num, num3 * vec.X * vec.Y - num2 * vec.Z, num3 * vec.X * vec.Z + num2 * vec.Y, num3 * vec.X * vec.Y + num2 * vec.Z, num3 * vec.Y * vec.Y + num, num3 * vec.Y * vec.Z - num2 * vec.X, num3 * vec.X * vec.Z - num2 * vec.Y, num3 * vec.Y * vec.Z + num2 * vec.X, num3 * vec.Z * vec.Z + num);
			return Transpose(matrix);
		}

		public static Matrix3 Transpose(Matrix3 matrix)
		{
			Matrix3 result = default(Matrix3);
			result.A1 = matrix.A1;
			result.A2 = matrix.B1;
			result.A3 = matrix.C1;
			result.B1 = matrix.A2;
			result.B2 = matrix.B2;
			result.B3 = matrix.C2;
			result.C1 = matrix.A3;
			result.C2 = matrix.B3;
			result.C3 = matrix.C3;
			return result;
		}

		public float Det()
		{
			return A1 * B2 * C3 + A2 * B3 * C1 + A3 * B1 * C2 - A3 * B2 * C1 - A1 * B3 * C2 - A2 * B1 * C3;
		}

		public static Matrix3 Slerp(Matrix3 a, Matrix3 b, float time)
		{
			Quaternion quaternion = a.Quaternion;
			Quaternion quaternion2 = b.Quaternion;
			return Quaternion.Slerp(quaternion, quaternion2, time).Matrix;
		}

		public static Matrix3 operator +(Matrix3 a, Matrix3 b)
		{
			return new Matrix3(a.A1 + b.A1, a.A2 + b.A2, a.A3 + b.A3, a.B1 + b.B1, a.B2 + b.B2, a.B3 + b.B3, a.C1 + b.C1, a.C2 + b.C2, a.C3 + b.C3);
		}

		public static Matrix3 operator *(Matrix3 a, Matrix3 b)
		{
			Matrix3 zero = Zero;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < 3; k++)
					{
						zero[i, j] += a[k, j] * b[i, k];
					}
				}
			}
			return zero;
		}

		public static bool operator ==(Matrix3 a, Matrix3 b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Matrix3 a, Matrix3 b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Matrix3 matrix = (Matrix3)obj;
			if (A1 == matrix.A1 && A2 == matrix.A2 && A3 == matrix.A3 && B1 == matrix.B1 && B2 == matrix.B2 && B3 == matrix.B3 && C1 == matrix.C1 && C2 == matrix.C2)
			{
				return C3 == matrix.C3;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return A1.GetHashCode() ^ B2.GetHashCode() ^ C3.GetHashCode();
		}
	}
}
