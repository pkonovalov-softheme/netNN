﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public interface IActivationFunction
    {
        // y = f(x)
        double Forward(double value);

        // return dx from dy
        double Gradient(double dy);
    }
}
