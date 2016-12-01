using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal static class GradUtils
    {
        public const double H = 1e-5;

        /// <summary>
        /// Verifies that analytical gradient is valid by calculating numerical
        /// </summary>
        /// <param name="fa">analitical gradient</param>
        /// <param name="f1Val">f(x + h)</param>
        /// <param name="f2Val">f(x - h)</param>
        /// <returns>Is analytical gradient valid</returns>
        public static bool ChechAnalGrad(double fa, double f1Val, double f2Val)
        {
            double fn = (f1Val - f2Val) / (2 * H); // Numerical gradient
            if (fn == 0 && fa == 0)
            {
                return true;
            }

            double relativeError = Math.Abs(fa - fn) / Math.Max(Math.Abs(fa), Math.Abs(fn));

            // relative error > 1e-2 usually means the gradient is probably wrong
            // 1e-2 > relative error > 1e-4 should make you feel uncomfortable
            // 1e-4 > relative error is usually okay for objectives with kinks.But if there are no kinks(e.g.use of tanh nonlinearities and softmax), then 1e-4 is too high.
            // 1e-7 and less you should be happy.

            return relativeError < 1e-4;
        }
    }
}
