using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Activations
{
    public class Identity : IActivationFunction
    {
        public double Forward(double value)
        {
            return value; 
        }

        public double Gradient(double x, double gradY)
        {
            return gradY;
        }
    }
}
