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
        readonly LinkedList<DoubleSideLayer> _layers = new LinkedList<DoubleSideLayer>();

        public Model(int inputsCount, int outputsCount)
        {
            AddLayerInternal(new InputLayer(inputsCount));
            AddLayerInternal(new OutputLayer(inputsCount));
        }

        public InputLayer InputLayer => (InputLayer)_layers.First.Value;

        public OutputLayer OutputLayer => (OutputLayer)_layers.Last.Value;

        public void AddAffineLayer(int unitsCount, IActivationFunction fucntion)
        {
            AddLayerInternal(new AffineLayer(unitsCount, fucntion));
        }

        private void AddLayerInternal(DoubleSideLayer layer)
        {
            _layers.AddBefore(_layers.Last, layer);
            LinkedListNode<DoubleSideLayer> node = _layers.Last;
            node.Value.SetListNode(node);
        }

        public DoubleSideLayer this[int key]
        {
            get
            {
                if (key > _layers.Count - 2)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _layers.ElementAt(key + 1);
            }
        }
    }
}
