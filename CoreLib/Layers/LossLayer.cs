using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.CostsFunctions;

namespace CoreLib.Layers
{
    public class LossLayer : Layer
    {
        private readonly ILossOperator _costFunction;
        public Matrix Losses { get; }

        public LossLayer(CostType costType, int unitsCount) : base(unitsCount)
        {
            Losses = new Matrix(unitsCount, 1);
            _costFunction = CostsHelper.GetCostFunction(costType);
        }

        public void ForwardPass(Matrix targetValues)
        {
            Matrix.ValidateMatricesDims(Values.Primal, targetValues);

            for (int raw = 0; raw < Values.Rows; raw++)
            {
                for (int column = 0; column < Values.Columns; column++)
                {
                    Losses[raw, column] = _costFunction.ForwardPass(
                        Values.Primal[raw, column], targetValues[raw, column]);
                }
            }
        }

        public void ComputeLossGradients(Matrix targetValues)
        {
            Matrix.ValidateMatricesDims(Values.Primal, targetValues);

            for (int raw = 0; raw < Values.Rows; raw++)
            {
                for (int column = 0; column < Values.Columns; column++)
                {
                    PrevLayer.Values.Extra[raw, column] = _costFunction.ComputeLossGradient(Losses[raw, column], targetValues[raw, column]);
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
