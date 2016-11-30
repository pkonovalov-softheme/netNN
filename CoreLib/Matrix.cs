using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    [Serializable]
    public class Matrix
    {
        public Matrix(double value) : this(1, 1)
        {
            _values[0, 0] = value;
        }

        public Matrix(int rows, int columns)
        {
            this.Rows = rows;
            this.Columns = columns;
            _values = new double[rows , columns];
        }

        public Matrix(double[,] values)
        {
            _values = values;
            this.Rows = values.GetLength(0);
            this.Columns = values.GetLength(1); 
        }

        public int Rows { get; }

        public int Columns { get; }

        public double this[int rows, int columns]
        {
            get
            {
                return _values[rows, columns];
            }
            set
            {
                _values[rows, columns] = value;
            }
        }

        public void ApplyActivation(IActivationFunction activation)
        {
            ApplyFunction(activation.Forward);
        }

        public void ApplyFunction(Func<double, double> func)
        {
            for (int raw = 0; raw < Rows; raw++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    this[raw, column] = func(this[raw, column]);
                }
            }
        }

        // resultY = ax + b
        public static void LinearTransform(Matrix resultY, Matrix a, Matrix x, Matrix b)
        {
            ValidateMatricesDims(resultY, a, x, b);

            for (int raw = 0; raw < resultY.Rows; raw++)
            {
                for (int column = 0; column < resultY.Columns; column++)
                {
                    resultY[raw, column] = a[raw, column] * resultY[raw, column] + b[raw, column];
                }
            }
        }

        /// <summary>
        /// resultY = f(ax + b)
        /// </summary>
        public static void NonLinearTransform(Matrix resultY, Matrix w, Matrix x, Matrix b, Func<double, double> f)
        {
            ValidateMatricesDims(resultY, w, x, b);

            for (int raw = 0; raw < resultY.Rows; raw++)
            {
                for (int column = 0; column < resultY.Columns; column++)
                {
#if DEBUG
                    if (f(w[raw, column] * x[raw, column] + b[raw, column]) == 0)
                    {
                        Utils.DebbuggerBreak();
                    }
#endif
                    resultY[raw, column] = f(w[raw, column] * x[raw, column] + b[raw, column]);
                }
            }
        }

        /// <summary>
        /// Performs arbitrary operation on sourceMatrix and write results in resultMatrix
        /// </summary>
        public static void ArbitraryOperation(Matrix resultMatrix, Matrix sourceMatrix, Func<double, double> operation)
        {
            ValidateMatricesDims(resultMatrix, sourceMatrix);

            for (int raw = 0; raw < sourceMatrix.Rows; raw++)
            {
                for (int column = 0; column < sourceMatrix.Columns; column++)
                {
                    resultMatrix[raw, column] = operation(sourceMatrix[raw, column]);
                }
            }
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix resultMatrix = new Matrix(m1.Rows, m2.Columns);
            MultiplyMatrices(ref resultMatrix, m1, m2);
            return resultMatrix;
        }

        public static void MultiplyMatrices(ref Matrix resultMatrix, Matrix m1, Matrix m2)
        {
            if (m1.Columns != m2.Rows)
            {
                throw new InvalidOperationException("The number of columns of the 1st matrix must equal the number of rows of the 2nd matrix.");
            }

            for (int curM1Raw = 0; curM1Raw < m1.Rows; curM1Raw++)
            {
                for (int curM2Column = 0; curM2Column < m2.Columns; curM2Column++)
                {
                    double dotProduct = 0;

                    for (int curM1ColumnM2Raw = 0; curM1ColumnM2Raw < m1.Columns; curM1ColumnM2Raw++)
                    {
                        double curM1Value = m1[curM1Raw, curM1ColumnM2Raw];
                        double curM2Value = m2[curM1ColumnM2Raw, curM2Column];

                        dotProduct += (curM1Value * curM2Value);
                    }

                    resultMatrix[curM1Raw, curM2Column] = dotProduct;
                }
            }
        }

        public static Matrix operator *(double scalar, Matrix m)
        {
            int rows = m.Rows;
            int columns = m.Columns;

            Matrix resultMatrix = new Matrix(m.Rows, m.Columns);

            for (int raw = 0; raw < rows; raw++)
            {
                for (int column = 0; column < columns; column++)
                {
                    resultMatrix[raw, column] = scalar * m[raw, column];
                }
            }

            return resultMatrix;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            var resultMatrix = new Matrix(m1.Rows, m1.Columns);
            AddMatrices(ref resultMatrix, m1, m2);
            return resultMatrix;
        }

        public static void AddMatrices(ref Matrix resultMatrix, Matrix m1, Matrix m2)
        {
            ValidateMatricesDims(m1, m2);

            int rows = m1.Rows;
            int columns = m1.Columns;


            for (int raws = 0; raws < rows; raws++)
            {
                for (int column = 0; column < columns; column++)
                {
                    resultMatrix[raws, column] = m1[raws, column] + m2[raws, column];
                }
            }
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {

            var resultMatrix = new Matrix(m1.Rows, m1.Columns);
            SubstractMatrices(ref resultMatrix, m1, m2);
            return resultMatrix;
        }

        public static void SubstractMatrices(ref Matrix resultMatrix, Matrix m1, Matrix m2)
        {
            ValidateMatricesDims(m1, m2);

            int rows = m1.Rows;
            int columns = m1.Columns;


            for (int raws = 0; raws < rows; raws++)
            {
                for (int column = 0; column < columns; column++)
                {
                    resultMatrix[raws, column] = m1[raws, column] - m2[raws, column];
                }
            }
        }

        protected bool Equals(Matrix other)
        {
            if (Columns != other.Columns || Rows != other.Rows)
            {
                return false;
            }

            for (int curRow = 0; curRow < Rows; curRow++)
            {
                for (int curColumn = 0; curColumn < Columns; curColumn++)
                {
                    if (!Utils.NearlyEqual(this[curRow, curColumn] , other[curRow, curColumn], 0.001))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Matrix)obj);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int raw = 0; raw < Rows; raw++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    sb.AppendFormat("{0} ", _values[raw, column]);
                }

               // sb.AppendLine(Environment.NewLine);
            }

            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return _values?.GetHashCode() ?? 0;
        }

        private readonly double[,] _values;

        public static void ValidateMatricesDims(params Matrix[] matrices)
        {
#if DEBUG
            Matrix firstMatrix = matrices.First();
            if (matrices.Any(m => m.Rows != firstMatrix.Rows) || matrices.Any(m => m.Columns != firstMatrix.Columns))
            {
                throw new InvalidOperationException("All matrices rows and colums must be the same.");
            }
#endif
        }
    }
}
