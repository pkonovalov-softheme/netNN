using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public static class Initialiser
    {
        private static Random _rnd = new Random();

        public static void InitWithConstValue(Matrix matrix, double value)
        {
            for (int r = 0; r < matrix.Rows; r++)
            {
                for (int c = 0; c < matrix.Columns; c++)
                {
                    matrix[r, c] = value;
                }
            }
        }

        public static void InitRndUniform(Matrix matrix, int seed = 0, double minimum = 0, double maximum = 0)
        {
            if (maximum < minimum)
            {
                throw new ArgumentException("maximum < minimum");
            }

            if (seed > 0)
            {
                _rnd = new Random(seed);
            }

            for (int r = 0; r < matrix.Rows; r++)
            {
                for (int c = 0; c < matrix.Columns; c++)
                {
                    if (minimum > 0 || maximum > 0)
                    {
                        matrix[r, c] = Utils.GetRandomNumber(_rnd, minimum, maximum);

                    }
                    else
                    {
                        matrix[r, c] = _rnd.NextDouble();
                    }
                }
            }
        }
    }
}
