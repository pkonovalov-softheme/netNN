using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.CostsFunctions
{
    public enum CostType { Abs };

    public static class CostsHelper
    {
        public static ILossOperator GetCostFunction(CostType costType)
        {
            switch (costType)
            {
                case CostType.Abs:
                    return new Abs();
                default:
                    throw new InvalidEnumArgumentException(costType.ToString());
            }
        }
    }
}
