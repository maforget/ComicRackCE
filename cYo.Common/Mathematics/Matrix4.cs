using System;
using System.ComponentModel;

namespace cYo.Common.Mathematics
{
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public struct Matrix4
	{
		public float A1;

		public float A2;

		public float A3;

		public float A4;

		public float B1;

		public float B2;

		public float B3;

		public float B4;

		public float C1;

		public float C2;

		public float C3;

		public float C4;

		public float D1;

		public float D2;

		public float D3;

		public float D4;

		public static Matrix4 Identity
		{
			get
			{
				Matrix4 zero = Zero;
				zero.A1 = (zero.B2 = (zero.C3 = (zero.D4 = 1f)));
				return zero;
			}
		}

		public static Matrix4 Zero => default(Matrix4);

		public Vector4 Column1
		{
			get
			{
				return new Vector4(A1, B1, C1, D1);
			}
			set
			{
				A1 = value.X;
				B1 = value.Y;
				C1 = value.Z;
				D1 = value.W;
			}
		}

		public Vector4 Column2
		{
			get
			{
				return new Vector4(A2, B2, C2, D2);
			}
			set
			{
				A2 = value.X;
				B2 = value.Y;
				C2 = value.Z;
				D2 = value.W;
			}
		}

		public Vector4 Column3
		{
			get
			{
				return new Vector4(A3, B3, C3, D3);
			}
			set
			{
				A3 = value.X;
				B3 = value.Y;
				C3 = value.Z;
				D3 = value.W;
			}
		}

		public Vector4 Column4
		{
			get
			{
				return new Vector4(A4, B4, C4, D4);
			}
			set
			{
				A4 = value.X;
				B4 = value.Y;
				C4 = value.Z;
				D4 = value.W;
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

		public Vector3 TranslationVector
		{
			get
			{
				return new Vector3(D1, D2, D3);
			}
			set
			{
				D1 = value.X;
				D2 = value.Y;
				D3 = value.Z;
			}
		}

		public Matrix3 RotationMatrix
		{
			get
			{
				return new Matrix3(A1, A2, A3, B1, B2, B3, C1, C2, C3);
			}
			set
			{
				A1 = value.A1;
				B1 = value.B1;
				C1 = value.C1;
				A2 = value.A2;
				B2 = value.B2;
				C2 = value.C2;
				A3 = value.A3;
				B3 = value.B3;
				C3 = value.C3;
			}
		}

		public Quaternion Quaternion
		{
			get
			{
				float num = A1 + B2 + C3;
				float x;
				float y;
				float z;
				float w;
				if (num > 0f)
				{
					float num2 = Numeric.Sqrt(num + 1f);
					float num3 = 0.5f / num2;
					x = (B3 - C2) * num3;
					y = (C1 - A3) * num3;
					z = (A2 - B1) * num3;
					w = 0.5f * num2;
				}
				else if (A1 > B2 && A1 > C3)
				{
					float num2 = Numeric.Sqrt(1f + A1 - B2 - C3);
					float num4 = 0.5f / num2;
					x = 0.5f * num2;
					y = (A2 + B1) * num4;
					z = (C1 + A3) * num4;
					w = (B3 - C2) * num4;
				}
				else if (B2 > C3)
				{
					float num2 = Numeric.Sqrt(1f + B2 - A1 - C3);
					float num5 = 0.5f / num2;
					x = (A2 + B1) * num5;
					y = 0.5f * num2;
					z = (B3 + C2) * num5;
					w = (C1 - A3) * num5;
				}
				else
				{
					float num2 = Numeric.Sqrt(1f + C3 - A1 - B2);
					float num6 = 0.5f / num2;
					x = (C1 + A3) * num6;
					y = (B3 + C2) * num6;
					z = 0.5f * num2;
					w = (A2 - B1) * num6;
				}
				return new Quaternion(x, y, z, w);
			}
		}

		public float this[int column, int row]
		{
			get
			{
				return this[column + row * 4];
			}
			set
			{
				this[column + row * 4] = value;
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
					return A4;
				case 4:
					return B1;
				case 5:
					return B2;
				case 6:
					return B3;
				case 7:
					return B4;
				case 8:
					return C1;
				case 9:
					return C2;
				case 10:
					return C3;
				case 11:
					return C4;
				case 12:
					return D1;
				case 13:
					return D2;
				case 14:
					return D3;
				case 15:
					return D4;
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
					A4 = value;
					break;
				case 4:
					B1 = value;
					break;
				case 5:
					B2 = value;
					break;
				case 6:
					B3 = value;
					break;
				case 7:
					B4 = value;
					break;
				case 8:
					C1 = value;
					break;
				case 9:
					C2 = value;
					break;
				case 10:
					C3 = value;
					break;
				case 11:
					C4 = value;
					break;
				case 12:
					D1 = value;
					break;
				case 13:
					D2 = value;
					break;
				case 14:
					D3 = value;
					break;
				case 15:
					D4 = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid matrix index!");
				}
			}
		}

		public Matrix4(float a1, float a2, float a3, float a4, float b1, float b2, float b3, float b4, float c1, float c2, float c3, float c4, float d1, float d2, float d3, float d4)
		{
			A1 = a1;
			B1 = b1;
			C1 = c1;
			D1 = d1;
			A2 = a2;
			B2 = b2;
			C2 = c2;
			D2 = d2;
			A3 = a3;
			B3 = b3;
			C3 = c3;
			D3 = d3;
			A4 = a4;
			B4 = b4;
			C4 = c4;
			D4 = d4;
		}

		public Matrix4(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
		{
			A1 = A.X;
			B1 = B.X;
			C1 = C.X;
			D1 = D.X;
			A2 = A.Y;
			B2 = B.Y;
			C2 = C.Y;
			D2 = D.Y;
			A3 = A.Z;
			B3 = B.Z;
			C3 = C.Z;
			D3 = D.Z;
			A4 = 0f;
			B4 = 0f;
			C4 = 0f;
			D4 = 1f;
		}

		public Matrix4(Matrix3 rot, Vector3 trans)
		{
			A1 = rot.A1;
			B1 = rot.B1;
			C1 = rot.C1;
			D1 = trans.X;
			A2 = rot.A2;
			B2 = rot.B2;
			C2 = rot.C2;
			D2 = trans.Y;
			A3 = rot.A3;
			B3 = rot.B3;
			C3 = rot.C3;
			D3 = trans.Z;
			A4 = 0f;
			B4 = 0f;
			C4 = 0f;
			D4 = 1f;
		}

		public static Matrix4 RotationX(float alpha)
		{
			float num = Numeric.Cos(alpha);
			float num2 = Numeric.Sin(alpha);
			return new Matrix4(1f, 0f, 0f, 0f, 0f, num, num2, 0f, 0f, 0f - num2, num, 0f, 0f, 0f, 0f, 1f);
		}

		public static Matrix4 RotationY(float alpha)
		{
			float num = Numeric.Cos(alpha);
			float num2 = Numeric.Sin(alpha);
			return new Matrix4(num, 0f, 0f - num2, 0f, 0f, 1f, 0f, 0f, num2, 0f, num, 0f, 0f, 0f, 0f, 1f);
		}

		public static Matrix4 RotationZ(float alpha)
		{
			float num = Numeric.Cos(alpha);
			float num2 = Numeric.Sin(alpha);
			return new Matrix4(num, num2, 0f, 0f, 0f - num2, num, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
		}

		public static Matrix4 Rotation(float alpha, float beta, float gamma)
		{
			float num = Numeric.Sin(alpha);
			float num2 = Numeric.Cos(alpha);
			float num3 = Numeric.Sin(beta);
			float num4 = Numeric.Cos(beta);
			float num5 = Numeric.Sin(gamma);
			float num6 = Numeric.Cos(gamma);
			float num7 = num3 * num;
			float num8 = num3 * num2;
			Matrix4 zero = Zero;
			zero.A1 = num6 * num4;
			zero.A2 = num5 * num4;
			zero.A3 = 0f - num3;
			zero.A4 = 0f;
			zero.B1 = num6 * num7 - num5 * num2;
			zero.B2 = num5 * num7 + num6 * num2;
			zero.B3 = num4 * num;
			zero.B4 = 0f;
			zero.C1 = num6 * num8 + num5 * num;
			zero.C2 = num5 * num8 - num6 * num;
			zero.C3 = num4 * num2;
			zero.C4 = 0f;
			zero.D1 = 0f;
			zero.D2 = 0f;
			zero.D3 = 0f;
			zero.D4 = 1f;
			return zero;
		}

		public static Matrix4 Translation(Vector3 vec)
		{
			return new Matrix4(Vector3.Right, Vector3.Up, Vector3.LookAt, vec);
		}

		public static Matrix4 Translation(float x, float y, float z)
		{
			return Translation(new Vector3(x, y, z));
		}

		public static Matrix4 Scaling(float value)
		{
			return Scaling(new Vector3(value, value, value));
		}

		public static Matrix4 Scaling(Vector3 vec)
		{
			Matrix4 zero = Zero;
			zero.A1 = vec.X;
			zero.B2 = vec.Y;
			zero.C3 = vec.Z;
			zero.D4 = 1f;
			return zero;
		}

		public void Translate(Vector3 vec)
		{
			D1 += vec.X;
			D2 += vec.Y;
			D3 += vec.Z;
		}

		public void Translate(float x, float y, float z)
		{
			D1 += x;
			D2 += y;
			D3 += z;
		}

		public static Matrix4 Perspective(float width, float height, float near, float far)
		{
			if (far == float.PositiveInfinity)
			{
				return PerspectiveInfinity(width, height, near);
			}
			return new Matrix4(2f * near / width, 0f, 0f, 0f, 0f, 2f * near / height, 0f, 0f, 0f, 0f, far / (far - near), 1f, 0f, 0f, near * far / (near - far), 0f);
		}

		public static Matrix4 PerspectiveInfinity(float width, float height, float near)
		{
			return new Matrix4(2f * near / width, 0f, 0f, 0f, 0f, 2f * near / height, 0f, 0f, 0f, 0f, 0.999f, 1f, 0f, 0f, near * -0.999f, 0f);
		}

		public static Matrix4 PerspectiveFOV(float fovY, float ratio, float near, float far)
		{
			if (far == float.PositiveInfinity)
			{
				return PerspectiveFOVInfinity(fovY, ratio, near);
			}
			float num = Numeric.Cot(fovY / 2f);
			float a = num / ratio;
			return new Matrix4(a, 0f, 0f, 0f, 0f, num, 0f, 0f, 0f, 0f, far / (far - near), 1f, 0f, 0f, (0f - near) * far / (far - near), 0f);
		}

		public static Matrix4 Orthogonal(float w, float h, float near, float far)
		{
			return new Matrix4(2f / w, 0f, 0f, 0f, 0f, 2f / h, 0f, 0f, 0f, 0f, 1f / (far - near), 0f, 0f, 0f, near / (near - far), 1f);
		}

		public static Matrix4 PerspectiveFOVInfinity(float fovY, float ratio, float near)
		{
			float num = Numeric.Cot(fovY / 2f);
			float a = num / ratio;
			return new Matrix4(a, 0f, 0f, 0f, 0f, num, 0f, 0f, 0f, 0f, 0.999f, 1f, 0f, 0f, near * -0.999f, 0f);
		}

		public static Matrix4 LookAt(Vector3 eye, Vector3 at, Vector3 up)
		{
			Vector3 vector = Vector3.Unit(at - eye);
			Vector3 vector2 = Vector3.Unit(Vector3.Cross(up, vector));
			Vector3 a = Vector3.Cross(vector, vector2);
			return new Matrix4(vector2.X, vector2.Y, vector2.Z, 0f, a.X, a.Y, a.Z, 0f, vector.X, vector.Y, vector.Z, 0f, -vector2 * eye, -a * eye, -vector * eye, 1f);
		}

		public static Matrix4 Rotation(Vector3 vec, float angle)
		{
			float num = Numeric.Cos(angle);
			float num2 = Numeric.Sin(angle);
			float num3 = 1f - num;
			Matrix4 matrix = new Matrix4(num3 * vec.X * vec.X + num, num3 * vec.X * vec.Y + num2 * vec.Z, num3 * vec.X * vec.Z + num2 * vec.Y, 0f, num3 * vec.X * vec.Y - num2 * vec.Z, num3 * vec.Y * vec.Y + num, num3 * vec.Y * vec.Z + num2 * vec.X, 0f, num3 * vec.X * vec.Z + num2 * vec.Y, num3 * vec.Y * vec.Z - num2 * vec.X, num3 * vec.Z * vec.Z + num, 0f, 0f, 0f, 0f, 1f);
			return Transpose(matrix);
		}

		public static Matrix4 Slerp(Matrix4 a, Matrix4 b, float time)
		{
			Quaternion quaternion = a.Quaternion;
			Quaternion quaternion2 = b.Quaternion;
			Matrix4 matrix = Quaternion.Slerp(quaternion, quaternion2, time).Matrix4;
			matrix.TranslationVector = Vector3.Lerp(a.TranslationVector, b.TranslationVector, time);
			return matrix;
		}

		public static Matrix4 Squad(Matrix4 pre, Matrix4 a, Matrix4 b, Matrix4 post, float time)
		{
			Quaternion quaternion = pre.Quaternion;
			Quaternion quaternion2 = a.Quaternion;
			Quaternion quaternion3 = b.Quaternion;
			Quaternion quaternion4 = post.Quaternion;
			return Quaternion.SimpleSquad(quaternion, quaternion2, quaternion3, quaternion4, time).Matrix4;
		}

		public static Matrix4 Transpose(Matrix4 matrix)
		{
			Matrix4 zero = Zero;
			zero.A1 = matrix.A1;
			zero.A2 = matrix.B1;
			zero.A3 = matrix.C1;
			zero.A4 = matrix.D1;
			zero.B1 = matrix.A2;
			zero.B2 = matrix.B2;
			zero.B3 = matrix.C2;
			zero.B4 = matrix.D2;
			zero.C1 = matrix.A3;
			zero.C2 = matrix.B3;
			zero.C3 = matrix.C3;
			zero.C4 = matrix.D3;
			zero.D1 = matrix.A4;
			zero.D2 = matrix.B4;
			zero.D3 = matrix.C4;
			zero.D4 = matrix.D4;
			return zero;
		}

		public static Matrix3 Minor(Matrix4 source, int column, int row)
		{
			int num = 0;
			Matrix3 result = default(Matrix3);
			for (int i = 0; i < 4; i++)
			{
				int num2 = 0;
				if (i == row)
				{
					continue;
				}
				for (int j = 0; j < 4; j++)
				{
					if (j != column)
					{
						result[num2, num] = source[j, i];
						num2++;
					}
				}
				num++;
			}
			return result;
		}

		public static Matrix4 Adjoint(Matrix4 source)
		{
			Matrix4 zero = Zero;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if ((j + i) % 2 == 0)
					{
						zero[j, i] = Minor(source, i, j).Det();
					}
					else
					{
						zero[j, i] = 0f - Minor(source, i, j).Det();
					}
				}
			}
			return zero;
		}

		public float Det()
		{
			return A4 * B3 * C2 * D1 - A3 * B4 * C2 * D1 - A4 * B2 * C3 * D1 + A2 * B4 * C3 * D1 + A3 * B2 * C4 * D1 - A2 * B3 * C4 * D1 - A4 * B3 * C1 * D2 + A3 * B4 * C1 * D2 + A4 * B1 * C3 * D2 - A1 * B4 * C3 * D2 - A3 * B1 * C4 * D2 + A1 * B3 * C4 * D2 + A4 * B2 * C1 * D3 - A2 * B4 * C1 * D3 - A4 * B1 * C2 * D3 + A1 * B4 * C2 * D3 + A2 * B1 * C4 * D3 - A1 * B2 * C4 * D3 - A3 * B2 * C1 * D4 + A2 * B3 * C1 * D4 + A3 * B1 * C2 * D4 - A1 * B3 * C2 * D4 - A2 * B1 * C3 * D4 + A1 * B2 * C3 * D4;
		}

		public static Matrix4 Invert(Matrix4 m)
		{
			Matrix4 zero = Zero;
			zero.A1 = m.B3 * m.C4 * m.D2 - m.B4 * m.C3 * m.D2 + m.B4 * m.C2 * m.D3 - m.B2 * m.C4 * m.D3 - m.B3 * m.C2 * m.D4 + m.B2 * m.C3 * m.D4;
			zero.A2 = m.A4 * m.C3 * m.D2 - m.A3 * m.C4 * m.D2 - m.A4 * m.C2 * m.D3 + m.A2 * m.C4 * m.D3 + m.A3 * m.C2 * m.D4 - m.A2 * m.C3 * m.D4;
			zero.A3 = m.A3 * m.B4 * m.D2 - m.A4 * m.B3 * m.D2 + m.A4 * m.B2 * m.D3 - m.A2 * m.B4 * m.D3 - m.A3 * m.B2 * m.D4 + m.A2 * m.B3 * m.D4;
			zero.A4 = m.A4 * m.B3 * m.C2 - m.A3 * m.B4 * m.C2 - m.A4 * m.B2 * m.C3 + m.A2 * m.B4 * m.C3 + m.A3 * m.B2 * m.C4 - m.A2 * m.B3 * m.C4;
			zero.B1 = m.B4 * m.C3 * m.D1 - m.B3 * m.C4 * m.D1 - m.B4 * m.C1 * m.D3 + m.B1 * m.C4 * m.D3 + m.B3 * m.C1 * m.D4 - m.B1 * m.C3 * m.D4;
			zero.B2 = m.A3 * m.C4 * m.D1 - m.A4 * m.C3 * m.D1 + m.A4 * m.C1 * m.D3 - m.A1 * m.C4 * m.D3 - m.A3 * m.C1 * m.D4 + m.A1 * m.C3 * m.D4;
			zero.B3 = m.A4 * m.B3 * m.D1 - m.A3 * m.B4 * m.D1 - m.A4 * m.B1 * m.D3 + m.A1 * m.B4 * m.D3 + m.A3 * m.B1 * m.D4 - m.A1 * m.B3 * m.D4;
			zero.B4 = m.A3 * m.B4 * m.C1 - m.A4 * m.B3 * m.C1 + m.A4 * m.B1 * m.C3 - m.A1 * m.B4 * m.C3 - m.A3 * m.B1 * m.C4 + m.A1 * m.B3 * m.C4;
			zero.C1 = m.B2 * m.C4 * m.D1 - m.B4 * m.C2 * m.D1 + m.B4 * m.C1 * m.D2 - m.B1 * m.C4 * m.D2 - m.B2 * m.C1 * m.D4 + m.B1 * m.C2 * m.D4;
			zero.C2 = m.A4 * m.C2 * m.D1 - m.A2 * m.C4 * m.D1 - m.A4 * m.C1 * m.D2 + m.A1 * m.C4 * m.D2 + m.A2 * m.C1 * m.D4 - m.A1 * m.C2 * m.D4;
			zero.C3 = m.A2 * m.B4 * m.D1 - m.A4 * m.B2 * m.D1 + m.A4 * m.B1 * m.D2 - m.A1 * m.B4 * m.D2 - m.A2 * m.B1 * m.D4 + m.A1 * m.B2 * m.D4;
			zero.C4 = m.A4 * m.B2 * m.C1 - m.A2 * m.B4 * m.C1 - m.A4 * m.B1 * m.C2 + m.A1 * m.B4 * m.C2 + m.A2 * m.B1 * m.C4 - m.A1 * m.B2 * m.C4;
			zero.D1 = m.B3 * m.C2 * m.D1 - m.B2 * m.C3 * m.D1 - m.B3 * m.C1 * m.D2 + m.B1 * m.C3 * m.D2 + m.B2 * m.C1 * m.D3 - m.B1 * m.C2 * m.D3;
			zero.D2 = m.A2 * m.C3 * m.D1 - m.A3 * m.C2 * m.D1 + m.A3 * m.C1 * m.D2 - m.A1 * m.C3 * m.D2 - m.A2 * m.C1 * m.D3 + m.A1 * m.C2 * m.D3;
			zero.D3 = m.A3 * m.B2 * m.D1 - m.A2 * m.B3 * m.D1 - m.A3 * m.B1 * m.D2 + m.A1 * m.B3 * m.D2 + m.A2 * m.B1 * m.D3 - m.A1 * m.B2 * m.D3;
			zero.D4 = m.A2 * m.B3 * m.C1 - m.A3 * m.B2 * m.C1 + m.A3 * m.B1 * m.C2 - m.A1 * m.B3 * m.C2 - m.A2 * m.B1 * m.C3 + m.A1 * m.B2 * m.C3;
			return zero * (1f / m.Det());
		}

		public static void Decompose(Matrix4 mat, out Vector3 translation, out Vector3 scaling, out Matrix4 rotation)
		{
			translation = Vector3.Zero;
			scaling = Vector3.Zero;
			rotation = Identity;
			translation.X = mat.D1;
			translation.Y = mat.D2;
			translation.Z = mat.D3;
			Vector3[] array = new Vector3[3]
			{
				new Vector3(mat.A1, mat.A2, mat.A3),
				new Vector3(mat.B1, mat.B2, mat.B3),
				new Vector3(mat.C1, mat.C2, mat.C3)
			};
			scaling.X = array[0].Length();
			scaling.Y = array[1].Length();
			scaling.Z = array[2].Length();
			if (scaling.X != 0f)
			{
				array[0].X /= scaling.X;
				array[0].Y /= scaling.X;
				array[0].Z /= scaling.X;
			}
			if (scaling.Y != 0f)
			{
				array[1].X /= scaling.Y;
				array[1].Y /= scaling.Y;
				array[1].Z /= scaling.Y;
			}
			if (scaling.Z != 0f)
			{
				array[2].X /= scaling.Z;
				array[2].Y /= scaling.Z;
				array[2].Z /= scaling.Z;
			}
			rotation.A1 = array[0].X;
			rotation.B1 = array[0].Y;
			rotation.C1 = array[0].Z;
			rotation.A4 = 0f;
			rotation.D1 = 0f;
			rotation.A2 = array[1].X;
			rotation.B2 = array[1].Y;
			rotation.C2 = array[1].Z;
			rotation.B4 = 0f;
			rotation.D2 = 0f;
			rotation.A3 = array[2].X;
			rotation.B3 = array[2].Y;
			rotation.C3 = array[2].Z;
			rotation.C4 = 0f;
			rotation.D3 = 0f;
			rotation.D4 = 1f;
		}

		public static Matrix4 operator +(Matrix4 a, Matrix4 b)
		{
			return new Matrix4(a.A1 + b.A1, a.A2 + b.A2, a.A3 + b.A3, a.A4 + b.A4, a.B1 + b.B1, a.B2 + b.B2, a.B3 + b.B3, a.B4 + b.B4, a.C1 + b.C1, a.C2 + b.C2, a.C3 + b.C3, a.C4 + b.C4, a.D1 + b.D1, a.D2 + b.D2, a.D3 + b.D3, a.D4 + b.D4);
		}

		public static Matrix4 operator *(Matrix4 a, Matrix4 b)
		{
			float a2 = a.A1 * b.A1 + a.A2 * b.B1 + a.A3 * b.C1 + a.A4 * b.D1;
			float a3 = a.A1 * b.A2 + a.A2 * b.B2 + a.A3 * b.C2 + a.A4 * b.D2;
			float a4 = a.A1 * b.A3 + a.A2 * b.B3 + a.A3 * b.C3 + a.A4 * b.D3;
			float a5 = a.A1 * b.A4 + a.A2 * b.B4 + a.A3 * b.C4 + a.A4 * b.D4;
			float b2 = a.B1 * b.A1 + a.B2 * b.B1 + a.B3 * b.C1 + a.B4 * b.D1;
			float b3 = a.B1 * b.A2 + a.B2 * b.B2 + a.B3 * b.C2 + a.B4 * b.D2;
			float b4 = a.B1 * b.A3 + a.B2 * b.B3 + a.B3 * b.C3 + a.B4 * b.D3;
			float b5 = a.B1 * b.A4 + a.B2 * b.B4 + a.B3 * b.C4 + a.B4 * b.D4;
			float c = a.C1 * b.A1 + a.C2 * b.B1 + a.C3 * b.C1 + a.C4 * b.D1;
			float c2 = a.C1 * b.A2 + a.C2 * b.B2 + a.C3 * b.C2 + a.C4 * b.D2;
			float c3 = a.C1 * b.A3 + a.C2 * b.B3 + a.C3 * b.C3 + a.C4 * b.D3;
			float c4 = a.C1 * b.A4 + a.C2 * b.B4 + a.C3 * b.C4 + a.C4 * b.D4;
			float d = a.D1 * b.A1 + a.D2 * b.B1 + a.D3 * b.C1 + a.D4 * b.D1;
			float d2 = a.D1 * b.A2 + a.D2 * b.B2 + a.D3 * b.C2 + a.D4 * b.D2;
			float d3 = a.D1 * b.A3 + a.D2 * b.B3 + a.D3 * b.C3 + a.D4 * b.D3;
			float d4 = a.D1 * b.A4 + a.D2 * b.B4 + a.D3 * b.C4 + a.D4 * b.D4;
			return new Matrix4(a2, a3, a4, a5, b2, b3, b4, b5, c, c2, c3, c4, d, d2, d3, d4);
		}

		public static Matrix4 operator *(Matrix4 source, float scalar)
		{
			return new Matrix4(source.A1 * scalar, source.A2 * scalar, source.A3 * scalar, source.A4 * scalar, source.B1 * scalar, source.B2 * scalar, source.B3 * scalar, source.B4 * scalar, source.C1 * scalar, source.C2 * scalar, source.C3 * scalar, source.C4 * scalar, source.D1 * scalar, source.D2 * scalar, source.D3 * scalar, source.D4 * scalar);
		}

		public static Matrix4 operator /(Matrix4 source, float scalar)
		{
			return source * (1f / scalar);
		}

		public static Matrix4 From(float[] values)
		{
			return new Matrix4(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9], values[10], values[11], values[12], values[13], values[14], values[15]);
		}

		public unsafe static Matrix4[] From(byte[] data, int offset, int count)
		{
			Matrix4[] array = new Matrix4[count];
			fixed (byte* ptr = &data[offset])
			{
				float* ptr2 = (float*)ptr;
				for (int i = 0; i < count; i++)
				{
					array[i] = new Matrix4(*ptr2, ptr2[1], ptr2[2], ptr2[3], ptr2[4], ptr2[5], ptr2[6], ptr2[7], ptr2[8], ptr2[9], ptr2[10], ptr2[11], ptr2[12], ptr2[13], ptr2[14], ptr2[15]);
					ptr2 += 16;
				}
			}
			return array;
		}

		public static Matrix4 From(byte[] bytes)
		{
			return From(bytes, 0);
		}

		public unsafe static Matrix4 From(byte[] bytes, int offset)
		{
			fixed (byte* ptr = &bytes[offset])
			{
				float* ptr2 = (float*)ptr;
				return new Matrix4(*ptr2, ptr2[1], ptr2[2], ptr2[3], ptr2[4], ptr2[5], ptr2[6], ptr2[7], ptr2[8], ptr2[9], ptr2[10], ptr2[11], ptr2[12], ptr2[13], ptr2[14], ptr2[15]);
			}
		}

		public static Matrix4 From(Matrix3 m)
		{
			return new Matrix4(m.A1, m.A2, m.A3, 0f, m.B1, m.B2, m.B3, 0f, m.C1, m.C2, m.C3, 0f, 0f, 0f, 0f, 1f);
		}

		public static bool operator ==(Matrix4 a, Matrix4 b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Matrix4 a, Matrix4 b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Matrix4 matrix = (Matrix4)obj;
			if (A1 == matrix.A1 && A2 == matrix.A2 && A3 == matrix.A3 && A4 == matrix.A4 && B1 == matrix.B1 && B2 == matrix.B2 && B3 == matrix.B3 && B4 == matrix.B4 && C1 == matrix.C1 && C2 == matrix.C2 && C3 == matrix.C3 && C4 == matrix.C4 && D1 == matrix.D1 && D2 == matrix.D2 && D3 == matrix.D3)
			{
				return D4 == matrix.D4;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return A1.GetHashCode() ^ B2.GetHashCode() ^ C3.GetHashCode() ^ D4.GetHashCode();
		}
	}
}
