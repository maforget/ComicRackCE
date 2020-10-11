using System;
using System.IO;

namespace cYo.Common.Mathematics
{
	public class Matrix
	{
		private readonly double[][] data;

		private readonly int rows;

		private readonly int columns;

		private static readonly Random random = new Random();

		internal double[][] Array => data;

		public int Rows => rows;

		public int Columns => columns;

		public bool IsSquare => rows == columns;

		public bool IsSymmetric
		{
			get
			{
				if (!IsSquare)
				{
					return false;
				}
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j <= i; j++)
					{
						if (data[i][j] != data[j][i])
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		public double this[int i, int j]
		{
			get
			{
				return data[i][j];
			}
			set
			{
				data[i][j] = value;
			}
		}

		public double Norm1
		{
			get
			{
				double num = 0.0;
				for (int i = 0; i < columns; i++)
				{
					double num2 = 0.0;
					for (int j = 0; j < rows; j++)
					{
						num2 += Math.Abs(data[j][i]);
					}
					num = Math.Max(num, num2);
				}
				return num;
			}
		}

		public double InfinityNorm
		{
			get
			{
				double num = 0.0;
				for (int i = 0; i < rows; i++)
				{
					double num2 = 0.0;
					for (int j = 0; j < columns; j++)
					{
						num2 += Math.Abs(data[i][j]);
					}
					num = Math.Max(num, num2);
				}
				return num;
			}
		}

		public double FrobeniusNorm
		{
			get
			{
				double num = 0.0;
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						num = Hypotenuse(num, data[i][j]);
					}
				}
				return num;
			}
		}

		public Matrix Inverse => Solve(Diagonal(rows, rows, 1.0));

		public double Determinant => new LuDecomposition(this).Determinant;

		public double Trace
		{
			get
			{
				double num = 0.0;
				for (int i = 0; i < Math.Min(rows, columns); i++)
				{
					num += data[i][i];
				}
				return num;
			}
		}

		public Matrix(int rows, int columns)
		{
			this.rows = rows;
			this.columns = columns;
			data = new double[rows][];
			for (int i = 0; i < rows; i++)
			{
				data[i] = new double[columns];
			}
		}

		public Matrix(int rows, int columns, double value)
		{
			this.rows = rows;
			this.columns = columns;
			data = new double[rows][];
			for (int i = 0; i < rows; i++)
			{
				data[i] = new double[columns];
			}
			for (int j = 0; j < rows; j++)
			{
				data[j][j] = value;
			}
		}

		public Matrix(double[][] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException();
			}
			rows = data.Length;
			columns = data[0].Length;
			for (int i = 0; i < rows; i++)
			{
				if (data[i].Length != columns)
				{
					throw new ArgumentException();
				}
			}
			this.data = data;
		}

		public Matrix Submatrix(int i0, int i1, int j0, int j1)
		{
			if (i0 > i1 || j0 > j1 || i0 < 0 || i0 >= rows || i1 < 0 || i1 >= rows || j0 < 0 || j0 >= columns || j1 < 0 || j1 >= columns)
			{
				throw new ArgumentException();
			}
			Matrix matrix = new Matrix(i1 - i0 + 1, j1 - j0 + 1);
			double[][] array = matrix.Array;
			for (int k = i0; k <= i1; k++)
			{
				for (int l = j0; l <= j1; l++)
				{
					array[k - i0][l - j0] = data[k][l];
				}
			}
			return matrix;
		}

		public Matrix Submatrix(int[] r, int[] c)
		{
			if (r == null || c == null)
			{
				throw new ArgumentNullException();
			}
			Matrix matrix = new Matrix(r.Length, c.Length);
			double[][] array = matrix.Array;
			for (int i = 0; i < r.Length; i++)
			{
				for (int j = 0; j < c.Length; j++)
				{
					if (r[i] < 0 || r[i] >= rows || c[j] < 0 || c[j] >= columns)
					{
						throw new ArgumentException();
					}
					array[i][j] = data[r[i]][c[j]];
				}
			}
			return matrix;
		}

		public Matrix Submatrix(int i0, int i1, int[] c)
		{
			if (c == null)
			{
				throw new ArgumentNullException();
			}
			if (i0 > i1 || i0 < 0 || i0 >= rows || i1 < 0 || i1 >= rows)
			{
				throw new ArgumentException();
			}
			Matrix matrix = new Matrix(i1 - i0 + 1, c.Length);
			double[][] array = matrix.Array;
			for (int j = i0; j <= i1; j++)
			{
				for (int k = 0; k < c.Length; k++)
				{
					if (c[k] < 0 || c[k] >= columns)
					{
						throw new ArgumentException();
					}
					array[j - i0][k] = data[j][c[k]];
				}
			}
			return matrix;
		}

		public Matrix Submatrix(int[] r, int j0, int j1)
		{
			if (r == null)
			{
				throw new ArgumentNullException("r", "Array can not be null");
			}
			if (j0 > j1 || j0 < 0 || j0 >= columns || j1 < 0 || j1 >= columns)
			{
				throw new ArgumentException();
			}
			Matrix matrix = new Matrix(r.Length, j1 - j0 + 1);
			double[][] array = matrix.Array;
			for (int i = 0; i < r.Length; i++)
			{
				for (int k = j0; k <= j1; k++)
				{
					if (r[i] < 0 || r[i] >= rows)
					{
						throw new ArgumentException();
					}
					array[i][k - j0] = data[r[i]][k];
				}
			}
			return matrix;
		}

