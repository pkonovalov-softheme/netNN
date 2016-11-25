using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.CostsFunctions;
using CoreLib.Layers;

namespace CoreLib
{
    public class Model
    {
        private readonly LinkedList<Layer> _layers = new LinkedList<Layer>();

        public Model(int inputsCount, ActivationType inputActivationType, int outputsCount, ActivationType outputActivationType, CostType costType)
        {
            var inputLayer = new AffineLayer(inputsCount, inputActivationType);
            _layers.AddFirst(inputLayer);
            inputLayer.SetListNode(_layers.First);

            var outputLayer = new AffineLayer(outputsCount, outputActivationType);
            _layers.AddLast(outputLayer);
            outputLayer.SetListNode(_layers.Last);

            OutputLayer = new LossLayer(costType, outputsCount);
            _layers.AddLast(OutputLayer);
            OutputLayer.SetListNode(_layers.Last);
        }

        public Layer InputLayer => _layers.First.Value;

        public LossLayer OutputLayer { get; }

        public void AddAffineLayer(int unitsCount, ActivationType activationType)
        {
            AddLayerInternal(new AffineLayer(unitsCount, activationType));
        }

        public double FirstOutputValue => OutputLayer.Values.Primal[0, 0];

        public double FirstInputValue
        {
            get { return InputLayer.Values.Primal[0, 0]; }
            set { InputLayer.Values.Primal[0, 0] = value; }
        }

        public void ForwardPass()
        {
            foreach (AffineLayer layer in AffineLayers())
            {
                layer.ForwardPass();
            }
        }

        public void InitWithConstWeights(double value)
        {
            foreach (AffineLayer layer in AffineLayers())
            {
                Initialiser.InitWithConstValue(layer.Weights.Primal, value);
            }
        }

        public void InitWithRandomWeights(Random rnd, double? minimum = null, double? maximum = null)
        {
            foreach (AffineLayer layer in AffineLayers())
            {
                Initialiser.InitRndUniform(layer.Weights.Primal, rnd, minimum, maximum);
            }
        }

        public void BackwardPass(Matrix targetValues)
        {
            LinkedListNode<Layer> curLayer = _layers.Last;
            LossLayer lossLayer = (LossLayer) curLayer.Value;
            lossLayer.ComputeLossGradients(targetValues);

            while(true)
            {
                curLayer = curLayer.Previous;
                if (curLayer.Previous == null)
                {
                    break;
                }

                curLayer.Value.BackwardPass();
            }
        }

        public void InitWithRandomValues(Random rnd)
        {
            foreach (AffineLayer layer in AffineLayers())
            {
                Initialiser.InitRndUniform(layer.Weights.Primal, rnd);
                //Initialiser.InitRndUniform(layer.Biases, rnd);
            }
        }

        public AffineLayer this[int key] => AffineLayers().ElementAt(key);

        private IEnumerable<AffineLayer> AffineLayers()
        {
            return _layers.OfType<AffineLayer>();
        }

        private void AddLayerInternal(Layer layer)
        {
            _layers.AddBefore(_layers.Last, layer);
            layer.SetListNode(_layers.Last.Previous);
        }
    }
}
