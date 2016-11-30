using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.CostsFunctions
{
    [Serializable]
    public class Abs : ILossOperator
    {
        public double ForwardPass(double h, double target)
        {
            return Math.Abs(h - target);
        }

        public double ComputeLossGradient(double h, double target)
        {
            double dif = h - target;
            if (dif > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
