using System.Collections.Generic;
using System.Diagnostics;

namespace CoreLib.Layers
{
    public abstract class DoubleSideLayer : GradLayer
    {
        protected LinkedListNode<DoubleSideLayer> LayersListNode;

        protected DoubleSideLayer(int unitsCount)
            : base(unitsCount)
        {
        }

        public void SetListNode(LinkedListNode<DoubleSideLayer> layersListNode)
        {
            LayersListNode = layersListNode;
        }

        public void ForwardPass()
        {
        }

        public void BackwardPass()
        {
        }

        public DoubleSideLayer NextLayer
        {
            get
            {
                Debug.Assert(LayersListNode.Next != null, "LayersListNode.Next != null");
                return LayersListNode.Next.Value;
            }
            set
            {
                Debug.Assert(LayersListNode.Next != null, "LayersListNode.Next != null");
                LayersListNode.Next.Value = value;
            }
        }

        public DoubleSideLayer PrevLayer
        {
            get
            {
                Debug.Assert(LayersListNode.Previous != null, "LayersListNode.Previous != null");
                return LayersListNode.Previous.Value;
            }
            set
            {
                Debug.Assert(LayersListNode.Previous != null, "LayersListNode.Previous != null");
                LayersListNode.Previous.Value = value;
            }
        }

        protected bool HasPrevLayer => LayersListNode.Previous != null;

        protected bool HasNextLayer => LayersListNode.Previous != null;
    }
}
