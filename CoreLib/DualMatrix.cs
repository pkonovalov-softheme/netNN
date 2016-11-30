using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    [Serializable]
    public class DualMatrix
    {
        public int Rows { get; }
        public int Columns { get; }

        public Matrix Primal { get; }
        public Matrix Extra { get; }

        public DualMatrix(double value)
        {
            Primal = new Matrix(value);
            Extra = new Matrix(value);
        }

        public DualMatrix(int rows, int columns)
        {
            Primal = new Matrix(rows, columns);
            Extra = new Matrix(rows, columns);
            Rows = rows;
            Columns = columns;
        }
    }
}
