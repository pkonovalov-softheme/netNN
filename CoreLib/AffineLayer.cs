using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    /// <summary>
    /// Fully-connected layer with form y = f(Wx + b). Where f - nonlinear activation function.
    /// q = W*x
    /// dx = W*dq
    /// </summary>
    public sealed class AffineLayer : DoubleSideLayer
    {
        const double LearningRate = 0.001;

        private readonly Matrix _weights;
        private readonly Matrix _biases;
        private double _biasGrad;

        readonly IActivationFunction _activation;

        public AffineLayer(int unitsCount, IActivationFunction activation) : base(unitsCount)
        {
            _activation = activation;
            _weights = new Matrix(unitsCount, 1);
            _biases = new Matrix(unitsCount, 1);
        }

        public Matrix Weights
        {
            get { return _weights; }
        }

        public void ForwardPass(BasicLayer prevLayer)
        {
            // Values = prevLayer.Values * _weights + _biases;
            // Values.ApplyActivation(_activation);
            Matrix.NonLinearTransform(Values, _weights, prevLayer.Values, _biases, _activation.Forward);
        }

        public void BackwardPass(DoubleSideLayer prevLayer)
        {
            ComputeGradient(prevLayer);
            ApplyGradient();
        }

        private void ComputeGradient(DoubleSideLayer prevLayer)
        {
            if (prevLayer != null)
            {
                Matrix.ValidateMatricesDims(prevLayer.Gradients, Gradients);
            }

            for (int raw = 0; raw < Gradients.Rows; raw++)
            {
                for (int column = 0; column < Gradients.Columns; column++)
                {
                    double x = Values[raw, column]; // Current value
                    double dy = Gradients[raw, column]; // Current gradient

                    double df = _activation.Gradient(x, dy);
                    double dw = x*df;
                    _biasGrad = df;

                    if (prevLayer != null)
                    {
                        prevLayer.Gradients[raw, column] += dw; // Take the gradient in output unit and chain it with the local gradients
                    }
                }
            }
        }

        private void ApplyGradient()
        {
            Matrix.ValidateMatricesDims(Gradients, Values);

            for (int raw = 0; raw < Gradients.Rows; raw++)
            {
                for (int column = 0; column < Gradients.Columns; column++)
                {
                    Weights[raw, column] -= Gradients[raw, column]*LearningRate;
                    _biases[raw, column] -= _biasGrad * LearningRate;
                }
            }
        }
    }
}
