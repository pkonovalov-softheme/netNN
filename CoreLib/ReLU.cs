using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    class ReLU : IActivationFunction
    {
        /// <summary>
        /// y=max(0, x).
        /// </summary>
        public double Forward(double value)
        {
            return Math.Max(0, value);
        }

        public double Gradient(double x, double dy)
        {
            var dx = x > 0 ? dy : 0.0;
            return dx;
        }
    }
}
