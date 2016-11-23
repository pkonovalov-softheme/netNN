using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.CostsFunctions;

namespace CoreLib.Layers
{
    class LossLayer : Layer
    {
        private readonly ILossOperator _costFunction;

        public LossLayer(CostType costType, int unitsCount) : base(unitsCount)
        {
            _costFunction = CostsHelper.GetCostFunction(costType);
        }

        public void ForwardPass(Matrix targetValues)
        {
            Matrix.ValidateMatricesDims(Values, targetValues);

            for (int raw = 0; raw < Values.Rows; raw++)
            {
                for (int column = 0; column < Values.Columns; column++)
                {
                    Gradients[raw, column] = _costFunction.ForwardPass(Values[raw, column], targetValues[raw, column]);
                }
            }
        }

        public void ComputeLossGradients(Matrix targetValues)
        {
            Matrix.ValidateMatricesDims(Values, targetValues);

            for (int raw = 0; raw < Values.Rows; raw++)
            {
                for (int column = 0; column < Values.Columns; column++)
                {
                    Gradients[raw, column] = _costFunction.ComputeLossGradient(Values[raw, column], targetValues[raw, column]);
                }
            }
        }

        public override void BackwardPass()
        {
            throw new NotSupportedException();
        }

        public override void ForwardPass()
        {
            throw new NotSupportedException();
        }
    }
}
