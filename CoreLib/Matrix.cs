using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public class Matrix
    {
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

        // y = ax + b
        public static void LinearTransform(ref Matrix y, Matrix a, Matrix x, Matrix b)
        {
            
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix resultMatrix = new Matrix(m1.Rows, m2.Columns);

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

                        dotProduct += (curM1Value*curM2Value);
                    }


                    resultMatrix[curM1Raw, curM2Column] = dotProduct;
                }
            }

            return resultMatrix;
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
            if (m1.Rows != m2.Rows)
            {
                throw new InvalidOperationException("Matrix rows must the same.");
            }

            if (m1.Columns != m2.Columns)
            {
                throw new InvalidOperationException("Matrix columns must the same.");
            }

            int rows = m1.Rows;
            int columns = m1.Columns;

            var resultMatrix = new Matrix(m1.Rows, m1.Columns);

            for (int raws = 0; raws < rows; raws++)
            {
                for (int column = 0; column < columns; column++)
                {
                    resultMatrix[raws, column] = m1[raws, column] + m2[raws, column];
                }
            }

            return resultMatrix;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows)
            {
                throw new InvalidOperationException("Matrix rows must the same.");
            }

            if (m1.Columns != m2.Columns)
            {
                throw new InvalidOperationException("Matrix columns must the same.");
            }

            int rows = m1.Rows;
            int columns = m1.Columns;

            var resultMatrix = new Matrix(m1.Rows, m1.Columns);

            for (int raws = 0; raws < rows; raws++)
            {
                for (int column = 0; column < columns; column++)
                {
                    resultMatrix[raws, column] = m1[raws, column] - m2[raws, column];
                }
            }

            return resultMatrix;
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

        public override int GetHashCode()
        {
            return _values?.GetHashCode() ?? 0;
        }

        private readonly double[,] _values;
    }
}
