using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Layers;

namespace CoreLib
{
    public class Model
    {
        private readonly LinkedList<DoubleSideLayer> _layers = new LinkedList<DoubleSideLayer>();

        public Model(int inputsCount, int outputsCount)
        {
            var inputLayer = new InputLayer(inputsCount);
            _layers.AddFirst(inputLayer);
            inputLayer.SetListNode(_layers.First);

            var outputLayer = new OutputLayer(outputsCount);
            _layers.AddLast(outputLayer);
            outputLayer.SetListNode(_layers.Last);
       }

        public InputLayer InputLayer => (InputLayer)_layers.First.Value;

        public OutputLayer OutputLayer => (OutputLayer)_layers.Last.Value;

        public void AddAffineLayer(int unitsCount, ActivationType activationType)
        {
            AddLayerInternal(new AffineLayer(unitsCount, activationType));
        }

        public double FirstOutputValue => OutputLayer.Values[0, 0];

        public double FirstInputValue
        {
            get { return InputLayer.Values[0, 0]; }
            set { InputLayer.Values[0, 0] = value; }
        }

        public void ForwardPass()
        {
            foreach (AffineLayer layer in AffineLayers())
            {
                layer.ForwardPass();
            }
        }

        public void BackwardPass()
        {
            foreach (DoubleSideLayer layer in AffineLayers())
            {
                layer.BackwardPass();
            }
        }

        public void InitWithRandomValues(Random rnd)
        {
            foreach (AffineLayer layer in AffineLayers())
            {
                Initialiser.InitRndUniform(layer.Weights, rnd);
                //Initialiser.InitRndUniform(layer.Biases, rnd);
            }
        }

        public AffineLayer this[int key] => AffineLayers().ElementAt(key);

        private IEnumerable<AffineLayer> AffineLayers()
        {
            return _layers.OfType<AffineLayer>();
        }

        private void AddLayerInternal(DoubleSideLayer layer)
        {
            _layers.AddBefore(_layers.Last, layer);
            layer.SetListNode(_layers.Last.Previous);
        }
    }
}
