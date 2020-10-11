using System;

namespace cYo.Common.Mathematics
{
	public class LuDecomposition
	{
		private readonly Matrix LU;

		private readonly int pivotSign;

		private readonly int[] pivotVector;

		public bool IsNonsingular
		{
			get
			{
				for (int i = 0; i < LU.Columns; i++)
				{
					if (LU[i, i] == 0.0)
					{
						return false;
					}
				}
				return true;
			}
		}

		public double Determinant
		{
			get
			{
				if (LU.Rows != LU.Columns)
				{
					throw new ArgumentException("Matrix must be square.");
				}
				double num = pivotSign;
				for (int i = 0; i < LU.Columns; i++)
				{
					num *= LU[i, i];
				}
				return num;
			}
		}

		public Matrix LowerTriangularFactor
		{
			get
			{
				int rows = LU.Rows;
				int columns = LU.Columns;
				Matrix matrix = new Matrix(rows, columns);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						matrix[i, j] = ((i > j) ? LU[i, j] : ((i == j) ? 1.0 : 0.0));
					}
				}
				return matrix;
			}
		}

		public Matrix UpperTriangularFactor
		{
			get
			{
				int rows = LU.Rows;
				int columns = LU.Columns;
				Matrix matrix = new Matrix(rows, columns);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						matrix[i, j] = ((i <= j) ? LU[i, j] : 0.0);
					}
				}
				return matrix;
			}
		}

		public LuDecomposition(Matrix A)
		{
			if (A == null)
			{
				throw new ArgumentNullException();
			}
			LU = A.Clone();
			double[][] array = LU.Array;
			int rows = A.Rows;
			int columns = A.Columns;
			pivotVector = new int[rows];
			for (int i = 0; i < rows; i++)
			{
				pivotVector[i] = i;
			}
			pivotSign = 1;
			double[] array2 = new double[rows];
			for (int j = 0; j < columns; j++)
			{
				for (int k = 0; k < rows; k++)
				{
					array2[k] = array[k][j];
				}
				for (int l = 0; l < rows; l++)
				{
					double[] array3 = array[l];
					int num = Math.Min(l, j);
					double num2 = 0.0;
					for (int m = 0; m < num; m++)
					{
						num2 += array3[m] * array2[m];
					}
					array3[j] = (array2[l] -= num2);
				}
				int num3 = j;
				for (int n = j + 1; n < rows; n++)
				{
					if (Math.Abs(array2[n]) > Math.Abs(array2[num3]))
					{
						num3 = n;
					}
				}
				if (num3 != j)
				{
					for (int num4 = 0; num4 < columns; num4++)
					{
						double num5 = array[num3][num4];
						array[num3][num4] = array[j][num4];
						array[j][num4] = num5;
					}
					int num6 = pivotVector[num3];
					pivotVector[num3] = pivotVector[j];
					pivotVector[j] = num6;
					pivotSign = -pivotSign;
				}
				if ((j < rows) & (array[j][j] != 0.0))
				{
					for (int num7 = j + 1; num7 < rows; num7++)
					{
						array[num7][j] /= array[j][j];
					}
				}
			}
		}

		public double[] CreatePivotPermutationVector()
		{
			int rows = LU.Rows;
			double[] array = new double[rows];
			for (int i = 0; i < rows; i++)
			{
				array[i] = pivotVector[i];
			}
			return array;
		}

		public Matrix Solve(Matrix B)
		{
			if (B == null)
			{
				throw new ArgumentNullException();
			}
			if (B.Rows != LU.Rows)
			{
				throw new ArgumentException("Invalid matrix dimensions.");
			}
			if (!IsNonsingular)
			{
				throw new InvalidOperationException("Matrix is singular");
			}
			int columns = B.Columns;
			Matrix matrix = B.Submatrix(pivotVector, 0, columns - 1);
			int columns2 = LU.Columns;
			double[][] array = LU.Array;
			for (int i = 0; i < columns2; i++)
			{
				for (int j = i + 1; j < columns2; j++)
				{
					for (int k = 0; k < columns; k++)
					{
						matrix[j, k] -= matrix[i, k] * array[j][i];
					}
				}
			}
			for (int num = columns2 - 1; num >= 0; num--)
			{
				for (int l = 0; l < columns; l++)
				{
					matrix[num, l] /= array[num][num];
				}
				for (int m = 0; m < num; m++)
				{
					for (int n = 0; n < columns; n++)
					{
						matrix[m, n] -= matrix[num, n] * array[m][num];
					}
				}
			}
			return matrix;
		}
	}
}
