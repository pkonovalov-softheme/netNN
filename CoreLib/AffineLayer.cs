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
    public sealed class AffineLayer
    {
        private readonly Matrix _weights;
        private readonly Matrix _biases;

        readonly IActivationFunction _activation;

        public AffineLayer(int unitsCount, IActivationFunction activation)
        {
            _activation = activation;
            _weights = new Matrix(unitsCount, 1);
            _biases = new Matrix(unitsCount, 1);
            Values = new Matrix(unitsCount, 1);
            Gradients = new Matrix(unitsCount, 1);
        }

        public Matrix Values { get; private set; }
        public Matrix Gradients { get; private set; }


        public void ForwardPass(AffineLayer prevLayer)
        {
            Values = prevLayer.Values * _weights + _biases;
            Values.ApplyActivation(_activation);
        }

        public void BackwardPass(AffineLayer prevLayer)
        {
            Values = prevLayer.Values * _weights + _biases;
            _activation.Backward()
        }
    }
}
