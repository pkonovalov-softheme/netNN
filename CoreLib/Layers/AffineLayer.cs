using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Layers;

namespace CoreLib
{
    /// <summary>
    /// Fully-connected layer with form y = f(Wx + b). Where f - nonlinear activation function.
    /// q = W*x
    /// dx = W*dq
    /// </summary>
    public sealed class AffineLayer : Layer
    {
        const double LearningRate = 0.01;

        private double _biasGrad;

        readonly IActivationFunction _activation;

        public AffineLayer(int unitsCount, ActivationType activationType) : base(unitsCount)
        {
            _activation = ActivationHelper.GetActivationFunction(activationType);
            Weights = new Matrix(unitsCount, 1);
            Biases = new Matrix(unitsCount, 1);
        }

        public Matrix Weights { get; set; }

        public Matrix Biases { get; set; }

        public override void ForwardPass()
        {
            // Values = NextLayer.Values * _weights + _biases;
            // Values.ApplyActivation(_activation);
            Matrix.NonLinearTransform(NextLayer.Values, Weights, Values, Biases, _activation.Forward);

#if DEBUG
            if (GeneralSettings.ValuesTracingEnabled)
            {
                Debug.WriteLine("Forward pass." + Environment.NewLine + "Values:");
                Debug.WriteLine(Values.ToString());
                Debug.WriteLine("Weights:");
                Debug.WriteLine(Weights.ToString());
                Debug.WriteLine("Biases:");
                Debug.WriteLine(Biases.ToString());
            }
#endif 
        }

        public override void BackwardPass()
        {
            ComputeGradient();
            ApplyGradient();
        }

        public void ComputeGradient()
        {
            Matrix.ValidateMatricesDims(NextLayer.Gradients, Gradients);

            for (int raw = 0; raw < Gradients.Rows; raw++)
            {
                for (int column = 0; column < Gradients.Columns; column++)
                {
                    double x = PrevLayer.Values[raw, column]; // Current value
                    double dy = NextLayer.Gradients[raw, column]; // Current gradient

                    double df = _activation.Gradient(x, dy);
                    double dw = x*df;
                    _biasGrad = df;

                    Gradients[raw, column] = dw; // Was += Take the gradient in output unit and chain it with the local gradients . This will allow us to possibly use the output of one gate multiple times (think of it as a wire branching out), since it turns out that the gradients from these different branches just add up when computing the final gradient with respect to the circuit output.
                }
            }

#if DEBUG
            if (GeneralSettings.GradientsTracingEnabled)
            {
                Debug.WriteLine("Backward pass. /n Weights gradients:");
                Debug.WriteLine(Gradients.ToString());
                Debug.WriteLine("Bias gradient: " + _biasGrad);
            }
#endif 
        }

        public void ApplyGradient()
        {
            Matrix.ValidateMatricesDims(Gradients, Values);

            for (int raw = 0; raw < Gradients.Rows; raw++)
            {
                for (int column = 0; column < Gradients.Columns; column++)
                {
                    Weights[raw, column] -= Gradients[raw, column]*LearningRate;
                    Gradients[raw, column] = 0; // Zero out grad. In some cases we can not make it zero after single update. For example for large minibatches. 
                    Biases[raw, column] -= _biasGrad * LearningRate;
                    _biasGrad = 0;
                }
            }
        }
    }
}