		public Matrix Clone()
		{
			Matrix matrix = new Matrix(rows, columns);
			double[][] array = matrix.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					array[i][j] = data[i][j];
				}
			}
			return matrix;
		}

		public Matrix Transpose()
		{
			Matrix matrix = new Matrix(columns, rows);
			double[][] array = matrix.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					array[j][i] = data[i][j];
				}
			}
			return matrix;
		}

		public static Matrix Negate(Matrix a)
		{
			if (a == null)
			{
				throw new ArgumentNullException();
			}
			int num = a.Rows;
			int num2 = a.Columns;
			double[][] array = a.Array;
			Matrix matrix = new Matrix(num, num2);
			double[][] array2 = matrix.Array;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					array2[i][j] = 0.0 - array[i][j];
				}
			}
			return matrix;
		}

		public static Matrix operator -(Matrix a)
		{
			return Negate(a);
		}

		public static Matrix Add(Matrix a, Matrix b)
		{
			if (a == null)
			{
				throw new ArgumentNullException("Matrix can not be null", "a");
			}
			if (b == null)
			{
				throw new ArgumentNullException("Matrix can not be null", "b");
			}
			int num = a.Rows;
			int num2 = a.Columns;
			double[][] array = a.Array;
			if (num != b.Rows || num2 != b.Columns)
			{
				throw new ArgumentException("Matrix dimension do not match.");
			}
			Matrix matrix = new Matrix(num, num2);
			double[][] array2 = matrix.Array;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					array2[i][j] = array[i][j] + b[i, j];
				}
			}
			return matrix;
		}

		public static Matrix operator +(Matrix a, Matrix b)
		{
			return Add(a, b);
		}

		public static Matrix Subtract(Matrix a, Matrix b)
		{
			if (a == null || b == null)
			{
				throw new ArgumentNullException();
			}
			int num = a.Rows;
			int num2 = a.Columns;
			double[][] array = a.Array;
			if (num != b.Rows || num2 != b.Columns)
			{
				throw new ArgumentException("Matrix dimension do not match.");
			}
			Matrix matrix = new Matrix(num, num2);
			double[][] array2 = matrix.Array;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					array2[i][j] = array[i][j] - b[i, j];
				}
			}
			return matrix;
		}

		public static Matrix operator -(Matrix a, Matrix b)
		{
			return Subtract(a, b);
		}

		public static Matrix Multiply(Matrix a, double s)
		{
			if (a == null)
			{
				throw new ArgumentNullException();
			}
			int num = a.Rows;
			int num2 = a.Columns;
			double[][] array = a.Array;
			Matrix matrix = new Matrix(num, num2);
			double[][] array2 = matrix.Array;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					array2[i][j] = array[i][j] * s;
				}
			}
			return matrix;
		}

		public static Matrix operator *(Matrix a, double s)
		{
			if (a == null)
			{
				throw new ArgumentNullException();
			}
			return Multiply(a, s);
		}

		public static Matrix Multiply(Matrix a, Matrix b)
		{
			if (a == null || b == null)
			{
				throw new ArgumentNullException();
			}
			int num = a.Rows;
			double[][] array = a.Array;
			if (b.Rows != a.columns)
			{
				throw new ArgumentException("Matrix dimensions are not valid.");
			}
			int num2 = b.Columns;
			Matrix matrix = new Matrix(num, num2);
			double[][] array2 = matrix.Array;
			int num3 = a.columns;
			double[] array3 = new double[num3];
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num3; j++)
				{
					array3[j] = b[j, i];
				}
				for (int k = 0; k < num; k++)
				{
					double[] array4 = array[k];
					double num4 = 0.0;
					for (int l = 0; l < num3; l++)
					{
						num4 += array4[l] * array3[l];
					}
					array2[k][i] = num4;
				}
			}
			return matrix;
		}

		public static Matrix operator *(Matrix a, Matrix b)
		{
			return Multiply(a, b);
		}

		public Matrix Solve(Matrix rhs)
		{
			if (rhs == null)
			{
				throw new ArgumentNullException();
			}
			if (rows != columns)
			{
				return new QrDecomposition(this).Solve(rhs);
			}
			return new LuDecomposition(this).Solve(rhs);
		}

		public static Matrix Random(int rows, int columns)
		{
			Matrix matrix = new Matrix(rows, columns);
			double[][] array = matrix.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					array[i][j] = random.NextDouble();
				}
			}
			return matrix;
		}

		public static Matrix Diagonal(int rows, int columns, double value)
		{
			Matrix matrix = new Matrix(rows, columns);
			double[][] array = matrix.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					array[i][j] = ((i == j) ? value : 0.0);
				}
			}
			return matrix;
		}

		public string ToString(IFormatProvider provider)
		{
			using (StringWriter stringWriter = new StringWriter(provider))
			{
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						stringWriter.Write(data[i][j] + " ");
					}
					stringWriter.WriteLine();
				}
				return stringWriter.ToString();
			}
		}

		public override string ToString()
		{
			return ToString(null);
		}

		private static double Hypotenuse(double a, double b)
		{
			if (Math.Abs(a) > Math.Abs(b))
			{
				double num = b / a;
				return Math.Abs(a) * Math.Sqrt(1.0 + num * num);
			}
			if (b != 0.0)
			{
				double num2 = a / b;
				return Math.Abs(b) * Math.Sqrt(1.0 + num2 * num2);
			}
			return 0.0;
		}
	}
}
