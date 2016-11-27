using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.CostsFunctions
{
    public enum CostType { Abs, Square };

    public static class CostsHelper
    {
        public static ILossOperator GetCostFunction(CostType costType)
        {
            switch (costType)
            {
                case CostType.Abs:
                    return new Abs();
                case CostType.Square:
                    return new SquareError();
                default:
                    throw new InvalidEnumArgumentException(costType.ToString());
            }
        }
    }
}
