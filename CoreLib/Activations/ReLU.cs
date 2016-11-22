using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public class ReLU : IActivationFunction
    {
        /// <summary>
        /// y = max(0, x).
        /// </summary>
        public double Forward(double x)
        {
            return Math.Max(0, x);
        }

        /// <summary>
        /// f'(x)=
        /// { 1, if x > 0
        /// { 0, otherwise
        /// </summary>
        /// <param name="x">current x value</param>
        /// <param name="gradY">source grad from backprob</param>
        /// <returns></returns>
        public double Gradient(double x, double gradY)
        {
            var gradX = x > 0 ? gradY : 0.0;
            return gradX;
        }
    }
}
