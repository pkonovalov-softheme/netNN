using System.Collections.Generic;

namespace CoreLib.Layers
{
    public abstract class DoubleSideLayer : GradLayer
    {
        private LinkedListNode<DoubleSideLayer> _layersListNode;

        protected DoubleSideLayer(int unitsCount)
            : base(unitsCount)
        {
        }

        public void SetListNode(LinkedListNode<DoubleSideLayer> layersListNode)
        {
            _layersListNode = layersListNode;
        }

        public DoubleSideLayer NextLayer
        {
            get { return _layersListNode.Next.Value; }
            set { _layersListNode.Next.Value = value; }
        }

        public DoubleSideLayer PrevLayer
        {
            get { return _layersListNode.Previous.Value; }
            set { _layersListNode.Previous.Value = value; }
        }

        protected bool HasPrevLayer => _layersListNode.Previous != null;

        protected bool HasNextLayer => _layersListNode.Previous != null;
    }
}
