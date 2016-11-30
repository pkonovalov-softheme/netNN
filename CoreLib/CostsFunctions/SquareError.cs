using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.CostsFunctions
{
    [Serializable]
    public class SquareError : ILossOperator
    {
        public double ForwardPass(double h, double target)
        {
            return (h - target) * (h - target);
        }

        public double ComputeLossGradient(double h, double target)
        {
            return 2 * (h - target);
        }
    }
}
