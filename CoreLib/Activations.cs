using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public enum ActivationType { ReLU};

    public static class ActivationHelper
    {
        public static IActivationFunction GetActivationFunction(ActivationType activationType)
        {
            switch (activationType)
            {
                case ActivationType.ReLU:
                    return new ReLU();
                default:
                    throw new InvalidEnumArgumentException(activationType.ToString()); 
            }
        }
    }
}
