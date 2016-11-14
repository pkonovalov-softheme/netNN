using System.Collections.Generic;

namespace CoreLib.Layers
{
    public class InputLayer : GradLayer
    {
        private readonly LinkedListNode<DoubleSideLayer> _layersListNode;
        protected readonly DoubleSideLayer NextLayer;

        public InputLayer(int unitsCount, LinkedListNode<DoubleSideLayer> layersListNode) : base(unitsCount)
        {
            _layersListNode = layersListNode;
        }

        public LinkedListNode<DoubleSideLayer> LayersListNode
        {
            get { return _layersListNode; }
        }
    }
}
