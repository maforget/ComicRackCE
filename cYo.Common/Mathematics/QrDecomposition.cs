using System;

namespace cYo.Common.Mathematics
{
	public class QrDecomposition
	{
		private readonly Matrix QR;

		private readonly double[] Rdiag;

		public bool IsFullRank
		{
			get
			{
				int columns = QR.Columns;
				for (int i = 0; i < columns; i++)
				{
					if (Rdiag[i] == 0.0)
					{
						return false;
					}
				}
				return true;
			}
		}

		public Matrix UpperTriangularFactor
		{
			get
			{
				int columns = QR.Columns;
				Matrix matrix = new Matrix(columns, columns);
				double[][] array = matrix.Array;
				double[][] array2 = QR.Array;
				for (int i = 0; i < columns; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						array[i][j] = ((i < j) ? array2[i][j] : ((i == j) ? Rdiag[i] : 0.0));
					}
				}
				return matrix;
			}
		}

		public Matrix OrthogonalFactor
		{
			get
			{
				Matrix matrix = new Matrix(QR.Rows, QR.Columns);
				double[][] array = matrix.Array;
				double[][] array2 = QR.Array;
				for (int num = QR.Columns - 1; num >= 0; num--)
				{
					for (int i = 0; i < QR.Rows; i++)
					{
						array[i][num] = 0.0;
					}
					array[num][num] = 1.0;
					for (int j = num; j < QR.Columns; j++)
					{
						if (array2[num][num] != 0.0)
						{
							double num2 = 0.0;
							for (int k = num; k < QR.Rows; k++)
							{
								num2 += array2[k][num] * array[k][j];
							}
							num2 = (0.0 - num2) / array2[num][num];
							for (int l = num; l < QR.Rows; l++)
							{
								array[l][j] += num2 * array2[l][num];
							}
						}
					}
				}
				return matrix;
			}
		}

		public QrDecomposition(Matrix A)
		{
			if (A == null)
			{
				throw new ArgumentNullException("A", "Matrix can not be null");
			}
			QR = A.Clone();
			double[][] array = QR.Array;
			int rows = A.Rows;
			int columns = A.Columns;
			Rdiag = new double[columns];
			for (int i = 0; i < columns; i++)
			{
				double num = 0.0;
				for (int j = i; j < rows; j++)
				{
					num = Hypotenuse(num, array[j][i]);
				}
				if (num != 0.0)
				{
					if (array[i][i] < 0.0)
					{
						num = 0.0 - num;
					}
					for (int k = i; k < rows; k++)
					{
						array[k][i] /= num;
					}
					array[i][i] += 1.0;
					for (int l = i + 1; l < columns; l++)
					{
						double num2 = 0.0;
						for (int m = i; m < rows; m++)
						{
							num2 += array[m][i] * array[m][l];
						}
						num2 = (0.0 - num2) / array[i][i];
						for (int n = i; n < rows; n++)
						{
							array[n][l] += num2 * array[n][i];
						}
					}
				}
				Rdiag[i] = 0.0 - num;
			}
		}

		public Matrix Solve(Matrix rightHandSideMatrix)
		{
			if (rightHandSideMatrix == null)
			{
				throw new ArgumentNullException("rightHandSideMatrix", "Matrix must not be null");
			}
			if (rightHandSideMatrix.Rows != QR.Rows)
			{
				throw new ArgumentException("Matrix row dimensions must agree.");
			}
			if (!IsFullRank)
			{
				throw new InvalidOperationException("Matrix is rank deficient.");
			}
			int columns = rightHandSideMatrix.Columns;
			Matrix matrix = rightHandSideMatrix.Clone();
			int rows = QR.Rows;
			int columns2 = QR.Columns;
			double[][] array = QR.Array;
			for (int i = 0; i < columns2; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					double num = 0.0;
					for (int k = i; k < rows; k++)
					{
						num += array[k][i] * matrix[k, j];
					}
					num = (0.0 - num) / array[i][i];
					for (int l = i; l < rows; l++)
					{
						matrix[l, j] += num * array[l][i];
					}
				}
			}
			for (int num2 = columns2 - 1; num2 >= 0; num2--)
			{
				for (int m = 0; m < columns; m++)
				{
					matrix[num2, m] /= Rdiag[num2];
				}
				for (int n = 0; n < num2; n++)
				{
					for (int num3 = 0; num3 < columns; num3++)
					{
						matrix[n, num3] -= matrix[num2, num3] * array[n][num2];
					}
				}
			}
			return matrix.Submatrix(0, columns2 - 1, 0, columns - 1);
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
