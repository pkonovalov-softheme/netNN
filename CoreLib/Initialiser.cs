using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public static class Initialiser
    {
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

        public static void InitRndUniform(Matrix matrix, Random rnd, double? minimum = null, double? maximum = null)
        {
            if ((minimum.HasValue || maximum.HasValue) && (maximum.GetValueOrDefault() < minimum.GetValueOrDefault()))
            {
                throw new ArgumentException("maximum < minimum");
            }

            for (int r = 0; r < matrix.Rows; r++)
            {
                for (int c = 0; c < matrix.Columns; c++)
                {
                    if (minimum.HasValue || maximum.HasValue)
                    {
                        matrix[r, c] = Utils.GetRandomNumber(rnd, minimum.GetValueOrDefault(), maximum.GetValueOrDefault());

                    }
                    else
                    {
                        matrix[r, c] = rnd.NextDouble();
                    }
                }
            }
        }
    }
}
