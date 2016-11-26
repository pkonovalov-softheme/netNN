using System.Collections.Generic;
using System.Diagnostics;

namespace CoreLib.Layers
{
    public abstract class Layer
    {
        protected LinkedListNode<Layer> LayersListNode;

        public DualMatrix Values { get; protected set; }

        protected Layer(int unitsCount)
        {
            Values = new DualMatrix(unitsCount, 1);
        }

        public void SetListNode(LinkedListNode<Layer> layersListNode)
        {
            LayersListNode = layersListNode;
        }

        public abstract void ForwardPass();


        public abstract void BackwardPass();

        public Layer NextLayer
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

        public Layer PrevLayer
        {
            get
            {
                return LayersListNode.Previous?.Value;
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
