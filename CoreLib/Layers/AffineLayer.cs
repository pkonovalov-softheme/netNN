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
    [Serializable]
    public sealed class AffineLayer : Layer
    {
        const double LearningRate = 0.01;

      //  private double _biasGrad;

        readonly IActivationFunction _activation;

        public AffineLayer(int unitsCount, ActivationType activationType) : base(unitsCount)
        {
            _activation = ActivationHelper.GetActivationFunction(activationType);
            Weights = new DualMatrix(unitsCount, 1);
            Biases = new DualMatrix(unitsCount, 1);
        }

        public DualMatrix Weights { get; set; }

        public DualMatrix Biases { get; set; }

        public override void ForwardPass()
        {
            // Values = NextLayer.Primal * _weights + _biases;
            // Values.ApplyActivation(_activation);
            Matrix.NonLinearTransform(NextLayer.Values.Primal,
                Weights.Primal,
                Values.Primal,
                Biases.Primal,
                _activation.Forward);

#if DEBUG
            if (GeneralSettings.ValuesTracingEnabled)
            {
                Trace.Write("Affine layer: " + GetHashCode() + ". Values:");
                Trace.Write(Values.Primal.ToString());
                Trace.Write("Weights:");
                Trace.Write(Weights.Primal.ToString());
                Trace.Write("Biases:");
                Trace.WriteLine(Biases.Primal.ToString());
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
            Matrix.ValidateMatricesDims(NextLayer.Values.Extra, Values.Extra);

            for (int raw = 0; raw < Values.Rows; raw++)
            {
                for (int column = 0; column < Values.Columns; column++)
                {
                    double x = Values.Primal[raw, column]; // Current value
                    double dy = Values.Extra[raw, column]; // Current gradient

                    double df = _activation.Gradient(x, dy);
                    double dw = x*df;
                    Biases.Extra[raw, column] = df;
                    Weights.Extra[raw, column] = dw;

                    if (PrevLayer != null)
                    {
                        PrevLayer.Values.Extra[raw, column] += df*Weights.Primal[raw, column]; // Take the gradient in output unit and chain it with the local gradients . This will allow us to possibly use the output of one gate multiple times (think of it as a wire branching out), since it turns out that the gradients from these different branches just add up when computing the final gradient with respect to the circuit output.
                    }
                }
            }

#if DEBUG
            if (GeneralSettings.GradientsTracingEnabled)
            {
                Trace.Write("Affine layer: " + GetHashCode() + ". Values grad:");
                Trace.Write(Values.Extra.ToString());
                Trace.Write("Bias grad: " + Biases.Extra);
                Trace.WriteLine("Weights grad: " + Weights.Extra);
            }
#endif 
        }

        public void ApplyGradient()
        {
            Matrix.ValidateMatricesDims(Values.Primal, Weights.Primal);

            for (int raw = 0; raw < Values.Rows; raw++)
            {
                for (int column = 0; column < Values.Columns; column++)
                {
                    Weights.Primal[raw, column] -= Weights.Extra[raw, column] * LearningRate;
                    Weights.Extra[raw, column] = 0;
                    Biases.Primal[raw, column] -= Biases.Extra[raw, column] * LearningRate;
                    Biases.Extra[raw, column] = 0;
                    Values.Extra[raw, column] = 0;
                }
            }
        }
    }
}
